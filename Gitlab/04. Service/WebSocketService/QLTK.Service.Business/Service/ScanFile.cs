using Aspose.BarCode.BarCodeRecognition;
using NTS.Common;
using NTS.Common.Resource;
using QLTK.Service.Business.Model;
using QLTK.Service.Business.Utilities;
using QLTK.Service.Common;
using QLTK.Service.Model.Api;
using QLTK.Service.Model.Files;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

namespace QLTK.Service.Business.Service
{
    public class ScanFile
    {
        string _productCode = "";
        string _pathfileJPG = "";
        string _groupCode = "";
        List<string> lstError = new List<string>();
        private ApiUtil apiUtil = new ApiUtil();
        //string _filePath = "";
        string _folderScan = Constants.Disk_Design + "Scan";
        public List<FileModel> ListFile = new List<FileModel>();
        public List<FolderScanModel> ListFolderScan = new List<FolderScanModel>();
        int _id = 1;
        string _pathTKC = "";

        public void loadFileWillRename()
        {
            if (!Directory.Exists(_folderScan))
            {
                Directory.CreateDirectory(_folderScan);
            }
            string[] listFiles = Directory.GetFiles(_folderScan);
            //string[] listFiles = Directory.GetFiles(_folderScan, "*.jpg|.pdf*", SearchOption.AllDirectories);
            if (listFiles.Count() > 0)
            {
                bool check = false;
                for (int i = 0; i < listFiles.Count(); i++)
                {
                    var end = Path.GetFileName(listFiles[i]);
                    if (end.Contains("jpg"))
                    {
                        check = true;
                    }
                    ListFile.Add(new FileModel
                    {
                        Id = i,
                        FileName = Path.GetFileName(listFiles[i]),
                        FilePath = listFiles[i],
                        //Base64 = check ? Convert.ToBase64String(File.ReadAllBytes(listFiles[i])) : "",
                        End = check
                    });

                    check = false;
                }
            }
        }

        public void CheckCode(ScanFileModel model)
        {
            if (!string.IsNullOrEmpty(model.ModuleCode))
            {
                _productCode = model.ModuleCode.Trim().ToUpper();
                FileService fileService = new FileService();
                var dataCheck = apiUtil.GetDataDefinitions(model.ApiUrl, model.Token, Constants.Design_Type_CK, Constants.Definition_ObjectType_Module);

                if (!dataCheck.SuccessStatus)
                {
                    throw NTSException.CreateInstance(ErrorResourceKey.ERR0003);
                }

                List<string> listError = new List<string>();

                ResultApiModel<ModuleModel> resultApiModel = new ResultApiModel<ModuleModel>();

                resultApiModel = apiUtil.GetModuleInfo(model.ApiUrl, model.ModuleCode, model.Token);
                ModuleModel moduleModel = new ModuleModel();
                if (resultApiModel.SuccessStatus)
                {
                    moduleModel = resultApiModel.Data;
                }
                else
                {
                    throw NTSException.CreateInstance(resultApiModel.Message);
                }
                _groupCode = moduleModel.ModuleGroupCode;
                _pathTKC = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_BCCk, model.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, dataCheck.Data.ListFolderDefinition, dataCheck.Data.ListFileDefinition);
                _pathfileJPG = Constants.Disk_Design + fileService.GetPathFolder(Constants.FolderDefinition_FolderType_BCCAD, model.ModuleCode, moduleModel.ModuleGroupCode, moduleModel.ParentGroupCode, dataCheck.Data.ListFolderDefinition, dataCheck.Data.ListFileDefinition);
                if (!Directory.Exists(_pathTKC))
                {
                    throw new Exception("Không tồn tại thiết kế cho sản phẩm này trong ổ D!");
                }
            }
            Refresh(model);
        }

