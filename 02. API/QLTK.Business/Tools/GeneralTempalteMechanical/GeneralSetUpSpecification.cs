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

namespace QLTK.Business.GeneralSetUpSpecification
{
    public class GeneralSetUpSpecification
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplateSetUpSpecification(GeneralMechanicalModel model)
        {
            //checkExistProduct(model);
            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/LapThongSoKyThuat.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];


                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "LapThongSoKyThuat_" + specifyName + ".xlsx");
                workbook.SaveAs(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralSetUpSpecification);

                workbook.Close();
                excelEngine.Dispose();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "LapThongSoKyThuat_" + specifyName + ".xlsx";
            return pathreturn;
        }
    }
}
