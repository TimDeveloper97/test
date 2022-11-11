using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Document;
using NTS.Model.Materials;
using NTS.Model.Question;
using NTS.Model.Repositories;
using NTS.Model.SkillEmployee;
using NTS.Model.TaskFlowStage;
using NTS.Model.UserHistory;
using NTS.Model.WorkType;
using NTS.Model.WorkTypeSkill;
using NTS.Model.WorldSkill;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.WorkTypes
{
    public class WorkTypeBussiess
    {
        private QLTKEntities db = new QLTKEntities();
        public SearchResultModel<WorkTypeSearchResultModel> SearchWorkType(WorkTypeSearchModel modelSearch)
        {
            SearchResultModel<WorkTypeSearchResultModel> searchResult = new SearchResultModel<WorkTypeSearchResultModel>();

            var dataQuey = (from a in db.WorkTypes.AsNoTracking()
                            join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                            from adn in ad.DefaultIfEmpty()
                            join u in db.SBUs.AsNoTracking() on a.SBUId equals u.Id into au
                            from aun in au.DefaultIfEmpty()
                            join f in db.FlowStages.AsNoTracking() on a.FlowStageId equals f.Id into af
                            from afn in af.DefaultIfEmpty()
                            orderby a.Code
                            select new WorkTypeSearchResultModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Code = a.Code,
                                Quantity = a.Quantity,
                                DepartmentId = a.DepartmentId,
                                DepartmentName = adn != null ? adn.Name : string.Empty,
                                SBUId = a.SBUId,
                                SBUName = aun != null ? aun.Name : string.Empty,
                                FlowStageId = a.FlowStageId,
                                FlowStageName = afn != null ? afn.Name : string.Empty,
                            }).AsQueryable();
            // Tên
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuey = dataQuey.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()) || r.Code.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuey = dataQuey.Where(r => modelSearch.DepartmentId.Equals(r.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuey = dataQuey.Where(r => modelSearch.SBUId.Equals(r.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.FlowStageId))
            {
                dataQuey = dataQuey.Where(r => modelSearch.FlowStageId.Equals(r.FlowStageId));
            }

            searchResult.TotalItem = dataQuey.Count();
            var listResult = dataQuey.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;

            foreach (var item in searchResult.ListResult)
            {
                item.Quantity = db.Employees.AsNoTracking().Count(r => item.Id.Equals(r.WorkTypeId) && r.Status == Constants.Employee_Status_Use);
            }

            return searchResult;
        }

        public void DeleteWorkType(WorkTypeModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var employee = db.Employees.AsNoTracking().Where(a => a.WorkTypeId.Equals(model.Id)).ToList();

                if (employee.Count > 0)
                {
                    throw NTSException.CreateInstance($"Vị trí công việc đang được sử dụng cho nhân sự: {employee.FirstOrDefault().Code} - {employee.FirstOrDefault().Name}");
                }
                try
                {
                    var workType = db.WorkTypes.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (workType == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkType);
                    }

                    var task = db.Tasks.FirstOrDefault(a => a.WorkTypeRId.Equals(model.Id) || a.WorkTypeAId.Equals(model.Id) || a.WorkTypeCId.Equals(model.Id) || a.WorkTypeSId.Equals(model.Id) || a.WorkTypeIId.Equals(model.Id));
                    if (task != null)
                    {
                        throw NTSException.CreateInstance($"Vị trí công việc đang được sử dụng trong Công việc: {task.Code} - {task.Name}");
                    }

                    var CandidateWorkHistories = db.CandidateWorkHistories.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    db.CandidateWorkHistories.RemoveRange(CandidateWorkHistories);

                    var CandidateWorkTypeFit = db.CandidateWorkTypeFits.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    db.CandidateWorkTypeFits.RemoveRange(CandidateWorkTypeFit);

                    var EmployeeHistoryJobTranfer = db.EmployeeHistoryJobTranfers.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    db.EmployeeHistoryJobTranfers.RemoveRange(EmployeeHistoryJobTranfer);

                    var EmployeeWorkHistory = db.EmployeeWorkHistories.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    db.EmployeeWorkHistories.RemoveRange(EmployeeWorkHistory);

                    var RecruitmentRequest = db.RecruitmentRequests.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    if (RecruitmentRequest.Count > 0)
                    {
                        throw NTSException.CreateInstance($"Vị trí công việc đang được sử dụng trong yêu cầu tuyển dụng: {RecruitmentRequest.FirstOrDefault().Code}");
                    }

                    var workTypeInterview = db.WorkTypeInterviews.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    foreach (var item in workTypeInterview)
                    {
                        var questions = db.WorkTypeInterviewQuestions.Where(a => a.WorkTypeInterviewId.Equals(item.Id)).ToList();
                        db.WorkTypeInterviewQuestions.RemoveRange(questions);
                    }
                    db.WorkTypeInterviews.RemoveRange(workTypeInterview);

                    var workTypeSkill = db.WorkTypeSkills.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    db.WorkTypeSkills.RemoveRange(workTypeSkill);

                    var workTypeDocument = db.WorkTypeDocuments.Where(a => a.WorkTypeId.Equals(model.Id)).ToList();
                    db.WorkTypeDocuments.RemoveRange(workTypeDocument);

                    var NameOrCode = workType.Name;

                    db.WorkTypes.Remove(workType);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public void AddWorkType(WorkTypeModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                if (db.WorkTypes.AsNoTracking().Where(o => o.Name.Equals(model.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkSkill);
                }

                try
                {
                    WorkType newWorkType = new WorkType()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name.NTSTrim(),
                        Quantity = model.Quantity,
                        Code = model.Code.NTSTrim(),
                        DepartmentId = model.DepartmentId,
                        SBUId = model.SBUId,
                        FlowStageId = model.FlowStageId,
                        SalaryLevelMinId = model.SalaryLevelMinId,
                        SalaryLevelMaxId = model.SalaryLevelMaxId,
                        SalaryGroupId = model.SalaryGroupId,
                        SalaryTypeId = model.SalaryTypeId,
                        Value = model.Value,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };
                    db.WorkTypes.Add(newWorkType);

                    UserLogUtil.LogHistotyAdd(db, userId, newWorkType.Name, newWorkType.Id.ToString(), Constants.LOG_WorkType);

                    if (model.ListWorkTypeSkill.Count > 0)
                    {
                        List<WorkTypeSkill> listWorkTypeSkill = new List<WorkTypeSkill>();
                        foreach (var it in model.ListWorkTypeSkill)
                        {
                            WorkTypeSkill works = new WorkTypeSkill()
                            {
                                Id = Guid.NewGuid().ToString(),
                                WorkTypeId = newWorkType.Id,
                                WorkSkillId = it.Id,
                            };
                            listWorkTypeSkill.Add(works);
                        }
                        db.WorkTypeSkills.AddRange(listWorkTypeSkill);
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
        public WorkTypeModel GetWorkTypeInfo(WorkTypeModel model)
        {
            try
            {
                var resultInfor = db.WorkTypes.AsNoTracking().Where(u => model.Id == u.Id).Select(p => new WorkTypeModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Quantity = p.Quantity,
                    FlowStageId = p.FlowStageId,
                    SBUId = p.SBUId,
                    DepartmentId = p.DepartmentId,
                    SalaryLevelMinId = p.SalaryLevelMinId,
                    SalaryLevelMaxId = p.SalaryLevelMaxId,
                    SalaryTypeId = p.SalaryTypeId,
                    SalaryGroupId = p.SalaryGroupId,
                    Value = p.Value
                }).FirstOrDefault();

                if (resultInfor == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.WorkType);
                }

                resultInfor.Quantity = db.Employees.AsNoTracking().Count(r => resultInfor.Id.Equals(r.WorkTypeId) && r.Status == Constants.Employee_Status_Use);

                var listWorkTypeSkill = (from a in db.WorkSkills.AsNoTracking()
                                         join b in db.WorkTypeSkills.AsNoTracking() on a.Id equals b.WorkSkillId
                                         join c in db.WorkTypes.AsNoTracking() on b.WorkTypeId equals c.Id
                                         where c.Id.Equals(model.Id)
                                         select new WorkTypeSkillModel()
                                         {
                                             Id = a.Id,
                                             Name = a.Name,
                                         }).ToList();

                resultInfor.ListWorkTypeSkill = listWorkTypeSkill;

                var taskRs = (from a in db.Tasks.AsNoTracking()
                              where a.WorkTypeRId.Equals(resultInfor.Id)
                              select a.Id).ToList();

                List<MaterialResultModel> materials = new List<MaterialResultModel>();
                List<WorkTypeDocumentModel> documents = new List<WorkTypeDocumentModel>();

                //lấy danh sách công cụ
                materials.AddRange((from a in db.TaskMaterils.AsNoTracking()
                                    join b in db.Materials.AsNoTracking() on a.MaterialId equals b.Id
                                    join c in db.MaterialGroups.AsNoTracking() on b.MaterialGroupId equals c.Id
                                    join d in db.Manufactures.AsNoTracking() on b.ManufactureId equals d.Id
                                    where taskRs.Contains(a.TaskId)
                                    select new MaterialResultModel
                                    {
                                        Id = b.Id,
                                        Name = b.Name,
                                        Code = b.Code,
                                        MaterialGroupName = c.Name,
                                        ManufactureCode = d.Name,
                                        Pricing = b.Pricing,
                                        Quantity = a.Quantity
                                    }).ToList());

                materials = materials.GroupBy(g => new { g.Name, g.Id, g.Code, g.MaterialGroupName, g.ManufactureCode, g.Pricing })
                    .Select(s => new MaterialResultModel
                    {
                        Name = s.Key.Name,
                        Code = s.Key.Code,
                        MaterialGroupName = s.Key.Name,
                        ManufactureCode = s.Key.Name,
                        Pricing = s.Key.Pricing,
                        Quantity = s.Max(m => m.Quantity)
                    }).ToList();

                resultInfor.Materials = materials;

                resultInfor.Tasks = (from a in db.Tasks.AsNoTracking()
                                     where a.WorkTypeRId.Equals(resultInfor.Id) || a.WorkTypeAId.Equals(resultInfor.Id) || a.WorkTypeSId.Equals(resultInfor.Id)
                                     || a.WorkTypeCId.Equals(resultInfor.Id) | a.WorkTypeIId.Equals(resultInfor.Id)
                                     select new TaskFlowStageModel
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Code = a.Code,
                                         WorkTypeSId = a.WorkTypeSId,
                                         WorkTypeAId = a.WorkTypeAId,
                                         WorkTypeCId = a.WorkTypeCId,
                                         WorkTypeIId = a.WorkTypeIId,
                                         WorkTypeRId = a.WorkTypeRId,
                                     }).ToList();

                documents = (from a in db.DocumentObjects.AsNoTracking()
                             join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                             join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                             join d in db.DocumentTypes.AsNoTracking() on b.DocumentTypeId equals d.Id
                             where a.ObjectId.Equals(model.Id)
                             select new WorkTypeDocumentModel
                             {
                                 Id = b.Id,
                                 Name = b.Name,
                                 Code = b.Code,
                                 DocumentGroupName = c.Name,
                                 DocumentTypeName = d.Name,
                                 IsDocumentOfTask = a.ObjectType == Constants.ObjectType_Work ? true : false
                             }).ToList();

                foreach (var item in resultInfor.Tasks)
                {
                    //Lấy danh sách tài liệu từ task
                    var documentOfTask = (from a in db.DocumentObjects.AsNoTracking()
                                          join b in db.Documents.AsNoTracking() on a.DocumentId equals b.Id
                                          join c in db.DocumentGroups.AsNoTracking() on b.DocumentGroupId equals c.Id
                                          join d in db.DocumentTypes.AsNoTracking() on b.DocumentTypeId equals d.Id
                                          where a.ObjectId.Equals(item.Id) || a.ObjectId.Equals(resultInfor.Id)
                                          select new WorkTypeDocumentModel
                                          {
                                              Id = b.Id,
                                              Name = b.Name,
                                              Code = b.Code,
                                              DocumentGroupName = c.Name,
                                              DocumentTypeName = d.Name,
                                              IsDocumentOfTask = a.ObjectType == Constants.ObjectType_Work ? true : false
                                          }).ToList();

                    foreach (var document in documentOfTask)
                    {
                        var documentExist = documents.FirstOrDefault(a => a.Id.Equals(document.Id));
                        if (documentExist == null)
                        {
                            documents.Add(document);
                        }
                        else
                        {
                            documentExist.TaskName += string.IsNullOrEmpty(documentExist.TaskName) ? item.Name : ";" + item.Name;
                        }
                    }
                }

                resultInfor.Documents = documents;
                foreach (var item in resultInfor.Documents)
                {
                    item.ListFile = (from a in db.DocumentFiles.AsNoTracking()
                                     where a.DocumentId.Equals(item.Id)
                                     select new DocumentFileModel
                                     {
                                         Id = a.Id,
                                         FileName = a.FileName,
                                         FileSize = a.FileSize,
                                         Path = a.Path
                                     }).ToList();
                }

                return resultInfor;
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        public void UpdateWorkType(WorkTypeModel model, string userId)
        {
            string nameOld = "";
            using (var trans = db.Database.BeginTransaction())
            {
                if (db.WorkTypes.AsNoTracking().Where(o => !(model.Id == o.Id) && model.Name.Equals(o.Name)).Count() > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.WorkSkill);
                }
                try
                {
                    var workTypeEdit = db.WorkTypes.Where(o => model.Id == o.Id).FirstOrDefault();

                    //var jsonApter = AutoMapperConfig.Mapper.Map<WorkTypeHistoryModel>(workTypeEdit);

                    nameOld = workTypeEdit.Name.NTSTrim();
                    workTypeEdit.Name = model.Name.NTSTrim();
                    workTypeEdit.Quantity = model.Quantity;
                    workTypeEdit.Code = model.Code;
                    workTypeEdit.Value = model.Value;
                    workTypeEdit.SBUId = model.SBUId;
                    workTypeEdit.DepartmentId = model.DepartmentId;
                    workTypeEdit.SalaryLevelMinId = model.SalaryLevelMinId;
                    workTypeEdit.SalaryLevelMaxId = model.SalaryLevelMaxId;
                    workTypeEdit.SalaryGroupId = model.SalaryGroupId;
                    workTypeEdit.SalaryTypeId = model.SalaryTypeId;
                    workTypeEdit.FlowStageId = model.FlowStageId;
                    workTypeEdit.UpdateBy = userId;
                    workTypeEdit.UpdateDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<WorkTypeHistoryModel>(workTypeEdit);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_WorkType, workTypeEdit.Id, workTypeEdit.Name, jsonBefor, jsonApter);

                    var workTypeSkill = db.WorkTypeSkills.Where(t => t.WorkTypeId.Equals(model.Id));
                    if (workTypeSkill != null)
                    {
                        // Xóa list kĩ năng theo Id
                        db.WorkTypeSkills.RemoveRange(workTypeSkill);
                        if (model.ListWorkTypeSkill.Count > 0)
                        {
                            List<WorkTypeSkill> listCourseSkills = new List<WorkTypeSkill>();
                            foreach (var it in model.ListWorkTypeSkill)
                            {
                                WorkTypeSkill works = new WorkTypeSkill()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    WorkTypeId = workTypeEdit.Id,
                                    WorkSkillId = it.Id,
                                };
                                listCourseSkills.Add(works);
                            }
                            db.WorkTypeSkills.AddRange(listCourseSkills);
                        }
                    }

                    //Update tài liệu
                    var documentsExist = db.DocumentObjects.Where(a => a.ObjectId.Equals(model.Id)).ToList();
                    db.DocumentObjects.RemoveRange(documentsExist);
                    foreach (var item in model.Documents)
                    {
                        if (!item.IsDocumentOfTask)
                        {
                            DocumentObject newDocumentObject = new DocumentObject()
                            {
                                Id = Guid.NewGuid().ToString(),
                                DocumentId = item.Id,
                                ObjectId = model.Id,
                                ObjectType = Constants.ObjectType_WorkType
                            };
                            db.DocumentObjects.Add(newDocumentObject);
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
            try
            {
                string decription = String.Empty;
                if (nameOld.ToLower() == model.Name.ToLower())
                {
                    decription = "Cập nhật loại công việc tên là: " + nameOld;
                }
                else
                {
                    decription = "Cập nhật loại công việc có tên ban đầu là:  " + nameOld + " thành " + model.Name; ;
                }
            }
            catch (Exception) { }
        }

        public SearchResultModel<WorkSkillModel> SearchWorkSkill(WorkSkillModel modelSearch)
        {
            SearchResultModel<WorkSkillModel> searchResult = new SearchResultModel<WorkSkillModel>();
            var dataQuery = (from a in db.WorkSkills.AsNoTracking()
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             orderby a.Name
                             select new WorkSkillModel()
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                             }).AsQueryable();

            // Tìm kiếm theo mã khóa học
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => t.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();
            return searchResult;
        }

        /// <summary>
        /// Lấy kĩ năng 
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<WorkSkillModel> SearchEmployeeWorkSkill(WorkSkillModel modelSearch)
        {

            SearchResultModel<WorkSkillModel> searchResult = new SearchResultModel<WorkSkillModel>();

            var dataQuery = (from a in db.WorkTypeSkills.AsNoTracking()
                             join b in db.WorkSkills.AsNoTracking() on a.WorkSkillId equals b.Id
                             join c in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals c.Id
                             join d in db.WorkSkillGroups.AsNoTracking() on b.WorkSkillGroupId equals d.Id
                             where !modelSearch.ListIdSelect.Contains(a.Id)
                             select new WorkSkillModel()
                             {
                                 Id = a.Id,
                                 WorkSkillId = b.Id,
                                 Name = b.Name,
                                 WorkTypeId = c.Id,
                                 WorkTypeName = d.Name,
                                 Description = b.Description,
                                 Score = b.Score
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(t => modelSearch.Name.Equals(t.Name));
            }

            if (modelSearch.WorkTypeId != null)
            {
                dataQuery = dataQuery.Where(t => modelSearch.WorkTypeId == t.WorkTypeId);

            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }
    }
}
