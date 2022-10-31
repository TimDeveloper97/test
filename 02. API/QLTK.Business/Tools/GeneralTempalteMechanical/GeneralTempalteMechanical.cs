using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.QLTKGROUPMODUL;
using NTS.Model.Repositories;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QLTK.Business.GeneralTempalteMechanical
{
    public class GeneralTempalteMechanical
    {
        private QLTKEntities db = new QLTKEntities();

        public string GeneralDegignMechanical(GeneralMechanicalModel model)
        {
            checkExistProduct(model);
            model.IsExport = true;
            GeneralMechanicalModel newModel = new GeneralMechanicalModel();
            var data = (from a in db.Users.AsNoTracking()
                        join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                        where a.Id.Equals(model.CreateBy)
                        select new GeneralMechanicalModel
                        {
                            UserName = b.Name
                        }).FirstOrDefault();
            var modulegrId = db.Modules.AsNoTracking().FirstOrDefault(a => a.Name.Equals(model.ProductName)).ModuleGroupId;
            var dataQuery = (from a in db.ModuleGroups.AsNoTracking()
                             where a.Id.Equals(modulegrId) || a.ParentId.Equals(modulegrId)
                             select new GroupModuleModel
                             {
                                 Name = a.Name,
                                 Code = a.Code
                             }).FirstOrDefault();

            string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
            string templatePath = HttpContext.Current.Server.MapPath("~/Template/PhacThaoThietKe.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {
                IWSection section = document.AddSection();
                IWParagraph paragraph = section.AddParagraph();

                var now = DateTime.Today;
                document.NTSReplaceFirst("<day>", now.Day.ToString());
                document.NTSReplaceFirst("<month>", now.Month.ToString());
                document.NTSReplaceFirst("2013", now.Year.ToString());
                document.NTSReplaceFirst("<now>", DateTime.Now.ToString("dd/MM/yyyy hh:mm: ss tt "));
                document.NTSReplaceFirst("<user>", data.UserName);
                document.NTSReplaceAll("Nguyễn Quốc Đạt", data.UserName);
                document.NTSReplaceAll("TPAD.M0102", model.ProductCode);
                document.NTSReplaceAll("Bộ thực hành test", model.ProductName);
                document.NTSReplaceAll("<name>", dataQuery.Name);
                document.NTSReplaceAll("<code>", dataQuery.Code);
                document.NTSReplaceImage("picture1", model.TechnologicalScheme);
                document.NTSReplaceImage("picture2", model.FaceModule);
                document.NTSReplaceImage("picture3", model.RelationshipClusters);
                document.NTSReplaceAll("pcname", model.UserNamePC);

                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "PhacThaoThietKe_" + specifyName + ".docm");
                document.Save(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_GeneralDegignMechanical);

                document.Close();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "PhacThaoThietKe_" + specifyName + ".docm";

            return pathreturn;
        }


        public void checkExistProduct(GeneralMechanicalModel model)
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
