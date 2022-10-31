using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using iTextSharp.text.pdf.qrcode;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Common;
using QLTK.Service.Model;
using QLTK.Service.Model.Api;
using QLTK.Service.Model.ClassRoom;
using QLTK.Service.Model.Design3D;
using QLTK.Service.Model.DesignStructure;
using QLTK.Service.Model.Downloads;
using QLTK.Service.Model.Files;
using QLTK.Service.Model.IGS;
using QLTK.Service.Model.MAT;
using QLTK.Service.Model.Materials;
using QLTK.Service.Model.Modules;
using QLTK.Service.Model.Products;
using QLTK.Service.Model.Solution;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;

namespace QLTK.Service.Business.Service
{
    public class TestDesign
    {

        private ApiUtil apiUtil = new ApiUtil();
        FileService fileService = new FileService();

        #region Kiểm tra tài liệu thiết kế

        /// <summary>
        /// Kiểm tra tài liệu thiết kế
        /// </summary>
        /// <param name="modelTest"></param>
        /// <returns></returns>
        public object CheckDocumentDesign(TestDesignStructureModel modelTest)
        {
            Stopwatch stopwatch = new Stopwatch();

            ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();
            string selectedPath = modelTest.SelectedPath;

            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (!selectedPath.StartsWith(Constants.Disk_Start_Design))
                {
                    throw NTSException.CreateInstance("Chọn sai thư mục");
                }

                modelTest.ModuleCode = Path.GetFileNameWithoutExtension(selectedPath);
            }
            stopwatch.Start();
            resultApiModel = apiUtil.GetModuleInfo(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);

            stopwatch.Stop();
            Debug.WriteLine($"Thời gian kiểm tra tồn tại Module từ API: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Restart();

            ModuleModel moduleModel = new ModuleModel();
            if (resultApiModel.SuccessStatus)
            {
                moduleModel = resultApiModel.Data;
            }
            else
            {
                throw NTSException.CreateInstance(resultApiModel.Message);
            }

            modelTest.ModuleGroupCode = moduleModel.ModuleGroupCode;

            FileService fileService = new FileService();

            string folderParent = string.Empty;
            List<SoftDesignModel> listSoftĐesign = new List<SoftDesignModel>();
            List<HardDesignModel> listHardĐesign = new List<HardDesignModel>();
            List<ErrorDesignStructureModel> listErrorDesignCAD = new List<ErrorDesignStructureModel>();
            ResultCheckDMVTModel checkDMVTModel = new ResultCheckDMVTModel();
            List<MaterialModel> listMaterial = new List<MaterialModel>();

            List<MATFileResultModel> listFileMAT = new List<MATFileResultModel>();
            List<IGSFileResultModel> listFileIGS = new List<IGSFileResultModel>();
            CADResultModel cadResultModel = new CADResultModel();
            UploadFolderResultModel uploadFolderResult = new UploadFolderResultModel();
            UploadFolderModel model = new UploadFolderModel();
            var checkStatus = true;
            string messageError = string.Empty;
            List<string> listError = new List<string>();
            List<string> listErrorCheckCAD = new List<string>();
            List<ErrorCheckModel> listFileSize = new List<ErrorCheckModel>();
            List<string> listFileRedundant = new List<string>();
            List<DocumentFileSizeModel> listDocumentFileSize = new List<DocumentFileSizeModel>();

            try
            {
                // Lấy dữ liệu từ server
                var resultAPi = apiUtil.GetData(modelTest.ApiUrl, modelTest.Token, modelTest.Type);

                stopwatch.Stop();
                Debug.WriteLine($"Thời gian lấy thông tin Module từ API: {stopwatch.ElapsedMilliseconds} ms");
                stopwatch.Restart();

                if (!resultAPi.SuccessStatus)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
                }

                var dataCheck = resultAPi.Data;
                model.ListCodeRule = dataCheck.ListCodeRule;
                model.ListFileDefinition = dataCheck.ListFileDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
                model.ListFolderDefinition = dataCheck.ListFolderDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
                model.ListManufacturerModel = dataCheck.ListManufacturerModel;
                model.ListMaterialGroupModel = dataCheck.ListMaterialGroupModel;
                model.ListMaterialModel = dataCheck.ListMaterialModel;
                model.ListRawMaterialsModel = dataCheck.ListRawMaterialsModel;
                model.ListUnitModel = dataCheck.ListUnitModel;

                if (string.IsNullOrEmpty(selectedPath))
                {
                    selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                    if (!Directory.Exists(selectedPath))
                    {
                        throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, selectedPath);
                    }

                    modelTest.SelectedPath = selectedPath;
                }

                string folderName = Path.GetFileName(selectedPath);

