using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.MasterLibrary;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.MasterLibrary
{
    public class MasterLibraryBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<MasterLibraryModel> SearchMasterLibrary(MasterLibraryModel model)
        {

            SearchResultModel<MasterLibraryModel> searchResult = new SearchResultModel<MasterLibraryModel>();

                var dataQuery = (from a in db.Subjects.AsNoTracking()
                                 join b in db.SubjectsClassRooms.AsNoTracking() on a.Id equals b.SubjectsId into ab
                                 from ba in ab.DefaultIfEmpty()
                                 join c in db.ClassRooms.AsNoTracking() on ba.ClassRoomId equals c.Id into ac
                                 from ca in ac.DefaultIfEmpty() 
                                 join d in db.ClassSkills.AsNoTracking() on ca.Id equals d.ClassRoomId into ad
                                 from da in ad.DefaultIfEmpty()
                                 join e in db.Skills.AsNoTracking() on da.SkillId equals e.Id into ae
                                 from ea in ae.DefaultIfEmpty()
                                 join f in db.PracticeSkills.AsNoTracking() on ea.Id equals f.SkillId into af
                                 from fa in af.DefaultIfEmpty()
                                 join g in db.Practices.AsNoTracking() on fa.PracticeId equals g.Id into ag
                                 from ga in ag.DefaultIfEmpty()
                                 join h in db.PracticInProducts.AsNoTracking() on ga.Id equals h.PracticeId into ah
                                 from ha in ah.DefaultIfEmpty()
                                 join k in db.Products.AsNoTracking() on ha.ProductId equals k.Id into ak
                                 from ka in ak.DefaultIfEmpty()
                                 join i in db.ModuleInPractices.AsNoTracking() on ga.Id equals i.PracticeId into gi
                                 from ig in gi.DefaultIfEmpty()
                                 join j in db.Modules.AsNoTracking() on ig.ModuleId equals j.Id into ij
                                 from ji in ij.DefaultIfEmpty()
                                 select new MasterLibraryModel
                                 {
                                     SubjectsName = a.Name,
                                     SubjectsCode = a.Code,
                                     SkillName = ea.Name,
                                     SkillCode = ea.Code,
                                     PracticeCode = ga.Code,
                                     PracticeName = ga.Name,
                                     ModuleCode = ji.Code,
                                     ModuleName = ji.Name,
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(model.SubjectsCode))
                {
                    dataQuery = dataQuery.Where(r => r.SubjectsCode.ToUpper().Contains(model.SubjectsCode.ToUpper()));
                }
                if (!string.IsNullOrEmpty(model.SkillCode))
                {
                    dataQuery = dataQuery.Where(r => r.SkillCode.ToUpper().Contains(model.SkillCode.ToUpper()));
                }
                if (!string.IsNullOrEmpty(model.PracticeCode))
                {
                    dataQuery = dataQuery.Where(r => r.PracticeCode.ToUpper().Contains(model.PracticeCode.ToUpper()));
                }
                if (!string.IsNullOrEmpty(model.ModuleCode))
                {
                    dataQuery = dataQuery.Where(r => r.ModuleCode.ToUpper().Contains(model.ModuleCode.ToUpper()));
                }
                searchResult.TotalItem = dataQuery.Count();
                var listResult = SQLHelpper.OrderBy(dataQuery, model.OrderBy, model.OrderType).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
                searchResult.ListResult = listResult;
                return searchResult;
        }

        public string ExportExcel(MasterLibraryModel model)
        {
            var dataQuery = (from a in db.Subjects.AsNoTracking()
                             join b in db.SubjectsClassRooms.AsNoTracking() on a.Id equals b.SubjectsId into ab
                             from ba in ab.DefaultIfEmpty()
                             join c in db.ClassRooms.AsNoTracking() on ba.ClassRoomId equals c.Id into ac
                             from ca in ac.DefaultIfEmpty()
                             join d in db.ClassSkills.AsNoTracking() on ca.Id equals d.ClassRoomId into ad
                             from da in ad.DefaultIfEmpty()
                             join e in db.Skills.AsNoTracking() on da.SkillId equals e.Id into ae
                             from ea in ae.DefaultIfEmpty()
                             join f in db.PracticeSkills.AsNoTracking() on ea.Id equals f.SkillId into af
                             from fa in af.DefaultIfEmpty()
                             join g in db.Practices.AsNoTracking() on fa.PracticeId equals g.Id into ag
                             from ga in ag.DefaultIfEmpty()
                             join h in db.PracticInProducts.AsNoTracking() on ga.Id equals h.PracticeId into ah
                             from ha in ah.DefaultIfEmpty()
                             join k in db.Products.AsNoTracking() on ha.ProductId equals k.Id into ak
                             from ka in ak.DefaultIfEmpty()
                             join i in db.ModuleInPractices.AsNoTracking() on ga.Id equals i.PracticeId into gi
                             from ig in gi.DefaultIfEmpty()
                             join j in db.Modules.AsNoTracking() on ig.ModuleId equals j.Id into ij
                             from ji in ij.DefaultIfEmpty()
                             select new MasterLibraryModel
                             {
                                 SubjectsName = a.Name,
                                 SubjectsCode = a.Code,
                                 SkillName = ea.Name,
                                 SkillCode = ea.Code,
                                 PracticeCode = ga.Name,
                                 PracticeName = ga.Code,
                                 ModuleCode = ji.Name,
                                 ModuleName = ji.Code,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(model.SubjectsCode))
            {
                dataQuery = dataQuery.Where(r => r.SubjectsCode.ToUpper().Contains(model.SubjectsCode.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.SkillCode))
            {
                dataQuery = dataQuery.Where(r => r.SkillCode.ToUpper().Contains(model.SkillCode.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.PracticeCode))
            {
                dataQuery = dataQuery.Where(r => r.PracticeCode.ToUpper().Contains(model.PracticeCode.ToUpper()));
            }
            if (!string.IsNullOrEmpty(model.ModuleCode))
            {
                dataQuery = dataQuery.Where(r => r.ModuleCode.ToUpper().Contains(model.ModuleCode.ToUpper()));
            }
            List<MasterLibraryModel> listModel = dataQuery.ToList();
            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/MasterLibrary.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.OrderBy(i => i.SubjectsName).Select((a, i) => new
                {
                    Index = i + 1,
                    a.SubjectsCode,
                    a.SubjectsName,
                    a.SkillCode,
                    a.SkillName,
                    a.PracticeCode,
                    a.PracticeName,
                    a.ModuleCode,
                    a.ModuleName,
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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Tổng hợp dữ liệu thư viện" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Tổng hợp dữ liệu thư viện" + ".xls";

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
