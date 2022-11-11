using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using NTS.Common;
using QLTK.Service.Business.Common;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Model.Modules;
using QLTK.Service.Model.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLTK.Service.Business.Service
{
    public class GoogleApi
    {
        private ApiUtil apiUtil = new ApiUtil();

        #region Setting Drive
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.DriveFile };
        static string ApplicationName = "Drive API .NET Quickstart";
        //static string APIKey = "AIzaSyBcF5rg9deP42GnByvtSO0L0hiFpD9TS2k";
        FileService fileService = new FileService();

        static UserCredential initDrive()
        {
            UserCredential credential;
            var filePath = Application.StartupPath + "/Lib/credentials.json";
            using (var stream =
                new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                string credPath = Application.StartupPath + "/token.json";
                IDataStore StoredRefreshToken = new FileDataStore(credPath, true);
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
            return credential;
        }

        #endregion

        #region ListFile
        public void ListFile()
        {
            UserCredential credential = initDrive();

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApiKey = APIKey,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();

        }
        #endregion

        #region Download
        public void DownloadFile(string Id, string fileName, string path)
        {
            UserCredential credential = initDrive();
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            DownloadAFile(service, Id, fileName, path);
        }

        public void DownloadFolder(string Id, string path)
        {
            string pathString = string.Empty;
            DirectoryInfo direct;
            UserCredential credential = initDrive();
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            //Lấy tên thư mục trên google drive và tạo thư mục trên client
            var folderName = service.Files.Get(Id).Execute().Name;
            if (!string.IsNullOrEmpty(folderName))
            {
                pathString = System.IO.Path.Combine(path, folderName);
                direct = Directory.CreateDirectory(pathString);
            }

            //Duyệt file trong folder và download
            DownloadAllFile(service, pathString, Id);
        }

        private void DownloadAllFile(DriveService service, string path, string Id)
        {
            var request = service.Files.List();
            request.Q = "'" + Id + "' in parents";
            IList<Google.Apis.Drive.v3.Data.File> files = request.Execute()
                .Files;
            Console.WriteLine("Files:");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (!file.MimeType.Equals("application/vnd.google-apps.folder"))
                    {
                        DownloadAFile(service, file.Id, file.Name, path);
                    }
                    else
                    {
                        string pathString = string.Empty;
                        DirectoryInfo direct;
                        var folderName = service.Files.Get(file.Id).Execute().Name;
                        if (!string.IsNullOrEmpty(folderName))
                        {
                            pathString = System.IO.Path.Combine(path, folderName);
                            direct = Directory.CreateDirectory(pathString);
                            DownloadAllFile(service, pathString, file.Id);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
        }

        public void DownloadAFile(DriveService service, string Id, string fileName, string path)
        {
            var request = service.Files.Get(Id);
            request.Execute();
            var stream = new System.IO.MemoryStream();

            request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download complete.");
                                SaveStream(stream, fileName, path);
                                break;
                            }
                        case DownloadStatus.Failed:
                            {

                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
            request.Download(stream);
        }
        #endregion

        #region Upload
        public UploadFolderResultModel UploadFolder(UploadFolderModel model)
        {
            var apiModel = apiUtil.GetModuleInfo(model.ApiUrl, model.ModuleCode, model.Token);
            ModuleModel moduleModel = new ModuleModel();

            if (apiModel.Data != null)
            {
                moduleModel = (ModuleModel)apiModel.Data;
            }
            else
            {
                throw NTSException.CreateInstance(apiModel.Message);
            }

            bool isCheck = true;
            if (model.DesignType == 2)
            {
                isCheck = false;
            }
            UploadFolderResultModel uploadFolderResult = new UploadFolderResultModel();
            var resultAPI = apiUtil.GetData(model.ApiUrl, model.Token, model.DesignType);
            if (resultAPI.SuccessStatus)
            {
                var dataCheck = resultAPI.Data;

                //Gán giá trị vào model
                model.ListCodeRule = dataCheck.ListCodeRule;
                model.ListFileDefinition = dataCheck.ListFileDefinition;
                model.ListFolderDefinition = dataCheck.ListFolderDefinition;
                model.ListManufacturerModel = dataCheck.ListManufacturerModel;
                model.ListMaterialGroupModel = dataCheck.ListMaterialGroupModel;
                model.ListMaterialModel = dataCheck.ListMaterialModel;
                model.ListRawMaterialsModel = dataCheck.ListRawMaterialsModel;
                model.ListUnitModel = dataCheck.ListUnitModel;
                List<string> pathErrors = new List<string>();
                //Check cấu trúc file
                var check = fileService.CheckFolderUpload(model.DesignType, model.Path, model.ModuleCode, model.ModuleGroupCode, moduleModel.ParentGroupCode,
                    uploadFolderResult.ListError, model.ListFolderDefinition, model.ListFileDefinition, model.ListMaterialModel, model.ListRawMaterialsModel, model.ListManufacturerModel,
                    model.ListMaterialGroupModel, model.ListUnitModel, model.ListCodeRule, uploadFolderResult.LstError, uploadFolderResult.ListFolder, isCheck, out string materialPath, 
                    dataCheck.Modules, pathErrors);
                if (check.Status)
                {
                    var isDiff = CheckUploadFileDoc(model);
                    if (isDiff)
                    {
                        uploadFolderResult.Materials = check.Result.ListModuleMaterial;
                        bool isSuccess = true;
                        List<ModuleDesignDocumentModel> ListResult = new List<ModuleDesignDocumentModel>();
                        UserCredential credential = initDrive();
                        var service = new DriveService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential,
                            ApplicationName = ApplicationName,
                            //ApiKey = APIKey,
                        });
                        string rootId = "root";
                        if (model.DesignType == 1)
                        {
                            rootId = CheckFileExist("Thietke.CK", "root", service);
                            if (string.IsNullOrEmpty(rootId))
                            {
                                rootId = CreateFolder("Thietke.CK", "root", service);
                            }
                        }
                        else if (model.DesignType == 2)
                        {
                            rootId = CheckFileExist("Thietke.DN", "root", service);
                            if (string.IsNullOrEmpty(rootId))
                            {
                                rootId = CreateFolder("Thietke.CK", "root", service);
                            }
                        }
                        else if (model.DesignType == 3)
                        {
                            rootId = CheckFileExist("Thietke.DT", "root", service);
                            if (string.IsNullOrEmpty(rootId))
                            {
                                rootId = CreateFolder("Thietke.CK", "root", service);
                            }
                        }

                        string folderName = Path.GetFileName(model.Path);
                        FolderModel folderModel = new FolderModel()
                        {
                            Id = model.Path,
                            Name = folderName + "_" + DateTime.Now.ToString("dd/MM/yyyy"),
                            Path = folderName,
                            FullPath = model.Path,
                            ParentId = rootId,
                            IsRoot = true
                        };

                        var parentId = string.Empty;
                        var checkFileIdExist = CheckFileExist(folderModel.Name, folderModel.ParentId, service);
                        if (string.IsNullOrEmpty(checkFileIdExist))
                        {
                            parentId = CreateFolder(service, folderModel, ListResult, model.ModuleId);
                        }
                        else
                        {
                            parentId = checkFileIdExist;
                        }

                        var listFolderChild = fileService.GetChildDirectories(model.Path).Select(t => new FolderModel
                        {
                            Id = t.FullName,
                            Name = t.Name,
                            Path = folderModel.Path + "\\" + t.Name,
                            FullPath = t.FullName,
                            ParentId = parentId,
                            IsRoot = false
                        }).ToList();

                        if (listFolderChild.Count > 0)
                        {
                            UploadFile(service, listFolderChild, ListResult, model.ModuleId, isSuccess);
                        }

                        var listFile = fileService.GetChildFiles(model.Path).Select(t => new FolderModel
                        {
                            Id = t.FullName,
                            Name = t.Name,
                            Path = folderModel.Path + "\\" + t.Name,
                            FullPath = t.FullName,
                            ParentId = parentId,
                            Extension = t.Extension,
                            IsRoot = false
                        }).ToList();

                        if (listFile.Count > 0)
                        {
                            string checkId;
                            ModuleDesignDocumentModel fileModel;
                            foreach (var ite in listFile)
                            {
                                checkId = CheckFileExist(ite.Name, ite.ParentId, service);
                                if (string.IsNullOrEmpty(checkId))
                                {
                                    var fileId = UploadAFile(service, ite);
                                    if (!string.IsNullOrEmpty(fileId))
                                    {
                                        fileModel = new ModuleDesignDocumentModel()
                                        {
                                            Id = fileId,
                                            Name = ite.Name,
                                            FileType = "1",
                                            ParentId = ite.ParentId,
                                            Path = ite.Path,
                                            ModuleId = model.ModuleId
                                        };
                                        ListResult.Add(fileModel);
                                    }
                                    else
                                    {
                                        isSuccess = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    UpdateFile(service, ite, checkId);
                                }

                            }
                        }
                        uploadFolderResult.Materials = check.Result.ListModuleMaterial;
                        var first = ListResult.FirstOrDefault(t => rootId.Equals(t.ParentId));
                        if (first != null)
                        {
                            first.ParentId = model.DesignType.ToString();
                        }
                        //uploadFolderResult.DesignDocuments = ListResult;
                        uploadFolderResult.Status = true;
                        uploadFolderResult.IsUploadSuccess = isSuccess;
                    }
                    else
                    {
                        uploadFolderResult.LstError.Add("Thư mục upload không khác với nguồn!");
                    }

                }
                else
                {
                    uploadFolderResult.Status = false;
                }
            }
            else
            {
                uploadFolderResult.Status = false;
            }
            return uploadFolderResult;
        }

        public GoogleUploadFolderProductResultModel UploadFolderProduct(UploadFolderProductModel model)
        {
            GoogleUploadFolderProductResultModel uploadFolderResult = new GoogleUploadFolderProductResultModel();
            bool isSuccess = true;
            List<ProductDesignDocumentModel> ListResult = new List<ProductDesignDocumentModel>();
            UserCredential credential = initDrive();
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
                //ApiKey = APIKey,
            });
            string rootId = "root";
            rootId = CheckFileExist("Thietke.Tb", "root", service);
            if (string.IsNullOrEmpty(rootId))
            {
                rootId = CreateFolder("Thietke.Tb", "root", service);
            }

            string folderName = Path.GetFileName(model.Path);
            FolderModel folderModel = new FolderModel()
            {
                Id = model.Path,
                Name = folderName,
                Path = folderName,
                FullPath = model.Path,
                ParentId = rootId,
                IsRoot = true
            };

            var parentId = string.Empty;
            var checkFileIdExist = CheckFileExist(folderModel.Name, folderModel.ParentId, service);
            if (string.IsNullOrEmpty(checkFileIdExist))
            {
                parentId = CreateFolderProduct(service, folderModel, ListResult, model.Id);
            }
            else
            {
                parentId = checkFileIdExist;
            }

            var listFolderChild = fileService.GetChildDirectories(model.Path).Select(t => new FolderModel
            {
                Id = t.FullName,
                Name = t.Name,
                Path = folderModel.Path + "\\" + t.Name,
                FullPath = t.FullName,
                ParentId = parentId,
                IsRoot = false
            }).ToList();

            if (listFolderChild.Count > 0)
            {
                UploadFileProduct(service, listFolderChild, ListResult, model.Id, isSuccess);
            }

            var listFile = fileService.GetChildFiles(model.Path).Select(t => new FolderModel
            {
                Id = t.FullName,
                Name = t.Name,
                Path = folderModel.Path + "\\" + t.Name,
                FullPath = t.FullName,
                ParentId = parentId,
                Extension = t.Extension,
                FileSize = t.Length,
                IsRoot = false
            }).ToList();

            if (listFile.Count > 0)
            {
                string checkId;
                ProductDesignDocumentModel fileModel;
                string fileId;
                foreach (var ite in listFile)
                {
                    checkId = CheckFileExist(ite.Name, ite.ParentId, service);
                    if (string.IsNullOrEmpty(checkId))
                    {
                        fileId = UploadAFile(service, ite);
                        if (!string.IsNullOrEmpty(fileId))
                        {
                            fileModel = new ProductDesignDocumentModel()
                            {
                                Id = fileId,
                                Name = ite.Name,
                                FileType = "1",
                                ParentId = ite.ParentId,
                                Path = ite.Path,
                                ProductId = model.Id,
                                FileSize = ite.FileSize
                            };
                            ListResult.Add(fileModel);
                        }
                        else
                        {
                            isSuccess = false;
                            break;
                        }
                    }
                    else
                    {
                        UpdateFile(service, ite, checkId);
                    }

                }
            }
            var first = ListResult.FirstOrDefault(t => rootId.Equals(t.ParentId));
            if (first != null)
            {
                first.ParentId = string.Empty;
            }
            uploadFolderResult.ListResult = ListResult;
            uploadFolderResult.IsUploadSuccess = isSuccess;
            return uploadFolderResult;
        }

        public void UploadFile(DriveService service, List<FolderModel> list, List<ModuleDesignDocumentModel> ListResult, string moduleId, bool isSuccess)
        {
            string checkFolderId;
            string parentId;
            List<FolderModel> listFile;
            List<FolderModel> listFolder;
            string checkFileId;
            ModuleDesignDocumentModel fileModel;
            string fileId;
            foreach (var item in list)
            {
                checkFolderId = CheckFileExist(item.Name, item.ParentId, service);
                parentId = string.Empty;
                if (string.IsNullOrEmpty(checkFolderId))
                {
                    parentId = CreateFolder(service, item, ListResult, moduleId);
                }
                else
                {
                    parentId = checkFolderId;
                }

                listFolder = fileService.GetChildDirectories(item.FullPath).Select(t => new FolderModel
                {
                    Id = t.FullName,
                    Name = t.Name,
                    Path = item.Path + "\\" + t.Name,
                    FullPath = t.FullName,
                    ParentId = parentId,
                    IsRoot = false
                }).ToList();

                if (listFolder.Count > 0)
                {
                    UploadFile(service, listFolder, ListResult, moduleId, isSuccess);
                }

                listFile = fileService.GetChildFiles(item.FullPath).Select(t => new FolderModel
                {
                    Id = t.FullName,
                    Name = t.Name,
                    Path = item.Path + "\\" + t.Name,
                    FullPath = t.FullName,
                    ParentId = parentId,
                    Extension = t.Extension,
                    FileSize = t.Length,
                    IsRoot = false
                }).ToList();

                if (listFile.Count > 0)
                {
                    foreach (var ite in listFile)
                    {
                        checkFileId = CheckFileExist(ite.Name, ite.ParentId, service);
                        if (string.IsNullOrEmpty(checkFileId))
                        {
                            fileId = UploadAFile(service, ite);
                            if (!string.IsNullOrEmpty(fileId))
                            {
                                fileModel = new ModuleDesignDocumentModel()
                                {
                                    Id = fileId,
                                    Name = ite.Name,
                                    FileType = "1",
                                    ParentId = ite.ParentId,
                                    Path = ite.Path,
                                    ModuleId = moduleId,
                                    FileSize = ite.FileSize
                                };
                                ListResult.Add(fileModel);
                            }
                            else
                            {
                                isSuccess = false;
                                break;
                            }
                        }
                        else
                        {
                            UpdateFile(service, ite, checkFileId);
                        }
                    }
                }
            }
        }

        public void UploadFileProduct(DriveService service, List<FolderModel> list, List<ProductDesignDocumentModel> ListResult, string productId, bool isSuccess)
        {
            string checkFolderId;
            string parentId;
            List<FolderModel> listFolder;
            List<FolderModel> listFile;
            string checkFileId;
            ProductDesignDocumentModel fileModel;
            string fileId;
            foreach (var item in list)
            {
                checkFolderId = CheckFileExist(item.Name, item.ParentId, service);
                parentId = string.Empty;
                if (string.IsNullOrEmpty(checkFolderId))
                {
                    parentId = CreateFolderProduct(service, item, ListResult, productId);
                }
                else
                {
                    parentId = checkFolderId;
                }

                listFolder = fileService.GetChildDirectories(item.FullPath).Select(t => new FolderModel
                {
                    Id = t.FullName,
                    Name = t.Name,
                    Path = item.Path + "\\" + t.Name,
                    FullPath = t.FullName,
                    ParentId = parentId,
                    IsRoot = false
                }).ToList();

                if (listFolder.Count > 0)
                {
                    UploadFileProduct(service, listFolder, ListResult, productId, isSuccess);
                }

                listFile = fileService.GetChildFiles(item.FullPath).Select(t => new FolderModel
                {
                    Id = t.FullName,
                    Name = t.Name,
                    Path = item.Path + "\\" + t.Name,
                    FullPath = t.FullName,
                    ParentId = parentId,
                    Extension = t.Extension,
                    FileSize = t.Length,
                    IsRoot = false
                }).ToList();

                if (listFile.Count > 0)
                {
                    foreach (var ite in listFile)
                    {
                        checkFileId = CheckFileExist(ite.Name, ite.ParentId, service);
                        if (string.IsNullOrEmpty(checkFileId))
                        {
                            fileId = UploadAFile(service, ite);
                            if (!string.IsNullOrEmpty(fileId))
                            {
                                fileModel = new ProductDesignDocumentModel()
                                {
                                    Id = fileId,
                                    Name = ite.Name,
                                    FileType = "1",
                                    ParentId = ite.ParentId,
                                    Path = ite.Path,
                                    ProductId = productId,
                                    FileSize = ite.FileSize
                                };
                                ListResult.Add(fileModel);
                            }
                            else
                            {
                                isSuccess = false;
                                break;
                            }
                        }
                        else
                        {
                            UpdateFile(service, ite, checkFileId);
                        }
                    }
                }
            }
        }

        public string UploadAFile(DriveService service, FolderModel model)
        {
            var contentType = MimeTypeMap.GetMimeType(model.Extension);
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = model.Name,
                Parents = new List<string> { model.ParentId },
                MimeType = contentType,

            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(model.FullPath,
                                    System.IO.FileMode.Open))
            {
                request = service.Files.Create(
                    fileMetadata, stream, contentType);
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            return file.Id;
        }

        public string CreateFolder(DriveService service, FolderModel model, List<ModuleDesignDocumentModel> ListResult, string moduleId)
        {
            var name = string.Empty;
            if (model.IsRoot)
            {
                name = model.Path;
            }
            else
            {
                name = model.Name;
            }

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = model.Name,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { model.ParentId }
            };
            var folder = service.Files.Create(fileMetadata).Execute();
            if (!string.IsNullOrEmpty(folder.Id))
            {
                ModuleDesignDocumentModel fileModel = new ModuleDesignDocumentModel()
                {
                    Id = folder.Id,
                    Name = name,
                    FileType = "2",
                    ParentId = model.ParentId,
                    Path = model.Path,
                    ModuleId = moduleId
                };
                ListResult.Add(fileModel);
            }
            return folder.Id;
        }

        public string CreateFolderProduct(DriveService service, FolderModel model, List<ProductDesignDocumentModel> ListResult, string productId)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = model.Name,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { model.ParentId }
            };
            var folder = service.Files.Create(fileMetadata).Execute();
            if (!string.IsNullOrEmpty(folder.Id))
            {
                ProductDesignDocumentModel fileModel = new ProductDesignDocumentModel()
                {
                    Id = folder.Id,
                    Name = model.Name,
                    FileType = "2",
                    ParentId = model.ParentId,
                    Path = model.Path,
                    ProductId = productId
                };
                ListResult.Add(fileModel);
            }
            return folder.Id;
        }
        #endregion

        #region Update
        public void UpdateFile(DriveService service, FolderModel model, string fileId)
        {

            var contentType = MimeTypeMap.GetMimeType(model.Extension);
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = model.Name,
                MimeType = contentType
            };
            FilesResource.UpdateMediaUpload request;
            using (var stream = new System.IO.FileStream(model.FullPath,
                                    System.IO.FileMode.Open))
            {
                request = service.Files.Update(fileMetadata, fileId, stream, contentType);
                request.Upload();
                var file = request.ResponseBody;
            }
        }
        #endregion

        #region Private
        private static void SaveStream(System.IO.MemoryStream stream, string fileName, string path)
        {
            var saveTo = System.IO.Path.Combine(path, fileName);
            using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }

        private string CheckFileExist(string folderName, string parentId, DriveService service)
        {
            var fileId = string.Empty;
            var request = service.Files.List();
            var res = service.Files.List();
            res.Q = "mimeType = 'application/vnd.google-apps.folder'";
            request.Q = "'" + parentId + "' in parents and name contains '" + folderName + "'";
            Google.Apis.Drive.v3.Data.File file = request.Execute()
                .Files.FirstOrDefault();
            var lst = res.Execute();
            if (file != null)
            {
                fileId = file.Id;
            }
            return fileId;
        }

        private string CreateFolder(string folderName, string parentId, DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { parentId }
            };
            var folder = service.Files.Create(fileMetadata).Execute();
            return folder.Id;
        }
        #endregion



        #region check file and folder 

        public bool CheckUploadFileDoc(UploadFolderModel model)
        {
            bool checkOther = false; // không có sai khác

            model.ListFileModuleDesignDoc = apiUtil.GetAllModuleDesignDocument(model.ApiUrl, model.Token, model.ModuleId, model.DesignType).Data;

            string folderName = Path.GetFileName(model.Path);

            var listFiles = checkFile(model);

            if (model.ListFileModuleDesignDoc.Count == listFiles.Count)
            {
                foreach (var item in listFiles)
                {
                    var checkExits = model.ListFileModuleDesignDoc.Where(a => a.Name.Equals(item.Name) && a.FileSize.Equals(item.FileSize)).FirstOrDefault();
                    if (checkExits == null)
                    {
                        checkOther = true;
                    }
                    break;
                }
            }
            else
            {
                checkOther = true; // có sai khác
            }

            return checkOther;
        }

        public List<ModuleDesignDocumentModel> checkFile(UploadFolderModel model)
        {
            List<ModuleDesignDocumentModel> ListResult = new List<ModuleDesignDocumentModel>();

            string folderName = Path.GetFileName(model.Path);
            FolderModel folderModel = new FolderModel()
            {
                Id = model.Path,
                Name = folderName,
                Path = folderName,
                FullPath = model.Path,
            };
            if (!string.IsNullOrEmpty(folderModel.Id))
            {
                ModuleDesignDocumentModel fileModel = new ModuleDesignDocumentModel()
                {
                    Id = folderModel.Id,
                    Name = folderModel.Name,
                    Path = model.Path,
                    FileSize = 0
                };
                ListResult.Add(fileModel);
            }

            var listFolderChild = fileService.GetChildDirectories(model.Path).Select(t => new FolderModel
            {
                Id = t.FullName,
                Name = t.Name,
                Path = folderModel.Path + "\\" + t.Name,
                FullPath = t.FullName,
                FileSize = 0
            }).ToList();

            if (listFolderChild.Count > 0)
            {
                checkFiles(listFolderChild, ListResult);
            }

            var listFile = fileService.GetChildFiles(model.Path).Select(t => new FolderModel
            {
                Id = t.FullName,
                Name = t.Name,
                Path = folderModel.Path + "\\" + t.Name,
                FullPath = t.FullName,
                Extension = t.Extension,
                FileSize = t.Length
            }).ToList();

            if (listFile.Count > 0)
            {
                foreach (var ite in listFile)
                {

                    ModuleDesignDocumentModel fileModel = new ModuleDesignDocumentModel()
                    {
                        Name = ite.Name,
                        FileType = "1",
                        ParentId = ite.ParentId,
                        Path = ite.Path,
                        ModuleId = model.ModuleId,
                        FileSize = ite.FileSize
                    };
                    ListResult.Add(fileModel);
                }
            }
            return ListResult;
        }

        // hàm đệ quy check foder con
        public void checkFiles(List<FolderModel> list, List<ModuleDesignDocumentModel> ListResult)
        {
            foreach (var item in list)
            {
                ModuleDesignDocumentModel folderModel = new ModuleDesignDocumentModel()
                {
                    Name = item.Name,
                    FileType = "2",
                    FileSize = item.FileSize
                };
                ListResult.Add(folderModel);

                var listFolder = fileService.GetChildDirectories(item.FullPath).Select(t => new FolderModel
                {
                    Id = t.FullName,
                    Name = t.Name,
                    Path = item.Path + "\\" + t.Name,
                    FullPath = t.FullName,
                    FileSize = 0
                }).ToList();

                if (listFolder.Count() > 0)
                {
                    checkFiles(listFolder, ListResult);
                }

                var listFile = fileService.GetChildFiles(item.FullPath).Select(t => new FolderModel
                {
                    Id = t.FullName,
                    Name = t.Name,
                    Path = item.Path + "\\" + t.Name,
                    FullPath = t.FullName,
                    Extension = t.Extension,
                    FileSize = t.Length
                }).ToList();

                if (listFile.Count > 0)
                {
                    foreach (var ite in listFile)
                    {
                        ModuleDesignDocumentModel fileModel = new ModuleDesignDocumentModel()
                        {
                            Name = ite.Name,
                            FileType = "1",
                            ParentId = ite.ParentId,
                            Path = ite.Path,
                            FileSize = ite.FileSize
                        };
                        ListResult.Add(fileModel);
                    }
                }
            }
        }
        #endregion
    }
}