                modelTest.PathFileMaterial = Constants.Disk_Design + fileService.GetPathFile(Constants.FileDefinition_FileType_ListMaterial, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                modelTest.PathFileIDW = Constants.Disk_Design + fileService.GetPathFile(Constants.FileDefinition_FileType_IDW, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                modelTest.PathFolderBCCAD = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_BCCAD, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                modelTest.PathFolderFileCAD = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_FileCAD, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                modelTest.PathFolderMAT = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_MAT, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                modelTest.PathFolder3D = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_3D, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                modelTest.PathFolderIGS = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_IGS, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);

                model.ModuleCode = modelTest.ModuleCode;
                model.Path = selectedPath;
                model.ModuleGroupCode = moduleModel.ModuleGroupCode;
                model.ModuleGroupId = moduleModel.ModuleGroupId;
                model.DesignType = modelTest.Type;

                if (modelTest.CheckModel.IsCheckDesignFolder)
                {
                    var check = fileService.CheckFolderUpload(model.DesignType, model.Path, model.ModuleCode, model.ModuleGroupCode, moduleModel.ParentGroupCode,
                        uploadFolderResult.ListError, model.ListFolderDefinition, model.ListFileDefinition, model.ListMaterialModel, model.ListRawMaterialsModel, model.ListManufacturerModel,
                        model.ListMaterialGroupModel, model.ListUnitModel, model.ListCodeRule, uploadFolderResult.LstError, uploadFolderResult.ListFolder, false, out string materialPath, dataCheck.Modules,
                        listFileRedundant);

                    checkStatus = check.Status;
                    if (check.Result != null)
                    {
                        if (!string.IsNullOrEmpty(check.Result.MessageError))
                        {
                            messageError = check.Result.MessageError;
                        }
                    }

                    listError.AddRange(uploadFolderResult.LstError);
                }

                stopwatch.Stop();
                Debug.WriteLine($"Thời gian Kiểm tra Folder upload: {stopwatch.ElapsedMilliseconds} ms");
                stopwatch.Restart();

                if (!string.IsNullOrEmpty(messageError))
                {
                    listError.Add(messageError);
                }
                else
                {
                    if (modelTest.CheckModel.IsCheckMatch)
                    {
                        listSoftĐesign = FillDataInGridIDW(modelTest, listError).OrderBy(a => a.NameCompare).ToList();
                        listHardĐesign = LoadHardDesign(modelTest).OrderBy(a => a.NameCompare).ToList();
                    }

                    stopwatch.Stop();
                    Debug.WriteLine($"Thời gian kiểm tra IsCheckMatch: {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();


                    if (modelTest.CheckModel.IsCheckDesignCAD)
                    {
                        listErrorDesignCAD = CheckCAD(modelTest, listErrorCheckCAD);
                    }
                    stopwatch.Stop();
                    Debug.WriteLine($"Thời gian kiểm tra CheckCAD: {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();

                    if (modelTest.CheckModel.IsCheckSoftHardCAD)
                    {
                        cadResultModel = CheckSoftHardCAD(modelTest, listError);
                    }
                    stopwatch.Stop();
                    Debug.WriteLine($"Thời gian kiểm tra CheckSoftHardCAD: {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();

                    if (modelTest.CheckModel.IsCheckMAT)
                    {
                        listFileMAT = CheckMAT(modelTest);
                    }
                    stopwatch.Stop();
                    Debug.WriteLine($"Thời gian kiểm tra CheckMAT: {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();

                    if (modelTest.CheckModel.IsCheckIGS)
                    {
                        listFileIGS = CheckIGS(modelTest);
                    }
                    stopwatch.Stop();
                    Debug.WriteLine($"Thời gian kiểm tra CheckIGS: {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();

                    if (modelTest.CheckModel.IsCheckDMVT)
                    {
                        checkDMVTModel = CheckFileDMVT(modelTest, listError);
                        listMaterial = LoadDMVT(modelTest.PathFileMaterial);
                    }
                    stopwatch.Stop();
                    Debug.WriteLine($"Thời gian kiểm tra IsCheckDMVT: {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();

                    if (modelTest.CheckModel.IsCheckDesignFolder)
                    {
                        CheckFolder3D(modelTest, listError, model.ListMaterialModel, listFileSize, listFileRedundant, listDocumentFileSize);

                    }
                    stopwatch.Stop();
                    Debug.WriteLine($"Thời gian kiểm tra CheckFolder3D: {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();

                }

                return new
                {
                    listError,
                    listSoftĐesign,
                    listHardĐesign,
                    listErrorDesignCAD,
                    cadResultModel,
                    listFileMAT,
                    listFileIGS,
                    checkDMVTModel,
                    listMaterial,
                    listErrorCheckCAD,
                    listFileSize,
                    listFileRedundant,
                    listDocumentFileSize
                };
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }
        }

        #endregion

        public void CheckFolder3D(TestDesignStructureModel modelTest, List<string> listError, List<MaterialFromDBModel> materials, List<ErrorCheckModel> listFileSize, List<string> listFileRedundant, List<DocumentFileSizeModel> listDocumentFileSize)
        {
            FileService fileService = new FileService();
            string moduleCode = string.Empty;

            var materialFiles = LoadFullDMVT(modelTest.PathFileMaterial, out moduleCode);
            var allFiles = fileService.GetAllFileInFolder(modelTest.PathFolder3D);
            var fileTPA = "COM." + modelTest.ModuleCode + "\\TPA";
            var allFilesExceptTPA = allFiles.Where(t => !t.FullName.ToUpper().Contains(fileTPA.ToUpper())).ToList();

            var allFolders = fileService.GetAllFolder(modelTest.PathFolder3D);
            allFolders = allFolders.Where(r => !r.ToLower().Contains("oldversion") && !Path.GetFileName(r).ToUpper().StartsWith("COM.")).ToList();

            var allFoldersExceptTPA = allFolders.Where(r => !Path.GetFileName(r).ToUpper().EndsWith("TPA")).ToList();

            var files = allFiles.Where(r => !r.Extension.ToLower().Equals(".idw") && !r.Extension.ToLower().Equals(".ipj")
                                           && !r.FullName.ToLower().Contains("lockfile") && !r.FullName.ToLower().Contains("oldversion")).ToList();

            var filesExceptTPA = allFilesExceptTPA.Where(r => !r.Extension.ToLower().Equals(".idw") && !r.Extension.ToLower().Equals(".ipj")
                                           && !r.FullName.ToLower().Contains("lockfile") && !r.FullName.ToLower().Contains("oldversion")).ToList();

            // Kiểm tra file trong thư mục 3d trên máy nếu có file khác file Ipt thì cảnh báo THỪA(trừ những file trong thư mục Lockfile, oldveríon và file.idw, .ipj, .iam)
            var fileErrors = filesExceptTPA.Where(r => !r.Extension.ToLower().Equals(".ipt")
                                            && !r.Extension.ToLower().Equals(".iam")).ToList();

            foreach (var file in fileErrors)
            {
                listError.Add("Thừa file " + file.FullName);
                listFileRedundant.Add(file.FullName);
            }

            // Tên file khác với mã mdule và nằm ngoài thư mục COM
            string pathFolderCom = modelTest.PathFolder3D + "\\COM.";
            fileErrors = files.Where(r => !r.Name.ToUpper().StartsWith(modelTest.ModuleCode.ToUpper()) && !r.FullName.StartsWith(pathFolderCom)).ToList();
            foreach (var file in fileErrors)
            {
                listError.Add("File " + file.FullName + " không đúng địa chỉ");
            }

            // Kiểm tra thư mục COM và tên file  = mã module 
            fileErrors = filesExceptTPA.Where(r => r.FullName.ToLower().StartsWith(pathFolderCom.ToLower()) && r.Name.ToUpper().Contains(modelTest.ModuleCode.ToUpper())).ToList();
            foreach (var file in fileErrors)
            {
                listError.Add("File " + file.FullName + " không đúng địa chỉ");
            }

            // Kiểm tra thư mục COM và tên file không bắt đầu TPA nếu không nằm đúng thư mục hãng 
            fileErrors = filesExceptTPA.Where(r => r.FullName.ToLower().StartsWith(pathFolderCom.ToLower()) && !r.Name.ToLower().StartsWith(Constants.TPA)).ToList();
            string name, folderName;
            MaterialFromDBModel material;
            foreach (var file in fileErrors)
            {
                name = Path.GetFileNameWithoutExtension(file.FullName);
                folderName = Path.GetFileName(Path.GetDirectoryName(file.FullName));
                material = materials.FirstOrDefault(r => r.Code.ToLower().Equals(name.ToLower()));

                if (material != null && !material.ManufactureCode.ToLower().Equals(folderName.ToLower()))
                {
                    listError.Add("File " + file.FullName + " không đúng địa chỉ");
                }
            }

            // Kiểm tra thư mục COM và tên file bắt đầu bằng TPA nếu không năm trong thư mục TPA
            fileErrors = files.Where(r => r.FullName.ToLower().StartsWith(pathFolderCom.ToLower()) && r.Name.ToLower().StartsWith(Constants.TPA)).ToList();
            foreach (var file in fileErrors)
            {
                folderName = Path.GetFileName(Path.GetDirectoryName(file.FullName));
                if (Constants.TPA.Equals(folderName.ToUpper()))
                {
                    listError.Add("File " + file.FullName + " không đúng địa chỉ");
                }
            }

            // Kiểm tra thiết kế 3D trên nguồn với trên thiết kế, nếu khác size

            // Kiểm tra mã vật tư có tồn tại trong DMVT nhưng không có file .ipt hoặc .iam
            int countExist = 0;
            foreach (var item in materialFiles)
            {
                var a = item.Code.Replace('/', ')');
                countExist = files.Count(r => Path.GetFileNameWithoutExtension(r.FullName).ToUpper().Equals(a.ToUpper()) && (r.Extension.ToLower().Equals(".ipt") || r.Extension.ToLower().Equals(".iam"))); ; ;
                if (countExist == 0)
                {
                    listError.Add("Vật tư " + item.Code + " không có tài liệu 3D");
                }
            }

            // Kiểm tra tên file không có trong DMVT và tên file khác mã Module thì báo thừa
            int temp = 0;
            foreach (var item in filesExceptTPA)
            {
                var fileName = Path.GetFileNameWithoutExtension(item.FullName);
                if (!fileName.Equals(modelTest.ModuleCode))
                {
                    temp = materialFiles.Count(r => fileName.ToUpper().Equals(r.Code.Replace('/', ')').ToUpper()));
                    if (temp == 0)
                    {
                        listError.Add("File " + item.FullName + " thừa");
                        listFileRedundant.Add(item.FullName);
                    }
                }
            }

            // Nếu trong thư mục OldVersions có file
            string pathFolderOld = modelTest.PathFolder3D + @"\OldVersions";
            fileErrors = allFilesExceptTPA.Where(r => r.FullName.ToLower().StartsWith(pathFolderOld.ToLower())).ToList();
            foreach (var file in fileErrors)
            {
                listError.Add("File " + file.FullName + " thừa");
                listFileRedundant.Add(file.FullName);
            }

            // Nếu tồn tại các folder không có ở cột mã và hãng trong DMVT
            var folders = allFoldersExceptTPA.Where(t => !t.EndsWith("3D." + modelTest.ModuleCode)).ToList();
            MaterialModel materialModel;
            foreach (var item in folders)
            {
                folderName = Path.GetFileName(item);
                materialModel = materialFiles.FirstOrDefault(r => r.Code.Replace('/', ')').ToLower().Equals(folderName.ToLower()) || r.ManufactureCode.ToLower().Equals(folderName.ToLower()));

                if (materialModel == null)
                {
                    listError.Add("Folder " + item + " thừa");
                    listFileRedundant.Add(item);
                }
            }

            Design3DFileModel design3DFileModel = new Design3DFileModel();
            design3DFileModel.Files = (from f in files
                                       join m in materialFiles on Path.GetFileNameWithoutExtension(f.Name).ToUpper() equals m.Code.Replace('/', ')') into fm
                                       where fm != null && fm.Count() > 0
                                       select new FileModel
                                       {
                                           FileName = f.Name,
                                           FilePath = f.FullName,
                                           Size = f.Length
                                       }).ToList();

            design3DFileModel.ModuleCode = modelTest.ModuleCode;

            var dataCheck = apiUtil.CheckFile3D(modelTest, design3DFileModel, modelTest.Token);

            if (!dataCheck.SuccessStatus)
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
            }

            foreach (var item in dataCheck.Data.ListDocumentFileSize)
            {
                listDocumentFileSize.Add(new DocumentFileSizeModel
                {
                    ModuleId = item.ModuleId,
                    Name = item.Name,
                    FilePath = item.FilePath,
                    ServerPath = item.ServerPath
                });
            }

            // Kiểm tra thư viện 3D, File không bắt đầu bằng TPA, file khác size với file trên thư viện 3D VT
            foreach (var item in dataCheck.Data.FilePartDifferentSize)
            {

                listError.Add("File " + item + " khác size");
                listFileSize.Add(new ErrorCheckModel
                {
                    FileName = Path.GetFileName(item),
                    FilePath = item,
                    Type = Constants.Type_DownLoad_Server,
                    Index = listError.Count()
                });
            }

            // Kiểm tra thiết kế 3D File bắt đầu bằng mã module với trên nguồn thiết kế, nếu khác size. 
            foreach (var item in dataCheck.Data.FileModuleDifferentSize)
            {
                listError.Add("File " + item + " khác size với thiết kế cũ trên nguồn");
                listFileSize.Add(new ErrorCheckModel
                {
                    FileName = Path.GetFileName(item),
                    FilePath = item,
                    Type = Constants.Type_DownLoad_Ftp,
                    Index = listError.Count()
                });
            }

            // File.ipt hoặc .iam dùng chung không có trên thư viện của module dùng chung
            foreach (var item in dataCheck.Data.FileShareNotExist)
            {
                listError.Add("File " + item + " dùng chung không có trên thư viện");
                listFileSize.Add(new ErrorCheckModel
                {
                    FileName = Path.GetFileName(item),
                    FilePath = item,
                    Type = Constants.Type_DownLoad_Ftp,
                    Index = listError.Count()
                });
            }

            // File.ipt hoặc .iam dùng chung khác size trên thư viện của module dùng chung 
            foreach (var item in dataCheck.Data.FileShareDifferentSize)
            {
                listError.Add("File " + item + " dùng chung khác size trên thư viện");
                listFileSize.Add(new ErrorCheckModel
                {
                    FileName = Path.GetFileName(item),
                    FilePath = item,
                    Type = Constants.Type_DownLoad_Ftp,
                    Index = listError.Count()
                });
            }
        }

        public void GetUserDefProperties(string mPath, List<string> listError, ref string[] mName, ref string[] mVal)
        {
            Inventor.Property oProp = default(Inventor.Property);
            Inventor.PropertySet oPropSet = default(Inventor.PropertySet);
            int mArrIndex = default(int);
            Inventor.ApprenticeServerComponent oApprenticeApp = new Inventor.ApprenticeServerComponent();
            Inventor.ApprenticeServerDocument oApprenticeServerDoc = default(Inventor.ApprenticeServerDocument);
            try
            {
                oApprenticeServerDoc = oApprenticeApp.Open(mPath);
            }
            catch (Exception e)
            {
                listError.Add("File " + mPath + " không tồn tại.");
                throw;
            }

            foreach (Inventor.PropertySet tempLoopVar_oPropSet in oApprenticeServerDoc.PropertySets)
            {
                oPropSet = tempLoopVar_oPropSet;
                if (oPropSet.DisplayName == "User Defined Properties")
                {
                    mArrIndex = 0;
                    mName = new string[oPropSet.Count + 1];
                    mVal = new string[oPropSet.Count + 1];
                    foreach (Inventor.Property tempLoopVar_oProp in oPropSet)
                    {
                        oProp = tempLoopVar_oProp;

                        if (oProp.Name.ToLower().StartsWith("plot"))
                        {
                            continue;
                        }

                        //if (Microsoft.VisualBasic.Information.VarType(oProp.Value) != Constants.vbDate)
                        //{
                        //    mName[mArrIndex] = (string)oProp.Name;
                        //    mVal[mArrIndex] = (string)oProp.Value;
                        //    mArrIndex++;
                        //}

                        if (oProp.Value.GetType() != typeof(DateTime))
                        {
                            mName[mArrIndex] = (string)oProp.Name;
                            mVal[mArrIndex] = (string)oProp.Value;
                            mArrIndex++;
                        }
                    }
                    break;
                }
            }
        }

        public string DateString(string ToFileName)
        {
            try
            {
                long mVal = System.Convert.ToInt64(Convert.ToInt64(ToFileName));
                TimeSpan a1 = TimeSpan.FromSeconds(mVal);
                DateTime mDate = default(DateTime);
                mDate = DateTime.MinValue;
                mDate = mDate.AddDays(System.Convert.ToDouble(a1.TotalDays));
                return mDate.ToString();
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                return String.Empty;
                // throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }

        }

        public List<SoftDesignModel> FillDataInGridIDW(TestDesignStructureModel model, List<string> listError)
        {
            List<SoftDesignModel> listSoftDesign = new List<SoftDesignModel>();
            int M = default(int);
            int i = default(int);
            string[] mName = null;
            string[] mVal = null;
            string mSt;

            try
            {
                GetUserDefProperties(model.PathFileIDW, listError, ref mName, ref mVal);
                M = mName.Length;

                SoftDesignModel softDesignModel;
                for (i = 0; i <= M - 2; i += 2)
                {
                    softDesignModel = new SoftDesignModel();
                    if (!string.IsNullOrEmpty(mName[i]))
                    {
                        mSt = "";
                        softDesignModel.Name = mName[i].Replace("Size.", "");
                        softDesignModel.NameCompare = softDesignModel.Name.Replace(".ipt", "").Replace(".iam", "");
                        softDesignModel.IValue = mVal[i];
                        softDesignModel.WValue = mVal[i + 1];
                        softDesignModel.Date = DateString(mVal[i + 1]);
                        softDesignModel.IsExistName = false;
                        softDesignModel.TxtPathSC = model.PathFileIDW;
                        softDesignModel.PathDMVT = model.PathFileMaterial;
                        softDesignModel.ModuleCode = model.ModuleCode;
                        if (mName[i] != "" && mName[i].ToUpper().Contains((string)model.ModuleCode.ToUpper()))
                        {
                            listSoftDesign.Add(softDesignModel);
                        }
                    }
                }
            }
            catch (NTSException ntsEx)
            {
                NtsLog.LogError(ntsEx);
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                listError.Add("Không đọc được dữ liệu file" + model.PathFileIDW);
            }

            if (listSoftDesign.Count() > 0)
            {
                listSoftDesign[0].SelectedPath = model.SelectedPath;
            }

            return listSoftDesign;

        }

        public List<HardDesignModel> LoadHardDesign(TestDesignStructureModel model)
        {
            List<HardDesignModel> listHardDesign = new List<HardDesignModel>();

            if (!Directory.Exists(model.PathFolderBCCAD))
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, model.PathFolderBCCAD);
            }

            Stopwatch stopwatch = new Stopwatch();

            string[] filepaths = Directory.GetFiles(model.PathFolderBCCAD);

            HardDesignModel hardDesignModel;
            Parallel.ForEach(filepaths, async item =>
            {
                // some pre stuff
                hardDesignModel = await CheckHardDesign(item, model.ModuleGroupCode, model.ModuleCode);
                if (hardDesignModel != null)
                {
                    listHardDesign.Add(hardDesignModel);
                }
            });

            return listHardDesign;

        }

        private async Task<HardDesignModel> CheckHardDesign(string filePath, string modelGroupCode, string moduleCode)
        {
            System.Collections.ArrayList barcodes;
            List<object> codes;
            int indexOfDot = 0;
            int length = 0;
            int count = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            HardDesignModel hardDesignModel = new HardDesignModel();
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            if (!fileName.Contains(modelGroupCode))
            {
                return hardDesignModel;
            }

            barcodes = new System.Collections.ArrayList();
            //Image img = Image.FromFile(filePath);
            using (Bitmap mBitmap = new Bitmap(filePath))
            {
                BarcodeImaging.FullScanPage(ref barcodes, mBitmap, 100);
            }

            if (barcodes.Count == 0)
            {
                return null;
            }

            codes = barcodes.ToArray().Where(o => o.ToString().StartsWith(modelGroupCode)).ToList();
            if (codes.Count > 0)
            {
                hardDesignModel.Code = codes.FirstOrDefault().ToString();
            }
            else
            {
                return null;
            }

            hardDesignModel.NameCompare = hardDesignModel.Code;

            indexOfDot = moduleCode.IndexOf(".");
            length = moduleCode.Length - 1;
            if (!string.IsNullOrEmpty(hardDesignModel.Code) && !hardDesignModel.Code.Contains(moduleCode.Substring(indexOfDot + 1, length - indexOfDot)))
            {
                return null;
            }

            var date = barcodes.ToArray().Where(o => o.ToString().StartsWith("1") && o.ToString().Length > 2).ToArray();
            if (date != null && date.Count() > 0)
            {
                try
                {
                    hardDesignModel.Date = date[0].ToString();
                    hardDesignModel.FullDate = DateString(hardDesignModel.Date.Substring(2));
                    hardDesignModel.DateCompare = hardDesignModel.Date.Substring(2);
                }
                catch (Exception ex)
                {
                    NtsLog.LogError(ex);
                }
            }

            var size = barcodes.ToArray().Where(o => o.ToString().StartsWith("2") && o.ToString().Length > 2).ToArray();
            if (size != null && size.Count() > 0)
            {
                hardDesignModel.Size = size[0].ToString();
                hardDesignModel.SizeCompare = hardDesignModel.Size.Substring(2);
            }

            hardDesignModel.ModuleCode = moduleCode;
            hardDesignModel.FilePath = filePath;
            hardDesignModel.IsExistName = false;
            hardDesignModel.IsDifferentDate = false;
            hardDesignModel.IsDifferentSize = false;

            stopwatch.Stop();
            Debug.WriteLine($" Thời gian xử lý file: {fileName} là: {stopwatch.ElapsedMilliseconds} ms");

            return hardDesignModel;
        }


        /// <summary>
        /// Tab Chuẩn danh mục vật tư
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<MaterialModel> LoadDMVT(string filePath)
        {
            string createBy = string.Empty;

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.OpenReadOnly(filePath);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<MaterialModel> listMaterial = new List<MaterialModel>();
            MaterialModel materialModel;
            createBy = sheet[4, 3].Value;
            //string Name, Specification, Code, RawMaterialCode, DV, SL, VL, KL, Manufacture, Note, MaterialGroupName, MaterialGroupCode;
            try
            {
                for (var i = 7; i < rowCount; i++)
                {
                    materialModel = new MaterialModel();
                    materialModel.Stt = sheet[i, 1].Value;
                    materialModel.Name = sheet[i, 2].Value;
                    materialModel.Specification = sheet[i, 3].Value;
                    materialModel.Code = sheet[i, 4].Value;
                    materialModel.RawMaterialCode = sheet[i, 5].Value;
                    materialModel.DV = sheet[i, 6].Value;
                    materialModel.SL = sheet[i, 7].Value;
                    materialModel.VL = sheet[i, 8].Value;
                    materialModel.KL = sheet[i, 9].Value;
                    materialModel.ManufactureCode = sheet[i, 10].Value;
                    materialModel.ManufactureName = sheet[i, 10].Value;
                    materialModel.Note = sheet[i, 11].Value;
                    materialModel.MaterialGroupName = sheet[i, 12].Value;
                    materialModel.MaterialGroupCode = sheet[i, 13].Value;

                    if (!string.IsNullOrEmpty(materialModel.Name))
                    {
                        if (!materialModel.Code.Contains("TPA") && !materialModel.Code.Contains("COC") && !materialModel.Code.Contains("ISO") && !materialModel.Code.Contains("BOX"))
                        {
                            listMaterial.Add(materialModel);
                        }
                    }
                }

                workbook.Close();
                excelEngine.Dispose();
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ex;
            }

            if (listMaterial.Count > 0)
            {
                listMaterial[0].CreateBy = createBy;
            }
            return listMaterial;
        }

        /// <summary>
        /// Tab Cấu trúc thiết kế
        /// </summary>
        public object CheckStructuralDesign(TestDesignStructureModel modelTest)
        {
            try
            {
                List<MaterialModel> listDMVT = new List<MaterialModel>();
                listDMVT = LoadDMVT(modelTest.PathFileMaterial);
                if (listDMVT.Count() == 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0012, "");
                }
                List<ErrorDesignStructureModel> listError = new List<ErrorDesignStructureModel>();
                string pathLocal = modelTest.SelectedPath + "\\3D." + modelTest.ModuleCode;
                //string _pathThuVien3D = "";// chưa có

                List<MaterialModel> _listMaterialSaiKichThuoc = new List<MaterialModel>();
                List<MaterialModel> _listMaterialNotUse = new List<MaterialModel>();
                List<string> listErrorMain = new List<string>();

                System.IO.DirectoryInfo dirLocal = new System.IO.DirectoryInfo(pathLocal);
                //System.IO.DirectoryInfo dir3D = new System.IO.DirectoryInfo(_pathThuVien3D);
                FileInfo[] listLocal = dirLocal.GetFiles("*.*", System.IO.SearchOption.AllDirectories);//Lấy danh sách các file trên thư viện 3D máy cá nhân

                if (listLocal.Length == 0)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0002);
                }

                modelTest.List3D = apiUtil.GetListDesign3D(modelTest.ApiUrl, modelTest.Token).Data;
                modelTest.ListMaterialDB = apiUtil.GetListMaterial(modelTest.ApiUrl, modelTest.Token).Data;
                modelTest.ListModuleDesignDocument = apiUtil.GetModuleDesignDocument(modelTest.ApiUrl, modelTest.Token).Data;
                modelTest.ListRawMaterial = apiUtil.GetRawMaterial(modelTest.ApiUrl, modelTest.Token).Data;
                modelTest.ListModuleError = apiUtil.GetErrorModuleNotDone(modelTest.ApiUrl, modelTest.Token).Data;
                modelTest.ListConvertUnit = apiUtil.GetConvertUnit(modelTest.ApiUrl, modelTest.Token).Data;
                modelTest.ListDesignStructure = apiUtil.GetDesignStructure(modelTest.ApiUrl, modelTest.Token).Data;
                modelTest.ListDesignStructureFile = apiUtil.GetDesignStructureFile(modelTest.ApiUrl, modelTest.Token).Data;
                //Lấy danh sách cấu hình
                //TODO : Fix cứng trong bảng ConfigSystem
                List<string> contentNotCheck = new List<string>();
                contentNotCheck.Add("PCB");
                contentNotCheck.Add("CAB");
                List<string> listVatLieuDMVT = new List<string>();
                listVatLieuDMVT.Add("AL6061");
                listVatLieuDMVT.Add("CT3");
                listVatLieuDMVT.Add("PVC");
                listVatLieuDMVT.Add("PVC-U");

                #region Kiểm tra thư mục 3D
                ErrorDesignStructureModel errorDesignStructureModel;
                foreach (FileInfo fi in listLocal)
                {
                    if (fi.FullName.Contains("lockfile") || fi.FullName.Contains("OldVersions") || Path.GetExtension(fi.FullName).ToLower() == ".idw" || Path.GetExtension(fi.FullName).ToLower() == ".ipj" || Path.GetExtension(fi.FullName).ToLower() == ".iam")
                    {
                        continue;
                    }

                    string name = Path.GetFileNameWithoutExtension(fi.Name);
                    string nameVT = "";
                    string serverPath = "";
                    string hang = "";

                    if (fi.Extension.ToLower() != ".ipt")
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = name;
                        errorDesignStructureModel.NameVT = nameVT;
                        errorDesignStructureModel.Hang = hang;
                        errorDesignStructureModel.Size = fi.Length;
                        errorDesignStructureModel.PathLocal = fi.FullName;
                        errorDesignStructureModel.Path3D = serverPath;
                        errorDesignStructureModel.Type = "Thừa";

                        listError.Add(errorDesignStructureModel);
                        continue;
                    }

                    var dtMaterialCSDL = modelTest.List3D.Where(a => a.FileName.Equals(fi.Name)).ToList();

                    if (dtMaterialCSDL.Count() > 0)
                    {
                        serverPath = dtMaterialCSDL[0].FilePath;
                    }

                    #region Những vật tư không cần kiểm tra
                    bool dk = false;
                    if (contentNotCheck != null)
                    {
                        foreach (string a in contentNotCheck)
                        {
                            if (a == null || a.Trim() == "")
                            {
                                continue;
                            }
                            if (a == contentNotCheck[0])
                            {
                                dk = System.Convert.ToBoolean(name.ToUpper().StartsWith(a));
                            }
                            dk = dk || name.ToUpper().StartsWith(a);
                            if (dk)
                            {
                                break;
                            }
                        }
                    }

                    if (dk)
                    {
                        continue;
                    }
                    #endregion

                    char[] sp = { '\\' };
                    string[] split = fi.FullName.Split(sp);
                    hang = split[split.Length - 2];
                    if (hang.Contains("3D.") || hang.StartsWith("TPA"))
                    {
                        hang = "";
                    }

                    int count = 0;
                    //string filterString = "F4 = '" + name.Replace(" ", "") + "' or F4 = '" + name.Replace(" ", "").Replace(")", "/").Replace("''", "''''") + "'";
                    List<MaterialModel> drsDMVT = new List<MaterialModel>();
                    try
                    {
                        drsDMVT = listDMVT.Where(a => a.Code.Equals(name) || a.Code.Equals(name.Replace(" ", "").Replace(")", "/").Replace("''", "''''"))).ToList();
                    }
                    catch (Exception ex)
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = name;
                        errorDesignStructureModel.NameVT = nameVT;
                        errorDesignStructureModel.Hang = hang;
                        errorDesignStructureModel.Size = fi.Length;
                        errorDesignStructureModel.PathLocal = fi.FullName;
                        errorDesignStructureModel.Path3D = serverPath;
                        errorDesignStructureModel.Type = "Thừa";

                        listError.Add(errorDesignStructureModel);
                        continue;
                    }

                    if (drsDMVT.Count() > 0)
                    {
                        nameVT = drsDMVT[0].Name; // TextUtils.ToString(drsDMVT[0]["F2"]);
                    }
                    count = drsDMVT.Count();
                    if (count > 0)
                    {
                        #region Kiểm tra thư viện 3D
                        if (dtMaterialCSDL.Count() > 0)
                        {
                            if (fi.Length != dtMaterialCSDL[0].Size)
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = name;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.Hang = hang;
                                errorDesignStructureModel.Size = fi.Length;
                                errorDesignStructureModel.PathLocal = fi.FullName;
                                errorDesignStructureModel.Path3D = serverPath;
                                errorDesignStructureModel.Type = "Khác size";
                                listError.Add(errorDesignStructureModel);
                            }

                            string materialCode = Path.GetFileNameWithoutExtension(dtMaterialCSDL[0].FileName.ToString()).Replace(")", "/");
                            MaterialModel model = null;
                            try
                            {
                                model = modelTest.ListMaterialDB.Where(c => c.Code.Equals(materialCode)).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }
                            if (model != null)
                            {
                                if (model.Code.Equals("TPAVT.Z02"))//TPAVT.Z02, Id=282 trong db cũ
                                {
                                    _listMaterialSaiKichThuoc.Add(model);
                                }
                            }
                        }
                        else
                        {
                            if (hang != "")
                            {
                                if (!name.StartsWith("TPA"))
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = name;
                                    errorDesignStructureModel.NameVT = nameVT;
                                    errorDesignStructureModel.Hang = hang;
                                    errorDesignStructureModel.Size = fi.Length;
                                    errorDesignStructureModel.PathLocal = fi.FullName;
                                    errorDesignStructureModel.Path3D = serverPath;
                                    errorDesignStructureModel.Type = "Không có trong thư viện 3D";
                                    listError.Add(errorDesignStructureModel);
                                    //type = "Không có trong thư viện 3D";
                                }
                            }
                        }

                        if (!name.ToLower().StartsWith(modelTest.ModuleCode.ToLower()))
                        {
                            string error = "";
                            if (!fi.FullName.Contains("COM." + modelTest.ModuleCode.ToUpper()))
                            {
                                error = "File không đúng địa chỉ";
                            }
                            else
                            {
                                if (!name.ToUpper().StartsWith("TPA"))
                                {
                                    if (Path.GetFileName(Path.GetDirectoryName(fi.FullName)).ToLower() != hang.ToLower())
                                    {
                                        error = "File không đúng địa chỉ";
                                    }
                                }
                                else
                                {
                                    if (!fi.FullName.Contains("\\TPA\\"))
                                    {
                                        error = "File không đúng địa chỉ";
                                    }
                                }
                            }

                            if (error != "")
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = name;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.Hang = hang;
                                errorDesignStructureModel.Size = fi.Length;
                                errorDesignStructureModel.PathLocal = fi.FullName;
                                errorDesignStructureModel.Path3D = serverPath;
                                errorDesignStructureModel.Type = error;
                                listError.Add(errorDesignStructureModel);
                            }
                        }
                        else //Kiểm tra thiết kế 3D trên nguồn với trên thiết kế
                        {
                            var folderName = Path.GetFileName(modelTest.SelectedPath);
                            string ftpFilePath = fi.FullName.Replace(modelTest.SelectedPath, folderName + "/");
                            foreach (var item in modelTest.ListModuleDesignDocument)
                            {
                                if (item.Path.Equals(ftpFilePath))
                                {
                                    if (item.FileSize != fi.Length)
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = name;
                                        errorDesignStructureModel.PathLocal = fi.FullName;
                                        errorDesignStructureModel.Path3D = ftpFilePath;
                                        errorDesignStructureModel.Type = "Khác size với thiết kế cũ trên nguồn";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (hang != "TPA")
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = name;
                            errorDesignStructureModel.NameVT = nameVT;
                            errorDesignStructureModel.Hang = hang;
                            errorDesignStructureModel.Size = fi.Length;
                            errorDesignStructureModel.PathLocal = fi.FullName;
                            errorDesignStructureModel.Path3D = serverPath;
                            errorDesignStructureModel.Type = "THỪA";
                            listError.Add(errorDesignStructureModel);
                        }
                    }
                    #endregion
                }
                #region Kiểm tra danh mục vật tư
                for (int i = 0; i < listDMVT.Count(); i++)
                {
                    string codeVT = listDMVT[i].Code;
                    if (codeVT == "") continue;
                    string nameVT = listDMVT[i].Name.Trim();
                    string stt = listDMVT[i].Stt;
                    string unit = listDMVT[i].DV;
                    string sourceCode = listDMVT[i].RawMaterialCode;
                    string vatLieu = listDMVT[i].VL;
                    string khoiLuong = listDMVT[i].KL;
                    string hang = listDMVT[i].ManufactureName;

                    string thongSo = listDMVT[i].Specification;
                    int parent = stt.Split('.').Count();

                    string comPath1 = string.Format(pathLocal + "\\COM.{0}\\", modelTest.ModuleCode);
                    if (codeVT.ToUpper() == modelTest.ModuleCode.ToUpper())
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = codeVT;
                        errorDesignStructureModel.NameVT = nameVT;
                        errorDesignStructureModel.PathLocal = "Trong DMVT";
                        errorDesignStructureModel.Type = "Vật tư không được cùng mã với module";
                        listError.Add(errorDesignStructureModel);
                        continue;
                    }

