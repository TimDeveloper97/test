using NTS.Common;
using NTS.Model;
using NTS.Model.ReportStatusModule;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.ReportStatusModule
{
    public class ReportStatusModuleBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public ReportStatusModuleModel searchModule(ReportStatusModuleSearchModel searchModel)
        {
            ReportStatusModuleModel model = new ReportStatusModuleModel();

            var data = (from a in db.Modules.AsNoTracking()
                        join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ModuleId
                        join c in db.Projects.AsNoTracking() on b.ProjectId equals c.Id
                        orderby a.Code
                        select new
                        {
                            Id = a.Id,
                            ModuleCode = a.Code,
                            ModuleName = a.Name,
                            ModuleId = a.Id,
                            ProjectId = c.Id,
                            ProjectCode = c.Code,
                            ProjectName = c.Name,
                            Quatity = b.RealQuantity,

                            Status = a.Status,

                            CreateDate = a.CreateDate
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ModuleCode))
            {
                data = data.Where(a => a.ModuleCode.ToLower().Contains(searchModel.ModuleCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.ModuleName))
            {
                data = data.Where(a => a.ModuleName.ToLower().Contains(searchModel.ModuleName.ToLower()));
            }

            if (searchModel.DateFrom.HasValue)
            {
                data = data.Where(a => a.CreateDate >= searchModel.DateFrom);
            }

            if (searchModel.DateTo.HasValue)
            {
                data = data.Where(a => a.CreateDate <= searchModel.DateTo);
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectId))
            {
                data = data.Where(a => a.ProjectId.Equals(searchModel.ProjectId));
            }

            var listRs = (from t in data
                          group t by new { t.ModuleId, t.ModuleCode, t.ModuleName, t.ProjectCode, t.ProjectName, t.Status } into x
                          select new ReportStatusModuleModel
                          {
                              ModuleId = x.Key.ModuleId,
                              ModuleCode = x.Key.ModuleCode,
                              ModuleName = x.Key.ModuleName,
                              ProjectCode = x.Key.ProjectCode,
                              ProjectName = x.Key.ProjectName,
                              TotalModuleInProject = x.Sum(y => y.Quatity),
                              TotalModule = x.Count(),
                              Status = x.Key.Status,
                          }).AsQueryable();

            var listExcel = (from t in data
                             group t by new { t.ModuleId, t.ModuleCode, t.ModuleName, t.Status } into x
                             select new ReportStatusModuleModel
                             {
                                 ModuleId = x.Key.ModuleId,
                                 ModuleCode = x.Key.ModuleCode,
                                 ModuleName = x.Key.ModuleName,
                                 TotalModuleInProject = x.Sum(y => y.Quatity),
                                 TotalModule = x.Count(),
                                 Status = x.Key.Status,
                             }).AsQueryable();


            model.TotalModule = listRs.Count();

            model.ListModule = listExcel.OrderBy(i => i.ModuleCode).ToList();

            if (searchModel.IsExcel)
            {
                model.ListModuleUse = listRs.OrderBy(i => i.ModuleCode).ToList();
            }
            else
            {
                model.ListModuleUse = SQLHelpper.OrderBy(listRs, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            }
            return model;
        }


        public string ExportExcel(ReportStatusModuleSearchModel searchModel)
        {
            var data = searchModule(searchModel);
            List<ReportStatusModuleModel> listModel = data.ListModule;

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportStatusModule.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.ModuleName,
                    a.ModuleCode,
                    a.TotalModule,
                    a.TotalModuleInProject,
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng module" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng module" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        public string ExportExcelModule(ReportStatusModuleSearchModel searchModel)
        {
            searchModel.IsExcel = true;
            var data = searchModule(searchModel);
            List<ReportStatusModuleModel> listModel = data.ListModuleUse;

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportStatusModuleSearch.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    Index = i + 1,
                    a.ModuleCode,
                    a.ModuleName,
                    a.ProjectCode,
                    a.ProjectName,
                    a.TotalModule,
                    a.TotalModuleInProject
                });


                if (listExport.Count() > 1)
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng module tìm kiếm" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng module tìm kiếm" + ".xls";

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