        public void loadFileRenamed(ScanFileModel model)
        {
            if (!string.IsNullOrEmpty(model.ModuleCode))
            {

                getAllFolderNormal(_pathTKC, 0);

                for (int i = 0; i < ListFolderScan.Count; i++)
                {
                    string[] listFiles = null;
                    try
                    {
                        listFiles = Directory.GetFiles(ListFolderScan[i].FilePath.ToString());
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    foreach (string item in listFiles)
                    {
                        ListFolderScan.Add(new FolderScanModel
                        {
                            FileName = Path.GetFileName(item),
                            ParentID = ListFolderScan[i].ID,
                            ID = _id++,
                            FilePath = item
                        });
                    }
                }

                ListFolderScan.Add(new FolderScanModel
                {
                    FileName = Path.GetFileName(_pathTKC),
                    ParentID = 1000,
                    ID = 0,
                    FilePath = _pathTKC
                });
                _id = 1;
            }
        }
        List<FolderScanModel> getAllFolderNormal(string initPath, int parent)
        {
            string[] listInitPath = Directory.GetDirectories(initPath);
            int count = 0;
            try
            {
                count = listInitPath.Length;
            }
            catch { }

            if (count <= 0) return new List<FolderScanModel>();

            foreach (string item in listInitPath)
            {
                int countFolder = 0;
                int countFile = 0;
                try
                {
                    countFolder = Directory.GetDirectories(initPath + "/" + Path.GetFileName(item)).Length;
                }
                catch (Exception)
                {
                }
                try
                {
                    countFile = Directory.GetFiles(initPath + "/" + Path.GetFileName(item)).Length;
                }
                catch (Exception)
                {
                }

                ListFolderScan.Add(new FolderScanModel
                {
                    FileName = Path.GetFileName(item),
                    ParentID = parent,
                    ID = _id,
                    FilePath = item
                });

                _id++;
                if (countFolder > 0)
                {
                    getAllFolderNormal(initPath + "/" + Path.GetFileName(item), _id);
                }
            }
            return ListFolderScan;
        }

        public void ScanFileJPG(ScanFileModel model)
        {
            foreach (var item in ListFile)
            {
                try
                {
                    string filePath = item.FilePath;

                    string thisProductCode = "";

                    if (Path.GetExtension(filePath) == ".jpg")
                    {
                        //Image img = Image.FromFile(filePath);
                        //Bitmap mBitmap = new Bitmap(img);

                        //ArrayList barcodes = new ArrayList();
                        //BarcodeImaging.FullScanPage(ref barcodes, mBitmap, 100);
                        ////string[] barcodes = BarcodeReader.read(mBitmap, BarcodeReader.CODE39);
                        //mBitmap.Dispose();
                        //string contentBarcode = barcodes.ToArray().Where(o => o.ToString().Contains(model.ModuleCode)).ToArray()[0].ToString();//nội dung barcode chứa tên biểu mẫu (VD: 2.BM01.A0101)
                        //thisProductCode = contentBarcode;
                        //if (!thisProductCode.ToUpper().Contains(_productCode))
                        //{
                        //    continue;
                        //}

                        //string[] results = BarcodeReader.read(filePath, BarcodeReader.CODE39);
                        string contentBarcode = string.Empty;
                        using (BarCodeReader reader = new BarCodeReader(filePath, DecodeType.Code39Standard, DecodeType.Code128))
                        {
                            foreach (BarCodeResult result in reader.ReadBarCodes())
                            {
                                if (result.CodeText.Contains(model.ModuleCode))
                                {
                                    contentBarcode = result.CodeText;
                                    thisProductCode = contentBarcode;
                                }
                            }

                            reader.Dispose();

                            if (!thisProductCode.ToUpper().Contains(_productCode))
                            {
                                continue;
                            }

                        }

                        string destFileName = _pathfileJPG + "\\" + thisProductCode + ".jpg";
                        //@"D:\Thietke.Ck\TPAD." + _productCode.Substring(5, 1) + "\\" + _productCode + ".Ck\\BCCk."
                        //+ _productCode + "\\BC-CAD." + _productCode + "\\" + thisProductCode + ".jpg";
                        if (File.Exists(destFileName))
                        {
                            if (!model.CheckDelete)
                            {
                                model.CheckDelete = true;
                                break;
                            }
                            if (!Directory.Exists(_pathfileJPG))
                            {
                                Directory.CreateDirectory(_pathfileJPG);
                            }
                            File.Delete(destFileName);
                            File.Copy(filePath, destFileName, true);
                            //img.Dispose();
                            //mBitmap.Dispose();
                            File.Delete(filePath);
                        }
                        else
                        {
                            File.Copy(filePath, destFileName, true);
                            //img.Dispose();
                            //mBitmap.Dispose();
                            File.Delete(filePath);
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public void ScanFilePDF(ScanFileModel model)
        {
            foreach (var item in model.ListFileScan)
            {
                foreach (var items in ListFile)
                {
                    if (items.Id == model.FileId)
                    {
                        string filePath = items.FilePath;

                        string thisProductCode = "";
                        if (Path.GetExtension(filePath).ToLower() == ".pdf")
                        {
                            //System.Collections.ArrayList barcodes = new System.Collections.ArrayList();
                            //string outputFilePath = Path.GetDirectoryName(filePath) + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".png";
                            //pdfConverter().Convert(filePath, outputFilePath);

                            //Bitmap mBitmap = new Bitmap(outputFilePath);

                            //BarcodeImaging.FullScanPage(ref barcodes, mBitmap, 100);
                            ////string[] barcodes = BarcodeReader.read(mBitmap, BarcodeReader.CODE39);

                            //mBitmap.Dispose();
                            //File.Delete(outputFilePath);

                            //string contentBarcode = barcodes.ToArray().Where(o => o.ToString().StartsWith("2.")).ToArray()[0].ToString();//nội dung barcode chứa tên biểu mẫu (VD: 2.BM01.A0101)
                            //string templateCode = contentBarcode.Split('.')[1];
                            //TemplateModel temModel = (TemplateModel)TemplateBO.Instance.FindByAttribute("Code", templateCode)[0];

                            int tempType = 1;
                            string code = model.ModuleCode; //contentBarcode.Split('.')[2];
                            if (tempType == 1 || tempType == 2) //1:cơ khí,2:điện,3:điện tử
                            {
                                thisProductCode = code;
                            }
                            else
                            {
                                thisProductCode = code;
                            }

                            if (_productCode != thisProductCode)
                            {
                                continue;
                            }
                            string tempName = item.Name.Replace("code", thisProductCode);
                            string tempFolderPath = item.PathFolderC.Replace("code", thisProductCode);

                            if (tempType == 1)
                            {
                                string destFileName = _pathTKC + "\\" + tempFolderPath + "\\" + tempName + Path.GetExtension(filePath);
                                checkBeforeCopyFile(filePath, destFileName);
                            }
                        }
                    }
                }
            }
        }

        void checkBeforeCopyFile(string sourceFileName, string destFileName)
        {
            try
            {
                if (File.Exists(destFileName))
                {
                    //if (true)
                    //{
                    File.Delete(destFileName);
                    File.Copy(sourceFileName, destFileName);
                    File.Delete(sourceFileName);
                    //}
                    //else
                    //{
                    //    RenameFileVB(sourceFileName, Path.GetFileName(destFileName));
                    //}
                }
                else
                {
                    File.Copy(sourceFileName, destFileName);
                    File.Delete(sourceFileName);
                }
            }
            catch (Exception ex)
            {
                //TextUtils.ShowError(ex);
            }

        }

        public static void RenameFileVB(string filePath, string newFileName)
        {
            if (Path.GetFileName(filePath).ToUpper() == newFileName.ToUpper()) return;
            (new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.RenameFile(filePath, newFileName);
        }

        public void Refresh(ScanFileModel model)
        {
            loadFileWillRename();
            loadFileRenamed(model);
        }

        public void DeleteFile(string Path)
        {
            try
            {
                if (Directory.Exists(Path))
                {
                    Directory.Delete(Path, true);
                }
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteListFile(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetBase64(string Path)
        {
            var link = Convert.ToBase64String(File.ReadAllBytes(Path));
            return link;
        }

        public void MoveFile(MoveFileModel model)
        {
            string destFileName = model.PathForder + "\\" + model.Name;
            checkBeforeCopyFile(model.Path, destFileName);
        }
    }
}
