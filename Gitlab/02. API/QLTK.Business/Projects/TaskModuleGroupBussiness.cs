using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.TaskModuleGroup;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.TaskModule
{
    public class TaskModuleGroupBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<TaskModuleGroupResultModel> SearchTaskModuleGroups(TaskModuleGroupSearchModel modelSearch)
        {
            SearchResultModel<TaskModuleGroupResultModel> searchResult = new SearchResultModel<TaskModuleGroupResultModel>();

            var dataQuery = (from a in db.TaskModuleGroups.AsNoTracking()
                             join b in db.Tasks.AsNoTracking() on a.TaskId equals b.Id
                             join c in db.ModuleGroups.AsNoTracking() on a.ModuleGroupId equals c.Id
                             orderby a.Index
                             select new TaskModuleGroupResultModel()
                             {
                                 Id = a.Id,
                                 Index = a.Index,
                                 ModuleGroupName = c.Name,
                                 ModuleGroupId = a.ModuleGroupId,
                                 TaskId = a.TaskId,
                                 TaskName = b.Name,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.ModuleGroupId))
            {
                dataQuery = dataQuery.Where(u => u.ModuleGroupId.Equals(modelSearch.ModuleGroupId));
            }
            if (!string.IsNullOrEmpty(modelSearch.TaskId))
            {
                dataQuery = dataQuery.Where(u => u.TaskId.Equals(modelSearch.TaskId));
            }
            searchResult.TotalItem = dataQuery.Count();

            //var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType)
            //    .Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }
        public TaskModuleGroupModel GetTaskModuleGroupInfo(TaskModuleGroupModel model)
        {
            var resultInfo = db.TaskModuleGroups.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new TaskModuleGroupModel()
            {
                Id = p.Id,
                ModuleGroupId = p.ModuleGroupId,
                TaskId = p.TaskId,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TaskModuleGroup);
            }
            return resultInfo;
        }
        public void CreateTaskModuleGroup(TaskModuleGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    List<TaskModuleGroupTimeStandard> taskModuleGroupTimeStandards = new List<TaskModuleGroupTimeStandard>();
                    var listTaskModuleGroup = db.TaskModuleGroups.Where(a => a.ModuleGroupId.Equals(model.ModuleGroupId)).ToList();
                    if (listTaskModuleGroup.Count > 0)
                    {
                        var list = listTaskModuleGroup.ToList();
                        foreach (var item in model.ListTickTask)
                        {
                            var data = listTaskModuleGroup.FirstOrDefault(i => i.TaskId.Equals(item.Id));
                            if (data != null)
                            {
                                list.Remove(data);
                            }
                        }
                        foreach (var item in list)
                        {
                            var taskModuleGroupTimeStandard = db.TaskModuleGroupTimeStandards.FirstOrDefault(i => i.TaskModuleGroupId.Equals(item.Id));
                            if (taskModuleGroupTimeStandard != null)
                            {
                                taskModuleGroupTimeStandards.Add(taskModuleGroupTimeStandard);
                            }
                            listTaskModuleGroup.Remove(item);
                        }
                        db.TaskModuleGroupTimeStandards.RemoveRange(taskModuleGroupTimeStandards);
                        db.TaskModuleGroups.RemoveRange(list);
                    }

                    foreach (var item in model.ListTickTask)
                    {
                        var data = listTaskModuleGroup.FirstOrDefault(i => i.TaskId.Equals(item.Id));
                        if (data != null)
                        {
                            data.Index = item.Index;
                            var taskModuleGroupTimeStandard = db.TaskModuleGroupTimeStandards.FirstOrDefault(i => i.TaskModuleGroupId.Equals(item.TaskModuleGroupId) && i.Year == DateTime.Now.Year);
                            if (taskModuleGroupTimeStandard != null)
                            {
                                taskModuleGroupTimeStandard.TimeStandard = item.TimeStandard;
                            }
                            else
                            {
                                TaskModuleGroupTimeStandard taskModuleGroupTimeStandar = new TaskModuleGroupTimeStandard()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    TaskModuleGroupId = item.TaskModuleGroupId,
                                    TimeStandard = item.TimeStandard,
                                    Year = DateTime.Now.Year
                                };
                                db.TaskModuleGroupTimeStandards.Add(taskModuleGroupTimeStandar);
                            }
                        }
                        else
                        {
                            NTS.Model.Repositories.TaskModuleGroup newTaskModuleGroup = new NTS.Model.Repositories.TaskModuleGroup
                            {
                                Id = Guid.NewGuid().ToString(),
                                TaskId = item.Id,
                                ModuleGroupId = model.ModuleGroupId,
                                Index = item.Index,
                            };
                            db.TaskModuleGroups.Add(newTaskModuleGroup);

                            TaskModuleGroupTimeStandard taskModuleGroupTimeStandard = new TaskModuleGroupTimeStandard()
                            {
                                Id = Guid.NewGuid().ToString(),
                                TaskModuleGroupId = newTaskModuleGroup.Id,
                                TimeStandard = item.TimeStandard,
                                Year = DateTime.Now.Year
                            };
                            db.TaskModuleGroupTimeStandards.Add(taskModuleGroupTimeStandard);
                        }
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
        public void UpdateTaskModuleGroup(TaskModuleGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newTaskModuleGroup = db.TaskModuleGroups.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();
                    newTaskModuleGroup.TaskId = model.TaskId;
                    newTaskModuleGroup.ModuleGroupId = model.ModuleGroupId;
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
        public void DeleteTaskModuleGroup(TaskModuleGroupModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var _taskModuleGroup = db.TaskModuleGroups.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_taskModuleGroup == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.TaskTimeStandard);
                    }

                    db.TaskModuleGroups.Remove(_taskModuleGroup);
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
