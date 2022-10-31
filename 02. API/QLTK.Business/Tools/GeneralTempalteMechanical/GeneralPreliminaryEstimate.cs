using NTS.Common;
using NTS.Model.Combobox;
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

namespace QLTK.Business.GeneralTempalteMechanical
{
    public class GeneralPreliminaryEstimate
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplatePreliminaryEstimate(GeneralMechanicalModel model)
        {
            checkExistProduct(model);

            SearchResultModel<GeneralMechanicalModel> searchResult = new SearchResultModel<GeneralMechanicalModel>();

            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new GeneralMechanicalModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();
            List<MaterialModel> listRs = new List<MaterialModel>();
            listRs = model.Materials;


            if (listRs.Count == 0)
            {
                throw NTSException.CreateInstance("Module không có vật tư, không thể xuất file");

            }

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/DuToanSoBo.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];

                sheet[3, 3].Value = model.ProductName;
                sheet[3, 8].Value = "Mã:" + model.ProductCode;
                sheet[4, 3].Value = data.UserName;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);
                var total = listRs.Count;

                var listExport = listRs.Select((a, i) => new
                {
                    Index = i + 1,
                    a.MaterialName, // tên
                    a.RawMaterialName,
                    a.MaterialCode, // mã
                    a.UnitName, // đơn vị
                    a.Quantity, // số lượng
                    a.DeliveryDays,
                    a.ManufacturerName,
                    b = a.Quantity,
                    a.Pricing,
                    c = a.Quantity * a.Pricing,
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



                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "DuToanSoBo_" + specifyName + ".xlsm");
                workbook.SaveAs(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralPreliminaryEstimate);

                workbook.Close();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "DuToanSoBo_" + specifyName + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(GeneralMechanicalModel model)
        {
            if (model.ProductCode == "")
            {
                //cần sửa lại
                throw NTSException.CreateInstance("Bạn phải nhập mã Mạch");
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
