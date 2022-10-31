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

namespace QLTK.Business.GeneralFormElectric
{
    public class DesignArticleBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public string DesignArticleElectric(DesignArticleModel model)
        {
            checkExistProduct(model);

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

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/HangMucTK.xlsm"));

                IWorksheet sheet = workbook.Worksheets[0];

                var now = DateTime.Today;
                sheet[10, 3].Value = model.ProductName;
                sheet[10, 4].Value = "Mã: " + model.ProductCode;
                sheet[11, 3].Value = data.UserName;
                sheet[21, 3].Value = "Ngày" + now.Date + "Tháng" + now.Month + "Năm" + now.Year;
                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "HangMucTK" + ".xlsm");
                workbook.SaveAs(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, null, Constants.LOG_DesignArticleElectric);

                workbook.Close();
                excelEngine.Dispose();
               }
            string pathreturn = "Template/" + Constants.FolderExport + "HangMucTK" + ".xlsm";
            return pathreturn;
        }
        public void checkExistProduct(DesignArticleModel model)
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
