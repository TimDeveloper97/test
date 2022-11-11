using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Model.Applys;
using NTS.Model.Candidates;
using NTS.Model.Combobox;
using NTS.Model.Recruitments.Applys;
using NTS.Model.Recruitments.Candidates;
using NTS.Model.Recruitments.Interviews;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Recruitments
{
    public class InterviewBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm ứng tuyển
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        //public SearchResultModel<InterviewSearchResultsModel> SearchInterviews(InterviewSearchModel searchModel)
        //{
        //    SearchResultModel<InterviewSearchResultsModel> searchResult = new SearchResultModel<InterviewSearchResultsModel>();
        //    DateTime dateToday = DateTime.Now.Date;
        //    var dataQuery = (from a in db.CandidateApplies.AsNoTracking()
        //                     orderby a.ApplyDate descending
        //                     join c in db.Candidates.AsNoTracking() on a.CandidateId equals c.Id
        //                     join t in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals t.Id
        //                     join d in db.Interviews.AsNoTracking() on a.Id equals d.CandidateApplyId into ad from da in ad.DefaultIfEmpty()
        //                     where a.InterviewDate.HasValue && a.ProfileStatus == Constants.CandidateApply_ProfileStatus && a.InterviewStatus != 2
        //                     orderby a.ApplyDate descending
        //                     select new InterviewSearchResultsModel
        //                     {
        //                         Id = a.Id,
        //                         ImagePath = c.ImagePath,
        //                         Name = c.Name,
        //                         Code = c.Code,
        //                         PhoneNumber = c.PhoneNumber,
        //                         Email = c.Email,
        //                         ApplyDate = a.ApplyDate,
        //                         InterviewDate = a.InterviewDate,
        //                         ProfileStatus = a.ProfileStatus,
        //                         Salary = a.Salary,
        //                         WorkTypeName = t.Name,
        //                         WorkTypeId = a.WorkTypeId,
        //                         EmployeeId = da.InterviewBy,
        //                     }).AsQueryable();

        //    if (!string.IsNullOrEmpty(searchModel.Name))
        //    {
        //        dataQuery = dataQuery.Where(r => r.Name.ToLower().Contains(searchModel.Name.ToLower()) || r.Code.ToLower().Contains(searchModel.Name.ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(searchModel.EmployeeId))
        //    {
        //        dataQuery = dataQuery.Where(r => r.EmployeeId.ToLower().Contains(searchModel.EmployeeId.ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
        //    {
        //        dataQuery = dataQuery.Where(r => r.PhoneNumber.ToLower().Contains(searchModel.PhoneNumber.ToLower()));
        //    }

        //    if (!string.IsNullOrEmpty(searchModel.WorkTypeId))
        //    {
        //        dataQuery = dataQuery.Where(r => r.WorkTypeId.Equals(searchModel.WorkTypeId));
        //    }

        //    if (!string.IsNullOrEmpty(searchModel.Email))
        //    {
        //        dataQuery = dataQuery.Where(r => r.Email.ToLower().Contains(searchModel.Email.ToLower()));
        //    }

        //    if (searchModel.ProfileStatus.HasValue)
        //    {
        //        dataQuery = dataQuery.Where(r => r.ProfileStatus == searchModel.ProfileStatus);
        //    }

        //    if (searchModel.ApplyDateTo.HasValue)
        //    {
        //        dataQuery = dataQuery.Where(r => r.ApplyDate >= searchModel.ApplyDateTo);
        //    }

        //    if (searchModel.ApplyDateFrom.HasValue)
        //    {
        //        DateTime applyDateFrom = searchModel.ApplyDateFrom.Value.ToEndDate();
        //        dataQuery = dataQuery.Where(r => r.ApplyDate <= applyDateFrom);
        //    }

        //    if (searchModel.InterviewDateTo.HasValue)
        //    {
        //        dataQuery = dataQuery.Where(r => r.InterviewDate <= searchModel.InterviewDateTo);
        //    }

        //    if (searchModel.InterviewDateFrom.HasValue)
        //    {
        //        dataQuery = dataQuery.Where(r => r.InterviewDate >= searchModel.InterviewDateFrom);
        //    }

        //    searchResult.TotalItem = dataQuery.Select(s => s.Id).Count();

        //    var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
        //    searchResult.ListResult = listResult;

        //    foreach (var item in searchResult.ListResult)
        //    {
        //        var interviewCount = (from a in db.Interviews.AsNoTracking()
        //                              where a.CandidateApplyId == item.Id
        //                              select new
        //                              {
        //                                  Id = a.Id,
        //                              }).Count();
        //        item.Count = interviewCount;
        //    }
        //    if (searchModel.Count != null)
        //    {
        //        listResult = listResult.Where(r => r.Count.Equals(searchModel.Count)).ToList();
        //    }
        //    return searchResult;
        //}

        public SearchResultModel<InterviewSearchResultsModel> SearchInterviews(InterviewSearchModel searchModel)
        {
            SearchResultModel<InterviewSearchResultsModel> searchResult = new SearchResultModel<InterviewSearchResultsModel>();
            var dataQuery = (from a in db.Interviews.AsNoTracking()
                             join b in db.CandidateApplies on a.CandidateApplyId equals b.Id
                             orderby b.ApplyDate descending
                             join c in db.Candidates.AsNoTracking() on b.CandidateId equals c.Id
                             join t in db.WorkTypes.AsNoTracking() on b.WorkTypeId equals t.Id
                             orderby a.InterviewDate descending
                             select new InterviewSearchResultsModel
                             {
                                 Id = a.Id,
                                 CandidateApplyId = a.CandidateApplyId,
                                 ImagePath = c.ImagePath,
                                 Name = c.Name,
                                 Code = c.Code,
                                 PhoneNumber = c.PhoneNumber,
                                 Email = c.Email,
                                 ApplyDate = b.ApplyDate,
                                 InterviewDate = a.InterviewDate,
                                 ProfileStatus = b.ProfileStatus,
                                 MinAppySalary = b.MinApplySalary,
                                 MaxAppySalary = b.MaxApplySalary,
                                 WorkTypeName = t.Name,
                                 WorkTypeId = b.WorkTypeId,
                                 EmployeeId = a.InterviewBy,
                                 Status = a.Status,
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(r => r.Name.ToLower().Contains(searchModel.Name.ToLower()) || r.Code.ToLower().Contains(searchModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(r => r.EmployeeId.ToLower().Contains(searchModel.EmployeeId.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
            {
                dataQuery = dataQuery.Where(r => r.PhoneNumber.ToLower().Contains(searchModel.PhoneNumber.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.WorkTypeId))
            {
                dataQuery = dataQuery.Where(r => r.WorkTypeId.Equals(searchModel.WorkTypeId));
            }

            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                dataQuery = dataQuery.Where(r => r.Email.ToLower().Contains(searchModel.Email.ToLower()));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.Status == searchModel.Status);
            }

            if (searchModel.ApplyDateTo.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.ApplyDate >= searchModel.ApplyDateTo);
            }

            if (searchModel.ApplyDateFrom.HasValue)
            {
                DateTime applyDateFrom = searchModel.ApplyDateFrom.Value.ToEndDate();
                dataQuery = dataQuery.Where(r => r.ApplyDate <= applyDateFrom);
            }

            if (searchModel.InterviewDateTo.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.InterviewDate <= searchModel.InterviewDateTo);
            }

            if (searchModel.InterviewDateFrom.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.InterviewDate >= searchModel.InterviewDateFrom);
            }

            searchResult.TotalItem = dataQuery.Select(s => s.Id).Count();
            var listResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            searchResult.ListResult = listResult;

            foreach (var item in searchResult.ListResult)
            {
                item.MainInterviewer = db.Employees.FirstOrDefault(r => r.Id.Equals(item.EmployeeId)).Name;
                var InterviewerSub = (from r in db.InterviewUsers.AsNoTracking()
                                      where r.InterviewId.Equals(item.Id)
                                      join b in db.Users on r.UserId equals b.Id
                                      join c in db.Employees on b.EmployeeId equals c.Id
                                      select c.Name).ToList();
                item.SubInterviewer = String.Join(", ", InterviewerSub);


            }


            if (searchModel.Count != null)
            {
                listResult = listResult.Where(r => r.Count.Equals(searchModel.Count)).ToList();
            }

            return searchResult;
        }

        public List<InterviewSearchResultsModel> GetInfoInterviewByRecruitmentRequestId(string Id)
        {
            var interviews = (from a in db.CandidateApplies.AsNoTracking()
                              orderby a.ApplyDate descending
                              join b in db.Interviews.AsNoTracking() on a.Id equals b.CandidateApplyId
                              join c in db.Candidates.AsNoTracking() on a.CandidateId equals c.Id
                              join t in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals t.Id
                              where a.RecruitmentRequestId == Id
                              orderby a.ApplyDate descending
                              select new InterviewSearchResultsModel
                              {
                                  Id = b.Id,
                                  ImagePath = c.ImagePath,
                                  Name = c.Name,
                                  Code = c.Code,
                                  PhoneNumber = c.PhoneNumber,
                                  Email = c.Email,
                                  ApplyDate = a.ApplyDate,                                                                      
                                  //InterviewDate = a.InterviewDate,
                                  ProfileStatus = a.ProfileStatus,
                                  MinAppySalary = a.MinApplySalary,
                                  MaxAppySalary = a.MaxApplySalary,
                                  WorkTypeName = t.Name,
                                  WorkTypeId = a.WorkTypeId,

                              }).ToList();

            return interviews;
        }

        /// <summary>
        /// Cập nhật ứng tuyển
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="applyCreateModel"></param>
        /// <param name="userId"></param>
        public void CreateInterview(InterviewModel interviewModel, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (interviewModel.Status == 2)
                    {
                        var candidate = (from a in db.Candidates.AsNoTracking()
                                         join b in db.CandidateApplies.AsNoTracking() on a.Id equals b.CandidateId
                                         where b.Id == interviewModel.Apply.Id
                                         select b).FirstOrDefault();
                        if (candidate != null)
                        {
                            //candidate.InterviewStatus = interviewModel.Status;
                            //var candi = db.Candidates.FirstOrDefault(c => c.Id.Equals(candidate.CandidateId));
                            //candi.InterviewStatus = interviewModel.Status;
                            //var capp = db.CandidateApplies.FirstOrDefault(ca => ca.Id.Equals(candidate.Id));
                            //capp.InterviewStatus = interviewModel.Status;
                        }

                        //var interviews = db.Interviews.FirstOrDefault(t => t.Id.Equals(interviewModel.InterviewId));
                        //if (interviews != null)
                        //{
                        //    db.Interviews.Remove(interviews);

                        //}
                        //Interview interview = new Interview()
                        //{
                        //    CandidateApplyId = interviewModel.Apply.Id,
                        //    CreateBy = userId,
                        //    DepartmentId = interviewModel.DepartmentId,
                        //    InterviewBy = interviewModel.EmployeeId,
                        //    Id = Guid.NewGuid().ToString(),
                        //    CreateDae = DateTime.Now,
                        //    Name = interviewModel.Name,
                        //    SBUId = interviewModel.SBUId,
                        //    Status = interviewModel.Status,
                        //    Comment = interviewModel.Comment,
                        //    InterviewDate = DateTime.Now,
                        //    UpdateBy = userId,
                        //    UpdateDate = DateTime.Now
                        //};
                        var interview = db.Interviews.Where(r => r.Id.Equals(interviewModel.InterviewId)).FirstOrDefault();

                        interview.CandidateApplyId = interviewModel.Apply.Id;
                        interview.DepartmentId = interviewModel.DepartmentId;
                        interview.InterviewBy = interviewModel.EmployeeId;;
                        interview.Name = interviewModel.Name;
                        interview.SBUId = interviewModel.SBUId;
                        interview.Status = interviewModel.Status;
                        interview.Comment = interviewModel.Comment;
                        interview.InterviewTime = interviewModel.InterviewTime;
                        interview.InterviewDate = DateTime.Now;
                        interview.UpdateBy = userId;
                        interview.UpdateDate = DateTime.Now;



                
                    }
                    else if(interviewModel.Status == 1)
                    {
                        var interview = db.Interviews.Where(r => r.Id.Equals(interviewModel.InterviewId)).FirstOrDefault();

                        interview.CandidateApplyId = interviewModel.Apply.Id;
                        interview.CreateBy = userId;
                        interview.DepartmentId = interviewModel.DepartmentId;
                        interview.InterviewBy = interviewModel.EmployeeId;
                        interview.CreateDae = DateTime.Now;
                        interview.Name = interviewModel.Name;
                        interview.SBUId = interviewModel.SBUId;
                        interview.Status = interviewModel.Status;
                        interview.Comment = interviewModel.Comment;
                        interview.InterviewTime = interviewModel.InterviewTime;
                        interview.InterviewDate = DateTime.Now;
                        interview.UpdateBy = userId;
                        interview.UpdateDate = DateTime.Now;


                        var apply = db.CandidateApplies.FirstOrDefault(t => t.Id.Equals(interviewModel.Apply.Id));
                        //apply.InterviewDate = null;
                        apply.ProfileStatus = 0;
                        //apply.InterviewStatus = interviewModel.Status;
                        apply.MaxApplySalary = interviewModel.Apply.MaxApplySalary;
                        apply.MinApplySalary = interviewModel.Apply.MinApplySalary;

                        var candidate = db.Candidates.FirstOrDefault(t => t.Id.Equals(apply.CandidateId));
                        //candidate.ProfileStatus = 0;
                        //candidate.InterviewStatus = interviewModel.Status;
                    }
                    else
                    {
                        var interview = db.Interviews.Where(r => r.Id.Equals(interviewModel.InterviewId)).FirstOrDefault();

                        interview.CandidateApplyId = interviewModel.Apply.Id;
                        interview.CreateBy = userId;
                        interview.DepartmentId = interviewModel.DepartmentId;
                        interview.InterviewBy = interviewModel.EmployeeId;
                        interview.CreateDae = DateTime.Now;
                        interview.Name = interviewModel.Name;
                        interview.SBUId = interviewModel.SBUId;
                        interview.Status = interviewModel.Status;
                        interview.Comment = interviewModel.Comment;
                        interview.InterviewTime = interviewModel.InterviewTime;
                        interview.InterviewDate = DateTime.Now;
                        interview.UpdateBy = userId;
                        interview.UpdateDate = DateTime.Now;
                    }


                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(interviewModel, ex);
                }
            }
        }



        /// <summary>
        /// Thêm câu hỏi
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="applyCreateModel"></param>
        /// <param name="userId"></param>
        public void CreateInterviewQuestion(InterviewQuestionCreateModel questionModel, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    InterviewQuestion question = new InterviewQuestion()
                    {
                        Id = Guid.NewGuid().ToString(),
                        InterviewId = questionModel.InterviewId,
                        QuestionAnswer = questionModel.QuestionAnswer,
                        QuestionType = questionModel.QuestionType,
                        QuestionScore = questionModel.QuestionScore,
                        QuestionContent = questionModel.QuestionContent
                    };

                    db.InterviewQuestions.Add(question);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(questionModel, ex);
                }
            }
        }

        /// <summary>
        /// Lấy thông tin ứng tuyển
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public InterviewModel GetInterviewInfo(InterviewGetInfoModel model)
       {
            var apply = (from a in db.CandidateApplies.AsNoTracking()
                         where a.Id.Equals(model.ApplyId)
                         join t in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals t.Id
                         select new InterviewApplyModel
                         {
                             ApplyDate = a.ApplyDate,
                             Id = a.Id,
                             //InterviewDate = a.InterviewDate,
                             //StrInterviewTime = a.InterviewTime,
                             //InterviewStatus = a.InterviewStatus,
                             Note = a.Note,
                             ProfileStatus = a.ProfileStatus,
                             MinApplySalary = a.MinApplySalary,
                             MaxApplySalary = a.MaxApplySalary,
                             Status = a.Status,
                             WorkTypeName = t.Name,
                             CandidateId = a.CandidateId,
                             WorkTypeId = a.WorkTypeId,
                             SalaryLevelMaxId = t.SalaryLevelMaxId,
                             SalaryLevelMinId = t.SalaryLevelMinId,
                             RecruitmentRequestId = a.RecruitmentRequestId
                         }).FirstOrDefault();

            if (apply == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CandidateApply);
            }

            //if (!string.IsNullOrEmpty(apply.StrInterviewTime))
            //{
            //    apply.InterviewTime = JsonConvert.DeserializeObject<object>(apply.StrInterviewTime);
            //}

            var salaryMax = db.SalaryLevels.AsNoTracking().FirstOrDefault(r => r.Id.Equals(apply.SalaryLevelMaxId));
            if (salaryMax != null)
            {
                apply.MaxCompanySalary = salaryMax.Salary;
            }

            var salaryMin = db.SalaryLevels.AsNoTracking().FirstOrDefault(r => r.Id.Equals(apply.SalaryLevelMinId));
            if (salaryMin != null)
            {
                apply.MinCompanySalary = salaryMin.Salary;
            }

            var request = db.RecruitmentRequests.FirstOrDefault(t => t.Id.Equals(apply.RecruitmentRequestId));
            if(request!=null)
            {
                apply.MinSalary = request.MinSalary;
                apply.MaxSalary = request.MaxSalary;
            }    

            InterviewModel interview = new InterviewModel();
            interview.Apply = apply;
            var workTypeInterviews = db.WorkTypeInterviews.AsNoTracking().Where(r => r.WorkTypeId.Equals(apply.WorkTypeId)).ToList();
            if (workTypeInterviews.Count > 0)
            {
                foreach (var workTypeInterview in workTypeInterviews)
                {
                    interview.Name = workTypeInterview.Name;

                    var question = (from b in db.WorkTypeInterviewQuestions.AsNoTracking()
                                    where b.WorkTypeInterviewId.Equals(workTypeInterview.Id)
                                    join q in db.Questions.AsNoTracking() on b.QuestionId equals q.Id
                                    select new InterviewQuestionModel
                                    {
                                        Id = q.Id,
                                        Name = workTypeInterview.Name,
                                        QuestionCode = q.Code,
                                        QuestionContent = q.Content,
                                        Score = q.Score,
                                        QuestionType = q.Type,
                                        Answer = q.Answer,
                                    }).ToList();
                    interview.Questions.AddRange(question);
                }
            }
            var interviewsQuestions = db.InterviewQuestions.AsNoTracking().Where(r => r.InterviewId.Equals(model.IdInterview)).ToList();
            if (interviewsQuestions.Count > 0)
            {
                foreach (var interviewsQuestion in interviewsQuestions)
                {

                        var question = (from b in db.InterviewQuestions.AsNoTracking()
                                        where b.Id.Equals(interviewsQuestion.Id)
                                        select new InterviewQuestionModel
                                        {
                                            Id = b.Id,
                                            QuestionCode = b.QuestionCode,
                                            QuestionContent = b.QuestionContent,
                                            Score = b.Score,
                                            QuestionType = b.QuestionType,
                                            Answer = b.Answer,
                                            QuestionScore = b.QuestionScore,
                                        }).ToList();
                        interview.Questions.AddRange(question);
                    
                    

                }
            }

            interview.Candidate = (from a in db.Candidates.AsNoTracking()
                                   where a.Id.Equals(apply.CandidateId)
                                   //join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                                   //from adn in ad.DefaultIfEmpty()
                                   //join s in db.SBUs.AsNoTracking() on a.SBUId equals s.Id into ass
                                   //from asn in ass.DefaultIfEmpty()
                                   join t in db.Districts.AsNoTracking() on a.DistrictId equals t.Id into at
                                   from atn in at.DefaultIfEmpty()
                                   join p in db.Provinces.AsNoTracking() on a.ProvinceId equals p.Id into ap
                                   from apn in ap.DefaultIfEmpty()
                                   join w in db.Provinces.AsNoTracking() on a.WardId equals w.Id into aw
                                   from awn in aw.DefaultIfEmpty()
                                   select new InterviewCandidateModel
                                   {
                                       Id = a.Id,
                                       AcquaintanceName = a.AcquaintanceName,
                                       AcquaintanceNote = a.AcquaintanceNote,
                                       AcquaintanceStatus = a.AcquaintanceStatus,
                                       Address = a.Address,
                                       Code = a.Code,
                                       DateOfBirth = a.DateOfBirth,
                                       //DepartmentName = adn != null ? adn.Name : string.Empty,
                                       DistrictName = atn != null ? atn.Name : string.Empty,
                                       Email = a.Email,
                                       Gender = a.Gender,
                                       //ApplyStatus = a.ApplyStatus,
                                       FollowStatus = a.FollowStatus,
                                       IdentifyAddress = a.IdentifyAddress,
                                       IdentifyDate = a.IdentifyDate,
                                       IdentifyNum = a.IdentifyNum,
                                       ImagePath = a.ImagePath,
                                       //InterviewStatus = a.InterviewStatus,
                                       Name = a.Name,
                                       PhoneNumber = a.PhoneNumber,
                                       ProvinceName = apn != null ? apn.Name : string.Empty,
                                       RecruitmentChannelId = a.RecruitmentChannelId,
                                       //SBUName = asn != null ? asn.Name : string.Empty,
                                       WardName = awn != null ? awn.Name : string.Empty,
                                       //ProfileStatus = a.ProfileStatus
                                   }).FirstOrDefault();

            interview.Candidate.WorkHistories = (from m in db.CandidateWorkHistories.AsNoTracking()
                                                 where m.CandidateId == apply.CandidateId
                                                 join t in db.WorkTypes.AsNoTracking() on m.WorkTypeId equals t.Id into mt
                                                 from mtn in mt.DefaultIfEmpty()
                                                 select new InterviewCandidateWorkHistoryModel
                                                 {
                                                     CompanyName = m.CompanyName,
                                                     Id = m.Id,
                                                     Income = m.Income,
                                                     NumberOfManage = m.NumberOfManage,
                                                     Position = m.Position,
                                                     ReferencePerson = m.ReferencePerson,
                                                     ReferencePersonPhone = m.ReferencePersonPhone,
                                                     Status = m.Status,
                                                     TotalTime = m.TotalTime,
                                                     WorkTypeName = mtn != null ? mtn.Name : string.Empty
                                                 }).ToList();

            interview.Candidate.Attachs = (from m in db.CandidateAttaches.AsNoTracking()
                                           where m.CandidateId == apply.CandidateId
                                           join u in db.Users.AsNoTracking() on m.CreateBy equals u.Id
                                           join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                           select new CandidateAttachModel
                                           {
                                               Id = m.Id,
                                               FileName = m.FileName,
                                               FilePath = m.FilePath,
                                               FileSize = m.FileSize,
                                               CreateDate = m.CreateDate,
                                               CreateName = e.Name
                                           }).ToList();

            interview.Candidate.Educations = (from m in db.CandidateEducations.AsNoTracking()
                                              where m.CandidateId == apply.CandidateId
                                              join c in db.Classifications.AsNoTracking() on m.ClassificationId equals c.Id into mc
                                              from mcn in mc.DefaultIfEmpty()
                                              join q in db.Qualifications.AsNoTracking() on m.QualificationId equals q.Id into mq
                                              from mqn in mq.DefaultIfEmpty()
                                              select new InterviewCandidateEducationModel
                                              {
                                                  Id = m.Id,
                                                  ClassificationName = mcn != null ? mcn.Name : string.Empty,
                                                  Major = m.Major,
                                                  Name = m.Name,
                                                  QualificationName = mqn != null ? mqn.Name : string.Empty,
                                                  Time = m.Time,
                                                  Type = m.Type
                                              }).ToList();

            interview.Candidate.Languages = (from m in db.CandidateLanguages.AsNoTracking()
                                             where m.CandidateId == apply.CandidateId
                                             select new CandidateLanguageModel
                                             {
                                                 Id = m.Id,
                                                 Level = m.Level,
                                                 Name = m.Name
                                             }).ToList();

            interview.Candidate.WorkTypes = (from m in db.CandidateWorkTypeFits.AsNoTracking()
                                             where m.CandidateId == apply.CandidateId
                                             join t in db.WorkTypes.AsNoTracking() on m.WorkTypeId equals t.Id
                                             select new InterviewCandidateWorkTypeFitModel
                                             {
                                                 WorkTypeName = t.Name
                                             }).ToList();

            return interview;
        }

        /// <summary>
        /// Lấy thông tin phỏng vấn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public InterviewModel GetInterviewById(string id)
        {
            InterviewModel interview = new InterviewModel();

            var info = db.Interviews.FirstOrDefault(t => t.Id.Equals(id));

            if (info == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Interview);
            }
            interview.Id = info.Id;
            interview.Status = info.Status;
            var apply = (from a in db.Interviews.AsNoTracking()
                         join b in db.CandidateApplies.AsNoTracking() on a.CandidateApplyId equals b.Id
                         where a.Id.Equals(id)
                         join t in db.WorkTypes.AsNoTracking() on b.WorkTypeId equals t.Id
                         select new InterviewApplyModel
                         {
                             ApplyDate = b.ApplyDate,
                             Id = b.Id,
                             //InterviewDate = b.InterviewDate,
                             //StrInterviewTime = b.InterviewTime,
                             //InterviewStatus = b.InterviewStatus,
                             Note = b.Note,
                             ProfileStatus = b.ProfileStatus,
                             MinApplySalary = b.MinApplySalary,
                             MaxApplySalary = b.MaxApplySalary,
                             Status = b.Status,
                             WorkTypeName = t.Name,
                             CandidateId = b.CandidateId,
                             WorkTypeId = b.WorkTypeId,
                             SalaryLevelMaxId = t.SalaryLevelMaxId,
                             SalaryLevelMinId = t.SalaryLevelMinId,
                         }).FirstOrDefault();
            interview.EmployeeId = info.InterviewBy;
            interview.DepartmentId = info.DepartmentId;
            interview.SBUId = info.SBUId;
            if (apply == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CandidateApply);
            }

            if (!string.IsNullOrEmpty(apply.StrInterviewTime))
            {
                apply.InterviewTime = JsonConvert.DeserializeObject<object>(apply.StrInterviewTime);
            }

            var salaryMax = db.SalaryLevels.AsNoTracking().FirstOrDefault(r => r.Id.Equals(apply.SalaryLevelMaxId));
            if (salaryMax != null)
            {
                apply.MaxCompanySalary = salaryMax.Salary;
            }

            var salaryMin = db.SalaryLevels.AsNoTracking().FirstOrDefault(r => r.Id.Equals(apply.SalaryLevelMinId));
            if (salaryMin != null)
            {
                apply.MinCompanySalary = salaryMin.Salary;
            }
            interview.Apply = apply;

            var workTypeInterview = db.WorkTypeInterviews.AsNoTracking().FirstOrDefault(r => r.WorkTypeId.Equals(apply.WorkTypeId) && r.DepartmentId.Equals(info.DepartmentId));

            if (workTypeInterview != null)
            {
                interview.Name = workTypeInterview.Name;

                interview.Questions = (from b in db.WorkTypeInterviewQuestions.AsNoTracking()
                                       where b.WorkTypeInterviewId.Equals(workTypeInterview.Id)
                                       join q in db.Questions.AsNoTracking() on b.QuestionId equals q.Id
                                       select new InterviewQuestionModel
                                       {
                                           Id = q.Id,
                                           QuestionCode = q.Code,
                                           QuestionContent = q.Content,
                                           QuestionScore = q.Score,
                                           QuestionType = q.Type,
                                           QuestionAnswer = q.Answer,
                                       }).ToList();
            }

            var questions = db.InterviewQuestions.Where(t => t.InterviewId.Equals(interview.Id)).Select(t => new InterviewQuestionModel()
            {
                Id = t.Id,
                QuestionContent = t.QuestionContent,
                QuestionScore = t.QuestionScore,
                QuestionType = t.QuestionType,
                QuestionAnswer = t.QuestionAnswer,
                Answer = t.Answer,
                Score = t.Score,
                Status = t.Status
            }).ToList();

            if (questions.Count > 0)
            {
                interview.Questions.AddRange(questions);
            }

            interview.Candidate = (from a in db.Candidates.AsNoTracking()
                                   where a.Id.Equals(apply.CandidateId)
                                   //join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                                   //from adn in ad.DefaultIfEmpty()
                                   //join s in db.SBUs.AsNoTracking() on a.SBUId equals s.Id into ass
                                   //from asn in ass.DefaultIfEmpty()
                                   join t in db.Districts.AsNoTracking() on a.DistrictId equals t.Id into at
                                   from atn in at.DefaultIfEmpty()
                                   join p in db.Provinces.AsNoTracking() on a.ProvinceId equals p.Id into ap
                                   from apn in ap.DefaultIfEmpty()
                                   join w in db.Provinces.AsNoTracking() on a.WardId equals w.Id into aw
                                   from awn in aw.DefaultIfEmpty()
                                   select new InterviewCandidateModel
                                   {
                                       Id = a.Id,
                                       AcquaintanceName = a.AcquaintanceName,
                                       AcquaintanceNote = a.AcquaintanceNote,
                                       AcquaintanceStatus = a.AcquaintanceStatus,
                                       Address = a.Address,
                                       Code = a.Code,
                                       DateOfBirth = a.DateOfBirth,
                                       //DepartmentName = adn != null ? adn.Name : string.Empty,
                                       DistrictName = atn != null ? atn.Name : string.Empty,
                                       Email = a.Email,
                                       Gender = a.Gender,
                                       //ApplyStatus = a.ApplyStatus,
                                       FollowStatus = a.FollowStatus,
                                       IdentifyAddress = a.IdentifyAddress,
                                       IdentifyDate = a.IdentifyDate,
                                       IdentifyNum = a.IdentifyNum,
                                       ImagePath = a.ImagePath,
                                       //InterviewStatus = a.InterviewStatus,
                                       Name = a.Name,
                                       PhoneNumber = a.PhoneNumber,
                                       ProvinceName = apn != null ? apn.Name : string.Empty,
                                       RecruitmentChannelId = a.RecruitmentChannelId,
                                       //SBUName = asn != null ? asn.Name : string.Empty,
                                       WardName = awn != null ? awn.Name : string.Empty,
                                       //ProfileStatus = a.ProfileStatus
                                   }).FirstOrDefault();

            interview.Candidate.WorkHistories = (from m in db.CandidateWorkHistories.AsNoTracking()
                                                 where m.CandidateId == apply.CandidateId
                                                 join t in db.WorkTypes.AsNoTracking() on m.WorkTypeId equals t.Id into mt
                                                 from mtn in mt.DefaultIfEmpty()
                                                 select new InterviewCandidateWorkHistoryModel
                                                 {
                                                     CompanyName = m.CompanyName,
                                                     Id = m.Id,
                                                     Income = m.Income,
                                                     NumberOfManage = m.NumberOfManage,
                                                     Position = m.Position,
                                                     ReferencePerson = m.ReferencePerson,
                                                     ReferencePersonPhone = m.ReferencePersonPhone,
                                                     Status = m.Status,
                                                     TotalTime = m.TotalTime,
                                                     WorkTypeName = mtn != null ? mtn.Name : string.Empty
                                                 }).ToList();

            interview.Candidate.Attachs = (from m in db.CandidateAttaches.AsNoTracking()
                                           where m.CandidateId == apply.CandidateId
                                           join u in db.Users.AsNoTracking() on m.CreateBy equals u.Id
                                           join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                           select new CandidateAttachModel
                                           {
                                               Id = m.Id,
                                               FileName = m.FileName,
                                               FilePath = m.FilePath,
                                               FileSize = m.FileSize,
                                               CreateDate = m.CreateDate,
                                               CreateName = e.Name
                                           }).ToList();

            interview.Candidate.Educations = (from m in db.CandidateEducations.AsNoTracking()
                                              where m.CandidateId == apply.CandidateId
                                              join c in db.Classifications.AsNoTracking() on m.ClassificationId equals c.Id into mc
                                              from mcn in mc.DefaultIfEmpty()
                                              join q in db.Qualifications.AsNoTracking() on m.QualificationId equals q.Id into mq
                                              from mqn in mq.DefaultIfEmpty()
                                              select new InterviewCandidateEducationModel
                                              {
                                                  Id = m.Id,
                                                  ClassificationName = mcn != null ? mcn.Name : string.Empty,
                                                  Major = m.Major,
                                                  Name = m.Name,
                                                  QualificationName = mqn != null ? mqn.Name : string.Empty,
                                                  Time = m.Time,
                                                  Type = m.Type
                                              }).ToList();

            interview.Candidate.Languages = (from m in db.CandidateLanguages.AsNoTracking()
                                             where m.CandidateId == apply.CandidateId
                                             select new CandidateLanguageModel
                                             {
                                                 Id = m.Id,
                                                 Level = m.Level,
                                                 Name = m.Name
                                             }).ToList();

            interview.Candidate.WorkTypes = (from m in db.CandidateWorkTypeFits.AsNoTracking()
                                             where m.CandidateId == apply.CandidateId
                                             join t in db.WorkTypes.AsNoTracking() on m.WorkTypeId equals t.Id
                                             select new InterviewCandidateWorkTypeFitModel
                                             {
                                                 WorkTypeName = t.Name
                                             }).ToList();


            interview.ListUser = (from a in db.InterviewUsers.AsNoTracking()
                                  join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                                  join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                  join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                                  where a.InterviewId.Equals(interview.Id)
                                  select new InterviewUserInfoModel
                                  {
                                      InterviewUserId = a.Id,
                                      InterviewId = a.InterviewId,
                                      UserId = a.UserId,
                                      Code = c.Code,
                                      Name = c.Name,
                                      PhoneNumber = c.PhoneNumber,
                                      Email = c.Email,
                                      Status = c.Status,
                                      DepartmentName = d.Name,
                                      IsNew = false
                                  }).ToList();
            return interview;
        }

        public void DeleteInterview(string id, string userId)
        {
            var interview = db.Interviews.FirstOrDefault(t => t.Id.Equals(id));
            if (interview == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Interview);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var questions = db.InterviewQuestions.Where(s => s.InterviewId.Equals(id)).ToList();
                    db.InterviewQuestions.RemoveRange(questions);

                    db.Interviews.Remove(interview);

                    //var jsonBefore = AutoMapperConfig.Mapper.Map<CandidateCreateModel>(interview);
                    //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_SaleProduct, interview.Id, interview.Name, jsonBefore);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(id, ex);
                }
            }
        }

        public string ExportExcel(InterviewSearchModel searchModel)
        {
            var data = SearchInterviews(searchModel);
            var list = data.ListResult.ToList();
            //ListModel listModel = new ListModel();

            if (list.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;
                application.EnableIncrementalFormula = true;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/sp_cho_kinh_doanh.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = list.Count;

                IRange iRangeData = sheet.FindFirst("<data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<data>", string.Empty);
                string path = AppDomain.CurrentDomain.BaseDirectory;
                var listExport = list.Select((a, i) => new
                {
                    Index = i + 1,

                });

                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }

                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 24].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 142].CellStyle.WrapText = true;

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách sản phẩm kinh doanh" + ".xlsx");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách sản phẩm kinh doanh" + ".xlsx";
                db.SaveChanges();
                return resultPathClient;

            }
            catch (Exception ex)
            {
                throw new NTSLogException(searchModel, ex);
            }
        }

        public List<InterviewQuestionModel> GetListQuestions(string Id)
        {
            var listQuestions = (from a in db.InterviewQuestions.AsNoTracking()
                                 where a.InterviewId.Equals(Id)
                                 select new InterviewQuestionModel
                                 {
                                     Id = a.Id,
                                     Answer = a.Answer,
                                     InterviewId = a.Id,
                                     QuestionCode = a.QuestionCode,
                                     Score = a.Score,
                                     QuestionAnswer = a.QuestionAnswer,
                                     Status = a.Status,
                                     QuestionType = a.QuestionType,
                                     QuestionScore = a.QuestionScore,
                                     QuestionContent = a.QuestionContent

                                 }).ToList();

            return listQuestions;
        }

        public void Delete(string id, string userId)
        {
            var request = db.CandidateApplies.FirstOrDefault(t => t.Id.Equals(id));
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CandidateApply);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //request.InterviewDate = null;

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(id, ex);
                }
            }
        }

        public List<InterviewUserInfoModel> getEmployee(string id)
        {
            var employee = (from a in db.InterviewUsers.AsNoTracking()
                          where a.InterviewId.Equals(id)
                          join r in db.Users on a.UserId equals r.Id into ar
                          from arc in ar.DefaultIfEmpty()
                          join b in db.Employees on arc.EmployeeId equals b.Id into rb
                          from rbc in rb.DefaultIfEmpty()
                          join c in db.Departments on rbc.DepartmentId equals c.Id into bc
                          from bcn in bc.DefaultIfEmpty()
                          select new InterviewUserInfoModel
                          {
                              Id = a.Id,
                              InterviewId = id,
                              Name = rbc.Name,
                              Code = rbc.Code,
                              Email = rbc.Email,
                              PhoneNumber = rbc.PhoneNumber,
                              DepartmentName = bcn.Name,
                              UserId = a.UserId,
                              ImagePath = rbc.ImagePath,
                          }).ToList();

            return employee;
        }

        public void UpdateEmployee(string id, List<InterviewUserInfoModel> model)
        {
            var interview = db.Interviews.FirstOrDefault(t => t.Id.Equals(id));

            if (interview == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Interview);
            }
            var interviewsUser = db.InterviewUsers.Where(r => r.InterviewId.Equals(id));
            db.InterviewUsers.RemoveRange(interviewsUser);
            foreach (var user in model.ToList()) {
                
                if (user.UserId != null) 
                {
                    InterviewUser interviewUsers = new InterviewUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        InterviewId = id,
                        UserId = user.UserId,
                    };
                    db.InterviewUsers.Add(interviewUsers);

                }
                else
                {
                    InterviewUser interviewUsers = new InterviewUser()
                    {
                            Id = Guid.NewGuid().ToString(),
                            InterviewId = id,
                            UserId = user.Id,
                        };

                        db.InterviewUsers.Add(interviewUsers);

                }
            }
            db.SaveChanges();
        }

        public void UpdateQuestions(string id, List<InterviewQuestionModel> model)
        {
            var interview = db.Interviews.FirstOrDefault(t => t.Id.Equals(id));

            if (interview == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Interview);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var interviewsQuestion = db.InterviewQuestions.Where(r => r.InterviewId.Equals(id));
                    db.InterviewQuestions.RemoveRange(interviewsQuestion);

                    InterviewQuestion interviewsQuestions;
                    foreach (var item in model)
                    {
                        interviewsQuestions = new InterviewQuestion()
                        {
                            Answer = item.Answer,
                            Id = Guid.NewGuid().ToString(),
                            InterviewId = interview.Id,
                            QuestionCode = item.QuestionCode,
                            Score = item.Score,
                            QuestionAnswer = item.QuestionAnswer,
                            Status = item.Status,
                            QuestionType = item.QuestionType,
                            QuestionScore = item.QuestionScore,
                            QuestionContent = item.QuestionContent
                        };

                        db.InterviewQuestions.Add(interviewsQuestions);
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