                    #region Kiểm tra vật liệu
                    if (vatLieu != "")
                    {
                        if (vatLieu != "-")
                        {
                            List<RawMaterialModel> dtVatLieu = modelTest.ListRawMaterial.Where(b => b.Code.Equals(vatLieu)).ToList();
                            if (dtVatLieu.Count() == 0)
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = codeVT;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.PathLocal = "Trong DMVT";
                                errorDesignStructureModel.Type = "Vật liệu không tồn tại";
                                listError.Add(errorDesignStructureModel);
                            }
                        }
                    }
                    else
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = codeVT;
                        errorDesignStructureModel.NameVT = nameVT;
                        errorDesignStructureModel.PathLocal = "Trong DMVT";
                        errorDesignStructureModel.Type = "Vật liệu không tồn tại";
                        listError.Add(errorDesignStructureModel);
                    }
                    #endregion

                    #region Kiểm tra thông số hàng hãng
                    if (hang.ToUpper() != "TPA" && thongSo != "")
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = codeVT;
                        errorDesignStructureModel.NameVT = nameVT;
                        errorDesignStructureModel.PathLocal = "Trong DMVT";
                        errorDesignStructureModel.Type = "Thông số sai";
                        listError.Add(errorDesignStructureModel);
                    }
                    #endregion

                    if (unit == "BỘ")
                    {
                        if (!codeVT.StartsWith("TPA") && !codeVT.StartsWith("PCB."))
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = codeVT;
                            errorDesignStructureModel.NameVT = nameVT;
                            errorDesignStructureModel.PathLocal = "Trong DMVT";
                            errorDesignStructureModel.Type = "Vật tư sai đơn vị";
                            listError.Add(errorDesignStructureModel);
                        }

                        #region Kiểm tra vật tư đơn vị là Bộ
                        List<MaterialModel> drs = listDMVT.Where(d => d.Stt.Contains(stt) && d.Code.Contains(modelTest.ModuleCode)).ToList();
                        if (drs.Count() > 0)
                        {
                            foreach (var r in drs)
                            {
                                string sttChild = r.Stt;
                                string[] splitFileName = sttChild.Split('.');
                                int child = splitFileName.Count();
                                if (child - parent != 1) continue;
                                string unitChild = r.DV;
                                if (unitChild == "BỘ") continue;
                                string materialCode = r.Code;
                                string materialName = r.Name;
                                if (materialCode.StartsWith(codeVT))
                                {
                                    string[] splitMaterialCode = materialCode.Split('.');
                                    string thisFilePath = pathLocal + "\\";
                                    string ne = modelTest.ModuleCode;
                                    for (int ii = 2; ii < splitMaterialCode.Length; ii++)
                                    {
                                        ne += "." + splitMaterialCode[ii];
                                        if (ii == splitMaterialCode.Length - 1)
                                        {
                                            thisFilePath += ne;
                                        }
                                        else
                                        {
                                            thisFilePath += ne + "\\";
                                        }
                                    }
                                    thisFilePath += ".ipt";
                                    if (!File.Exists(thisFilePath))
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = materialCode;
                                        errorDesignStructureModel.NameVT = materialName;
                                        errorDesignStructureModel.PathLocal = thisFilePath;
                                        errorDesignStructureModel.Type = "Vật tư không có tài liệu 3D";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (unit == "CÁI")
                        {
                            if (codeVT.StartsWith("PCB.") || (codeVT.StartsWith("TPA") && codeVT.Length == 10))
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = codeVT;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.PathLocal = "Trong DMVT";
                                errorDesignStructureModel.Type = "Vật tư sai đơn vị";
                                listError.Add(errorDesignStructureModel);
                            }

                            #region Kiểm tra vật tư thường được sử dụng
                            List<MaterialModel> dtViewMaterial = modelTest.ListMaterialDB.Where(a => a.Code.Replace(" ", "").Replace(")", "/").Equals(codeVT.Replace(" ", "").Replace(")", "/"))).ToList();
                            if (dtViewMaterial.Count() > 0)
                            {
                                if (!dtViewMaterial[0].IsUsuallyUse)
                                {
                                    _listMaterialNotUse.Add(dtViewMaterial[0]);
                                }
                            }
                            #endregion
                            if (codeVT.StartsWith(modelTest.ModuleCode) && parent == 1)
                            {
                                string thisFilePath = pathLocal + "\\";
                                if (!File.Exists(thisFilePath + codeVT + ".ipt"))
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = codeVT;
                                    errorDesignStructureModel.NameVT = nameVT;
                                    errorDesignStructureModel.PathLocal = "Trong DMVT";
                                    errorDesignStructureModel.Type = "Vật tư không có tài liệu 3D";
                                    listError.Add(errorDesignStructureModel);

                                }
                            }
                        }
                        else
                        {
                            if (unit != "M")
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = codeVT;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.PathLocal = "Trong DMVT";
                                errorDesignStructureModel.Type = "Vật tư sai đơn vị";
                                listError.Add(errorDesignStructureModel);
                            }
                        }

                        if (!Directory.Exists(comPath1 + hang))
                        {
                            #region Kiểm tra sự tồn tại của thư mục hãng trong thiết kế
                            if (!codeVT.StartsWith("TPA") && !codeVT.StartsWith("PCB"))
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = hang;
                                errorDesignStructureModel.PathLocal = comPath1;
                                errorDesignStructureModel.Type = "Không tồn tại hãng này trong thiết kế";
                                listError.Add(errorDesignStructureModel);
                            }
                            else if (codeVT.StartsWith("TPA"))
                            {
                                if (codeVT.Substring(0, 10) != modelTest.ModuleCode)
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = hang;
                                    errorDesignStructureModel.PathLocal = comPath1;
                                    errorDesignStructureModel.Type = "Không tồn tại hãng này trong thiết kế";
                                    listError.Add(errorDesignStructureModel);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (!codeVT.StartsWith("TPA") && !codeVT.StartsWith("PCB"))
                            {
                                //Kiểm tra xem vật tư có tồn tại trong thư mục hãng không
                                System.IO.DirectoryInfo dirComFile = new System.IO.DirectoryInfo(comPath1 + hang);
                                FileInfo[] listFileInHang = dirComFile.GetFiles("*.ipt*", SearchOption.TopDirectoryOnly);
                                int count = 0;
                                try
                                {
                                    count = listFileInHang.Count(o => Path.GetFileNameWithoutExtension(o.Name)
                                        .Replace(" ", "").Replace(")", "/")
                                        == codeVT.Replace(" ", "").Replace(")", "/"));
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }

                                if (count == 0)
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = codeVT;
                                    errorDesignStructureModel.NameVT = nameVT;
                                    errorDesignStructureModel.Hang = hang;
                                    errorDesignStructureModel.Type = "Vật tư không có tài liệu 3D";
                                    listError.Add(errorDesignStructureModel);
                                }
                                else
                                {
                                    #region Kiem tra hang co hop le
                                    var drsCustomer = modelTest.ListMaterialDB.Where(a => a.ManufactureCode.Equals(hang)).FirstOrDefault();
                                    if (drsCustomer == null)
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = codeVT;
                                        errorDesignStructureModel.NameVT = nameVT;
                                        errorDesignStructureModel.Hang = hang;
                                        errorDesignStructureModel.Type = "Hãng không được sử dụng";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                    #endregion
                                }

                                #region Vật tư dừng sử dụng
                                decimal currentQty = decimal.Parse(listDMVT[i].SL);
                                var dtViewMaterial = modelTest.ListMaterialDB.Where(a => a.Code.Replace(" ", "").Replace(")", "/").Equals(codeVT.Replace(" ", "").Replace(")", "/"))).FirstOrDefault();
                                if (dtViewMaterial != null)
                                {
                                    if (dtViewMaterial.MaterialGroupCode.Equals("TPAVT.Z01"))
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = codeVT;
                                        errorDesignStructureModel.NameVT = nameVT;
                                        errorDesignStructureModel.Hang = hang;
                                        errorDesignStructureModel.Type = "Vật tư ngừng sử dụng";
                                        listError.Add(errorDesignStructureModel);
                                        listErrorMain.Add(codeVT + " - " + errorDesignStructureModel.Type + ", ");
                                    }
                                    //Kiểm tra sự tạm dừng của vật tư
                                    if (dtViewMaterial.Status.Equals("2"))
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = codeVT;
                                        errorDesignStructureModel.NameVT = nameVT;
                                        errorDesignStructureModel.Hang = hang;
                                        errorDesignStructureModel.Type = "Vật tư tạm dừng sử dụng";
                                        listError.Add(errorDesignStructureModel);
                                        listErrorMain.Add(codeVT + " - " + errorDesignStructureModel.Type + ", ");
                                    }
                                }
                                #endregion

                                #region Kiểm tra trên quản lý sản xuất
                                if (dtViewMaterial == null)
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = codeVT;
                                    errorDesignStructureModel.NameVT = nameVT;
                                    errorDesignStructureModel.Hang = hang;
                                    errorDesignStructureModel.Type = "Vật tư không tồn tại trên QLSX";
                                    listError.Add(errorDesignStructureModel);
                                }
                                else
                                {
                                    if (hang.Equals(dtViewMaterial.ManufactureCode))
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = codeVT;
                                        errorDesignStructureModel.NameVT = nameVT;
                                        errorDesignStructureModel.Hang = hang;
                                        errorDesignStructureModel.Type = "Hãng khác với hãng trên QLSX (" + hang + " - " + dtViewMaterial.MaterialGroupCode + ")";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                #region Kiểm tra các module con
                                if (codeVT.StartsWith("TPA") && codeVT.Length == 10 && codeVT != modelTest.ModuleCode)
                                {
                                    string moduleCode = codeVT;
                                    var listModuleError = modelTest.ListModuleError.Where(a => a.ModuleErrorVisualCode.Equals(moduleCode)).ToList();
                                    //TODO
                                    //DataTable dtKPH = TextUtils.Select(string.Format("select * from [vMisMatch] where [ModuleCode]='{0}' StatusKCS = 0 and ConfirmTemp = 0", moduleCode));
                                    if (listModuleError.Count() > 0)
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = codeVT;
                                        errorDesignStructureModel.NameVT = nameVT;
                                        errorDesignStructureModel.Hang = hang;
                                        errorDesignStructureModel.Type = "Có " + listModuleError.Count() + " lỗi";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                    //Kiểm tra sự tồn tại trong thiết kế
                                    if (!File.Exists(comPath1 + "TPA\\" + codeVT + ".ipt"))
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = codeVT;
                                        errorDesignStructureModel.NameVT = nameVT;
                                        errorDesignStructureModel.Hang = hang;
                                        errorDesignStructureModel.Type = "Vật tư không có tài liệu 3D";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                }
                                #endregion
                            }
                        }

                        #region Kiểm tra vật tư nguồn
                        if (sourceCode != "")
                        {
                            var dtMaterialQLSX_VTN = modelTest.ListMaterialDB.Where(a => a.Code.Replace(" ", "").Replace(")", "/").Equals(sourceCode.Replace(" ", "").Replace(")", "/"))).ToList();
                            if (dtMaterialQLSX_VTN.Count() == 0)
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = codeVT;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.Hang = hang;
                                errorDesignStructureModel.Type = "Vật tư nguồn (" + sourceCode + ") không tồn tại trên QLSX";
                                listError.Add(errorDesignStructureModel);
                                listErrorMain.Add(codeVT + " - " + errorDesignStructureModel.Type + ", ");
                            }
                            else
                            {
                                var materialConverUnit = modelTest.ListConvertUnit.Where(a => a.MaterialCode.Replace(" ", "").Replace(")", "/").Equals(sourceCode.Replace(" ", "").Replace(")", "/"))).ToList();
                                if (materialConverUnit.Count() == 0)
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = codeVT;
                                    errorDesignStructureModel.NameVT = nameVT;
                                    errorDesignStructureModel.Hang = hang;
                                    errorDesignStructureModel.Type = "Vật tư nguồn (" + sourceCode + ") chưa được chuyển đổi đơn vị";
                                    listError.Add(errorDesignStructureModel);
                                    listErrorMain.Add(codeVT + " - " + errorDesignStructureModel.Type + ", ");
                                }
                            }

                            if (vatLieu == "" || vatLieu == "-")
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = codeVT;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.Hang = hang;
                                errorDesignStructureModel.Type = "Vật liệu không có giá trị trong DMVT";
                                listError.Add(errorDesignStructureModel);
                            }
                            if (khoiLuong == "" || khoiLuong == "-")
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = codeVT;
                                errorDesignStructureModel.NameVT = nameVT;
                                errorDesignStructureModel.Hang = hang;
                                errorDesignStructureModel.Type = "Khối lượng không có giá trị trong DMVT";
                                listError.Add(errorDesignStructureModel);
                            }
                        }

                        if (thongSo == "TPA" && codeVT.StartsWith("TPA") && hang == "TPA")
                        {
                            if (listVatLieuDMVT.Contains(vatLieu))
                            {
                                if (sourceCode == "")
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = codeVT;
                                    errorDesignStructureModel.NameVT = nameVT;
                                    errorDesignStructureModel.Hang = hang;
                                    errorDesignStructureModel.Type = "Vật liệu nguồn không thể để trống";
                                    listError.Add(errorDesignStructureModel);
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion
                //check cum QLSX
                listError.AddRange(CheckCumQLSX(pathLocal, modelTest));
                listError.AddRange(CheckCTTK(modelTest, listDMVT));

                #region Kiểm tra thông số kỹ thuật
                try
                {
                    if (modelTest.Module[0].Specification.Length <= 20)
                    {
                        string filePathTSKT = string.Format("D:/Thietke.Ck/{0}/{1}.Ck/DOC.{1}/TSKT.{1}.docx", modelTest.ModuleGroupCode, modelTest.ModuleCode);
                        if (!File.Exists(filePathTSKT))
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = Path.GetFileName(filePathTSKT); ;
                            errorDesignStructureModel.PathLocal = Path.GetDirectoryName(filePathTSKT);
                            errorDesignStructureModel.Type = "THIẾU FILE";
                            listError.Add(errorDesignStructureModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    NtsLog.LogError(ex);
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, "Module");
                }
                #endregion Kiểm tra thông số kỹ thuật

                #region Kiểm tra file 3D TPA dùng chung
                string tpaPath = string.Format("D:\\Thietke.Ck\\{0}\\{1}.Ck\\3D.{1}\\COM.{1}\\TPA", modelTest.ModuleGroupCode, modelTest.ModuleCode);
                if (Directory.Exists(tpaPath))
                {
                    string[] listTPA_File = Directory.GetFiles(tpaPath, "TPAD.*", SearchOption.AllDirectories);
                    foreach (string filePath in listTPA_File)
                    {
                        if (filePath.Contains("OldVersions")) continue;
                        FileInfo fInfo = new FileInfo(filePath);//OldVersions
                        if (fInfo.Extension != ".ipt") continue;
                        string fileName = Path.GetFileNameWithoutExtension(fInfo.Name);
                        var drs = listDMVT.Where(a => a.Code.Equals(fileName.Replace(" ", "").Replace(")", "/") + "'")).ToList();
                        if (drs.Count() == 0)
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = fInfo.Name;
                            errorDesignStructureModel.Size = fInfo.Length;
                            errorDesignStructureModel.PathLocal = fInfo.FullName;
                            errorDesignStructureModel.Type = "THỪA";
                            errorDesignStructureModel.Path3D = "";
                            continue;
                        }
                        string thisProductCode = fileName.Substring(0, 10);
                        string thisServerPath = string.Format("Thietke.Ck\\{0}\\{1}.Ck\\3D.{1}\\",
                              modelTest.ModuleGroupCode, thisProductCode);
                        string[] splitFileName = fileName.Split('.');
                        if (splitFileName.Length == 2)//vd: tpad.a0001.ipt
                        {
                            continue;
                        }
                        if (splitFileName.Length == 3)
                        {
                            thisServerPath += fInfo.Name;
                        }
                        if (splitFileName.Length > 3)
                        {
                            string ne = thisProductCode;
                            for (int i = 2; i < splitFileName.Length; i++)
                            {
                                if (i == splitFileName.Length - 1)
                                {
                                    thisServerPath += fInfo.Name;
                                }
                                else
                                {
                                    ne += "." + splitFileName[i];
                                    thisServerPath += ne + "\\";
                                }
                            }
                        }

                        ResultApiModel resultApiModel = apiUtil.CheckFileServer(modelTest.ApiUrl, thisServerPath, thisProductCode, fInfo.Length, modelTest.Token);

                        if (!resultApiModel.SuccessStatus)
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = fInfo.Name;
                            errorDesignStructureModel.Size = fInfo.Length;
                            errorDesignStructureModel.PathLocal = fInfo.FullName;
                            errorDesignStructureModel.Type = "FILE TPA DÙNG CHUNG KHÔNG CÓ TRÊN NGUỒN";
                            listError.Add(errorDesignStructureModel);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(resultApiModel.Message))
                            {
                                List<MaterialModel> dtVT_Server = new List<MaterialModel>();

                                var drsOld = dtVT_Server.Where(a => a.Code.Equals(Path.GetFileNameWithoutExtension(fInfo.Name))).ToList();//("F4 = '" + Path.GetFileNameWithoutExtension(fInfo.Name) + "'");
                                var drsNew = listDMVT.Where(a => a.Code.Equals(Path.GetFileNameWithoutExtension(fInfo.Name))).ToList();

                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = fInfo.Name;
                                errorDesignStructureModel.Size = fInfo.Length;
                                errorDesignStructureModel.PathLocal = fInfo.FullName;
                                errorDesignStructureModel.Type = resultApiModel.Message;
                                errorDesignStructureModel.Path3D = thisServerPath;
                                errorDesignStructureModel.KLOld = drsOld.Count() > 0 ? drsOld[0].KL : "";
                                errorDesignStructureModel.KLNew = drsNew.Count() > 0 ? drsNew[0].KL : "";
                                listError.Add(errorDesignStructureModel);
                            }
                        }
                    }
                }
                #endregion Kiểm tra file 3D TPA dùng chung

                #region Kiem tra ten hang
                string comPath = string.Format("D:\\Thietke.Ck\\{0}\\{1}.Ck\\3D.{1}\\COM.{1}\\",
                            modelTest.ModuleGroupCode, modelTest.ModuleCode);
                if (Directory.Exists(comPath))
                {
                    string[] listFolderHang = Directory.GetDirectories(comPath, "*", SearchOption.TopDirectoryOnly);
                    var dtDMVT_Hang = listDMVT.Select(a => a.ManufactureName).ToList();
                    foreach (string item in listFolderHang)
                    {
                        string tenHang = Path.GetFileName(item);
                        int count = 0;
                        count = dtDMVT_Hang.Where(b => b.Equals(tenHang)).Count();
                        if (count == 0)
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = tenHang;
                            errorDesignStructureModel.PathLocal = item;
                            errorDesignStructureModel.Type = "Không tồn tại hãng này trong danh mục vật tư";
                            listError.Add(errorDesignStructureModel);
                        }
                        if (tenHang.Equals("TPA"))
                        {
                            string[] listChildPath = Directory.GetDirectories(item, "*", SearchOption.TopDirectoryOnly);
                            foreach (string childPath in listChildPath)
                            {
                                string childName = Path.GetFileName(childPath);
                                if (childName == "OldVersions") continue;
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = childName;
                                errorDesignStructureModel.PathLocal = childPath;
                                errorDesignStructureModel.Type = "Folder THỪA";
                                listError.Add(errorDesignStructureModel);
                            }
                        }
                    }
                }
                #endregion
                var datatbleCT = listError.Where(a => !string.IsNullOrEmpty(a.Type)).OrderBy(b => b.Hang).ToList();
                string htmlText = ShowSuccessReport(listDMVT, modelTest, _listMaterialSaiKichThuoc, _listMaterialNotUse);
                return new { listError, listErrorMain, datatbleCT, htmlText };
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }
        }

        public string ShowSuccessReport(List<MaterialModel> listDMV, TestDesignStructureModel modelTest, List<MaterialModel> _listMaterialSaiKichThuoc, List<MaterialModel> _listMaterialNotUse)
        {
            try
            {
                string htmlText = $"<html><header></header><body><div style=\"width: 29cm; height: 20cm; \"><div style=\"margin - left:6cm; margin - top:3cm; \"><img src=\"<path>\" alt=\"logo\"></div><br><br><div style=\"text - align:center; font - size:22px; font - style:bold; \"><b>BÁO CÁO CẤU TRÚC THIẾT KẾ</b></div><br><br><table style=\"width: 100 %; margin - left:5cm; font - size:20px\"><THEAD><tr style=\"height: 1.5cm\"><td width=\"17 % \" >Tên sản phẩm:</td><td><productname></td></tr></THEAD><tr style=\"height: 1.5cm\"><td>Mã sản phẩm:</td><td><productcode></td></tr><tr style=\"height: 1.5cm\"><td>Nhân viên thiết kế:</td><td><username></td></tr><tr style=\"height: 1.5cm\"><td>Thời gian thiết kế:</td><td><datedesign></td></tr><tr style=\"height: 1.5cm\"><td>Tình trạng kiểm tra:</td><td><b>ĐÃ CHUẨN</b></td></tr><tr style=\"height: 1.5cm\"><td>Lỗi:</td><td><qtyError></td></tr><tr style=\"height: 1.5cm\"><td>Không phù hợp:</td><td><qtyKPH></td></tr></table><SaiKichThuoc><br><br><IsNotUse><br><br><div style=\"text - align:right; font - size:20px\">Tân Phát, ngày <day> tháng <month> năm <year></div></div></body></html>";
                htmlText = htmlText.Replace("<path>", "");
                htmlText = htmlText.Replace("<productname>", "Tên vật tư");
                htmlText = htmlText.Replace("<productcode>", modelTest.ModuleCode);
                htmlText = htmlText.Replace("<username>", listDMV[0].CreateBy);
                htmlText = htmlText.Replace("<datedesign>", DateTime.Now.ToString("dd/MM/yyyy"));
                htmlText = htmlText.Replace("<day>", DateTime.Now.Day.ToString());
                htmlText = htmlText.Replace("<month>", DateTime.Now.Month.ToString());
                htmlText = htmlText.Replace("<year>", DateTime.Now.Year.ToString());

                var moduleError = modelTest.ListModuleError.Where(a => modelTest.ModuleCode.Equals(a.ModuleErrorVisualCode)).AsQueryable();
                List<ErrorModel> listModuleError = new List<ErrorModel>();

                if (moduleError != null)
                {
                    listModuleError = moduleError.ToList();
                }
                //TODDO
                //DataTable dtKPH = TextUtils.Select(string.Format("select * from [vMisMatch] where [ModuleCode]='{0}' StatusKCS = 0 and ConfirmTemp = 0", moduleCode));
                htmlText = htmlText.Replace("<qtyError>", listModuleError.Count().ToString());
                htmlText = htmlText.Replace("<qtyKPH>", "");

                string saikichthuoc = $"<br><div style=\"margin - left:5cm; font - size:22px; \"><b>Danh sách vật tư sai kích thước</b></div><br><table style=\"width: 100 %; margin - left:5cm; font - size:20px\"  border=\"1\"><THEAD><tr><td style=\"width: 30 % \">Mã vật tư</td><td>Tên vật tư</td></tr></THEAD><items></table>";
                string saikichthuocItem = "<tr><td><MaterialCode></td><td><MaterialName></td></tr>";
                if (_listMaterialSaiKichThuoc.Count() > 0)
                {
                    string items = "";
                    foreach (MaterialModel item in _listMaterialSaiKichThuoc)
                    {
                        items += saikichthuocItem.Replace("<MaterialCode>", item.Code).Replace("<MaterialName>", item.Name);
                    }
                    saikichthuoc = saikichthuoc.Replace("<items>", items);
                }
                htmlText = htmlText.Replace("<SaiKichThuoc>", saikichthuoc);

                string isNotUse = $"<br><div style=\"margin - left:5cm; font - size:22px; \"><b>Danh sách vật tư không được sử dụng</b></div><br><table style=\"width: 100 %; margin - left:5cm; font - size:20px\"  border=\"1\"><THEAD><tr><td style=\"width: 30 % \">Mã vật tư</td><td>Tên vật tư</td></tr></THEAD><items></table>";
                if (_listMaterialNotUse.Count() > 0)
                {
                    string items = "";
                    foreach (MaterialModel item in _listMaterialNotUse)
                    {
                        items += saikichthuocItem.Replace("<MaterialCode>", item.Code).Replace("<MaterialName>", item.Name);
                    }
                    isNotUse = isNotUse.Replace("<items>", items);
                }

                htmlText = htmlText.Replace("<IsNotUse>", isNotUse);

                return htmlText;

            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }

        }

        public List<ErrorDesignStructureModel> CheckCumQLSX(string initPath, TestDesignStructureModel modelTest)
        {
            try
            {
                List<ErrorDesignStructureModel> listError = new List<ErrorDesignStructureModel>();
                string[] listDirectories = Directory.GetDirectories(initPath, modelTest.ModuleCode + "*", SearchOption.AllDirectories);
                foreach (string path in listDirectories)
                {
                    ErrorDesignStructureModel errorDesignStructureModel;
                    string[] listChild = Directory.GetDirectories(path, modelTest.ModuleCode + "*", SearchOption.TopDirectoryOnly);
                    string pathName = Path.GetFileName(path);
                    if (listChild.Length > 0) continue;
                    if (path.Contains("OldVersions")) continue;
                    if (path.Contains(initPath + "\\" + "COM." + modelTest.ModuleCode)) continue;
                    string[] listFileIPT = Directory.GetFiles(path, "*.ipt", SearchOption.TopDirectoryOnly);
                    string[] listFileIAM = Directory.GetFiles(path, "*.iam", SearchOption.TopDirectoryOnly);
                    if (listFileIAM.Length > 0 && listFileIPT.Length == 0)
                    {
                        if (listFileIAM.Length == 1)
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = Path.GetFileName(path);
                            errorDesignStructureModel.PathLocal = path;
                            errorDesignStructureModel.Type = "FILE iam sai thư mục chứa";
                            listError.Add(errorDesignStructureModel);
                        }
                        else
                        {
                            foreach (string fileiamPath in listFileIAM)
                            {
                                string fileNameIam = Path.GetFileName(fileiamPath);
                                if (!fileNameIam.StartsWith(pathName))
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = fileNameIam;
                                    errorDesignStructureModel.PathLocal = fileiamPath;
                                    errorDesignStructureModel.Type = "FILE iam sai thư mục chứa";
                                    listError.Add(errorDesignStructureModel);
                                }
                            }
                        }
                    }

                    if (listFileIAM.Length == 0 && listFileIPT.Length == 0)
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = Path.GetFileName(path);
                        errorDesignStructureModel.PathLocal = path;
                        errorDesignStructureModel.Type = "Folder THỪA";
                        listError.Add(errorDesignStructureModel);
                    }

                    if (listFileIPT.Length > 0)
                    {
                        foreach (string filePath in listFileIPT)
                        {
                            if (!Path.GetFileNameWithoutExtension(filePath).StartsWith(pathName))
                            {
                                errorDesignStructureModel = new ErrorDesignStructureModel();
                                errorDesignStructureModel.Name = Path.GetFileName(path);
                                errorDesignStructureModel.Size = new FileInfo(filePath).Length;
                                errorDesignStructureModel.PathLocal = path;
                                errorDesignStructureModel.Type = "File không đúng thư mục chứa";
                                listError.Add(errorDesignStructureModel);
                            }
                        }
                    }

                }

                return listError;
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }

        }

        public List<ErrorDesignStructureModel> CheckCTTK(TestDesignStructureModel modelTest, List<MaterialModel> listDMVT)
        {
            List<ErrorDesignStructureModel> listError = new List<ErrorDesignStructureModel>();
            try
            {
                ErrorDesignStructureModel errorDesignStructureModel;
                var dtCT = modelTest.ListDesignStructure.Where(a => a.Type == 0).ToList();
                string[] listDirectories = Directory.GetDirectories(modelTest.SelectedPath, "**", SearchOption.AllDirectories);
                foreach (string item in listDirectories)
                {
                    string folderName = Path.GetFileName(item);
                    if (item.Contains("COM." + modelTest.ModuleCode) || item.Contains("OldVersions") || item.Contains("DAT." + modelTest.ModuleCode)) continue;
                    if (item.Contains("3D." + modelTest.ModuleCode) && folderName != ("3D." + modelTest.ModuleCode))
                    {
                        if (!folderName.StartsWith(modelTest.ModuleCode))
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = folderName;
                            errorDesignStructureModel.PathLocal = item;
                            errorDesignStructureModel.Type = "Folder THỪA";
                            listError.Add(errorDesignStructureModel);
                        }
                    }
                    else
                    {
                        int count = 0;
                        foreach (var r in dtCT)
                        {
                            string formatFolder = r.Name.Replace("code", modelTest.ModuleCode);
                            if (folderName != formatFolder)
                            {
                                count = 1;
                            }
                            else
                            {
                                count = 0;
                                break;
                            }
                        }
                        if (count == 1)
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = folderName;
                            errorDesignStructureModel.PathLocal = item;
                            errorDesignStructureModel.Type = "Folder THỪA";
                            listError.Add(errorDesignStructureModel);
                        }
                    }
                }
                foreach (var itemRow in dtCT)
                {
                    string nameCT = itemRow.Name.Replace("code", modelTest.ModuleCode);//OldVersions
                    string id = itemRow.Id;
                    string[] arrExtension = null;
                    try
                    {
                        arrExtension = itemRow.Extension.Split(',').Where(o => o.Trim() != "").ToArray();
                    }
                    catch (Exception)
                    {
                    }

                    string folderPath = Path.GetDirectoryName(modelTest.SelectedPath) + "\\" + itemRow.Path.Replace("code", modelTest.ModuleCode);
                    if (!Directory.Exists(folderPath))
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = nameCT;
                        errorDesignStructureModel.PathLocal = folderPath;
                        errorDesignStructureModel.Type = "THIẾU THƯ MỤC";
                        listError.Add(errorDesignStructureModel);
                    }
                    else
                    {
                        var dtCTFile = modelTest.ListDesignStructureFile.Where(a => a.DesignStructureId.Equals(id)).ToList();
                        string[] listFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);

                        if (dtCTFile.Count() > 0)
                        {
                            foreach (var row in dtCTFile)
                            {
                                string fileNameCT = row.Name.Replace("code", modelTest.ModuleCode);
                                bool exist = row.Exist;
                                if (exist && !File.Exists(folderPath + "\\" + fileNameCT))
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = fileNameCT;
                                    errorDesignStructureModel.PathLocal = folderPath;
                                    errorDesignStructureModel.Type = "THIẾU FILE";
                                    listError.Add(errorDesignStructureModel);
                                }
                            }
                        }

                        if (listFiles.Count() > 0)
                        {
                            foreach (string item in listFiles)
                            {
                                string fileName = Path.GetFileName(item);
                                if (Path.GetExtension(fileName).ToLower() == ".ipt" || Path.GetExtension(fileName).ToLower() == ".lck") continue;
                                if (Path.GetExtension(fileName).ToLower() == ".iam") continue;//(sửa ngày 27/02/2015 không check file iam trong thư mục bất kỳ)
                                if (dtCTFile.Count() > 0)
                                {
                                    int count = 0;
                                    try { count = dtCTFile.Where(r => r.Name.Replace("code", modelTest.ModuleCode).Equals(fileName)).Count(); }
                                    catch { }
                                    if (count == 0)
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = fileName;
                                        errorDesignStructureModel.Size = new FileInfo(item).Length;
                                        errorDesignStructureModel.PathLocal = item;
                                        errorDesignStructureModel.Type = "THỪA";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                }

                                if (arrExtension != null && arrExtension.Count() > 0 && !arrExtension.Contains(Path.GetExtension(fileName)))
                                {
                                    errorDesignStructureModel = new ErrorDesignStructureModel();
                                    errorDesignStructureModel.Name = fileName;
                                    errorDesignStructureModel.Size = new FileInfo(item).Length;
                                    errorDesignStructureModel.PathLocal = item;
                                    errorDesignStructureModel.Type = "THỪA";
                                    listError.Add(errorDesignStructureModel);
                                }
                            }
                        }
                        #region Kiem tra thu muc mat
                        if (nameCT.StartsWith("MAT"))
                        {
                            string[] listFileMAT = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
                            if (listFileMAT.Count() > 0)
                            {
                                foreach (string item in listFileMAT)
                                {
                                    string fileName = Path.GetFileName(item);
                                    if (Path.GetExtension(fileName).ToUpper() != ".DWG")
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = fileName;
                                        errorDesignStructureModel.Size = new FileInfo(item).Length;
                                        errorDesignStructureModel.PathLocal = item;
                                        errorDesignStructureModel.Type = "THỪA";
                                        listError.Add(errorDesignStructureModel);
                                        continue;
                                    }
                                    if (!fileName.StartsWith("TPAD") && (!fileName.Contains("-IN") || !fileName.Contains("-KHAC")) && Path.GetExtension(fileName).ToLower() == ".dwg")
                                    {
                                        errorDesignStructureModel = new ErrorDesignStructureModel();
                                        errorDesignStructureModel.Name = fileName;
                                        errorDesignStructureModel.Size = new FileInfo(item).Length;
                                        errorDesignStructureModel.PathLocal = item;
                                        errorDesignStructureModel.Type = "File mặt không đúng định dạng";
                                        listError.Add(errorDesignStructureModel);
                                    }
                                    else
                                    {
                                        string part = "";
                                        string fileNameNoEx = Path.GetFileNameWithoutExtension(item);
                                        if (fileName.Contains("-IN"))
                                        {
                                            part = fileNameNoEx.Substring(0, fileNameNoEx.Length - 3);
                                        }
                                        else
                                        {
                                            part = fileNameNoEx.Substring(0, fileNameNoEx.Length - 5);
                                        }

                                        // DataRow[] drs = dtDMVT.Select("F4 = '" + part + "' or F4 like '" + part + "-%'");
                                        var drs = listDMVT.Where(a => a.Code.Contains(part));
                                        if (drs.Count() == 0)
                                        {
                                            errorDesignStructureModel = new ErrorDesignStructureModel();
                                            errorDesignStructureModel.Name = fileName;
                                            errorDesignStructureModel.Size = new FileInfo(item).Length;
                                            errorDesignStructureModel.PathLocal = item;
                                            errorDesignStructureModel.Type = "File mặt không có trong DMVT";
                                            listError.Add(errorDesignStructureModel);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                }

            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }
            return listError;
        }

        public void ExportReportNot3D(ReportTestDesignModel model)
        {
            try
            {

                string initFolder = model.PathLocal + "\\" + model.ModuleCode;
                if (model.DatatbleCT.Count() > 0)
                {
                    foreach (var item in model.DatatbleCT)
                    {
                        if (item.Type.Equals("Không có trong thư viện 3D"))
                        {
                            string hang = item.Hang;
                            string code = item.Name;
                            string codePath = item.PathLocal;
                            string hangFolder = initFolder + "\\" + hang;
                            Directory.CreateDirectory(hangFolder);
                            File.Copy(codePath, hangFolder + "\\" + Path.GetFileName(codePath), true);
                        }
                    }
                }
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }

        }

        public void ExportExcelTestDesignStructure(ReportTestDesignModel model)
        {
            try
            {
                if (!apiUtil.DownloadFile(model.ApiUrl, model.PathDownload, model.PathLocal, model.ModuleCode + ".xls"))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0006, model.ModuleCode);
                }

            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }

        }
        /// <summary>
        /// Tab 4 Kiểm tra thư mục CAD
        /// </summary>
        /// <param name="modelTest"></param>
        /// <returns></returns>
        public List<ErrorDesignStructureModel> CheckCAD(TestDesignStructureModel modelTest, List<string> listErrors)
        {
            List<ErrorDesignStructureModel> listError = new List<ErrorDesignStructureModel>();
            ErrorDesignStructureModel errorDesignStructureModel;

            if (!Directory.Exists(modelTest.PathFolderFileCAD))
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, modelTest.PathFolderFileCAD);
            }

            var listDMVT = LoadDMVTCheckCAD(modelTest.PathFileMaterial);
            List<string> cadFiles = Directory.GetFiles(modelTest.PathFolderFileCAD, "*.dwg", SearchOption.AllDirectories).ToList();

            if (listDMVT.Count() == 0)
            {
                // DMVT không có mã nào bắt đầu bằng TPA
                listErrors.Add("Không có cữ liệu trong Danh mục vật tư có mã bắt đầu bằng TPA");

                // DMVT không có mã nào bắt đầu bằng TPA và folder CAD không có file .dwg nào
                if (cadFiles.Count == 0 || cadFiles.Count(r => Path.GetFileNameWithoutExtension(r).ToUpper().Equals(modelTest.ModuleCode)) == 0)
                {
                    listErrors.Add("File CAD tổng của module không tồn tại!");
                }

                return listError;
            }

            try
            {
                var _cadNumber = cadFiles.Count;
                string nameCad;
                var listCADModule = listDMVT.Where(a => a.Code.ToUpper().StartsWith(modelTest.ModuleCode.ToUpper())).ToList();
                // DMVT có xuất hiện mã bắt đầu bằng mã module nhưng trong folder CAD không có file .dwg nào
                foreach (var item in listCADModule)
                {
                    nameCad = cadFiles.Find(o => Path.GetFileNameWithoutExtension(o).Replace(" ", "").ToUpper().Equals(item.Code.Replace(" ", "").ToUpper()));
                    if (item.Code != Path.GetFileNameWithoutExtension(nameCad))
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = item.Code;
                        errorDesignStructureModel.Type = "Không tồn tại file CAD trong thiết kế";
                        errorDesignStructureModel.NameVT = "";
                        errorDesignStructureModel.PathLocal = nameCad;
                        listError.Add(errorDesignStructureModel);
                    }
                }

                // Trong folder CAD tồn tại file có tên không bắt đầu bằng mã module
                // Trong folder CAD tồn tại file không có trong DMVT và tên file phải khác mã module
                int count = 0;
                for (int j = 0; j <= cadFiles.Count() - 1; j++)
                {
                    nameCad = Path.GetFileNameWithoutExtension(cadFiles[j]).Trim();

                    if (nameCad.ToUpper().Equals(modelTest.ModuleCode.ToUpper()))
                    {
                        continue;
                    }

                    count = listDMVT.Where(a => a.Code.ToUpper().Equals(nameCad.ToUpper())).Count();

                    if (!nameCad.ToUpper().StartsWith(modelTest.ModuleCode.Trim().ToUpper()) || count == 0)
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = "";
                        errorDesignStructureModel.Type = "Thừa file CAD";
                        errorDesignStructureModel.NameVT = nameCad;
                        errorDesignStructureModel.PathLocal = cadFiles[j];
                        listError.Add(errorDesignStructureModel);
                    }
                }

                #region  Kiểm tra ngày giờ
                var listSoftDesign = FillDataInGridIDW(modelTest, listErrors);
                string fileName;
                DateTime dateFile;
                DateTime iptDate;
                foreach (var item in cadFiles)
                {
                    fileName = Path.GetFileNameWithoutExtension(item);
                    dateFile = File.GetLastWriteTime(item);
                    count = 0;

                    var softDesign = listSoftDesign.Where(o => Path.GetFileNameWithoutExtension(o.Name).ToUpper().Equals(fileName.ToUpper())).FirstOrDefault();
                    if (softDesign != null)
                    {
                        iptDate = Convert.ToDateTime(softDesign.Date);
                        if (dateFile < iptDate)
                        {
                            errorDesignStructureModel = new ErrorDesignStructureModel();
                            errorDesignStructureModel.Name = "";
                            errorDesignStructureModel.Type = "Chưa xuất lại CAD";
                            errorDesignStructureModel.NameVT = fileName;
                            errorDesignStructureModel.PathLocal = item;
                            listError.Add(errorDesignStructureModel);
                        }
                    }
                }

                #endregion
            }
            //catch (NTSException ntsEx)
            //{
            //    throw;
            //}
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                //throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }

            #region Kiểm tra file CAD dùng chung
            var listCADShare = listDMVT.Where(a => a.Code.ToUpper().StartsWith("TPA") && !a.Code.ToUpper().StartsWith(modelTest.ModuleCode.ToUpper())).ToList();
            ApiUtil apiUtil = new ApiUtil();
            ResultApiModel resultApiModel;
            if (listCADShare.Count > 0)
            {
                foreach (var item in listCADShare)
                {
                    resultApiModel = new ResultApiModel();
                    resultApiModel = apiUtil.CheckCADShare(modelTest.ApiUrl, modelTest.SelectedPath, item.Code, 0, modelTest.Token);
                    if (resultApiModel.SuccessStatus == false)
                    {
                        errorDesignStructureModel = new ErrorDesignStructureModel();
                        errorDesignStructureModel.Name = item.Code;
                        errorDesignStructureModel.Type = "File CAD dùng chung chưa có trên hệ thống";
                        errorDesignStructureModel.NameVT = "";
                        errorDesignStructureModel.PathLocal = "";
                        // errorDesignStructureModel.TypeColor = Constants.NO_CAD_SHARE;
                        listError.Add(errorDesignStructureModel);
                    }
                }
            }

            #endregion Kết thúc kiểm tra file CAD dùng chung

            return listError;
        }

        public List<MaterialModel> LoadDMVTCheckCAD(string filePath)
        {
            string createBy = string.Empty;

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.OpenReadOnly(filePath);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<MaterialModel> listMaterial = new List<MaterialModel>();
            MaterialModel materialModel;
            createBy = sheet[4, 3].Value;
            //string Name, Specification, Code, RawMaterialCode, DV, SL, VL, KL, Manufacture, Note, MaterialGroupName, MaterialGroupCode;
            try
            {
                for (var i = 7; i < rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(sheet[i, 1].Value) && !string.IsNullOrEmpty(sheet[i, 2].Value))
                    {
                        materialModel = new MaterialModel();
                        materialModel.Stt = sheet[i, 2].Value;
                        materialModel.Name = sheet[i, 2].Value;
                        materialModel.Specification = sheet[i, 3].Value;
                        materialModel.Code = sheet[i, 4].Value;
                        materialModel.RawMaterialCode = sheet[i, 5].Value;
                        materialModel.DV = sheet[i, 6].Value;
                        materialModel.SL = sheet[i, 7].Value;
                        materialModel.VL = sheet[i, 8].Value;
                        materialModel.KL = sheet[i, 9].Value;
                        materialModel.ManufactureName = sheet[i, 10].Value;
                        materialModel.Note = sheet[i, 11].Value;
                        materialModel.MaterialGroupName = sheet[i, 12].Value;
                        materialModel.MaterialGroupCode = sheet[i, 13].Value;

                        if (!string.IsNullOrEmpty(materialModel.Code))
                        {
                            if (materialModel.Code.ToUpper().StartsWith("TPA"))
                            {
                                listMaterial.Add(materialModel);
                            }
                        }
                    }
                }
                workbook.Close();
                excelEngine.Dispose();
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ex;
            }
            if (listMaterial.Count > 0)
            {
                listMaterial[0].CreateBy = createBy;
            }
            return listMaterial;
        }

        public void DownloadAll(TestDesignStructureModel modelTest)
        {
            try
            {
                List<CADModel> list = new List<CADModel>();
                List<string> listErrors = new List<string>();
                var listError = CheckCAD(modelTest, listErrors);
                CADModel cadModel;
                foreach (var itemRow in listError)
                {
                    if (itemRow.Type.Equals("Không có file CAD"))
                    {
                        cadModel = new CADModel();
                        string fileName = itemRow.Name;
                        string productCode = fileName.Substring(0, 10);
                        cadModel.FTPFilePath = "Thietke.Ck\\" + productCode.Substring(0, 6) + "\\" + productCode + ".Ck\\CAD." + productCode + "\\" + fileName + ".dwg";
                        cadModel.FilePath = "D:\\Thietke.Ck\\" + modelTest.ModuleCode.Substring(0, 6) + "\\" + modelTest.ModuleCode + ".Ck\\CAD." + modelTest.ModuleCode;
                        cadModel.SourceFileName = modelTest.ApiUrl + cadModel.FTPFilePath;
                        cadModel.FileName = fileName;
                        list.Add(cadModel);
                    }
                }
                ApiUtil apiUtil = new ApiUtil();
                var listCADResult = apiUtil.LoadCADFile(modelTest.ApiUrl, list, modelTest.Token).Data;
                GoogleApi googleApi = new GoogleApi();
                foreach (var item in listCADResult)
                {
                    if (!string.IsNullOrEmpty(item.IdGoogleApi))
                    {
                        googleApi.DownloadFile(item.IdGoogleApi, item.FileName, item.FilePath + "/");
                    }
                    else
                    {
                        string sourceFileName = "\\SERVER\\data2\\Thietke\\Luutru\\Thietkechuan\\" + item.FTPFilePath;
                        if (File.Exists(sourceFileName))
                        {
                            File.Copy(sourceFileName, item.FilePath + "\\" + item.FilePath + ".dwg");
                        }
                    }
                }
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }
        }

        /// <summary>
        /// Kiểm tra bản cứng bản mềm CAD
        /// </summary>
        public CADResultModel CheckSoftHardCAD(TestDesignStructureModel modelTest, List<string> listError)
        {
            CADResultModel resultModel = new CADResultModel();
            List<CADModel> listSoftCAD = new List<CADModel>();
            List<CADModel> listHardCAD = new List<CADModel>();
            // string folderM = "E:/TPAD.E3109.Ck/CAD.TPAD.E3109";
            // string folderC = "E:/TPAD.E3109.Ck/BCCk.TPAD.E3109/BC-CAD.TPAD.E3109";
            List<string> listFileC = new List<string>();
            List<string> listFileM = new List<string>();
            bool isError = false;

            try
            {
                listFileC = Directory.GetFiles(modelTest.PathFolderBCCAD).ToList();
            }
            catch (Exception e)
            {
                listError.Add(MessageResourceKey.MSG0013);
                return resultModel;
                //throw NTSException.CreateInstance(MessageResourceKey.MSG0013);
            }

            try
            {
                listFileM = Directory.GetFiles(modelTest.PathFolderFileCAD).ToList();
            }
            catch (Exception e)
            {
                listError.Add(MessageResourceKey.MSG0014);
                return resultModel;
                //throw NTSException.CreateInstance(MessageResourceKey.MSG0014);
            }

            CADModel cadModel;


            try
            {
                foreach (var item in listFileC)
                {
                    cadModel = new CADModel();
                    cadModel.FileName = Path.GetFileNameWithoutExtension(item);
                    cadModel.IsExist = true;
                    cadModel.FilePath = item;
                    listHardCAD.Add(cadModel);

                }

                foreach (var item in listFileM)
                {
                    cadModel = new CADModel();
                    cadModel.FileName = Path.GetFileNameWithoutExtension(item);
                    cadModel.IsExist = true;
                    cadModel.FilePath = item;
                    listSoftCAD.Add(cadModel);
                }


                foreach (var item in listHardCAD)
                {
                    if (string.IsNullOrEmpty(item.FileName)) continue;
                    int rowcount = 0;
                    rowcount = listSoftCAD.Where(a => a.FileName.Equals(item.FileName)).Count();
                    if (rowcount == 0)
                    {
                        item.IsExist = false;
                        isError = true;
                    }
                }

                foreach (var item in listSoftCAD)
                {
                    if (string.IsNullOrEmpty(item.FileName)) continue;
                    int rowcount = 0;
                    rowcount = listHardCAD.Where(a => a.FileName.Equals(item.FileName)).Count();
                    if (rowcount == 0)
                    {
                        item.IsExist = false;
                        isError = true;
                    }
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }

            resultModel.ListHardCAD = listHardCAD;
            resultModel.ListSoftCAD = listSoftCAD;
            resultModel.IsSuccess = isError;
            return resultModel;
        }

        public void DownloadHardCAD(TestDesignStructureModel modelTest)
        {
            List<string> listErrors = new List<string>();
            CADResultModel resultCheck = (CADResultModel)CheckSoftHardCAD(modelTest, listErrors);
            List<CADModel> list = new List<CADModel>();
            CADModel cadModel;
            foreach (var item in resultCheck.ListSoftCAD)
            {
                cadModel = new CADModel();
                cadModel.FTPFilePath = "Thietke.Ck\\" + modelTest.ModuleGroupCode + "\\" + modelTest.ModuleCode + ".Ck\\BCCk."
                                + modelTest.ModuleCode + "\\BC-CAD." + modelTest.ModuleCode + "\\" + item.FileName + ".jpg";
                cadModel.FilePath = modelTest.SelectedPath + "\\BCCk." + modelTest.ModuleCode + "\\BC-CAD." + modelTest.ModuleCode;
                cadModel.FileName = item.FileName;
                list.Add(cadModel);
            }

            try
            {
                var listCADResult = apiUtil.LoadCADFile(modelTest.ApiUrl, list, modelTest.Token).Data;
                GoogleApi googleApi = new GoogleApi();
                foreach (var item in listCADResult)
                {
                    if (!string.IsNullOrEmpty(item.IdGoogleApi))
                    {
                        googleApi.DownloadFile(item.IdGoogleApi, item.FileName, item.FilePath + "/");
                    }

                }
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }
        }

        /// <summary>
        /// Kiểm tra danh mục vật tư
        /// </summary>
        /// <param name="modelTest"></param>

        #region Kiểm tra danh mục vật tư
        public List<ListDMVTResultModel> LoadFullDMVT(string filePath, out string moduleCode)
        {
            string createBy = string.Empty;
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.OpenReadOnly(filePath);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();
            List<ListDMVTResultModel> listMaterial = new List<ListDMVTResultModel>();
            ListDMVTResultModel materialModel;
            createBy = sheet[4, 3].Value;

            moduleCode = sheet[3, 10].Value;

            if (!string.IsNullOrEmpty(moduleCode))
            {
                moduleCode = moduleCode.Replace("Mã: ", "").Trim();
            }

            //string Name, Specification, Code, RawMaterialCode, DV, SL, VL, KL, Manufacture, Note, MaterialGroupName, MaterialGroupCode;
            try
            {
                for (var i = 7; i <= rowCount; i++)
                {
                    if (!string.IsNullOrEmpty(sheet[i, 1].Value) && !string.IsNullOrEmpty(sheet[i, 2].Value))
                    {
                        materialModel = new ListDMVTResultModel();
                        materialModel.Stt = sheet[i, 1].Value;
                        materialModel.Name = sheet[i, 2].Value;
                        materialModel.Specification = sheet[i, 3].Value;
                        materialModel.Code = sheet[i, 4].Value;
                        materialModel.RawMaterialCode = sheet[i, 5].Value; // Code của material
                        materialModel.DV = sheet[i, 6].Value;
                        materialModel.SL = sheet[i, 7].Value;
                        materialModel.VL = sheet[i, 8].Value; // Bảng rawmaterial
                        materialModel.KL = sheet[i, 9].Value;
                        materialModel.ManufactureCode = sheet[i, 10].Value;
                        materialModel.ManufactureName = sheet[i, 10].Value;
                        materialModel.Note = sheet[i, 11].Value;
                        materialModel.MaterialGroupName = sheet[i, 12].Value;
                        materialModel.MaterialGroupCode = sheet[i, 13].Value;

                        listMaterial.Add(materialModel);
                    }
                }

                workbook.Close();
                excelEngine.Dispose();
            }
            catch (Exception ex)
            {
                workbook.Close();
                excelEngine.Dispose();
                throw ex;
            }
            listMaterial[0].CreateBy = createBy;
            return listMaterial;
        }

        /// <summary>
        /// Kiêm tra danh mục vật tư
        /// </summary>
        /// <param name="modelTest"></param>
        /// <param name="listErrors"></param>
        /// <returns></returns>
        public ResultCheckDMVTModel CheckFileDMVT(TestDesignStructureModel modelTest, List<string> listErrors)
        {
            ResultCheckDMVTModel resultCheckDMVTModel = new ResultCheckDMVTModel();
            var resultApi = apiUtil.GetDataCheckDMVT(modelTest.ApiUrl, modelTest.Token);

            DataCheckDMVTModel dataCheckDMVT = new DataCheckDMVTModel();
            if (resultApi.SuccessStatus)
            {
                dataCheckDMVT = resultApi.Data;
            }
            else
            {
                listErrors.Add(ResourceUtil.GetResourcesNoLag(ErrorResourceKey.ERR0003));
                return resultCheckDMVTModel;
            }

            bool isOk = false;
            List<ListDMVTResultModel> listResult = new List<ListDMVTResultModel>();
            string listManuError = string.Empty;
            string listPartError = string.Empty;
            string listPartManuError = string.Empty;

            List<ListDMVTResultModel> listFullDMVT = new List<ListDMVTResultModel>();
            List<ListDMVTResultModel> listDMVTNotDB = new List<ListDMVTResultModel>();

            string moduleCode;
            try
            {
                listFullDMVT = LoadFullDMVT(modelTest.PathFileMaterial, out moduleCode);
            }
            catch (Exception ex)
            {
                listErrors.Add($"File {modelTest.PathFileMaterial} không đúng định dạng");
                resultCheckDMVTModel.ListManuError = $"File {modelTest.PathFileMaterial} không đúng định dạng";
                return resultCheckDMVTModel;
            }

            if (!string.IsNullOrEmpty(modelTest.ModuleCode))
            {
                moduleCode = modelTest.ModuleCode;
            }

            try
            {
                Parallel.ForEach(listFullDMVT, async item =>
                {
                    await CheckDMVT(item, dataCheckDMVT, moduleCode, listDMVTNotDB);
                    listResult.Add(item);
                });
                if (listResult.Count > 0)
                {
                    int maxLen = listResult.Select(s => s.Stt.Length).Max();

                    Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                    listResult = listResult
                               .Select(s =>
                                   new
                                   {
                                       OrgStr = s,
                                       SortStr = Regex.Replace(s.Stt, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                                   })
                               .OrderBy(x => x.SortStr)
                               .Select(x => x.OrgStr).ToList();
                }
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            resultCheckDMVTModel.ListMaterialNotDB = listDMVTNotDB;
            resultCheckDMVTModel.ListResult = listResult;
            resultCheckDMVTModel.ListManuError = listManuError;
            resultCheckDMVTModel.listPartError = listPartError;
            resultCheckDMVTModel.listPartManuError = listPartManuError;
            resultCheckDMVTModel.IsOK = resultCheckDMVTModel.ListResult.Any(r => r.ListEror.Count > 0);
            return resultCheckDMVTModel;
        }

        private async Task CheckDMVT(ListDMVTResultModel dmvt, DataCheckDMVTModel dataCheckDMVT, string moduleCode, List<ListDMVTResultModel> listDMVTNotDB)
        {
            bool check = false;
            bool isOk = false;

            List<string> specialChar = new List<string>() { "*", "{", "}", "!", "^", "<", ">", "?", "|", "_", "#", "~", "`", ":", @"\", "@", "%", "$" };
            CheckMatrialModel materialModel;
            CheckRawMaterialModel rawMaterialModel;
            CheckManufactureModel manufacture;
            CheckConverUnitModel converUnit;

            string index = string.Empty;

            if (dmvt.ManufactureCode.Equals(Constants.TPA) && dmvt.Specification.ToLower() == Constants.HAN)
            {
                index = dmvt.Stt;
            }

            if (!string.IsNullOrEmpty(index))
            {
                if (dmvt.Stt.Length >= (index.Length + 1))
                {
                    if (dmvt.Stt.Substring(0, index.Length + 1) == (index + "."))
                    {
                        check = true;
                    }
                }
            }

            if (!check)
            {
                if (string.IsNullOrEmpty(dmvt.Stt))
                {
                    if (!string.IsNullOrEmpty(dmvt.Name) && !string.IsNullOrEmpty(dmvt.Code) && !string.IsNullOrEmpty(dmvt.ManufactureCode) && !string.IsNullOrEmpty(dmvt.DV))
                    {
                        dmvt.ListIndexPart.Add(0);
                        dmvt.ListEror.Add("STT không được để trống");
                        isOk = true;
                    }
                    else
                    {
                        return;
                    }
                }

                // Kiểm tra tên vật tư 
                if (!CheckNullOrMaxLength(dmvt.Name, 100, "Danh mục vật tư "))
                {
                    dmvt.ListIndexPart.Add(1);
                    dmvt.ListEror.Add("Tên vật tư > 100 kí tự");
                    isOk = true;
                }

                // Kiểm tra mã vật liệu 
                if (!CheckMaxLength(dmvt.VL, 100, "Danh mục vật tư "))
                {
                    dmvt.ListIndexPart.Add(4);
                    dmvt.ListEror.Add("Độ dài mã vật liệu > 100 kí tự");
                    isOk = true;
                }

                // Kiểm tra mã vật tư
                if (!CheckNullOrMaxLength(dmvt.Code, 50, "Danh mục vật tư"))
                {
                    dmvt.ListIndexPart.Add(3);
                    dmvt.ListEror.Add("Độ dài mã vật tư > 50 kí tự");
                    isOk = true;
                }

                // Kiểm tra hãng sản xuất
                if (!CheckNullOrMaxLength(dmvt.ManufactureCode, 50, "Danh mục vật tư "))
                {
                    dmvt.ListIndexPart.Add(9);
                    dmvt.ListEror.Add("Độ dài mã hãng sản xuất > 50 kí tự");
                    isOk = true;
                }

                // Kiểm tra đơn vị
                if (!CheckNullOrMaxLength(dmvt.DV, 200, "Danh mục vật tư"))
                {
                    dmvt.ListIndexPart.Add(5);
                    dmvt.ListEror.Add("Độ dài đơn vị > 200 kí tự");
                    isOk = true;
                }

                var unit = dataCheckDMVT.Units.Where(a => a.Name.ToLower().Equals(dmvt.DV.ToLower())).FirstOrDefault();

                if (unit == null)
                {
                    dmvt.ListIndexPart.Add(5);
                    dmvt.ListEror.Add("Đơn vị không có trong thư viện");
                    isOk = true;
                }

                // Kiểm tra số lượng
                if (!CheckNullOrMaxLength(dmvt.SL, 9, "Danh mục vật tư "))
                {
                    dmvt.ListIndexPart.Add(6);
                    dmvt.ListEror.Add("Số lượng VT > 9 ký tự");
                    isOk = true;
                }

                // Kiểm tra số lượng
                if (!CheckMaxLength(dmvt.Specification, 500, "Danh mục vật tư"))
                {
                    dmvt.ListIndexPart.Add(2);
                    dmvt.ListEror.Add("Thống số VT > 500 ký tự");
                    isOk = true;
                }

                // Kiểm tra số lượng
                if (!CheckIsNumber(dmvt.SL, "Danh mục vật tư ", Constants.NumberFormatType.DECIMAL))
                {
                    dmvt.ListIndexPart.Add(6);
                    dmvt.ListEror.Add("Số lượng VT không phải số");
                    isOk = true;
                }
                // Kiểm tra số lượng    
                else if (Convert.ToDecimal(dmvt.SL) <= 0)
                {
                    dmvt.ListIndexPart.Add(6);
                    dmvt.ListEror.Add("Số lượng VT phải >= 0");
                    isOk = true;
                }

                if (!string.IsNullOrEmpty(dmvt.RawMaterialCode))
                {
                    // Kiểm tra mã linh kiện
                    if (!CheckMaxLength(dmvt.RawMaterialCode, 100, "Danh mục vật tư "))
                    {
                        dmvt.ListIndexPart.Add(4);
                        dmvt.ListEror.Add("VL có mã > 100 ký tự");
                        isOk = true;
                    }

                    materialModel = dataCheckDMVT.Materials.Where(a => a.Code.Equals(dmvt.RawMaterialCode)).FirstOrDefault();
                    if (materialModel == null)
                    {
                        dmvt.ListIndexPart.Add(4);
                        dmvt.ListEror.Add("Vật tư nguồn chưa tồn tại");
                        isOk = true;
                    }
                    else
                    {
                        // Nếu cột mã vật liệu (vật tư nguồn) trong DMVT có dữ liệu. Đơn vị là M hoặc CÁI, THANH hoặc MM, CUỘn và vật tư nguồn chưa đc đn chuyển đổi đơn vị
                        converUnit = dataCheckDMVT.ConverUnits.FirstOrDefault(a => a.MaterialCode.Equals(dmvt.RawMaterialCode));

                        if (converUnit == null && (materialModel.UnitName.ToUpper().Equals("M") || materialModel.UnitName.ToUpper().Equals("CÁI") ||
                            materialModel.UnitName.ToUpper().Equals("THANH") || materialModel.UnitName.ToUpper().Equals("MM") || materialModel.UnitName.ToUpper().Equals("CUỘN")))
                        {
                            dmvt.ListIndexPart.Add(4);
                            dmvt.ListEror.Add("Vật tư nguồn chưa được chuyển đổi đơn vị");
                            isOk = true;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dmvt.KL) && !dmvt.KL.Equals("-"))
                {
                    if (!CheckMaxLength(dmvt.KL, 9, "Danh mục vật tư "))
                    {
                        dmvt.ListIndexPart.Add(8);
                        dmvt.ListEror.Add("Khối lượng vật liệu > 9 ký tự");
                        isOk = true;
                    }

                    // Kiểm tra số lượng
                    if (!CheckIsNumber(dmvt.KL, "Danh mục vật tư ", Constants.NumberFormatType.DECIMAL))
                    {
                        dmvt.ListIndexPart.Add(8);
                        dmvt.ListEror.Add("Khối lượng VL không phải là số");
                        isOk = true;
                    }
                    // Kiểm tra số lượng
                    else if (Convert.ToDecimal(dmvt.KL) < 0)
                    {
                        dmvt.ListIndexPart.Add(8);
                        dmvt.ListEror.Add("Khối lượng VL phải >= 0");
                        isOk = true;
                    }
                }

                // khác hãng tpa
                bool codeTPA = dmvt.ManufactureCode.ToLower().Equals("tpa");
                materialModel = dataCheckDMVT.Materials.Where(a => a.Code.ToLower().Equals(dmvt.Code.ToLower())).FirstOrDefault();
                if (materialModel == null && !codeTPA)
                {
                    listDMVTNotDB.Add(dmvt);
                    dmvt.ListIndexPart.Add(3);
                    dmvt.ListEror.Add("Vật tư chưa tồn tại");
                    isOk = true;
                }
                else if (materialModel != null)
                {
                    if (!materialModel.Name.ToLower().Equals(dmvt.Name.ToLower()))
                    {
                        dmvt.ListIndexPart.Add(1);
                        dmvt.ListEror.Add($"Vật tư sai tên ({materialModel.Name})");
                        isOk = true;
                    }

                    if (!materialModel.Is3DExist && !dmvt.Code.ToUpper().StartsWith("TPA") && !dmvt.Code.ToUpper().StartsWith("PCB"))
                    {
                        dmvt.ListIndexPart.Add(3);
                        dmvt.ListEror.Add("Vật tư chưa có trong thư viện 3D");
                        isOk = true;
                    }

                    // Hãng sản xuất của vật tư không đúng
                    if (!materialModel.ManufactureCode.ToUpper().Equals(dmvt.ManufactureCode.ToUpper()))
                    {
                        dmvt.ListIndexPart.Add(9);
                        dmvt.ListEror.Add($"Sai khác hãng so với thư viện vật tư ({materialModel.ManufactureCode})");
                        isOk = true;
                    }

                    // Vật tư có thư viện nhưng khác đơn vị 
                    if (!materialModel.UnitName.ToLower().Equals(dmvt.DV.ToLower()))
                    {
                        dmvt.ListIndexPart.Add(5);
                        dmvt.ListEror.Add($"Sai khác đơn vị so với thư viện vật tư ({materialModel.UnitName})");
                        isOk = true;
                    }

                    // Vật tư tạm dừng sử dụng trong thư viện 
                    if (materialModel.Status.Equals(Constants.Material_Status_Pause))
                    {
                        dmvt.ListIndexPart.Add(3);
                        dmvt.ListEror.Add("Vật tư tạm dừng sử dụng");
                        isOk = true;
                    }
                    else if (materialModel.Status.Equals(Constants.Material_Status_Stop))
                    {
                        dmvt.ListIndexPart.Add(3);
                        dmvt.ListEror.Add("Vật tư ngừng sản xuất");
                        isOk = true;
                    }
                }

                if (!codeTPA && "TPA".Equals(dmvt.Specification.ToUpper()))
                {
                    dmvt.ListIndexPart.Add(2);
                    dmvt.ListEror.Add("Thống số VT không đúng");
                    isOk = true;
                }

                if (!string.IsNullOrEmpty(dmvt.ManufactureCode))
                {
                    manufacture = dataCheckDMVT.Manufactures.Where(a => a.Code.ToUpper().Equals(dmvt.ManufactureCode.ToUpper())).FirstOrDefault();
                    if (manufacture == null)
                    {
                        dmvt.ListIndexPart.Add(9);
                        dmvt.ListEror.Add("Hãng chưa có trên thư viện");
                        isOk = true;
                    }
                    else
                    {
                        if (manufacture.Status.Equals(Constants.manufacetureStop))
                        {
                            dmvt.ListIndexPart.Add(9);
                            dmvt.ListEror.Add("Hãng không sử dụng");
                            isOk = true;
                        }
                    }
                }


                if (!string.IsNullOrEmpty(dmvt.VL) && !dmvt.VL.Equals("-"))
                {
                    rawMaterialModel = dataCheckDMVT.RawMaterials.Where(a => a.Code.ToUpper().Equals(dmvt.VL.ToUpper())).FirstOrDefault();
                    // vật liệu không có trong thư viện

                    if (rawMaterialModel == null)
                    {
                        dmvt.ListIndexPart.Add(7);
                        dmvt.ListEror.Add("Vật liệu không có trong thư viện");
                        isOk = true;
                    }
                }

                // Mã vật tư trùng với module code                           
                if (dmvt.Code.ToUpper().Equals(moduleCode))
                {
                    dmvt.ListIndexPart.Add(3);
                    dmvt.ListEror.Add("Vật tư không được cùng mã với module");
                    isOk = true;
                }

                // Nếu đơn vị là BỘ và Mã Vật Tư không phải là TPA, PCB
                if (!dmvt.Code.ToUpper().StartsWith("TPA") && !dmvt.Code.ToUpper().StartsWith("PCB") && dmvt.DV.ToUpper().Equals("BỘ"))
                {
                    dmvt.ListIndexPart.Add(5);
                    dmvt.ListEror.Add("Vật tư sai đơn vị");
                    isOk = true;
                }

                // Nếu đơn vị là Cái và mã vật tư là PCB
                if (dmvt.Code.ToUpper().StartsWith("PCB") && dmvt.DV.ToUpper().Equals("CÁI"))
                {
                    dmvt.ListIndexPart.Add(5);
                    dmvt.ListEror.Add("Vật tư sai đơn vị");
                    isOk = true;
                }

                // Nếu Khối lượng để trống hoặc khối lượng bằng "-", Đơn vị là Cái, mã vật tư bắt đầu bằng TPA
                if ((string.IsNullOrEmpty(dmvt.KL) || dmvt.KL.Equals("-")) && dmvt.DV.ToLower().Equals("cái") && dmvt.Code.ToLower().StartsWith("tpa"))
                {
                    dmvt.ListIndexPart.Add(8);
                    dmvt.ListEror.Add("Khối lượng không có giá trị trong DMVT");
                    isOk = true;
                }

                // Nếu VL để trống hoặc VL bằng "-", Đơn vị là Cái, mã vật tư bắt đầu bằng TPA
                if ((string.IsNullOrEmpty(dmvt.VL) || dmvt.VL.Equals("-")) && dmvt.DV.ToLower().Equals("cái") && dmvt.Code.ToLower().StartsWith("tpa"))
                {
                    dmvt.ListIndexPart.Add(7);
                    dmvt.ListEror.Add("Vật liệu không có giá trị trong DMVT");
                    isOk = true;
                }

                // Vật tư có Hãng: TPA - Đơn vị: Bộ - Thông số: HAN => Phải có KL
                if (dmvt.Code.ToUpper().StartsWith("TPA") && dmvt.DV.ToUpper().Equals("BỘ") && dmvt.Specification.ToUpper().Equals("HÀN") && (string.IsNullOrEmpty(dmvt.KL) || dmvt.KL.Equals("-")))
                {
                    dmvt.ListIndexPart.Add(8);
                    dmvt.ListEror.Add("Thiếu khối lượng");
                    isOk = true;
                }

                // Vật tư có Hãng: TPA - Đơn vị: Bộ - Thông số: HAN => Phải có VL
                if (dmvt.Code.ToUpper().StartsWith("TPA") && dmvt.DV.ToUpper().Equals("BỘ") && dmvt.Specification.ToUpper().Equals("HÀN") && (string.IsNullOrEmpty(dmvt.VL) || dmvt.VL.Equals("-")))
                {
                    dmvt.ListIndexPart.Add(7);
                    dmvt.ListEror.Add("Thiếu vật liệu");
                    isOk = true;
                }

                // Vật tư có Hãng: TPA - Đơn vị: Cái - Có MVL => Không có vật liệu => Thông báo thiếu VL
                if (dmvt.Code.ToUpper().StartsWith("TPA") && dmvt.DV.ToUpper().Equals("CÁI") && !string.IsNullOrEmpty(dmvt.RawMaterialCode) && (string.IsNullOrEmpty(dmvt.VL) || dmvt.VL.Equals("-")))
                {
                    dmvt.ListIndexPart.Add(7);
                    dmvt.ListEror.Add("Thiếu vật liệu");
                    isOk = true;
                }
                // Kiểm tra mã có chưa kí tự dặc biệt 
                foreach (var item in specialChar)
                {
                    if (dmvt.Code.Contains(item))
                    {
                        dmvt.ListIndexPart.Add(3);
                        dmvt.ListEror.Add("Mã vật tư có chưa ký tự đặc biệt");
                        isOk = true;
                        break;
                    }
                }
            }
        }


        public static bool CheckNullOrMaxLength(string text, int maxLength, string paramete)
        {
            // Trường hợp chuỗi rỗng hoặc null
            if (text == null || string.IsNullOrEmpty(text.Trim()))
            {
                return false;
            }
            // Quá độ dài cho phép
            else if (text.Length > maxLength)
            {
                return false;
            }
            return true;
        }

        public static bool CheckIsNumber(string text, string paramete, Constants.NumberFormatType numberType)
        {
            try
            {
                // Kiểm tra có là kiểu số hay không
                switch (numberType)
                {
                    case Constants.NumberFormatType.INTEREGER:// Kiểu int
                        {
                            int value = int.Parse(text);
                            return true;
                        }
                    case Constants.NumberFormatType.FLOAT:
                        {
                            float value = float.Parse(text);
                            return true;
                        }
                    case Constants.NumberFormatType.DECIMAL:
                        {
                            decimal value = decimal.Parse(text);
                            return true;

                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckMaxLength(string text, int maxLength, string paramete)
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }
            // Quá độ dài cho phép
            else if (text.Length > maxLength)
            {
                //MessageBox.Show(Configuration.GetResource(MessageList.MSG027, new string[] { paramete, maxLength.ToString() }), Configuration.GetResource(MessageList.TITLE), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        #endregion kết thúc kiểm tra danh mục vật tư
        public void ExportResutlDMVT(TestDesignStructureModel model)
        {
            List<ListDMVTResultModel> listDMVT;
            List<string> listError = new List<string>();
            try
            {
                listDMVT = CheckFileDMVT(model, listError).ListResult;
            }
            catch (Exception ex)
            {
                throw new Exception("File không đúng định dạng!");
                //throw NTSException.CreateInstance(MessageResourceKey.MSG0015, "");
            }


            try
            {
                if (!apiUtil.DownloadFile(model.ApiUrl, model.PathDownload, model.SelectedPath, model.ModuleCode + ".xls"))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0006, model.ModuleCode);
                }
                string _moduleCode = Path.GetFileNameWithoutExtension(model.PathFileMaterial);
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.OpenReadOnly(model.SelectedPath + "/" + model.ModuleCode + ".xls");
                IWorksheet sheet = workbook.Worksheets[0];

                sheet.Replace("<code>", _moduleCode, ExcelFindOptions.MatchCase);
                sheet.Replace("<name>", listDMVT[0].MaterialGroupName, ExcelFindOptions.MatchCase);

                var total = listDMVT.Count;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport = listDMVT.Select((a, i) => new
                {
                    a.Stt,
                    a.Name,
                    a.Specification,
                    a.Code,
                    a.RawMaterialCode,
                    a.DV,
                    a.SL,
                    a.VL,
                    a.KL,
                    a.ManufactureCode,
                    a.Note
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders.Color = ExcelKnownColors.Black;
                sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 11].CellStyle.WrapText = true;

                workbook.Save();
                workbook.Close();
                excelEngine.Dispose();
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }
        }

        #region Điện
        public ElectricModel CheckFileElectric(UploadFolderModel model)
        {
            ElectricModel checkElectric = new ElectricModel();

            try
            {
                var resultDataApi = apiUtil.GetData(model.ApiUrl, model.Token, Constants.Design_Type_DN);

                if (!resultDataApi.SuccessStatus)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
                }
                var dataCheck = resultDataApi.Data;

                model.ListCodeRule = dataCheck.ListCodeRule;
                model.ListFileDefinition = dataCheck.ListFileDefinition.Where(i => i.TypeDefinitionId == Constants.Design_Type_DN).ToList();
                model.ListFolderDefinition = dataCheck.ListFolderDefinition.Where(i => i.TypeDefinitionId == Constants.Design_Type_DN).ToList();
                model.ListManufacturerModel = dataCheck.ListManufacturerModel;
                model.ListMaterialGroupModel = dataCheck.ListMaterialGroupModel;
                model.ListMaterialModel = dataCheck.ListMaterialModel;
                model.ListRawMaterialsModel = dataCheck.ListRawMaterialsModel;
                model.ListUnitModel = dataCheck.ListUnitModel;

                var apiModel = apiUtil.GetModuleInfo(model.ApiUrl, model.ModuleCode, model.Token);
                ModuleModel moduleModel = new ModuleModel();

                if (!apiModel.SuccessStatus)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
                }

                moduleModel = apiModel.Data;

                model.ModuleGroupCode = moduleModel.ModuleGroupCode;

                FileService fileService = new FileService();

                if (string.IsNullOrEmpty(model.Path))
                {
                    model.Path = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, model.ModuleCode, model.ModuleGroupCode, moduleModel.ParentGroupCode,
                        model.ListFolderDefinition, model.ListFileDefinition);
                }
                CheckFolderResult check = new CheckFolderResult();
                if (model.CheckModel.IsCheckDesignFolder)
                {
                    List<string> pathErrors = new List<string>();
                    check = fileService.CheckFolderUpload(model.DesignType, model.Path, model.ModuleCode, model.ModuleGroupCode, moduleModel.ParentGroupCode,
                    checkElectric.ListError, model.ListFolderDefinition, model.ListFileDefinition, model.ListMaterialModel, model.ListRawMaterialsModel, model.ListManufacturerModel,
                    model.ListMaterialGroupModel, model.ListUnitModel, model.ListCodeRule, checkElectric.LstError, checkElectric.ListFolder, false, out string materialPath, dataCheck.Modules,
                    pathErrors);
                }
                else
                {
                    check.Status = true;
                }

                // Kiểm tra size file
                if (!check.Status)
                {
                    return checkElectric;
                }

                checkElectric.Message = true;

                var resultApi = apiUtil.GetModuleDesignDocumentByType(model.ApiUrl, Constants.Design_Type_DN, model.Token);

                if (!resultApi.SuccessStatus)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
                }

                checkElectric.ListModuleDesignDoc = resultApi.Data;
                if (model.CheckModel.IsCheckElectric)
                {
                    var listFiles = CheckFile(model);
                    ModuleDesignDocumentModel pathLocal;
                    foreach (var item in listFiles)
                    {
                        pathLocal = checkElectric.ListModuleDesignDoc.Where(a => (item.Path.Equals(a.Path))).FirstOrDefault();

                        if (pathLocal != null)
                        {
                            ElectricModel elec = new ElectricModel();
                            elec.Name = item.Name;
                            elec.Size = item.FileSize;
                            elec.PathLocal = item.Path;
                            checkElectric.ListCheckFile.Add(elec);
                        }
                    }
                }
            }
            catch (NTSException ntsEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0001);
            }

            return checkElectric;
        }
        #endregion

        #region check file and folder 
        public List<ModuleDesignDocumentModel> CheckFile(UploadFolderModel model)
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

            var listFolderChild = fileService.GetChildDirectories(model.Path).Select(t => new FolderModel
            {
                Id = t.FullName,
                Name = t.Name,
                Path = folderModel.Path + "\\" + t.Name,
                FullPath = t.FullName,
            }).ToList();

            if (listFolderChild.Count > 0)
            {
                CheckFoders(listFolderChild, ListResult);
            }

            var listFile = fileService.GetChildFiles(model.Path).Select(t => new FolderModel
            {
                Id = t.FullName,
                Name = t.Name,
                Path = folderModel.Path + "\\" + t.Name,
                FullPath = t.FullName,
                Extension = t.Extension
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
                        ModuleId = model.ModuleId
                    };
                    ListResult.Add(fileModel);
                }
            }
            return ListResult;
        }

        // hàm đệ quy check foder con
        public void CheckFoders(List<FolderModel> list, List<ModuleDesignDocumentModel> ListResult)
        {
            foreach (var item in list)
            {
                var listFolder = fileService.GetChildDirectories(item.FullPath).Select(t => new FolderModel
                {
                    Id = t.FullName,
                    Name = t.Name,
                    Path = item.Path + "\\" + t.Name,
                    FullPath = t.FullName,
                }).ToList();

                if (listFolder.Count() > 0)
                {
                    CheckFoders(listFolder, ListResult);
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

        #region Kiểm tra điện tử
        public CheckElectronicModel CheckElectronic(UploadFolderModel model)
        {
            CheckElectronicModel checkElectronic = new CheckElectronicModel();

            var dataCheck = apiUtil.GetData(model.ApiUrl, model.Token, Constants.Design_Type_DT);

            if (!dataCheck.SuccessStatus)
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
            }

            model.ListCodeRule = dataCheck.Data.ListCodeRule;
            model.ListFileDefinition = dataCheck.Data.ListFileDefinition.Where(i => i.TypeDefinitionId == Constants.Design_Type_DT).ToList();
            model.ListFolderDefinition = dataCheck.Data.ListFolderDefinition.Where(i => i.TypeDefinitionId == Constants.Design_Type_DT).ToList();
            model.ListManufacturerModel = dataCheck.Data.ListManufacturerModel;
            model.ListMaterialGroupModel = dataCheck.Data.ListMaterialGroupModel;
            model.ListMaterialModel = dataCheck.Data.ListMaterialModel;
            model.ListRawMaterialsModel = dataCheck.Data.ListRawMaterialsModel;
            model.ListUnitModel = dataCheck.Data.ListUnitModel;

            var apiModel = apiUtil.GetModuleInfo(model.ApiUrl, model.ModuleCode, model.Token);
            ModuleModel moduleModel = new ModuleModel();

            if (!apiModel.SuccessStatus)
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
            }

            moduleModel = apiModel.Data;
            model.ModuleGroupCode = moduleModel.ModuleGroupCode;

            FileService fileService = new FileService();
            if (string.IsNullOrEmpty(model.Path))
            {
                model.Path = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, model.ModuleCode, model.ModuleGroupCode, moduleModel.ParentGroupCode,
                    model.ListFolderDefinition, model.ListFileDefinition);
            }

            if (model.CheckModel.IsCheckElectronic)
            {
                List<string> pathErrors = new List<string>();
                var check = fileService.CheckFolderUpload(model.DesignType, model.Path, model.ModuleCode, model.ModuleGroupCode, moduleModel.ParentGroupCode,
                checkElectronic.ListError, model.ListFolderDefinition, model.ListFileDefinition, model.ListMaterialModel, model.ListRawMaterialsModel, model.ListManufacturerModel,
                model.ListMaterialGroupModel, model.ListUnitModel, model.ListCodeRule, checkElectronic.LstError, checkElectronic.ListFolder, false, out string materialPath, dataCheck.Data.Modules,
                pathErrors);

                int countFile = 0;
                // Lấy list file
                string[] listFile = Directory.GetFiles(model.Path, "*.*", SearchOption.AllDirectories);

                // Kiểm tra tồn tại file điện tử
                string fileSchdoc = listFile.FirstOrDefault(o => Path.GetExtension(o).ToLower() == ".schdoc");
                if (fileSchdoc == null)
                {
                    checkElectronic.LstError.Add("Tài liệu không chứa các file điện tử");
                    //return checkElectronic;
                }

                // Lấy file .PcbDoc trong thư mục PRD
                string pathFilePRD = Directory.GetFiles(checkElectronic.ListFolder[0], "*.PcbDoc*", SearchOption.AllDirectories).FirstOrDefault();

                // Lấy file .PcbDoc trong thư mục PRJ
                string pathFilePRJ = Directory.GetFiles(checkElectronic.ListFolder[1], "*.PcbDoc*", SearchOption.AllDirectories).FirstOrDefault();

                // Kiểm tra thư mục có chứa file .PcbDoc
                if (!File.Exists(pathFilePRD))
                {
                    checkElectronic.LstError.Add("Thư mục " + checkElectronic.ListFolder[0] + " Không chứa file .PcbDoc");
                }
                else if (!File.Exists(pathFilePRJ))
                {
                    checkElectronic.LstError.Add("Thư mục " + checkElectronic.ListFolder[1] + " Không chứa file .PcbDoc");
                }
                else
                {
                    FileInfo filePRD = new FileInfo(pathFilePRD);
                    FileInfo filePRJ = new FileInfo(pathFilePRJ);

                    if (filePRD.Length != filePRJ.Length)
                    {
                        checkElectronic.LstError.Add("Size file .PcbDoc không trùng");
                        return checkElectronic;
                    }

                    if (filePRD.LastWriteTime != filePRJ.LastWriteTime)
                    {
                        checkElectronic.LstError.Add("Thời gian file .PcbDoc không trùng");
                        return checkElectronic;
                    }

                    var path = pathFilePRD.Remove(0, Constants.Disk_Design.Length);
                    ResultApiModel resultApiModel = apiUtil.CheckFileSize(model.ApiUrl, path, model.ModuleCode, filePRD.Length, model.Token);

                    if (!resultApiModel.SuccessStatus)
                    {
                        checkElectronic.LstError.Add(resultApiModel.Message);
                    }
                }
            }

            if (model.CheckModel.IsCheckDMVT)
            {
                TestDesignStructureModel modelTest = new TestDesignStructureModel();
                modelTest.PathFileMaterial = Constants.Disk_Design + fileService.GetPathFile(Constants.FileDefinition_FileType_ListMaterial, model.ModuleCode, model.ModuleGroupCode, moduleModel.ParentGroupCode,
                    model.ListFolderDefinition, model.ListFileDefinition);

                modelTest.ApiUrl = model.ApiUrl;
                var resultCheckDMVT = CheckFileDMVT(modelTest, checkElectronic.LstError);
                checkElectronic.resultCheckDMVTModel = resultCheckDMVT;
            }

            return checkElectronic;
        }
        #endregion

        #region Kiểm tra thư mục MAT
        public List<MATFileResultModel> CheckMAT(TestDesignStructureModel modelTest)
        {
            List<MATFileResultModel> listFileResult = new List<MATFileResultModel>();
            try
            {

                if (!Directory.Exists(modelTest.PathFolderMAT))
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, modelTest.PathFolderMAT);
                }

                DirectoryInfo dirLocal = new DirectoryInfo(modelTest.PathFolderMAT);
                List<MATFileResultModel> files = new List<MATFileResultModel>();
                MATFileResultModel fileModel;
                foreach (var file in dirLocal.GetFiles())
                {
                    fileModel = new MATFileResultModel()
                    {
                        FileName = file.Name,
                        FilePath = file.FullName,
                        FileSize = file.Length
                    };

                    files.Add(fileModel);
                }

                MATModel mATModel = new MATModel()
                {
                    FileLocals = files,
                    ModuleCode = modelTest.ModuleCode,
                    NamePath = dirLocal.Name,
                };

                listFileResult = apiUtil.GetFileInFolderMAT(modelTest.ApiUrl, mATModel, modelTest.Token).Data;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            return listFileResult;
        }
        #endregion Kết thúc kiểm tra thư mục MAT

        #region kiểm tra thư mục JGS
        public List<IGSFileResultModel> CheckIGS(TestDesignStructureModel modelTest)
        {
            List<IGSFileResultModel> listFileResult = new List<IGSFileResultModel>();
            try
            {

                if (!Directory.Exists(modelTest.PathFolderIGS))
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, modelTest.PathFolderIGS);
                }

                DirectoryInfo dirLocal = new DirectoryInfo(modelTest.PathFolderIGS);
                List<IGSFileResultModel> files = new List<IGSFileResultModel>();
                IGSFileResultModel fileModel;
                foreach (var file in dirLocal.GetFiles())
                {
                    fileModel = new IGSFileResultModel()
                    {
                        FileName = file.Name,
                        FilePath = file.FullName,
                        FileSize = file.Length
                    };

                    files.Add(fileModel);
                }

                IGSModel jGSModel = new IGSModel()
                {
                    FileLocals = files,
                    ModuleCode = modelTest.ModuleCode,
                    NamePath = dirLocal.Name,
                };

                listFileResult = apiUtil.GetFileInFolderIGS(modelTest.ApiUrl, jGSModel, modelTest.Token).Data;
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
            return listFileResult;
        }
        #endregion

        #region Xuất biểu mẫu
        public void GenaralTemplate(TestDesignStructureModel modelTest)
        {
            // Lấy dữ liệu từ server
            var dataCheck = apiUtil.GetDataDefinitions(modelTest.ApiUrl, modelTest.Token, modelTest.Type, Constants.Definition_ObjectType_Module);

            if (!dataCheck.SuccessStatus)
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
            }

            UploadFolderModel model = new UploadFolderModel();
            List<string> listError = new List<string>();

            model.ListFileDefinition = dataCheck.Data.ListFileDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
            model.ListFolderDefinition = dataCheck.Data.ListFolderDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();

            ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();

            resultApiModel = apiUtil.GetModuleInfo(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);

            string selectedPath = modelTest.PathFileMaterial;

            //if (!string.IsNullOrEmpty(selectedPath))
            //{
            //    modelTest.ModuleCode = Path.GetFileNameWithoutExtension(selectedPath);
            //}

            ModuleModel moduleModel = new ModuleModel();
            if (resultApiModel.SuccessStatus)
            {
                moduleModel = resultApiModel.Data;
            }
            else
            {
                throw NTSException.CreateInstance(resultApiModel.Message);
            }

            modelTest.ModuleGroupCode = moduleModel.ModuleGroupCode;


            selectedPath = Constants.Disk_Design + fileService.GetPathFileByName(modelTest.FileName, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
            string pathFolder = Path.GetDirectoryName(selectedPath);
            if (!Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }
            if (Directory.Exists(selectedPath))
            {
                File.Delete(selectedPath);
            }

            modelTest.SelectedPath = selectedPath;
            string folderName = Path.GetFileName(selectedPath);
            WebClient webClient = new WebClient();
            webClient.DownloadFile(modelTest.ApiUrl + modelTest.PathFileMaterial, selectedPath);
        }
        #endregion

        #region Xuất biểu mẫu hồ sơ cơ khí
        public void CraeteMechanicalProfile(MechanicalRecordsModel modelTest)
        {
            var dataCheck = apiUtil.GetDataDefinitions(modelTest.ApiUrl, modelTest.Token, modelTest.Type, Constants.Definition_ObjectType_Module);

            if (!dataCheck.SuccessStatus)
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
            }
            UploadFolderModel model = new UploadFolderModel();
            List<string> listError = new List<string>();
            model.ListFileDefinition = dataCheck.Data.ListFileDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
            model.ListFolderDefinition = dataCheck.Data.ListFolderDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
            ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();
            ResultApiModel resultProductApiModel = new ResultApiModel();
            resultApiModel = apiUtil.GetModuleInfo(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);
            resultProductApiModel = apiUtil.GetMosuleStand(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);
            ModuleModel moduleModel = new ModuleModel();

            List<ProductStandModel> productStandModel = new List<ProductStandModel>();
            if (resultApiModel.SuccessStatus)
            {
                moduleModel = resultApiModel.Data;
            }
            else
            {
                throw NTSException.CreateInstance(resultApiModel.Message);
            }

            if (resultProductApiModel.SuccessStatus)
            {
                productStandModel = JsonConvert.DeserializeObject<List<ProductStandModel>>(resultProductApiModel.Data.ToString());
            }
            else
            {
                throw NTSException.CreateInstance(resultProductApiModel.Message);
            }

            string fileName = "HS.";
            string selectedPath = Constants.Disk_Design + fileService.GetPathFileByName(modelTest.FileName, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
            string selectedPathDownload = Constants.Disk_Design + fileService.GetPathFileByName(fileName, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);

            modelTest.Path = selectedPath;
            Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, $"modelTest.Path: {modelTest.Path}");

            modelTest.PathFolder = selectedPathDownload;
            modelTest.ProductCode = moduleModel.Code;
            modelTest.ProductName = moduleModel.Name;
            Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, $"modelTest.Path: xxxx");
            modelTest.ListProduct = productStandModel;
            GeneralTemplateMechanicalRecord(modelTest);
        }

        public void GeneralTemplateMechanicalRecord(MechanicalRecordsModel model)
        {
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, $"run here : 00    ");

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(Application.StartupPath + "/Template/HoSoThietKe_CoKhi.xlsm");

                IWorksheet sheet = workbook.Worksheets[0];
                var now = DateTime.Today;
                sheet.PageSetup.LeftHeader = model.ProductCode + " - " + model.ProductName;
                sheet.PageSetup.RightHeader = model.UserName;


                var date = DateTime.Now;

                IRange iRangeData1 = sheet.FindFirst("<day>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData1.Text = iRangeData1.Text.Replace("<day>", date.Day.ToString());
                IRange iRangeData2 = sheet.FindFirst("<month>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData2.Text = iRangeData2.Text.Replace("<month>", date.Month.ToString());
                IRange[] iRangeData3 = sheet.FindAll("<year>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                for (int i = 0; i < iRangeData3.Length; i++)
                {
                    iRangeData3[i].Text = iRangeData3[i].Text.Replace("<year>", date.Year.ToString());
                }
                sheet.PageSetup.CenterFooter = "Date modified: " + date.ToString();

                sheet[5, 3].Value = model.ProductName;
                sheet[6, 3].Value = model.ProductCode;
                sheet[7, 3].Value = model.UserName;

                //sheet[9, 1].Value = "Tân Phát - " + DateTime.Now.Year;

                sheet[14, 3].Value = model.check_14_3 == true ? "VT." + model.ProductCode + ".xlsm" : "Không có";
                sheet[15, 3].Value = model.check_15_3 == true ? "DOC." + model.ProductCode : "Không có";
                sheet[16, 3].Value = model.check_16_3 == true ? "HS." + model.ProductCode + ".xlsx" : "Không có";
                sheet[17, 3].Value = model.check_17_3 == true ? "PTTK." + model.ProductCode + ".docm" : "Không có";
                sheet[18, 3].Value = model.check_18_3 == true ? "3D." + model.ProductCode : "Không có";
                sheet[19, 3].Value = model.check_19_3 == true ? "CAD." + model.ProductCode : "Không có";
                sheet[20, 3].Value = model.check_20_3 == true ? "MAT." + model.ProductCode : "Không có";
                sheet[21, 3].Value = model.check_21_3 == true ? "DATA1.Ck." + model.ProductCode : "Không có";
                sheet[22, 3].Value = model.check_22_3 == true ? "IGS." + model.ProductCode : "Không có";
                sheet[23, 3].Value = model.check_23_3 == true ? "LRP." + model.ProductCode : "Không có";
                sheet[24, 3].Value = model.check_24_3 == true ? "DAT." + model.ProductCode : "Không có";
                sheet[25, 3].Value = model.check_25_3 == true ? "TL." + model.ProductCode : "Không có";
                sheet[26, 3].Value = model.check_26_3 == true ? "KN." + model.ProductCode : "Không có";

                sheet[27, 3].Value = model.check_27_3 == true ? model.ProductCode : "Không có";
                sheet[28, 3].Value = model.check_28_3 == true ? "DAT." + model.ProductCode : "Không có";
                sheet[29, 3].Value = model.check_29_3 == true == true ? "PRJ." + model.ProductCode : "Không có";
                sheet[30, 3].Value = model.check_30_3 == true ? model.ProductCode + ".Dn.pdf" : "Không có";
                Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, "run here: 1");

                //Thêm mạch vào trong hồ sơ thiết kế
                string pathProduct = model.Path;
                string materialCode = string.Empty;
                List<MechanicalRecordsModel> list = new List<MechanicalRecordsModel>();
                MechanicalRecordsModel newModel;
                string materialName = string.Empty;
                List<int> rowData = new List<int>();
                if (!System.IO.File.Exists(pathProduct))
                {
                    pathProduct = "";
                    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                }
                else
                {
                    using (Stream fs = File.OpenRead(pathProduct))
                    {
                        ExcelEngine excelEngine1 = new ExcelEngine();
                        IApplication application1 = excelEngine1.Excel;
                        IWorkbook workbook_1 = application1.Workbooks.Open(fs);
                        IWorksheet sheet_1 = workbook_1.Worksheets[0];
                        Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, "run here: 2");

                        try
                        {
                            int rowCount = sheet_1.Rows.Count();
                            for (int i = 7; i < rowCount; i++)
                            {
                                materialName = sheet_1[i, 2].Text;
                                materialCode = sheet_1[i, 4].Text;
                                try
                                {
                                    if (!string.IsNullOrEmpty(materialCode))
                                    {
                                        if (materialCode.ToUpper().StartsWith("PCB"))
                                        {
                                            newModel = new MechanicalRecordsModel();
                                            newModel.Name = materialName;
                                            newModel.Code = materialCode;
                                            list.Add(newModel);
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            workbook_1.Close();
                            excelEngine1.Dispose();
                            Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, "run here: 3");

                            throw new Exception("Có lỗi phát sinh trong quá trình xử lý");
                            throw (ex);
                        }
                        workbook_1.Close();
                        excelEngine1.Dispose();
                    }

                    var total = list.Count;
                    var listExport = list.Select((a, i) => new
                    {
                        Index = "3." + (i + 1),
                        a.Name, // tên
                        a.Code, // mã
                    }).ToList();
                    Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, "run here: 4");

                    //sheet[36 + model.ListProduct.Count + listExport.Count, 3].Value = "Tân Phát,  ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;

                    if (listExport.Count() <= 0)
                    {
                        IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                        iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                    }

                    if (listExport.Count() > 0)
                    {
                        IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                        iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                        sheet.InsertRow(iRangeData.Row + 1, listExport.Count(), ExcelInsertOptions.FormatAsBefore);
                        sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders.Color = ExcelKnownColors.Black;
                        sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 3].CellStyle.WrapText = true;
                    }

                }
                Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, "run here: 5");

                if (model.ListProduct.Count == 0)
                {
                    IRange iRangeDatas = sheet.FindFirst("<DataStand>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeDatas.Text = iRangeDatas.Text.Replace("<DataStand>", string.Empty);
                }

                if (model.ListProduct.Count > 0)
                {
                    IRange iRangeDatas = sheet.FindFirst("<DataStand>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeDatas.Text = iRangeDatas.Text.Replace("<DataStand>", string.Empty);


                    var total = model.ListProduct.Count;

                    var listExports = model.ListProduct.Select((a, i) => new
                    {
                        Index = i + 1,
                        a.Name,
                        a.Code,
                    });


                    if (listExports.Count() > 1)
                    {
                        sheet.InsertRow(iRangeDatas.Row + 1, listExports.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                    }
                    sheet.ImportData(listExports, iRangeDatas.Row, iRangeDatas.Column, false);
                    sheet.Range[iRangeDatas.Row, 1, iRangeDatas.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDatas.Row, 1, iRangeDatas.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDatas.Row, 1, iRangeDatas.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDatas.Row, 1, iRangeDatas.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeDatas.Row, 1, iRangeDatas.Row + total - 1, 3].Borders.Color = ExcelKnownColors.Black;
                    sheet.Range[iRangeDatas.Row - 1, 1, iRangeDatas.Row + total - 1, 3].CellStyle.WrapText = true;

                }
                Log.WriteTracerProcessData(0, "GeneralTemplateMechanicalRecord", 0, "run here: 6");

                string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");

                string pathFileSave = model.PathFolder;
                workbook.SaveAs(pathFileSave);
                workbook.Close();

            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
        }
        #endregion

        #region Xuất biếu mẫu - Lấy file DMVT ở local
        public List<MaterialModel> GetDMVT(TestDesignStructureModel modelTest)
        {
            ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();
            string selectedPath = modelTest.SelectedPath;



            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (!selectedPath.StartsWith(Constants.Disk_Start_Design))
                {
                    throw NTSException.CreateInstance("Chọn sai thư mục");
                }

                modelTest.ModuleCode = Path.GetFileNameWithoutExtension(selectedPath);
            }

            resultApiModel = apiUtil.GetModuleInfo(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);

            ModuleModel moduleModel = new ModuleModel();
            if (!resultApiModel.SuccessStatus)
            {
                throw NTSException.CreateInstance(resultApiModel.Message);
            }

            moduleModel = resultApiModel.Data;

            modelTest.ModuleGroupCode = moduleModel.ModuleGroupCode;

            UploadFolderModel model = new UploadFolderModel();

            // Lấy dữ liệu từ server
            var resultAPi = apiUtil.GetDataDefinitions(modelTest.ApiUrl, modelTest.Token, modelTest.Type, Constants.Definition_ObjectType_Module);

            if (!resultAPi.SuccessStatus)
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
            }

            var dataCheck = resultAPi.Data;
            model.ListFileDefinition = dataCheck.ListFileDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
            model.ListFolderDefinition = dataCheck.ListFolderDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();

            if (string.IsNullOrEmpty(selectedPath))
            {
                selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                if (!Directory.Exists(selectedPath))
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, selectedPath);
                }

                modelTest.SelectedPath = selectedPath;
            }

            modelTest.PathFileMaterial = Constants.Disk_Design + fileService.GetPathFile(Constants.FileDefinition_FileType_ListMaterial, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);

            string moduleCode = string.Empty;
            var listMaterial = LoadFullDMVT(modelTest.PathFileMaterial, out moduleCode);
            var result = listMaterial.Where(t => !t.Code.ToLower().StartsWith("tpa") && (!t.ManufactureCode.ToLower().Equals("tpa") || !t.ManufactureCode.ToLower().Equals("yfs"))).Select(t => new MaterialModel
            {
                Stt = t.Stt,
                Name = t.Name,
                Specification = t.Specification,
                Code = t.Code,
                RawMaterialCode = t.RawMaterialCode,
                DV = t.DV,
                SL = t.SL,
                VL = t.VL,
                KL = t.KL,
                ManufactureCode = t.ManufactureCode,
                Note = t.Note
            }).ToList();
            return result;
        }
        #endregion

        #region Lấy username pc
        public string GetUserNamePC()
        {
            string userNamePC = Environment.MachineName;
            return userNamePC;
        }
        #endregion

        public void DownloadFile3DMaterial(TestDesignStructureModel model)
        {
            model.List3D = apiUtil.GetListDesign3D(model.ApiUrl, model.Token).Data;
            var file = model.List3D.FirstOrDefault(i => i.FileName.Equals(model.FileName));
            var folder = Path.GetDirectoryName(model.PathFileMaterial);
            if (file != null)
            {
                var data = apiUtil.DownloadFile(model.FileApiUrl, file.FilePath, folder, file.FileName);
                if (!data)
                {
                    throw NTSException.CreateInstance("Không có file!");
                }
                //if (File.Exists(model.PathFileMaterial))
                //{
                //    File.Delete(model.PathFileMaterial);
                //}
            }
            else
            {
                throw new Exception();
            }
        }

        public void DownloadFileModuleDocument(DownloadFileDesignDocumentModel model)
        {
            ModuleDesignDocument dowloadFileGoogle = new ModuleDesignDocument();
            DownloadFileModel downloadFileModel = new DownloadFileModel();
            var path = GetPathModuleDowload(model);
            downloadFileModel.DownloadPath = path + "\\TPA";
            var data = model.ListDocumentFileSize.FirstOrDefault(i => i.FilePath.Equals(model.FilePath));
            if (data != null)
            {
                downloadFileModel.ApiUrl = model.ApiUrl;
                downloadFileModel.ServerPath = data.ServerPath;
                downloadFileModel.Name = data.Name;
                downloadFileModel.Token = model.Token;
                var resultModel = dowloadFileGoogle.DownloadFileDesignDocument(downloadFileModel);
            }
        }

        public string GetPathModule(TestDesignStructureModel modelTest)
        {
            ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();
            string selectedPath = modelTest.SelectedPath;

            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (!selectedPath.StartsWith(Constants.Disk_Start_Design))
                {
                    throw NTSException.CreateInstance("Chọn sai thư mục");
                }

                modelTest.ModuleCode = Path.GetFileNameWithoutExtension(selectedPath);
            }

            resultApiModel = apiUtil.GetModuleInfo(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);

            ModuleModel moduleModel = new ModuleModel();
            if (resultApiModel.SuccessStatus)
            {
                moduleModel = resultApiModel.Data;
            }
            else
            {
                throw NTSException.CreateInstance(resultApiModel.Message);
            }

            modelTest.ModuleGroupCode = moduleModel.ModuleGroupCode;

            FileService fileService = new FileService();
            UploadFolderModel model = new UploadFolderModel();

            try
            {
                // Lấy dữ liệu từ server
                var resultAPi = apiUtil.GetData(modelTest.ApiUrl, modelTest.Token, modelTest.Type);

                if (!resultAPi.SuccessStatus)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
                }

                var dataCheck = resultAPi.Data;
                model.ListFileDefinition = dataCheck.ListFileDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
                model.ListFolderDefinition = dataCheck.ListFolderDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
                if (string.IsNullOrEmpty(selectedPath))
                {
                    selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                    if (!Directory.Exists(selectedPath))
                    {
                        throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, selectedPath);
                    }

                    modelTest.SelectedPath = selectedPath;
                }
            }
            catch (Exception ex) { }

            return selectedPath;
        }

        public string GetPathProduct(UploadProductFolderModel modelTest)
        {
            string selectedPath = string.Empty;

            FileService fileService = new FileService();
            UploadFolderModel model = new UploadFolderModel();

            try
            {
                // Lấy dữ liệu từ server
                var resultData = apiUtil.GetUploadProductData(modelTest.ApiUrl, modelTest.Token, modelTest.ProductId, modelTest.DesignType);
                if (!resultData.SuccessStatus)
                {
                    throw NTSException.CreateInstance(resultData.Message);
                }

                var dataCheck = resultData.Data;
                model.ListFileDefinition = dataCheck.FileDefinitions.Where(r => r.TypeDefinitionId == modelTest.DesignType).ToList();
                model.ListFolderDefinition = dataCheck.FolderDefinitions.Where(r => r.TypeDefinitionId == modelTest.DesignType).ToList();
                if (string.IsNullOrEmpty(selectedPath))
                {
                    selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, dataCheck.Product.Code, dataCheck.Product.GroupCode, dataCheck.Product.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                    if (!Directory.Exists(selectedPath))
                    {
                        throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, selectedPath);
                    }
                }
            }
            catch (Exception ex) { }

            return selectedPath;
        }

        public string GetPathClassRoom(UploadClassRoomFolderModel modelTest)
        {
            string selectedPath = string.Empty;

            FileService fileService = new FileService();
            UploadFolderModel model = new UploadFolderModel();

            try
            {
                // Lấy dữ liệu từ server
                var resultData = apiUtil.GetUploadClassRoomData(modelTest.ApiUrl, modelTest.Token, modelTest.ClassRoomId, modelTest.DesignType);
                if (!resultData.SuccessStatus)
                {
                    throw NTSException.CreateInstance(resultData.Message);
                }

                var dataCheck = resultData.Data;
                model.ListFileDefinition = dataCheck.FileDefinitions.Where(r => r.TypeDefinitionId == modelTest.DesignType).ToList();
                model.ListFolderDefinition = dataCheck.FolderDefinitions.Where(r => r.TypeDefinitionId == modelTest.DesignType).ToList();
                if (string.IsNullOrEmpty(selectedPath))
                {
                    selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, dataCheck.ClassRoom.Code, dataCheck.ClassRoom.GroupCode, dataCheck.ClassRoom.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                    if (!Directory.Exists(selectedPath))
                    {
                        throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, selectedPath);
                    }
                }
            }
            catch (Exception ex) { }

            return selectedPath;
        }
        public string GetPathSolution(UploadSolutionFolderModel modelTest)
        {
            string selectedPath = string.Empty;

            FileService fileService = new FileService();
            UploadFolderModel model = new UploadFolderModel();

            try
            {
                // Lấy dữ liệu từ server
                var resultData = apiUtil.GetUploadSolutionData(modelTest.ApiUrl, modelTest.Token, modelTest.SolutionId, modelTest.DesignType);
                if (!resultData.SuccessStatus)
                {
                    throw NTSException.CreateInstance(resultData.Message);
                }

                var dataCheck = resultData.Data;
                model.ListFileDefinition = dataCheck.FileDefinitions.Where(r => r.TypeDefinitionId == modelTest.DesignType).ToList();
                model.ListFolderDefinition = dataCheck.FolderDefinitions.Where(r => r.TypeDefinitionId == modelTest.DesignType).ToList();
                if (string.IsNullOrEmpty(selectedPath))
                {
                    selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_Upload, dataCheck.Solution.Code, dataCheck.Solution.GroupCode, dataCheck.Solution.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                    if (!Directory.Exists(selectedPath))
                    {
                        throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, selectedPath);
                    }
                }
            }
            catch (Exception ex) { }

            return selectedPath;
        }

        public List<ProductModuleModel> CompareFileIGS(TestDesignStructureModel modelTest)
        {
            var dataCheck = apiUtil.GetDataDefinitions(modelTest.ApiUrl, modelTest.Token, modelTest.Type, Constants.Definition_ObjectType_Module);

            if (!dataCheck.SuccessStatus)
            {
                throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
            }
            UploadFolderModel model = new UploadFolderModel();
            model.ListFileDefinition = dataCheck.Data.ListFileDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
            model.ListFolderDefinition = dataCheck.Data.ListFolderDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
            ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();
            List<ProductModuleModel> listData = new List<ProductModuleModel>();
            resultApiModel = apiUtil.GetModuleInfo(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);
            ModuleModel moduleModel = new ModuleModel();
            if (resultApiModel.SuccessStatus)
            {
                moduleModel = resultApiModel.Data;
            }
            else
            {
                throw NTSException.CreateInstance(resultApiModel.Message);
            }
            bool checkServer = true;
            string selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_IGS, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
            if (Directory.Exists(selectedPath))
            {
                string[] listTPA_File = Directory.GetFiles(selectedPath, "*.ipt", SearchOption.AllDirectories);
                foreach (string filePath in listTPA_File)
                {
                    FileInfo fInfo = new FileInfo(filePath);
                    if (fInfo.Extension != ".ipt") continue;
                    string fileName = Path.GetFileName(fInfo.Name);
                    string thisProductCode = fileName.Substring(0, 10);
                    string thisServerPath = string.Format("{0}.Ck\\IGS.{0}\\", modelTest.ModuleCode);
                    string[] splitFileName = fileName.Split('.');
                    if (splitFileName.Length == 2)//vd: tpad.a0001.ipt
                    {
                        continue;
                    }
                    if (splitFileName.Length == 3)
                    {
                        thisServerPath += fInfo.Name;
                    }
                    if (splitFileName.Length > 3)
                    {
                        string ne = thisProductCode;
                        for (int i = 2; i < splitFileName.Length; i++)
                        {
                            if (i == splitFileName.Length - 1)
                            {
                                thisServerPath += fInfo.Name;
                            }
                            else
                            {
                                ne += "." + splitFileName[i];
                                thisServerPath += ne + "\\";
                            }
                        }
                    }

                    ResultApiModel resultApiModels = apiUtil.CheckFileSize(modelTest.ApiUrl, thisServerPath, modelTest.ModuleCode, fInfo.Length, modelTest.Token);
                    if (resultApiModels.SuccessStatus)
                    {
                        if (!string.IsNullOrEmpty(resultApiModels.Message))
                        {
                            checkServer = false;
                        }
                    }
                    else
                    {
                        throw NTSException.CreateInstance(resultApiModels.Message);
                    }

                    if (!checkServer)
                    {
                        ResultApiModel resultApiModelData = apiUtil.GetListProductModule(modelTest.ApiUrl, moduleModel.Id, modelTest.Token);
                        if (resultApiModelData.SuccessStatus)
                        {
                            listData = JsonConvert.DeserializeObject<List<ProductModuleModel>>(resultApiModelData.Data.ToString());
                        }
                        else
                        {
                            throw NTSException.CreateInstance(resultApiModelData.ExceptionMessage);
                        }
                    }
                }
            }
            return listData;
        }
        public string GetPathModuleDowload(DownloadFileDesignDocumentModel modelTest)
        {
            ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();
            string selectedPath = "";

            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (!selectedPath.StartsWith(Constants.Disk_Start_Design))
                {
                    throw NTSException.CreateInstance("Chọn sai thư mục");
                }
            }

            resultApiModel = apiUtil.GetModuleInfo(modelTest.ApiUrl, modelTest.ModuleCode, modelTest.Token);

            ModuleModel moduleModel = new ModuleModel();
            if (resultApiModel.SuccessStatus)
            {
                moduleModel = resultApiModel.Data;
            }
            else
            {
                throw NTSException.CreateInstance(resultApiModel.Message);
            }

            modelTest.ModuleGroupCode = moduleModel.ModuleGroupCode;

            FileService fileService = new FileService();
            UploadFolderModel model = new UploadFolderModel();

            try
            {
                // Lấy dữ liệu từ server
                var resultAPi = apiUtil.GetData(modelTest.ApiUrl, modelTest.Token, modelTest.Type);

                if (!resultAPi.SuccessStatus)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
                }

                var dataCheck = resultAPi.Data;
                model.ListFileDefinition = dataCheck.ListFileDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
                model.ListFolderDefinition = dataCheck.ListFolderDefinition.Where(r => r.TypeDefinitionId == modelTest.Type).ToList();
                if (string.IsNullOrEmpty(selectedPath))
                {
                    selectedPath = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_TPA, modelTest.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, model.ListFolderDefinition, model.ListFileDefinition);
                    if (!Directory.Exists(selectedPath))
                    {
                        throw NTSException.CreateInstance(ErrorResourceKey.ERR0004, selectedPath);
                    }
                }
            }
            catch (Exception ex) { }

            return selectedPath;
        }
    }
}