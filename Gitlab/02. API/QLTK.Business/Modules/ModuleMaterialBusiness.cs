using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Datasheet;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterials;
using NTS.Model.ProductMaterials;
using NTS.Model.ProjectProducts;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using NTS.Model.SimilarMaterial;
using NTS.Model.SimilarMaterialConfig;
using QLTK.Business.Materials;
using QLTK.Business.ProjectProducts;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using Syncfusion.XPS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ModuleMaterials
{
    public class ModuleMaterialBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchModuleMaterialResultModel<ModuleMaterialResultModel> SearchModuleMaterial(ModuleMaterialSearchModel modelSearch)
        {
            SearchModuleMaterialResultModel<ModuleMaterialResultModel> searchResult = new SearchModuleMaterialResultModel<ModuleMaterialResultModel>();
            var dataQuery = (from a in db.ModuleMaterials.AsNoTracking()
                                 //join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             //join m in db.Manufactures.AsNoTracking() on a.ManufacturerId equals m.Id
                             where a.ModuleId.Equals(modelSearch.ModuleId)
                             orderby a.MaterialCode
                             select new ModuleMaterialResultModel
                             {
                                 Id = a.Id,
                                 ModuleId = a.ModuleId,
                                 MaterialId = a.MaterialId,
                                 MaterialCode = a.MaterialCode,
                                 MaterialName = a.MaterialName,
                                 Specification = a.Specification,
                                 RawMaterialCode = a.RawMaterialCode,
                                 RawMaterial = a.RawMaterial,
                                 Price = a.Price,
                                 Quantity = a.Quantity,
                                 ReadQuantity = a.Quantity,
                                 Amount = a.Amount,
                                 Weight = a.Weight,
                                 ManufacturerId = a.ManufacturerId,
                                 ManufacturerCode = a.ManufacturerCode,
                                 Note = a.Note,
                                 UnitName = a.UnitName,
                                 Pricing = c.Pricing,
                                 LastBuyDate = c.LastBuyDate,
                                 Index = a.Index,
                                 InputPriceDate = c.InputPriceDate,
                                 PriceHistory = c.PriceHistory,
                                 DeliveryDays = c.DeliveryDays,
                                 Path = a.Path,
                                 FileName = a.FileName,
                                 IsSetup = c.IsSetup
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.MaterialName))
            {
                dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
            {
                dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();

            if (listResult.Count() > 0)
            {
                int maxLen = listResult.Select(s => s.Index.Length).Max();

                Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                listResult = listResult
                           .Select(s =>
                               new
                               {
                                   OrgStr = s,
                                   SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                               })
                           .OrderBy(x => x.SortStr)
                           .Select(x => x.OrgStr).ToList();
            }

            UpdateMaterialPrice(listResult);

            SimilarMaterialConfig similarMaterialConfig;
            foreach (var item in listResult)
            {
                similarMaterialConfig = db.SimilarMaterialConfigs.AsNoTracking().FirstOrDefault(b => b.MaterialId.Equals(item.MaterialId));
                if (similarMaterialConfig != null)
                {
                    item.Check = similarMaterialConfig.MaterialId;
                }
            }

            searchResult.MaxDeliveryDay = listResult.Select(r => r.DeliveryDays).DefaultIfEmpty(0).Max();

            searchResult.ListResult = listResult;
            searchResult.TotalAmount = listResult.Where(r => r.Index.IndexOf('.') == -1).Sum(s => s.ParentPricing * s.Quantity);

            return searchResult;
        }

        public void UpdateMaterialPrice(List<ModuleMaterialResultModel> materials)
        {
            var parents = materials.Where(r => r.Index.IndexOf('.') == -1).ToList();

            ModulePriceInfoModel modulePriceInfoModel;
            int day = 0;
            MaterialBusiness materialBusiness = new MaterialBusiness();
            day = materialBusiness.GetConfigMaterialLastByDate();
            TimeSpan timeSpan;
            foreach (var item in parents)
            {
                item.Parent = true;
                if (item.LastBuyDate.HasValue)
                {
                    timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        item.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Pricing = 0;
                    }
                }

                //item.Quantity = item.Quantity * GetParentQuantity(materials, item.Index);
                modulePriceInfoModel = null;
                if ((item.MaterialCode.ToUpper().StartsWith(Constants.Manufacture_TPA) || item.MaterialCode.ToUpper().StartsWith("PCB")) && item.UnitName.ToUpper().Equals("BỘ") && !Constants.Material_Specification_HAN.Equals(item.Specification))
                {
                    modulePriceInfoModel = GetPriceAndPriceStatusModuleByModuleCode(item.MaterialCode);
                    item.Pricing = modulePriceInfoModel.Price;
                    item.IsNoPrice = modulePriceInfoModel.IsNoPrice;
                    item.IsFinal = true;
                }

                item.ParentPricing = item.Pricing;

                if (modulePriceInfoModel == null || !modulePriceInfoModel.IsModuleExist)
                {
                    if (!string.IsNullOrEmpty(item.Specification) && Constants.Material_Specification_HAN.Equals(item.Specification))
                    {
                        item.IsNoPrice = item.Pricing == 0;
                    }
                    else
                    {
                        UpdateMaterialPriceChild(item, materials, day);
                    }
                }
            }
        }
        public void UpdateMaterialPriceChild(ModuleMaterialResultModel parent, List<ModuleMaterialResultModel> materials, int day)
        {
            string parentIndex = parent.Index + ".";
            var childs = materials.Where(r => r.Index.StartsWith(parentIndex) && r.Index.Substring(parentIndex.Length, r.Index.Length - parentIndex.Length).IndexOf('.') == -1).ToList();

            if (childs.Count == 0)
            {
                parent.IsFinal = true;
                parent.IsNoPrice = parent.Pricing > 0 ? false : true;
                return;
            }

            bool isNoPrice = false;
            ModulePriceInfoModel modulePriceInfoModel;

            TimeSpan timeSpan;
            foreach (var item in childs)
            {
                item.ReadQuantity = item.Quantity * GetParentQuantity(materials, item.Index);
                if (item.LastBuyDate.HasValue)
                {
                    timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);
                    if (timeSpan.Days <= day)
                    {
                        item.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Pricing = 0;
                    }
                }

                //item.Quantity = item.Quantity * GetParentQuantity(materials, item.Index);
                modulePriceInfoModel = null;
                if ((item.MaterialCode.ToUpper().StartsWith(Constants.Manufacture_TPA) || item.MaterialCode.ToUpper().StartsWith("PCB")) && item.UnitName.ToUpper().Equals("BỘ") && !Constants.Material_Specification_HAN.Equals(item.Specification))
                {
                    modulePriceInfoModel = GetPriceAndPriceStatusModuleByModuleCode(item.MaterialCode);
                    item.Pricing = modulePriceInfoModel.Price;
                    item.IsNoPrice = modulePriceInfoModel.IsNoPrice;
                    item.IsFinal = true;
                }

                item.ParentPricing = item.Pricing;

                if (modulePriceInfoModel == null || !modulePriceInfoModel.IsModuleExist)
                {
                    if (!string.IsNullOrEmpty(item.Specification) && Constants.Material_Specification_HAN.Equals(item.Specification))
                    {
                        item.IsNoPrice = item.Pricing == 0;
                        item.IsFinal = true;
                    }
                    else
                    {
                        UpdateMaterialPriceChild(item, materials, day);
                    }
                }

                if (item.IsNoPrice)
                {
                    isNoPrice = true;
                }
            }

            parent.ParentPricing = childs.Sum(s => s.ParentPricing * s.Quantity);
            parent.Pricing = childs.Sum(s => s.ParentPricing * s.Quantity);
            parent.IsNoPrice = isNoPrice;
        }


        /// <summary>
        /// Lấy giá module con trong DMVT
        /// </summary>
        /// <returns></returns>
        private decimal GetPriceModuleChild(string moduleCode, decimal currentPrice)
        {
            decimal price = currentPrice;
            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(moduleCode.ToLower()));

            if (module != null)
            {
                var materials = (from a in db.ModuleMaterials.AsNoTracking()
                                 where a.ModuleId.Equals(module.Id)
                                 join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                 select new ModuleMaterialResultModel()
                                 {
                                     Index = a.Index,
                                     Pricing = b.Pricing,
                                     PriceHistory = b.PriceHistory,
                                     MaterialCode = b.Code,
                                     Quantity = a.Quantity,
                                     InputPriceDate = b.InputPriceDate,
                                     LastBuyDate = b.LastBuyDate,
                                     Specification = a.Specification,
                                     UnitName = a.UnitName,
                                 }).ToList();

                UpdateMaterialPrice(materials);

                price = materials.Where(r => r.Index.IndexOf('.') == -1).Sum(r => r.ParentPricing * r.Quantity);
            }

            return price;
        }

        /// <summary>
        /// Lấy giá module theo Id
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public decimal GetPriceModuleByModuleId(string moduleId, decimal currentPrice)
        {
            decimal price = currentPrice;

            var materials = (from a in db.ModuleMaterials.AsNoTracking()
                             where a.ModuleId.Equals(moduleId)
                             join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                             select new ModuleMaterialResultModel()
                             {
                                 Index = a.Index,
                                 Pricing = b.Pricing,
                                 PriceHistory = b.PriceHistory,
                                 MaterialCode = b.Code,
                                 Quantity = a.Quantity,
                                 InputPriceDate = b.InputPriceDate,
                                 LastBuyDate = b.LastBuyDate,
                                 Specification = a.Specification,
                                 UnitName = a.UnitName,
                             }).ToList();

            UpdateMaterialPrice(materials);

            price = materials.Where(r => r.Index.IndexOf('.') == -1).Sum(r => r.ParentPricing * r.Quantity);

            return price;
        }

        public ModulePriceInfoModel GetPriceAndPriceStatusModuleByModuleCode(string moduleCode)
        {
            ModulePriceInfoModel modulePriceInfoModel = new ModulePriceInfoModel()
            {
                Price = 0,
                IsNoPrice = false
            };

            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(moduleCode.ToLower()));

            if (module != null)
            {
                modulePriceInfoModel = GetPriceAndPriceStatusModuleByModuleId(module.Id);
                modulePriceInfoModel.IsModuleExist = true;
                modulePriceInfoModel.ModuleId = module.Id;
            }

            return modulePriceInfoModel;
        }

        public ModulePriceInfoModel GetPriceAndPriceStatusModuleByModuleId(string moduleId)
        {
            ModulePriceInfoModel modulePriceInfoModel = new ModulePriceInfoModel();
            var module = db.Modules.FirstOrDefault(t => t.Id.Equals(moduleId));

            var materials = (from a in db.ModuleMaterials.AsNoTracking()
                             where a.ModuleId.Equals(moduleId)
                             join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                             select new ModuleMaterialResultModel()
                             {
                                 Index = a.Index,
                                 Pricing = b.Pricing,
                                 PriceHistory = b.PriceHistory,
                                 MaterialCode = b.Code,
                                 Quantity = a.Quantity,
                                 InputPriceDate = b.InputPriceDate,
                                 LastBuyDate = b.LastBuyDate,
                                 Specification = a.Specification,
                                 UnitName = a.UnitName,
                             }).ToList();

            var materialErrors = materials.Where(t => t.MaterialCode.ToUpper().Equals(module.Code.ToUpper())).ToList();

            if (materialErrors.Count > 0)
            {
                foreach (var item in materialErrors)
                {
                    materials.Remove(item);
                }
            }

            UpdateMaterialPrice(materials);

            modulePriceInfoModel.Price = materials.Where(r => r.Index.IndexOf('.') == -1).Sum(r => r.ParentPricing * r.Quantity);
            modulePriceInfoModel.IsNoPrice = materials.Count(c => c.IsNoPrice) > 0 || modulePriceInfoModel.Price == 0;

            return modulePriceInfoModel;
        }

        public List<ModuleMaterialResultModel> GetMaterialNoPriceByModuleId(string moduleId)
        {
            List<ModuleMaterialResultModel> materialNoPrices = new List<ModuleMaterialResultModel>();
            var materials = (from a in db.ModuleMaterials.AsNoTracking()
                             where a.ModuleId.Equals(moduleId)
                             join m in db.Modules.AsNoTracking() on a.ModuleId equals m.Id
                             join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                             join f in db.Manufactures.AsNoTracking() on b.ManufactureId equals f.Id
                             select new ModuleMaterialResultModel()
                             {
                                 Index = a.Index,
                                 Pricing = b.Pricing,
                                 PriceHistory = b.PriceHistory,
                                 MaterialCode = b.Code,
                                 Quantity = a.Quantity,
                                 InputPriceDate = b.InputPriceDate,
                                 LastBuyDate = b.LastBuyDate,
                                 Specification = a.Specification,
                                 UnitName = a.UnitName,
                                 ModuleCode = m.Code,
                                 MaterialName = b.Name,
                                 ManufacturerId = f.Id,
                                 ManufacturerCode = f.Code
                             }).ToList();

            var parents = materials.Where(r => r.Index.IndexOf('.') == -1).ToList();

            int day = 0;
            MaterialBusiness materialBusiness = new MaterialBusiness();
            day = materialBusiness.GetConfigMaterialLastByDate();
            TimeSpan timeSpan;
            bool moduleExist = false;
            foreach (var item in parents)
            {
                item.Parent = true;
                if (item.LastBuyDate.HasValue)
                {
                    timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        item.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Pricing = 0;
                    }
                }

                //item.Quantity = item.Quantity * GetParentQuantity(materials, item.Index);
                moduleExist = false;
                if ((item.MaterialCode.ToUpper().StartsWith(Constants.Manufacture_TPA) || item.MaterialCode.ToUpper().StartsWith("PCB")) && item.UnitName.ToUpper().Equals("BỘ") && !Constants.Material_Specification_HAN.Equals(item.Specification))
                {
                    var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(item.MaterialCode.ToLower()));
                    moduleExist = false;
                    if (module != null)
                    {
                        moduleExist = true;
                        var materialsNoPricesSub = GetMaterialNoPriceByModuleId(module.Id);
                        if (materialsNoPricesSub.Count > 0)
                        {
                            item.Pricing = 0;
                            item.IsNoPrice = true;
                        }

                        materialNoPrices.AddRange(materialsNoPricesSub);
                    }
                    else
                    {
                        item.Pricing = 0;
                        item.IsNoPrice = true;
                        item.IsFinal = true;
                    }
                }

                item.ParentPricing = item.Pricing;

                if (!moduleExist)
                {
                    if (!string.IsNullOrEmpty(item.Specification) && Constants.Material_Specification_HAN.Equals(item.Specification))
                    {
                        item.IsNoPrice = item.Pricing == 0;
                        item.IsFinal = true;
                    }
                    else
                    {
                        UpdateMaterialNoPriceChild(item, materials, materialNoPrices, day);
                    }
                }
            }

            materialNoPrices.AddRange(materials.Where(r => r.IsFinal && r.IsNoPrice).ToList());

            return materialNoPrices;
        }
        public void UpdateMaterialNoPriceChild(ModuleMaterialResultModel parent, List<ModuleMaterialResultModel> materials, List<ModuleMaterialResultModel> materialNoPrices, int day)
        {
            string parentIndex = parent.Index + ".";
            var childs = materials.Where(r => r.Index.StartsWith(parentIndex) && r.Index.Substring(parentIndex.Length, r.Index.Length - parentIndex.Length).IndexOf('.') == -1).ToList();

            if (childs.Count == 0)
            {
                parent.IsFinal = true;
                parent.IsNoPrice = parent.Pricing > 0 ? false : true;
                return;
            }

            bool isNoPrice = false;

            TimeSpan timeSpan;
            bool moduleExist = false;
            foreach (var item in childs)
            {
                item.ReadQuantity = item.Quantity * GetParentQuantity(materials, item.Index);
                if (item.LastBuyDate.HasValue)
                {
                    timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);
                    if (timeSpan.Days <= day)
                    {
                        item.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Pricing = 0;
                    }
                }

                //item.Quantity = item.Quantity * GetParentQuantity(materials, item.Index);
                if ((item.MaterialCode.ToUpper().StartsWith(Constants.Manufacture_TPA) || item.MaterialCode.ToUpper().StartsWith("PCB")) && item.UnitName.ToUpper().Equals("BỘ") && !Constants.Material_Specification_HAN.Equals(item.Specification))
                {
                    var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Code.ToLower().Equals(item.MaterialCode.ToLower()));
                    moduleExist = false;
                    if (module != null)
                    {
                        moduleExist = true;
                        var materialsNoPricesSub = GetMaterialNoPriceByModuleId(module.Id);
                        if (materialsNoPricesSub.Count == 0)
                        {
                            item.Pricing = 0;
                            item.IsNoPrice = true;
                        }

                        materialNoPrices.AddRange(materialsNoPricesSub);
                    }
                    else
                    {
                        item.Pricing = 0;
                        item.IsNoPrice = true;
                    }

                    item.IsFinal = true;
                }

                item.ParentPricing = item.Pricing;

                if (!moduleExist)
                {
                    if (!string.IsNullOrEmpty(item.Specification) && Constants.Material_Specification_HAN.Equals(item.Specification))
                    {
                        item.IsNoPrice = item.Pricing == 0;
                        item.IsFinal = true;
                    }
                    else
                    {
                        UpdateMaterialPriceChild(item, materials, day);
                    }
                }

                if (item.IsNoPrice)
                {
                    isNoPrice = true;
                }
            }

            parent.ParentPricing = childs.Sum(s => s.ParentPricing * s.Quantity);
            parent.Pricing = childs.Sum(s => s.ParentPricing * s.Quantity);
            parent.IsNoPrice = isNoPrice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="indexChild"></param>
        /// <returns></returns>
        public decimal GetParentQuantity(List<ModuleMaterialResultModel> materials, string indexChild)
        {
            string indexParent = string.Empty;
            if (indexChild.LastIndexOf(".") != -1)
            {
                indexParent = indexChild.Substring(0, indexChild.LastIndexOf("."));
            }

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

        public SearchResultModel<SimilarMaterialConfigModel> SearchSimilarMaterialConfig(SimilarMaterialConfigSearchModel modelSearch)
        {
            SearchResultModel<SimilarMaterialConfigModel> searchResult = new SearchResultModel<SimilarMaterialConfigModel>();
            var dataQuery = (from a in db.SimilarMaterialConfigs.AsNoTracking()
                             join b in db.SimilarMaterials.AsNoTracking() on a.SimilarMaterialId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.MaterialGroups.AsNoTracking() on c.MaterialGroupId equals d.Id
                             join e in db.Manufactures.AsNoTracking() on c.ManufactureId equals e.Id
                             join f in db.Units.AsNoTracking() on c.UnitId equals f.Id
                             where a.SimilarMaterialId.Equals(modelSearch.SimilarMaterialId)
                             orderby c.Code
                             select new SimilarMaterialConfigModel
                             {
                                 Id = a.Id,
                                 MaterialId = a.MaterialId,
                                 SimilarMaterialId = a.SimilarMaterialId,
                                 MaterialGroupName = d.Name,
                                 Name = c.Name,
                                 Code = c.Code,
                                 ManufactureName = e.Name,
                                 UnitName = f.Name,
                                 Pricing = c.Pricing,
                                 Note = c.Note,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<SimilarMaterialModel> SearchSimilarMaterial(SimilarMaterialSearchModel modelSearch)
        {
            SearchResultModel<SimilarMaterialModel> searchResult = new SearchResultModel<SimilarMaterialModel>();

            var dataQuery = (from a in db.SimilarMaterials.AsNoTracking()
                             join b in db.SimilarMaterialConfigs.AsNoTracking() on a.Id equals b.SimilarMaterialId
                             orderby a.Name
                             select new SimilarMaterialModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 MaterialId = b.MaterialId,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.MaterialId))
            {
                dataQuery = dataQuery.Where(u => u.MaterialId.Equals(modelSearch.MaterialId));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="folderExport">Thư mục export file</param>
        /// <returns></returns>
        public string ExportExcel(ModuleMaterialSearchModel model, string folderExport = null)
        {
            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ModuleId));

            if (module == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Module);
            }

            var result = SearchModuleMaterial(model);

            if (result.ListResult.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0015, TextResourceKey.Material);
            }

            result.ListResult = ModuleMaterialSort(result.ListResult);

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ModuleMaterial.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = result.ListResult.Count;

            sheet[1, 1].Value = "DMVT Module " + module.Code;

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = result.ListResult.Select((a, i) => new
            {
                a.Index,
                a.MaterialName,
                a.Specification,
                a.MaterialCode,
                a.RawMaterialCode,
                a.UnitName,
                a.Quantity,
                a.RawMaterial,
                a.Weight,
                a.ManufacturerCode,
                a.Note,
                a.ReadQuantity,
                a.Pricing,
                Amount = a.Pricing * a.ReadQuantity
            });
            IRange iRangeData2 = sheet.FindFirst("<TotalAmount>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            iRangeData2.Text = iRangeData2.Text.Replace("<TotalAmount>", result.TotalAmount.ToString("#,###", cul.NumberFormat));
            if (string.IsNullOrEmpty(iRangeData2.Text))
            {
                iRangeData2.Text = "0";
            }

            if (listExport.Count() > 0)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count(), ExcelInsertOptions.FormatAsBefore);
            }

            for (int i = 0; i < result.ListResult.Count; i++)
            {
                if (result.ListResult[i].IsNoPrice)
                {
                    sheet.Range[iRangeData.Row + i, 1, iRangeData.Row + i, 14].CellStyle.Color = Color.Yellow;
                }
            }

            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders.Color = ExcelKnownColors.Black;
            //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 10].CellStyle.WrapText = true;

            string resultPathClient = "Template/" + Constants.FolderExport + module.Code + ".xlsx";

            if (!string.IsNullOrEmpty(folderExport))
            {
                resultPathClient = "Template/" + Constants.FolderExport + folderExport + "/" + module.Code + ".xlsx";
            }

            string pathExport = HttpContext.Current.Server.MapPath("~/" + resultPathClient);
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            return resultPathClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="folderExport">Thư mục export file</param>
        /// <returns></returns>
        public string ExportExcelMaterialBOMDraft(ModuleMaterialSearchModel model, string folderExport = null)
        {
            var module = db.Modules.AsNoTracking().FirstOrDefault(r => r.Id.Equals(model.ModuleId));
            var result = SearchModuleMaterialByProductId(model);
            List<MaterialImportBOMDraftModel> materialImportBOMDraftModels = new List<MaterialImportBOMDraftModel>();
            var listModule = result.ListResult.Where(a => string.IsNullOrEmpty(a.ParentId)).ToList();
            if(module == null)
            {
                foreach (var m in listModule)
                {
                    var listMaterialOfModule = result.ListResult.Where(a => m.Id.Equals(a.ParentId)).ToList();
                    materialImportBOMDraftModels.Add(m);
                    materialImportBOMDraftModels.AddRange(listMaterialOfModule);
                }
            }
            else
            {
                foreach (var m in listModule)
                {
                    if (m.Id.Equals(module.Id))
                    {
                        var listMaterialOfModule = result.ListResult.Where(a => m.Id.Equals(a.ParentId)).ToList();
                        materialImportBOMDraftModels.Add(m);
                        materialImportBOMDraftModels.AddRange(listMaterialOfModule);
                    }
                }
            }
            
            decimal totalPrice = GetPriceTHTK(materialImportBOMDraftModels);

            if (listModule.Count == 0)
            {
                materialImportBOMDraftModels = result.ListResult;
            }
            if(materialImportBOMDraftModels.Count == 0)
            {
                materialImportBOMDraftModels = result.ListResult;
            }
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ModuleMaterial.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = materialImportBOMDraftModels.Count;

            sheet[1, 1].Value =  module == null ? "DMVT Sản phẩm":"DMVT Module " +  module.Code;

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
   
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = materialImportBOMDraftModels.Select((a, i) => new
            {
                a.Index,
                Name = string.IsNullOrEmpty(a.ModuleName) ?a.MaterialName : a.ModuleName,
                a.Specification,
                a.MaterialCode,
                a.RawMaterialCode,
                a.UnitName,
                a.Quantity,
                a.RawMaterial,
                a.Weight,
                a.ManufacturerCode,
                a.Note,
                a.ReadQuantity,
                a.Pricing,
                Amount = a.Pricing * a.ReadQuantity
            });

            IRange iRangeData2 = sheet.FindFirst("<TotalAmount>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            iRangeData2.Text = iRangeData2.Text.Replace("<TotalAmount>", totalPrice.ToString("#,###", cul.NumberFormat));
            if (string.IsNullOrEmpty(iRangeData2.Text))
            {
                iRangeData2.Text = "0";
            }

            if (listExport.Count() > 0)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count(), ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            for (int i = 0; i < materialImportBOMDraftModels.Count; i++)
            {
                if (materialImportBOMDraftModels[i].IsNoPrice)
                {
                    sheet.Range[iRangeData.Row + i, 1, iRangeData.Row + i, 14].CellStyle.Color = Color.Yellow;
                }else if(!string.IsNullOrEmpty(materialImportBOMDraftModels[i].ModuleName))
                {
                    sheet.Range[iRangeData.Row + i, 1, iRangeData.Row + i, 14].CellStyle.Color = Color.Green;
                }
            }

            if (listExport.Count() > 1)
            {
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 14].Borders.Color = ExcelKnownColors.Black;
            }
            //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 10].CellStyle.WrapText = true;

            string resultPathClient = "Template/" + Constants.FolderExport +( module == null ? model.ContractCode : module.Code )+ ".xlsx";

            if (!string.IsNullOrEmpty(folderExport))
            {
                resultPathClient = "Template/" + Constants.FolderExport + folderExport + "/" + (module == null ? model.ContractCode : module.Code) + ".xlsx";
            }

            string pathExport = HttpContext.Current.Server.MapPath("~/" + resultPathClient);
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            return resultPathClient;
        }

        public void UpdateFileModuleMaterial(ModuleMaterialUploadFileModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var moduleMaterialAttach = db.ModuleMaterialAttaches.Where(a => a.ModuleMaterialId.Equals(model.Id)).ToList();
                    if (moduleMaterialAttach.Count > 0)
                    {
                        db.ModuleMaterialAttaches.RemoveRange(moduleMaterialAttach);
                    }
                    if (model.ListFileSetup.Count > 0)
                    {
                        ModuleMaterialAttach newModuleMaterialAttach;

                        foreach (var item in model.ListFileSetup)
                        {
                            if (string.IsNullOrEmpty(item.FilePath) || string.IsNullOrEmpty(item.FileName))
                            {
                                continue;
                            }
                            newModuleMaterialAttach = new ModuleMaterialAttach();
                            newModuleMaterialAttach.Id = Guid.NewGuid().ToString();
                            newModuleMaterialAttach.ModuleMaterialId = model.Id;
                            newModuleMaterialAttach.FileName = item.FileName;
                            newModuleMaterialAttach.FileSize = item.Size;
                            newModuleMaterialAttach.Path = item.FilePath;
                            newModuleMaterialAttach.CreateBy = model.CreateBy;
                            newModuleMaterialAttach.CreateDate = DateTime.Now;
                            newModuleMaterialAttach.UpdateBy = model.CreateBy;
                            newModuleMaterialAttach.UpdateDate = DateTime.Now;

                            db.ModuleMaterialAttaches.Add(newModuleMaterialAttach);
                        }
                    }

                    var moduleMaterialDataSheet = (from a in db.ModuleMaterials.AsNoTracking()
                                                   join b in db.MaterialDataSheets.AsNoTracking() on a.MaterialId equals b.MaterialId
                                                   join c in db.DataSheets.AsNoTracking() on b.DataSheetId equals c.Id
                                                   where a.MaterialId.Equals(model.MaterialId)
                                                   select new
                                                   {
                                                       b.Id,
                                                       b.MaterialId,
                                                       b.DataSheetId
                                                   }).ToList();

                    if (moduleMaterialDataSheet.Count > 0)
                    {
                        List<DataSheet> listDataSheet = new List<DataSheet>();
                        List<MaterialDataSheet> listMaterialDatasheet = new List<MaterialDataSheet>();
                        foreach (var item in moduleMaterialDataSheet)
                        {
                            var dataSheets = db.DataSheets.Where(i => i.Id.Equals(item.DataSheetId)).ToList();
                            var materialDataSheets = db.MaterialDataSheets.Where(i => i.Id.Equals(item.Id)).ToList();
                            listDataSheet.AddRange(dataSheets);
                            listMaterialDatasheet.AddRange(materialDataSheets);
                        }
                        db.MaterialDataSheets.RemoveRange(listMaterialDatasheet);
                        db.DataSheets.RemoveRange(listDataSheet);
                    }

                    if (model.ListFileDataSheet.Count > 0)
                    {
                        foreach (var item in model.ListFileDataSheet)
                        {
                            if (string.IsNullOrEmpty(item.FilePath) || string.IsNullOrEmpty(item.FileName))
                            {
                                continue;
                            }
                            DataSheet dataSheet = new DataSheet()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ManufactureId = model.ManufactureId,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                Size = Convert.ToInt32(item.Size),
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.CreateBy,
                                UpdateDate = DateTime.Now,
                            };
                            MaterialDataSheet materialDataSheet = new MaterialDataSheet()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = model.MaterialId,
                                DataSheetId = dataSheet.Id,
                            };
                            db.DataSheets.Add(dataSheet);
                            db.MaterialDataSheets.Add(materialDataSheet);
                        }
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Module, model.ModuleId, string.Empty, "Vật tư cài đặt");
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    NtsLog.LogError(ex);

                    throw new NTSLogException(model, ex);
                }
            }
        }

        public SearchResultModel<ModuleMaterialModel> SearchModuleMaterialsSetup(ModuleMaterialSearchModel model)
        {
            SearchResultModel<ModuleMaterialModel> searchResult = new SearchResultModel<ModuleMaterialModel>();
            var dataQuery = (from a in db.ModuleMaterials.AsNoTracking()
                             join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join m in db.Manufactures.AsNoTracking() on a.ManufacturerId equals m.Id
                             where a.ModuleId.Equals(model.ModuleId)
                             select new ModuleMaterialModel
                             {
                                 Id = a.Id,
                                 ModuleId = a.ModuleId,
                                 MaterialId = a.MaterialId,
                                 MaterialCode = a.MaterialCode,
                                 MaterialName = a.MaterialName,
                                 Specification = a.Specification,
                                 RawMaterialCode = a.RawMaterialCode,
                                 RawMaterial = a.RawMaterial,
                                 Price = a.Price,
                                 Quantity = a.Quantity,
                                 Amount = a.Amount,
                                 Weight = a.Weight,
                                 ManufacturerId = a.ManufacturerId,
                                 ManufacturerCode = a.ManufacturerCode,
                                 Note = a.Note,
                                 UnitName = a.UnitName,
                                 IsSetup = c.IsSetup,
                                 Path = a.Path,
                                 FileName = a.FileName
                             }).AsQueryable();
            dataQuery = dataQuery.Where(u => u.IsSetup == true);
            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            foreach (var item in listResult)
            {
                var datashet = (from a in db.MaterialDataSheets.AsNoTracking()
                                join c in db.DataSheets.AsNoTracking() on a.DataSheetId equals c.Id
                                where a.MaterialId.Equals(item.MaterialId)
                                select new DatasheetModel
                                {
                                    Id = c.Id,
                                    FilePath = c.FilePath,
                                    FileName = c.FileName,
                                    Size = c.Size,
                                    ManufactureId = c.ManufactureId,
                                    UpdateDate = c.UpdateDate,
                                }).ToList();
                item.ListDatashet = datashet;

                var fileSetup = (from a in db.ModuleMaterialAttaches.AsNoTracking()
                                 join u in db.Users.AsNoTracking() on a.UpdateBy equals u.Id into au
                                 from aun in au.DefaultIfEmpty()
                                 where a.ModuleMaterialId.Equals(item.Id)
                                 select new FileSetUpModel
                                 {
                                     Id = a.Id,
                                     ModuleMaterialId = a.ModuleMaterialId,
                                     FileName = a.FileName,
                                     Size = a.FileSize,
                                     FilePath = a.Path,
                                     UpdateBy = aun != null ? aun.UserName : string.Empty,
                                     UpdateDate = a.UpdateDate,
                                 }).ToList();
                item.ListFileSetup = fileSetup;
            }

            return searchResult;
        }

        private List<ModuleMaterialResultModel> ModuleMaterialSort(List<ModuleMaterialResultModel> moduleMaterials)
        {
            int maxLen = moduleMaterials.AsEnumerable().Select(s => s.Index.Length).Max();

            Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

            return moduleMaterials
                       .Select(s =>
                           new
                           {
                               OrgStr = s,
                               SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                           })
                       .OrderBy(x => x.SortStr)
                       .Select(x => x.OrgStr).ToList();
        }

        public SearchModuleMaterialResultModel<MaterialImportBOMDraftModel> SearchModuleMaterialByProductId(ModuleMaterialSearchModel modelSearch)
        {
            SearchModuleMaterialResultModel<MaterialImportBOMDraftModel> searchResult = new SearchModuleMaterialResultModel<MaterialImportBOMDraftModel>();
            List<ProjectProductsModel> listProjectProduct = new List<ProjectProductsModel>();

            listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.ParentId.Equals(modelSearch.ProjectProductId)
                                      orderby a.ContractIndex
                                      select new ProjectProductsModel
                                      {
                                          Id = a.Id,
                                          ProjectId = a.ProjectId,
                                          ParentId = a.ParentId,
                                          ModuleId = a.ModuleId,
                                          ProductId = a.ProductId,
                                          ContractCode = a.ContractCode,
                                          ContractName = a.ContractName,
                                          Specifications = a.Specifications,
                                          DataType = a.DataType,
                                          ModuleStatus = a.ModuleStatus,
                                          DesignStatus = a.DesignStatus,
                                          DesignFinishDate = a.DesignFinishDate,
                                          MakeFinishDate = a.MakeFinishDate,
                                          DeliveryDate = a.DeliveryDate,
                                          TransferDate = a.TransferDate,
                                          Note = a.Note,
                                          Quantity = a.Quantity,
                                          RealQuantity = a.RealQuantity,
                                          Price = a.Price,
                                          Amount = a.Quantity * a.Price,
                                          ContractIndex = a.ContractIndex,
                                          IsGeneralDesign = a.IsGeneralDesign,
                                          DesignWorkStatus = a.DesignWorkStatus,
                                          DesignCloseDate = a.DesignCloseDate,
                                          GeneralDesignLastDate = a.GeneralDesignLastDate,
                                          MaterialExist = a.MaterialExist,
                                          IsMaterial = true,
                                          IsIncurred = a.IsIncurred,
                                          ModuleName = db.Modules.FirstOrDefault(v => v.Id.Equals(a.ModuleId)).Code,
                                      }).ToList();
            if(listProjectProduct.Count == 0)
            {
                listProjectProduct = (from a in db.ProjectProducts.AsNoTracking()
                                      where a.Id.Equals(modelSearch.ProjectProductId)
                                      orderby a.ContractIndex
                                      select new ProjectProductsModel
                                      {
                                          Id = a.Id,
                                          ProjectId = a.ProjectId,
                                          ParentId = a.ParentId,
                                          ModuleId = a.ModuleId,
                                          ProductId = a.ProductId,
                                          ContractCode = a.ContractCode,
                                          ContractName = a.ContractName,
                                          Specifications = a.Specifications,
                                          DataType = a.DataType,
                                          ModuleStatus = a.ModuleStatus,
                                          DesignStatus = a.DesignStatus,
                                          DesignFinishDate = a.DesignFinishDate,
                                          MakeFinishDate = a.MakeFinishDate,
                                          DeliveryDate = a.DeliveryDate,
                                          TransferDate = a.TransferDate,
                                          Note = a.Note,
                                          Quantity = a.Quantity,
                                          RealQuantity = a.RealQuantity,
                                          Price = a.Price,
                                          Amount = a.Quantity * a.Price,
                                          ContractIndex = a.ContractIndex,
                                          IsGeneralDesign = a.IsGeneralDesign,
                                          DesignWorkStatus = a.DesignWorkStatus,
                                          DesignCloseDate = a.DesignCloseDate,
                                          GeneralDesignLastDate = a.GeneralDesignLastDate,
                                          MaterialExist = a.MaterialExist,
                                          IsMaterial = true,
                                          IsIncurred = a.IsIncurred,
                                          ModuleName = db.Modules.FirstOrDefault(v => v.Id.Equals(a.ModuleId)).Code,
                                      }).ToList();
            }
            if (listProjectProduct.Count > 0)
            {
                List<MaterialImportBOMDraftModel> listResult = new List<MaterialImportBOMDraftModel>();
                List<MaterialImportBOMDraftModel> listResultDone = new List<MaterialImportBOMDraftModel>();
                foreach (var item1 in listProjectProduct)
                {
                    var dataQuery = (from a in db.MaterialImportBOMDrafts.AsNoTracking()
                                     where a.ProjectId.Equals(item1.Id) && a.ModuleId.Equals(item1.ModuleId) && (a.UpdateStatus == 1 || a.UpdateStatus == 0)
                                     orderby a.Code
                                     select new MaterialImportBOMDraftModel
                                     {
                                         Id = a.Id,
                                         ModuleId = a.ModuleId,
                                         MaterialId = a.Id,
                                         MaterialCode = a.Code,
                                         MaterialName = a.Name,
                                         Specification = a.Specification,
                                         RawMaterialCode = a.RawMaterialCode,
                                         RawMaterial = a.RawMaterial,
                                         Pricing = a.Pricing,
                                         Quantity = a.Quantity,
                                         ReadQuantity = a.Quantity,
                                         Weight = a.Weight,
                                         //ManufacturerId = a.ManufacturerId,
                                         ManufacturerCode = a.ManufactureCode,
                                         Note = a.Note,
                                         UnitName = a.UnitName,
                                         Index = a.Index,
                                         Amount = a.Pricing*a.Quantity,
                                         ParentId = a.ModuleId
                                     }).AsQueryable();
                    if (modelSearch.IsParent)
                    {
                        var dataQueryParent = dataQuery.Where(a => a.MaterialCode.Equals(modelSearch.ContractCode) && !a.Index.Contains("."));
                        listResult.AddRange(dataQueryParent.ToList());
                        foreach (var item in dataQueryParent.ToList())
                        {
                            var listChild = dataQuery.Where(a => a.Index.StartsWith(item.Index + ".")).ToList();
                            listResult.AddRange(listChild);
                        }

                        if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                        {
                            listResult = listResult.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper())).ToList();
                        }
                        if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                        {
                            listResult = listResult.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper())).ToList();
                        }

                        if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                        {
                            listResult = listResult.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper())).ToList();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                        {
                            dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper()));
                        }
                        if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                        {
                            dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper()));
                        }

                        if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                        {
                            dataQuery = dataQuery.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper()));
                        }
                        searchResult.TotalItem = dataQuery.Count();
                        listResult = dataQuery.ToList();

                        var productChild = db.ProjectProducts.AsNoTracking().Where(a => a.ParentId.Equals(modelSearch.ProductProjectId)).ToList();
                        foreach (var item in listResult)
                        {
                            if (!item.Index.Contains("."))
                            {
                                var materialExist = productChild.FirstOrDefault(a => a.ContractCode.Equals(item.MaterialCode));
                                if (materialExist == null)
                                {
                                    item.IsNoProductChild = true;
                                }
                            }
                        }
                    }

                    if (listResult.Count() > 0)
                    {
                        int maxLen = listResult.Select(s => s.Index.Length).Max();

                        Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                        var listResult1 = listResult
                                   .Select(s =>
                                       new
                                       {
                                           OrgStr = s,
                                           SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                                       })
                                   .OrderBy(x => x.SortStr)
                                   .Select(x => x.OrgStr).ToList();
                        foreach (var item2 in listResult1)
                        {
                            listResultDone.Add(item2);
                        }
                        MaterialImportBOMDraftModel mm = new MaterialImportBOMDraftModel();
                        mm.ModuleName = item1.ModuleName;
                        mm.Id = item1.ModuleId;
                        listResultDone.Add(mm);
                    }
                }
                var count = 0;
                var stt = 1;
                var projectGeneralDesign = db.ProjectGeneralDesigns.Where(a => a.ProjectProductId.Equals(modelSearch.ProjectProductId)).ToList();
                foreach (var projectd in projectGeneralDesign)
                {
                    var listMeterialDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                              where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Material
                                              join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                              join c in db.Manufactures.AsNoTracking() on b.ManufactureId equals c.Id
                                              join d in db.Units.AsNoTracking() on b.UnitId equals d.Id
                                              where a.ProjectGeneralDesignId.Equals(projectd.Id)
                                              orderby b.Code
                                              select new MaterialImportBOMDraftModel
                                              {
                                                  Id = a.MaterialId,
                                                  ModuleId = "vtp",
                                                  MaterialName = b.Name,
                                                  MaterialCode = b.Code,
                                                  ManufacturerCode = c.Code,
                                                  Quantity = (int)a.Quantity,
                                                  Pricing = projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                  UnitName = d.Name,
                                                  Amount = (int)a.Quantity * projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                  ParentId = "vtp"
                                              }).ToList();

                    var listModuleDesign = (from a in db.ProjectGeneralDesignMaterials.AsNoTracking()
                                            where a.Type == Constants.ProjectGaneralDesignMaterial_Type_Module
                                            join b in db.Modules.AsNoTracking() on a.MaterialId equals b.Id
                                            where a.ProjectGeneralDesignId.Equals(projectd.Id)
                                            orderby b.Code
                                            select new MaterialImportBOMDraftModel
                                            {
                                                Id = a.MaterialId,
                                                ModuleId = "vtp",
                                                MaterialName = b.Name,
                                                MaterialCode = b.Code,
                                                Quantity = (int)a.Quantity,
                                                ManufacturerCode = Constants.Manufacture_TPA,
                                                UnitName = Constants.Unit_Bo,
                                                Pricing = projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_Approved ? a.Price : b.Pricing,
                                                ParentId = "vtp"
                                            }).ToList();

                    if (listModuleDesign.Count > 0 && projectd.ApproveStatus == Constants.ProjectGeneralDesigns_ApproveStatus_NotApproved)
                    {
                        ModulePriceInfoModel modulePriceInfoModel;
                        foreach (var item in listModuleDesign)
                        {
                            modulePriceInfoModel = GetPriceAndPriceStatusModuleByModuleId(item.Id);
                            if (modulePriceInfoModel != null)
                            {
                                item.Pricing = modulePriceInfoModel.Price;
                            }
                        }
                    }
                    foreach (var module in listModuleDesign)
                    {
                        module.Amount = module.Quantity * module.Pricing;
                    }

                    foreach (var ma in listMeterialDesign)
                    {
                        var material = listResultDone.FirstOrDefault(a => ma.MaterialCode.Equals(a.MaterialCode) && ma.ModuleId.Equals(a.ModuleId));
                        if (material == null)
                        {
                            ma.Index = stt.ToString();
                            listResultDone.Add(ma);
                            stt++;
                        }
                        count++;
                    }
                    foreach (var mo in listModuleDesign)
                    {
                        var module = listResultDone.FirstOrDefault(a => mo.MaterialCode.Equals(a.MaterialCode) && mo.ModuleId.Equals(a.ModuleId));
                        if (module == null)
                        {
                            mo.Index = stt.ToString();
                            listResultDone.Add(mo);
                            stt++;
                        }
                        count++;
                    }
                }
                if (count > 0)
                {
                    MaterialImportBOMDraftModel mm = new MaterialImportBOMDraftModel();
                    mm.ModuleName = "VATTUPHU." + modelSearch.ContractCode;
                    mm.Id = "vtp";
                    listResultDone.Add(mm);
                }
                searchResult.ListResult = listResultDone;
                searchResult.Status9 = 20;
            }
            else
            {
                var dataQuery = (from a in db.MaterialImportBOMDrafts.AsNoTracking()
                                 where a.ProjectId.Equals(modelSearch.ProjectProductId) && a.ModuleId.Equals(modelSearch.ModuleId) && (a.UpdateStatus == 1 || a.UpdateStatus == 0)
                                 orderby a.Code
                                 select new MaterialImportBOMDraftModel
                                 {
                                     Id = a.Id,
                                     ModuleId = a.ModuleId,
                                     MaterialId = a.Id,
                                     MaterialCode = a.Code,
                                     MaterialName = a.Name,
                                     Specification = a.Specification,
                                     RawMaterialCode = a.RawMaterialCode,
                                     RawMaterial = a.RawMaterial,
                                     Pricing = a.Pricing,
                                     Quantity = a.Quantity,
                                     ReadQuantity = a.Quantity,
                                     Weight = a.Weight,
                                     //ManufacturerId = a.ManufacturerId,
                                     ManufacturerCode = a.ManufactureCode,
                                     Note = a.Note,
                                     UnitName = a.UnitName,
                                     Index = a.Index,
                                     Amount = a.Amount
                                 }).AsQueryable();

                List<MaterialImportBOMDraftModel> listResult = new List<MaterialImportBOMDraftModel>(); ;

                if (modelSearch.IsParent)
                {
                    var dataQueryParent = dataQuery.Where(a => a.MaterialCode.Equals(modelSearch.ContractCode) && !a.Index.Contains("."));
                    listResult.AddRange(dataQueryParent.ToList());
                    foreach (var item in dataQueryParent.ToList())
                    {
                        var listChild = dataQuery.Where(a => a.Index.StartsWith(item.Index + ".")).ToList();
                        listResult.AddRange(listChild);
                    }

                    if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                    {
                        listResult = listResult.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper())).ToList();
                    }
                    if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                    {
                        listResult = listResult.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper())).ToList();
                    }

                    if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                    {
                        listResult = listResult.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper())).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(modelSearch.MaterialName))
                    {
                        dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(modelSearch.MaterialName.ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
                    {
                        dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(modelSearch.MaterialCode.ToUpper()));
                    }

                    if (!string.IsNullOrEmpty(modelSearch.ManufacturerCode))
                    {
                        dataQuery = dataQuery.Where(u => u.ManufacturerCode.ToUpper().Contains(modelSearch.ManufacturerCode.ToUpper()));
                    }
                    searchResult.TotalItem = dataQuery.Count();
                    listResult = dataQuery.ToList();

                    var productChild = db.ProjectProducts.AsNoTracking().Where(a => a.ParentId.Equals(modelSearch.ProductProjectId)).ToList();
                    foreach (var item in listResult)
                    {
                        if (!item.Index.Contains("."))
                        {
                            var materialExist = productChild.FirstOrDefault(a => a.ContractCode.Equals(item.MaterialCode));
                            if (materialExist == null)
                            {
                                item.IsNoProductChild = true;
                            }
                        }
                    }
                }

                if (listResult.Count() > 0)
                {
                    int maxLen = listResult.Select(s => s.Index.Length).Max();

                    Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                    listResult = listResult
                               .Select(s =>
                                   new
                                   {
                                       OrgStr = s,
                                       SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                                   })
                               .OrderBy(x => x.SortStr)
                               .Select(x => x.OrgStr).ToList();
                }

                searchResult.MaxDeliveryDay = listResult.Select(r => r.DeliveryDays).DefaultIfEmpty(0).Max();

                searchResult.ListResult = listResult;
                searchResult.TotalAmount = listResult.Where(r => r.Index.IndexOf('.') == -1).Sum(s => s.ParentPricing * s.Quantity);

            }
            return searchResult;
        }
        public decimal GetPriceTHTK(List<MaterialImportBOMDraftModel> listmaterial)
        {
            decimal priceTHTK = 0;
            var materials = db.Materials.AsNoTracking().ToList();
            var modules = listmaterial.Where(a => string.IsNullOrEmpty(a.Index)).ToList();
            if (modules.Count != 0)
            {
                foreach (var item in modules)
                {
                    var listMaterialOfModule = listmaterial.Where(a => item.Id.Equals(a.ParentId)).ToList();
                    var price = GetPriceModule(listMaterialOfModule, materials);
                    priceTHTK = priceTHTK + price;
                }
            }
            else
            {
                var price = GetPriceModule(listmaterial, materials);
                priceTHTK = priceTHTK + price;
            }
            return priceTHTK;
        }
        public decimal GetPriceModule(List<MaterialImportBOMDraftModel> listmaterial, List<Material> materials)
        {
            var listResult = (from a in listmaterial
                              join c in materials on a.MaterialCode equals c.Code
                              orderby a.MaterialCode
                              select new ModuleMaterialResultModel
                              {
                                  Id = a.Id,
                                  ModuleId = a.ModuleId,
                                  MaterialId = a.MaterialId,
                                  MaterialCode = a.MaterialCode,
                                  MaterialName = a.MaterialName,
                                  Specification = a.Specification,
                                  RawMaterialCode = a.RawMaterialCode,
                                  RawMaterial = a.RawMaterial,
                                  Price = a.Price,
                                  Quantity = a.Quantity,
                                  ReadQuantity = a.Quantity,
                                  Amount = a.Amount,
                                  Weight = a.Weight,
                                  ManufacturerId = a.ManufacturerId,
                                  ManufacturerCode = a.ManufacturerCode,
                                  Note = a.Note,
                                  UnitName = a.UnitName,
                                  Pricing = c.Pricing,
                                  LastBuyDate = c.LastBuyDate,
                                  Index = a.Index,
                                  InputPriceDate = c.InputPriceDate,
                                  PriceHistory = c.PriceHistory,
                                  DeliveryDays = c.DeliveryDays,
                                  Path = a.Path,
                                  FileName = a.FileName,
                                  IsSetup = c.IsSetup
                              }).ToList();
            decimal priceTHTK = 0;
            if (listResult.Count == listmaterial.Count)
            {
                UpdateMaterialPrice(listResult);
                priceTHTK = listResult.Where(r => r.Index.IndexOf('.') == -1).Sum(s => s.ParentPricing * s.Quantity);
            }
            else
            {
                var data = (from a in listmaterial
                            orderby a.MaterialCode
                            select new ModuleMaterialResultModel
                            {
                                Id = a.Id,
                                ModuleId = a.ModuleId,
                                MaterialId = a.MaterialId,
                                MaterialCode = a.MaterialCode,
                                MaterialName = a.MaterialName,
                                Specification = a.Specification,
                                RawMaterialCode = a.RawMaterialCode,
                                RawMaterial = a.RawMaterial,
                                Price = a.Price,
                                Quantity = a.Quantity,
                                ReadQuantity = a.Quantity,
                                Amount = a.Amount,
                                Weight = a.Weight,
                                ManufacturerId = a.ManufacturerId,
                                ManufacturerCode = a.ManufacturerCode,
                                Note = a.Note,
                                UnitName = a.UnitName,
                                Pricing = a.Pricing,
                                Index = a.Index,
                                Path = a.Path,
                                FileName = a.FileName,
                            }).ToList();
                UpdateMaterialPrice(data);
                priceTHTK = data.Where(r => r.Index.IndexOf('.') == -1).Sum(s => s.ParentPricing * s.Quantity);
            }
            return priceTHTK;
        }
    }
}
