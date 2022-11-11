using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ModuleMaterials;
using NTS.Model.Practice;
using NTS.Model.PracticeProduct;
using NTS.Model.Product;
using NTS.Model.Repositories;
using QLTK.Business.ModuleMaterials;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.PracticeProduct
{
    public class PracticeProductBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        private ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();

        public PracticeProductSearchResultModel SearchPracticeProduct(PracticeProductSearchModel modelSearch)
        {

            PracticeProductSearchResultModel searchResult = new PracticeProductSearchResultModel();

            try
            {
                var dataQuey = (from a in db.Products.AsNoTracking()
                                join b in db.PracticInProducts.AsNoTracking() on a.Id equals b.ProductId
                                join d in db.ProductGroups.AsNoTracking() on a.ProductGroupId equals d.Id
                                where b.PracticeId.Equals(modelSearch.PracticeId)
                                orderby a.Code
                                select new ProductResultModel
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    Name = a.Name,
                                    ProductGroupId = a.ProductGroupId,
                                    ProductGroupName = d.Name,
                                    ProductGroupCode = d.Code,
                                    Quantity = b.Qty,
                                    Pricing = a.Pricing,
                                    ProcedureTime = a.ProcedureTime
                                }).ToList();

                decimal moduleAmount = 0;

                foreach (var item in dataQuey)
                {
                    var modules = (from b in db.ProductModules.AsNoTracking()
                                   where b.ProductId.Equals(item.Id)
                                   select new
                                   {
                                       b.ModuleId,
                                       b.Quantity
                                   }).ToList();

                    moduleAmount = 0;
                    foreach (var module in modules)
                    {
                        moduleAmount += module.Quantity * moduleMaterialBusiness.GetPriceModuleByModuleId(module.ModuleId, 0);
                    }

                    item.Pricing = moduleAmount;
                }

                searchResult.Products = dataQuey;


                var listModuleInPractice = (from h in db.ModuleInPractices.AsNoTracking()
                                            join k in db.Modules.AsNoTracking() on h.ModuleId equals k.Id
                                            join g in db.ModuleGroups.AsNoTracking() on k.ModuleGroupId equals g.Id
                                            where h.PracticeId.Equals(modelSearch.PracticeId)
                                            orderby k.Code
                                            select new ModuleInPracticeModel
                                            {
                                                Id = h.Id,
                                                ModuleId = h.ModuleId,
                                                ModuleName = k.Name,
                                                ModuleCode = k.Code,
                                                ModuleGroupCode = g.Code,
                                                Qty = h.Qty,
                                                LeadTime = k.Leadtime
                                            }).ToList();

                ModulePriceInfoModel modulePriceInfoModel;
                foreach (var module in listModuleInPractice)
                {
                    modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(module.ModuleId);
                    module.Pricing = modulePriceInfoModel.Price;
                    module.IsNoPrice = modulePriceInfoModel.IsNoPrice;
                }

                searchResult.Modules = listModuleInPractice;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;

        }

        public PracticeProductSearchResultModel SearchPracticeModule(PracticeProductSearchModel modelSearch)
        {
            PracticeProductSearchResultModel searchResult = new PracticeProductSearchResultModel();

            var listModuleInPractice = (from h in db.ModuleInPractices.AsNoTracking()
                                        join k in db.Modules.AsNoTracking() on h.ModuleId equals k.Id
                                        join g in db.ModuleGroups.AsNoTracking() on k.ModuleGroupId equals g.Id
                                        where h.PracticeId.Equals(modelSearch.PracticeId)
                                        orderby k.Code
                                        select new ModuleInPracticeModel
                                        {
                                            Id = h.Id,
                                            ModuleId = h.ModuleId,
                                            ModuleName = k.Name,
                                            ModuleCode = k.Code,
                                            ModuleGroupCode = g.Code,
                                            Qty = h.Qty,
                                            LeadTime = k.Leadtime
                                        }).ToList();

            ModulePriceInfoModel modulePriceInfoModel;
            if (listModuleInPractice.Count() > 0)
            {
                foreach (var module in listModuleInPractice)
                {
                    modulePriceInfoModel = moduleMaterialBusiness.GetPriceAndPriceStatusModuleByModuleId(module.ModuleId);
                    module.Pricing = modulePriceInfoModel.Price;
                    module.IsNoPrice = modulePriceInfoModel.IsNoPrice;
                }
            }

            searchResult.Modules = listModuleInPractice;

            return searchResult;
        }

        public void AddModuleInPratice(PracticeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var moduleInPracticeOld = db.ModuleInPractices.Where(a => a.PracticeId.Equals(model.Id)).ToList();
                    db.ModuleInPractices.RemoveRange(moduleInPracticeOld);

                    List<ModuleInPractice> listNewModule = new List<ModuleInPractice>();
                    if (model.ListModuleInPractice.Count() > 0)
                    {
                        ModuleInPractice newModule;
                        foreach (var item in model.ListModuleInPractice)
                        {
                            newModule = new ModuleInPractice();
                            newModule.Id = Guid.NewGuid().ToString();
                            newModule.ModuleId = item.ModuleId;
                            newModule.PracticeId = model.Id;
                            newModule.Qty = item.Qty;
                            newModule.Version = 0;
                            listNewModule.Add(newModule);
                        }
                    }
                    db.ModuleInPractices.AddRange(listNewModule);
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Practice, model.Id, string.Empty, "Module");

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
        public string ExportExcelPracticeProduct(PracticeProductSearchModel modelSearch)
        {
            List<ProductResultModel> listPracticeProduct = SearchPracticeProduct(modelSearch).Products;

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/PracticeProduct.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];
                var total = listPracticeProduct.Count;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport = listPracticeProduct.Select((o, i) => new
                {
                    Index = i + 1,
                    o.Name,
                    o.Code,
                    o.ProductGroupName,
                    o.Pricing,
                    o.Quantity,
                    total = o.Pricing * o.Quantity,
                    o.ProcedureTime
                });

                if (listExport.Count() == 0)
                {
                    throw NTSException.CreateInstance("Không có dữ liệu!");
                }

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 8].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 8].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 8].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách Thiết bị" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client

                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách Thiết bị" + ".xls";
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
