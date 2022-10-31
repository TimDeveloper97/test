using CoreFtp;
using CoreFtp.Infrastructure;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using NTS.Common;
using NTS.Common.FTP;
using NTS.Common.Logs;
using NTS.Common.Resource;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Common;
using QLTK.Service.Model.Downloads;
using QLTK.Service.Model.Files;
using QLTK.Service.Model.FTP;
using QLTK.Service.Model.Modules;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTK.Service.Business.Service
{
    public class ModuleDesignDocumentNew
    {
        private ApiUtil apiUtil = new ApiUtil();
        private NtsFTPClient ntsFTPClient;
        FileService fileService = new FileService();
        FTPServer fTPServer;

        public ModuleDesignDocumentNew()
        {

        }

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

        public async Task<UploadFolderResultModel> UploadFolderDesignDocument(UploadFolderModel folderModel)
        {
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatchTotal = new Stopwatch();
            stopwatchTotal.Start();
            stopwatch.Start();
            Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", 0, "Bắt đầu upload tài liệu thiết kế");

            Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", 0, $"ID Module upload: {folderModel.ModuleId}");

            var apiModel = apiUtil.GetModuleByModuleId(folderModel.ApiUrl, folderModel.ModuleId, folderModel.Token);

            if (!apiModel.SuccessStatus)
            {
                Log.WriteTracerProcessData(0, "UploadFolderDesignDocument.Error", stopwatch.ElapsedMilliseconds, apiModel.Message);
                stopwatchTotal.Stop();
                stopwatch.Stop();

                throw NTSException.CreateInstance(apiModel.Message);
            }

            ModuleModel moduleModel = apiModel.Data;

            Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", 0, $"Mã Module upload: {moduleModel.Code}");

            // Lấy dữ liệu
            var resultData = apiUtil.GetData(folderModel.ApiUrl, folderModel.Token, folderModel.DesignType);

            if (!resultData.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultData.Message);
            }

            var dataCheck = resultData.Data;

            UploadFolderResultModel uploadFolderResult = new UploadFolderResultModel();
            uploadFolderResult.ModuleId = folderModel.ModuleId;

            bool isCheckMaterial = true;
            if (folderModel.DesignType == QLTK.Service.Common.Constants.Design_Type_DN)
            {
                isCheckMaterial = false;
            }

            List<string> pathErrors = new List<string>();
            var check = fileService.CheckFolderUpload(folderModel.DesignType, folderModel.Path, moduleModel.Code, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode,
                   uploadFolderResult.ListError, dataCheck.ListFolderDefinition, dataCheck.ListFileDefinition, dataCheck.ListMaterialModel, dataCheck.ListRawMaterialsModel, dataCheck.ListManufacturerModel,
                   dataCheck.ListMaterialGroupModel, dataCheck.ListUnitModel, dataCheck.ListCodeRule, uploadFolderResult.LstError, uploadFolderResult.ListFolder, isCheckMaterial, out string materialPath,
                   dataCheck.Modules, pathErrors);

            if (!check.Status)
            {
                uploadFolderResult.Status = false;
                return uploadFolderResult;
            }


            if (folderModel.DesignType != QLTK.Service.Common.Constants.Design_Type_DN)
            {
                materialPath = QLTK.Service.Common.Constants.Disk_Design + fileService.GetPathFile(QLTK.Service.Common.Constants.FileDefinition_FileType_ListMaterial, moduleModel.Code, moduleModel.ModuleGroupCode,
                    moduleModel.ParentGroupCode, dataCheck.ListFolderDefinition, dataCheck.ListFileDefinition);

                if (!File.Exists(materialPath))
                {
                    Log.WriteTracerProcessData(0, "UploadFolderDesignDocument.Error", 0, ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0001, "File DMVT"));
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, "File DMVT");
                }

                uploadFolderResult.Materials = fileService.LoadListMaterial(materialPath);

                uploadFolderResult.Designers = GetMechanicalDesigner(materialPath, folderModel.DesignType);
            }
            else
            {
                uploadFolderResult.Designers = GetElectricDesigner(folderModel.DesignType, moduleModel, dataCheck);
            }

            //if (!CheckChangeOnServer(folderModel))
            //{
            //    uploadFolderResult.LstError.Add("Thư mục upload không khác với nguồn!");
            //    Log.WriteTracerProcessData(0, "UploadFolderDesignDocument.Error", 0, "Thư mục upload không khác với nguồn");
            //    return uploadFolderResult;
            //}

            // Lấy danh sách thư mục, file trong thư mục thiết kế dưới Local
            var folderUploads = fileService.GetAllFolderLocalUpload(folderModel.Path);

            // Không lấy được thư mục được chọn
            if (!folderUploads[0].LocalPath.Equals(folderModel.Path))
            {
                uploadFolderResult.LstError.Add("Lấy thông tin thư mục sai!");
                Log.WriteTracerProcessData(0, "UploadFolderDesignDocument.Error", 0, "Lấy thông tin thư mục sai!");
                return uploadFolderResult;
            }

            var pathFolderMAT = QLTK.Service.Common.Constants.Disk_Design + fileService.GetPathFolder(QLTK.Service.Common.Constants.FolderDefinition_FolderType_MAT, moduleModel.Code, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, dataCheck.ListFolderDefinition, dataCheck.ListFileDefinition);
            var pathFolderPLC = QLTK.Service.Common.Constants.Disk_Design + fileService.GetPathFolder(QLTK.Service.Common.Constants.FolderDefinition_FolderType_PLC, moduleModel.Code, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, dataCheck.ListFolderDefinition, dataCheck.ListFileDefinition);
            var pathFolderHMI = QLTK.Service.Common.Constants.Disk_Design + fileService.GetPathFolder(QLTK.Service.Common.Constants.FolderDefinition_FolderType_HMI, moduleModel.Code, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, dataCheck.ListFolderDefinition, dataCheck.ListFileDefinition);
            var pathFolderPM = QLTK.Service.Common.Constants.Disk_Design + fileService.GetPathFolder(QLTK.Service.Common.Constants.FolderDefinition_FolderType_Software, moduleModel.Code, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, dataCheck.ListFolderDefinition, dataCheck.ListFileDefinition);

            if (!string.IsNullOrEmpty(pathFolderMAT) && Directory.Exists(pathFolderMAT) && Directory.GetFiles(pathFolderMAT).Length > 0)
            {
                uploadFolderResult.FilmExist = true;
            }

            if (!string.IsNullOrEmpty(pathFolderPLC) && Directory.Exists(pathFolderPLC) && Directory.GetFiles(pathFolderPLC).Length > 0)
            {
                uploadFolderResult.PLCExist = true;
            }

            if (!string.IsNullOrEmpty(pathFolderHMI) && Directory.Exists(pathFolderHMI) && Directory.GetFiles(pathFolderHMI).Length > 0)
            {
                uploadFolderResult.HMIExist = true;
            }

            if (!string.IsNullOrEmpty(pathFolderPM) && Directory.Exists(pathFolderPM) && Directory.GetFiles(pathFolderPM).Length > 0)
            {
                uploadFolderResult.SoftwareExist = true;
            }

            // Đường dẫn thư mục cha trên server
            string serverPath = folderModel.Path.Replace(Path.GetPathRoot(folderModel.Path), "") + $"\\ver.{apiModel.Data.CurrentVersion}";


            Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", stopwatch.ElapsedMilliseconds, "Kết thúc xử lý dữ liệu");
            stopwatch.Restart();

            // Khởi tạo thông tin FTP
            CreateFTP(folderModel.ApiUrl, folderModel.Token);

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
                        Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", stopwatch.ElapsedMilliseconds, $"Kiểm tra folder lỗi: Không tạo được thư mục {folderModel.Path}");
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
                List<string> files = (await ftpClient.ListFilesAsync()).Select(r=> r.Name).ToList();

                string temp = string.Empty;
                // Upload thư mục lên server
                foreach (var folderUpload in folderUploads)
                {
                    stopwatch.Restart();
                    Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", 0, $"Bắt đầu kiểm tra folder: {folderModel.Path}");

                    // Đường dẫn thư mục trên server
                    folderUpload.ServerPath = folderUpload.LocalPath.Replace(folderModel.Path, serverPath);

                    folderUpload.LocalPath = folderUpload.LocalPath.Replace(folderModel.Path, Path.GetFileName(folderModel.Path));

                    if (!isOk)
                    {
                        break;
                    }

                    Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", stopwatch.ElapsedMilliseconds, $"Kết thúc kiểm tra folder: {folderModel.Path}");

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
                            var exten = Path.GetExtension(fileUpload.LocalPath);
                            fileUpload.ServerPath = Path.Combine(serverPath, Guid.NewGuid().ToString() + exten);

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
                Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", stopwatchTotal.ElapsedMilliseconds, $"Kết thúc Upload thiết kế: Module {moduleModel.Code} thành công!");
            }
            else
            {
                Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", stopwatchTotal.ElapsedMilliseconds, $"Kết thúc Upload thiết kế: Module {moduleModel.Code} không thành công!");
            }

            Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", 0, "===================================================================================================");

            stopwatchTotal.Stop();
            stopwatch.Stop();

            return uploadFolderResult;
        }

        private async Task UploadFile(string localFilePath, string serverFilePath, FtpClient ftpClient, UploadFolderResultModel uploadFolderResult)
        {
            var fileinfo = new FileInfo(localFilePath);

            int tryingTimes = 0;

            bool isUpload = false;
            byte[] buffer;
            int bytesRead = 0;
            long lengt = 0;

            while (true)
            {
                try
                {
                    using (var writeStream = await ftpClient.OpenFileWriteStreamNoCheckDirectoryAsync(Path.GetFileName(serverFilePath)))
                    {
                        var fileReadStream = fileinfo.OpenRead();
                        buffer = new byte[1048576];
                        bytesRead = fileReadStream.Read(buffer, 0, 1048576);
                        lengt = 0;

                        while (bytesRead != 0)
                        {
                            await writeStream.WriteAsync(buffer, 0, bytesRead);
                            bytesRead = await fileReadStream.ReadAsync(buffer, 0, 1048576);
                            lengt += bytesRead;
                        }

                        writeStream.Close();
                        fileReadStream.Close();
                    }

                    isUpload = true;
                    break;
                }
                catch (Exception ex)
                {
                    tryingTimes++;
                    Log.WriteTracerProcessData(0, "UploadFolderDesignDocument", 0, $"Upload lỗi {ex.Message}");

                    if (tryingTimes > 10)
                    {
                        isUpload = false;
                        break;
                    }

                    Thread.Sleep(100);
                }
            }

            if (!isUpload)
            {
                uploadFolderResult.LstError.Add($"Không upload được file {localFilePath}");
                return;
            }
        }


        private List<ModuleDesignerModel> GetElectricDesigner(int designType, ModuleModel moduleModel, DataCheckModuleUploadModel dataCheck)
        {
            List<ModuleDesignerModel> designers = new List<ModuleDesignerModel>();
            string electricPDFPath = QLTK.Service.Common.Constants.Disk_Design + fileService.GetPathFile(QLTK.Service.Common.Constants.FileDefinition_FileType_DnPDF, moduleModel.Code, moduleModel.ModuleGroupCode,
                moduleModel.ParentGroupCode, dataCheck.ListFolderDefinition, dataCheck.ListFileDefinition);
            PdfReader reader1 = null;
            try
            {
                reader1 = new PdfReader(electricPDFPath);

                string designer = reader1.Info["Author"];

                if (!string.IsNullOrEmpty(designer))
                {
                    ModuleDesignerModel moduleDesignerModel = new ModuleDesignerModel()
                    {
                        Designer = designer,
                        DesignType = designType
                    };

                    designers.Add(moduleDesignerModel);
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }

            if (reader1 != null)
            {
                reader1.Close();
            }

            return designers;
        }

        private List<ModuleDesignerModel> GetMechanicalDesigner(string path, int designType)
        {
            List<ModuleDesignerModel> designers = new List<ModuleDesignerModel>();

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.OpenReadOnly(path);
            IWorksheet sheet = workbook.Worksheets[0];

            try
            {
                string designer = sheet[4, 3].Value;

                if (!string.IsNullOrEmpty(designer))
                {
                    ModuleDesignerModel moduleDesignerModel = new ModuleDesignerModel()
                    {
                        Designer = designer,
                        DesignType = designType
                    };

                    designers.Add(moduleDesignerModel);
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }

            workbook.Close();
            excelEngine.Dispose();

            return designers;
        }

        /// <summary>
        /// Kiểm tra thay đổi so với server
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool CheckChangeOnServer(UploadFolderModel model)
        {
            bool checkOther = false;

            var resultApi = apiUtil.GetAllModuleDesignDocument(model.ApiUrl, model.Token, model.ModuleId, model.DesignType);
            if (!resultApi.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultApi.Message);
            }

            var moduelDocuments = resultApi.Data;

            var listFiles = fileService.GetAllFileInFolder(model.Path);

            if (moduelDocuments.Count == listFiles.Count && moduelDocuments.Count > 0)
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
                        //await ftpClient.ChangeWorkingDirectoryAsync(fileModel.ServerPath.Replace(fileModel.Name, ""));

                        //using (var ftpReadStream = await ftpClient.OpenFileReadStreamAsync(Path.GetFileName(file.ServerPath)))
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
                        downloadCount++;
                        if (downloadCount > 10)
                        {
                            isOk = false;
                            break;
                        }
                    }


                    Thread.Sleep(200);
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

            GoogleApi googleApi = new GoogleApi();
            string downloadPath;
            NtsFTPResult downloadResult;
            int downloadCount = 0;
            foreach (var item in result.Data)
            {
                if (!string.IsNullOrEmpty(item.Path))
                {
                    CreateFolder(Path.Combine(model.DownloadPath + "/" + item.Path));
                    if (item.IsExportMaterial)
                    {
                        DownloadDMVT(item, model.DownloadPath);
                    }
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

        /// <summary>
        /// Download tài liệu thiết kế module phân bổ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<string> DownloadProductDocuments(DownloadModuleDesignModel model)
        {
            List<string> lstError = new List<string>();

            ApiUtil apiUtil = new ApiUtil();
            var result = apiUtil.GetDataDownloadModule(model);

            if (!result.SuccessStatus)
            {
                throw NTSException.CreateInstance(result.Message);
            }

            // Khởi tạo thông tin FTP
            CreateFTP(model.ApiUrl, model.Token);

            GoogleApi googleApi = new GoogleApi();
            string downloadPath;
            NtsFTPResult downloadResult;
            int downloadCount = 0;
            foreach (var item in result.Data)
            {
                if (!string.IsNullOrEmpty(item.Path))
                {
                    CreateFolder(Path.Combine(model.DownloadPath + "/" + item.Path));
                    if (item.IsExportMaterial)
                    {
                        DownloadDMVT(item, model.DownloadPath);
                    }
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

        /// <summary>
        /// Download tài liệu thiết kế vật tư phân bổ
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<string>> DownloadMaterialDesignDocumentShare(DownloadMaterialDesignModel model)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;


            IWorkbook workbook = application.Workbooks.Open(new MemoryStream(Convert.FromBase64String(model.MaterialPath.Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", string.Empty))));
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            int column = sheet.Columns.Count();
            List<DownloadMarterialModel> listModuleMaterial = new List<DownloadMarterialModel>();
            DownloadMarterialModel moduleMaterial = new DownloadMarterialModel();
            try
            {
                string moduleCode, materialCode, specification;
                for (int i = 2; i <= rowCount; i++)
                {
                    moduleCode = sheet[i, 2].Value;
                    materialCode = sheet[i, 3].Value;
                    specification = sheet[i, 4].Value;
                    if (!string.IsNullOrEmpty(moduleCode) && !string.IsNullOrEmpty(materialCode))
                    {
                        moduleMaterial = new DownloadMarterialModel();
                        moduleMaterial.ModuleCode = moduleCode.Trim();
                        moduleMaterial.MaterialCode = materialCode.Trim();
                        moduleMaterial.Specification = specification;

                        listModuleMaterial.Add(moduleMaterial);
                    }
                }

                model.ListMaterialModule = listModuleMaterial.GroupBy(x => new { x.ModuleCode })
                                                            .Select(a => new DownloadMarterialModel
                                                            {
                                                                ModuleCode = a.Key.ModuleCode,
                                                                ListMaterials = a.ToList()
                                                            }).ToList();
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ex;
            }

            workbook.Close();
            List<string> lstError = new List<string>();
            if (string.IsNullOrEmpty(moduleMaterial.ModuleCode) || string.IsNullOrEmpty(moduleMaterial.MaterialCode))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0017, TextResourceKey.Material);
            }

            ApiUtil apiUtil = new ApiUtil();
            var result = await apiUtil.GetDataDownloadMaterial(model);

            if (!result.SuccessStatus)
            {
                throw NTSException.CreateInstance(result.Message);
            }

            // Khởi tạo thông tin FTP
            CreateFTP(model.ApiUrl, model.Token);

            GoogleApi googleApi = new GoogleApi();
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

        public void DownloadDMVT(DownloadModuleDesignDataModel model, string downloadPath)
        {
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(Application.StartupPath + "/Template/DownloadDMVT_Template.xls");

            IWorksheet sheet = workbook.Worksheets[0];
            IRange iRangeData1 = sheet.FindFirst("<Code>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData1.Text = iRangeData1.Text.Replace("<Code>", model.ProjectCode);
            IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

            string folderName = Path.GetFileName(model.Path);

            IRange iRangeDataRoom = sheet.FindFirst("<room>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataRoom.Text = iRangeDataRoom.Text.Replace("<room>", folderName);

            var total = model.ListMaterial.Count;
            var listExport = model.ListMaterial.Select((a, i) => new
            {
                a.IndexExport,
                a.ModuleCode,
                a.MaterialName, // tên
                a.MaterialCode, // mã
                a.Quantity,
                view6 = "",
                view7 = "",
                view8 = "",
                view9 = "",
                view10 = "",
                view11 = "",
                view12 = "",
                view13 = "",
                view14 = "",
                a.RawMaterial
            }).ToList();

            if (listExport.Count > 0)
            {
                if (listExport.Count > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders.Color = ExcelKnownColors.Black;
                sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 3].CellStyle.WrapText = true;
            }

            string pathFileSave = Path.Combine(downloadPath, model.Path, model.ProjectCode + "-" + folderName + ".xls");
            workbook.SaveAs(pathFileSave);
            workbook.Close();
        }
    }
}
