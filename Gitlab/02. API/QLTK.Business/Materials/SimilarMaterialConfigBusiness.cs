using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Materials;
using NTS.Model.Repositories;
using NTS.Model.SimilarMaterialConfig;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.SimilarMaterialConfigs
{
    public class SimilarMaterialConfigBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<MaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultModel<MaterialModel> searchResult = new SearchResultModel<MaterialModel>();
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Units.AsNoTracking() on a.UnitId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Manufactures.AsNoTracking() on a.ManufactureId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.RawMaterials.AsNoTracking() on a.RawMaterialId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new MaterialModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ManufactureId = a.ManufactureId,
                                 ManufactureName = d.Name,
                                 MaterialGroupName = c.Name,
                                 UnitName = b.Name,
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
            //var listResult = dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public SearchResultModel<SimilarMaterialConfigModel> SearchSimilarMaterialConfig(SimilarMaterialConfigSearchModel modelSearch)
        {
            SearchResultModel<SimilarMaterialConfigModel> searchResult = new SearchResultModel<SimilarMaterialConfigModel>();
            var dataQuery = (from a in db.SimilarMaterialConfigs.AsNoTracking()
                             join b in db.SimilarMaterials.AsNoTracking() on a.SimilarMaterialId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.MaterialGroups.AsNoTracking() on c.MaterialGroupId equals d.Id
                             join e in db.Manufactures.AsNoTracking() on c.ManufactureId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Units.AsNoTracking() on c.UnitId equals f.Id into af
                             from f in af.DefaultIfEmpty()
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
                                 Parameter = a.Parameter,
                                 Note = c.Note,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.SimilarMaterialId))
            {
                dataQuery = dataQuery.Where(u => u.SimilarMaterialId.Equals(modelSearch.SimilarMaterialId));
            }
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.Contains(modelSearch.Name));
            }
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.Contains(modelSearch.Code));
            }
            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddSimilarMaterialConfig(SimilarMaterialConfigModel model)
        {
            List<string> exsit = new List<string>();
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.ListSimilarMaterialConfig.Count() > 0)
                    {
                        foreach (var item in model.ListSimilarMaterialConfig)
                        {
                            var similarMaterial = db.SimilarMaterialConfigs.AsNoTracking().FirstOrDefault(u => u.MaterialId.Equals(item.Id));
                            if (similarMaterial != null)
                            {
                                exsit.Add(item.Name);
                            }

                            SimilarMaterialConfig similarMaterialConfig = new SimilarMaterialConfig
                            {
                                Id = Guid.NewGuid().ToString(),
                                SimilarMaterialId = model.SimilarMaterialId,
                                MaterialId = item.Id,
                                Parameter = item.Parameter,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.UpdateBy,
                                UpdateDate = DateTime.Now,
                            };
                            db.SimilarMaterialConfigs.Add(similarMaterialConfig);
                        }
                    }

                    if (exsit.Count > 0)
                    {
                        throw NTSException.CreateInstance("Vật tư <" + string.Join(", ", exsit) + "> đã có trong nhóm khác!");
                        //throw new Exception("File Upload không đúng định dạng hãy kiểm tra lại");
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

        public void UpdateSimilarMaterialConfig(SimilarMaterialConfigModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var similarMaterialConfigs = db.SimilarMaterialConfigs.Where(u => u.SimilarMaterialId.Equals(model.SimilarMaterialId)).ToList();
                if (similarMaterialConfigs.Count > 0)
                {
                    db.SimilarMaterialConfigs.RemoveRange(similarMaterialConfigs);
                }
                try
                {
                    if (model.ListSimilarMaterialConfig.Count() > 0)
                    {
                        foreach (var item in model.ListSimilarMaterialConfig)
                        {
                            SimilarMaterialConfig similarMaterialConfig = new SimilarMaterialConfig
                            {
                                Id = Guid.NewGuid().ToString(),
                                SimilarMaterialId = model.SimilarMaterialId,
                                MaterialId = item.MaterialId,
                                Parameter = item.Parameter,
                                CreateBy = model.CreateBy,
                                CreateDate = DateTime.Now,
                                UpdateBy = model.UpdateBy,
                                UpdateDate = DateTime.Now,
                            };
                            db.SimilarMaterialConfigs.Add(similarMaterialConfig);
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

        public void DeleteSimilarMaterialConfig(SimilarMaterialConfigModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var similarMaterialConfig = db.SimilarMaterialConfigs.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (similarMaterialConfig == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Material);
                    }

                    db.SimilarMaterialConfigs.Remove(similarMaterialConfig);
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

        public MaterialModel GetMaterialInfo(MaterialModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                throw new Exception("Có lỗi trong quá trình xử lý!");
            }
            try
            {
                var material = db.Materials.AsNoTracking().FirstOrDefault(o => o.Id.Equals(model.Id));
                model.Id = material.Id;
                model.Code = material.Code;
                model.Name = material.Name;
                model.Note = material.Note;
                model.MaterialGroupId = material.MaterialGroupId;
                model.Pricing = material.Pricing;
                model.DeliveryDays = material.DeliveryDays;
                model.ImagePath = material.ImagePath;
                model.ThumbnailPath = material.ThumbnailPath;
                model.IsUsuallyUse = material.IsUsuallyUse;
                model.MechanicalType = material.MechanicalType;
                model.RawMaterial = material.RawMaterial;
                model.Is3D = material.Is3D;
                model.IsDataSheet = material.IsDataSheet;
                model.IsSetup = material.IsSetup;
                model.Weight = material.Weight;
                model.RawMaterialId = material.RawMaterialId;
                model.LastBuyDate = material.LastBuyDate;
                model.CreateBy = material.CreateBy;
                model.CreateDate = material.CreateDate;
                model.UpdateBy = material.UpdateBy;
                model.UpdateDate = material.UpdateDate;
                if (!string.IsNullOrEmpty(material.MaterialGroupId))
                {
                    model.MaterialGroupName = db.MaterialGroups.AsNoTracking().Where(t => t.Id.Equals(material.MaterialGroupId)).FirstOrDefault().Name;
                }
                if (!string.IsNullOrEmpty(material.MaterialGroupTPAId))
                {
                    model.MaterialGroupTPAId = db.MaterialGroupTPAs.AsNoTracking().Where(t => t.Id.Equals(material.MaterialGroupTPAId)).FirstOrDefault().Name;
                }
                if (!string.IsNullOrEmpty(material.ManufactureId))
                {
                    model.ManufactureId = db.Manufactures.AsNoTracking().Where(t => t.Id.Equals(material.ManufactureId)).FirstOrDefault().Name;
                }
                if (!string.IsNullOrEmpty(material.UnitId))
                {
                    model.UnitId = db.Units.AsNoTracking().Where(t => t.Id.Equals(material.UnitId)).FirstOrDefault().Name;
                }

                if (material.MaterialType.Equals("1"))
                {
                    model.MaterialType = "Vật tư tiêu chuẩn";
                }
                else
                {
                    model.MaterialType = "Vật tư phi tiêu chuẩn";
                }
                if (material.Status.Equals("0"))
                {
                    model.Status = "Đang sử dụng";
                }
                else if (material.Status.Equals("1"))
                {
                    model.Status = "Tạm dừng";
                }
                else if (material.Status.Equals("2"))
                {
                    model.Status = "Ngưng sản xuất";
                }
                var listImage = (from a in db.MaterialImages.AsNoTracking()
                                 where a.MaterialId.Equals(model.Id)
                                 select new MaterialImageModel
                                 {
                                     Id = a.Id,
                                     MaterialId = a.MaterialId,
                                     Path = a.Path,
                                     ThumbnailPath = a.ThumbnailPath
                                 });
                model.ListImage = listImage.ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình xử lý ");
            }
        }

        public string ExportExcel(SimilarMaterialConfigSearchModel model)
        {
            var dataQuery = (from a in db.SimilarMaterialConfigs.AsNoTracking()
                             join b in db.SimilarMaterials.AsNoTracking() on a.SimilarMaterialId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.MaterialGroups.AsNoTracking() on c.MaterialGroupId equals d.Id
                             join e in db.Manufactures.AsNoTracking() on c.ManufactureId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Units.AsNoTracking() on c.UnitId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             orderby c.Code
                             select new SimilarMaterialConfigModel
                             {
                                 Id = a.Id,
                                 MaterialId = a.MaterialId,
                                 SimilarMaterialId = a.SimilarMaterialId,
                                 SimilarMaterialName = b.Name,
                                 MaterialGroupName = d.Name,
                                 Name = c.Name,
                                 Code = c.Code,
                                 ManufactureName = e.Name,
                                 UnitName = f.Name,
                                 Parameter = a.Parameter,
                                 Note = c.Note,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.SimilarMaterialId))
            {
                dataQuery = dataQuery.Where(u => u.SimilarMaterialId.Equals(model.SimilarMaterialId));
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.Contains(model.Name));
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.Contains(model.Code));
            }

            var listModel = dataQuery.ToList();
            if (listModel.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/SimilarMaterialConfig.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.SimilarMaterialName,
                    a.Name,
                    a.Code,
                    a.ManufactureName,
                    a.UnitName,
                    a.Note,
                    a.Parameter
                });


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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách vật tư tương tự" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách vật tư tương tự" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }
    }
}
