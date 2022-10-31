using NTS.Common;
using NTS.Model.CodeRule;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.CodeRules
{
    public class CodeRuleBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public List<CodeRuleModel> GetCodeRules()
        {
            List<CodeRuleModel> result = new List<CodeRuleModel>();
            result = (from a in db.CodeRules.AsNoTracking()
                      join b in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals b.Id
                      join c in db.MaterialGroupTPAs.AsNoTracking() on a.MaterialGroupTPAId equals c.Id
                      select new CodeRuleModel
                      {
                          Id = a.Id,
                          Code = a.Code,
                          MaterialGroupId = a.MaterialGroupId,
                          MaterialGroupName = b.Name,
                          MaterialGroupTPAId = a.MaterialGroupTPAId,
                          MaterialGroupTPAName = c.Name,
                          Length = a.Length.Value,
                          ManufactureId = a.ManufactureId,
                          UnitId = a.UnitId,
                          Type = a.Type,
                          CreateDate = a.CreateDate
                      }).OrderBy(t => t.CreateDate).ToList();
            return result;
        }

        public List<CodeRuleModel> SearchCodeRule(SearchCodeRuleModel model)
        {
            List<CodeRuleModel> result = new List<CodeRuleModel>();
            result = (from a in db.CodeRules.AsNoTracking()
                      join b in db.MaterialGroups.AsNoTracking() on a.MaterialGroupId equals b.Id
                      join c in db.MaterialGroupTPAs.AsNoTracking() on a.MaterialGroupTPAId equals c.Id
                      select new CodeRuleModel
                      {
                          Id = a.Id,
                          Code = a.Code,
                          MaterialGroupId = a.MaterialGroupId,
                          MaterialGroupName = b.Name,
                          MaterialGroupTPAId = a.MaterialGroupTPAId,
                          MaterialGroupTPAName = c.Name,
                          Length = a.Length.Value,
                          ManufactureId = a.ManufactureId,
                          UnitId = a.UnitId,
                          Type = a.Type,
                          CreateDate = a.CreateDate
                      }).OrderBy(t => t.CreateDate).ToList();
            if (!string.IsNullOrEmpty(model.Code))
            {
                result = result.Where(t => model.Code.Trim().Equals(t.Code, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return result;
        }

        public bool SaveCodeRule(CodeRuleData model, string userId)
        {
            bool rs = false;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    List<CodeRule> adds = new List<CodeRule>();
                    db.CodeRules.RemoveRange(db.CodeRules);

                    if (model.ListModel.Count > 0)
                    {
                        foreach (var item in model.ListModel)
                        {
                            CodeRule entity = new CodeRule()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Code = item.Code,
                                MaterialGroupId = item.MaterialGroupId,
                                MaterialGroupTPAId = item.MaterialGroupTPAId,
                                Length = item.Length,
                                ManufactureId = item.ManufactureId,
                                UnitId = item.UnitId,
                                Type = item.Type,
                                CreateDate = DateTime.Now
                            };
                            adds.Add(entity);

                        }
                        db.CodeRules.AddRange(adds);
                    }

                    UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_CodeRule, string.Empty, string.Empty, string.Empty, string.Empty);

                    db.SaveChanges();
                    trans.Commit();
                    rs = true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            return rs;
        }
    }
}
