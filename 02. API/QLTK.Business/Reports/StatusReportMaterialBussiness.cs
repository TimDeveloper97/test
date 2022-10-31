using NTS.Common;
using NTS.Model.ModuleMaterials;
using NTS.Model.QLTKMODULE;
using NTS.Model.ReportStatusMaterial;
using NTS.Model.Repositories;
using QLTK.Business.Materials;
using QLTK.Business.ReportStatusModule;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using NTS.Common.Resource;

namespace QLTK.Business.StatusReportMaterial
{
    public class StatusReportMaterialBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public ReportStatusMaterialSearchResultModel<ReportStatusMaterialModuleModel> GetStatusReportMaterial(ReportStatusMaterialSearchModel model)
        {
            var material = db.Materials.FirstOrDefault(i => i.Code.Equals(model.Code));
            if (material == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Material);
            }

            var moduleInProjects = (from d in db.ProjectProducts.AsNoTracking()
                                    group d by d.ModuleId into g
                                    select new
                                    {
                                        ModuleId = g.Key,
                                        Quantity = g.Sum(s => s.RealQuantity)
                                    }).ToList();

            var modules = (from a in db.ModuleMaterials.AsEnumerable()
                           join m in moduleInProjects on a.ModuleId equals m.ModuleId into am
                           from amn in am.DefaultIfEmpty()
                           join b in db.Modules.AsEnumerable() on a.ModuleId equals b.Id
                           join c in db.ModuleGroups.AsEnumerable() on b.ModuleGroupId equals c.Id
                           group a by new { b.Name, b.Code, b.Status, b.CreateDate, GroupCode = c.Code, a.ModuleId, Quantity = amn != null ? amn.Quantity : 0 } into g
                           select new
                           {
                               ModuleCode = g.Key.Code,
                               ModuleName = g.Key.Name,
                               g.Key.GroupCode,
                               g.Key.Status,
                               g.Key.ModuleId,
                               QuantityInProject = g.Key.Quantity,
                               g.Key.CreateDate
                           }).ToList();

            if (model.DateFrom.HasValue)
            {
                modules = modules.Where(a => a.CreateDate >= model.DateFrom).ToList();
            }

            if (model.DateTo.HasValue)
            {
                modules = modules.Where(a => a.CreateDate <= model.DateTo).ToList();
            }

            if (!string.IsNullOrEmpty(model.ModuleId))
            {
                modules = modules.Where(a => a.ModuleId.Equals(model.ModuleId)).ToList();
            }

            List<ReportStatusMaterialModuleMaterialModel> moduleMaterials = new List<ReportStatusMaterialModuleMaterialModel>();
            List<ReportStatusMaterialModuleMaterialModel> moduleMaterialSearch;

            var moduleMaterialDB = (from a in db.ModuleMaterials.AsEnumerable()
                                    join m in modules on a.ModuleId equals m.ModuleId
                                    select a).ToList();

            foreach (var module in modules)
            {
                moduleMaterialSearch = (from a in moduleMaterialDB
                                        where a.ModuleId.Equals(module.ModuleId)
                                        select new ReportStatusMaterialModuleMaterialModel
                                        {
                                            ModuleId = a.ModuleId,
                                            MaterialId = a.MaterialId,
                                            Specification = a.Specification,
                                            Quantity = a.Quantity,
                                            ReadQuantity = a.Quantity,
                                            ReadQuantityInProject = a.Quantity * module.QuantityInProject,
                                            Index = a.Index,
                                        }).ToList();


                UpdateMaterialQuantity(moduleMaterialSearch, module.QuantityInProject);

                moduleMaterials.AddRange(moduleMaterialSearch);
            }

