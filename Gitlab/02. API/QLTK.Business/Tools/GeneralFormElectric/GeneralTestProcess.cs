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

namespace QLTK.Business.GeneralTestProcess
{
    public class GeneralTestProcess
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTempalteTestProcess(TestProcessModel model)
        {
            checkExistProduct(model);

            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new TestProcessModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/QuaTrinhThuNghiem.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];

                var now = DateTime.Today;
                sheet[8, 4].Value = model.ProductName;
                sheet[8, 8].Value = "Mã: " + model.ProductCode;
                sheet[9, 4].Value = data.UserName;
                sheet[24, 7].Value = "Ngày " + "... ..." + " tháng " + "... ..." + " năm " + DateTime.Now.Year;


                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "QuaTrinhThuNghiem_" + specifyName + ".xlsm");
                workbook.SaveAs(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, null, Constants.LOG_GeneralTestProcess);

                workbook.Close();
                excelEngine.Dispose();
            }

            string pathreturn = "Template/" + Constants.FolderExport + "QuaTrinhThuNghiem_" + specifyName + ".xlsm";
            return pathreturn;
        }

        public void checkExistProduct(TestProcessModel model)
        {
            if (model.ProductCode == null)
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
