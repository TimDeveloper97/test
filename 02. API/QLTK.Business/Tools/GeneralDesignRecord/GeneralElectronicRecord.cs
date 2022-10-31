using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.GeneralElectronicRecord
{
    public class GeneralElectronicRecord
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplateMaterial(ElectronicRecordsModel model)
        {
            checkExistProduct(model);
            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/HoSoThietKeDien.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];


                //IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                //iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var now = DateTime.Today;
                //sheet.Replace("<day>", now.Day.ToString(), ExcelFindOptions.MatchCase);
                //sheet.Replace("<month>", now.Month.ToString(), ExcelFindOptions.MatchCase);
                //sheet.Replace("<year>", now.Year.ToString(), ExcelFindOptions.MatchCase);
                var data = (from a in db.Users.AsNoTracking()
                            join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                            where a.Id.Equals(model.CreateBy)
                            select new ElectronicRecordsModel
                            {
                                UserName = b.Name
                            }).FirstOrDefault();
                sheet[5, 3].Value = model.ProductName;
                sheet[6, 3].Value = model.ProductCode;
                sheet[7, 3].Value = data.UserName;

                sheet[9, 1].Value = "Tân Phát - " + DateTime.Now.Year;

                sheet[13, 3].Value = "DAT." + model.ProductCode;
                sheet[14, 3].Value = "BCD." + model.ProductCode + ".Dn.docm";
                sheet[15, 3].Value = "BKT." + model.ProductCode + ".Dn.xlsm";
                sheet[16, 3].Value = "BLT." + model.ProductCode + ".Dn.xlsm";
                sheet[17, 3].Value = "MTK." + model.ProductCode + ".Dn.xlsm";
                sheet[18, 3].Value = "HS." + model.ProductCode + ".Dn.xlsm";
                sheet[19, 3].Value = "PRJ." + model.ProductCode;
                sheet[20, 3].Value = model.ProductCode + ".Dn.zw1";
                sheet[21, 3].Value = "PRD." + model.ProductCode;
                sheet[22, 3].Value = model.Code + ".Dn.pdf";
                sheet[23, 3].Value = "HMI." + model.ProductCode;
                sheet[24, 3].Value = "HMI." + model.ProductCode + ".rar";
                sheet[25, 3].Value = "PLC." + model.ProductCode;
                sheet[26, 3].Value = "PLC." + model.ProductCode + ".rar";

                sheet[14, 4].Value = model.check_14_4 ? "Có" : "Không có";
                sheet[15, 4].Value = model.check_15_4 ? "Có" : "Không có";
                sheet[16, 4].Value = model.check_16_4 ? "Có" : "Không có";
                sheet[17, 4].Value = model.check_17_4 ? "Có" : "Không có";
                sheet[18, 4].Value = model.check_18_4 ? "Có" : "Không có";
                sheet[20, 4].Value = model.check_20_4 ? "Có" : "Không có";
                sheet[22, 4].Value = model.check_22_4 ? "Có" : "Không có";
                sheet[24, 4].Value = model.check_24_4 ? "Có" : "Không có";
                sheet[26, 4].Value = model.check_26_4 ? "Có" : "Không có";


                var moduleGroupId = db.Modules.AsNoTracking().Where(a => a.Code.Equals(model.ProductCode)).Select(a => a.ModuleGroupId).FirstOrDefault();

                var listProductStand = (from a in db.ModuleGroupProductStandards.AsNoTracking()
                                        join b in db.ProductStandards.AsNoTracking() on a.ProductStandardId equals b.Id
                                        where a.ModuleGroupId.Equals(moduleGroupId) && b.DataType == Constants.Employee_WorkType_Dn
                                        orderby b.Name
                                        select new GenealProductStandModel()
                                        {
                                            Name = b.Name,
                                            Code = b.Code,
                                        }).ToList();

                sheet[34 + listProductStand.Count, 3].Value = "Tân Phát,  ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                var total = listProductStand.Count;

                var listExport = listProductStand.Select((a, i) => new
                {
                    Index = i + 1,
                    a.Name,
                    a.Code,
                });


                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);

                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 3].Borders.Color = ExcelKnownColors.Black;
                    //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 3].CellStyle.WrapText = true;
                }

                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "HoSoThietKeDien_" + specifyName + ".xlsm");
                workbook.SaveAs(pathFileSave);
                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralElectronicRecordn);
                workbook.Close();
                excelEngine.Dispose();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "HoSoThietKeDien_" + specifyName + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(ElectronicRecordsModel model)
        {
            if (model.ProductCode == "")
            {
                //cần sửa lại
                throw NTSException.CreateInstance("Bạn phải nhập mã Module");
            }

            var module = db.Modules.AsNoTracking().FirstOrDefault(u => u.Code.Equals(model.ProductCode));
            if (module != null)
            {
                model.ProductName = module.Name;
            }
            else
            {
                throw NTSException.CreateInstance("Mã này không tồn tại!");
            }

        }
    }
}
