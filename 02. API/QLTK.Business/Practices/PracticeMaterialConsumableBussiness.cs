using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Materials;
using NTS.Model.PracticeMaterial;
using NTS.Model.PracticeMaterialConsumable;
using NTS.Model.Repositories;
using QLTK.Business.Materials;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.PracticeMaterialConsumable
{
    public class PracticeMaterialConsumableBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        // get vật tư tương tự theo practice 
        public SearchResultModel<PracticeMaterialConsumableModel> SearchPracticeMaterialConsumable(PracticeMaterialConsumableSearchModel modelSearch)
        {
            SearchResultModel<PracticeMaterialConsumableModel> searchResult = new SearchResultModel<PracticeMaterialConsumableModel>();
            var dataQuery = (from a in db.PracticeMaterialConsumables.AsNoTracking()
                             join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.MaterialGroups.AsNoTracking() on c.MaterialGroupId equals d.Id
                             join f in db.Units.AsNoTracking() on c.UnitId equals f.Id
                             join e in db.Manufactures.AsNoTracking() on c.ManufactureId equals e.Id
                             where a.PracticeId.Equals(modelSearch.PracticeId)
                             orderby c.Code
                             select new
                             {
                                 Id = a.Id,
                                 MaterialId = a.MaterialId,
                                 Code = c.Code,
                                 PracticeId = a.PracticeId,
                                 UnitName = f.Name,
                                 Name = c.Name,
                                 MaterialGroupName = d.Name,
                                 MaterialGroupCode = d.Code,
                                 Pricing = c.Pricing,
                                 ManufactureName = e.Name,
                                 Quantity = a.Quantity,
                                 Leadtime = c.DeliveryDays,
                                 c.LastBuyDate,
                                 c.InputPriceDate,
                                 c.PriceHistory
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = new List<PracticeMaterialConsumableModel>();

            PracticeMaterialConsumableModel practiceMaterialConsumableModel;
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var item in dataQuery.ToList())
            {
                practiceMaterialConsumableModel = new PracticeMaterialConsumableModel()
                {
                    Id = item.Id,
                    MaterialId = item.MaterialId,
                    PracticeId = item.PracticeId,
                    MaterialGroupName = item.MaterialGroupName,
                    MaterialGroupCode = item.MaterialGroupCode,
                    Name = item.Name,
                    Code = item.Code,
                    ManufactureName = item.ManufactureName,
                    Quantity = item.Quantity,
                    Pricing = item.Pricing,
                    Leadtime = item.Leadtime,
                    UnitName = item.UnitName
                };

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        practiceMaterialConsumableModel.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        practiceMaterialConsumableModel.Pricing = 0;
                    }
                }
                searchResult.ListResult.Add(practiceMaterialConsumableModel);
            }

            return searchResult;
        }
        //Chọn vật tư
        public SearchResultModel<MaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultModel<MaterialModel> searchResult = new SearchResultModel<MaterialModel>();
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
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
                                 Note = a.Note,
                                 Leadtime = a.DeliveryDays
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
            searchResult.ListResult = dataQuery.ToList();
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var item in searchResult.ListResult)
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

        /// <summary>
        ///  Khi lưu 
        /// </summary>
        /// <param name="model"></param>
        public void AddPracticeMaterialConsumable(PracticeMaterialConsumableModel model)
        {
            var prac = db.Practices.Where(o => o.Id.Equals(model.PracticeId)).FirstOrDefault();
            using (var trans = db.Database.BeginTransaction())
            {
                var practices = db.PracticeMaterialConsumables.Where(u => u.PracticeId.Equals(model.PracticeId)).ToList();
                if (practices.Count > 0)
                {
                    db.PracticeMaterialConsumables.RemoveRange(practices);
                }
                try
                {
                    prac.MaterialConsumableExist = false;
                    if (model.listSelect != null)
                    {
                        foreach (var item in model.listSelect)
                        {
                            if (item.MaterialId != null)
                            {
                                item.Id = item.MaterialId;
                            }

                            NTS.Model.Repositories.PracticeMaterialConsumable practiceMaterialConsumable = new NTS.Model.Repositories.PracticeMaterialConsumable
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = item.Id,
                                PracticeId = model.PracticeId,
                                Quantity = item.Quantity,
                                Price = item.Pricing,
                                Leadtime = item.Leadtime,
                                TotalPrice = Convert.ToInt32(item.Quantity * item.Pricing),
                            };
                            db.PracticeMaterialConsumables.Add(practiceMaterialConsumable);
                        }

                        if (model.listSelect.Count > 0)
                        {
                            prac.MaterialConsumableExist = true;
                        }
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Practice, model.PracticeId, string.Empty, "Vật tư tiêu hao");

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


        public string ExportExcelPracticeMaterialConsumable(PracticeMaterialConsumableSearchModel modelSearch)
        {
            List<PracticeMaterialConsumableModel> listPracticeConsumable = SearchPracticeMaterialConsumable(modelSearch).ListResult;
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/MaterialConsumable.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];
                var total = listPracticeConsumable.Count;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport = listPracticeConsumable.Select((o, i) => new
                {
                    Index = i + 1,
                    o.Name,
                    o.Code,
                    o.MaterialGroupName,
                    o.UnitName,
                    o.Pricing,
                    o.Quantity,
                    Amount = o.Pricing * o.Quantity,
                    o.ManufactureName,
                    o.Leadtime
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
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 10].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách Vật tư tiêu hao" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client

                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách Vật tư tiêu hao" + ".xls";
                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }


        /// <summary>
        /// Import vật tư
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<PracticeMaterialConsumableModel> ImportMaterialConsumable(string userId, HttpPostedFile file)
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

            PracticeMaterialConsumableModel practiceMaterialConsumableModel = new PracticeMaterialConsumableModel();
            List<PracticeMaterialConsumableModel> materials = new List<PracticeMaterialConsumableModel>();
            string materialCode;
            int quantity = 0;
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            List<int> codeErrorIndex = new List<int>();
            List<int> quantityErrorIndex = new List<int>();
            for (int i = 3; i < rowCount; i++)
            {
                if (!string.IsNullOrEmpty(sheet[i, 1].Value) && !string.IsNullOrEmpty(sheet[i, 2].Value) && !string.IsNullOrEmpty(sheet[i, 3].Value))
                {
                    materialCode = sheet[i, 2].Value;

                    try
                    {
                        quantity = int.Parse(sheet[i, 3].Value);
                    }
                    catch
                    {
                        quantityErrorIndex.Add(i);
                        continue;
                    }

                    var material = (from m in db.Materials.AsNoTracking()
                                    join mt in db.Manufactures.AsNoTracking() on m.ManufactureId equals mt.Id
                                    join g in db.MaterialGroups.AsNoTracking() on m.MaterialGroupId equals g.Id
                                    join u in db.Units.AsNoTracking() on m.UnitId equals u.Id
                                    where m.Code.ToLower().Equals(materialCode.ToLower())
                                    select new
                                    {
                                        m.Code,
                                        m.Name,
                                        MaterialGroupCode = g.Code,
                                        ManufactureName = mt.Name,
                                        m.Pricing,
                                        m.LastBuyDate,
                                        m.InputPriceDate,
                                        m.PriceHistory,
                                        m.DeliveryDays,
                                        m.Id,
                                       UnitName = u.Name
                                    }).FirstOrDefault();

                    if (material == null)
                    {
                        codeErrorIndex.Add(i);
                        continue;
                    }

                    practiceMaterialConsumableModel = new PracticeMaterialConsumableModel()
                    {
                        Code = material.Code,
                        Name = material.Name,
                        ManufactureName = material.ManufactureName,
                        MaterialGroupCode = material.MaterialGroupCode,
                        Leadtime = material.DeliveryDays,
                        Pricing = material.Pricing,
                        Quantity = quantity,
                        MaterialId = material.Id,
                        UnitName = material.UnitName
                    };

                    if (material.LastBuyDate.HasValue)
                    {
                        TimeSpan timeSpan = DateTime.Now.Subtract(material.LastBuyDate.Value);
                        if (timeSpan.Days <= day)
                        {
                            practiceMaterialConsumableModel.Pricing = material.PriceHistory;
                        }
                        else if (!material.InputPriceDate.HasValue || material.InputPriceDate.Value.Date < material.LastBuyDate.Value.Date)
                        {
                            practiceMaterialConsumableModel.Pricing = 0;
                        }
                    }

                    materials.Add(practiceMaterialConsumableModel);
                }
                else
                {
                    break;
                }
            }

            if (codeErrorIndex.Count > 0)
            {
                throw NTSException.CreateInstance("Mã vật tư dòng <" + string.Join(", ", codeErrorIndex) + "> không đúng!");
            }

            if (quantityErrorIndex.Count > 0)
            {
                throw NTSException.CreateInstance("Số lượng dòng <" + string.Join(", ", quantityErrorIndex) + "> không đúng!");
            }

            return materials;
        }
    }
}
