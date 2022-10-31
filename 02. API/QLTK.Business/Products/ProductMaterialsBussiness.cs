using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Datasheet;
using NTS.Model.Materials;
using NTS.Model.ModuleMaterials;
using NTS.Model.Product;
using NTS.Model.ProductDocument;
using NTS.Model.ProductMaterials;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ProductMaterials
{
    public class ProductMaterialsBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<ProductMaterialsModel> SearchProductMaterials(ProductMaterialSearchModel model)
        {
            SearchResultModel<ProductMaterialsModel> searchResult = new SearchResultModel<ProductMaterialsModel>();
            var dataQuery = (from a in db.ModuleInPractices.AsNoTracking()
                             join b in db.Modules.AsNoTracking() on a.ModuleId equals b.Id
                             join c in db.ModuleMaterials.AsNoTracking() on b.Id equals c.ModuleId
                             join d in db.Materials.AsNoTracking() on c.MaterialId equals d.Id
                             join m in db.Manufactures.AsNoTracking() on c.ManufacturerId equals m.Id
                             //where a.ProductId.Equals(model.ProductId)
                             select new ProductMaterialsModel
                             {
                                 Id = c.Id,
                                 //ProductId = c.ProductId,
                                 MaterialId = c.MaterialId,
                                 MaterialCode = c.MaterialCode,
                                 MaterialName = c.MaterialName,
                                 Specification = c.Specification,
                                 RawMaterialCode = c.RawMaterialCode,
                                 RawMaterial = c.RawMaterial,
                                 Price = c.Price,
                                 Quantity = c.Quantity,
                                 Amount = c.Amount,
                                 Weight = c.Weight,
                                 ManufacturerId = c.ManufacturerId,
                                 ManufacturerCode = m.Code,
                                 Note = c.Note,
                                 UnitName = c.UnitName,
                                 //SetupFilePath = c.SetupFilePath,
                                 //DatasheetPath = c.DatasheetPath,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.MaterialName))
            {
                dataQuery = dataQuery.Where(u => u.MaterialName.ToUpper().Contains(model.MaterialName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(model.MaterialCode))
            {
                dataQuery = dataQuery.Where(u => u.MaterialCode.ToUpper().Contains(model.MaterialCode.ToUpper()));
            }
            //searchResult.Status1 = dataQuery.Max(u=>u.lea)
            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }
        public SearchResultModel<ModuleMaterialResultModel> SearchProductMaterialsSetup(ProductMaterialSearchModel model)
        {
            SearchResultModel<ModuleMaterialResultModel> searchResult = new SearchResultModel<ModuleMaterialResultModel>();
            var dataQuery = (from a in db.PracticInProducts.AsNoTracking()
                             join b in db.ModuleInPractices.AsNoTracking() on a.PracticeId equals b.PracticeId
                             join c in db.Modules.AsNoTracking() on b.ModuleId equals c.Id
                             join d in db.ModuleMaterials.AsNoTracking() on c.Id equals d.ModuleId
                             join e in db.Materials.AsNoTracking() on d.MaterialId equals e.Id
                             join f in db.ProductModuleMaterials.AsNoTracking() on a.ProductId equals f.ProductId into af
                             from f in af.DefaultIfEmpty()
                             join m in db.Manufactures.AsNoTracking() on e.ManufactureId equals m.Id
                             where a.ProductId.Equals(model.ProductId)
                             select new ModuleMaterialResultModel
                             {
                                 Id = d.Id,
                                 ModuleMaterialId = d.Id,
                                 MaterialId = e.Id,
                                 IsSetup = e.IsSetup,
                                 MaterialCode = d.MaterialCode,
                                 MaterialName = d.MaterialName,
                                 Specification = d.Specification,
                                 RawMaterialCode = d.RawMaterialCode,
                                 RawMaterial = d.RawMaterial,
                                 ManufacturerId = d.ManufacturerId,
                                 ManufacturerCode = m.Code,
                                 DeliveryDays = e.DeliveryDays,
                                 Note = d.Note,
                                 Path = f.Path,
                                 ModuleCode = c.Code,
                                 ModuleId = c.Id,
                             }).AsQueryable();
            dataQuery = dataQuery.Where(u => u.IsSetup == true);
            searchResult.TotalItem = dataQuery.Count();
            if (dataQuery.Count() > 0)
            {
                searchResult.Status1 = dataQuery.Max(u => u.DeliveryDays);
            }
            else
            {
                searchResult.Status1 = 0;
            }

            List<ModuleMaterialResultModel> listRs = new List<ModuleMaterialResultModel>();
            var lstRs = dataQuery.GroupBy(t => new { t.ModuleId, t.ModuleCode, t.Id, t.ModuleMaterialId, t.MaterialId, t.ManufacturerId, t.MaterialCode, t.MaterialName, t.Specification, t.RawMaterialCode, t.ManufacturerCode, t.Note, t.Path }).ToList();
            foreach (var item in lstRs)
            {
                ModuleMaterialResultModel rs = new ModuleMaterialResultModel();
                rs.Id = item.Key.Id;
                rs.ModuleMaterialId = item.Key.ModuleMaterialId;
                rs.MaterialId = item.Key.MaterialId;
                rs.ManufacturerId = item.Key.ManufacturerId;
                rs.MaterialCode = item.Key.MaterialCode;
                rs.MaterialName = item.Key.MaterialName;
                rs.Specification = item.Key.Specification;
                rs.RawMaterialCode = item.Key.RawMaterialCode;
                rs.ManufacturerCode = item.Key.ManufacturerCode;
                rs.Note = item.Key.Note;
                rs.Path = item.Key.Path;
                rs.ModuleId = item.Key.ModuleId;
                rs.ModuleCode = item.Key.ModuleCode;
                listRs.Add(rs);
            }
            searchResult.ListResult = listRs;
            foreach (var item in listRs)
            {
                var datashet = (from a in db.MaterialDataSheets.AsNoTracking()
                                join c in db.DataSheets.AsNoTracking() on a.DataSheetId equals c.Id
                                where a.MaterialId.Equals(item.MaterialId)
                                select new DatasheetModel
                                {
                                    FilePath = c.FilePath,
                                    FileName = c.FileName
                                }).ToList();
                item.ListDatashet = datashet;
                var fileSetup = (from a in db.ModuleMaterialAttaches.AsNoTracking()
                                 where a.ModuleMaterialId.Equals(item.ModuleMaterialId)
                                 select new FileSetupModel
                                 {
                                     Id = a.Id,
                                     ModuleMaterialId = a.ModuleMaterialId,
                                     FileName = a.FileName,
                                     Size = a.FileSize,
                                     Path = a.Path,
                                 }).ToList();
                item.ListFileSetup = fileSetup;
            }
            return searchResult;
        }

        public SearchResultModel<MaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultModel<MaterialModel> searchResult = new SearchResultModel<MaterialModel>();
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                             join c in db.Units.AsNoTracking() on a.UnitId equals c.Id
                             join d in db.RawMaterials.AsNoTracking() on a.RawMaterialId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new MaterialModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ManufactureId = a.ManufactureId,
                                 ManufactureName = b.Name,
                                 RawMaterial = a.RawMaterial,
                                 RawMaterialId = a.RawMaterialId,
                                 RawMaterialCode = d.Code,
                                 UnitId = a.UnitId,
                                 UnitName = c.Name,
                                 Weight = a.Weight,
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

        public void AddProductMaterials(MaterialModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in model.listSelect)
                    {
                        if (item.Weight == "")
                        {
                            item.Weight = "0";
                        }
                        ProductMaterial newProductMaterial = new ProductMaterial
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProductId = model.ProductId,
                            MaterialId = item.Id,
                            MaterialCode = item.Code,
                            MaterialName = item.Name,
                            //Specification
                            RawMaterialCode = item.RawMaterialCode,
                            RawMaterial = item.RawMaterial,
                            Price = item.Pricing,
                            Quantity = item.Quantity,
                            Amount = Convert.ToInt32(item.Pricing * item.Quantity),
                            Weight = Convert.ToInt32(item.Weight),
                            ManufacturerId = item.ManufactureId,
                            Note = item.Note,
                            UnitName = item.UnitName,
                        };

                        db.ProductMaterials.Add(newProductMaterial);
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

        public void DeleteProductMaterials(ProductMaterialsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productMaterials = db.ProductMaterials.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (productMaterials == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ProductMaterials);
                    }
                    db.ProductMaterials.Remove(productMaterials);
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
        public void UpdateProductMaterials(ProductMaterialsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var productDocuments = db.ProductDocuments.Where(a => a.ProductId.Equals(model.ProductId)).ToList();
                    if (productDocuments.Count > 0)
                    {
                        db.ProductDocuments.RemoveRange(productDocuments);
                    }

                    if (model.ListFielDocument.Count > 0)
                    {
                        foreach (var item in model.ListFielDocument)
                        {
                            if (item.FileName != null && item.Path != null)
                            {
                                ProductDocument productDocument = new ProductDocument()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductId = model.ProductId,
                                    Path = item.Path,
                                    FileName = item.FileName,
                                    CreateDate = DateTime.Now,
                                    FileSize = item.FileSize,
                                    Note = item.Note,
                                    FileType = item.FileType,
                                };
                                db.ProductDocuments.Add(productDocument);
                            }
                        }

                    }

                    if (model.ListFileSetup.Count > 0)
                    {

                        //var productMaterialAttach=db.ProductMaterialAttaches.Where(a=>a.ProductMaterialId.Equals())
                        var productModuleMaterial = db.ProductModuleMaterials.Where(u => u.ProductId.Equals(model.ProductId)).FirstOrDefault(u => u.ModuleMaterialId.Equals(model.Id));
                        if (productModuleMaterial == null)
                        {
                            ProductModuleMaterial productModule = new ProductModuleMaterial()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = model.ProductId,
                                ModuleMaterialId = model.Id,
                                Path = model.ListFileSetup[0].Path,
                            };
                            db.ProductModuleMaterials.Add(productModule);
                        }
                        else
                        {
                            productModuleMaterial.Path = model.ListFileSetup[0].Path;
                        }
                    }

                    if (model.ListFileDatasheet.Count() > 0)
                    {
                        foreach (var item in model.ListFileDatasheet)
                        {
                            DataSheet dataSheet = new DataSheet()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ManufactureId = item.ManufactureId,
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

        public ProductMaterialsModel GetProductInfo(ProductMaterialsModel model)
        {
            try
            {
                var list = (from a in db.ProductDocuments.AsNoTracking()
                            where a.ProductId.Equals(model.ProductId) && a.FileType.Equals(1)
                            select new ProductDocumentModel
                            {
                                Id = a.Id,
                                ProductId = a.ProductId,
                                Path = a.Path,
                                FileName = a.FileName,
                                CreateDate = a.CreateDate,
                                FileSize = a.FileSize,
                                Note = a.Note,
                                FileType = a.FileType
                            }).ToList();
                model.ListFielDocument = list;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ExportExcel(ProductMaterialsModel model)
        {
            model.IsExport = true;
            var dataQuery = (from a in db.ProductMaterials.AsNoTracking()
                             join b in db.Products.AsNoTracking() on a.ProductId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join m in db.Manufactures.AsNoTracking() on a.ManufacturerId equals m.Id
                             //where model.ProductId.Equals(model.ProductId)
                             orderby c.Name
                             select new ProductMaterialsModel
                             {
                                 Id = a.Id,
                                 ProductId = a.ProductId,
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
                                 ManufacturerCode = m.Code,
                                 Note = a.Note,
                                 UnitName = a.UnitName,
                                 SetupFilePath = a.SetupFilePath,
                                 DatasheetPath = a.DatasheetPath,
                             }).AsQueryable();

            List<ProductMaterialsModel> listModel = dataQuery.ToList();

            if (listModel.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProductMaterials.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.MaterialName,
                    a.MaterialCode,
                    a.Specification,
                    a.RawMaterialCode,
                    a.UnitName,
                    a.Quantity,
                    a.Price,
                    a.Amount,
                    a.RawMaterial,
                    a.Weight,
                    a.ManufacturerCode,
                    a.Note
                });
                if (listExport.Count() > 0)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count(), ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách vật tư" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách vật tư" + ".xls";

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
