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

namespace QLTK.Business.GeneralDrawControlAlgorithm
{
    public class GeneralDrawControlAlgorithm
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralTempalteDrawControlAlgorithm(DrawControlAlgorithmModel model)
        {
            checkExistProduct(model);
            model.IsExport = true;
            GeneralMechanicalModel newModel = new GeneralMechanicalModel();

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            string templatePath = HttpContext.Current.Server.MapPath("~/Template/VeSoDoThuatToanDieuKhien.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {

                var now = DateTime.Today;

                var data = (from a in db.Users.AsNoTracking()
                            join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                            where a.Id.Equals(model.CreateBy)
                            select new GeneralMechanicalModel
                            {
                                UserName = b.Name
                            }).FirstOrDefault();
                document.NTSReplaceAll("NguyenVanA", data.UserName);
                document.NTSReplaceAll("tpad.a0000", model.ProductCode);
                document.NTSReplaceAll("MachInSo1", model.ProductName);

                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "VeSoDoThuatToanDieuKhien_" + specifyName + ".docm");
                document.Save(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, null, Constants.LOG_GeneralDrawControlAlgorithmModel);

                document.Close();
            }

            string pathreturn = "Template/" + Constants.FolderExport + "VeSoDoThuatToanDieuKhien_" + specifyName + ".docm";

            return pathreturn;
        }
        public void checkExistProduct(DrawControlAlgorithmModel model)
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