            var materials = (from a in moduleMaterials
                             where a.MaterialId.Equals(material.Id)
                             join b in modules on a.ModuleId equals b.ModuleId
                             group a by new { b.ModuleCode, b.ModuleName, b.GroupCode, b.Status, b.ModuleId } into g
                             select new ReportStatusMaterialModuleModel
                             {
                                 Name = g.Key.ModuleName,
                                 Code = g.Key.ModuleCode,
                                 Status = g.Key.Status,
                                 ModuleGroupName = g.Key.GroupCode,
                                 Quantity = g.Sum(s => s.ReadQuantity),
                                 RealQuantityInProject = g.Sum(s => s.ReadQuantityInProject),
                                 ModuleId = g.Key.ModuleId
                             }).ToList();

            var totalModule = materials.Count();
            var totalMaterial = materials.Sum(x => x.RealQuantityInProject);

            ReportStatusMaterialSearchResultModel<ReportStatusMaterialModuleModel> result = new ReportStatusMaterialSearchResultModel<ReportStatusMaterialModuleModel>();
            result.TotalModule = totalModule;
            result.TotalMaterial = totalMaterial;
            result.ListResult = materials.OrderByDescending(i => i.Quantity).ToList();

            return result;
        }

        public string Excel(ReportStatusMaterialSearchModel model)
        {
            var result = GetStatusReportMaterial(model);

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportStatusMaterial.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = result.ListResult.Count;

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = result.ListResult.Select((a, i) => new
            {
                Index = i + 1,
                a.Code,
                a.Name,
                a.ModuleGroupName,
                a.Quantity,
                ViewStatus = a.Status == 1 ? "Chỉ sử dụng một lần" : a.Status == 2 ? "Module chuẩn" : a.Status == 3 ? "Module ngừng sử dụng" : "",
            });


            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders.Color = ExcelKnownColors.Black;
            //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].CellStyle.WrapText = true;


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng vật tư" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng vật tư" + ".xls";

            return resultPathClient;
        }

        /// <summary>
        /// Tân xuất sử dụng vật tư
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public string ReportModuleMaterial(ReportStatusMaterialSearchModel searchModel)
        {
            // Danh sách dự án
            var projectSearch = (from p in db.Projects.AsNoTracking()
                                 select new
                                 {
                                     p.Id,
                                     p.KickOffDate
                                 }).AsQueryable();

            string dateFrom = string.Empty;
            string dateTo = string.Empty;
            if (searchModel.DateFrom.HasValue)
            {
                projectSearch = projectSearch.Where(r => r.KickOffDate >= searchModel.DateFrom);
                dateFrom = searchModel.DateFrom.Value.ToStringDDMMYY();
            }

            if (searchModel.DateTo.HasValue)
            {
                searchModel.DateTo = searchModel.DateTo.Value.ToEndDate();
                projectSearch = projectSearch.Where(r => r.KickOffDate <= searchModel.DateTo);
                dateTo = searchModel.DateTo.Value.ToStringDDMMYY();
            }

            var projects = projectSearch.ToList();

            var projectMaterialSub = (from d in db.ProjectGeneralDesigns.AsEnumerable()
                                      join p in projects on d.ProjectId equals p.Id
                                      join m in db.ProjectGeneralDesignMaterials.AsEnumerable() on d.Id equals m.ProjectGeneralDesignId
                                      group m by m.MaterialId into g
                                      select new
                                      {
                                          MaterialId = g.Key,
                                          Quantity = g.Sum(s => s.Quantity)
                                      }).ToList();

            // Danh sách module theo dự án và THTK
            var moduleInProject = (from p in projects
                                   join pp in db.ProjectProducts.AsNoTracking() on p.Id equals pp.ProjectId
                                   join d in db.ProjectGeneralDesignModules.AsNoTracking() on pp.Id equals d.ProjectProductId
                                   select new
                                   {
                                       pp.ModuleId,
                                       d.RealQuantity
                                   }).ToList();

            var modules = (from m in db.Modules.AsEnumerable()
                           join p in moduleInProject on m.Id equals p.ModuleId into mp
                           from mpn in mp.DefaultIfEmpty()
                           group new { m, mpn } by m.Id into g
                           select new
                           {
                               ModuleId = g.Key,
                               QuantityInProject = g.Sum(s => s.mpn != null ? s.mpn.RealQuantity : 0)
                           }).ToList();

            // Lấy danh sách VT trong module
            var moduleMaterialDB = db.ModuleMaterials.AsNoTracking().ToList();
            List<ReportStatusMaterialModuleMaterialModel> moduleMaterials = new List<ReportStatusMaterialModuleMaterialModel>();
            List<ReportStatusMaterialModuleMaterialModel> moduleMaterialSearch;

            foreach (var module in modules)
            {
                moduleMaterialSearch = (from a in moduleMaterialDB
                                        where a.ModuleId.Equals(module.ModuleId)
                                        select new ReportStatusMaterialModuleMaterialModel
                                        {
                                            ModuleId = a.ModuleId,
                                            MaterialId = a.MaterialId,
                                            Specification = a.Specification,
                                            Quantity = a.Quantity,
                                            ReadQuantity = a.Quantity,
                                            ReadQuantityInProject = a.Quantity * module.QuantityInProject,
                                            Index = a.Index,
                                        }).ToList();

                //if (moduleMaterialSearch.Count() > 0)
                //{
                //    int maxLen = moduleMaterialSearch.Select(s => s.Index.Length).Max();

                //    Func<string, char> PaddingChar = s => char.IsDigit(s[0]) ? ' ' : char.MaxValue;

                //    moduleMaterialSearch = moduleMaterialSearch
                //               .Select(s =>
                //                   new
                //                   {
                //                       OrgStr = s,
                //                       SortStr = Regex.Replace(s.Index, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, PaddingChar(m.Value)))
                //                   })
                //               .OrderBy(x => x.SortStr)
                //               .Select(x => x.OrgStr).ToList();
                //}

                UpdateMaterialQuantity(moduleMaterialSearch, module.QuantityInProject);

                moduleMaterials.AddRange(moduleMaterialSearch);
            }

            // Danh sách vật tư
            var materials = (from a in db.Materials.AsEnumerable()
                             join b in moduleMaterials on a.Id equals b.MaterialId
                             group b by new { a.Id, a.Name, a.Code, a.LastBuyDate, a.Pricing, a.PriceHistory, a.InputPriceDate } into x
                             select new ModuleMaterialModel
                             {
                                 MaterialId = x.Key.Id,
                                 MaterialCode = x.Key.Code,
                                 MaterialName = x.Key.Name,
                                 LastBuyDate = x.Key.LastBuyDate,
                                 Price = x.Key.Pricing,
                                 PriceHistory = x.Key.PriceHistory,
                                 InputPriceDate = x.Key.InputPriceDate,
                                 Total = x.Sum(s => s.ReadQuantity),
                                 TotalModule = x.Where(r => !string.IsNullOrEmpty(r.ModuleId)).GroupBy(g => g.ModuleId).Count(),
                                 TotalInProject = x.Sum(s => s.ReadQuantityInProject)
                             }).ToList();

            materials = (from a in materials
                         join b in projectMaterialSub on a.MaterialId equals b.MaterialId into ab
                         from abn in ab.DefaultIfEmpty()
                         select new ModuleMaterialModel
                         {
                             MaterialId = a.MaterialId,
                             MaterialCode = a.MaterialCode,
                             MaterialName = a.MaterialName,
                             LastBuyDate = a.LastBuyDate,
                             Price = a.Price,
                             PriceHistory = a.PriceHistory,
                             InputPriceDate = a.InputPriceDate,
                             Total = a.Total,
                             TotalModule = a.TotalModule,
                             TotalInProject = a.TotalInProject + (abn != null ? abn.Quantity : 0)
                         }).ToList();

            MaterialBusiness materialBusiness = new MaterialBusiness();
            int day = materialBusiness.GetConfigMaterialLastByDate();
            foreach (var item in materials)
            {
                if (item.LastBuyDate.HasValue)
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(item.LastBuyDate.Value);

                    if (timeSpan.Days <= day)
                    {
                        item.Price = item.PriceHistory;
                    }
                    else if (!item.InputPriceDate.HasValue || item.InputPriceDate.Value.Date < item.LastBuyDate.Value.Date)
                    {
                        item.Price = 0;
                    }
                }
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportModuleMaterial_TemPlate.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = materials.Count;

                IRange iRangeData = sheet.FindFirst("<dateFrom>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<dateFrom>", dateFrom);

                iRangeData = sheet.FindFirst("<dateTo>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<dateTo>", dateTo);

                iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

                var listExport = materials.OrderByDescending(o => o.TotalInProject).Select((a, i) => new
                {
                    Index = i + 1,
                    a.MaterialCode,
                    a.MaterialName,
                    a.TotalModule,
                    a.Total,
                    a.TotalInProject,
                    a.Price,
                    a.LastBuyDate,
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

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng vật tư_Tần suất sử dụng trong module" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng vật tư_Tần suất sử dụng trong module" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }

        }

        public void UpdateMaterialQuantity(List<ReportStatusMaterialModuleMaterialModel> materials, decimal quantityInProject)
        {
            var parents = materials.Where(r => r.Index.IndexOf('.') == -1).ToList();

            foreach (var item in parents)
            {
                if (!Constants.Material_Specification_HAN.Equals(item.Specification))
                {
                    UpdateMaterialQuantityChild(item, materials, quantityInProject);
                }
            }
        }

        public void UpdateMaterialQuantityChild(ReportStatusMaterialModuleMaterialModel parent, List<ReportStatusMaterialModuleMaterialModel> materials, decimal quantityInProject)
        {
            string parentIndex = parent.Index + ".";
            var childs = materials.Where(r => r.Index.StartsWith(parentIndex) && r.Index.Substring(parentIndex.Length, r.Index.Length - parentIndex.Length).IndexOf('.') == -1).ToList();

            foreach (var item in childs)
            {
                item.ReadQuantity = item.Quantity * GetParentQuantity(materials, item.Index);
                item.ReadQuantityInProject = item.ReadQuantity * quantityInProject;

                if (!Constants.Material_Specification_HAN.Equals(item.Specification))
                {
                    UpdateMaterialQuantityChild(item, materials, quantityInProject);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="indexChild"></param>
        /// <returns></returns>
        public decimal GetParentQuantity(List<ReportStatusMaterialModuleMaterialModel> materials, string indexChild)
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


        public string ModuleMaterialCheck3D()
        {
            List<ModuleMaterialModel> listModuleMaterials = new List<ModuleMaterialModel>();
            var modeleMaterial = (from a in db.ModuleMaterials.AsNoTracking()
                                      //group a by a.MaterialId into b
                                  select new ModuleMaterialModel
                                  {
                                      MaterialId = a.MaterialId,
                                      MaterialCode = a.MaterialCode,
                                      MaterialName = a.MaterialName
                                  }).ToList();

            List<ModuleMaterialModel> listRs = new List<ModuleMaterialModel>();
            var lstRs = modeleMaterial.GroupBy(t => new { t.MaterialId, t.MaterialCode, t.MaterialName }).ToList();
            foreach (var item in lstRs)
            {
                ModuleMaterialModel rs = new ModuleMaterialModel();
                rs.MaterialId = item.Key.MaterialId;
                rs.MaterialCode = item.Key.MaterialCode;
                rs.MaterialName = item.Key.MaterialName;
                listRs.Add(rs);
            }

            foreach (var item in listRs)
            {
                if (db.MaterialDesign3D.AsNoTracking().Where(i => i.MaterialId.Equals(item.MaterialId)).ToList().Count == 0 && !item.MaterialCode.Contains("TPA"))
                {
                    listModuleMaterials.Add(item);
                }
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportModuleMaterial_TemPlateCheck3D.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModuleMaterials.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);

                var listExport = listModuleMaterials.OrderBy(e => e.MaterialCode).Select((a, i) => new
                {
                    Index = i + 1,
                    a.MaterialCode,
                    a.MaterialName
                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 3].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng vật tư_Kiểm tra thư viện 3D" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng vật tư_Kiểm tra thư viện 3D" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(null, ex);
            }
        }
    }
}
