using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Repositories;
using NTS.Model.TestDesign;
using NTS.Model.WebService;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.TestDesign
{
    public class TestDesignBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public string Excel(ReportTestDesignModel model)
        {
            string moduleCode = model.ListHardDesign[0].ModuleCode;
            List<SoftDesignModel> listModel = new List<SoftDesignModel>();
            SoftDesignModel softDesign;
            foreach (var itemSoftDesign in model.ListSoftDesign)
            {
                if (itemSoftDesign.IsExistName == false)
                {
                    itemSoftDesign.NoteReport = "Bản cứng không có";
                    listModel.Add(itemSoftDesign);
                }
            }

            foreach (var hardDesign in model.ListHardDesign)
            {
                bool isAdd = false;
                softDesign = new SoftDesignModel();
                softDesign.Name = "TPAD." + hardDesign.Code.Substring(3);


                if (hardDesign.IsExistName == false)
                {
                    softDesign.NoteReport = "Bản mềm không có";
                    isAdd = true;
                }
                if (hardDesign.IsDifferentSize == true)
                {
                    softDesign.SizeReport = "V";
                    isAdd = true;
                }
                if (hardDesign.IsDifferentDate == true)
                {
                    softDesign.DateReport = "V";
                    isAdd = true;
                }
                if (isAdd == true)
                {
                    listModel.Add(softDesign);
                }
            }



            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0015, "");
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportCM.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeDataCode = sheet.FindFirst("<name>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataCode.Text = iRangeDataCode.Text.Replace("<name>", moduleCode);

                IRange iRangeDataDay = sheet.FindFirst("<day>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataDay.Text = iRangeDataDay.Text.Replace("<day>", "Hà Nội, ngày " + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString())
                    + " tháng " + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString()) + " năm " + DateTime.Now.Year);

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = listModel.Select((a, i) => new
                {
                    a.Name,
                    a.NoteReport,
                    dateReport = a.DateReport != null ? a.DateReport : "",
                    sizeReport = a.SizeReport != null ? a.SizeReport : ""
                });


                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 5].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "ReportCM" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "ReportCM" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public string ExportReportDMVT(ReportDMVTModel reportDMVTModel)
        {
            reportDMVTModel.ModuleName = db.Modules.AsNoTracking().FirstOrDefault(a => a.Code.Equals(reportDMVTModel.ModuleCode)).Name;
            if (reportDMVTModel.ModuleName == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResource.Module);
            }
            if (reportDMVTModel.ListMaterial.Count() == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0015, "");
            }

            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(reportDMVTModel.CreateBy)
                        select new 
                        {
                            UserName = b.Name
                        }).FirstOrDefault();

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/TP-TT13-BM08 - xac nhan vat tu dien, dien tu.xlsm"));

            IWorksheet sheet = workbook.Worksheets[0];

            IRange iRangeDataCode = sheet.FindFirst("<moduleCode>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataCode.Text = iRangeDataCode.Text.Replace("<moduleCode>", reportDMVTModel.ModuleCode);

            IRange iRangeDataName = sheet.FindFirst("<moduleName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataName.Text = iRangeDataName.Text.Replace("<moduleName>", reportDMVTModel.ModuleName);

            IRange iRangeDataCreateBy = sheet.FindFirst("<createBy>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataCreateBy.Text = iRangeDataCreateBy.Text.Replace("<createBy>", data.UserName);

            IRange iRangeDataDay = sheet.FindFirst("<day>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeDataDay.Text = iRangeDataDay.Text.Replace("<day>", "Tân Phát, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);

            var total = reportDMVTModel.ListMaterial.Count;
            IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);
            var listExport = reportDMVTModel.ListMaterial.Select((a, i) => new
            {
                a.Stt,
                a.Name,
                a.Specification,
                a.Code,
                a.RawMaterialCode,
                a.DV,
                a.SL,
                a.VL,
                a.KL,
                a.ManufactureCode,
                a.Note
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


            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "XNVT." + reportDMVTModel.ModuleCode + ".xlsm");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "XNVT." + reportDMVTModel.ModuleCode + ".xlsm";

            return resultPathClient;
        }

        public string ExportReportTestDesignStructure(ReportTestDesignModel model)
        {
            if (model.DatatbleCT.Count() == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0015, "");
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportTestDesignStruture.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = model.DatatbleCT.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var listExport = model.DatatbleCT.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.NameVT,
                    a.Size,
                    a.Type,
                    a.Hang,
                    a.PathLocal,
                    a.Path3D,
                    a.KLOld,
                    a.KLNew
                });

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


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + model.ModuleCode + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + model.ModuleCode + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }

        public string ExportResultDMVT(ReportTestDesignModel model)
        {

            var employee = (from a in db.Users.AsNoTracking()
                            where a.Id.Equals(model.Designer)
                            join b in db.Employees on a.EmployeeId equals b.Id into ab
                            from b in ab.DefaultIfEmpty()
                            select b.Name).FirstOrDefault();


            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Template_PartError.xls"));

                IWorksheet sheet = workbook.Worksheets[0];

                IRange iRangeData = sheet.FindFirst("<designer>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<designer>", employee);

                IRange iRangeDataDay = sheet.FindFirst("<day>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeDataDay.Text = iRangeDataDay.Text.Replace("<day>", "Tân Phát, ngày " + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day.ToString())
                    + " tháng " + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month.ToString()) + " năm " + DateTime.Now.Year);

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + model.FileName + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + model.FileName + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }

        }
    }
}
