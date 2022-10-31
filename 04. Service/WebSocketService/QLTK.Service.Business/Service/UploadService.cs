using CoreFtp;
using NTS.Common;
using NTS.Common.FTP;
using NTS.Common.Logs;
using NTS.Common.Resource;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Model.Downloads;
using QLTK.Service.Model.FTP;
using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Service
{
    public class UploadService
    {
        private ApiUtil apiUtil = new ApiUtil();
        private NtsFTPClient ntsFTPClient;
        FileService fileService = new FileService();
        FTPServer fTPServer;

        private void CreateFTP(string apiUrl, string token)
        {
            var resultApi = apiUtil.GetFTPServer(apiUrl, token);

            if (!resultApi.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultApi.Message);
            }

            fTPServer = resultApi.Data;
            ntsFTPClient = new NtsFTPClient(resultApi.Data.ServerIP, resultApi.Data.Port, resultApi.Data.User, resultApi.Data.Password);
        }



        public async Task<UploadFolderResultModel> UploadFolder(UploadFolderModel folderModel, string localFolderPath, string serverRootFolder, int currentVersion)
        {
            Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, $"Bắt đầu upload thư mục: {localFolderPath}");


            UploadFolderResultModel uploadFolderResult = new UploadFolderResultModel();
            uploadFolderResult.ModuleId = folderModel.ModuleId;

            // Lấy danh sách thư mục, file trong thư mục thiết kế dưới Local
            var folderUploads = fileService.GetAllFolderLocalUpload(localFolderPath);

            // Không lấy được thư mục được chọn
            if (!folderUploads[0].LocalPath.Equals(localFolderPath))
            {
                uploadFolderResult.LstError.Add("Lấy thông tin thư mục sai!");
                Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, "Lấy thông tin thư mục sai!");
                return uploadFolderResult;
            }

            // Đường dẫn thư mục cha trên server
            string serverPath = folderModel.Path.Replace(Path.GetPathRoot(folderModel.Path), "") + $"\\ver.{currentVersion}";

            // Khởi tạo thông tin FTP
            CreateFTP(folderModel.ApiUrl, folderModel.Token);

            bool isOk = true;

            FtpClientConfiguration config = new FtpClientConfiguration()
            {
                Host = fTPServer.ServerIP,
                Username = fTPServer.User,
                Password = fTPServer.Password,
                Port = fTPServer.Port,
                IgnoreCertificateErrors = true
            };

            using (var ftpClient = new FtpClient(config))
            {
                await ftpClient.LoginAsync();
                try
                {
                    await ftpClient.ChangeWorkingDirectoryAsync(serverPath);
                }
                catch
                {
                    try
                    {
                        await ftpClient.CreateDirectoryAsync(serverPath);
                        await ftpClient.ChangeWorkingDirectoryAsync(serverPath);
                    }
                    catch
                    {
                        Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, $"Kiểm tra folder lỗi: Không tạo được thư mục {folderModel.Path}");
                        uploadFolderResult.LstError.Add($"Không tạo được thư mục {serverPath}");
                        isOk = false;
                    }
                }

                // Lấy dữ liệu Design của Modue từ API
                var resultApi = apiUtil.GetAllModuleDesignDocument(folderModel.ApiUrl, folderModel.Token, folderModel.ModuleId, folderModel.DesignType);
                if (!resultApi.SuccessStatus)
                {
                    throw NTSException.CreateInstance(resultApi.Message);
                }

                List<ModuleDesignDocumentModel> moduelDocuments = resultApi.Data;


                // Lấy danh sách file trên server
                List<string> files = (await ftpClient.ListFilesAsync()).Select(r => r.Name).ToList();

                string temp = string.Empty;
                // Upload thư mục lên server
                foreach (var folderUpload in folderUploads)
                {
                    Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, $"Bắt đầu kiểm tra folder: {folderModel.Path}");

                    // Đường dẫn thư mục trên server
                    folderUpload.ServerPath = folderUpload.LocalPath.Replace(folderModel.Path, serverPath);

                    folderUpload.LocalPath = folderUpload.LocalPath.Replace(folderModel.Path, Path.GetFileName(folderModel.Path));

                    if (!isOk)
                    {
                        break;
                    }

                    Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, $"Kết thúc kiểm tra folder: {folderModel.Path}");

                    foreach (var fileUpload in folderUpload.Files)
                    {

                        // Thực hiện Băm file để lấy giá trị Hash
                        fileUpload.HashValue = FileUtils.GetChecksum(fileUpload.LocalPath);

                        temp = fileUpload.LocalPath.Substring(fileUpload.LocalPath.IndexOf(folderModel.ModuleCode));
                        var moduelDocument = (from r in moduelDocuments
                                              where r.Path.Equals(temp) && files.Contains(Path.GetFileName(r.ServerPath))
                                              select r).FirstOrDefault();

                        // Trường hợp đã có file trên Server
                        if (moduelDocument != null)
                        {
                            fileUpload.ServerPath = moduelDocument.ServerPath;
                            // Trường hợp file không thay đổi (khi hash value không thay đổi), thì không cần upload lại file
                            if (!string.IsNullOrEmpty(moduelDocument.HashValue) && fileUpload.HashValue.Equals(moduelDocument.HashValue))
                            {
                                // Gán lại cờ để đánh dấu bản ghi không có sự thay đổi so với server
                                fileUpload.RowState = 1;
                            }
                            else
                            {
                                // Trường hợp có thay đổi dữ liệu, thực hiện upload lại file
                                await UploadFile(fileUpload.LocalPath, fileUpload.ServerPath, ftpClient, uploadFolderResult);

                                // Gán lại cờ để đánh dấu bản ghi được update
                                fileUpload.RowState = 2;
                            }
                        }
                        // Trường hợp chưa tồn tại file trên Server
                        else
                        {
                            // thực hiện tạo tên file để lưu vào CSDL
                            fileUpload.ServerPath = Path.Combine(serverPath, Guid.NewGuid().ToString() + Path.GetExtension(fileUpload.LocalPath));

                            await UploadFile(fileUpload.LocalPath, fileUpload.ServerPath, ftpClient, uploadFolderResult);

                            // Gán cờ đánh dấu bản ghi được thêm mới
                            fileUpload.RowState = 3;
                            fileUpload.LocalPath = temp;
                        }
                    }
                }

                await ftpClient.LogOutAsync();
            }

            if (isOk)
            {
                uploadFolderResult.DesignDocuments = folderUploads;
                Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, $"Kết thúc Upload thành công!");
            }
            else
            {
                Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, $"Kết thúc Upload không thành công!");
            }

            Log.WriteTracerProcessData(0, "UploadService.UploadFolder: ", 0, "===================================================================================================");

            return uploadFolderResult;
        }

        private async Task UploadFile(string localFilePath, string serverFilePath, FtpClient ftpClient, UploadFolderResultModel uploadFolderResult)
        {
            var fileinfo = new FileInfo(localFilePath);

            int retryTimes = 0;

            bool isUpload = false;
            byte[] buffer;
            int bytesRead = 0;
            long length = 0;
            FileStream fileReadStream;

            while (true)
            {
                try
                {
                    using (var writeStream = await ftpClient.OpenFileWriteStreamNoCheckDirectoryAsync(Path.GetFileName(serverFilePath)))
                    {
                        fileReadStream = fileinfo.OpenRead();
                        buffer = new byte[1048576];
                        bytesRead = fileReadStream.Read(buffer, 0, 1048576);
                        length = 0;

                        while (bytesRead != 0)
                        {
                            await writeStream.WriteAsync(buffer, 0, bytesRead);
                            bytesRead = await fileReadStream.ReadAsync(buffer, 0, 1048576);
                            length += bytesRead;
                        }

                        writeStream.Close();
                        fileReadStream.Close();
                    }

                    isUpload = true;
                    break;
                }
                catch (Exception ex)
                {
                    retryTimes++;
                    Log.WriteTracerProcessData(0, "UploadService.UploadFile: ", 0, $"Upload file lỗi: {ex.Message}");

                    if (retryTimes > 10)
                    {
                        isUpload = false;
                        break;
                    }

                    Thread.Sleep(100);
                }

                Thread.Sleep(10);
            }

            if (!isUpload)
            {
                uploadFolderResult.LstError.Add($"UploadService.UploadFile: Upload file thất bại: {localFilePath}");
                return;
            }
        }


        public async Task<ResultModel> DownloadFileDesignDocument(DownloadFileModel fileModel)
        {
            // Khởi tạo thông tin FTP
            CreateFTP(fileModel.ApiUrl, fileModel.Token);

            bool isOk = true;
            using (var ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = fTPServer.ServerIP,
                Username = fTPServer.User,
                Password = fTPServer.Password,
                Port = fTPServer.Port,
                IgnoreCertificateErrors = true
            }))
            {
                await ftpClient.LoginAsync();

                int downloadCount = 0;
                while (true)
                {
                    try
                    {
                        using (var ftpReadStream = await ftpClient.OpenFileReadStreamAsync(fileModel.ServerPath))
                        {
                            using (FileStream fileStream = new FileStream(Path.Combine(fileModel.DownloadPath, fileModel.Name), FileMode.Create, FileAccess.Write))
                            {
                                await ftpReadStream.CopyToAsync(fileStream);
                            }
                        }

                        break;
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(200);

                        downloadCount++;
                        if (downloadCount > 10)
                        {
                            isOk = false;
                            break;
                        }
                    }

                    Thread.Sleep(10);
                }

                await ftpClient.LogOutAsync();
            }

            ResultModel resultModel = new ResultModel();
            resultModel.StatusCode = ResultModel.StatusCodeSuccess;


            if (!isOk)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = "Download lỗi";
            }

            return resultModel;
        }

        public async Task<ResultModel> DownloadFolderDesignDocument(DownloadFolderModel folderModel)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.StatusCode = ResultModel.StatusCodeSuccess;

            // Khởi tạo thông tin FTP
            CreateFTP(folderModel.ApiUrl, folderModel.Token);

            var resultApi = apiUtil.GetFolderDownloadModuleDesignDocument(folderModel.ApiUrl, folderModel.Token, folderModel.ObjectId, folderModel.Id);
            if (!resultApi.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultApi.Message);
            }

            string rootPath = resultApi.Data[0].LocalPath;
            string localPath;
            bool isOK = true;
            NtsFTPResult downloadResult;
            int downloadCount = 0;
            using (var ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = fTPServer.ServerIP,
                Username = fTPServer.User,
                Password = fTPServer.Password,
                Port = fTPServer.Port,
                IgnoreCertificateErrors = true
            }))
            {
                await ftpClient.LoginAsync();
                //await ftpClient.ChangeWorkingDirectoryAsync(resultApi.Data.FirstOrDefault().ServerPath);
                foreach (var item in resultApi.Data)
                {
                    localPath = Path.Combine(folderModel.DownloadPath, item.LocalPath.Replace(rootPath, Path.GetFileName(rootPath)));

                    CreateFolder(localPath);
                    //await ftpClient.ChangeWorkingDirectoryAsync(item.ServerPath);
                    foreach (var file in item.Files)
                    {
                        downloadCount = 0;
                        while (true)
                        {
                            Log.WriteTracerProcessData(0, "DownloadFolderDesignDocument", 0, $"Bắt đầu donwload:{file.ServerPath}");
                            try
                            {
                                //using (var ftpReadStream = await ftpClient.OpenFileReadStreamAsync(Path.GetFileName(file.ServerPath)))
                                using (var ftpReadStream = await ftpClient.OpenFileReadStreamAsync(file.ServerPath))
                                {
                                    using (FileStream fileStream = new FileStream(Path.Combine(localPath, file.Name), FileMode.Create, FileAccess.Write))
                                    {
                                        await ftpReadStream.CopyToAsync(fileStream);
                                        Log.WriteTracerProcessData(0, "DownloadFolderDesignDocument", 0, $"Download thành công:{file.ServerPath}");
                                    }
                                }

                                break;
                            }
                            catch (Exception ex)
                            {
                                downloadCount++;
                                if (downloadCount > 10)
                                {
                                    Log.WriteTracerProcessData(0, "DownloadFolderDesignDocument", 0, $"EXCEPTION:{ex.Message}");
                                    Log.WriteTracerProcessData(0, "DownloadFolderDesignDocument", 0, $"OK=FALSE:{file.ServerPath}");
                                    isOK = false;
                                    break;
                                }
                            }

                            Thread.Sleep(200);
                        }


                        if (!isOK)
                        {
                            Log.WriteTracerProcessData(0, "DownloadFolderDesignDocument", 0, $"Download lỗi:{file.ServerPath}");
                            resultModel.StatusCode = ResultModel.StatusCodeError;
                            resultModel.Message = "Download lỗi";
                            break;
                        }
                    }

                    await ftpClient.ChangeWorkingDirectoryAsync("/");


                    if (!isOK)
                    {
                        break;
                    }
                }

                await ftpClient.LogOutAsync();
            }

            return resultModel;
        }

        private void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                List<string> pathChild = path.Split('/').ToList();

                string root = pathChild[0];

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                for (int i = 1; i < pathChild.Count; i++)
                {
                    root += "/" + pathChild[i];
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }
                }
            }
        }
    }
}
