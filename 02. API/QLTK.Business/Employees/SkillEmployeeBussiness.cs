using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.SkillEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SkillEmployee
{
    public class SkillEmployeeBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SkillEmployeeModel> SearchWorkSkill(SkillEmployeeModel modelSearch)
        {
            SearchResultModel<SkillEmployeeModel> searchResult = new SearchResultModel<SkillEmployeeModel>();
            try
            {

                var resuldInfor = (from b in db.EmployeeSkills.AsNoTracking() 
                                   join c in db.WorkSkills.AsNoTracking() on b.WorkSkillId equals c.Id 
                                   join d in db.WorkSkillGroups.AsNoTracking() on c.WorkSkillGroupId equals d.Id
                                   where b.EmployeeId.Equals(modelSearch.EmployeeId)
                                   orderby c.Name
                                   select new SkillEmployeeModel
                                   {
                                       Id = b.Id,
                                       EmployeeId = b.EmployeeId,
                                       WorkSkillId = b.WorkSkillId,
                                       Mark = b.Mark,
                                       Grade = b.Grade,
                                       Name = c.Name,
                                       Description = c.Description,
                                       WorkTypeName = d.Name,
                                       Score = c.Score,
                                   }).AsQueryable();

                searchResult.TotalItem = resuldInfor.Count();
                var listResult = resuldInfor.ToList();
                searchResult.ListResult = listResult;

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        public void AddSkillEmployee(SkillEmployeeModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var workTypeSkill = db.EmployeeSkills.Where(t => t.EmployeeId.Equals(model.EmployeeId)).ToList();
                    if (workTypeSkill.Count() > 0)
                    {
                        db.EmployeeSkills.RemoveRange(workTypeSkill);
                    }
                    if (model.ListResult.Count > 0)
                    {
                        foreach (var it in model.ListResult)
                        {
                            EmployeeSkill newWorkSkill = new EmployeeSkill()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeId = model.EmployeeId,
                                WorkSkillId = it.WorkSkillId,
                                Mark = it.Mark,
                                Grade = it.Grade,
                            };
                            db.EmployeeSkills.Add(newWorkSkill);
                        }
                    }

                    var skillEmployee = db.EmployeeSkills.Where( a=> a.EmployeeId.Equals(model.EmployeeId)).ToList();

                    if(skillEmployee.Count > 0)
                    {

                    }

                    db.SaveChanges();
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }

            }

        }
    }
}
