using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.XlsIO;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Pdf;
using System.Web;
using System.Data.Entity.Core.Objects;
using NTS.Model.Repositories;
using NTS.Model.Combobox;
using NTS.Model.SearchResults;
using NTS.Model.SearchCondition;
using NTS.Model;
using NTS.Utils;
using DateTimeUtils = NTS.Utils.DateTimeUtils;
using SQLHelpper = NTS.Model.SQLHelpper;

namespace QLTK.Business
{
    public class EventLogBusiness
    {
        private static EventLogBusiness _eventLogBusiness;
        private QLTKEntities db = new QLTKEntities();



        public static EventLogBusiness GetInstance()
        {
            if (_eventLogBusiness == null)
            {
                _eventLogBusiness = new EventLogBusiness();
            }

            return _eventLogBusiness;
        }

        public SearchResultModel<UserEventLogSearchResult> SearchUserEventLog(UserEventLogSearchCondition searchCondition)
        {
            SearchResultModel<UserEventLogSearchResult> searchResult = new SearchResultModel<UserEventLogSearchResult>();
            try
            {
                var listmodel = (from a in db.ActivityLogs.AsNoTracking()
                                 join b in db.Users.AsNoTracking() on a.UserId equals b.Id into ab
                                 from abv in ab.DefaultIfEmpty()
                                 select new UserEventLogSearchResult()
                                 {
                                     UserEventLogId = a.Id,
                                     UserId = a.UserId,
                                     Description = a.LogContent,
                                     CreateDate = a.LogDate,
                                     UserName = abv.UserName,
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(searchCondition.Description))
                {
                    listmodel = listmodel.Where(r => r.Description.ToUpper().Contains(searchCondition.Description.ToUpper()));
                }
             
                if (searchCondition.LogDateFrom.HasValue)
                {
                    searchCondition.LogDateFrom = DateTimeUtils.ConvertDateFrom(searchCondition.LogDateFrom);
                    listmodel = listmodel.Where(r => r.CreateDate.HasValue && r.CreateDate >= searchCondition.LogDateFrom);
                }
                if (searchCondition.LogDateTo.HasValue)
                {
                    searchCondition.LogDateTo = DateTimeUtils.ConvertDateTo(searchCondition.LogDateTo);
                    listmodel = listmodel.Where(r => r.CreateDate.HasValue && r.CreateDate <= searchCondition.LogDateTo);
                }

                searchResult.TotalItem = listmodel.Select(r => r.UserEventLogId).Count();
                var listResult = SQLHelpper.OrderBy(listmodel, "CreateDate", false).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize)
                            .Take(searchCondition.PageSize)
                            .ToList();


           
                searchResult.ListResult = listResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh", ex.InnerException);
            }

            return searchResult;
        }

        private string Export(string saveOption, List<UserEventLogSearchResult> eventLogList, UserEventLogSearchCondition model)
        {
            string pathExport = string.Empty;
            // Khỏi tạo bảng excel
            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath("/Template/LichSuTruyCapSuDung.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            IRange rangeDateFrom = sheet.FindFirst("<DateFrom>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            rangeDateFrom.Text = rangeDateFrom.Text.Replace("<DateFrom>", (model.LogDateFrom.HasValue ? model.LogDateFrom.Value.ToString("dd/MM/yyy") : "--/--/----"));
            IRange rangeDateTo = sheet.FindFirst("<DateTo>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            rangeDateTo.Text = rangeDateTo.Text.Replace("<DateTo>", (model.LogDateTo.HasValue ? model.LogDateTo.Value.ToString("dd/MM/yyy") : "--/--/----"));

            int total = eventLogList.Count;
            int index = 1;
            var listExport = (from a in eventLogList
                              select new
                              {
                                  Index = index++,
                                  a.LogTypeName,
                                  a.UserType,
                                  a.Description,
                                  CreateDate = a.CreateDate.HasValue ? a.CreateDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : string.Empty,
                                  a.UserName
                              }).ToList();

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);

            if (saveOption.Equals("PDF"))
            {
                sheet.Range[iRangeData.Row, 1, total + 4, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, total + 4, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, total + 4, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, total + 4, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, total + 4, 6].Borders.Color = ExcelKnownColors.Black;

                pathExport = "/Template/Export/" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "LichSuTruyCapSuDung.pdf";
                //convert the sheet to pdf

                ExcelToPdfConverter converter = new ExcelToPdfConverter(sheet);

                PdfDocument pdfDocument = new PdfDocument();

                pdfDocument = converter.Convert();

                pdfDocument.Save(HttpContext.Current.Server.MapPath(pathExport));

                pdfDocument.Close();

                converter.Dispose();

                workbook.Close();
                excelEngine.Dispose();
            }
            else
            {
                pathExport = "/Template/Export/" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "LichSuTruyCapSuDung.xlsx";
                workbook.SaveAs(HttpContext.Current.Server.MapPath(pathExport));
            }

            return pathExport;
        }
    }
}
