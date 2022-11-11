using NTS.Common;
using NTS.Model.GeneralTemplate;
using NTS.Model.Repositories;
using NTS.Model.QLTKGROUPMODUL;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using NTS.Model.Employees;
using QLTK.Business.Users;

namespace QLTK.Business.CheckDesignPlan
{
    public class CheckDesignPlanBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public string CheckDesignPlan(GeneralMechanicalModel model)
        {
            checkExistProduct(model);
            model.IsExport = true;
            var data = (from a in db.Modules.AsNoTracking()
                        join b in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals b.Id
                        where a.Code.Equals(model.ProductCode)
                        select new GroupModuleModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            Code = b.Code,
                        }).FirstOrDefault();
            var module = db.Modules.AsNoTracking().Where(u => u.Code.Equals(model.ProductCode)).FirstOrDefault().Id;
            var user = db.Users.AsNoTracking().FirstOrDefault(u => u.Id.Equals(model.CreateBy)).UserName;
            var design = (from a in db.Users.AsNoTracking()
                          join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
                          where a.Id.Equals(model.CreateBy)
                          select new GroupModuleModel
                          {
                              Name = b.Name
                          }).FirstOrDefault();
            GeneralMechanicalModel newModel = new GeneralMechanicalModel();

            string templatePath = HttpContext.Current.Server.MapPath("~/Template/BieuMauKiemTraThietKe.docm");
            using (WordDocument document = new WordDocument(templatePath))
            {
                var now = DateTime.Today;

                document.NTSReplaceAll("<Header1>", model.ProductName);
                document.NTSReplaceAll("<Header2>", model.ProductCode);
                document.NTSReplaceAll("<Header3>", data.Name + "/" + data.Code);
                document.NTSReplaceAll("<ModuleName>", model.ProductName);
                document.NTSReplaceAll("<ModuleCode>", model.ProductCode);
                document.NTSReplaceAll("<ModuleGroupName>", data.Name);
                document.NTSReplaceAll("<ModuleGroupCode>", data.Code);
                document.NTSReplaceAll("<data>", design.Name);
                document.NTSReplaceAll("<user>", user);
                document.NTSReplaceAll("<now>", now.ToString());
                //if (model.FileElectric == false && model.FileElectronic == false && model.FileMechanics == false)
                //{
                //    document.NTSReplaceAll("<TK1>", "");
                //    document.NTSReplaceAll("<TK2>", "");
                //    document.NTSReplaceAll("<TK3>", "");
                //}
                //else
                //{
                //    document.NTSReplaceAll("<TK1>", model.FileElectric == true ? "Điện" : "");
                //    if (model.FileElectric == false)
                //    {
                //        document.NTSReplaceAll("<TK1>", model.FileElectronic == true ? "Điện tử" : "");
                //        document.NTSReplaceAll("<TK2>", "");
                //        document.NTSReplaceAll("<TK3>", "");
                //    }
                //    else
                //    {
                //        document.NTSReplaceAll("<TK2>", model.FileElectronic == true ? "Điện tử" : "");
                //    }
                //    if (model.FileElectronic == false)
                //    {
                //        document.NTSReplaceAll("<TK2>", model.FileMechanics == true ? "Cơ khí" : "");
                //        document.NTSReplaceAll("<TK3>", "");
                //    }
                //    else
                //    {
                //        document.NTSReplaceAll("<TK3>", model.FileMechanics == true ? "Cơ khí" : "");
                //    }
                //}

                string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "BieuMauKiemTraThietKe." + model.ProductCode + ".docm");
                document.Save(pathFileSave);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, model.ProductCode, null, Constants.LOG_CheckDesignPlan);

                document.Close();
            }
            string pathreturn = "Template/" + Constants.FolderExport + "BieuMauKiemTraThietKe." + model.ProductCode + ".docm";

            return pathreturn;
        }

        public void checkExistProduct(GeneralMechanicalModel model)
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
                model.FileMechanics = module.FileMechanics;
                model.FileElectronic = module.FileElectronic;
                model.FileElectric = module.FileElectric;
            }
            else
            {
                throw NTSException.CreateInstance("Mã này không tồn tại!");
            }

        }
    }
}
