using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.HistoryVersion;
using NTS.Model.Repositories;
using NTS.Model.WorkTIme;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business
{
    public class WorkTimeBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<WorkTimeModel> SearchWorkTime(WorkTimeSearchModel modelSearch)
        {
            SearchResultModel<WorkTimeModel> searchResult = new SearchResultModel<WorkTimeModel>();
            var dataQuey = (from a in db.WorkTimes.AsNoTracking()
                            orderby a.Name
                            select new WorkTimeModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                StartTime = a.StartTime,
                                EndTime = a.EndTime,
                            }).AsQueryable();
            // Tên
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            searchResult.TotalItem = dataQuey.Count();
            var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void DeleteWorkTime(WorkTimeModel model,string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var workSkill = db.WorkTimes.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (workSkill == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkTime);
                    }

                    //var jsonApter = AutoMapperConfig.Mapper.Map<WorkTimeHistoryModel>(workSkill);
                    //UserLogUtil.LogHistotyDelete(db, userLoginId, Constants.LOG_TimeWorking, workSkill.Id, workSkill.Name, jsonApter);

                    db.WorkTimes.Remove(workSkill);
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

        public void AddWorkTime(WorkTimeModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                // Check tên nhóm tiêu chí đã tồn tại chưa
                if (db.WorkTimes.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkTime);
                }
                try
                {
                    WorkTime newWorkSkill = new WorkTime()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        StartTime = model.StartTime.NTSTrim(),
                        EndTime = model.EndTime.NTSTrim(),
                    };

                    db.WorkTimes.Add(newWorkSkill);

                    UserLogUtil.LogHistotyAdd(db, userLoginId, newWorkSkill.Name, newWorkSkill.Id, Constants.LOG_TimeWorking);

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
        public WorkTimeModel GetWorkTimeInfo(WorkTimeModel model)
        {          
            try
            {
                var resuldInfor = db.WorkTimes.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new WorkTimeModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    StartTime = p.StartTime,
                    EndTime = p.EndTime,
                }).FirstOrDefault();

                if (resuldInfor == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkTime);
                }

                return resuldInfor;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void UpdateWorkTime(WorkTimeModel model, string userLoginId)
        {
            string nameOld = "";
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.WorkTimes.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Name.Equals(o.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkTime);
                }

                try
                {
                    
                    var groupEdit = db.WorkTimes.AsQueryable().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<WorkTimeHistoryModel>(groupEdit);


                    nameOld = groupEdit.Name.NTSTrim();
                    groupEdit.Name = model.Name.NTSTrim();
                    groupEdit.StartTime = model.StartTime.NTSTrim();
                    groupEdit.EndTime = model.EndTime.NTSTrim();

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkTimeHistoryModel>(groupEdit);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userLoginId, Constants.LOG_WorkingTime, groupEdit.Id, groupEdit.Name, jsonBefor, jsonApter);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
            try
            {
                string decription = String.Empty;
                if (nameOld.ToLower() == model.Name.ToLower())
                {
                    decription = "Cập nhật thời gian làm việc tên là: " + nameOld;
                }
                else
                {
                    decription = "Cập nhật thời gian làm việc có tên ban đầu là:  " + nameOld + " thành " + model.Name; ;
                }
            }
            catch (Exception) { }
        }
    }
}
