using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.Task;
using NTS.Model.TaskModuleGroupHistory;
using NTS.Utils;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Tasks
{
    public class TasksBussiness
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<TasksResultModel> SearchTasks(TasksSearchModel modelSearch)
        {
            SearchResultModel<TasksResultModel> searchResult = new SearchResultModel<TasksResultModel>();

            var dataQuery = (from a in db.Tasks.AsNoTracking()
                             select new TasksResultModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Type = a.Type,
                                 Description = a.Description,

                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            // Thiết kế
            if (modelSearch.Type > 0)
            {
                dataQuery = dataQuery.Where(u => u.Type.Equals(modelSearch.Type));
            }

            searchResult.TotalItem = dataQuery.Count();

            var listResult = dataQuery.ToList();

            foreach (var item in listResult)
            {
                var a = (from b in db.TaskModuleGroups.AsNoTracking()
                         join c in db.TaskModuleGroupTimeStandards.AsNoTracking() on b.Id equals c.TaskModuleGroupId
                         where b.TaskId.Equals(item.Id) && b.ModuleGroupId.Equals(modelSearch.ModuleGroupId) && c.Year == DateTime.Now.Year
                         select new
                         {
                             b.Id,
                             b.Index,
                             c.TimeStandard,
                         }).FirstOrDefault();
                if (a != null)
                {
                    item.Index = a.Index;
                    item.Checked = true;
                    item.TaskModuleGroupId = a.Id;
                    item.TimeStandard = a.TimeStandard;
                }
            }
            List<TasksResultModel> listIndex = new List<TasksResultModel>();
            List<TasksResultModel> listNoIndex = new List<TasksResultModel>();
            List<TasksResultModel> listTask = new List<TasksResultModel>();

            foreach (var item in listResult)
            {
                listIndex = listResult.Where(a =>  a.Checked == true).OrderBy(a => a.Index).ToList();
                listNoIndex = listResult.Where(a =>  a.Checked == false).OrderBy(a => a.Name).ToList();
            }
            int total = 0;
            if (listIndex.Count > 0)
            {
                total = listIndex.Max(x => x.Index);
            }

            foreach (var it in listNoIndex)
            {
                total++;
                it.Index = total;
            }

            listTask.AddRange(listIndex);
            listTask.AddRange(listNoIndex);

            searchResult.ListResult = listTask;
            return searchResult;
        }
        public TasksModel GetTaskInfo(TasksModel model)
        {
            var resultInfo = db.Tasks.AsNoTracking().Where(u => u.Id.Equals(model.Id)).Select(p => new TasksModel()
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type,
                Description = p.Description,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Task);
            }
            return resultInfo;
        }
        public void CreateTask(TasksModel model)
        {
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    NTS.Model.Repositories.Task newTask = new NTS.Model.Repositories.Task
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = model.Type,
                        Name = model.Name.Trim(),
                        Description = model.Description.Trim(),
                        CreateBy = model.CreateBy,
                        UpdateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                    };

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newTask.Name, newTask.Id, Constants.LOG_Task);

                    db.Tasks.Add(newTask);
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
        public void UpdateTask(TasksModel model)
        {
            CheckExistedForUpdate(model);
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newTask = db.Tasks.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<TaskModuleGroupHistoryModel>(newTask);

                    newTask.Name = model.Name.Trim();
                    newTask.Type = model.Type;
                    newTask.Description = model.Description.Trim();
                    model.UpdateBy = model.UpdateBy;
                    model.UpdateDate = model.UpdateDate;
                    model.CreateBy = model.UpdateBy;
                    //var jsonBefor = AutoMapperConfig.Mapper.Map<TaskModuleGroupHistoryModel>(newTask);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_Task, newTask.Id, newTask.Name, jsonBefor, jsonApter);

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
        public void DeleteTask(TasksModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                //var plan = db.Plans.AsNoTracking().Where(u => u.TaskId.Equals(model.Id)).FirstOrDefault();
                //if (plan != null)
                //{
                //    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Task);
                //}
                var taskModule = db.TaskModuleGroups.AsNoTracking().Where(m => m.TaskId.Equals(model.Id)).FirstOrDefault();
                if (taskModule != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Task);
                }
                try
                {
                    var _task = db.Tasks.FirstOrDefault(a => a.Id.Equals(model.Id));
                    if (_task == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Task);
                    }

                    var NameOrCode = _task.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<TaskModuleGroupHistoryModel>(_task);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_Task, _task.Id, NameOrCode, jsonBefor);
                    db.Tasks.Remove(_task);
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
        private void CheckExistedForAdd(TasksModel model)
        {
            if (db.Tasks.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Task);
            }
        }
        public void CheckExistedForUpdate(TasksModel model)
        {
            if (db.Tasks.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Name.Equals(model.Name)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Task);
            }
        }

    }
}
