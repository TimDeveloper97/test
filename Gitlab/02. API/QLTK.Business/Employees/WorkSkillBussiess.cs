using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Repositories;
using NTS.Model.UserHistory;
using NTS.Model.WorkSkill;
using NTS.Model.WorldSkill;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.WorkSkills
{
    public class WorkSkillBussiess
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<WorkSkillModel> SearchWorkSkill(WorkSkillSearchModel modelSearch)
        {
            SearchResultModel<WorkSkillModel> searchResult = new SearchResultModel<WorkSkillModel>();
            try
            {
                var dataQuey = (from a in db.WorkSkills.AsNoTracking()
                                join b in db.WorkSkillGroups.AsNoTracking() on a.WorkSkillGroupId equals b.Id
                                orderby a.Name
                                select new WorkSkillModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    WorkSkillGroupId = a.WorkSkillGroupId,
                                    Description = a.Description,
                                    WorkSkillGroupName = b.Name,
                                    Score = a.Score,
                                }).AsQueryable();
                // Tên
                if (!string.IsNullOrEmpty(modelSearch.Name))
                {
                    dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
                }
                if (!string.IsNullOrEmpty(modelSearch.WorkSkillGroupId))
                {
                    dataQuey = dataQuey.Where(r => r.WorkSkillGroupId.Equals(modelSearch.WorkSkillGroupId));
                }
                searchResult.TotalItem = dataQuey.Count();
                var listResult = SQLHelpper.OrderBy(dataQuey, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
                searchResult.ListResult = listResult;

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh trong quá trình xử lý" + ex);
            }
            return searchResult;
        }

        public SearchResultModel<WorkSkillGroupModel> SearchWorkSkillGroup(WorkSkillGroupModel modelSearch)
        {
            SearchResultModel<WorkSkillGroupModel> searchResult = new SearchResultModel<WorkSkillGroupModel>();

            var dataQuey = (from a in db.WorkSkillGroups.AsNoTracking()
                            orderby a.Code
                            select new WorkSkillGroupModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Code = a.Code,
                                ParentId = a.ParentId,
                            }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataQuey = dataQuey.Where(r => r.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }
            searchResult.TotalItem = dataQuey.Count();

            searchResult.ListResult = dataQuey.ToList();

            return searchResult;
        }

        public void DeleteWorkSkill(WorkSkillModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.CourseSkills.AsNoTracking().Where(a => a.WorkSkillId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.WorkSkill);
                }
                if (db.EmployeeSkills.AsNoTracking().Where(a => a.WorkSkillId.Equals(model.Id)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.WorkSkill);
                }
                try
                {
                    var workSkill = db.WorkSkills.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (workSkill == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkSkill);
                    }

                    var workTypeSkills = db.WorkTypeSkills.Where(t => model.Id.Equals(t.WorkSkillId));
                    if(workTypeSkills.ToList().Count > 0)
                    {
                        db.WorkTypeSkills.RemoveRange(workTypeSkills);
                    }

                    db.WorkSkills.Remove(workSkill);

                    var NameOrCode = workSkill.Name;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkSkilHistoryModel>(workSkill);
                    //UserLogUtil.LogHistotyDelete(db, model.UpdateBy, Constants.LOG_WorkSkill, workSkill.Id, NameOrCode, jsonBefor);

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

        public void AddWorkSkill(WorkSkillModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                // Check tên nhóm tiêu chí đã tồn tại chưa
                if (db.WorkSkills.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkSkill);
                }
                try
                {
                    WorkSkill newWorkSkill = new WorkSkill()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        WorkSkillGroupId = model.WorkSkillGroupId,
                        Description = model.Description.NTSTrim(),
                        Score = model.Score,
                    };
                    db.WorkSkills.Add(newWorkSkill);

                    UserLogUtil.LogHistotyAdd(db, model.CreateBy, newWorkSkill.Name, newWorkSkill.Id, Constants.LOG_WorkSkill);

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
        public WorkSkillModel GetWorkSkillInfo(WorkSkillModel model)
        {          
            try
            {
                var resuldInfor = db.WorkSkills.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new WorkSkillModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    WorkSkillGroupId = p.WorkSkillGroupId,
                    Description = p.Description,
                    Score = p.Score,
                }).FirstOrDefault();

                if (resuldInfor == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkSkill);
                }

                return resuldInfor;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void UpdateWorkSkill(WorkSkillModel model)
        {
            string nameOld = "";
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.WorkSkills.AsNoTracking().Where(o => !model.Id.Equals(o.Id) && model.Name.Equals(o.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkSkill);
                }
                try
                {
                    var groupEdit = db.WorkSkills.AsQueryable().Where(o => model.Id.Equals(o.Id)).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<WorkSkilHistoryModel>(groupEdit);

                    nameOld = groupEdit.Name.NTSTrim();
                    groupEdit.Name = model.Name.NTSTrim();
                    groupEdit.Score = model.Score;
                    groupEdit.WorkSkillGroupId = model.WorkSkillGroupId;
                    groupEdit.Description = model.Description.NTSTrim();

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkSkilHistoryModel>(groupEdit);

                    //UserLogUtil.LogHistotyUpdateInfo(db, model.UpdateBy, Constants.LOG_WorkSkill, groupEdit.Id, groupEdit.Name, jsonBefor, jsonApter);

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
                    decription = "Cập nhật kỹ năng tên là: " + nameOld;
                }
                else
                {
                    decription = "Cập nhật kỹ năng có tên ban đầu là:  " + nameOld + " thành " + model.Name; ;
                }
            }
            catch (Exception) { }
        }

        public void AddWorkSkillGroup(WorkSkillGroupModel model)
        {
            var existWorkSkillGroup = db.WorkSkillGroups.AsNoTracking().Where(a => a.Code.ToUpper().Equals(model.Code)).FirstOrDefault();
            if (existWorkSkillGroup != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.WorkSkillGroup);
            }

            using (var trans = db.Database.BeginTransaction())
            {

                try
                {
                    if(string.IsNullOrEmpty(model.ParentId))
                    {
                        model.ParentId = null;
                    }
                    WorkSkillGroup newWorkSkillGroup = new WorkSkillGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = model.ParentId,
                        Name = model.Name.NTSTrim(),
                        Code = model.Code.NTSTrim(),
                    };

                    db.WorkSkillGroups.Add(newWorkSkillGroup);


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

        public WorkSkillGroupModel GetWorkSkillGroupInfo(WorkSkillGroupModel model)
        {
            var resultInfo = db.WorkSkillGroups.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(p => new WorkSkillGroupModel
            {
                Id = p.Id,
                Name = p.Name,
                ParentId = p.ParentId,
                Code = p.Code,
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkSkillGroup);
            }

            return resultInfo;
        }

        public void UpdateWWorkSkillGroup(WorkSkillGroupModel model)
        {
            var existWorkSkillGroup = db.WorkSkillGroups.AsNoTracking().Where(a => !a.Id.Equals(model.Id) && a.Code.ToLower().Equals(model.Code.ToLower())).FirstOrDefault();
            if (existWorkSkillGroup != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.WorkSkillGroup);
            }

            var dataQuery = (from a in db.WorkSkillGroups.AsNoTracking()
                             orderby a.Name
                             where a.ParentId != null
                             select new WorkSkillGroupModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentId = a.ParentId,
                                 Code = a.Code
                             }).AsQueryable();

            List<WorkSkillGroupModel> list = new List<WorkSkillGroupModel>();
            list = dataQuery.ToList();

            var listChild = GetChildWorkSkillGroup(model.Id, list);
            listChild.Add(model);
            bool isParentIdOk = true;

            foreach (var item in listChild)
            {
                if (item.Id.Equals(model.ParentId) || item.Id.Equals(model.ParentId))
                {
                    isParentIdOk = false;
                    break;
                }
            }

            if (!isParentIdOk)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0027, TextResourceKey.WorkSkillGroup);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newWorkSkillGroup = db.WorkSkillGroups.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    newWorkSkillGroup.Name = model.Name.NTSTrim();
                    newWorkSkillGroup.ParentId = model.ParentId;
                    newWorkSkillGroup.Code = model.Code.NTSTrim();

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

        public List<WorkSkillGroupModel> GetChildWorkSkillGroup(string parentId, List<WorkSkillGroupModel> list)
        {
            List<WorkSkillGroupModel> result = new List<WorkSkillGroupModel>();
            List<WorkSkillGroupModel> listChild = list.Where(a => a.ParentId.Equals(parentId)).ToList();
            List<WorkSkillGroupModel> listChildChild = new List<WorkSkillGroupModel>();
            foreach (var item in listChild)
            {
                listChildChild = GetChildWorkSkillGroup(item.Id, list);
                result.Add(item);
                result.AddRange(listChildChild);
            }

            return result;

        }

        public void DeleteWorkSkillGroup(WorkSkillGroupModel model)
        {
            var useWorkSkill = db.WorkSkills.AsNoTracking().Where(a => a.WorkSkillGroupId.Equals(model.Id)).FirstOrDefault();
            var useWorkSkillGroup = db.WorkSkillGroups.AsNoTracking().Where(a => a.ParentId.Equals(model.Id)).FirstOrDefault();

            if (useWorkSkill != null || useWorkSkillGroup != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.WorkSkillGroup);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var workSkillGroup = db.WorkSkillGroups.Where(m => m.Id.Equals(model.Id)).FirstOrDefault();
                    if (workSkillGroup != null)
                    {
                        db.WorkSkillGroups.Remove(workSkillGroup);
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

        /// <summary>
        /// Lấy kĩ năng 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<WorkSkillModel> SearchSelectWorkSkill(WorkSkillModel modelSearch)
        {

            SearchResultModel<WorkSkillModel> searchResult = new SearchResultModel<WorkSkillModel>();

            var dataQuery = (from b in db.WorkSkills.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(b.Id)
                             join g in db.WorkSkillGroups.AsNoTracking() on b.WorkSkillGroupId equals g.Id
                             select new WorkSkillModel()
                             {
                                 Id = b.Id,
                                 Name = b.Name,
                                 Description = b.Description,
                                 WorkSkillGroupName = g.Name
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => modelSearch.Name.Equals(t.Name));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }
    }
}
