using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.GeneralCheckList
{
    public class GeneralCheckList
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTempalteCheckList(GeneralCheckListModel model)
        {
            checkExistProduct(model);
            model.IsExport = true;
            GeneralCheckListModel newModel = new GeneralCheckListModel();

            string templatePath = HttpContext.Current.Server.MapPath("~/Template/DanhMucKiemTra.docm");
            WordDocument document = new WordDocument(templatePath);
            var now = DateTime.Today;

            document.NTSReplaceAll("2014", now.Year.ToString());
            document.NTSReplaceAll("tpad.a0001", model.ProductCode);
            document.NTSReplaceAll("tên_sản_phẩm", model.ProductName);
            if (model.UserCheck != "")
            {
                document.NTSReplaceAll("Nguyễn Thu Hiền", model.UserCheck);
            }
            document.NTSReplaceAll("04/08/2017 10:06:07 AM", now.ToString());
            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "DanhMucKiemTra_" + specifyName + ".doc");
            document.Save(pathFileSave);

            UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralCheckList);

            document.Close();
            string pathreturn = "Template/" + Constants.FolderExport + "DanhMucKiemTra_" + specifyName + ".doc";

            return pathreturn;
        }

        public void checkExistProduct(GeneralCheckListModel model)
        {
            if (model.ProductCode == "")
            {
                //cần sửa lại
                throw NTSException.CreateInstance("Bạn phải nhập mã Module");
            }

            var module = db.Modules.FirstOrDefault(u => u.Code.Equals(model.ProductCode));

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
