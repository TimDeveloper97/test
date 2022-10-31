using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.Skills;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.PacticeSkillBussiness
{
    public class PracticeSkillBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        // getInfor của Kỹ năng theo bài thực hành
        public SearchResultModel<SkillsResultModel> GetSkillInPractice(SkillsSearchModel modelSearch)
        {
            SearchResultModel<SkillsResultModel> searchResult = new SearchResultModel<SkillsResultModel>();
            var dataQuery = (from a in db.PracticeSkills.AsNoTracking()
                             join b in db.Skills.AsNoTracking() on a.SkillId equals b.Id
                             join c in db.SkillGroups.AsNoTracking() on b.SkillGroupId equals c.Id
                             where a.PracticeId.Equals(modelSearch.PracticeId)
                             select new SkillsResultModel
                             {
                                 Id = b.Id,
                                 SkillGroupId = b.SkillGroupId,
                                 SkillGroupName = c.Name,
                                 DegreeName = c.Name,
                                 Code = b.Code,
                                 Name = b.Name,
                                 Description = b.Description,
                                 Note = b.Note,
                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }
        // Chọn Kỹ năng
        public SearchResultModel<SkillsResultModel> SearchSkillInPractice(SkillsSearchModel modelSearch)
        {
            SearchResultModel<SkillsResultModel> searchResult = new SearchResultModel<SkillsResultModel>();
            var dataQuery = (from a in db.Skills.AsNoTracking()
                             join b in db.SkillGroups.AsNoTracking() on a.SkillGroupId equals b.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Code
                             select new SkillsResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 Name = a.Name,
                                 SkillGroupName = b.Name,
                                 Description = a.Description,
                                 Note = a.Note
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }
        public void AddSkillInPractice(SkillsModel model)
        {
            var prac = db.Skills.Where(o => o.Id.Equals(model.PracticeId)).FirstOrDefault();
            using (var trans = db.Database.BeginTransaction())
            {
                var practices = db.PracticeSkills.Where(u => u.PracticeId.Equals(model.PracticeId)).ToList();
                if (practices.Count > 0)
                {
                    db.PracticeSkills.RemoveRange(practices);
                }
                try
                {
                    if (model.listSelect != null)
                    {
                        foreach (var item in model.listSelect)
                        {
                            NTS.Model.Repositories.PracticeSkill practiceSkill = new NTS.Model.Repositories.PracticeSkill
                            {
                                Id = Guid.NewGuid().ToString(),
                                SkillId = item.Id,
                                PracticeId = model.PracticeId,

                            };
                            db.PracticeSkills.Add(practiceSkill);
                        }
                    }
                    UserLogUtil.LogHistotyUpdateSub(db, model.CreateBy, Constants.LOG_Practice, model.PracticeId, string.Empty, "Kỹ năng");

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
