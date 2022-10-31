using NTS.Common;
using NTS.Model.ExportTemplate;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.GeneralTemplate
{
    public class GeneralElectronic
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplateElectronic(GeneralElectronicModel model)
        {
            checkExistProduct(model);
            model.IsExport = true;
            GeneralElectronicModel newModel = new GeneralElectronicModel();


            newModel.DateNow = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(newModel.DateNow.ToString()));

            newModel.Day = datevalue.Day.ToString();
            newModel.Month = datevalue.Month.ToString();
            newModel.Year = datevalue.Year.ToString();

            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new GeneralElectronicModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            string templatePath = HttpContext.Current.Server.MapPath("~/Template/KiemTraMachIn.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {

                var now = DateTime.Today;
                document.NTSReplaceAll("<day>", now.Day.ToString());
                document.NTSReplaceAll("<month>", now.Month.ToString());
                document.NTSReplaceAll("<year>", now.Year.ToString());
                document.NTSReplaceAll("<now>", DateTime.Now.ToString());
                document.NTSReplaceAll("<user>", model.CreateBy);


                document.NTSReplaceAll("PCB.CODE", model.ProductCode);
                document.NTSReplaceAll("<productname>", model.ProductName);
                document.NTSReplaceAll("<paint>", model.Paint);
                document.NTSReplaceAll("<check>", model.Check);
                document.NTSReplaceAll("<approve>", model.Approve);
                document.NTSReplaceAll("<designer>", data.UserName);
                document.NTSReplaceAll("<code>", model.Code);
                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "KiemTraMachIn_" + specifyName + ".docm");
                document.Save(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralElectronic);

                document.Close();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "KiemTraMachIn_" + specifyName + ".docm";

            return pathreturn;
        }

        public void checkExistProduct(GeneralElectronicModel model)
        {
            if (model.ProductCode == null)
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
