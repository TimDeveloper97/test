using QLTK.Service.Business.Common;
using QLTK.Service.Business.Model;
using QLTK.Service.Common;
using QLTK.Service.Model.Upload;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NTS.Common;
using QLTK.Service.Model.Files;
using NTS.Common.Logs;
using QLTK.Service.Model.Definitions;
using QLTK.Service.Model.Commons;
using QLTK.Service.Model.Materials;
using QLTK.Service.Model.Modules;

namespace QLTK.Service.Business.Service
{
    public class FileService
    {
        public List<FolderModel> GetRoot()
        {
            var list = GetRootDirectories();
            List<FolderModel> rs = new List<FolderModel>();
            FolderModel desktopModel = new FolderModel()
            {
                Id = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Name = "Desktop",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            rs.Add(desktopModel);
            FolderModel childDesktopModel = new FolderModel()
            {
                Id = desktopModel.Id + "1",
                Name = desktopModel.Name + "1",
                Path = desktopModel.Path + "1",
                ParentId = desktopModel.Id
            };
            rs.Add(childDesktopModel);

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    FolderModel model = new FolderModel()
                    {
                        Id = item.Name,
                        Name = item.Name,
                        Path = item.Name
                    };
                    rs.Add(model);

                    FolderModel childModel = new FolderModel()
                    {
                        Id = item.Name + "1",
                        Name = item.Name + "1",
                        Path = item.Name + "1",
                        ParentId = item.Name
                    };
                    rs.Add(childModel);
                }
            }

            return rs;
        }

