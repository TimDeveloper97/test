using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.ProductAccessories;
using NTS.Model.Materials;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Resource;
using Syncfusion.XlsIO;
using System.Web.Hosting;
using System.Web;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;

namespace QLTK.Business.ProductAccessories
{
    public class ProductAccessoriesBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();
        private readonly MaterialBusiness materialBusiness = new MaterialBusiness();
        public SearchResultModel<ProductAccessoriesModel> SearchProductAccessories(ProductAccessoriesSearchModel modelSearch)
        {
            SearchResultModel<ProductAccessoriesModel> searchResult = new SearchResultModel<ProductAccessoriesModel>();
            var dataQuery = (from a in db.ProductAccessories.AsNoTracking()
                             join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.Manufactures.AsNoTracking() on c.ManufactureId equals d.Id
                             where a.ProductId.Equals(modelSearch.ProductId) && a.Type == Constants.PracticeSupMaterial_Type_Material
                             orderby c.Code
                             select new
                             {
                                 Id = a.MaterialId,
                                 Code = c.Code,
                                 Name = c.Name,
                                 ProductId = a.ProductId,
                                 MaterialId = a.MaterialId,
                                 Manafacture = d.Name,
                                 Quantity = a.Quantity,
                                 Price = c.Pricing,
                                 Amount = a.Amount,
                                 Note = a.Note,
                                 c.LastBuyDate,
                                 c.InputPriceDate,
                                 c.PriceHistory,
                                 Type = a.Type
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
            var data = dataQuery.ToList();

            searchResult.ListResult = new List<ProductAccessoriesModel>();
            ProductAccessoriesModel productAccessoriesModel;

            int day = materialBusiness.GetConfigMaterialLastByDate();

            foreach (var item in data)
            {
                productAccessoriesModel = new ProductAccessoriesModel()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    ProductId = item.ProductId,
                    MaterialId = item.MaterialId,
                    Manafacture = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Amount = item.Amount,
                    Note = item.Note,
                    Type = item.Type
                };

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        productAccessoriesModel.Price = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        productAccessoriesModel.Price = 0;
                    }
                }

                searchResult.ListResult.Add(productAccessoriesModel);
            }

            var products = (from a in db.ProductAccessories.AsNoTracking()
                            join b in db.Modules.AsNoTracking() on a.MaterialId equals b.Id
                            join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                            where a.ProductId.Equals(modelSearch.ProductId) && a.Type == Constants.PracticeSupMaterial_Type_Module
                            orderby b.Code
                            select new ProductAccessoriesModel
                            {
                                Id = b.Id,
                                MaterialId = a.MaterialId,
                                Name = b.Name,
                                Code = b.Code,
                                Manafacture = Constants.Manufacture_TPA,
                                Quantity = 1,
                                Price = b.Pricing,
                                Type = a.Type
                            }).ToList();

            foreach (var item in products)
            {
                item.Price = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
                item.Amount = item.Quantity * item.Price;
            }

            searchResult.ListResult.AddRange(products);
            searchResult.ListResult = searchResult.ListResult.OrderBy(i => i.Code).ToList();
            return searchResult;
        }
        public SearchResultModel<MaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultModel<MaterialModel> searchResult = new SearchResultModel<MaterialModel>();
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new MaterialModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ManufactureId = a.ManufactureId,
                                 ManufactureName = b.Name,
                                 Pricing = a.Pricing,
                                 Note = a.Note,
                                 LastBuyDate = a.LastBuyDate,
                                 InputPriceDate = a.InputPriceDate,
                                 PriceHistory = a.PriceHistory
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.ManufactureId))
            {
                dataQuery = dataQuery.Where(a => a.ManufactureId.Equals(modelSearch.ManufactureId));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;

            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var item in listResult)
            {
                int i = 1;
                item.Index = i;
                i++;

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);
                    if (timeSpan.Days <= day)
                    {
                        item.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Pricing = 0;
                    }
                }
            }

            return searchResult;
        }

        public void AddProductAccessories(MaterialModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productAccessories = db.ProductAccessories.Where(i => i.ProductId.Equals(model.ProductId)).ToList();
                    if (productAccessories.Count > 0)
                    {
                        db.ProductAccessories.RemoveRange(productAccessories);
                    }

                    foreach (var item in model.listSelect)
                    {
                        ProductAccessory newProductAccessory = new ProductAccessory
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = model.ProductId,
                            MaterialId = item.Id,
                            Quantity = item.Quantity,
                            Price = item.Pricing,
                            Amount = item.Quantity * item.Pricing,
                            Note = item.Note,
                        };
                        db.ProductAccessories.Add(newProductAccessory);
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

        public void UpdateProductAccessories(ProductAccessoriesModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productAccessories = db.ProductAccessories.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
                    productAccessories.Quantity = model.Quantity;
                    productAccessories.Amount = productAccessories.Price * model.Quantity;
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

        public void DeleteProductAccessories(ProductAccessoriesModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productAccessories = db.ProductAccessories.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (productAccessories == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductAccessories);
                    }
                    db.ProductAccessories.Remove(productAccessories);
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

        public string ExportExcel(ProductAccessoriesExcelModel model)
        {
            List<ProductAccessoriesModel> listModel = model.ListData;
            var nameProduct = db.Products.AsNoTracking().FirstOrDefault(a => a.Id.Equals(model.ProductId)).Name;
            if (listModel.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProductAccessories.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.Code,
                    a.Manafacture,
                    a.Quantity,
                    a.Price,
                    a.Amount
                });
                if (listExport.Count() > 0)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 7].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + nameProduct + "_Danh sách phụ kiện" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + nameProduct + "_Danh sách phụ kiện" + ".xls";

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
