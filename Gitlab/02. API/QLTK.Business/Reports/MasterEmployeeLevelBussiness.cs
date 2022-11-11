using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.MasterEmployeeLevel;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.MasterEmployeeLevel
{
    public class MasterEmployeeLevelBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<MasterEmployeeLevelModel> SearchEmployeeLevel(MasterEmployeeLevelModel model)
        {

            SearchResultModel<MasterEmployeeLevelModel> searchResult = new SearchResultModel<MasterEmployeeLevelModel>();

            var dataQuery = (from a in db.WorkTypes.AsNoTracking()
                        join b in db.WorkTypeSkills.AsNoTracking() on a.Id equals b.WorkTypeId into ab
                        from ba in ab.DefaultIfEmpty()
                        join c in db.WorkSkills.AsNoTracking() on ba.WorkSkillId equals c.Id into ac
                        from ca in ac.DefaultIfEmpty()
                        join d in db.CourseSkills.AsNoTracking() on ca.Id equals d.WorkSkillId into ad
                        from da in ad.DefaultIfEmpty()
                        join e in db.Courses.AsNoTracking() on da.CourseId equals e.Id into ae
                        from ea in ae.DefaultIfEmpty()
                        select new MasterEmployeeLevelModel
                        {
                            WorkTypeName = a.Name,
                            WorkSkillName = ca.Name,
                            CouseCode = ea.Code,
                            CouseName = ea.Name,
                            DeviceForCourse = ea.DeviceForCourse,
                        }).AsQueryable();
            if (!string.IsNullOrEmpty(model.CouseCode))
            {
                dataQuery = dataQuery.Where(r => r.CouseCode.ToUpper().Contains(model.CouseCode.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public string ExportExcel(MasterEmployeeLevelModel model)
        {

            var dataQuery = (from a in db.WorkTypes.AsNoTracking()
                             join b in db.WorkTypeSkills.AsNoTracking() on a.Id equals b.WorkTypeId into ab
                             from ba in ab.DefaultIfEmpty()
                             join c in db.WorkSkills.AsNoTracking() on ba.WorkSkillId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join d in db.CourseSkills.AsNoTracking() on ca.Id equals d.WorkSkillId into ad
                             from da in ad.DefaultIfEmpty()
                             join e in db.Courses.AsNoTracking() on da.CourseId equals e.Id into ae
                             from ea in ae.DefaultIfEmpty()
                             select new MasterEmployeeLevelModel
                             {
                                 WorkTypeName = a.Name,
                                 WorkSkillName = ca.Name,
                                 CouseCode = ea.Code,
                                 CouseName = ea.Name,
                                 DeviceForCourse = ea.DeviceForCourse,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.CouseCode))
            {
                dataQuery = dataQuery.Where(r => r.CouseCode.ToUpper().Contains(model.CouseCode.ToUpper()));
            }
            List<MasterEmployeeLevelModel> listModel = dataQuery.ToList();
            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/EmployeeLevel.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.OrderBy(i => i.WorkTypeName).Select((a, i) => new
                {
                    Index = i + 1,
                    a.WorkTypeName,
                    a.WorkSkillName,
                    a.CouseCode,
                    a.CouseName,
                    a.DeviceForCourse,
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Tổng hợp dữ liệu vị trí nhân sự" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Tổng hợp dữ liệu vị trí nhân sự" + ".xls";

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
