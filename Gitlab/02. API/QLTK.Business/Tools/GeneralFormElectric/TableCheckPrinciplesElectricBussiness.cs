using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.GeneralFormElectric
{
    public class TableCheckPrinciplesElectricBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralCheckPrinciplesElectric(TableCheckPrinciplesElectricModel model)
        {
            checkExistProduct(model);

            TableCheckPrinciplesElectricModel newModel = new TableCheckPrinciplesElectricModel();


            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new TableCheckPrinciplesElectricModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/BangKiemTraNguyenLy.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];

                var now = DateTime.Today;
                sheet[9, 4].Value = model.ProductName;
                sheet[9, 9].Value = model.ProductCode;
                sheet[30, 7].Value = "Ngày" + now.Date + "Tháng" + now.Month + "Năm" + now.Year;
                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "BangKiemTraNguyenLy" + ".xlsm");
                workbook.SaveAs(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, null, Constants.LOG_GeneralCheckPrinciplesElectric);

                workbook.Close();
                excelEngine.Dispose();
            }

            string pathreturn = "Template/" + Constants.FolderExport + "Bangkiemtranguyenly" + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(TableCheckPrinciplesElectricModel model)
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
