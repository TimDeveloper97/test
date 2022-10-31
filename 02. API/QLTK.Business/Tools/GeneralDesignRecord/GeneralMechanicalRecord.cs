using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.GeneralMechanicalRecords
{
    public class GeneralMechanicalRecord
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplateMechanicalRecord(MechanicalRecordsModel model)
        {

            checkExistProduct(model);

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/HoSoThietKe_CoKhi.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];
                var data = (from a in db.Users.AsNoTracking()
                            join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                            where a.Id.Equals(model.CreateBy)
                            select new MechanicalRecordsModel
                            {
                                UserName = b.Name
                            }).FirstOrDefault();
                var now = DateTime.Today;
                sheet.PageSetup.LeftHeader = "model.Code" + " - " + "model.Name";
                sheet.PageSetup.RightHeader = "model.UserName";

                sheet[5, 3].Value = model.ProductName;
                sheet[6, 3].Value = model.ProductCode;
                sheet[7, 3].Value = data.UserName;

                sheet[9, 1].Value = "Tân Phát - " + DateTime.Now.Year;
                sheet[34, 3].Value = "Tân Phát,  ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;

                sheet[14, 3].Value = model.check_14_3 == true ? "VT." + model.ProductCode + ".xlsm" : "Không có";
                sheet[15, 3].Value = model.check_15_3 == true ? "DOC." + model.ProductCode : "Không có";
                sheet[16, 3].Value = model.check_16_3 == true ? "HS." + model.ProductCode + ".xlsx" : "Không có";
                sheet[17, 3].Value = model.check_17_3 == true ? "PTTK." + model.ProductCode + ".docm" : "Không có";
                sheet[18, 3].Value = model.check_18_3 == true ? "3D." + model.ProductCode : "Không có";
                sheet[19, 3].Value = model.check_19_3 == true ? "CAD." + model.ProductCode : "Không có";
                sheet[20, 3].Value = model.check_20_3 == true ? "MAT." + model.ProductCode : "Không có";
                sheet[21, 3].Value = model.check_21_3 == true ? "DATA1.Ck." + model.ProductCode : "Không có";
                sheet[22, 3].Value = model.check_22_3 == true ? "IGS." + model.ProductCode : "Không có";
                sheet[23, 3].Value = model.check_23_3 == true ? "LRP." + model.ProductCode : "Không có";
                sheet[24, 3].Value = model.check_24_3 == true ? "DAT." + model.ProductCode : "Không có";
                sheet[25, 3].Value = model.check_25_3 == true ? "TL." + model.ProductCode : "Không có";
                sheet[26, 3].Value = model.check_26_3 == true ? "KN." + model.ProductCode : "Không có";

                sheet[27, 3].Value = model.check_27_3 == true ? model.ProductCode : "Không có";
                sheet[28, 3].Value = model.check_28_3 == true ? "DAT." + model.ProductCode : "Không có";
                sheet[29, 3].Value = model.check_29_3 == true == true ? "PRJ." + model.ProductCode : "Không có";
                sheet[30, 3].Value = model.check_30_3 == true ? model.ProductCode + ".Dn.pdf" : "Không có";
                //Thêm mạch vào trong hồ sơ thiết kế
                string pathProduct = model.Path;
                string materialCode = string.Empty;
                List<MechanicalRecordsModel> list = new List<MechanicalRecordsModel>();
                MechanicalRecordsModel newModel;
                string materialName = string.Empty;
                List<int> rowData = new List<int>();
                if (!System.IO.File.Exists(pathProduct))
                {
                    pathProduct = "";
                    sheet[32, 1].Value = "";
                }
                else
                {
                    using (Stream fs = File.OpenRead(pathProduct))
                    {
                        ExcelEngine excelEngine1 = new ExcelEngine();
                        IApplication application1 = excelEngine1.Excel;
                        IWorkbook workbook_1 = application1.Workbooks.Open(fs);
                        IWorksheet sheet_1 = workbook_1.Worksheets[0];

                        try
                        {
                            int rowCount = sheet_1.Rows.Count();
                            for (int i = 7; i < rowCount; i++)
                            {
                                materialName = sheet_1[i, 2].Text;
                                materialCode = sheet_1[i, 4].Text;
                                try
                                {
                                    if (!string.IsNullOrEmpty(materialCode))
                                    {
                                        if (materialCode.ToUpper().StartsWith("PCB"))
                                        {
                                            newModel = new MechanicalRecordsModel();
                                            newModel.Name = materialName;
                                            newModel.Code = materialCode;
                                            list.Add(newModel);
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            workbook_1.Close();
                            excelEngine1.Dispose();
                            throw new Exception("Có lỗi phát sinh trong quá trình xử lý");
                            throw (ex);
                        }
                        workbook_1.Close();
                        excelEngine1.Dispose();
                    }

                    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                    var total = list.Count;
                    var listExport = list.Select((a, i) => new
                    {
                        Index = "3." + (i + 1),
                        a.Name, // tên
                        a.Code, // mã
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
                }


                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "HoSoThietKe_CoKhi_" + specifyName + ".xlsm");
                workbook.SaveAs(pathFileSave);
                workbook.Close();
                excelEngine.Dispose();
            }

            string pathreturn = "Template/" + Constants.FolderExport + "HoSoThietKe_CoKhi_" + specifyName + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(MechanicalRecordsModel model)
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
