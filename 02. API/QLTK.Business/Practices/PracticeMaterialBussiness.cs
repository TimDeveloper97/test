using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterials;
using NTS.Model.Practice;
using NTS.Model.PracticeMaterial;
using NTS.Model.Repositories;
using QLTK.Business.ModuleMaterials;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.PracticeMaterials
{
    public class PracticeMaterialBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public PracticeMaterialSearchResultModel<ModuleInPracticeModel> SearchModuleInPractice(PracticeMaterialSearchModel modelSearch)
        {
            PracticeMaterialSearchResultModel<ModuleInPracticeModel> searchResult = new PracticeMaterialSearchResultModel<ModuleInPracticeModel>();
            var ListMaterial = (from a in db.ModuleMaterials.AsNoTracking()
                                join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                join d in db.MaterialGroups.AsNoTracking() on b.MaterialGroupId equals d.Id
                                join e in db.Manufactures.AsNoTracking() on b.ManufactureId equals e.Id
                                join f in db.Units.AsNoTracking() on b.UnitId equals f.Id
                                join p in db.ModuleInPractices.AsNoTracking() on a.ModuleId equals p.ModuleId
                                where p.PracticeId.Equals(modelSearch.PracticeId)
                                orderby b.Code
                                select new ModuleMaterialResultModel
                                {
                                    Id = a.Id,
                                    ModuleId = a.ModuleId,
                                    MaterialId = a.MaterialId,
                                    MaterialName = b.Name,
                                    MaterialCode = b.Code,
                                    ManufactureName = e.Name,
                                    MaterialGroupName = d.Name,
                                    MaterialGroupCode = d.Code,
                                    UnitName = f.Name,
                                    Quantity = a.Quantity,
                                    Pricing = b.Pricing,
                                    DeliveryDays = b.DeliveryDays,
                                    Index = a.Index,
                                    LastBuyDate = b.LastBuyDate,
                                    InputPriceDate = b.InputPriceDate,
                                    PriceHistory = b.PriceHistory,
                                    Specification = a.Specification
                                }).AsQueryable();

            var moduleInPractices = (from h in db.ModuleInPractices.AsNoTracking()
                                     join k in db.Modules.AsNoTracking() on h.ModuleId equals k.Id
                                     where h.PracticeId.Equals(modelSearch.PracticeId)
                                     orderby k.Code
                                     select new ModuleInPracticeModel
                                     {
                                         Id = h.Id,
                                         ModuleId = h.ModuleId,
                                         ModuleName = k.Name,
                                         ModuleCode = k.Code,
                                         Qty = h.Qty
                                         //ListMaterial = ListMaterial.Where(a => a.ModuleId.Equals(h.ModuleId)).ToList(),
                                     }).ToList();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(modelSearch.MaterialName))
            {
                ListMaterial = ListMaterial.Where(u => u.MaterialName.ToLower().Contains(modelSearch.MaterialName.ToLower()));
            }
            if (!string.IsNullOrEmpty(modelSearch.MaterialCode))
            {
                ListMaterial = ListMaterial.Where(u => u.MaterialCode.ToLower().Contains(modelSearch.MaterialCode.ToLower()) || u.MaterialName.ToLower().Contains(modelSearch.MaterialCode.ToLower()));
            }

            

            switch (modelSearch.Operators)
            {
                // <=
                case 5:
                    ListMaterial = ListMaterial.Where(u => u.DeliveryDays <= modelSearch.DeliveryDay);
                    break;
                // >= 
                case 3:
                    ListMaterial = ListMaterial.Where(u => u.DeliveryDays >= modelSearch.DeliveryDay);
                    break;
                // <
                case 4:
                    ListMaterial = ListMaterial.Where(u => u.DeliveryDays < modelSearch.DeliveryDay);
                    break;
                // >
                case 2:
                    ListMaterial = ListMaterial.Where(u => u.DeliveryDays > modelSearch.DeliveryDay);
                    break;
                // ==
                case 1:
                    ListMaterial = ListMaterial.Where(u => u.DeliveryDays == modelSearch.DeliveryDay);
                    break;
                default:
                    break;
            }

            var i = 1;

            decimal priceMax = 0;
            ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
            foreach (var item in moduleInPractices)
            {
                item.ListMaterial = ListMaterial.Where(a => a.ModuleId.Equals(item.ModuleId)).ToList();

                moduleMaterialBusiness.UpdateMaterialPrice(item.ListMaterial);

                if (item.ListMaterial.Count() > 0)
                {
                    int maxLen = item.ListMaterial.Select(s => s.Index.Length).Max();

                    Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                    item.ListMaterial = item.ListMaterial
                               .Select(s =>
                                   new
                                   {
                                       OrgStr = s,
                                       SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                                   })
                               .OrderBy(x => x.SortStr)
                               .Select(x => x.OrgStr).ToList();
                }

                if (modelSearch.Pricing.HasValue)
                {
                    //decimal pricing = modelSearch.Pricing.Value;
                    //ListMaterial = ListMaterial.Where(u => u.Pricing == pricing);
                    switch (modelSearch.MaterialPriceType)
                    {
                        // <=
                        case 5:
                            item.ListMaterial = item.ListMaterial.Where(u => u.Pricing <= modelSearch.Pricing).ToList();
                            break;
                        // >= 
                        case 3:
                            item.ListMaterial = item.ListMaterial.Where(u => u.Pricing >= modelSearch.Pricing).ToList();
                            break;
                        // <
                        case 4:
                            item.ListMaterial = item.ListMaterial.Where(u => u.Pricing < modelSearch.Pricing.Value).ToList();
                            break;
                        // >
                        case 2:
                            item.ListMaterial = item.ListMaterial.Where(u => u.Pricing > modelSearch.Pricing).ToList();
                            break;
                        // ==
                        case 1:
                            item.ListMaterial = item.ListMaterial.Where(u => u.Pricing == modelSearch.Pricing).ToList();
                            break;
                        default:
                            break;
                    }
                }

                item.Pricing = item.ListMaterial.Sum(r => r.Pricing * r.Quantity);

                priceMax = item.ListMaterial.Select(r => r.Pricing).DefaultIfEmpty(0).Max();

                if (priceMax > searchResult.MaxPricing)
                {
                    searchResult.MaxPricing = priceMax;
                }

                item.Index = ConvertToRoman(i);
                i++;
            }
            

            searchResult.MaxDeliveryDay = ListMaterial.Select(r => r.DeliveryDays).DefaultIfEmpty(0).Max();

            searchResult.ListResult = moduleInPractices;
            searchResult.TotalItem = moduleInPractices.Count();
            searchResult.TotalAmount = moduleInPractices.Sum(s => s.Pricing * s.Qty);

            return searchResult;
        }

        private static string ConvertToRoman(int convertThis)
        {
            int leftovers;              //store mod results
            string RomanNumeral = "";   //store roman numeral string
            Dictionary<string, int> dict = new Dictionary<string, int>()
            {
                {"M", 1000},// 1000 = M
                {"CM", 900},// 900 = CM
                {"D", 500}, // 500 = D
                {"CD", 400},// 400 = CD
                {"C", 100}, // 100 = C
                {"XC", 90}, // 90 = XC
                {"L", 50},  // 50 = L
                {"XL", 40}, // 40 = XL
                {"X", 10},  // 10 = X
                {"IX", 9},  // 9 = IX
                {"V", 5},   // 5 = V
                {"IV", 4},  // 4 = IV
                {"I", 1},   // 1 = I
            };
            foreach (KeyValuePair<string, int> pair in dict)
            {
                if (convertThis >= pair.Value)
                {
                    leftovers = convertThis % pair.Value;
                    int remainder = (convertThis - leftovers) / pair.Value;
                    convertThis = leftovers;
                    while (remainder > 0)
                    {
                        RomanNumeral += pair.Key; remainder--;
                    }
                }
            }
            return RomanNumeral;
        }
        public SearchResultModel<PracticeMaterialModel> SearchPracticeMaterial(PracticeMaterialSearchModel modelSearch)
        {
            SearchResultModel<PracticeMaterialModel> searchResult = new SearchResultModel<PracticeMaterialModel>();
            var dataQuery = (from a in db.PracticeMaterials.AsNoTracking()
                             join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.MaterialGroups.AsNoTracking() on c.MaterialGroupId equals d.Id
                             join e in db.Manufactures.AsNoTracking() on c.ManufactureId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Units.AsNoTracking() on c.UnitId equals f.Id
                             where a.PracticeId.Equals(modelSearch.PracticeId)
                             select new PracticeMaterialModel
                             {
                                 Id = a.Id,
                                 MaterialId = a.MaterialId,
                                 PracticeId = a.PracticeId,
                                 MaterialGroupName = d.Name,
                                 Name = c.Name,
                                 Code = c.Code,
                                 ManufactureName = e.Name,
                                 UnitName = f.Name,
                                 Quantity = a.Quantity,
                                 Pricing = a.Price,
                                 TotalPrice = a.TotalPrice,
                                 Leadtime = a.Leadtime.ToString()
                             }).AsQueryable();
            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }
        public SearchResultModel<MaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultModel<MaterialModel> searchResult = new SearchResultModel<MaterialModel>();
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id
                             join d in db.Units.AsNoTracking() on a.UnitId equals d.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new MaterialModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ManufactureId = a.ManufactureId,
                                 ManufactureName = b.Name,
                                 MaterialGroupName = c.Name,
                                 UnitName = d.Name,
                                 Pricing = a.Pricing,
                                 Note = a.Note
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
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddPracticeMaterial(PracticeMaterialModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var practices = db.PracticeMaterials.Where(u => u.PracticeId.Equals(model.PracticeId)).ToList();
                if (practices.Count > 0)
                {
                    db.PracticeMaterials.RemoveRange(practices);
                }
                try
                {
                    if (model.listSelect != null)
                    {
                        foreach (var item in model.listSelect)
                        {
                            if (item.MaterialId != null)
                            {
                                item.Id = item.MaterialId;
                            }
                            PracticeMaterial practiceMaterial = new PracticeMaterial
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = item.Id,
                                PracticeId = model.PracticeId,
                                Quantity = item.Quantity,
                                Price = item.Pricing,
                                Leadtime = item.Leadtime,
                                TotalPrice = Convert.ToInt32(item.Quantity * item.Pricing),
                            };
                            db.PracticeMaterials.Add(practiceMaterial);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        public string ExportExcelPracticeMaterial(PracticeMaterialSearchModel modelSearch)
        {
            List<ModuleInPracticeModel> listPracticeMaterial = SearchModuleInPractice(modelSearch).ListResult;

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/PracticeMaterial.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                List<ModuleMaterialExcelModel> listMaterial = new List<ModuleMaterialExcelModel>();
                ModuleMaterialExcelModel moduleMaterialResultModel;
                foreach (var module in listPracticeMaterial)
                {
                    moduleMaterialResultModel = new ModuleMaterialExcelModel();
                    moduleMaterialResultModel.Index = module.Index;
                    moduleMaterialResultModel.MaterialName = module.ModuleName + " - " + module.ModuleCode;
                    moduleMaterialResultModel.IsExport = true;
                    listMaterial.Add(moduleMaterialResultModel);
                    if (module.ListMaterial != null)
                    {
                        foreach (var material in module.ListMaterial)
                        {
                            moduleMaterialResultModel = new ModuleMaterialExcelModel();
                            moduleMaterialResultModel.MaterialName = material.MaterialName;
                            moduleMaterialResultModel.MaterialCode = material.MaterialCode;
                            moduleMaterialResultModel.ModuleId = material.ModuleId;
                            moduleMaterialResultModel.MaterialId = material.MaterialId;
                            moduleMaterialResultModel.ManufactureName = material.ManufactureName;
                            moduleMaterialResultModel.MaterialGroupName = material.MaterialGroupName;
                            moduleMaterialResultModel.UnitName = material.UnitName;
                            moduleMaterialResultModel.Quantity = material.Quantity.ToString();
                            moduleMaterialResultModel.Pricing = material.Pricing;
                            moduleMaterialResultModel.DeliveryDays = material.DeliveryDays;
                            moduleMaterialResultModel.IsExport = false;
                            moduleMaterialResultModel.Index = material.Index;
                            listMaterial.Add(moduleMaterialResultModel);
                        }
                    }
                }

                var listExport = listMaterial.Select((o, i) => new
                {
                    stt = o.Index,
                    o.MaterialName,
                    o.MaterialCode,
                    o.MaterialGroupName,
                    o.UnitName,
                    o.Pricing,
                    o.Quantity,
                    totalPrice = o.Pricing * Convert.ToDecimal(o.Quantity),
                    o.ManufactureName,
                    o.DeliveryDays
                });

                var total = listMaterial.Count;
                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders.Color = ExcelKnownColors.Black;
                //foreach (var module in listPracticeMaterial)
                //{
                //    if(module.ListMaterial.Count == 0)
                //    {
                //        sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].CellStyle.Font.Bold = true;
                //    }
                //    else
                //    {
                //        foreach (var material in module.ListMaterial)
                //        {
                //            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].CellStyle.Font.Bold = false;
                //        }
                //    }

                //}
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 10].CellStyle.WrapText = true;
                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách Vật tư" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client

                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách Vật tư" + ".xls";
                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }

        }
    }
}