        public List<FolderModel> GetChild(string Id, List<FolderModel> list)
        {
            var lst = GetChildDirectories(Id);
            var folders = list.Where(t => Id.Equals(t.ParentId)).ToList();
            if (folders.Count > 0)
            {
                foreach (var item in folders)
                {
                    list.Remove(item);
                }
            }
            if (lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    FolderModel model = new FolderModel()
                    {
                        Id = item.FullName,
                        Name = item.Name,
                        Path = item.FullName,
                        ParentId = Id
                    };
                    list.Add(model);

                    FolderModel childModel = new FolderModel()
                    {
                        Id = item.FullName + "1",
                        Name = item.Name + "1",
                        Path = "",
                        ParentId = item.FullName
                    };
                    list.Add(childModel);
                }
            }
            return list;
        }

        public List<FolderModel> GetFoder(string path)
        {
            //\\TPAE.K\\TPAE.K4500.Ck
            //string path = "D:\\Thietke.Ck\\TPAE";
            string[] listPath = path.Split('\\');
            int index = 0;
            var list = GetRootDirectories();
            List<FolderModel> rs = new List<FolderModel>();
            FolderModel desktopModel = new FolderModel()
            {
                Id = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Name = "Desktop",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            rs.Add(desktopModel);
            FolderModel childDesktopModel = new FolderModel()
            {
                Id = desktopModel.Id + "1",
                Name = desktopModel.Name + "1",
                Path = desktopModel.Path + "1",
                ParentId = desktopModel.Id
            };
            rs.Add(childDesktopModel);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    FolderModel model = new FolderModel()
                    {
                        Id = item.Name,
                        Name = item.Name,
                        Path = item.Name
                    };
                    rs.Add(model);

                    FolderModel childModel = new FolderModel()
                    {
                        Id = item.Name + "1",
                        Name = item.Name + "1",
                        Path = item.Name + "1",
                        ParentId = item.Name
                    };
                    rs.Add(childModel);
                }
            }

            foreach (var item in list)
            {
                if (item.Name.Equals(listPath[index] + "\\"))
                {
                    index++;
                    GetFolderChild(item.Name, rs, index, listPath, path);
                }
            }

            return rs;
        }

        public List<FolderModel> GetFolderChild(string Id, List<FolderModel> list, int index, string[] listPath, string path)
        {
            var folders = list.Where(t => Id.Equals(t.ParentId)).ToList();
            if (folders.Count > 0)
            {
                foreach (var item in folders)
                {
                    list.Remove(item);
                }
            }
            var lst = GetChildDirectories(Id);
            if (lst.Count > 0 && index <= listPath.Length)
            {
                foreach (var item in lst)
                {
                    FolderModel model = new FolderModel()
                    {
                        Id = item.FullName,
                        Name = item.Name,
                        Path = item.FullName,
                        ParentId = Id
                    };
                    list.Add(model);

                    FolderModel childModel = new FolderModel()
                    {
                        Id = item.FullName + "1",
                        Name = item.Name + "1",
                        Path = "",
                        ParentId = item.FullName
                    };
                    list.Add(childModel);
                }
            }
            foreach (var item in lst)
            {
                if (index < listPath.Length && item.Name.Equals(listPath[index]))
                {
                    index++;
                    if (item.FullName.Equals(path))
                    {
                        break;
                    }
                    GetFolderChild(item.FullName, list, index, listPath, path);
                }
            }

            return list;
        }

        private static IList<DriveInfo> GetRootDirectories()
        {
            return (from x in DriveInfo.GetDrives() select x).ToList();
        }

        public List<DirectoryInfo> GetChildDirectories(string directory)
        {
            List<DirectoryInfo> list = new List<DirectoryInfo>();
            try
            {
                list = (from x in Directory.GetDirectories(directory) select new DirectoryInfo(x)).ToList();
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public IList<FileInfo> GetChildFiles(string directory)
        {
            try
            {
                return (from x in Directory.GetFiles(directory) select new FileInfo(x)).ToList();
            }
            catch (Exception e)
            {

            }
            return new List<FileInfo>();
        }

        public IList<FileInfo> GetAllFileInFolder(string directory)
        {
            try
            {
                List<FileInfo> files = new List<FileInfo>();
                var listFolders = Directory.GetDirectories(directory);

                foreach (var fodler in listFolders)
                {
                    files.AddRange(GetAllFileInFolder(fodler));
                }

                files.AddRange(GetChildFiles(directory));

                return files;
            }
            catch (Exception e)
            {

            }
            return new List<FileInfo>();
        }

        public IList<string> GetAllFolder(string directory)
        {
            try
            {
                List<string> folders = new List<string>();
                var listFolders = Directory.GetDirectories(directory);
                folders.AddRange(listFolders);
                foreach (var fodler in listFolders)
                {
                    folders.AddRange(GetAllFolder(fodler));
                }

                return folders;
            }
            catch (Exception e)
            {

            }
            return new List<string>();
        }

        public List<FolderUploadModel> GetAllFolderLocalUpload(string directory)
        {
            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            try
            {

                FolderUploadModel folderParentModel;

                folderParentModel = new FolderUploadModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    LocalPath = directory,
                    Name = Path.GetFileName(directory),
                    Files = GetAllFileLocalUpload(directory)
                };

                folders.Add(folderParentModel);

                FolderUploadModel folderModel;

                var listFolders = Directory.GetDirectories(directory);
                foreach (var fodler in listFolders)
                {
                    folderModel = new FolderUploadModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = folderParentModel.Id,
                        LocalPath = fodler,
                        Name = Path.GetFileName(fodler),
                        Files = GetAllFileLocalUpload(fodler)
                    };

                    folders.Add(folderModel);

                    folders.AddRange(GetAllFolderChildLocalUpload(fodler, folderModel.Id));
                }
            }
            catch
            {

            }
            return folders;
        }

        public List<FolderUploadModel> GetAllFolderChildLocalUpload(string directory, string parentId)
        {
            List<FolderUploadModel> folders = new List<FolderUploadModel>();
            try
            {

                FolderUploadModel folderModel;

                var listFolders = Directory.GetDirectories(directory);
                foreach (var fodler in listFolders)
                {
                    folderModel = new FolderUploadModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = parentId,
                        LocalPath = fodler,
                        Name = Path.GetFileName(fodler),
                        Files = GetAllFileLocalUpload(fodler)
                    };

                    folders.Add(folderModel);

                    folders.AddRange(GetAllFolderChildLocalUpload(fodler, folderModel.Id));
                }
            }
            catch
            {

            }
            return folders;
        }

        public List<FileUploadModel> GetAllFileLocalUpload(string directory)
        {
            List<FileUploadModel> files = new List<FileUploadModel>();
            try
            {
                var listFiles = GetChildFiles(directory);
                FileUploadModel fielModel;
                foreach (var file in listFiles)
                {
                    fielModel = new FileUploadModel()
                    {
                        LocalPath = file.FullName,
                        Name = file.Name,
                        Size = file.Length,
                        CreateDate = file.CreationTime,
                        UpdateDate = file.LastWriteTime
                    };

                    files.Add(fielModel);
                }
            }
            catch
            {

            }
            return files;
        }

        #region Kiểm tra thư mục upload

        /// <summary>
        /// Kiểm tra thư mục upload
        /// </summary>
        /// <param name="typeFunctionDefinitionId"></param>
        /// <param name="pathFolder"></param>
        /// <param name="productCode"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public CheckFolderResult CheckFolderUpload(int typeFunctionDefinitionId, string pathFolder, string productCode, string groupCode, string parentGroupCode,
            List<CheckUploadEntity> listError, List<FolderDefinitionModel> folderDefinitions, List<FileDefinitionModel> fileDefinitions,
            List<MaterialFromDBModel> materials, List<RawMaterialModel> rawMaterials, List<ManufactureResultModel> manufacturers, List<MaterialGroupFromDBModel> materialGroups,
            List<UnitModel> units, List<CodeRuleModel> codeRules, List<string> lstError, List<string> ListFolder, bool hasCheckListMaterial, out string materialPath, List<DataCheckModuleModel> modules,
            List<string> pathErrors)
        {
            CheckFolderResult result = new CheckFolderResult();

            result.Status = true;
            materialPath = string.Empty;
            FolderDefinitionModel folderUpload = folderDefinitions.FirstOrDefault(r => r.FolderType == Constants.FolderDefinition_FolderType_Upload && typeFunctionDefinitionId.Equals(r.TypeDefinitionId));

            if (folderUpload == null)
            {
                lstError.Add("Chưa có thư mục định nghĩa upload");
                result.Status = false;
                return result;
            }

            string folderName = Path.GetFileName(pathFolder);
            string folderNameTree = folderName;
            string folderDefinitionFirst = string.Empty;
            string folderDefinitionLast = string.Empty;

            if (!string.IsNullOrEmpty(folderUpload.FolderDefinitionLast))
            {
                folderDefinitionLast = folderUpload.FolderDefinitionLast;
            }

            if (!string.IsNullOrEmpty(folderUpload.FolderDefinitionFirst))
            {
                folderDefinitionFirst = folderUpload.FolderDefinitionFirst;
            }

            // Trường hơp không có ký tự ở giữa
            if (folderUpload.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.None)
            {
                if (folderUpload.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length, folderDefinitionLast.Length, (int)folderUpload.FolderDefinitionBetweenIndex, folderName);
                }

                if (string.IsNullOrEmpty(folderName))
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    result.Status = false;
                    return result;
                }
                if (folderName != (folderDefinitionFirst + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    result.Status = false;
                    return result;
                }
            }
            // Trường hợp ký tự ở giữa là mã thiết bị
            else if (folderUpload.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ProductCode)
            {
                if (folderUpload.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length + productCode.Length, folderDefinitionLast.Length, (int)folderUpload.FolderDefinitionBetweenIndex, folderName);
                }
                if (string.IsNullOrEmpty(folderName))
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    //MessageBox.Show(Configuration.GetResource(MessageList.MSG101, new string[] { "Tên thư mục " + pathFolder + " không đúng với định nghĩa", "upload thư mục" }), Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Status = false;
                    return result;
                }
                if (folderName != (folderDefinitionFirst + productCode + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    //MessageBox.Show(Configuration.GetResource(MessageList.MSG101, new string[] { "Tên thư mục " + pathFolder + " không đúng với định nghĩa", "upload thư mục" }), Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Status = false;
                    return result;
                }
            }

            // Trường hợp ký tự ở giữa là mã nhóm
            else if (folderUpload.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.GroupCode)
            {
                if (folderUpload.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length + groupCode.Length, folderDefinitionLast.Length, (int)folderUpload.FolderDefinitionBetweenIndex, folderName);
                }
                if (string.IsNullOrEmpty(folderName))
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    //MessageBox.Show(Configuration.GetResource(MessageList.MSG101, new string[] { "Tên thư mục " + pathFolder + " không đúng với định nghĩa", "upload thư mục" }), Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Status = false;
                    return result;
                }
                if (folderName != (folderDefinitionFirst + groupCode + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    //MessageBox.Show(Configuration.GetResource(MessageList.MSG101, new string[] { "Tên thư mục " + pathFolder + " không đúng với định nghĩa", "upload thư mục" }), Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    result.Status = false;
                    return result;
                }
            }
            else if (folderUpload.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ParentGroupCode)
            {
                if (folderUpload.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length + parentGroupCode.Length, folderDefinitionLast.Length, (int)folderUpload.FolderDefinitionBetweenIndex, folderName);
                }


                if (string.IsNullOrEmpty(folderName) || folderName != (folderDefinitionFirst + parentGroupCode + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    result.Status = false;
                    return result;
                }
            }
            else if (folderUpload.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ModuleCode)
            {
                if (string.IsNullOrEmpty(folderName) || modules.FirstOrDefault(r => r.Code.ToUpper().Equals(folderName.ToUpper())) == null)
                {
                    lstError.Add("Tên thư mục" + pathFolder + " không đúng với định nghĩa");
                    pathErrors.Add(pathFolder);
                    result.Status = false;
                    return result;
                }
            }

            // Trường hợp có thư mục cha
            if (!string.IsNullOrEmpty(folderUpload.FolderDefinitionManageId))
            {
                if (!CheckFolderParent(folderDefinitions, Path.GetDirectoryName(pathFolder), folderUpload.FolderDefinitionManageId, productCode, groupCode, parentGroupCode, lstError))
                {
                    result.Status = false;
                }
            }

            // Kiểm tra file
            if (!Convert.ToBoolean(folderUpload.StatusCheckFile))
            {
                if (!CheckFile(pathFolder, folderUpload, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units, fileDefinitions, lstError, hasCheckListMaterial, pathErrors))
                {
                    result.Status = false;
                }
            }
            else
            {
                DirectoryInfo drc = new DirectoryInfo(pathFolder);
                foreach (FileInfo fileInfo in drc.GetFiles())
                {
                    if (fileInfo.Length == 0)
                    {
                        lstError.Add("Tài liệu " + fileInfo.FullName + " độ lớn = 0");
                        pathErrors.Add(fileInfo.FullName);
                        //MessageBox.Show(Configuration.GetResource(MessageList.MSG101, new string[] { "Tài liệu " + fileInfo.FullName + " độ lớn = 0", "upload thư mục" }), Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result.Status = false;
                    }
                }
            }
            if (!Convert.ToBoolean(folderUpload.StatusCheckFolder))
            {
                // Kiểm tra thư mục con
                if (!CheckFolderChildren(pathFolder, folderUpload.FolderDefinitionId, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units,
                    folderDefinitions, fileDefinitions, lstError, ListFolder, hasCheckListMaterial, modules, pathErrors))
                {
                    result.Status = false;
                }
            }
            else
            {
                if (!GetTotalFile(pathFolder, lstError))
                {
                    result.Status = false;
                }
            }

            //// CheckFileUpload
            //if ((StatusDataTable.FileDesignType)typeFunctionDefinitionId == StatusDataTable.FileDesignType.Mechanical
            //    && !CheckFilePartList(typeFunctionDefinitionId, pathFolder, productCode, groupCode, parentGroupCode, listError, folderDefinitions, fileDefinitions, lstError, out materialPath))
            //{
            //    result.Status = false;
            //    return result;
            //}

            // CheckFileUpload
            //if ((StatusDataTable.FileDesignType)typeFunctionDefinitionId == StatusDataTable.FileDesignType.Electron
            //    && !CheckFilePartList(typeFunctionDefinitionId, pathFolder, productCode, groupId, groupCode, listError, folderDefinitions, fileDefinitions, lstError, out materialPath))
            //{
            //    result.Status = false;
            //    return result;
            //}

            return result;
        }

        /// <summary>
        /// Kiểm tra folder theo định nghĩa
        /// </summary>
        /// <param name="pathFolder"></param>
        /// <param name="folderDefinitionId"></param>
        /// <param name="productCode"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        private bool CheckFile(string pathFolder, FolderDefinitionModel folderDefinition, string productCode, string groupCode, string parentGroupCode, List<MaterialFromDBModel> materials,
            List<RawMaterialModel> rawMaterials, List<ManufactureResultModel> manufacturers, List<UnitModel> units, List<FileDefinitionModel> fileDefinitions,
            List<string> lstError, bool hasCheckListMaterial, List<string> pathErrors)
        {
            string folderName = Path.GetFileName(pathFolder);

            string extension = string.Empty;
            if (!string.IsNullOrEmpty(folderDefinition.ExtensionFile))
            {
                extension = folderDefinition.ExtensionFile;
            }

            // Lấy danh sách file định nghĩa theo thư mục
            List<FileDefinitionModel> listFileDefinitionModel = fileDefinitions.Where(r => r.FolderDefinitionId.Equals(folderDefinition.FolderDefinitionId)).ToList();

            // Lấy danh sach file trong thư mục 
            string[] files = Directory.GetFiles(pathFolder);
            DirectoryInfo drc = new DirectoryInfo(pathFolder);
            int fileTotal = 0;
            bool isCheckOk = true;
            foreach (FileInfo fileInfo in drc.GetFiles())
            {
                if (fileInfo.Length == 0)
                {
                    lstError.Add("Tài liệu " + fileInfo.Name + " size = 0");
                    pathErrors.Add(fileInfo.FullName);
                    isCheckOk = false;
                }

                if (fileInfo.FullName.Length > 230)
                {
                    lstError.Add("Đường dẫn tài liệu không được lớn hơn 230 ký tự \n" + fileInfo.Name);
                    pathErrors.Add(fileInfo.FullName);
                    isCheckOk = false;
                }

                if (string.IsNullOrEmpty(extension) || fileInfo.Extension.ToLower().Equals(extension.ToLower()))
                {
                    fileTotal++;
                }
            }

            // Số lượng tài liệu không đủ theo định nghĩa
            int fileDifinitionNotIndex = 0;
            foreach (var item in listFileDefinitionModel)
            {
                if (item.FileDefinitionNameBetweenIndex == (int)StatusDataTable.TypeFolderIndex.None)
                {
                    fileDifinitionNotIndex++;
                }
            }

            //if (fileTotal < fileDifinitionNotIndex)
            //{
            //    lstError.Add($"Số lượng file của thư mục {folderName} không đủ");
            //    return false;
            //}

            string fileName = string.Empty;
            string fileNameCheck = string.Empty;
            string folderDefinitionFirst = string.Empty;
            string folderDefinitionLast = string.Empty;
            bool check = false;
            string fileCheck;
            string productCodeF = string.Empty;
            int countFileDefinition = 0;
            int countDefinitionExist = 0;
            bool isExtension = false;
            // Duyệt tài liệu
            foreach (string name in files)
            {
                fileName = GetLowerExtension(Path.GetFileName(name));
                check = true;
                countFileDefinition = 0;
                isExtension = false;

                // Duyệt tài liệu định nghĩa
                foreach (var item in listFileDefinitionModel)
                {
                    fileNameCheck = fileName;
                    fileCheck = string.Empty;
                    folderDefinitionFirst = string.Empty;
                    folderDefinitionLast = string.Empty;

                    if (!string.IsNullOrEmpty(item.FileDefinitionNameFirst))
                    {
                        folderDefinitionFirst = item.FileDefinitionNameFirst;
                    }

                    if (!string.IsNullOrEmpty(item.FileDefinitionNameLast))
                    {
                        folderDefinitionLast = item.FileDefinitionNameLast;
                    }

                    // Trường hợp ký tự giữa trống
                    if (item.FileDefinitionNameBetween == (int)StatusDataTable.TypeFolderName.None)
                    {
                        if (item.FileDefinitionNameBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length, folderDefinitionLast.Length, (int)item.FileDefinitionNameBetweenIndex, fileNameCheck);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                fileNameCheck = fileCheck;
                            }
                        }

                        if (fileNameCheck == GetLowerExtension((folderDefinitionFirst + folderDefinitionLast)))
                        {
                            if (item.FileDefinitionNameBetweenIndex == (int)StatusDataTable.TypeFolderIndex.None)
                            {
                                countDefinitionExist++;
                            }

                            if (item.FileType == Constants.FileDefinition_FileType_ListMaterial)
                            {
                                if (hasCheckListMaterial)
                                {
                                    if (!CheckPartList(name, productCode, materials, rawMaterials, manufacturers, units, lstError))
                                    {
                                        check = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            countFileDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa là mã thiết bị
                    else if (item.FileDefinitionNameBetween == (int)StatusDataTable.TypeFolderName.ProductCode)
                    {
                        if (item.FileDefinitionNameBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length + productCode.Length, folderDefinitionLast.Length, (int)item.FileDefinitionNameBetweenIndex, fileNameCheck);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                fileNameCheck = fileCheck;
                            }
                        }

                        productCodeF = productCode;
                        if (folderDefinitionFirst == Constants.CheckElectron)
                        {
                            productCodeF = productCode.Substring(folderDefinitionFirst.Length - 1, productCode.Length - folderDefinitionFirst.Length + 1);
                        }

                        if (fileNameCheck == GetLowerExtension((folderDefinitionFirst + productCodeF + folderDefinitionLast)))
                        {
                            if (item.FileDefinitionNameBetweenIndex == (int)StatusDataTable.TypeFolderIndex.None)
                            {
                                countDefinitionExist++;
                            }

                            if (item.FileType == Constants.FileDefinition_FileType_ListMaterial)
                            {
                                if (hasCheckListMaterial)
                                {
                                    if (!CheckPartList(name, productCode, materials, rawMaterials, manufacturers, units, lstError))
                                    {
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countFileDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa là mã nhóm
                    else if (item.FileDefinitionNameBetween == (int)StatusDataTable.TypeFolderName.GroupCode)
                    {
                        if (item.FileDefinitionNameBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length + groupCode.Length, folderDefinitionLast.Length, (int)item.FileDefinitionNameBetweenIndex, fileNameCheck);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                fileNameCheck = fileCheck;
                            }
                        }

                        if (fileNameCheck == GetLowerExtension((folderDefinitionFirst + groupCode + folderDefinitionLast)))
                        {
                            if (item.FileDefinitionNameBetweenIndex == (int)StatusDataTable.TypeFolderIndex.None)
                            {
                                countDefinitionExist++;
                            }

                            if (item.FileType == Constants.FileDefinition_FileType_ListMaterial)
                            {
                                if (hasCheckListMaterial)
                                {
                                    if (!CheckPartList(name, productCode, materials, rawMaterials, manufacturers, units, lstError))
                                    {
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countFileDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa là mã nhóm
                    else if (item.FileDefinitionNameBetween == (int)StatusDataTable.TypeFolderName.ParentGroupCode)
                    {
                        if (item.FileDefinitionNameBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length + parentGroupCode.Length, folderDefinitionLast.Length, (int)item.FileDefinitionNameBetweenIndex, fileNameCheck);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                fileNameCheck = fileCheck;
                            }
                        }

                        if (fileNameCheck == GetLowerExtension((folderDefinitionFirst + parentGroupCode + folderDefinitionLast)))
                        {
                            if (item.FileDefinitionNameBetweenIndex == (int)StatusDataTable.TypeFolderIndex.None)
                            {
                                countDefinitionExist++;
                            }

                            if (item.FileType == Constants.FileDefinition_FileType_ListMaterial)
                            {
                                if (hasCheckListMaterial)
                                {
                                    if (!CheckPartList(name, productCode, materials, rawMaterials, manufacturers, units, lstError))
                                    {
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countFileDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa ( ) là file DocPcb
                    else if (item.FileDefinitionNameBetween == (int)StatusDataTable.TypeFolderName.ExceptPCB)
                    {
                        var code = productCode.ToUpper().Replace("PCB.", "");
                        if (item.FileDefinitionNameBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length + code.Length, folderDefinitionLast.Length, (int)item.FileDefinitionNameBetweenIndex, fileNameCheck);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                fileNameCheck = fileCheck;
                            }
                        }

                        if (fileNameCheck != GetLowerExtension((folderDefinitionFirst + code + folderDefinitionLast)))
                        {
                            countFileDefinition++;
                        }
                        else
                        {
                            if (item.FileDefinitionNameBetweenIndex == (int)StatusDataTable.TypeFolderIndex.None)
                            {
                                countDefinitionExist++;
                            }
                        }
                    }
                }

                // Trường hợp không đúng định nghĩa
                if (countFileDefinition == listFileDefinitionModel.Count)
                {
                    if (string.IsNullOrEmpty(extension) || !Path.GetExtension(name).ToLower().Equals(extension.ToLower()))
                    {
                        lstError.Add("File " + name + " không đúng với định nghĩa");
                        pathErrors.Add(name);
                        isCheckOk = false;
                        isExtension = true;
                    }
                }
                else if (!check)
                {
                    lstError.Add("File " + name + " không đúng với định nghĩa");
                    pathErrors.Add(name);
                    isCheckOk = false;
                }
            }

            if (countDefinitionExist != fileDifinitionNotIndex)
            {
                lstError.Add($"Số lượng file của thư mục {folderName} không đủ");
                isCheckOk = false;
            }

            return isCheckOk;
        }

        public string CheckBetweenIndex(int lengthFist, int lengthLast, int betWeenIndex, string fileName)
        {
            if ((lengthFist + lengthLast + 3) != fileName.Length)
            {
                return string.Empty;
            }
            else
            {
                string first = fileName.Substring(0, lengthFist);
                string last = fileName.Substring(lengthFist + 3, lengthLast);
                string between = fileName.Substring(0, lengthFist + 3).Substring(lengthFist + 1, 2);
                string character = fileName.Substring(0, lengthFist + 3).Substring(lengthFist, 1);

                if (character != ".")
                {
                    return string.Empty;
                }
                if (betWeenIndex == (int)StatusDataTable.TypeFolderIndex.Number)
                {
                    try
                    {
                        Convert.ToInt32(between);
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    Regex myRegex = new Regex("^[A-Za-z_,)?'!(&/:`;-]*[A-Za-z][A-Za-z_.)]*$");

                    if (!myRegex.IsMatch(between))
                    {
                        return string.Empty;
                    }
                }
                return first + last;
            }

        }

        /// <summary>
        /// Kiểm tra thư mục cha
        /// </summary>
        /// <param name="pathFolder"></param>
        /// <param name="folderDefinitionId"></param>
        /// <param name="productCode"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        private bool CheckFolderParent(List<FolderDefinitionModel> folderDefinitions, string pathFolder, string folderDefinitionId, string productCode, string groupCode, string parentGroupCode, List<string> lstError)
        {
            FolderDefinitionModel folderDefinitionModel = folderDefinitions.FirstOrDefault(r => r.FolderDefinitionId.Equals(folderDefinitionId));

            if (folderDefinitionModel == null)
            {
                lstError.Add("Thư mục cấu hình không tồn tại!");
                return false;
            }

            string folderName = Path.GetFileName(pathFolder);
            string folderNameTree = folderName;
            string folderDefinitionFirst = string.Empty;
            string folderDefinitionLast = string.Empty;

            if (!string.IsNullOrEmpty(folderDefinitionModel.FolderDefinitionLast))
            {
                folderDefinitionLast = folderDefinitionModel.FolderDefinitionLast;
            }

            if (!string.IsNullOrEmpty(folderDefinitionModel.FolderDefinitionFirst))
            {
                folderDefinitionFirst = folderDefinitionModel.FolderDefinitionFirst;
            }

            // Trường hợpký giữa giữa trống
            if (folderDefinitionModel.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.None)
            {
                if (folderDefinitionModel.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length, folderDefinitionLast.Length, (int)folderDefinitionModel.FolderDefinitionBetweenIndex, folderName);
                }
                if (string.IsNullOrEmpty(folderName))
                {
                    lstError.Add("Tên thư mục " + folderNameTree + " không đúng với định nghĩa");
                    return false;
                }
                if (folderName != (folderDefinitionFirst + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục " + folderNameTree + " không đúng với định nghĩa");
                    return false;
                }
            }
            // Trường hợp ký tự giữa là mã thiết bị
            else if (folderDefinitionModel.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ProductCode)
            {
                if (folderDefinitionModel.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length + productCode.Length, folderDefinitionLast.Length, (int)folderDefinitionModel.FolderDefinitionBetweenIndex, folderName);
                }
                if (string.IsNullOrEmpty(folderName))
                {
                    lstError.Add("Tên thư mục " + folderNameTree + " không đúng với định nghĩa");
                    return false;
                }
                if (folderName != (folderDefinitionFirst + productCode + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục " + folderNameTree + " không đúng với định nghĩa");
                    return false;
                }
            }
            // trường hợp ký tự giữa là mã nhóm
            else if (folderDefinitionModel.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.GroupCode)
            {
                if (folderDefinitionModel.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length + groupCode.Length, folderDefinitionLast.Length, (int)folderDefinitionModel.FolderDefinitionBetweenIndex, folderName);
                }
                if (string.IsNullOrEmpty(folderName))
                {
                    lstError.Add("Tên thư mục " + folderNameTree + " không đúng với định nghĩa");
                    return false;
                }
                if (folderName != (folderDefinitionFirst + groupCode + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục " + folderNameTree + " không đúng với định nghĩa");
                    return false;
                }
            }
            else if (folderDefinitionModel.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ParentGroupCode)
            {
                if (folderDefinitionModel.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                {
                    folderName = CheckBetweenIndex(folderDefinitionFirst.Length + parentGroupCode.Length, folderDefinitionLast.Length, (int)folderDefinitionModel.FolderDefinitionBetweenIndex, folderName);
                }

                if (string.IsNullOrEmpty(folderName) || folderName != (folderDefinitionFirst + parentGroupCode + folderDefinitionLast))
                {
                    lstError.Add("Tên thư mục " + folderNameTree + " không đúng với định nghĩa");
                    return false;
                }
            }

            // Kiểm tra thư mục cha
            if (!string.IsNullOrEmpty(folderDefinitionModel.FolderDefinitionManageId))
            {
                if (!CheckFolderParent(folderDefinitions, Path.GetDirectoryName(pathFolder), folderDefinitionModel.FolderDefinitionManageId, productCode, groupCode, parentGroupCode, lstError))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Kiểm tra thư mục con
        /// </summary>
        /// <param name="pathFolder"></param>
        /// <param name="folderDefinitionId"></param>
        /// <param name="productCode"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        private bool CheckFolderChildren(string pathFolder, string folderDefinitionId, string productCode, string groupCode, string parentGroupCode, List<MaterialFromDBModel> materials,
            List<RawMaterialModel> rawMaterials, List<ManufactureResultModel> manufacturers, List<UnitModel> units, List<FolderDefinitionModel> folderDefinitions,
            List<FileDefinitionModel> fileDefinitions, List<string> lstError, List<string> ListFolder, bool hasCheckListMaterial, List<DataCheckModuleModel> modules,
            List<string> pathErrors)
        {
            var folderDefinitionModel = folderDefinitions.Where(f => folderDefinitionId.Equals(f.FolderDefinitionManageId)).ToList();

            List<string> folderChildrentName = new List<string>();
            // Lấy danh sach thư mục
            foreach (DirectoryInfo SubFolder in new DirectoryInfo(pathFolder).GetDirectories())
            {
                //Bỏ qua các thư mục hệ thống
                if (SubFolder.Attributes.ToString().Contains("System")) continue;
                folderChildrentName.Add(SubFolder.FullName);
                if ("PRD".Equals(SubFolder.Name.Substring(0, 3)) || "PRJ".Equals(SubFolder.Name.Substring(0, 3)))
                {
                    ListFolder.Add(SubFolder.FullName);
                }
            }

            int countFolderDifinitionNotIndex = folderDefinitionModel.Count(r => r.FolderDefinitionBetweenIndex == (int)StatusDataTable.TypeFolderIndex.None);
            int countFolderDifinitionIndex = folderDefinitionModel.Count(r => r.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None);


            // Trường hợp ít thư mục so với định nghĩa
            if (folderChildrentName.Count < countFolderDifinitionNotIndex)
            {
                lstError.Add("Số lượng thư mục của thư mục " + pathFolder + " không đủ");
                //MessageBox.Show(Configuration.GetResource(MessageList.MSG101, new string[] { "Số lượng thư mục của thư mục " + pathFolder + " không đủ", "upload thư mục" }), Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //else if (folderChildrentName.Count > folderDifinitionNotIndex)
            //{
            //    lstError.Add("Số lượng thư mục của thư mục " + pathFolder + " nhiều hơn định nghĩa!");
            //    return false;
            //}
            else if (folderChildrentName.Count == 0)
            {
                return true;
            }

            string file = string.Empty;
            string fileTree = string.Empty;
            string folderDefinitionFirst;
            string folderDefinitionLast;
            bool check = false;
            string fileCheck = string.Empty;
            int countNotDefinition = 0;
            // Duyệt danh sách thư mục con
            bool isCheckOK = true;
            foreach (string name in folderChildrentName)
            {
                file = Path.GetFileName(name);
                fileTree = file;
                check = true;
                countNotDefinition = 0;
                // Duyệt danh sách thư mục định nghĩa
                foreach (var row in folderDefinitionModel)
                {
                    fileCheck = string.Empty;
                    folderDefinitionFirst = string.Empty;
                    folderDefinitionLast = string.Empty;

                    if (!string.IsNullOrEmpty(row.FolderDefinitionLast))
                    {
                        folderDefinitionLast = row.FolderDefinitionLast;
                    }

                    if (!string.IsNullOrEmpty(row.FolderDefinitionFirst))
                    {
                        folderDefinitionFirst = row.FolderDefinitionFirst;
                    }

                    // Trường hợp ký tự giữa trống
                    if (row.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.None)
                    {
                        if (row.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length, folderDefinitionLast.Length, (int)row.FolderDefinitionBetweenIndex, file);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                file = fileCheck;
                            }
                        }

                        if (file == (folderDefinitionFirst + folderDefinitionLast))
                        {
                            if (!Convert.ToBoolean(row.StatusCheckFolder))
                            {
                                // Kiểm tra thư mục con
                                if (!CheckFolderChildren(name, row.FolderDefinitionId, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units,
                                    folderDefinitions, fileDefinitions, lstError, ListFolder, hasCheckListMaterial, modules, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                if (!GetTotalFile(name, lstError))
                                {
                                    check = false;
                                }

                            }
                            if (!Convert.ToBoolean(row.StatusCheckFile))
                            {
                                // Kiểm tra file thư mục hiện tại
                                if (!CheckFile(name, row, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units, fileDefinitions, lstError, hasCheckListMaterial, pathErrors))
                                {
                                    check = false;
                                }

                            }
                            else
                            {
                                DirectoryInfo drc = new DirectoryInfo(name);
                                foreach (FileInfo fileInfo in drc.GetFiles())
                                {
                                    if (fileInfo.Length == 0)
                                    {
                                        lstError.Add("Tài liệu " + fileInfo.FullName + " độ lớn = 0");
                                        pathErrors.Add(fileInfo.FullName);
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countNotDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa là mã thiêt bị
                    else if (row.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ProductCode)
                    {
                        if (row.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length + productCode.Length, folderDefinitionLast.Length, (int)row.FolderDefinitionBetweenIndex, file);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                file = fileCheck;
                            }
                        }

                        if (file == (folderDefinitionFirst + productCode + folderDefinitionLast))
                        {
                            if (!Convert.ToBoolean(row.StatusCheckFolder))
                            {
                                // Kiểm tra thư mục con
                                if (!CheckFolderChildren(name, row.FolderDefinitionId, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units,
                                    folderDefinitions, fileDefinitions, lstError, ListFolder, hasCheckListMaterial, modules, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                if (!GetTotalFile(name, lstError))
                                {
                                    check = false;
                                }

                            }
                            if (!Convert.ToBoolean(row.StatusCheckFile))
                            {
                                // Kiểm tra file thư mục hiện tại
                                if (!CheckFile(name, row, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units, fileDefinitions, lstError, hasCheckListMaterial, pathErrors))
                                {
                                    check = false;
                                }

                            }
                            else
                            {
                                DirectoryInfo drc = new DirectoryInfo(name);
                                foreach (FileInfo fileInfo in drc.GetFiles())
                                {
                                    if (fileInfo.Length == 0)
                                    {
                                        lstError.Add("Tài liệu " + fileInfo.FullName + " độ lớn = 0");
                                        pathErrors.Add(fileInfo.FullName);
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countNotDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa là mã nhóm
                    else if (row.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.GroupCode)
                    {
                        if (row.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length + groupCode.Length, folderDefinitionLast.Length, (int)row.FolderDefinitionBetweenIndex, file);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                file = fileCheck;
                            }
                        }

                        if (file == (folderDefinitionFirst + groupCode + folderDefinitionLast))
                        {
                            if (!Convert.ToBoolean(row.StatusCheckFolder))
                            {
                                // Kiểm tra thư mục con
                                if (!CheckFolderChildren(name, row.FolderDefinitionId, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units,
                                    folderDefinitions, fileDefinitions, lstError, ListFolder, hasCheckListMaterial, modules, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                if (!GetTotalFile(name, lstError))
                                {
                                    check = false;
                                }
                            }
                            if (!Convert.ToBoolean(row.StatusCheckFile))
                            {
                                // Kiểm tra file thư mục hiện tại
                                if (!CheckFile(name, row, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units, fileDefinitions, lstError, hasCheckListMaterial, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                DirectoryInfo drc = new DirectoryInfo(name);
                                foreach (FileInfo fileInfo in drc.GetFiles())
                                {
                                    if (fileInfo.Length == 0)
                                    {
                                        lstError.Add("Tài liệu " + fileInfo.FullName + " độ lớn = 0");
                                        pathErrors.Add(fileInfo.FullName);
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countNotDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa là mã nhóm cha
                    else if (row.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ParentGroupCode)
                    {
                        if (row.FolderDefinitionBetweenIndex != (int)StatusDataTable.TypeFolderIndex.None)
                        {
                            fileCheck = CheckBetweenIndex(folderDefinitionFirst.Length + parentGroupCode.Length, folderDefinitionLast.Length, (int)row.FolderDefinitionBetweenIndex, file);

                            if (!string.IsNullOrEmpty(fileCheck))
                            {
                                file = fileCheck;
                            }
                        }

                        if (file == (folderDefinitionFirst + parentGroupCode + folderDefinitionLast))
                        {
                            if (!Convert.ToBoolean(row.StatusCheckFolder))
                            {
                                // Kiểm tra thư mục con
                                if (!CheckFolderChildren(name, row.FolderDefinitionId, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units,
                                    folderDefinitions, fileDefinitions, lstError, ListFolder, hasCheckListMaterial, modules, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                if (!GetTotalFile(name, lstError))
                                {
                                    check = false;
                                }
                            }
                            if (!Convert.ToBoolean(row.StatusCheckFile))
                            {
                                // Kiểm tra file thư mục hiện tại
                                if (!CheckFile(name, row, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units, fileDefinitions, lstError, hasCheckListMaterial, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                DirectoryInfo drc = new DirectoryInfo(name);
                                foreach (FileInfo fileInfo in drc.GetFiles())
                                {
                                    if (fileInfo.Length == 0)
                                    {
                                        lstError.Add("Tài liệu " + fileInfo.FullName + " độ lớn = 0");
                                        pathErrors.Add(fileInfo.FullName);
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countNotDefinition++;
                        }
                    }
                    // Trường hợp ký tự giữa là mã module nguồn
                    else if (row.FolderDefinitionBetween == (int)StatusDataTable.TypeFolderName.ModuleCode)
                    {
                        if (modules.FirstOrDefault(r => r.Code.ToUpper().Equals(file.ToUpper())) != null)
                        {
                            if (!Convert.ToBoolean(row.StatusCheckFolder))
                            {
                                // Kiểm tra thư mục con
                                if (!CheckFolderChildren(name, row.FolderDefinitionId, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units,
                                    folderDefinitions, fileDefinitions, lstError, ListFolder, hasCheckListMaterial, modules, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                if (!GetTotalFile(name, lstError))
                                {
                                    check = false;
                                }
                            }
                            if (!Convert.ToBoolean(row.StatusCheckFile))
                            {
                                // Kiểm tra file thư mục hiện tại
                                if (!CheckFile(name, row, productCode, groupCode, parentGroupCode, materials, rawMaterials, manufacturers, units, fileDefinitions, lstError, hasCheckListMaterial, pathErrors))
                                {
                                    check = false;
                                }
                            }
                            else
                            {
                                DirectoryInfo drc = new DirectoryInfo(name);
                                foreach (FileInfo fileInfo in drc.GetFiles())
                                {
                                    if (fileInfo.Length == 0)
                                    {
                                        lstError.Add("Tài liệu " + fileInfo.FullName + " độ lớn = 0");
                                        pathErrors.Add(fileInfo.FullName);
                                        check = false;
                                    }
                                }

                            }
                        }
                        else
                        {
                            countNotDefinition++;
                        }
                    }
                }

                // Trường hợp không đúng định nghĩa
                if (!check || countNotDefinition == folderDefinitionModel.Count)
                {
                    lstError.Add("Thư mục " + name + " không đúng với định nghĩa");
                    isCheckOK = false;
                }

                if (countNotDefinition == folderDefinitionModel.Count)
                {
                    pathErrors.Add(name);
                }
            }

            return isCheckOK;
        }

        private bool GetTotalFile(string pathFolder, List<string> lstError)
        {
            DirectoryInfo drc = new DirectoryInfo(pathFolder);
            foreach (DirectoryInfo info in drc.GetDirectories())
            {
                foreach (FileInfo fileInfo in info.GetFiles())
                {
                    if (fileInfo.Length == 0)
                    {
                        lstError.Add("Tài liệu " + fileInfo.FullName + " độ lớn = 0");
                        return false;
                    }

                    if (fileInfo.FullName.Length > 230)
                    {
                        lstError.Add("Đường dẫn tài liệu không được lớn hơn 230 ký tự");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckPartList(string pathFile, string moduleCode, List<MaterialFromDBModel> materials, List<RawMaterialModel> rawMaterials, List<ManufactureResultModel> manufacturers,
            List<UnitModel> units, List<string> lstError)
        {
            // Bảng chưa thông tin lấy từ file excel
            DataTable dataTable = new DataTable();

            // Đọc file excel
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.OpenReadOnly(pathFile);
            IWorksheet sheet = workbook.Worksheets[0];

            dataTable = sheet.ExportDataTable(sheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);

            return CheckFileExcel(dataTable, moduleCode, materials, rawMaterials, manufacturers, units, lstError);
        }

        private string GetLowerExtension(string extension)
        {
            string[] ex = extension.Split('.');
            string name = string.Empty;
            if (ex.Length > 0)
            {
                for (int i = 0; i < ex.Length; i++)
                {
                    if (i == ex.Length - 1)
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            name = name + "." + ex[i].ToLower();
                        }
                        else
                        {
                            name = ex[i].ToLower();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            name = name + "." + ex[i];
                        }
                        else
                        {
                            name = ex[i];
                        }
                    }
                }
            }
            else
            {
                return extension;
            }
            return name;
        }

        /// <summary>
        /// Kiểm tra file excell
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool CheckFileExcel(DataTable dt, string moduleCode, List<MaterialFromDBModel> materials, List<RawMaterialModel> rawMaterials,
           List<ManufactureResultModel> manufacturers, List<UnitModel> units, List<string> lstError)
        {
            bool isOK = true;
            try
            {
                string index = string.Empty;
                bool check = false;
                string listPartError = string.Empty;
                string listPartManuError = string.Empty;
                string listManuError = string.Empty;
                string code = dt.Rows[1][9].ToString().Trim().Substring(dt.Rows[1][9].ToString().Trim().LastIndexOf(" ") + 1, dt.Rows[1][9].ToString().Trim().Length - dt.Rows[1][9].ToString().Trim().LastIndexOf(" ") - 1);

                if (moduleCode != code)
                {
                    lstError.Add("Mã module của danh mục vật tư không đúng với module hiện tại.\nBạn hãy kiểm tra lại");
                    //MessageBox.Show("Mã module của danh mục vật tư không đúng với module hiện tại.\nBạn hãy kiểm tra lại", Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string message = string.Empty;
                //Kiểm tra tên vật tư
                if (!CheckItem.CheckNullOrMaxLength(code, 50, "Danh mục vật tư \nMã thiết bị ", out message))
                {
                    lstError.Add(message);
                    return false;
                }

                // Id hãng sản xuất
                string manuId = string.Empty;
                MaterialFromDBModel material;
                ManufactureResultModel manufacture;
                for (int i = 5; i < dt.Rows.Count; i++)
                {
                    // Nếu dòng trắng
                    //if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                    //{
                    check = false;
                    if (!string.IsNullOrEmpty(index))
                    {
                        if (dt.Rows[i][0].ToString().Length >= (index.Length + 1))
                        {
                            if (dt.Rows[i][0].ToString().Substring(0, index.Length + 1) == (index + "."))
                            {
                                check = true;
                            }
                        }
                    }
                    if (!check)
                    {
                        manuId = string.Empty;

                        if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i][1].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][3].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][5].ToString()) && !string.IsNullOrEmpty(dt.Rows[i][9].ToString()))
                            {
                                lstError.Add("Dòng " + (i + 2) + " STT không được để trống");
                                isOK = false;
                            }
                            else
                            {
                                break;
                            }
                        }


                        // Kiểm tra tên vật tư
                        if (!CheckItem.CheckNullOrMaxLength(dt.Rows[i][1].ToString(), 200, " Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " tên vật tư", out message))
                        {
                            lstError.Add(message);
                            isOK = false;
                        }

                        // Kiểm tra mã vật tư
                        if (!CheckItem.CheckNullOrMaxLength(dt.Rows[i][3].ToString(), 100, "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " mã vật tư", out message))
                        {
                            lstError.Add(message);
                            isOK = false;
                        }

                        // Kiểm tra hãng sản xuất
                        if (!CheckItem.CheckNullOrMaxLength(dt.Rows[i][9].ToString(), 100, "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " hãng sản xuất", out message))
                        {
                            lstError.Add(message);
                            isOK = false;
                        }

                        // Kiểm tra đơn vị
                        if (!CheckItem.CheckNullOrMaxLength(dt.Rows[i][5].ToString(), 45, "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " đơn vị", out message))
                        {
                            lstError.Add(message);
                            isOK = false;
                        }

                        if (!CheckUnit(units, dt.Rows[i][5].ToString()))
                        {
                            lstError.Add("Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " đơn  vị tính không hợp lệ");
                            isOK = false;
                        }

                        // Kiểm tra số lượng
                        if (!CheckItem.CheckNullOrMaxLength(dt.Rows[i][6].ToString(), 9, "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " số lượng", out message))
                        {
                            lstError.Add(message);
                            isOK = false;
                        }

                        // Kiểm tra số lượng
                        if (!CheckItem.CheckIsNumber(dt.Rows[i][6].ToString(), "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " số lượng", Constants.NumberFormatType.DECIMAL, out message))
                        {
                            lstError.Add(message);
                            isOK = false;
                        }
                        else if (Convert.ToDecimal(dt.Rows[i][6].ToString()) <= 0)
                        {
                            lstError.Add("Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " số lượng");
                            isOK = false;
                        }

                        // Kiểm tra tên vật tư
                        if (!CheckItem.CheckMaxLength(dt.Rows[i][7].ToString(), 100, "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " vật liệu", out message))
                        {
                            lstError.Add(message);
                            isOK = false;
                        }

                        if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()) && CheckMaterialsModel(rawMaterials, dt.Rows[i][7].ToString())
                         && !string.IsNullOrEmpty(dt.Rows[i][8].ToString()) && dt.Rows[i][2].ToString() == Constants.TPA)
                        {
                            // Kiểm tra mã vật tư
                            if (!CheckItem.CheckNullOrMaxLength(dt.Rows[i][4].ToString(), 100, "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " mã vật liệu", out message))
                            {
                                lstError.Add(message);
                                isOK = false;
                            }

                            material = materials.FirstOrDefault(m => m.Code.ToLower().Equals(dt.Rows[i][4].ToString().ToLower()));
                            if (material == null)
                            {
                                lstError.Add("Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " mã vật liệu không tồn tại");
                                isOK = false;
                            }

                            if (!CheckItem.CheckMaxLength(dt.Rows[i][8].ToString(), 9, "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " số lượng vật liệu", out message))
                            {
                                lstError.Add(message);
                                isOK = false;
                            }

                            // Kiểm tra số lượng
                            if (!CheckItem.CheckIsNumber(dt.Rows[i][8].ToString(), "Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " số lượng vật liệu", Constants.NumberFormatType.DECIMAL, out message))
                            {
                                lstError.Add(message);
                                isOK = false;
                            }

                            // Kiểm tra số lượng
                            if (Convert.ToDecimal(dt.Rows[i][8].ToString()) <= 0)
                            {
                                lstError.Add("Danh mục vật tư " + moduleCode + " \nDòng " + (i + 2) + " số lượng vật liệu");
                                isOK = false;
                            }
                        }

                        manufacture = manufacturers.FirstOrDefault(mu => mu.Code.ToLower().Equals(dt.Rows[i][9].ToString().ToLower()));
                        // trường hợp hãng sản xuất không tồn tại
                        if (manufacture == null)
                        {
                            lstError.Add("Dòng " + (i + 2) + " : Hãng sản xuất " + dt.Rows[i][9].ToString() + " không đúng");
                            isOK = false;
                        }

                        material = materials.FirstOrDefault(m => m.Code.ToLower().Equals(dt.Rows[i][3].ToString().ToLower()));

                        // Kiểm tra vật tư
                        if (material == null && dt.Rows[i][9].ToString() != Constants.TPA)
                        {
                            lstError.Add("Dòng " + (i + 2) + " : Vật tư " + dt.Rows[i][3].ToString() + " chưa có trong hệ thống");
                            isOK = false;
                        }
                        else if (material != null && !string.IsNullOrEmpty(material.Status))
                        {
                            if (material.Status.Equals(Constants.Material_Status_Stop))
                            {
                                lstError.Add("Dòng " + (i + 2) + " : Vật tư " + dt.Rows[i][3].ToString() + " đã ngừng sản xuất");
                                isOK = false;
                            }
                            else if (material.Status.Equals(Constants.Material_Status_Pause))
                            {
                                lstError.Add("Dòng " + (i + 2) + " : Vật tư " + dt.Rows[i][3].ToString() + " đã tạm dừng sử dụng");
                                isOK = false;
                            }

                        }

                        if (material != null && manufacture != null && !manufacture.Id.Equals(material.ManufactureId))
                        {
                            lstError.Add("Dòng " + (i + 2) + " : Vật tư " + dt.Rows[i][3].ToString() + "  không đúng Hãng sản xuất " + dt.Rows[i][9].ToString());
                            isOK = false;
                        }

                        if (dt.Rows[i][9] != null && !"TPA".Equals(dt.Rows[i][9].ToString()) && dt.Rows[i][2] != null && "TPA".Equals(dt.Rows[i][2].ToString().ToUpper()))
                        {

                            lstError.Add("Dòng " + (i + 2) + " : Vật tư " + dt.Rows[i][3].ToString() + "  sai thông số");
                            isOK = false;
                        }

                        if (dt.Rows[i][9].ToString().Equals(Constants.TPA) && dt.Rows[i][2].ToString().ToUpper().Equals(Constants.HAN))
                        {
                            index = dt.Rows[i][0].ToString();
                        }
                    }
                    //}
                    //else
                    //{
                    //    break;
                    //}
                }
            }
            catch (SqlException ex)
            {
                int errorNumber = ex.Number;

                if (errorNumber == 10054 || errorNumber == 2)
                {
                    //throw BusinessException.CreateInstance(MessageList.ERR011, new string[] { null });
                }
                else if (errorNumber == 17142 || errorNumber == 233)
                {
                    //throw BusinessException.CreateInstance(MessageList.ERR012, new string[] { null });
                }
                else
                {
                    //throw BusinessException.CreateInstance(MessageList.ERR004, new string[] { ex.Message });
                }
            }

            return isOK;
        }

        private bool CheckUnit(List<UnitModel> units, string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
            {
                return false;
            }
            var unit = units.Where(u => u.Name.ToLower().Equals(unitName.ToLower()));
            if (unit == null)
            {
                return false;
            }
            return true;
        }

        private bool CheckMaterialsModel(List<RawMaterialModel> rawMaterials, string rawMarerialCode)
        {
            var rawMaterial = rawMaterials.FirstOrDefault(r => r.Code.ToLower().Equals(rawMarerialCode.ToLower()));

            if (rawMaterial == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeFunctionDefinitionId"></param>
        /// <param name="productCode"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        private bool CheckFilePartList(int typeFunctionDefinitionId, string pathFolder, string productCode, string groupCode, string parentGroupCode,
            List<CheckUploadEntity> fileError, List<FolderDefinitionModel> folderDefinitions, List<FileDefinitionModel> fileDefinitions, List<string> lstError, out string materialPath)
        {
            materialPath = string.Empty;
            fileError = new List<CheckUploadEntity>();
            materialPath = Constants.Disk_Design + GetPathFile(Constants.FileDefinition_FileType_ListMaterial, productCode, groupCode, parentGroupCode, folderDefinitions, fileDefinitions);

            // Trường đường dẫn rỗng hoặc null
            if (string.IsNullOrEmpty(materialPath))
            {
                lstError.Add("File danh mục vật tư không tồn tại");
                return false;
            }

            if (!File.Exists(materialPath))
            {
                lstError.Add("File danh mục vật tư không tồn tại");
                return false;
            }

            // Bảng chưa thông tin lấy từ file excel
            // Đọc file excel
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.OpenReadOnly(materialPath);
            IWorksheet sheet = workbook.Worksheets[0];
            List<CheckUploadEntity> listCheckUploadCAD = new List<CheckUploadEntity>();
            try
            {
                CheckUploadEntity checkUploadEntity;
                LinkFileErrorEntity linkFileErrorEntity;
                List<LinkFileErrorEntity> lstLinkFileErrorEntity;
                string[] folderPartFile = Directory.GetDirectories(pathFolder + "\\" + "3D." + productCode + "\\" + "COM." + productCode);
                foreach (string file in folderPartFile)
                {
                    checkUploadEntity = new CheckUploadEntity();
                    checkUploadEntity.LinkFolder = file;
                    checkUploadEntity.ManuFacturer = Path.GetFileName(file);
                    checkUploadEntity.StatusError = false;
                    string[] filePartFile = Directory.GetFiles(file);
                    lstLinkFileErrorEntity = new List<LinkFileErrorEntity>();
                    foreach (string file1 in filePartFile)
                    {
                        linkFileErrorEntity = new LinkFileErrorEntity();
                        linkFileErrorEntity.StatusFile = false;
                        linkFileErrorEntity.FileName = Path.GetFileName(file1);
                        linkFileErrorEntity.LinkFile = file1;
                        lstLinkFileErrorEntity.Add(linkFileErrorEntity);
                    }
                    checkUploadEntity.LinkFileErrorEntity = lstLinkFileErrorEntity;
                    fileError.Add(checkUploadEntity);
                }

                folderPartFile = Directory.GetFiles(pathFolder + "\\" + "CAD." + productCode);
                foreach (string file in folderPartFile)
                {
                    checkUploadEntity = new CheckUploadEntity();
                    checkUploadEntity.LinkFolder = file;
                    checkUploadEntity.ManuFacturer = Path.GetFileName(file);
                    checkUploadEntity.StatusError = false;
                    listCheckUploadCAD.Add(checkUploadEntity);
                }
            }
            catch (Exception ex)
            {
                lstError.Add(ex.Message);

                return false;
            }
            //string partCode = dataTable.Rows[1][9].ToString().Trim().Substring(dataTable.Rows[1][7].ToString().Trim().LastIndexOf(" ") + 1, dataTable.Rows[1][7].ToString().Trim().Length - dataTable.Rows[1][7].ToString().Trim().LastIndexOf(" ") - 1);
            bool checkFolder = false;
            bool checkFile = false;
            string productCodeParent = string.Empty;
            List<string> fileOK = new List<string>();
            List<string> listPartsCode = new List<string>();
            // Duyệt danh sách để thêm vào database
            for (int i = 7; i < sheet.Rows.Count(); i++)
            {
                if (!string.IsNullOrEmpty(sheet[i, 1].Text))
                {
                    if (sheet[i, 4].Text.Contains("TPA"))
                    {
                        checkFolder = false;
                        foreach (CheckUploadEntity checkUploadCAD in listCheckUploadCAD)
                        {
                            if ((sheet[i, 4].Text + ".dwg").ToUpper() == checkUploadCAD.ManuFacturer.ToUpper())
                            {
                                checkUploadCAD.StatusError = true;
                                checkFolder = true;
                                break;
                            }
                        }

                        if (!checkFolder && listPartsCode.IndexOf(sheet[i, 4].Text) == -1)
                        {
                            lstError.Add("Vật tư " + sheet[i, 4].Text + " chưa có file CAD");
                        }
                    }

                    if (sheet[i, 10].Text == Constants.TPA)
                    {
                        if (sheet[i, 1].Text.Split('.').Length <= 1)
                        {
                            productCodeParent = sheet[i, 4].Text;
                        }

                        if (sheet[i, 4].Text.Contains(productCode))
                        {
                            if (!CheckFileAndFolderByIndex(sheet[i, 1].Text, productCode, sheet[i, 4].Text, CheckPartsChildExits(sheet, sheet[i, 4].Text), pathFolder, fileOK, lstError))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        checkFolder = false;
                        foreach (CheckUploadEntity checkUploadEntity in fileError)
                        {
                            if (string.Equals(sheet[i, 10].Text.Trim().ToUpper(), checkUploadEntity.ManuFacturer.Trim().ToUpper(), StringComparison.CurrentCulture))
                            {
                                checkFile = false;
                                foreach (LinkFileErrorEntity linkFileErrorEntity in checkUploadEntity.LinkFileErrorEntity)
                                {
                                    if ((sheet[i, 4].Text + ".ipt").Replace('/', ')').Replace(" ", "") == linkFileErrorEntity.FileName.Replace(" ", ""))
                                    {
                                        linkFileErrorEntity.StatusFile = true;
                                        checkFile = true;
                                        break;
                                    }
                                }
                                if (!checkFile)
                                {
                                    lstError.Add("Vật tư " + sheet[i, 4].Text + " chưa có file ipt");
                                    return false;
                                }
                                checkFolder = true;
                                checkUploadEntity.StatusError = true;
                                break;
                            }
                        }
                        if (!checkFolder)
                        {
                            lstError.Add("Hãng sản xuất " + sheet[i, 10].Text + " chưa có thư mục chứa file ipt");
                            return false;
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return true;
        }

        private bool CheckFileAndFolderByIndex(string index, string sourceCode, string productCode, string productCodeParent, string pathFolder, List<string> fileOK, List<string> lstError)
        {
            string pathFile = string.Empty;
            if (!fileOK.Contains(productCode))
            {
                if (index.Split('.').Length > 1)
                {
                    pathFile = pathFolder + "\\" + "3D." + sourceCode + "\\" + productCodeParent + "\\" + productCode;
                    if (!File.Exists(pathFile + ".ipt") && !File.Exists(pathFile + ".ipn") && !File.Exists(pathFile + ".iam"))
                    {
                        lstError.Add("Vật tư " + productCode + " không có tài liệu 3D");
                        return false;
                    }
                }
                else
                {
                    pathFile = pathFolder + "\\" + "3D." + sourceCode + "\\" + productCode;
                    if (!Directory.Exists(pathFile) && !File.Exists(pathFile + ".ipt") && !File.Exists(pathFile + ".iam") && !File.Exists(pathFile + ".idw"))
                    {
                        lstError.Add("Vật tư " + productCode + " không có tài liệu 3D");
                        return false;
                    }
                    else if (Directory.Exists(pathFile) && !File.Exists(pathFile + ".ipt") && !File.Exists(pathFile + ".iam") && !File.Exists(pathFile + ".idw"))
                    {
                        if (!File.Exists(pathFile + "\\" + productCode + ".ipt") && !File.Exists(pathFile + "\\" + productCode + ".iam") && !File.Exists(pathFile + "\\" + productCode + ".ipn"))
                        {
                            lstError.Add("Vật tư " + productCode + " không có tài liệu 3D");
                            return false;
                        }
                    }
                }
                fileOK.Add(productCode);
            }
            return true;
        }

        private string CheckPartsChildExits(IWorksheet sheet, string partsCode)
        {
            bool isChild = false;
            string chil = string.Empty;
            string parent = string.Empty;
            for (int i = 7; i < sheet.Rows.Count(); i++)
            {
                if (sheet[i, 4].Text != null && sheet[i, 4].Text.Length > partsCode.Length && sheet[i, 4].Text.Contains(partsCode))
                {
                    isChild = true;
                    chil = sheet[i, 4].Text;
                    break;
                }
            }

            string path = string.Empty;
            if (isChild)
            {

                string[] e = partsCode.Split('.');
                if (e.Length == 3)
                {
                    path = partsCode;
                }
                else if (e.Length > 3)
                {
                    parent = e[0] + "." + e[1] + "." + e[2];
                    path = parent;
                    for (int j = 3; j < e.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(e[j]) && e[j] != ".")
                        {
                            parent += "." + e[j];
                            path += "\\" + parent;
                        }
                    }
                }
            }
            else
            {
                string[] e = partsCode.Split('.');
                if (e.Length == 3)
                {
                    path = partsCode;
                }
                else if (e.Length > 3)
                {
                    parent = e[0] + "." + e[1] + "." + e[2];
                    path = parent;
                    for (int j = 3; j < e.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(e[j]) && e[j] != "." && j < e.Length - 1)
                        {
                            parent += "." + e[j];
                            path += "\\" + parent;
                        }
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Lấy đường dẫn file theo định nghĩa
        /// </summary>
        /// <param name="moduleCode">Mã module</param>
        /// <param name="groupCode">Mã nhóm</param>
        /// <param name="parentGroupCode">Mã nhóm cha</param>
        /// <param name="folderDefinitions">Danh sách định nghĩa folder </param>
        /// <param name="fileDefinitions">Danh sách định nghĩa file</param>
        /// <param name="lstError"></param>
        /// <returns></returns>
        public string GetPathFile(int fileType, string moduleCode, string groupCode, string parentGroupCode, List<FolderDefinitionModel> folderDefinitions, List<FileDefinitionModel> fileDefinitions)
        {
            string filePath = string.Empty;

            var fileDefinition = fileDefinitions.FirstOrDefault(r => r.FileType == fileType);

            if (fileDefinition != null)
            {
                filePath = GetPathFolderParent(fileDefinition.FolderDefinitionId, moduleCode, groupCode, parentGroupCode, folderDefinitions) + "\\" + GetFileNameInfor(fileDefinition, moduleCode, groupCode, parentGroupCode);
            }

            return filePath;
        }

        /// <summary>
        /// Lấy đường dẫn file theo định nghĩa theo tên file
        /// </summary>
        /// <param name="moduleCode">Mã module</param>
        /// <param name="groupCode">Mã nhóm</param>
        /// <param name="parentGroupCode">Mã nhóm cha</param>
        /// <param name="folderDefinitions">Danh sách định nghĩa folder </param>
        /// <param name="fileDefinitions">Danh sách định nghĩa file</param>
        /// <param name="lstError"></param>
        /// <returns></returns>
        public string GetPathFileByName(string fileName, string moduleCode, string groupCode, string parentGroupCode, List<FolderDefinitionModel> folderDefinitions, List<FileDefinitionModel> fileDefinitions)
        {
            string filePath = string.Empty;

            if (string.IsNullOrEmpty(fileName))
            {
                return filePath;
            }

            var fileDefinition = fileDefinitions.FirstOrDefault(r => !string.IsNullOrEmpty(r.FileDefinitionNameFirst) && r.FileDefinitionNameFirst.ToLower().Contains(fileName.ToLower()));

            if (fileDefinition != null)
            {
                filePath = GetPathFolderParent(fileDefinition.FolderDefinitionId, moduleCode, groupCode, parentGroupCode, folderDefinitions) + "\\" + GetFileNameInfor(fileDefinition, moduleCode, groupCode, parentGroupCode);
            }

            return filePath;
        }


        /// <summary>
        /// Lấy đường dẫn folder theo định nghĩa
        /// </summary>
        /// <param name="moduleCode">Mã module</param>
        /// <param name="groupCode">Mã nhóm</param>
        /// <param name="parentGroupCode">Mã nhóm cha</param>
        /// <param name="folderDefinitions">Danh sách định nghĩa folder </param>
        /// <param name="fileDefinitions">Danh sách định nghĩa file</param>
        /// <param name="lstError"></param>
        /// <returns></returns>
        public string GetPathFolder(int folderType, string moduleCode, string groupCode, string parentGroupCode, List<FolderDefinitionModel> folderDefinitions, List<FileDefinitionModel> fileDefinitions)
        {
            string folderPath = string.Empty;

            var folderUpload = folderDefinitions.FirstOrDefault(r => r.FolderType == folderType);

            if (folderUpload != null)
            {
                folderPath = GetPathFolderParent(folderUpload.FolderDefinitionManageId, moduleCode, groupCode, parentGroupCode, folderDefinitions) + "\\" + GetFolderName(folderUpload, moduleCode, groupCode, parentGroupCode);
            }

            return folderPath;
        }

        /// <summary>
        /// Lây đường dẫn tên thư mục cha
        /// </summary>
        /// <param name="folderDefinitionId"></param>
        /// <returns></returns>
        private string GetPathFolderParent(string folderDefinitionId, string sourceCode, string groupCode, string parentGroupCode, List<FolderDefinitionModel> folderDefinitions)
        {
            string path = string.Empty;
            var folderDefinition = folderDefinitions.FirstOrDefault(r => r.FolderDefinitionId.Equals(folderDefinitionId));
            if (folderDefinition == null)
            {
                return path;
            }

            path += GetFolderName(folderDefinition, sourceCode, groupCode, parentGroupCode);

            if (!string.IsNullOrEmpty(folderDefinition.FolderDefinitionManageId))
            {
                return GetPathFolderParent(folderDefinition.FolderDefinitionManageId, sourceCode, groupCode, parentGroupCode, folderDefinitions) + "\\" + path;
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// Lấy tên thư mục
        /// </summary>
        /// <param name="row"></param>
        /// <param name="sourceCode"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string GetFolderName(FolderDefinitionModel row, string sourceCode, string groupCode, string parentGroupCode)
        {
            string folerName = string.Empty;
            if (!string.IsNullOrEmpty(row.FolderDefinitionFirst))
            {
                folerName += row.FolderDefinitionFirst;
            }
            switch ((StatusDataTable.TypeFolderName)row.FolderDefinitionBetween)
            {
                case StatusDataTable.TypeFolderName.None:
                    {


                        break;
                    }
                case StatusDataTable.TypeFolderName.GroupCode:
                    {
                        folerName += groupCode;


                        break;
                    }
                case StatusDataTable.TypeFolderName.ProductCode:
                    {

                        folerName += sourceCode;

                        break;
                    }
                case StatusDataTable.TypeFolderName.ParentGroupCode:
                    {
                        folerName += parentGroupCode;

                        break;
                    }
            }


            if (!string.IsNullOrEmpty(row.FolderDefinitionLast))
            {
                folerName += row.FolderDefinitionLast;
            }


            return folerName;
        }

        /// </summary>
        /// <param name="row"></param>
        /// <param name="sourceCode"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public string GetFileNameInfor(FileDefinitionModel model, string sourceCode, string groupCode, string parentGroupCode)
        {
            string folerName = string.Empty;

            if (!string.IsNullOrEmpty(model.FileDefinitionNameFirst))
            {
                folerName += model.FileDefinitionNameFirst;
            }

            switch ((StatusDataTable.TypeFolderName)model.FileDefinitionNameBetween)
            {
                case StatusDataTable.TypeFolderName.None:
                    {
                        break;
                    }
                case StatusDataTable.TypeFolderName.GroupCode:
                    {
                        folerName += groupCode;


                        break;
                    }
                case StatusDataTable.TypeFolderName.ProductCode:
                    {

                        folerName += sourceCode;

                        break;
                    }
                case StatusDataTable.TypeFolderName.ParentGroupCode:
                    {
                        folerName += parentGroupCode;

                        break;
                    }
            }

            if (!string.IsNullOrEmpty(model.FileDefinitionNameLast))
            {
                folerName += model.FileDefinitionNameLast;
            }

            return folerName;
        }
        #endregion

        //Đọc file Danh mục vật tư
        public List<ModuleMaterialModel> LoadListMaterial(string path)
        {
            List<ModuleMaterialModel> materials = new List<ModuleMaterialModel>();
            List<string> listIds = new List<string>();
            string index = string.Empty;

            // Đọc file excel
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.OpenReadOnly(path);
            IWorksheet sheet = workbook.Worksheets[0];

            ModuleMaterialModel moduleMaterialModel;

            try
            {
                for (int i = 7; i <= sheet.Rows.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(sheet[i, 1].Value) && !string.IsNullOrEmpty(sheet[i, 2].Value))
                    {
                        moduleMaterialModel = new ModuleMaterialModel()
                        {
                            Index = sheet[i, 1].Value,
                            MaterialName = sheet[i, 2].Value,
                            Specification = sheet[i, 3].Value,
                            MaterialCode = sheet[i, 4].Value,
                            RawMaterialCode = sheet[i, 5].Value,
                            UnitName = sheet[i, 6].Value,
                            Quantity = sheet[i, 7].Value.ConvertToDecimal(),
                            RawMaterial = sheet[i, 8].Value,
                            Weight = sheet[i, 9].Value.ConvertToDecimal(),
                            ManufacturerCode = sheet[i, 10].Value,
                            Note = sheet[i, 11].Text
                        };

                        materials.Add(moduleMaterialModel);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }

            workbook.Close();
            excelEngine.Dispose();

            return materials;
        }
    }
}
