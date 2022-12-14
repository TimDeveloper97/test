using NTS.Common;
using NTS.Common.FTP;
using NTS.Common.Resource;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Model.ClassRoom;
using QLTK.Service.Model.Downloads;
using QLTK.Service.Model.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Service
{
    public class ClassRoomDesignDocumentBusiness
    {
        private ApiUtil apiUtil = new ApiUtil();
        private NtsFTPClient ntsFTPClient;
        FileService fileService = new FileService();

        public ClassRoomDesignDocumentBusiness()
        {

        }

        private void CreateFTP(string apiUrl, string token)
        {
            var resultApi = apiUtil.GetFTPServer(apiUrl, token);

            if (!resultApi.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultApi.Message);
            }

            ntsFTPClient = new NtsFTPClient(resultApi.Data.ServerIP, resultApi.Data.Port, resultApi.Data.User, resultApi.Data.Password);
        }

        public UploadFolderClassRoomResultModel UploadFolderDesignDocument(UploadClassRoomFolderModel folderModel)
        {
            var resultData = apiUtil.GetUploadClassRoomData(folderModel.ApiUrl, folderModel.Token, folderModel.ClassRoomId, folderModel.DesignType);
            if (!resultData.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultData.Message);
            }

            var dataCheck = resultData.Data;

            UploadFolderClassRoomResultModel uploadFolderResult = new UploadFolderClassRoomResultModel();

            bool isCheckMaterial = false;
            List<string> listFolder = new List<string>();
            List<string> pathErrors = new List<string>();
            var check = fileService.CheckFolderUpload(folderModel.DesignType, folderModel.Path, dataCheck.ClassRoom.Code, dataCheck.ClassRoom.GroupCode, dataCheck.ClassRoom.ParentGroupCode,
                   uploadFolderResult.ListError, dataCheck.FolderDefinitions, dataCheck.FileDefinitions, null, null, null,
                  null, null, null, uploadFolderResult.LstError, listFolder, isCheckMaterial, out string materialPath, dataCheck.Modules, pathErrors);

            if (!check.Status)
            {
                uploadFolderResult.Status = false;
                return uploadFolderResult;
            }

            if (!CheckChangeOnServer(folderModel))
            {
                uploadFolderResult.LstError.Add("Thư mục upload không khác với nguồn!");
                return uploadFolderResult;
            }

            // Lấy danh sách thư mục, file
            var folderUploads = fileService.GetAllFolderLocalUpload(folderModel.Path);

            // Không lấy được thư mục được chọn
            if (!folderUploads[0].LocalPath.Equals(folderModel.Path))
            {
                uploadFolderResult.LstError.Add("Lấy thông tin thư mục sai!");
                return uploadFolderResult;
            }

            // Đường dẫn thư mục tra trên server
            string serverPath = folderModel.Path.Replace(Path.GetPathRoot(folderModel.Path), "") + "_" + DateTime.Now.ToStringYYMMDD();

            // Khởi tạo thông tin FTP
            CreateFTP(folderModel.ApiUrl, folderModel.Token);

            //if (!ntsFTPClient.DirectoryExist(serverPath))
            //{
            //    if (!ntsFTPClient.DeleteDirectory(serverPath))
            //    {
            //        uploadFolderResult.LstError.Add("Không xóa được thư mục trên server!");
            //        return uploadFolderResult;
            //    }
            //}

            bool isOk = true;

            NtsFTPResult uploadResult;
            int downloadCount = 0;

            // Upload thư mục lên server
            foreach (var folderUpload in folderUploads)
            {
                // Đường dẫn thư mục trên server
                folderUpload.ServerPath = folderUpload.LocalPath.Replace(folderModel.Path, serverPath);

                folderUpload.LocalPath = folderUpload.LocalPath.Replace(folderModel.Path, Path.GetFileName(folderModel.Path));


                // Kiểm tra tồn tại thư mục, Nếu chưa có thì tạo mới
                if (!ntsFTPClient.DirectoryExist(folderUpload.ServerPath))
                {
                    if (!ntsFTPClient.CreateDirectory(folderUpload.ServerPath))
                    {
                        uploadFolderResult.LstError.Add($"Không tạo được thư mục {folderUpload.LocalPath}");
                        isOk = false;
                        break;
                    }
                }

                foreach (var fileUpload in folderUpload.Files)
                {
                    // Đường dẫn file trên server
                    fileUpload.ServerPath = fileUpload.LocalPath.Replace(folderModel.Path, serverPath);

                    // Upload file lên server 
                    downloadCount = 0;
                    while (true)
                    {
                        uploadResult = ntsFTPClient.UploadFile(fileUpload.LocalPath, fileUpload.ServerPath);
                        if (uploadResult.FtpStatus == FtpStatus.Success)
                        {
                            break;
                        }

                        downloadCount++;

                        if (downloadCount > 10)
                        {
                            break;
                        }

                        Thread.Sleep(200);
                    }

                    if (uploadResult.FtpStatus == FtpStatus.Failed)
                    {
                        uploadFolderResult.LstError.Add($"Không upload được file {fileUpload.LocalPath}");
                        isOk = false;
                        break;
                    }

                    fileUpload.LocalPath = fileUpload.LocalPath.Replace(folderModel.Path, Path.GetFileName(folderModel.Path));
                }
            }

            if (isOk)
            {
                uploadFolderResult.DesignDocuments = folderUploads;
            }

            return uploadFolderResult;
        }

        /// <summary>
        /// Kiểm tra thay đổi so với server
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckChangeOnServer(UploadClassRoomFolderModel model)
        {
            bool checkOther = false;

            var resultApi = apiUtil.GetAllClassRoomDesignDocument(model.ApiUrl, model.Token, model.ClassRoomId);
            if (!resultApi.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultApi.Message);
            }

            var moduelDocuments = resultApi.Data;

            var listFiles = fileService.GetAllFileInFolder(model.Path);

            if (moduelDocuments.Count == listFiles.Count)
            {
                foreach (var item in listFiles)
                {
                    var checkExits = moduelDocuments.Where(a => a.Name.Equals(item.Name) && a.FileSize.Equals(item.Length)).FirstOrDefault();
                    if (checkExits == null)
                    {
                        checkOther = true;
                    }
                    break;
                }
            }
            else
            {
                checkOther = true;
            }

            return checkOther;
        }

        public ResultModel DownloadFileDesignDocument(DownloadFileModel fileModel)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.StatusCode = ResultModel.StatusCodeSuccess;

            // Khởi tạo thông tin FTP
            CreateFTP(fileModel.ApiUrl, fileModel.Token);

            NtsFTPResult downloadResult;
            int downloadCount = 0;
            while (true)
            {
                downloadResult = ntsFTPClient.DownloadFile(Path.Combine(fileModel.DownloadPath, fileModel.Name), fileModel.ServerPath);
                if (downloadResult.FtpStatus == FtpStatus.Success)
                {
                    break;
                }

                downloadCount++;

                if (downloadCount > 10)
                {
                    break;
                }

                Thread.Sleep(200);
            }

            if (downloadResult.FtpStatus == FtpStatus.Failed)
            {
                resultModel.StatusCode = ResultModel.StatusCodeError;
                resultModel.Message = downloadResult.Message;
            }

            return resultModel;
        }

        public ResultModel DownloadFolderDesignDocument(DownloadFolderModel folderModel)
        {
            ResultModel resultModel = new ResultModel();
            resultModel.StatusCode = ResultModel.StatusCodeSuccess;

            // Khởi tạo thông tin FTP
            CreateFTP(folderModel.ApiUrl, folderModel.Token);

            var resultApi = apiUtil.GetFolderDownloadClassRoomDesignDocument(folderModel.ApiUrl, folderModel.Token, folderModel.ObjectId, folderModel.Id);
            if (!resultApi.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultApi.Message);
            }

            string rootPath = resultApi.Data[0].LocalPath;
            string localPath;
            bool isOK = true;
            NtsFTPResult downloadResult;
            int downloadCount = 0;
            foreach (var item in resultApi.Data)
            {
                localPath = Path.Combine(folderModel.DownloadPath, item.LocalPath.Replace(rootPath, Path.GetFileName(rootPath)));

                CreateFolder(localPath);

                foreach (var file in item.Files)
                {
                    downloadCount = 0;
                    while (true)
                    {
                        downloadResult = ntsFTPClient.DownloadFile(Path.Combine(localPath, file.Name), file.ServerPath);

                        if (downloadResult.FtpStatus == FtpStatus.Success)
                        {
                            break;
                        }

                        downloadCount++;

                        if (downloadCount > 10)
                        {
                            break;
                        }

                        Thread.Sleep(200);
                    }

                    if (downloadResult.FtpStatus == FtpStatus.Failed)
                    {
                        isOK = false;
                        resultModel.StatusCode = ResultModel.StatusCodeError;
                        resultModel.Message = downloadResult.Message;
                        break;
                    }
                }

                if (!isOK)
                {
                    break;
                }
            }

            return resultModel;
        }

        /// <summary>
        /// Download tài liệu thiết kế module phân bổ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<string> DownloadDesignDocumentShare(DownloadModuleDesignModel model)
        {
            List<string> lstError = new List<string>();
            if (model.ModuleIds == null || model.ModuleIds.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0017, TextResourceKey.Module);
            }

            ApiUtil apiUtil = new ApiUtil();
            var result = apiUtil.GetDataDownloadModule(model);

            if (!result.SuccessStatus)
            {
                throw NTSException.CreateInstance(result.Message);
            }

            // Khởi tạo thông tin FTP
            CreateFTP(model.ApiUrl, model.Token);

            string downloadPath;
            NtsFTPResult downloadResult;
            int downloadCount = 0;
            foreach (var item in result.Data)
            {
                if (!string.IsNullOrEmpty(item.Path))
                {
                    CreateFolder(Path.Combine(model.DownloadPath + "/" + item.Path));
                }

                foreach (var file in item.Files)
                {
                    downloadPath = Path.Combine(model.DownloadPath + "/" + file.Path);
                    CreateFolder(downloadPath);

                    downloadCount = 0;
                    while (true)
                    {
                        downloadResult = ntsFTPClient.DownloadFile(Path.Combine(downloadPath, file.Name), file.ServerPath);
                        if (downloadResult.FtpStatus == FtpStatus.Success)
                        {
                            break;
                        }

                        downloadCount++;

                        if (downloadCount > 10)
                        {
                            break;
                        }

                        Thread.Sleep(200);
                    }

                    if (downloadResult.FtpStatus == FtpStatus.Failed)
                    {
                        lstError.Add(downloadResult.Message);
                    }
                }
            }

            return lstError;
        }

        public void CreateFolder(string path)
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
