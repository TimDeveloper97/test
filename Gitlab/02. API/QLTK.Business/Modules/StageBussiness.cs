using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.Stage;
using System;
using System.Linq;

namespace QLTK.Business.Stage
{
    public class StageBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public SearchResultModel<StageModel> SearchStage(StageSearchModel modelSearch)
        {
            SearchResultModel<StageModel> searchResult = new SearchResultModel<StageModel>();

            var dataQuery = (from a in db.Stages.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                             orderby a.index
                             select new StageModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                                 DepartmentId = b.Id,
                                 DepartmentName = b.Name,
                                 SBUId = c.Id,
                                 SBUName = c.Name,
                                 Time = a.Time,
                                 Code = a.Code,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                                 IsEnable = a.IsEnable,
                                 Color = a.Color,
                                 index = a.index,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            searchResult.TotalItem = dataQuery.Count();

            //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        public SearchResultModel<StageModel> SearchListStage(string projectProductId)
        {
            var listPlan = db.Plans.Where(a => a.ProjectProductId.Equals(projectProductId)).Select(a => a.StageId).ToList();

            SearchResultModel<StageModel> searchResult = new SearchResultModel<StageModel>();
            var dataQuery = (from a in db.Stages.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                             where !listPlan.Contains(a.Id) && a.IsEnable
                             orderby a.index
                             select new StageModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Note = a.Note,
                                 DepartmentId = b.Id,
                                 DepartmentName = b.Name,
                                 SBUId = c.Id,
                                 SBUName = c.Name,
                                 Time = a.Time,
                                 Code = a.Code,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                                 IsEnable = a.IsEnable,
                                 Color = a.Color,
                                 index = a.index,
                             }).AsQueryable();


            searchResult.TotalItem = dataQuery.Count();
            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddStage(StageModel model)
        {
            if (db.Stages.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Stage);
            }


            var stage = db.Stages.AsNoTracking().ToList();


            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    NTS.Model.Repositories.Stage newSBU = new NTS.Model.Repositories.Stage
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        Code = model.Code.NTSTrim(),
                        DepartmentId = model.DepartmentId,
                        Time = model.Time,
                        Note = model.Note.NTSTrim(),
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now,
                        IsEnable = model.IsEnable,
                        Color = model.Color,
                        index = stage.Max(r => r.index) + 1,

                    };

                    db.Stages.Add(newSBU);
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
        public void UpdateStage(StageModel model, bool isCheckPermission, string depatermentId)
        {
            if (db.Stages.AsNoTracking().Where(o => o.Name.Equals(model.Name) && !o.Id.Equals(model.Id)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Stage);
            }



            //xoá ký tự đặc biệt
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newSBU = db.Stages.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                    if (!isCheckPermission && !newSBU.DepartmentId.Equals(depatermentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0032, TextResourceKey.Stage);
                    }

                    newSBU.Name = model.Name.NTSTrim();
                    newSBU.Note = model.Note.NTSTrim();
                    newSBU.Code = model.Code.NTSTrim();
                    newSBU.UpdateBy = model.UpdateBy;
                    newSBU.Time = model.Time;
                    newSBU.DepartmentId = model.DepartmentId;
                    newSBU.UpdateDate = DateTime.Now;
                    newSBU.IsEnable = model.IsEnable;
                    newSBU.Color = model.Color;

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

        public void DeleteStage(StageModel model, bool isCheckPermission, string depatermentId)
        {

            var stageModule = db.ModuleGroupStages.AsNoTracking().Where(m => m.StageId.Equals(model.Id)).FirstOrDefault();
            if (stageModule != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Stage);
            }

            var stageError = db.Errors.AsNoTracking().Where(m => m.StageId.Equals(model.Id)).FirstOrDefault();
            if (stageError != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Stage);
            }


            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var stages = db.Stages.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (stages == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Stage);
                    }

                    if (!isCheckPermission && !stages.DepartmentId.Equals(depatermentId))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0032, TextResourceKey.Stage);
                    }

                    db.Stages.Remove(stages);

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
        public object GetStageInfo(StageModel model, string departmentId, bool isViewOtherDepartment)
        {
            var resultInfo = db.Stages.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new StageModel
            {
                Id = p.Id,
                Name = p.Name,
                Note = p.Note,
                DepartmentId = p.DepartmentId,
                Code = p.Code,
                Time = p.Time,
                Color = p.Color,
                IsEnable = p.IsEnable,
            }).FirstOrDefault();

            if (!resultInfo.DepartmentId.Equals(departmentId) && !isViewOtherDepartment)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0030, TextResourceKey.Stage);
            }

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Stage);
            }

            return resultInfo;
        }

        public void createIndex(StageSearchModel model)
        {
            for (var i = 0; i < model.ListStage.Count; i++)
            {
                string id = model.ListStage[i].Id;
                var stage = db.Stages.Where(r => r.Id.Equals(id)).FirstOrDefault();
                if (stage != null)
                {
                    stage.index = i;
                }
            }



            db.SaveChanges();

        }
    }
}
