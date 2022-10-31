using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.ModuleGroupTimeStandards;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.ModuleGroupTimeStandard
{
    public class ModuleGroupTimeStandardBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public void CreateModuleGroupTimeStandard(ModuleGroupTimeStandardsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                NTS.Model.Repositories.ModuleGroupTimeStandard moduleGrTimeStandard = new NTS.Model.Repositories.ModuleGroupTimeStandard
                {
                    Id = Guid.NewGuid().ToString(),
                    DepartmentId = model.DepartmentId,
                    ModuleGroupId = model.ModuleGroupId,
                    Status = model.Status,
                };

                db.ModuleGroupTimeStandards.Add(moduleGrTimeStandard);
                db.SaveChanges();
                trans.Commit();
            }
        }
        public void UpdateModuleGroupTimeStandard(ModuleGroupTimeStandardsModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var moduleGrTimeStandard = db.ModuleGroupTimeStandards.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (moduleGrTimeStandard == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0040, TextResourceKey.ModuleGroupTimeStandard);
                }

                moduleGrTimeStandard.ModuleGroupId = model.ModuleGroupId;
                moduleGrTimeStandard.DepartmentId = model.DepartmentId;
                moduleGrTimeStandard.Status = model.Status;
                db.SaveChanges();
                trans.Commit();

            }
        }
    }
}
