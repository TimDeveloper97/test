using Ionic.Zip;
using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.ConverUnit;
using NTS.Model.DMVTImportSAP;
using NTS.Model.MaterialBuyHistory;
using NTS.Model.ModuleMaterials;
using NTS.Model.ProductMaterials;
using NTS.Model.Project;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using QLTK.Business.Materials;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.DMVTimportSAP
{
    public class DMVTImportSAPBussiness
    {
        QLTKEntities db = new QLTKEntities();

        public SearchResultModel<DesignModuleModel> GetModuleInProjectProductByProjectId(DesignModuleInfoModel model)
        {
            SearchResultModel<DesignModuleModel> searchResult = new SearchResultModel<DesignModuleModel>();

            var ListModule = (from a in db.ProjectProducts.AsNoTracking()
                              join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                              join c in db.Modules.AsNoTracking() on a.ModuleId equals c.Id
                              join d in db.ProjectGeneralDesignModules.AsNoTracking() on a.Id equals d.ProjectProductId
                              where a.ProjectId.Equals(model.ProjectId) && !model.ListIdSelect.Contains(a.Id) && d.Quantity > 0
                              select new DesignModuleModel()
                              {
                                  Id = c.Id,
                                  ModuleName = c.Name,
                                  ModuleCode = c.Code,
                                  Quantity = d.Quantity,
                                  RealQuantity = d.RealQuantity,
                                  ProjectCode = b.Code,
                                  WarehouseCode = b.WarehouseCode,
                                  ParentId = a.ParentId,
                                  ProjectProductId = a.Id
                              }).AsQueryable();

            if (!string.IsNullOrEmpty(model.ProjectProductId))
            {
                ListModule = ListModule.Where(r => model.ProjectProductId.Equals(r.ParentId) || model.ProjectProductId.Equals(r.ProjectProductId));
            }

            searchResult.ListResult = ListModule.ToList();
            searchResult.TotalItem = ListModule.Count();

            return searchResult;
        }


        #region Xuất DMVT SAP
        public string GenerateMaterialSAP(DesignModuleInfoModel model, string userLoginId)
        {
            var project = db.Projects.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ProjectId));

            if (project == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Project);
            }

            List<string> ListfileZip = new List<string>();

            List<DesignMaterialExportModel> materialExport = new List<DesignMaterialExportModel>();

            string date = DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss");

            string exportPath = $"Template/Export/{model.ProjectId}_{date}";

            string newPath = HttpContext.Current.Server.MapPath("~/" + exportPath);
            Directory.CreateDirectory(newPath);

            int index = 1;
            foreach (var module in model.Modules)
            {
                var materials = GetModuleMaterials(module.Id, string.Empty);

                module.Materials = GenerateMaterials(materials, index.ToString(), module.RealQuantity);

                materialExport.AddRange(ExportModuleMaterials(module, index.ToString(), project.Code, newPath, false));
                index++;
            }

            var projectProduct = db.ProjectProducts.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ProjectProductId));

            UserLogUtil.LogHistotyAdd(db, userLoginId, project.Code, project.Id, Constants.LOG_DataDistribution);

            ExportBOM(materialExport, newPath, projectProduct);
            return Path.Combine(exportPath, ZipDMVTSAP(newPath, date, project.Code));

        }

        public List<DesignMaterialModel> GetModuleMaterials(string moduleId, string indexParent)
        {
            indexParent = string.IsNullOrEmpty(indexParent) ? string.Empty : indexParent + ".";

            var moduleMaterials = (from a in db.ModuleMaterials.AsNoTracking()
                                   join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                                   join g in db.MaterialGroups.AsNoTracking() on c.MaterialGroupId equals g.Id
                                   join t in db.MaterialGroupTPAs.AsNoTracking() on g.MaterialGroupTPAId equals t.Id
                                   where a.ModuleId.Equals(moduleId)
                                   orderby a.MaterialCode
                                   select new
                                   {
                                       Index = indexParent + a.Index,
                                       MaterialName = a.MaterialName,
                                       Parameter = a.Specification,
                                       MaterialCode = a.MaterialCode,
                                       RawMaterialCode = a.RawMaterialCode,
                                       Unit = a.UnitName,
                                       Quantity = a.Quantity,
                                       RawMaterial = a.RawMaterial,
                                       Weight = a.Weight,
                                       Manufacturer = a.ManufacturerCode,
                                       Note = a.Note,
                                       GroupCode = t.Code,
                                       c.Pricing,
                                       c.PriceHistory,
                                       c.LastBuyDate,
                                       c.InputPriceDate
                                   }).ToList();

            List<DesignMaterialModel> materials = new List<DesignMaterialModel>();
            DesignMaterialModel materialModel;
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var material in moduleMaterials)
            {
                materialModel = new DesignMaterialModel()
                {
                    Index = material.Index,
                    MaterialName = material.MaterialName,
                    Parameter = material.Parameter,
                    MaterialCode = material.MaterialCode,
                    RawMaterialCode = material.RawMaterialCode,
                    Unit = material.Unit,
                    Quantity = material.Quantity,
                    RawMaterial = material.RawMaterial,
                    Weight = material.Weight,
                    Manufacturer = material.Manufacturer,
                    Note = material.Note,
                    GroupCode = material.GroupCode,
                    Price = material.Pricing
                };

                if (material.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(material.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        materialModel.Price = material.PriceHistory;
                    }
                    else if (!material.InputPriceDate.HasValue || material.InputPriceDate.Value.Date < material.LastBuyDate.Value.Date)
                    {
                        materialModel.Price = 0;
                    }
                }

                materials.Add(materialModel);

                if (material.MaterialCode.StartsWith("TPA") || material.MaterialCode.StartsWith("PCB") && material.Unit.ToUpper().Equals("BỘ"))
                {
                    materials.AddRange(GetModuleMaterialChild(material.MaterialCode, material.Index));
                }
            }

            return materials;
        }

        public List<DesignMaterialModel> GetModuleMaterialChild(string materialCode, string indexParent)
        {
            List<DesignMaterialModel> materials = new List<DesignMaterialModel>();
            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToUpper().Equals(materialCode.ToUpper()));

            if (module != null)
            {
                materials = GetModuleMaterials(module.Id, indexParent);
            }

            return materials;
        }

        public List<DesignMaterialModel> GenerateMaterials(List<DesignMaterialModel> materials, string moduleIndex, decimal moduleQuantity)
        {
            materials = SortMaterials(materials);

            string indexHan = string.Empty;

            decimal parentQuantity = 0;
            for (int i = 0; i < materials.Count; i++)
            {
                if (!string.IsNullOrEmpty(indexHan))
                {
                    if (CheckChild(materials[i].Index, indexHan))
                    {
                        materials.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(materials[i].Parameter) && "HÀN".Equals(materials[i].Parameter.Trim().ToUpper()))
                {
                    indexHan = materials[i].Index;
                }
                else
                {
                    indexHan = string.Empty;
                }

                parentQuantity = GetParentQuantity(materials, materials[i].Index);

                materials[i].ModuleQuantity = materials[i].Quantity * parentQuantity * moduleQuantity;
            }

            foreach (var material in materials)
            {
                material.ModuleIndex = moduleIndex + "." + material.Index;
            }

            return materials;
        }

        public List<DesignMaterialModel> GenerateMaterialProjectGarenalDesign(List<DesignMaterialModel> materials, string moduleIndex, decimal moduleQuantity)
        {
            //materials = SortMaterials(materials);

            string indexHan = string.Empty;

            decimal parentQuantity = 0;
            for (int i = 0; i < materials.Count; i++)
            {
                if (!string.IsNullOrEmpty(indexHan))
                {
                    if (CheckChild(materials[i].Index, indexHan))
                    {
                        materials.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(materials[i].Parameter) && "HÀN".Equals(materials[i].Parameter.Trim().ToUpper()))
                {
                    indexHan = materials[i].Index;
                }
                else
                {
                    indexHan = string.Empty;
                }

                parentQuantity = GetParentQuantity(materials, materials[i].Index);

                materials[i].ModuleQuantity = materials[i].Quantity * parentQuantity * moduleQuantity;
            }

            foreach (var material in materials)
            {
                material.ModuleIndex = moduleIndex + "." + material.ModuleIndex;
            }

            return materials;
        }

        public static List<DesignMaterialModel> SortMaterials(List<DesignMaterialModel> data)
        {
            try
            {
                int maxLen = data.AsEnumerable().Select(s => s.Index.Length).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                return data.AsEnumerable()
                        .Select(s =>
                            new
                            {
                                OrgStr = s,
                                SortStr = Regex.Replace(s.Index.ToString(), @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                            })
                        .OrderBy(x => x.SortStr)
                        .Select(x => x.OrgStr).ToList();
            }
            catch
            {
                return data;
            }
        }

        private bool CheckChild(string indexChild, string indexParent)
        {
            if (indexChild.Length > indexParent.Length && indexChild.Trim().Substring(0, indexParent.Length + 1).Equals(indexParent + "."))
            {
                return true;
            }

            return false;
        }

        public decimal GetParentQuantity(List<DesignMaterialModel> materials, string indexChild)
        {
            string indexParent = GetIndexParent(indexChild);

            if (string.IsNullOrEmpty(indexParent))
            {
                return 1;
            }

            var parent = materials.FirstOrDefault(r => r.Index.Equals(indexParent));

            if (parent != null)
            {
                return parent.Quantity * GetParentQuantity(materials, parent.Index);
            }

            return 1;
        }

        public decimal GetParentQuantityModule(List<DesignModuleModel> moudles, string indexChild)
        {
            string indexParent = GetIndexParent(indexChild);

            if (string.IsNullOrEmpty(indexParent))
            {
                return 1;
            }

            var parent = moudles.FirstOrDefault(r => r.Index.Equals(indexParent));

            if (parent != null)
            {
                return parent.Quantity * GetParentQuantityModule(moudles, parent.Index);
            }

            return 1;
        }

        public string GetIndexParent(string indexChild)
        {
            if (indexChild.LastIndexOf(".") != -1)
            {
                return indexChild.Substring(0, indexChild.LastIndexOf("."));
            }

            return string.Empty;
        }

        public List<DesignMaterialExportModel> ExportModuleMaterials(DesignModuleModel module, string moduleIndex, string projectCode, string path, bool isMaterialSub)
        {

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/BOMTemplate.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", (string.Empty));

            List<DesignMaterialExportModel> materialExport = new List<DesignMaterialExportModel>();

            string moduleCode = module.ModuleCode + (isMaterialSub ? string.Empty : "." + projectCode);
            materialExport.Add(new DesignMaterialExportModel
            {
                ProjectCode = module.ProjectCode,
                IndexDesign = string.Empty,
                IndexSAP = moduleIndex,
                //IndexStandard = string.Empty,
                MaterialName = module.ModuleName,
                Prameter = module.Parameter,
                MaterialCode = moduleCode,
                Unit = "BỘ",
                Quantity = module.Quantity,
                RawMaterial = string.Empty,
                Manufacturer = Constants.Manufacture_TPA,
                Note = string.Empty,
                GroupCode = "101",
                Price = default(decimal?),
                StoreCode = module.WarehouseCode,
                LeadTime = 7,
                MakeBuy = "M",
                ModuleQuantity = module.RealQuantity,
            });

            materialExport.AddRange(
                module.Materials.Select(s => new DesignMaterialExportModel
                {
                    ProjectCode = module.ProjectCode,
                    IndexDesign = s.Index,
                    IndexSAP = s.ModuleIndex,
                    //IndexStandard = string.Empty,
                    MaterialName = s.MaterialName,
                    Prameter = s.Parameter,
                    MaterialCode = s.MaterialCode.ToUpper().StartsWith("PCB") ? s.MaterialCode + "." + projectCode : s.MaterialCode,
                    Unit = s.Unit,
                    Quantity = s.Quantity,
                    RawMaterial = s.RawMaterial,
                    Manufacturer = s.Manufacturer,
                    Note = s.Note,
                    Price = s.Price,
                    GroupCode = s.GroupCode,
                    StoreCode = module.WarehouseCode,
                    LeadTime = 7,
                    MakeBuy = !string.IsNullOrEmpty(s.Parameter) && s.Parameter.ToUpper().Equals("TPA") && !string.IsNullOrEmpty(s.Manufacturer) && s.Parameter.ToUpper().Equals("TPA") ? "M" : "B",
                    ModuleQuantity = s.ModuleQuantity,
                }).ToList());

            sheet.ImportData(materialExport, iRangeData.Row, iRangeData.Column, false);


            string pathExport = $"{path}/BOM.{module.ModuleCode}.xlsx";

            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            return materialExport;
        }

        public void ExportBOM(List<DesignMaterialExportModel> materials, string path, ProjectProduct projectProduct)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/BOMTemplate.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", (string.Empty));

            string fileName = "BOM.xlsx";
            if (projectProduct != null)
            {
                sheet[iRangeData.Row - 1, 4].Value = projectProduct.ContractName;
                sheet[iRangeData.Row - 1, 6].Value = projectProduct.ContractCode;
                sheet[iRangeData.Row - 1, 7].Value = "BỘ";
                sheet[iRangeData.Row - 1, 8].Value2 = projectProduct.Quantity;
                sheet[iRangeData.Row - 1, 17].Value2 = projectProduct.RealQuantity;
                if (!string.IsNullOrEmpty(projectProduct.ContractCode))
                {
                    fileName = $"BOM.{projectProduct.ContractCode}.xlsx";
                }
            }

            List<DesignMaterialExportModel> materialExport = new List<DesignMaterialExportModel>();

            string pathExport = $"{path}/{fileName}";
            sheet.ImportData(materials, iRangeData.Row, iRangeData.Column, false);
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();
        }

        /// <summary>
        /// Tạo file zip BOM
        /// </summary>
        /// <param name="ListDatashet"></param>
        /// <param name="path"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string ZipDMVTSAP(string path, string date, string projectCode)
        {
            string[] fileBOMModule = Directory.GetFiles(path);

            int countErro = 0;
            var result = new ResultDownload();
            string tempError = string.Empty;
            var listString = new List<string>();

            if (fileBOMModule.Length == 0)
            {
                throw NTSException.CreateInstance("Không có DMVT để tạo BOM");
            }

            string fileName = "BOM_" + projectCode + ".zip";
            string pathZip = Path.Combine(path, fileName);
            try
            {
                string tempPath = string.Empty;

                for (int i = 0; i < fileBOMModule.Length; i++)
                {
                    tempPath = fileBOMModule[i];
                    if (!File.Exists(fileBOMModule[i]))
                    {
                        countErro++;
                        tempError += "! ";
                        throw new FileNotFoundException();
                    }
                    else
                    {
                        listString.Add(fileBOMModule[i]);
                    }
                }

                using (var zip = new ZipFile())
                {
                    zip.AddFiles(listString, "BOM_" + projectCode);
                    zip.Save(pathZip);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new NTSLogException(null, ex);
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }

            if (!string.IsNullOrEmpty(tempError))
            {
                NtsLog.LogError(tempError);
                throw NTSException.CreateInstance("Tạo BOM không thành công");
            }

            return fileName;
        }
        #endregion Kết thúc

        public string ImportModule(string userId, HttpPostedFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (!extension.Equals(".xlsx") && !extension.Equals(".xls") && !extension.Equals(".xlsm"))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0038, TextResourceKey.File);
            }

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(file.InputStream);
            IWorksheet sheet = workbook.Worksheets[0];
            int rowCount = sheet.Rows.Count();

            string projectCode = sheet[3, 2].Value;

            List<DesignModuleModel> modules = new List<DesignModuleModel>();
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            List<int> codeErrorIndex = new List<int>();
            List<int> quantityErrorIndex = new List<int>();
            List<int> typeErrorIndex = new List<int>();
            DesignModuleModel moduleModel;
            Module moduleExist;
            for (int i = 6; i <= rowCount; i++)
            {
                if (!string.IsNullOrEmpty(sheet[i, 1].Value) && !string.IsNullOrEmpty(sheet[i, 2].Value) && !string.IsNullOrEmpty(sheet[i, 3].Value) && !string.IsNullOrEmpty(sheet[i, 4].Value))
                {
                    moduleModel = new DesignModuleModel()
                    {
                        ModuleCode = sheet[i, 3].Value.Trim().Replace("\t", ""),
                        Index = sheet[i, 1].Value,
                        ModuleName = sheet[i, 2].Value,
                        ProjectCode = projectCode
                    };

                    try
                    {
                        moduleModel.Quantity = int.Parse(sheet[i, 4].Value);
                    }
                    catch
                    {
                        moduleModel.Quantity = 1;
                    }


                    moduleExist = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(moduleModel.ModuleCode.ToLower()));

                    if (moduleExist != null)
                    {
                        moduleModel.Materials = GetModuleMaterials(moduleExist.Id, string.Empty);
                    }

                    modules.Add(moduleModel);
                }
                else
                {
                    break;
                }
            }

            foreach (var module in modules)
            {
                module.RealQuantity = module.Quantity * GetParentQuantityModule(modules, module.Index);

                if (module.Materials.Count > 0)
                {
                    module.Materials = GenerateMaterials(module.Materials, module.Index, module.RealQuantity);
                }
            }

            if (modules.Count() > 0)
            {
                int maxLen = modules.Select(s => s.Index.Length).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                modules = modules
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }

            string date = DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss");
            string exportPath = $"Template/Export/{Guid.NewGuid().ToString()}_{date}";
            string newPath = HttpContext.Current.Server.MapPath("~/" + exportPath);

            List<DesignMaterialExportModel> materialExport = new List<DesignMaterialExportModel>();
            foreach (var module in modules)
            {
                materialExport.AddRange(ExportModuleMaterials(module, module.Index, projectCode, newPath, false));
            }

            ExportBOM(materialExport, newPath, null);

            return Path.Combine(exportPath, ZipDMVTSAP(newPath, date, projectCode));
        }
    }
}
