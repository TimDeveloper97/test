using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Materials;
using NTS.Model.PracticeSupMaterial;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using QLTK.Business.Materials;
using QLTK.Business.ModuleMaterials;
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

namespace QLTK.Business.PracticeSupMaterial
{
    public class PracticeSupMaterialBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly ModuleMaterialBusiness moduleMaterialBusiness = new ModuleMaterialBusiness();

        public SearchResultModel<PracticeSupMaterialModel> SearchPracticeSupMaterial(PracticeSupMaterialSearchModel modelSearch)
        {
            SearchResultModel<PracticeSupMaterialModel> searchResult = new SearchResultModel<PracticeSupMaterialModel>();
            var dataQuery = (from a in db.PracticeSubMaterials.AsNoTracking()
                             join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                             join c in db.Materials.AsNoTracking() on a.MaterialId equals c.Id
                             join d in db.MaterialGroups.AsNoTracking() on c.MaterialGroupId equals d.Id
                             join e in db.Manufactures.AsNoTracking() on c.ManufactureId equals e.Id
                             join u in db.Units.AsNoTracking() on c.UnitId equals u.Id
                             where a.PracticeId.Equals(modelSearch.PracticeId) && a.Type == Constants.PracticeSupMaterial_Type_Material
                             orderby c.Code
                             select new
                             {
                                 Id = c.Id,
                                 MaterialId = a.MaterialId,
                                 PracticeId = a.PracticeId,
                                 MaterialGroupName = d.Name,
                                 MaterialGroupCode = d.Code,
                                 Name = c.Name,
                                 Code = c.Code,
                                 ManufactureName = e.Name,
                                 Quantity = a.Quantity,
                                 Pricing = c.Pricing,
                                 Leadtime = c.DeliveryDays,
                                 c.LastBuyDate,
                                 c.InputPriceDate,
                                 c.PriceHistory,
                                 UnitName = u.Name,
                                 a.Type
                             }).ToList();

            searchResult.ListResult = new List<PracticeSupMaterialModel>();
            PracticeSupMaterialModel practiceSupMaterialModel;

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();

            foreach (var item in dataQuery)
            {
                practiceSupMaterialModel = new PracticeSupMaterialModel()
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
                    UnitName = item.UnitName,
                    Type = item.Type
                };

                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);
                    if (timeSpan.Days <= day)
                    {
                        practiceSupMaterialModel.Pricing = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        practiceSupMaterialModel.Pricing = 0;
                    }
                }

                searchResult.ListResult.Add(practiceSupMaterialModel);
            }

            var data = (from a in db.PracticeSubMaterials.AsNoTracking()
                        join b in db.Modules.AsNoTracking() on a.MaterialId equals b.Id
                        join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                        where a.PracticeId.Equals(modelSearch.PracticeId) && a.Type == Constants.PracticeSupMaterial_Type_Module
                        orderby b.Code
                        select new PracticeSupMaterialModel
                        {
                            Id = b.Id,
                            MaterialId = a.MaterialId,
                            PracticeId = a.PracticeId,
                            MaterialGroupCode = c.Code,
                            Name = b.Name,
                            Code = b.Code,
                            ManufactureName = Constants.Manufacture_TPA,
                            Quantity = a.Quantity,
                            Pricing = b.Pricing,
                            Leadtime = a.Leadtime.Value,
                            Type = a.Type
                        }).ToList();

            foreach (var item in data)
            {
                item.Pricing = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
            }

            searchResult.ListResult.AddRange(data);
            searchResult.ListResult = searchResult.ListResult.OrderBy(i => i.Code).ToList();
            searchResult.TotalItem = searchResult.ListResult.Count();
            return searchResult;
        }

        public SearchResultModel<PracticeSupMaterialModel> SearchMaterial(MaterialSearchModel modelSearch)
        {
            SearchResultModel<PracticeSupMaterialModel> searchResult = new SearchResultModel<PracticeSupMaterialModel>();
            var dataQuery = (from a in db.Materials.AsNoTracking()
                             join b in db.Manufactures.AsNoTracking() on a.ManufactureId equals b.Id
                             join c in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals c.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new PracticeSupMaterialModel
                             {
                                 Id = a.Id,
                                 MaterialId = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 ManufactureName = b.Name,
                                 MaterialGroupName = c.Name,
                                 Pricing = a.Pricing,
                                 LastBuyDate = a.LastBuyDate,
                                 InputPriceDate = a.InputPriceDate,
                                 PriceHistory = a.PriceHistory,
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
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();

            foreach (var item in listResult)
            {
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

        public SearchResultModel<PracticeSupMaterialModel> SearchModule(ModuleSearchModel model)
        {
            SearchResultModel<PracticeSupMaterialModel> searchResult = new SearchResultModel<PracticeSupMaterialModel>();

            var data = (from a in db.Modules.AsNoTracking()
                        join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                        where !model.ListIdSelect.Contains(a.Id)
                        orderby a.Code
                        select new PracticeSupMaterialModel
                        {
                            Id = a.Id,
                            MaterialId = a.Id,
                            Code = a.Code,
                            Name = a.Name,
                            ManufactureName = Constants.Manufacture_TPA,
                            MaterialGroupCode = b.Code,
                            Leadtime = a.Leadtime
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
            {
                data = data.Where(u => u.Name.ToUpper().Contains(model.Name.ToUpper()) || u.Code.ToUpper().Contains(model.Name.ToUpper()));
            }

            searchResult.ListResult = data.ToList();
            return searchResult;
        }

        public void AddPracticeSupMaterial(PracticeSupMaterialInfoModel model, string userId)
        {
            var prac = db.Practices.Where(o => o.Id.Equals(model.PracticeId)).FirstOrDefault();
            using (var trans = db.Database.BeginTransaction())
            {
                var practices = db.PracticeSubMaterials.Where(u => u.PracticeId.Equals(model.PracticeId)).ToList();
                if (practices.Count > 0)
                {
                    db.PracticeSubMaterials.RemoveRange(practices);
                }
                try
                {
                    if (model.Materials.Count > 0)
                    {
                        PracticeSubMaterial practiceSubMaterial;
                        foreach (var item in model.Materials)
                        {
                            practiceSubMaterial = new PracticeSubMaterial
                            {
                                Id = Guid.NewGuid().ToString(),
                                MaterialId = item.MaterialId,
                                PracticeId = model.PracticeId,
                                Quantity = item.Quantity,
                                Price = item.Pricing,
                                TotalPrice = item.Quantity * item.Pricing,
                                Leadtime = item.Leadtime,
                                Type = item.Type
                            };
                            db.PracticeSubMaterials.Add(practiceSubMaterial);
                        }
                        prac.SupMaterialExist = true;
                    }
                    else
                    {
                        prac.SupMaterialExist = false;
                    }

                    UserLogUtil.LogHistotyUpdateSub(db, userId, Constants.LOG_Practice, model.PracticeId, string.Empty, "Thiết bị phụ trợ");

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

        public string ExportExcelPracticeSupMaterial(PracticeSupMaterialSearchModel modelSearch)
        {
            List<PracticeSupMaterialModel> listPracticeSupMaterial = SearchPracticeSupMaterial(modelSearch).ListResult;

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/PracticeSupMaterial.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];
                var total = listPracticeSupMaterial.Count;
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var listExport = listPracticeSupMaterial.Select((o, i) => new
                {
                    Index = i + 1,
                    o.Name,
                    o.Code,
                    o.MaterialGroupName,
                    o.Pricing,
                    o.Quantity,
                    Amount = o.Pricing * o.Quantity,
                    o.ManufactureName,
                    o.Leadtime
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 9].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 9].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách Thiết bị phụ trợ" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client

                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách Thiết bị phụ trợ" + ".xls";
                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }

        }

        public List<MaterialModel> GetPriceModule(List<MaterialModel> listModule)
        {
            foreach (var item in listModule)
            {
                item.Pricing = moduleMaterialBusiness.GetPriceModuleByModuleId(item.Id, 0);
            }
            return listModule;
        }

        /// <summary>
        /// Import vật tư
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<PracticeSupMaterialModel> ImportSubMaterial(string userId, HttpPostedFile file)
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

            PracticeSupMaterialModel practiceSupMaterialModel = new PracticeSupMaterialModel();
            List<PracticeSupMaterialModel> materials = new List<PracticeSupMaterialModel>();
            string materialCode;
            int quantity = 0;
            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            List<int> codeErrorIndex = new List<int>();
            List<int> quantityErrorIndex = new List<int>();
            List<int> typeErrorIndex = new List<int>();
            int type = 0;
            for (int i = 3; i <= rowCount; i++)
            {
                if (!string.IsNullOrEmpty(sheet[i, 1].Value) && !string.IsNullOrEmpty(sheet[i, 2].Value) && !string.IsNullOrEmpty(sheet[i, 3].Value) && !string.IsNullOrEmpty(sheet[i, 4].Value))
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

                    if ("Vật tư".Equals(sheet[i, 4].Value))
                    {
                        type = Constants.PracticeSupMaterial_Type_Material;
                    }
                    else if ("Module".Equals(sheet[i, 4].Value))
                    {
                        type = Constants.PracticeSupMaterial_Type_Module;
                    }
                    else
                    {
                        typeErrorIndex.Add(i);
                        continue;
                    }

                    if (type == Constants.PracticeSupMaterial_Type_Material)
                    {
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
                                            m.Id
                                        }).FirstOrDefault();

                        if (material == null)
                        {
                            codeErrorIndex.Add(i);
                            continue;
                        }

                        practiceSupMaterialModel = new PracticeSupMaterialModel()
                        {
                            Code = material.Code,
                            Name = material.Name,
                            ManufactureName = material.ManufactureName,
                            MaterialGroupCode = material.MaterialGroupCode,
                            Leadtime = material.DeliveryDays,
                            Pricing = material.Pricing,
                            Quantity = quantity,
                            Type = type,
                            MaterialId = material.Id
                        };

                        if (material.LastBuyDate.HasValue)
                        {
                            TimeSpan timeSpan = DateTime.Now.Subtract(material.LastBuyDate.Value);
                            if (timeSpan.Days <= day)
                            {
                                practiceSupMaterialModel.Pricing = material.PriceHistory;
                            }
                            else if (!material.InputPriceDate.HasValue || material.InputPriceDate.Value.Date < material.LastBuyDate.Value.Date)
                            {
                                practiceSupMaterialModel.Pricing = 0;
                            }
                        }
                    }
                    else
                    {
                        var module = (from b in db.Modules.AsNoTracking()
                                      join c in db.ModuleGroups.AsNoTracking() on b.ModuleGroupId equals c.Id
                                      where b.Code.ToLower().Equals(materialCode.ToLower())
                                      select new
                                      {
                                          b.Id,
                                          MaterialGroupCode = c.Code,
                                          b.Name,
                                          b.Code,
                                          ManufactureName = Constants.Manufacture_TPA,
                                      }).FirstOrDefault();

                        if (module == null)
                        {
                            codeErrorIndex.Add(i);
                            continue;
                        }

                        practiceSupMaterialModel = new PracticeSupMaterialModel()
                        {
                            Code = module.Code,
                            Name = module.Name,
                            ManufactureName = module.ManufactureName,
                            MaterialGroupCode = module.MaterialGroupCode,
                            Quantity = quantity,
                            Type = type,
                            MaterialId = module.Id
                        };

                        practiceSupMaterialModel.Pricing = moduleMaterialBusiness.GetPriceModuleByModuleId(module.Id, 0);

                    }

                    materials.Add(practiceSupMaterialModel);
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

            if (typeErrorIndex.Count > 0)
            {
                throw NTSException.CreateInstance("Loại vật tư dòng <" + string.Join(", ", typeErrorIndex) + "> không đúng!");
            }

            return materials;
        }
    }
}
