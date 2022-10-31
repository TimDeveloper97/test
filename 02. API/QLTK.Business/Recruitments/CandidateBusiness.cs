using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Bussiness;
using NTS.Model.Bussiness.Application;
using NTS.Model.Candidates;
using NTS.Model.Classification;
using NTS.Model.Combobox;
using NTS.Model.Job;
using NTS.Model.Recruitments.Candidates;
using NTS.Model.Repositories;
using NTS.Model.Sale.SaleProduct;
using NTS.Model.UserHistory;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Recruitments
{
    public class CandidateBusiness
    {
        private QLTKEntities db = new QLTKEntities();

        public SearchResultModel<CandidateSearchResultsModel> SearchCandidates(CandidateSearchModel searchModel)
        {
            SearchResultModel<CandidateSearchResultsModel> searchResult = new SearchResultModel<CandidateSearchResultsModel>();

            List<string> candidateIds = new List<string>();
            var dataQuery = (from a in db.Candidates.AsNoTracking()
                             join b in db.CandidateApplies.AsNoTracking() on a.Id equals b.CandidateId
                             join t in db.WorkTypes.AsNoTracking() on b.WorkTypeId equals t.Id
                             where b.ProfileStatus == Constants.CandidateApply_ProfileStatus
                             orderby b.ApplyDate descending
                             select new CandidateSearchResultsModel
                             {
                                 ApplyId = b.Id,
                                 Id = a.Id,
                                 ImagePath = a.ImagePath,
                                 Name = a.Name,
                                 Code = a.Code,
                                 PhoneNumber = a.PhoneNumber,
                                 DateOfBirth = a.DateOfBirth,
                                 Email = a.Email,
                                 ApplyDate = b.ApplyDate,
                                 ApplyStatus = b.Status,
                                 WorkTypeName = t.Name,
                                 //InterviewDate = b.InterviewDate,
                                 WorkTypeId = t.Id,
                                 MinApplySalary = b.MinApplySalary,
                                 MaxApplySalary = b.MaxApplySalary
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                dataQuery = dataQuery.Where(r => r.Name.ToLower().Contains(searchModel.Name.ToLower()) || r.Code.ToLower().Contains(searchModel.Name.ToLower()));
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

            if (searchModel.ApplyDateTo.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.ApplyDate >= searchModel.ApplyDateTo);
            }

            if (searchModel.ApplyDateFrom.HasValue)
            {
                DateTime applyDateFrom = searchModel.ApplyDateFrom.Value.ToEndDate();
                dataQuery = dataQuery.Where(r => r.ApplyDate <= applyDateFrom);
            }

            searchResult.TotalItem = dataQuery.Select(s => s.Id).Count();

            var listResult = dataQuery.OrderByDescending(t => t.ApplyDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            var interview = db.Interviews.AsNoTracking();
            foreach (var item in listResult)
            {
                int total = interview.Where(r => r.CandidateApplyId.Equals(item.ApplyId)).Count();
                int unInterview = interview.Where(r => r.CandidateApplyId.Equals(item.ApplyId) && r.Status == 0).Count();
                int pass = interview.Where(r => r.CandidateApplyId.Equals(item.ApplyId) && r.Status == 2).Count();
                int fail = interview.Where(r => r.CandidateApplyId.Equals(item.ApplyId) && r.Status == 1).Count();

                if (total > 0)
                {
                    if (unInterview > 0)
                    {
                        item.InterviewStatus = 0;

                    }
                    else if (pass == total)
                    {
                        item.InterviewStatus = 2;
                    }
                    else
                    {
                        item.InterviewStatus = 1;
                    }
                }
                else
                {
                    item.InterviewStatus = 0;
                }
            }

            searchResult.ListResult = listResult;

            return searchResult;
        }

        public SearchResultModel<CandidateSearchResultsModel> SearchCandidatesByRecruitmentRequestId(string id)
        {
            SearchResultModel<CandidateSearchResultsModel> searchResult = new SearchResultModel<CandidateSearchResultsModel>();

            List<string> candidateIds = new List<string>();
            var dataQuery = (from a in db.Candidates.AsNoTracking()
                             join b in db.CandidateApplies.AsNoTracking() on a.Id equals b.CandidateId
                             join t in db.WorkTypes.AsNoTracking() on b.WorkTypeId equals t.Id
                             where /* a.ProfileStatus == Constants.CandidateApply_ProfileStatus && */  b.RecruitmentRequestId.Equals(id)
                             orderby b.ApplyDate descending
                             select new CandidateSearchResultsModel
                             {
                                 ApplyId = b.Id,
                                 Id = a.Id,
                                 ImagePath = a.ImagePath,
                                 Name = a.Name,
                                 Code = a.Code,
                                 PhoneNumber = a.PhoneNumber,
                                 DateOfBirth = a.DateOfBirth,
                                 Email = a.Email,
                                 ApplyDate = b.ApplyDate,
                                 //ApplyStatus = a.ApplyStatus,
                                 //InterviewStatus = a.InterviewStatus,
                                 WorkTypeName = t.Name,
                                 //InterviewDate = b.InterviewDate,
                                 WorkTypeId = t.Id,
                                 MinApplySalary = b.MinApplySalary,
                                 MaxApplySalary = b.MaxApplySalary,

                             }).AsQueryable();

            searchResult.TotalItem = dataQuery.Select(s => s.Id).Count();

            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        public CandidateCodeModel GenerateCode()
        {
            var dateNow = DateTime.Now;
            string code = string.Empty;
            var maxIndex = db.Candidates.AsNoTracking().Where(t => t.CreateDate.Year == dateNow.Year).Select(r => r.Index).DefaultIfEmpty(0).Max();
            maxIndex++;
            code = $"HS.{dateNow.ToString("yy")}.{string.Format("{0:00000}", maxIndex)}";

            return new CandidateCodeModel
            {
                Code = code,
                Index = maxIndex
            };
        }

        public void CreateCandidate(CandidateCreateModel candidateCreateModel, string userId)
        {
            while (db.Candidates.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(candidateCreateModel.Code.ToUpper())) != null)
            {
                var codeModel = GenerateCode();
                candidateCreateModel.Code = codeModel.Code;
                candidateCreateModel.Index = codeModel.Index;
            }

            var candidateEmailExist = db.Candidates.AsNoTracking().FirstOrDefault(r => r.Email.ToLower().Equals(candidateCreateModel.Email.ToLower()));

            if (candidateEmailExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Candidate);
            }

            var candidatePhoneExist = db.Candidates.AsNoTracking().FirstOrDefault(r => r.PhoneNumber.Equals(candidateCreateModel.PhoneNumber));

            if (candidatePhoneExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0084, TextResourceKey.PhoneNumber, TextResourceKey.Candidate);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Candidate candidate = new Candidate()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = candidateCreateModel.Name,
                        Code = candidateCreateModel.Code,
                        AcquaintanceName = candidateCreateModel.AcquaintanceName,
                        AcquaintanceNote = candidateCreateModel.AcquaintanceNote,
                        AcquaintanceStatus = candidateCreateModel.AcquaintanceStatus,
                        Address = candidateCreateModel.Address,
                        DateOfBirth = candidateCreateModel.DateOfBirth,
                        //DepartmentId = candidateCreateModel.DepartmentId,
                        Email = candidateCreateModel.Email,
                        Gender = candidateCreateModel.Gender,
                        IdentifyAddress = candidateCreateModel.IdentifyAddress,
                        IdentifyDate = candidateCreateModel.IdentifyDate,
                        IdentifyNum = candidateCreateModel.IdentifyNum,
                        ImagePath = candidateCreateModel.ImagePath,
                        PhoneNumber = candidateCreateModel.PhoneNumber,
                        RecruitmentChannelId = candidateCreateModel.RecruitmentChannelId,
                        //SBUId = candidateCreateModel.SBUId,
                        WardId = candidateCreateModel.WardId,
                        DistrictId = candidateCreateModel.DistrictId,
                        ProvinceId = candidateCreateModel.ProvinceId,
                        FollowStatus = candidateCreateModel.FollowStatus,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };

                    db.Candidates.Add(candidate);

                    CandidateAttach candidateAttach;
                    foreach (var item in candidateCreateModel.Attachs)
                    {
                        candidateAttach = new CandidateAttach()
                        {
                            CandidateId = candidate.Id,
                            FileSize = item.FileSize,
                            Id = Guid.NewGuid().ToString(),
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            FileName = item.FileName,
                            FilePath = item.FilePath,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        db.CandidateAttaches.Add(candidateAttach);
                    }

                    CandidateEducation candidateEducation;
                    foreach (var item in candidateCreateModel.Educations)
                    {
                        candidateEducation = new CandidateEducation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CandidateId = candidate.Id,
                            ClassificationId = item.ClassificationId,
                            Major = item.Major,
                            Name = item.Name,
                            QualificationId = item.QualificationId,
                            Time = item.Time,
                            Type = item.Type
                        };

                        db.CandidateEducations.Add(candidateEducation);
                    }

                    CandidateLanguage candidateLanguage;
                    foreach (var item in candidateCreateModel.Languages)
                    {
                        candidateLanguage = new CandidateLanguage()
                        {
                            CandidateId = candidate.Id,
                            Id = Guid.NewGuid().ToString(),
                            Level = item.Level,
                            Name = item.Name
                        };

                        db.CandidateLanguages.Add(candidateLanguage);
                    }

                    CandidateWorkHistory candidateWorkHistory;
                    foreach (var item in candidateCreateModel.WorkHistories)
                    {
                        candidateWorkHistory = new CandidateWorkHistory()
                        {
                            CandidateId = candidate.Id,
                            CompanyName = item.CompanyName,
                            Id = Guid.NewGuid().ToString(),
                            Income = item.Income,
                            NumberOfManage = item.NumberOfManage,
                            Position = item.Position,
                            ReferencePerson = item.ReferencePerson,
                            ReferencePersonPhone = item.ReferencePersonPhone,
                            Status = item.Status,
                            TotalTime = item.TotalTime,
                            WorkTypeId = item.WorkTypeId
                        };

                        db.CandidateWorkHistories.Add(candidateWorkHistory);
                    }

                    CandidateWorkTypeFit candidateWorkTypeFit;
                    foreach (var item in candidateCreateModel.WorkTypes)
                    {
                        candidateWorkTypeFit = new CandidateWorkTypeFit()
                        {
                            WorkTypeId = item.WorkTypeId,
                            CandidateId = candidate.Id,
                            Id = Guid.NewGuid().ToString(),
                        };

                        db.CandidateWorkTypeFits.Add(candidateWorkTypeFit);
                    }

                    UserLogUtil.LogHistotyAdd(db, userId, candidate.Code, candidate.Id, Constants.LOG_Candidate);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(candidateCreateModel, ex);
                }
            }
        }


        public void UpdateCandidate(string id, CandidateCreateModel candidateCreateModel, string userId)
        {
            var candidateEmailExist = db.Candidates.AsNoTracking().FirstOrDefault(r => !r.Id.Equals(id) && r.Email.ToLower().Equals(candidateCreateModel.Email.ToLower()));

            if (candidateEmailExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0007, TextResourceKey.Candidate);
            }

            var candidatePhoneExist = db.Candidates.AsNoTracking().FirstOrDefault(r => !r.Id.Equals(id) && r.PhoneNumber.Equals(candidateCreateModel.PhoneNumber));

            if (candidatePhoneExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0084, TextResourceKey.PhoneNumber, TextResourceKey.Candidate);
            }

            var candidate = db.Candidates.FirstOrDefault(t => t.Id.Equals(id));

            if (candidate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Candidate);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    candidate.Name = candidateCreateModel.Name;
                    candidate.Code = candidateCreateModel.Code;
                    candidate.AcquaintanceName = candidateCreateModel.AcquaintanceName;
                    candidate.AcquaintanceNote = candidateCreateModel.AcquaintanceNote;
                    candidate.AcquaintanceStatus = candidateCreateModel.AcquaintanceStatus;
                    candidate.Address = candidateCreateModel.Address;
                    candidate.DateOfBirth = candidateCreateModel.DateOfBirth;
                    //candidate.DepartmentId = candidateCreateModel.DepartmentId;
                    candidate.Email = candidateCreateModel.Email;
                    candidate.Gender = candidateCreateModel.Gender;
                    candidate.IdentifyAddress = candidateCreateModel.IdentifyAddress;
                    candidate.IdentifyDate = candidateCreateModel.IdentifyDate;
                    candidate.IdentifyNum = candidateCreateModel.IdentifyNum;
                    candidate.ImagePath = candidateCreateModel.ImagePath;
                    candidate.PhoneNumber = candidateCreateModel.PhoneNumber;
                    candidate.RecruitmentChannelId = candidateCreateModel.RecruitmentChannelId;
                    //candidate.SBUId = candidateCreateModel.SBUId;
                    candidate.FollowStatus = candidateCreateModel.FollowStatus;

                    candidate.ProvinceId = candidateCreateModel.ProvinceId;
                    candidate.DistrictId = candidateCreateModel.DistrictId;
                    candidate.WardId = candidateCreateModel.WardId;

                    candidate.UpdateBy = userId;
                    candidate.UpdateDate = DateTime.Now;

                    CandidateAttach candidateAttach;
                    foreach (var item in candidateCreateModel.Attachs)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            candidateAttach = new CandidateAttach()
                            {
                                CandidateId = candidate.Id,
                                FileSize = item.FileSize,
                                Id = Guid.NewGuid().ToString(),
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            };

                            db.CandidateAttaches.Add(candidateAttach);
                        }
                        else
                        {
                            candidateAttach = db.CandidateAttaches.FirstOrDefault(r => r.Id.Equals(item.Id));

                            if (candidateAttach != null)
                            {
                                if (item.IsDelete)
                                {
                                    db.CandidateAttaches.Remove(candidateAttach);
                                }
                                else
                                {
                                    candidateAttach.FileName = item.FileName;
                                    candidateAttach.FilePath = item.FilePath;
                                    candidateAttach.FileSize = item.FileSize;
                                    candidateAttach.UpdateBy = userId;
                                    candidateAttach.UpdateDate = DateTime.Now;
                                }
                            }
                        }
                    }

                    var educations = db.CandidateEducations.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateEducations.RemoveRange(educations);

                    CandidateEducation candidateEducation;
                    foreach (var item in candidateCreateModel.Educations)
                    {
                        candidateEducation = new CandidateEducation()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CandidateId = candidate.Id,
                            ClassificationId = item.ClassificationId,
                            Major = item.Major,
                            Name = item.Name,
                            QualificationId = item.QualificationId,
                            Time = item.Time,
                            Type = item.Type
                        };

                        db.CandidateEducations.Add(candidateEducation);
                    }

                    var languages = db.CandidateLanguages.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateLanguages.RemoveRange(languages);

                    CandidateLanguage candidateLanguage;
                    foreach (var item in candidateCreateModel.Languages)
                    {
                        candidateLanguage = new CandidateLanguage()
                        {
                            CandidateId = candidate.Id,
                            Id = Guid.NewGuid().ToString(),
                            Level = item.Level,
                            Name = item.Name
                        };

                        db.CandidateLanguages.Add(candidateLanguage);
                    }

                    var workHistories = db.CandidateWorkHistories.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateWorkHistories.RemoveRange(workHistories);

                    CandidateWorkHistory candidateWorkHistory;
                    foreach (var item in candidateCreateModel.WorkHistories)
                    {
                        candidateWorkHistory = new CandidateWorkHistory()
                        {
                            CandidateId = candidate.Id,
                            CompanyName = item.CompanyName,
                            Id = Guid.NewGuid().ToString(),
                            Income = item.Income,
                            NumberOfManage = item.NumberOfManage,
                            Position = item.Position,
                            ReferencePerson = item.ReferencePerson,
                            ReferencePersonPhone = item.ReferencePersonPhone,
                            Status = item.Status,
                            TotalTime = item.TotalTime,
                            WorkTypeId = item.WorkTypeId
                        };

                        db.CandidateWorkHistories.Add(candidateWorkHistory);
                    }

                    var workTypeFits = db.CandidateWorkTypeFits.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateWorkTypeFits.RemoveRange(workTypeFits);

                    CandidateWorkTypeFit candidateWorkTypeFit;
                    foreach (var item in candidateCreateModel.WorkTypes)
                    {
                        candidateWorkTypeFit = new CandidateWorkTypeFit()
                        {
                            WorkTypeId = item.WorkTypeId,
                            CandidateId = candidate.Id,
                            Id = Guid.NewGuid().ToString(),
                        };

                        db.CandidateWorkTypeFits.Add(candidateWorkTypeFit);
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(candidateCreateModel, ex);
                }
            }
        }

        public void UpdateFollow(string candidateId, List<CandidateFollowModel> candidateFollows)
        {
            var candidate = db.Candidates.FirstOrDefault(t => t.Id.Equals(candidateId));

            if (candidate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Candidate);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var follows = db.CandidateFollows.Where(r => r.CandidateId.Equals(candidateId));
                    db.CandidateFollows.RemoveRange(follows);

                    CandidateFollow candidateFollow;
                    foreach (var item in candidateFollows)
                    {
                        candidateFollow = new CandidateFollow()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CandidateId = candidateId,
                            Content = item.Content,
                            FollowDate = item.FollowDate
                        };

                        db.CandidateFollows.Add(candidateFollow);
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(candidateFollows, ex);
                }
            }
        }
        /// <summary>
        /// Chi tiết sản phẩm
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CandidateCreateModel GetCandidateById(string id)
        {
            var candidate = (from a in db.Candidates.AsNoTracking()
                             where a.Id.Equals(id)
                             select new CandidateCreateModel
                             {
                                 Id = id,
                                 AcquaintanceName = a.AcquaintanceName,
                                 AcquaintanceNote = a.AcquaintanceNote,
                                 AcquaintanceStatus = a.AcquaintanceStatus,
                                 Address = a.Address,
                                 Code = a.Code,
                                 DateOfBirth = a.DateOfBirth,
                                 //DepartmentId = a.DepartmentId,
                                 DistrictId = a.DistrictId,
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
                                 ProvinceId = a.ProvinceId,
                                 RecruitmentChannelId = a.RecruitmentChannelId,
                                 //SBUId = a.SBUId,
                                 WardId = a.WardId,
                                 //ProfileStatus = a.ProfileStatus
                             }).FirstOrDefault();

            if (candidate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Candidate);
            }

            candidate.WorkHistories = (from m in db.CandidateWorkHistories.AsNoTracking()
                                       where m.CandidateId == id
                                       select new CandidateWorkHistoryModel
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
                                           WorkTypeId = m.WorkTypeId
                                       }).ToList();

            candidate.Attachs = (from m in db.CandidateAttaches.AsNoTracking()
                                 where m.CandidateId == id
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

            candidate.Educations = (from m in db.CandidateEducations.AsNoTracking()
                                    where m.CandidateId == id
                                    select new CandidateEducationModel
                                    {
                                        Id = m.Id,
                                        ClassificationId = m.ClassificationId,
                                        Major = m.Major,
                                        Name = m.Name,
                                        QualificationId = m.QualificationId,
                                        Time = m.Time,
                                        Type = m.Type
                                    }).ToList();

            candidate.Languages = (from m in db.CandidateLanguages.AsNoTracking()
                                   where m.CandidateId == id
                                   select new CandidateLanguageModel
                                   {
                                       Id = m.Id,
                                       Level = m.Level,
                                       Name = m.Name
                                   }).ToList();

            candidate.WorkTypes = (from m in db.CandidateWorkTypeFits.AsNoTracking()
                                   where m.CandidateId == id
                                   select new CandidateWorkTypeFitModel
                                   {
                                       Id = m.Id,
                                       WorkTypeId = m.WorkTypeId
                                   }).ToList();

            return candidate;
        }

        public List<CandidateFollowModel> GetFollow(string id)
        {
            var follows = (from a in db.CandidateFollows.AsNoTracking()
                           where a.CandidateId.Equals(id)
                           select new CandidateFollowModel
                           {
                               FollowDate = a.FollowDate,
                               Content = a.Content
                           }).ToList();

            return follows;
        }

        public List<CandidateApplyModel> GetApplys(string id)
        {
            var applys = (from a in db.CandidateApplies.AsNoTracking()
                          where a.CandidateId.Equals(id)
                          join w in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals w.Id
                          orderby a.ApplyDate descending
                          select new CandidateApplyModel
                          {
                              ApplyDate = a.ApplyDate,
                              Id = a.Id,
                              //InterviewDate = a.InterviewDate,
                              //InterviewStatus = a.InterviewStatus,
                              Note = a.Note,
                              ProfileStatus = a.ProfileStatus,
                              MinApplySalary = a.MinApplySalary,
                              MaxApplySalary = a.MaxApplySalary,
                              Status = a.Status,
                              WorkTypeName = w.Name
                          }).ToList();

            return applys;
        }

        public void DeleteCandidate(string id, string userId)
        {
            var candidate = db.Candidates.FirstOrDefault(t => t.Id.Equals(id));
            if (candidate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Candidate);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var languages = db.CandidateLanguages.Where(s => s.CandidateId.Equals(id)).ToList();
                    db.CandidateLanguages.RemoveRange(languages);

                    var candidateWorkTypeFits = db.CandidateWorkTypeFits.Where(s => s.CandidateId.Equals(id)).ToList();
                    db.CandidateWorkTypeFits.RemoveRange(candidateWorkTypeFits);

                    var interviewQuestion = (from t in db.Interviews
                                             join c in db.CandidateApplies on t.CandidateApplyId equals c.Id
                                             where c.CandidateId.Equals(id)
                                             join q in db.InterviewQuestions on t.Id equals q.InterviewId
                                             select q).ToList();
                    db.InterviewQuestions.RemoveRange(interviewQuestion);

                    var interviews = (from t in db.Interviews
                                      join c in db.CandidateApplies on t.CandidateApplyId equals c.Id
                                      where c.CandidateId.Equals(id)
                                      select t).ToList();
                    db.Interviews.RemoveRange(interviews);

                    var candidateApplies = db.CandidateApplies.Where(s => s.CandidateId.Equals(id)).ToList();
                    db.CandidateApplies.RemoveRange(candidateApplies);

                    var candidateAttaches = db.CandidateAttaches.Where(s => s.CandidateId.Equals(id)).ToList();
                    db.CandidateAttaches.RemoveRange(candidateAttaches);

                    var candidateWorkHistories = db.CandidateWorkHistories.Where(s => s.CandidateId.Equals(id)).ToList();
                    db.CandidateWorkHistories.RemoveRange(candidateWorkHistories);

                    var candidateEducation = db.CandidateEducations.Where(s => s.CandidateId.Equals(id)).ToList();
                    db.CandidateEducations.RemoveRange(candidateEducation);

                    db.Candidates.Remove(candidate);

                    //var jsonBefore = AutoMapperConfig.Mapper.Map<CandidateHistoryModel>(candidate);
                    //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_SaleProduct, candidate.Id, candidate.Code, jsonBefore);

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

        public string ExportExcel(CandidateSearchModel searchModel)
        {
            var data = SearchCandidates(searchModel);
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

        public void CancelCandidate(string id)
        {
            var apply = db.CandidateApplies.FirstOrDefault(t => t.Id.Equals(id));
            if (apply == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CandidateApply);
            }

            var candidate = db.Candidates.FirstOrDefault(t => t.Id.Equals(apply.CandidateId));
            if (candidate == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Candidate);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //candidate.ProfileStatus = 1;
                    apply.ProfileStatus = 1;

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
    }
}
