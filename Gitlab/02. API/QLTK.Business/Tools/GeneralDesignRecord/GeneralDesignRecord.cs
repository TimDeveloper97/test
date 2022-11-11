using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.GeneralDesignRecord
{
    public class GeneralDesignRecord
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTempalteDesignRecord(DesignRecordModel model)
        {
            checkExistProduct(model);
            model.IsExport = true;
            DesignRecordModel newModel = new DesignRecordModel();


            newModel.DateNow = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(newModel.DateNow.ToString()));

            newModel.Day = datevalue.Day.ToString();
            newModel.Month = datevalue.Month.ToString();
            newModel.Year = datevalue.Year.ToString();

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            string templatePath = HttpContext.Current.Server.MapPath("~/Template/HoSoThietKe.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {
                var now = DateTime.Today;
                document.NTSReplaceFirst("<day>", now.Day.ToString());
                document.NTSReplaceFirst("<month>", now.Month.ToString());
                document.NTSReplaceAll("<year>", now.Year.ToString());
                document.NTSReplaceFirst("<now>", DateTime.Now.ToString());
                document.NTSReplaceFirst("<user>", model.CreateBy);

                document.NTSReplaceAll("PCB.CODE", model.ProductCode);
                document.NTSReplaceAll("<productname>", model.ProductName);

                document.NTSReplaceFirst("<4M>", model.check_9_M ? "Có" : "Không có");
                document.NTSReplaceFirst("<4C>", model.check_9_C ? "Có" : "Không có");
                document.NTSReplaceFirst("<5M>", model.check_8_M ? "Có" : "Không có");
                document.NTSReplaceFirst("<5C>", model.check_8_C ? "Có" : "Không có");
                document.NTSReplaceFirst("<11B>", model.check_11b_3 ? "Có" : "Không có");
                document.NTSReplaceFirst("<12A>", model.check_12a_3 ? "Có" : "Không có");
                document.NTSReplaceFirst("<12B>", model.check_12b_3 ? "Có" : "Không có");


                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "HoSoThietKe_" + specifyName + ".docm");
                document.Save(pathFileSave);
                document.Close();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "HoSoThietKe_" + specifyName + ".docm";

            return pathreturn;
        }

        public void checkExistProduct(DesignRecordModel model)
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
