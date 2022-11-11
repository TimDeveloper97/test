using NTS.Common;
using NTS.Model.ExportTemplate;
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

namespace QLTK.Business.GeneralTemplate
{
    public class GeneralFunctionDesignOptions
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTemplateFunctionDesignOptions(GeneralElectronicModel model)
        {

            checkExistProduct(model);
            model.IsExport = true;
            GeneralElectronicModel newModel = new GeneralElectronicModel();


            newModel.DateNow = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(newModel.DateNow.ToString()));

            newModel.Day = datevalue.Day.ToString();
            newModel.Month = datevalue.Month.ToString();
            newModel.Year = datevalue.Year.ToString();



            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            string templatePath = HttpContext.Current.Server.MapPath("~/Template/PATK_DanhMucKhoiChucNang.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {

                var now = DateTime.Today;
                document.NTSReplaceFirst("<day>", now.Day.ToString());
                document.NTSReplaceFirst("<month>", now.Month.ToString());
                document.NTSReplaceFirst("<year>", now.Year.ToString());
                document.NTSReplaceFirst("<now>", DateTime.Now.ToString());
                document.NTSReplaceFirst("<user>", model.CreateBy);


                document.NTSReplaceAll("PCB.CODE", model.ProductCode);
                document.NTSReplaceAll("<productname>", model.ProductName);
                document.NTSReplaceFirst("<paint>", model.Paint);
                document.NTSReplaceFirst("<check>", model.Check);
                document.NTSReplaceFirst("<approve>", model.Approve);
                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "PATK_DanhMucKhoiChucNang_" + specifyName + ".docm");
                document.Save(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralFunctionDesignOptions);

                document.Close();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "PATK_DanhMucKhoiChucNang_" + specifyName + ".docm";

            return pathreturn;
        }

        public void checkExistProduct(GeneralElectronicModel model)
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
