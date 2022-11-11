using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.SolutionGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.SolutionGroups
{
    public class SolutionGroupBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<SolutionGroupModel> SearchSolutionGroup(SolutionGroupModel modelSearch)
        {
            SearchResultModel<SolutionGroupModel> searchResult = new SearchResultModel<SolutionGroupModel>();

            var dataQuery = (from a in db.SolutionGroups.AsNoTracking()
                             orderby a.Code
                             select new SolutionGroupModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Code = a.Code,
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
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddSolutionGroup(SolutionGroupModel model)
        {
            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    SolutionGroup solutionGroup = new SolutionGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        Code = model.Code.NTSTrim(),
                        Name = model.Name.NTSTrim(),
                        ParentId = model.ParentId
                    };

                    db.SolutionGroups.Add(solutionGroup);
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
        public void UpdateSolutionGroup(SolutionGroupModel model)
        {
            //xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var solutionGroup = db.SolutionGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    solutionGroup.Name = model.Name.NTSTrim();
                    solutionGroup.Code = model.Code.NTSTrim();
                    solutionGroup.ParentId = model.ParentId;
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

        public void DeleteSolutionGroup(SolutionGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var solution = db.Solutions.AsNoTracking().Where(m => m.SolutionGroupId.Equals(model.Id)).FirstOrDefault();
                if (solution != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.SolutionGroup);
                }

                try
                {
                    var solutionGroup = db.SolutionGroups.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (solutionGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SolutionGroup);
                    }

                    db.SolutionGroups.Remove(solutionGroup);
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
        public object GetSolutionGroupInfo(SolutionGroupModel model)
        {
            var resultInfo = db.SolutionGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new SolutionGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                ParentId = p.ParentId,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.SolutionGroup);
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(SolutionGroupModel model)
        {
            if (db.SolutionGroups.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SolutionGroup);
            }

            if (db.SolutionGroups.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SolutionGroup);
            }
        }

        public void CheckExistedForUpdate(SolutionGroupModel model)
        {
            if (db.SolutionGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.SolutionGroup);
            }

            if (db.SolutionGroups.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.SolutionGroup);
            }
        }
    }
}
