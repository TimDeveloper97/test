using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.MasterEmployee;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.MasterEmployee
{
    public class MasterEmployeeBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<MasterEmployeeModel> SearchMasterEmployee(MasterEmployeeModel model)
        {

            SearchResultModel<MasterEmployeeModel> searchResult = new SearchResultModel<MasterEmployeeModel>();

            var dataQuery = (from a in db.Employees.AsNoTracking()
                             join b in db.EmployeeSkills.AsNoTracking() on a.Id equals b.EmployeeId into ab
                             from ba in ab.DefaultIfEmpty()
                             join c in db.WorkSkills.AsNoTracking() on ba.WorkSkillId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join d in db.CourseSkills.AsNoTracking() on ca.Id equals d.WorkSkillId into ad
                             from da in ad.DefaultIfEmpty()
                             join e in db.Courses.AsNoTracking() on da.CourseId equals e.Id into ae
                             from ea in ae.DefaultIfEmpty()
                             select new MasterEmployeeModel
                             {
                                 EmployeeName = a.Name,
                                 EmployeeCode = a.Code,
                                 WorkSkillName = ca.Name,
                                 Mark = ba == null ? "": ba.Mark.ToString(),
                                 Grade = ba == null ? "" : ba.Grade.ToString(),
                                 FutureGoals = ba.Grade < ba.Mark ? ca.Name : ba.Grade >= ba.Mark ? "":"",
                                 CouseCode = ea.Code,
                                 CouseNameOld = ba.Grade < ba.Mark ? ea.Name : ba.Grade >= ba.Mark ? "" : "",
                                 CouseNameNew = ba.Grade >= ba.Mark? ea.Name : ba.Grade < ba.Mark ? "" : "",
                                 Note = ea.Description,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.EmployeeName))
            {
                dataQuery = dataQuery.Where(r => r.EmployeeName.ToUpper().Contains(model.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.EmployeeCode))
            {
                dataQuery = dataQuery.Where(r => r.EmployeeCode.ToUpper().Contains(model.EmployeeCode.ToUpper()));
            }
            searchResult.TotalItem = dataQuery.Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public string ExportExcel(MasterEmployeeModel model)
        {

            var dataQuery = (from a in db.Employees.AsNoTracking()
                             join b in db.EmployeeSkills.AsNoTracking() on a.Id equals b.EmployeeId into ab
                             from ba in ab.DefaultIfEmpty()
                             join c in db.WorkSkills.AsNoTracking() on ba.WorkSkillId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join d in db.CourseSkills.AsNoTracking() on ca.Id equals d.WorkSkillId into ad
                             from da in ad.DefaultIfEmpty()
                             join e in db.Courses.AsNoTracking() on da.CourseId equals e.Id into ae
                             from ea in ae.DefaultIfEmpty()
                             select new MasterEmployeeModel
                             {
                                 EmployeeName = a.Name,
                                 EmployeeCode = a.Code,
                                 WorkSkillName = ca.Name,
                                 Mark = ba == null ? "" : ba.Mark.ToString(),
                                 Grade = ba == null ? "" : ba.Grade.ToString(),
                                 FutureGoals = ba.Grade < ba.Mark ? ca.Name : ba.Grade >= ba.Mark ? "" : "",
                                 CouseCode = ea.Code,
                                 CouseNameOld = ba.Grade < ba.Mark ? ea.Name : ba.Grade >= ba.Mark ? "" : "",
                                 CouseNameNew = ba.Grade >= ba.Mark ? ea.Name : ba.Grade < ba.Mark ? "" : "",
                                 Note = ea.Description,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.EmployeeName))
            {
                dataQuery = dataQuery.Where(r => r.EmployeeName.ToUpper().Contains(model.EmployeeName.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.EmployeeCode))
            {
                dataQuery = dataQuery.Where(r => r.EmployeeCode.ToUpper().Contains(model.EmployeeCode.ToUpper()));
            }
            List<MasterEmployeeModel> listModel = dataQuery.ToList();
            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/EmployeeCourse.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.OrderBy(i => i.EmployeeName).Select((a, i) => new
                {
                    Index = i + 1,
                    a.EmployeeName,
                    a.EmployeeCode,
                    a.WorkSkillName,
                    a.Mark,
                    a.Grade,
                    a.FutureGoals,
                    a.CouseCode,
                    a.CouseNameOld,
                    a.CouseNameNew,
                    a.Note,
                });


                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 11].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 11].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Tổng hợp dữ liệu nhân sự" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Tổng hợp dữ liệu nhân sự" + ".xls";

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
