using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.GeneralFormElectric
{
    public class DataProgramElectricBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralDataProgramElectric(DataProgramElectricModel model)
        {
            checkExistProduct(model);

            string templatePath = HttpContext.Current.Server.MapPath("~/Template/DuLieuCaiDatDien.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {
                var now = DateTime.Today;

                var data = (from a in db.Users.AsNoTracking()
                            join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                            where a.Id.Equals(model.CreateBy)
                            select new DataProgramElectricModel
                            {
                                UserName = b.Name
                            }).FirstOrDefault();

                document.NTSReplaceAll("Nv", data.UserName);
                document.NTSReplaceAll("Masp", model.ProductCode);
                document.NTSReplaceAll("Tensp", model.ProductName);


                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "DuLieuCaiDatDien" + ".docm");
                document.Save(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.Code, null, Constants.LOG_GeneralDataProgramElectric);

                document.Close();
            }

            string pathreturn = "Template/" + Constants.FolderExport + "DuLieuCaiDatDien" + ".docm";

            return pathreturn;
        }
        public void checkExistProduct(DataProgramElectricModel model)
        {
            if (string.IsNullOrEmpty(model.ProductCode))
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
