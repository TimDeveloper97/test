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
using NTS.Model.UserHistory;
using NTS.Model.WorkType;
using QLTK.Business.AutoMappers;
using QLTK.Business.Users;
using Syncfusion.DocIO.DLS;
using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO;
using Syncfusion.XPS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using static System.Collections.Specialized.BitVector32;

namespace QLTK.Business.Recruitments
{
    public class ApplyBusiness
    {
        private QLTKEntities db = new QLTKEntities();
        private CandidateBusiness _candidateBusiness = new CandidateBusiness();

        /// <summary>
        /// Tìm kiếm ứng tuyển
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public SearchResultModel<ApplySearchResultsModel> SearchApplys(ApplySearchModel searchModel)
        {
            SearchResultModel<ApplySearchResultsModel> searchResult = new SearchResultModel<ApplySearchResultsModel>();

            List<string> candidateIds = new List<string>();

            var dataQuery = (from a in db.CandidateApplies.AsNoTracking()
                             orderby a.CreateDate descending
                             join c in db.Candidates.AsNoTracking() on a.CandidateId equals c.Id
                             join t in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals t.Id
                             join d in db.RecruitmentRequests.AsNoTracking() on a.RecruitmentRequestId equals d.Id
                             into ad
                             from adx in ad.DefaultIfEmpty()
                                 //where a.ProfileStatus != Constants.CandidateApply_ProfileStatus 
                             select new ApplySearchResultsModel
                             {
                                 Id = a.Id,
                                 ImagePath = c.ImagePath,
                                 Name = c.Name,
                                 Code = c.Code,
                                 PhoneNumber = c.PhoneNumber,
                                 Email = c.Email,
                                 ApplyDate = a.ApplyDate,
                                 Status = a.Status,
                                 //InterviewDate = a.InterviewDate,
                                 //InterviewStatus = a.InterviewStatus,
                                 ProfileStatus = a.ProfileStatus,
                                 MinApplySalary = a.MinApplySalary,
                                 MaxApplySalary = a.MaxApplySalary,
                                 WorkTypeName = t.Name,
                                 WorkTypeId = t.Id,
                                 RecruitmentRequestCode = adx != null ? adx.Code : string.Empty,
                                 RecruitmentRequestId = a.RecruitmentRequestId,
                                 FollowStatus = c.FollowStatus,
                                 CandidatesId = c.Id,
                                 Total = db.Interviews.Where(r => r.CandidateApplyId.Equals(a.Id)).Count(),
                                 UnInterview = db.Interviews.Where(r => r.CandidateApplyId.Equals(a.Id) && r.Status == 0).Count(),
                                 Pass = db.Interviews.Where(r => r.CandidateApplyId.Equals(a.Id) && r.Status == 2).Count(),
                                 Fail = db.Interviews.Where(r => r.CandidateApplyId.Equals(a.Id) && r.Status == 1).Count(),

                             }).AsQueryable();
            List<string> idCandidateApply = new List<string>();
            idCandidateApply = dataQuery.Select(r => r.Id).ToList();
            var listId = string.Join(";", idCandidateApply);
            List<ApplySearchResultsModel> Candidate = new List<ApplySearchResultsModel>();
            if (!string.IsNullOrEmpty(searchModel.FollowStatus))
            {
                if (searchModel.FollowStatus.Equals("1"))
                {
                    dataQuery = dataQuery.Where(r => r.FollowStatus == true);
                }
                if (searchModel.FollowStatus.Equals("2"))
                {
                    dataQuery = dataQuery.Where(r => r.FollowStatus == false);
                }
            }

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

            if (!string.IsNullOrEmpty(searchModel.RecruitmentRequestId))
            {
                dataQuery = dataQuery.Where(r => r.RecruitmentRequestId.Equals(searchModel.RecruitmentRequestId));
            }

            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                dataQuery = dataQuery.Where(r => r.Email.ToLower().Contains(searchModel.Email.ToLower()));
            }

            if (searchModel.ProfileStatus.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.ProfileStatus == searchModel.ProfileStatus);
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(r => r.Status == searchModel.Status);
            }

            //if (searchModel.ApplyDateTo.HasValue)
            //{
            //    dataQuery = dataQuery.Where(r => r.ApplyDate >= searchModel.ApplyDateTo);
            //}

            if (searchModel.ApplyDateTo.HasValue)
            {
                searchModel.ApplyDateTo = searchModel.ApplyDateTo.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.ApplyDate <= searchModel.ApplyDateTo);
            }

            if (searchModel.ApplyDateFrom.HasValue)
            {
                searchModel.ApplyDateFrom = searchModel.ApplyDateFrom.Value.ToEndDate();
                dataQuery = dataQuery.Where(a => a.ApplyDate >= searchModel.ApplyDateFrom);
            }

            if (searchModel.InterviewStatus.HasValue)
            {
                if (searchModel.InterviewStatus == 0)
                {

                    dataQuery = dataQuery.Where(r => r.UnInterview > 0 || r.Total == 0);

                }
                else if (searchModel.InterviewStatus == 2)
                {
                    dataQuery = dataQuery.Where(r => r.Pass == r.Total && r.Total!=0);
                }
                else
                {
                    dataQuery = dataQuery.Where(r => r.Fail > 0);
                }
            }

            //if (searchModel.ApplyDateFrom.HasValue)
            //{
            //    DateTime applyDateFrom = searchModel.ApplyDateFrom.Value.ToEndDate();
            //    dataQuery = dataQuery.Where(r => r.ApplyDate <= applyDateFrom);
            //}
            var list = dataQuery.ToList();
            var listCandidateLanguages = db.CandidateLanguages.ToList();
            if (!string.IsNullOrEmpty(searchModel.LanguagesId))
            {
                foreach (var item in list.ToList())
                {
                    var listLanguages = listCandidateLanguages.Where(r => r.CandidateId.Equals(item.CandidatesId)).ToList();
                    int a = 0;
                    foreach (var language in listLanguages)
                    {
                        if (language.Name.Equals(searchModel.LanguagesId))
                        {
                            a++;
                        }
                    }
                    if (a == 0)
                    {
                        list.Remove(item);
                    }
                }
            }
            var listCandidateEducations = db.CandidateEducations.ToList();
            if (!string.IsNullOrEmpty(searchModel.NameTraining))
            {
                foreach (var item in list.ToList())
                {
                    var listEducations = listCandidateEducations.Where(r => r.CandidateId.Equals(item.CandidatesId)).ToList();
                    int a = 0;
                    foreach (var educations in listEducations)
                    {
                        if (educations.Name.ToLower().Contains(searchModel.NameTraining.ToLower()))
                        {
                            a++;
                        }
                    }
                    if (a == 0)
                    {
                        list.Remove(item);
                    }
                }
            }

            searchResult.TotalItem = list.Select(s => s.Id).Count();

            var listResult = list.OrderByDescending(t => t.ApplyDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            foreach (var item in listResult)
            {

                if (item.Total > 0)
                {
                    if (item.UnInterview > 0)
                    {
                        item.InterviewStatus = 0;

                    }
                    else if (item.Pass == item.Total)
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

        public SearchResultModel<ApplySearchResultsModel> SearchApplysByRecruitmentRequestId(string id)
        {
            SearchResultModel<ApplySearchResultsModel> searchResult = new SearchResultModel<ApplySearchResultsModel>();

            List<string> candidateIds = new List<string>();

            var dataQuery = (from a in db.CandidateApplies.AsNoTracking()
                             join c in db.Candidates.AsNoTracking() on a.CandidateId equals c.Id
                             join t in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals t.Id
                             join d in db.RecruitmentRequests.AsNoTracking() on a.RecruitmentRequestId equals d.Id into ad
                             from adx in ad.DefaultIfEmpty()
                             where a.ProfileStatus != Constants.CandidateApply_ProfileStatus /* || a.InterviewStatus == 1 || a.Status == 1 */
                             orderby a.ApplyDate descending
                             select new ApplySearchResultsModel
                             {
                                 Id = a.Id,
                                 ImagePath = c.ImagePath,
                                 Name = c.Name,
                                 Code = c.Code,
                                 PhoneNumber = c.PhoneNumber,
                                 Email = c.Email,
                                 ApplyDate = a.ApplyDate,
                                 Status = a.Status,
                                 //InterviewDate = a.InterviewDate,
                                 //InterviewStatus = a.InterviewStatus,
                                 ProfileStatus = a.ProfileStatus,
                                 MinApplySalary = a.MinApplySalary,
                                 MaxApplySalary = a.MaxApplySalary,
                                 WorkTypeName = t.Name,
                                 WorkTypeId = t.Id,
                                 RecruitmentRequestCode = adx != null ? adx.Code : string.Empty,
                                 RecruitmentRequestId = a.RecruitmentRequestId,
                             }).AsQueryable();

            dataQuery = dataQuery.Where(t => t.RecruitmentRequestId.Equals(id));

            searchResult.TotalItem = dataQuery.Select(s => s.Id).Count();
            searchResult.ListResult = dataQuery.ToList();

            return searchResult;
        }

        public List<CandidateSearchResultsModel> CheckCandidate(ApplyCheckCandidateModel checkCandidateModel)
        {
            var dataQuery = (from a in db.Candidates.AsNoTracking()
                             orderby a.Code
                             select new CandidateSearchResultsModel
                             {
                                 Id = a.Id,
                                 ImagePath = a.ImagePath,
                                 Name = a.Name,
                                 Code = a.Code,
                                 PhoneNumber = a.PhoneNumber,
                                 Email = a.Email,
                                 DateOfBirth = a.DateOfBirth
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(checkCandidateModel.Name))
            {
                dataQuery = dataQuery.Where(r => r.Name.ToLower().Contains(checkCandidateModel.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(checkCandidateModel.PhoneNumber))
            {
                dataQuery = dataQuery.Where(r => r.PhoneNumber.ToLower().Contains(checkCandidateModel.PhoneNumber.ToLower()));
            }

            if (!string.IsNullOrEmpty(checkCandidateModel.Email))
            {
                dataQuery = dataQuery.Where(r => r.Email.ToLower().Contains(checkCandidateModel.Email.ToLower()));
            }


            return dataQuery.ToList();
        }

        /// <summary>
        /// Thêm mới ứng tuyển
        /// </summary>
        /// <param name="applyCreateModel"></param>
        /// <param name="userId"></param>
        public CandidateCreateModel CreateApply(ApplyCreateModel applyCreateModel, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                while (db.Candidates.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(applyCreateModel.Candidate.Code.ToUpper())) != null)
                {
                    var codeModel = _candidateBusiness.GenerateCode();
                    applyCreateModel.Candidate.Code = codeModel.Code;
                    applyCreateModel.Candidate.Index = codeModel.Index;
                }
                try
                {
                    Candidate candidate;
                    if (applyCreateModel.Candidate != null && !string.IsNullOrEmpty(applyCreateModel.Candidate.Id))
                    {
                        candidate = db.Candidates.FirstOrDefault(t => t.Id.Equals(applyCreateModel.Candidate.Id));

                        if (candidate == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Candidate);
                        }

                        candidate.Name = applyCreateModel.Candidate.Name;
                        candidate.Code = applyCreateModel.Candidate.Code;
                        candidate.AcquaintanceName = applyCreateModel.Candidate.AcquaintanceName;
                        candidate.AcquaintanceNote = applyCreateModel.Candidate.AcquaintanceNote;
                        candidate.AcquaintanceStatus = applyCreateModel.Candidate.AcquaintanceStatus;
                        candidate.Address = applyCreateModel.Candidate.Address;
                        candidate.DateOfBirth = applyCreateModel.Candidate.DateOfBirth;
                        //candidate.DepartmentId = applyCreateModel.Candidate.DepartmentId;
                        candidate.Email = applyCreateModel.Candidate.Email;
                        candidate.Gender = applyCreateModel.Candidate.Gender;
                        candidate.IdentifyAddress = applyCreateModel.Candidate.IdentifyAddress;
                        candidate.IdentifyDate = applyCreateModel.Candidate.IdentifyDate;
                        candidate.IdentifyNum = applyCreateModel.Candidate.IdentifyNum;
                        candidate.ImagePath = applyCreateModel.Candidate.ImagePath;
                        candidate.PhoneNumber = applyCreateModel.Candidate.PhoneNumber;
                        candidate.RecruitmentChannelId = applyCreateModel.Candidate.RecruitmentChannelId;
                        //candidate.SBUId = applyCreateModel.Candidate.SBUId;
                        candidate.FollowStatus = applyCreateModel.FollowStatus;
                        //candidate.ProfileStatus = applyCreateModel.ProfileStatus;
                        //candidate.InterviewStatus = applyCreateModel.InterviewStatus;
                        //candidate.ApplyStatus = applyCreateModel.Status;
                        //candidate.ApplyDate = applyCreateModel.ApplyDate;
                        candidate.Index = applyCreateModel.Candidate.Index;

                        candidate.ProvinceId = applyCreateModel.Candidate.ProvinceId;
                        candidate.DistrictId = applyCreateModel.Candidate.DistrictId;
                        candidate.WardId = applyCreateModel.Candidate.WardId;

                        candidate.UpdateBy = userId;
                        candidate.UpdateDate = DateTime.Now;

                        var educations = db.CandidateEducations.Where(r => r.CandidateId.Equals(candidate.Id));
                        db.CandidateEducations.RemoveRange(educations);

                        var languages = db.CandidateLanguages.Where(r => r.CandidateId.Equals(candidate.Id));
                        db.CandidateLanguages.RemoveRange(languages);

                        var workHistories = db.CandidateWorkHistories.Where(r => r.CandidateId.Equals(candidate.Id));
                        db.CandidateWorkHistories.RemoveRange(workHistories);

                        var workTypeFits = db.CandidateWorkTypeFits.Where(r => r.CandidateId.Equals(candidate.Id));
                        db.CandidateWorkTypeFits.RemoveRange(workTypeFits);
                    }
                    else
                    {
                        candidate = new Candidate()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = applyCreateModel.Candidate.Name,
                            Code = applyCreateModel.Candidate.Code,
                            AcquaintanceName = applyCreateModel.Candidate.AcquaintanceName,
                            AcquaintanceNote = applyCreateModel.Candidate.AcquaintanceNote,
                            AcquaintanceStatus = applyCreateModel.Candidate.AcquaintanceStatus,
                            Address = applyCreateModel.Candidate.Address,
                            DateOfBirth = applyCreateModel.Candidate.DateOfBirth,
                            //DepartmentId = applyCreateModel.Candidate.DepartmentId,
                            Email = applyCreateModel.Candidate.Email,
                            Gender = applyCreateModel.Candidate.Gender,
                            IdentifyAddress = applyCreateModel.Candidate.IdentifyAddress,
                            IdentifyDate = applyCreateModel.Candidate.IdentifyDate,
                            IdentifyNum = applyCreateModel.Candidate.IdentifyNum,
                            ImagePath = applyCreateModel.Candidate.ImagePath,
                            PhoneNumber = applyCreateModel.Candidate.PhoneNumber,
                            RecruitmentChannelId = applyCreateModel.Candidate.RecruitmentChannelId,
                            //SBUId = applyCreateModel.Candidate.SBUId,
                            WardId = applyCreateModel.Candidate.WardId,
                            DistrictId = applyCreateModel.Candidate.DistrictId,
                            ProvinceId = applyCreateModel.Candidate.ProvinceId,
                            //ProfileStatus = applyCreateModel.ProfileStatus,
                            FollowStatus = applyCreateModel.FollowStatus,
                            //ApplyDate = applyCreateModel.ApplyDate,
                            //ApplyStatus = applyCreateModel.Status,
                            //InterviewStatus = applyCreateModel.InterviewStatus,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now,
                            Index = applyCreateModel.Candidate.Index
                        };

                        db.Candidates.Add(candidate);
                    }

                    //CandidateApply apply = new CandidateApply()
                    //{
                    //    Id = Guid.NewGuid().ToString(),
                    //    RecruitmentRequestId = applyCreateModel.RecruitmentRequestId,
                    //    ApplyDate = applyCreateModel.ApplyDate,
                    //    CandidateId = candidate.Id,
                    //    //InterviewStatus = applyCreateModel.InterviewStatus,
                    //    //InterviewDate = applyCreateModel.InterviewDate,
                    //    Note = applyCreateModel.Note,
                    //    ProfileStatus = applyCreateModel.ProfileStatus,
                    //    MinApplySalary = applyCreateModel.MinApplySalary,
                    //    MaxApplySalary = applyCreateModel.MaxApplySalary,
                    //    Status = applyCreateModel.Status,
                    //    WorkTypeId = applyCreateModel.WorkTypeId,
                    //    CreateBy = userId,
                    //    CreateDate = DateTime.Now,
                    //    UpdateBy = userId,
                    //    UpdateDate = DateTime.Now
                    //};

                    //db.CandidateApplies.Add(apply);

                    UpdateSubData(applyCreateModel.Candidate, candidate.Id, userId);

                    UserLogUtil.LogHistotyAdd(db, userId, candidate.Code, candidate.Id, Constants.LOG_Candidate);
                    CandidateCreateModel candidateCreateModel = new CandidateCreateModel();
                    candidateCreateModel.Id = candidate.Id;
                    db.SaveChanges();
                    trans.Commit();
                    return candidateCreateModel;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(applyCreateModel, ex);
                }
            }
        }

        public void CreateCandidateApply(CandidateApplyModel candidateapplyCreateModel, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    CandidateApply apply = new CandidateApply()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RecruitmentRequestId = candidateapplyCreateModel.RecruitmentRequestId,
                        ApplyDate = candidateapplyCreateModel.ApplyDate,
                        CandidateId = candidateapplyCreateModel.CandidateId,
                        //InterviewStatus = applyCreateModel.InterviewStatus,
                        //InterviewDate = applyCreateModel.InterviewDate,
                        Note = candidateapplyCreateModel.Note,
                        ProfileStatus = candidateapplyCreateModel.ProfileStatus,
                        MinApplySalary = candidateapplyCreateModel.MinApplySalary,
                        MaxApplySalary = candidateapplyCreateModel.MaxApplySalary,
                        Status = candidateapplyCreateModel.Status,
                        WorkTypeId = candidateapplyCreateModel.WorkTypeId,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };

                    db.CandidateApplies.Add(apply);

                    db.SaveChanges();
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(candidateapplyCreateModel, ex);
                }
            }

        }

        private void UpdateSubData(CandidateCreateModel candidateCreateModel, string candidateId, string userId)
        {
            CandidateAttach candidateAttach;
            foreach (var item in candidateCreateModel.Attachs)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    candidateAttach = new CandidateAttach()
                    {
                        CandidateId = candidateId,
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
            CandidateEducation candidateEducation;
            foreach (var item in candidateCreateModel.Educations)
            {
                candidateEducation = new CandidateEducation()
                {
                    Id = Guid.NewGuid().ToString(),
                    CandidateId = candidateId,
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
                    CandidateId = candidateId,
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
                    CandidateId = candidateId,
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
                    CandidateId = candidateId,
                    Id = Guid.NewGuid().ToString(),
                };

                db.CandidateWorkTypeFits.Add(candidateWorkTypeFit);
            }
        }

        /// <summary>
        /// Cập nhật ứng tuyển
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="applyCreateModel"></param>
        /// <param name="userId"></param>
        public void UpdateApply(string Id, ApplyCreateModel applyCreateModel, string userId)
        {
            var apply = db.CandidateApplies.FirstOrDefault(t => t.Id.Equals(Id));

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
                    //apply.InterviewDate = applyCreateModel.InterviewDate;
                    //apply.InterviewTime = JsonConvert.SerializeObject(applyCreateModel.InterviewTime);
                    //apply.InterviewStatus = applyCreateModel.InterviewStatus;
                    //apply.Note = applyCreateModel.Note;
                    //apply.ProfileStatus = applyCreateModel.ProfileStatus;
                    //apply.MinApplySalary = applyCreateModel.MinApplySalary;
                    //apply.MaxApplySalary = applyCreateModel.MaxApplySalary;
                    apply.Status = applyCreateModel.Status;
                    //apply.WorkTypeId = applyCreateModel.WorkTypeId;
                    //apply.RecruitmentRequestId = applyCreateModel.RecruitmentRequestId;
                    //apply.UpdateBy = userId;
                    //apply.UpdateDate = DateTime.Now;
                    //apply.ApplyDate = applyCreateModel.ApplyDate;

                    candidate.Name = applyCreateModel.Candidate.Name;
                    candidate.Code = applyCreateModel.Candidate.Code;
                    candidate.AcquaintanceName = applyCreateModel.Candidate.AcquaintanceName;
                    candidate.AcquaintanceNote = applyCreateModel.Candidate.AcquaintanceNote;
                    candidate.AcquaintanceStatus = applyCreateModel.Candidate.AcquaintanceStatus;
                    candidate.Address = applyCreateModel.Candidate.Address;
                    candidate.DateOfBirth = applyCreateModel.Candidate.DateOfBirth;
                    //candidate.DepartmentId = applyCreateModel.Candidate.DepartmentId;
                    candidate.Email = applyCreateModel.Candidate.Email;
                    candidate.Gender = applyCreateModel.Candidate.Gender;
                    candidate.IdentifyAddress = applyCreateModel.Candidate.IdentifyAddress;
                    candidate.IdentifyDate = applyCreateModel.Candidate.IdentifyDate;
                    candidate.IdentifyNum = applyCreateModel.Candidate.IdentifyNum;
                    candidate.ImagePath = applyCreateModel.Candidate.ImagePath;
                    candidate.PhoneNumber = applyCreateModel.Candidate.PhoneNumber;
                    candidate.RecruitmentChannelId = applyCreateModel.Candidate.RecruitmentChannelId;
                    //candidate.SBUId = applyCreateModel.Candidate.SBUId;
                    candidate.FollowStatus = applyCreateModel.FollowStatus;
                    //candidate.ProfileStatus = applyCreateModel.ProfileStatus;
                    //candidate.InterviewStatus = applyCreateModel.InterviewStatus;
                    //candidate.ApplyStatus = applyCreateModel.Status;

                    candidate.ProvinceId = applyCreateModel.Candidate.ProvinceId;
                    candidate.DistrictId = applyCreateModel.Candidate.DistrictId;
                    candidate.WardId = applyCreateModel.Candidate.WardId;

                    candidate.UpdateBy = userId;
                    candidate.UpdateDate = DateTime.Now;

                    var educations = db.CandidateEducations.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateEducations.RemoveRange(educations);

                    var languages = db.CandidateLanguages.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateLanguages.RemoveRange(languages);

                    var workHistories = db.CandidateWorkHistories.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateWorkHistories.RemoveRange(workHistories);

                    var workTypeFits = db.CandidateWorkTypeFits.Where(r => r.CandidateId.Equals(candidate.Id));
                    db.CandidateWorkTypeFits.RemoveRange(workTypeFits);

                    db.CandidateWorkTypeFits.RemoveRange(workTypeFits);

                    UpdateSubData(applyCreateModel.Candidate, candidate.Id, userId);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(applyCreateModel, ex);
                }
            }
        }

        /// <summary>
        /// Lấy thông tin ứng tuyển
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ApplyCreateModel GetApplyById(string id)
        {
            var apply = (from a in db.CandidateApplies.AsNoTracking()
                         join b in db.Candidates.AsNoTracking() on a.CandidateId equals b.Id
                         where a.Id.Equals(id)
                         select new ApplyCreateModel
                         {
                             ApplyDate = a.ApplyDate,
                             Id = a.Id,
                             //InterviewDate = a.InterviewDate,
                             //InterviewStatus = a.InterviewStatus,
                             //StrInterviewTime = a.InterviewTime,
                             Note = a.Note,
                             ProfileStatus = a.ProfileStatus,
                             MinApplySalary = a.MinApplySalary,
                             MaxApplySalary = a.MaxApplySalary,
                             Status = a.Status,
                             WorkTypeId = a.WorkTypeId,
                             CandidateId = a.CandidateId,
                             RecruitmentRequestId = a.RecruitmentRequestId,
                             FollowStatus = b.FollowStatus
                         }).FirstOrDefault();

            if (apply == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CandidateApply);
            }

            if (!string.IsNullOrEmpty(apply.StrInterviewTime))
            {
                //apply.InterviewTime = JsonConvert.DeserializeObject<object>(apply.StrInterviewTime);
            }

            apply.Candidate = (from a in db.Candidates.AsNoTracking()
                               where a.Id.Equals(apply.CandidateId)
                               select new CandidateCreateModel
                               {
                                   Id = a.Id,
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

            apply.Candidate.WorkHistories = (from m in db.CandidateWorkHistories.AsNoTracking()
                                             where m.CandidateId == apply.CandidateId
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

            apply.Candidate.Attachs = (from m in db.CandidateAttaches.AsNoTracking()
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

            apply.Candidate.Educations = (from m in db.CandidateEducations.AsNoTracking()
                                          where m.CandidateId == apply.CandidateId
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

            apply.Candidate.Languages = (from m in db.CandidateLanguages.AsNoTracking()
                                         where m.CandidateId == apply.CandidateId
                                         select new CandidateLanguageModel
                                         {
                                             Id = m.Id,
                                             Level = m.Level,
                                             Name = m.Name
                                         }).ToList();

            apply.Candidate.WorkTypes = (from m in db.CandidateWorkTypeFits.AsNoTracking()
                                         where m.CandidateId == apply.CandidateId
                                         select new CandidateWorkTypeFitModel
                                         {
                                             Id = m.Id,
                                             WorkTypeId = m.WorkTypeId
                                         }).ToList();


            string candidateApplyId = db.CandidateApplies.AsNoTracking().Where(r => r.CandidateId.Equals(apply.CandidateId)).Select(r => r.Id).FirstOrDefault();
            var interview = db.Interviews.AsNoTracking().Where(r => r.CandidateApplyId.Equals(candidateApplyId));
            int total = interview.Count();
            int unInterview = interview.Where(r => r.Status == 0).Count();
            int pass = interview.Where(r => r.Status == 2).Count();
            int fail = interview.Where(r => r.Status == 1).Count();


            return apply;
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

        public List<ApplyInterviewModel> GetInterviews(string applyId)
        {
            var interviews = (from a in db.Interviews.AsNoTracking()
                              where a.CandidateApplyId.Equals(applyId)
                              join e in db.Employees.AsNoTracking() on a.InterviewBy equals e.Id
                              select new ApplyInterviewModel
                              {
                                  Comment = a.Comment,
                                  Id = a.Id,
                                  InterviewBy = e.Name,
                                  Name = a.Name,
                                  InterviewDate = a.InterviewDate,
                                  Status = a.Status
                              }).ToList();

            return interviews;
        }

        public object GetSalaryLevelById(string workTypeId)
        {
            var interviews = (from a in db.WorkTypes.AsNoTracking()
                              where a.Id.Equals(workTypeId)
                              join b in db.SalaryLevels.AsNoTracking() on a.SalaryLevelMinId equals b.Id into ab
                              from abn in ab.DefaultIfEmpty()
                              join c in db.SalaryLevels.AsNoTracking() on a.SalaryLevelMaxId equals c.Id into ac
                              from acn in ac.DefaultIfEmpty()
                              select new
                              {
                                  SalaryLevelMin = abn != null ? abn.Salary : 0,
                                  SalaryLevelMax = acn != null ? acn.Salary : 0,
                              }).FirstOrDefault();

            return interviews;
        }

        public List<CandidateApplyModel> GetApplys(string id)
        {
            var applys = (from a in db.CandidateApplies.AsNoTracking()
                          where a.CandidateId.Equals(id)
                          join w in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals w.Id
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

        public void DeleteApply(string id, string userId)
        {
            var apply = db.CandidateApplies.FirstOrDefault(t => t.Id.Equals(id));
            if (apply == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CandidateApply);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var questions = from q in db.InterviewQuestions
                                    join i in db.Interviews on q.InterviewId equals i.Id
                                    where i.CandidateApplyId.Equals(id)
                                    select q;

                    db.InterviewQuestions.RemoveRange(questions);

                    var interviews = db.Interviews.Where(s => s.CandidateApplyId.Equals(id)).ToList();
                    db.Interviews.RemoveRange(interviews);

                    db.CandidateApplies.Remove(apply);

                    //var jsonBefore = AutoMapperConfig.Mapper.Map<CandidateApplyHistoryModel>(apply);
                    //UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_SaleProduct, apply.Id, apply.WorkTypeId, jsonBefore);

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

        public string ExportExcel(ApplySearchModel searchModel)
        {
            var data = SearchApplys(searchModel);
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

        public void AddMoreInterviews(InterviewSearchResultsModel model, string userId)
        {

            using (var trans = db.Database.BeginTransaction())
            {

                try
                {

                    Interview interview = new Interview
                    {
                        Id = Guid.NewGuid().ToString(),
                        CandidateApplyId = model.CandidateApplyId,
                        InterviewBy = model.InterviewBy,
                        InterviewDate = model.InterviewDate.Value,
                        Status = model.Status,
                        SBUId = model.SBUId,
                        DepartmentId = model.DepartmentId,
                        InterviewTime = model.InterviewTime,
                        CreateBy = userId,
                        CreateDae = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,

                    };

                    db.Interviews.Add(interview);

                    InterviewUser interviewUser;
                    foreach (var item in model.ListUser)
                    {
                        interviewUser = new InterviewUser()
                        {
                            Id = Guid.NewGuid().ToString(),
                            InterviewId = interview.Id,
                            UserId = item.Id,
                        };
                        db.InterviewUsers.Add(interviewUser);

                    }

                    InterviewQuestion interviewQuestion;
                    foreach (var item in model.Questions)
                    {
                        interviewQuestion = new InterviewQuestion()
                        {
                            Id = Guid.NewGuid().ToString(),
                            InterviewId = interview.Id,
                            //QuestionId = item.Id,
                            Answer = item.Answer,
                            Score = item.Score,
                            QuestionAnswer = item.QuestionAnswer,
                            QuestionScore = item.QuestionScore,
                            Status = item.Status,
                            QuestionType = item.Type,
                            QuestionContent = item.Content,

                        };
                        db.InterviewQuestions.Add(interviewQuestion);
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

        public void Delete(string id, string userId)
        {
            var request = db.Interviews.FirstOrDefault(t => t.Id.Equals(id));
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Interview);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var interviewsQuestion = db.InterviewQuestions.Where(a => a.InterviewId.Equals(id));
                    var interviewsUser = db.InterviewUsers.Where(a => a.InterviewId.Equals(id));

                    db.InterviewUsers.RemoveRange(interviewsUser);
                    db.InterviewQuestions.RemoveRange(interviewsQuestion);
                    db.Interviews.Remove(request);

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

        /// <summary>
        /// Cập nhật phỏng vấn
        /// </summary>
        /// <param name="userId"></param>
        public void UpdateMoreInterviews(string id, InterviewSearchResultsModel model, string userId)
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
                    interview.CandidateApplyId = model.CandidateApplyId;
                    interview.InterviewBy = model.InterviewBy;
                    interview.InterviewDate = model.InterviewDate.Value;
                    interview.Status = model.Status;
                    interview.SBUId = model.SBUId;
                    interview.DepartmentId = model.DepartmentId;
                    interview.InterviewTime = model.InterviewTime;
                    interview.UpdateBy = userId;
                    interview.UpdateDate = DateTime.Now;

                    var interviewEmployees = db.InterviewUsers.Where(r => r.InterviewId.Equals(id)).ToList();
                    if (interviewEmployees.Count > 0)
                    {
                        db.InterviewUsers.RemoveRange(interviewEmployees);
                    }
                    InterviewUser interviewUser;
                    foreach (var item in model.ListUser)
                    {
                        if (item.UserId != null)
                        {
                            interviewUser = new InterviewUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                InterviewId = interview.Id,
                                UserId = item.UserId,
                            };
                            db.InterviewUsers.Add(interviewUser);
                        }
                        else
                        {
                            interviewUser = new InterviewUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                InterviewId = interview.Id,
                                UserId = item.Id,
                            };
                            db.InterviewUsers.Add(interviewUser);

                        }
                    }

                    var interviewQuestions = db.InterviewQuestions.Where(r => r.InterviewId.Equals(id)).ToList();
                    if (interviewQuestions.Count > 0)
                    {
                        db.InterviewQuestions.RemoveRange(interviewQuestions);
                    }
                    InterviewQuestion interviewQuestion;
                    foreach (var item in model.Questions)
                    {
                        interviewQuestion = new InterviewQuestion()
                        {
                            Id = Guid.NewGuid().ToString(),
                            InterviewId = interview.Id,
                            //QuestionId = item.Id,
                            Answer = item.Answer,
                            Score = item.Score,
                            QuestionAnswer = item.QuestionAnswer,
                            QuestionScore = item.QuestionScore,
                            Status = item.Status,
                            QuestionType = item.Type,
                            QuestionContent = item.Content,
                        };
                        db.InterviewQuestions.Add(interviewQuestion);
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

        public InterviewSearchResultsModel GetInforMoreInterviews(string id)
        {
            var follows = (from a in db.Interviews.AsNoTracking()
                           where a.Id.Equals(id)
                           select new InterviewSearchResultsModel
                           {
                               CandidateApplyId = a.CandidateApplyId,
                               InterviewBy = a.InterviewBy,
                               InterviewDate = a.InterviewDate,
                               Status = a.Status,
                               SBUId = a.SBUId,
                               DepartmentId = a.DepartmentId,
                               InterviewTime = a.InterviewTime,
                           }).FirstOrDefault();

            follows.ListUser = (from a in db.InterviewUsers.AsNoTracking()
                                where a.InterviewId.Equals(id)
                                join r in db.Users on a.UserId equals r.Id
                                join b in db.Employees on r.EmployeeId equals b.Id
                                join c in db.Departments on b.DepartmentId equals c.Id
                                select new InterviewUserInfoModel
                                {
                                    Id = a.Id,
                                    InterviewId = id,
                                    Name = b.Name,
                                    Code = b.Code,
                                    Email = b.Email,
                                    PhoneNumber = b.PhoneNumber,
                                    DepartmentName = c.Name,
                                    UserId = a.UserId,
                                }).ToList();

            follows.Questions = (from a in db.InterviewQuestions.AsNoTracking()
                                 where a.InterviewId.Equals(id)
                                 //join d in db.Questions.AsNoTracking() on a.QuestionId equals d.Id
                                 //join c in db.QuestionGroups.AsNoTracking() on d.QuestionGroupId equals c.Id into dc
                                 //from acn in dc.DefaultIfEmpty()
                                 select new InterviewQuestionModel
                                 {
                                     Id = a.Id,
                                     InterviewId = id,
                                     // QuestionId = a.QuestionId,
                                     Answer = a.Answer,
                                     Score = a.Score,
                                     QuestionAnswer = a.QuestionAnswer,
                                     QuestionScore = a.QuestionScore,
                                     Status = a.Status,
                                     Type = a.QuestionType,
                                     Content = a.QuestionContent,
                                     //QuestionGroupName = acn != null ? acn.Name : string.Empty

                                 }).ToList();

            return follows;
        }

        /// <summary>
        /// Lấy thông tin vị trí ứng tuyển
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ApplyCreateModel GetCandidateApplyById(string id)
        {
            var apply = (from a in db.CandidateApplies.AsNoTracking()
                         join b in db.Candidates.AsNoTracking() on a.CandidateId equals b.Id
                         where a.Id.Equals(id)
                         select new ApplyCreateModel
                         {
                             ApplyDate = a.ApplyDate,
                             Id = a.Id,
                             //InterviewDate = a.InterviewDate,
                             //InterviewStatus = a.InterviewStatus,
                             //StrInterviewTime = a.InterviewTime,
                             Note = a.Note,
                             ProfileStatus = a.ProfileStatus,
                             MinApplySalary = a.MinApplySalary,
                             MaxApplySalary = a.MaxApplySalary,
                             Status = a.Status,
                             WorkTypeId = a.WorkTypeId,
                             CandidateId = a.CandidateId,
                             RecruitmentRequestId = a.RecruitmentRequestId,
                             FollowStatus = b.FollowStatus
                         }).FirstOrDefault();

            return apply;
        }

        public void UpdateCandidateApply(string Id, ApplyCreateModel applyCreateModel, string userId)
        {
            var apply = db.CandidateApplies.FirstOrDefault(t => t.Id.Equals(Id));

            if (apply == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.CandidateApply);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //apply.InterviewDate = applyCreateModel.InterviewDate;
                    //apply.InterviewTime = JsonConvert.SerializeObject(applyCreateModel.InterviewTime);
                    //apply.InterviewStatus = applyCreateModel.InterviewStatus;
                    apply.Note = applyCreateModel.Note;
                    apply.ProfileStatus = applyCreateModel.ProfileStatus;
                    apply.MinApplySalary = applyCreateModel.MinApplySalary;
                    apply.MaxApplySalary = applyCreateModel.MaxApplySalary;
                    apply.Status = applyCreateModel.Status;
                    apply.WorkTypeId = applyCreateModel.WorkTypeId;
                    apply.RecruitmentRequestId = applyCreateModel.RecruitmentRequestId;
                    apply.UpdateBy = userId;
                    apply.UpdateDate = DateTime.Now;
                    apply.ApplyDate = applyCreateModel.ApplyDate;


                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(applyCreateModel, ex);
                }
            }
        }


        public string InterviewExport(string id)
        {

            InterviewExportModel newModel = new InterviewExportModel();

            newModel.DateNow = DateTime.Now.ToString();
            //DateTime datevalue = (Convert.ToDateTime(newModel.DateNow.ToString()));

            //newModel.Day = datevalue.Day.ToString();
            //newModel.Month = datevalue.Month.ToString();
            //newModel.Year = datevalue.Year.ToString();

            //var data = (from a in db.Users.AsNoTracking()
            //            join b in db.Employees.AsNoTracking() on a.EmployeeId equals b.Id
            //            where a.Id.Equals(model.CreateBy)
            //            select new GeneralMechanicalModel
            //            {
            //                UserName = b.Name
            //            }).FirstOrDefault();

            var interviews = (from a in db.Interviews.AsNoTracking()
                              where a.CandidateApplyId.Equals(id)
                              join e in db.Employees.AsNoTracking() on a.InterviewBy equals e.Id
                              join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                              orderby a.InterviewDate
                              select new ApplyInterviewModel
                              {
                                  Comment = a.Comment,
                                  Id = a.Id,
                                  InterviewBy = e.Name,
                                  Name = a.Name,
                                  InterviewDate = a.InterviewDate,
                                  Status = a.Status,
                                  DepartmentName = d.Name,
                              }).ToList();
            var candidateApply = (from a in db.CandidateApplies.AsNoTracking()
                                  where a.Id.Equals(id)
                                  join b in db.Candidates.AsNoTracking() on a.CandidateId equals b.Id
                                  join c in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals c.Id
                                  select new CandidateCreateModel
                                  {
                                      Id = a.Id,
                                      Name = b.Name,
                                      WorkTypeName = c.Name,
                                      IdWorkType = c.Id,
                                      PhoneNumber = b.PhoneNumber,
                                      Gender = b.Gender,
                                      DateOfBirth = b.DateOfBirth,
                                      Email = b.Email,
                                      Address = b.Address,
                                      AcquaintanceStatus = b.AcquaintanceStatus,
                                      RecruitmentRequestId = b.RecruitmentChannelId,
                                      AcquaintanceNote = b.AcquaintanceNote,
                                      MinApplySalary = a.MinApplySalary,
                                      MaxApplySalary = a.MaxApplySalary,
                                      CandidateId = b.Id,
                                      ImagePath = b.ImagePath
                                  }).FirstOrDefault();
            if (!string.IsNullOrEmpty(candidateApply.RecruitmentRequestId)) {
                candidateApply.RecruitmentChannelName = db.RecruitmentChannels.FirstOrDefault(r => r.Id.Equals(candidateApply.RecruitmentRequestId)).Name;
            }
            WorkTypeSalaryModel workType = new WorkTypeSalaryModel();
            if (candidateApply != null)
            {
                if (!string.IsNullOrEmpty(candidateApply.IdWorkType))
                {
                    workType = (from a in db.WorkTypes.AsNoTracking()
                                join b in db.SalaryLevels.AsNoTracking() on a.SalaryLevelMinId equals b.Id
                                join c in db.SalaryLevels.AsNoTracking() on a.SalaryLevelMaxId equals c.Id
                                where a.Id.Equals(candidateApply.IdWorkType)
                                select new WorkTypeSalaryModel()
                                {
                                    MinCompanySalary = b.Salary,
                                    MaxCompanySalary = c.Salary
                                }).FirstOrDefault();
                }
            }
            WordDocument documentResult = new WordDocument();

            string templatePath = HttpContext.Current.Server.MapPath("~/Template/Xuatketquaphongvan.docx");
            string template = HttpContext.Current.Server.MapPath("~/Template/KetQuaPhongVan.docx");

            WordDocument documentInterviews = new WordDocument();//HttpContext.Current.Server.MapPath("~/Template/.docm"));

            int z = 1;
            using (WordDocument document = new WordDocument(templatePath))
            {
                var now = DateTime.Today;

                document.NTSReplaceFirst("<namecandidate>", candidateApply.Name);

                document.NTSReplaceFirst("<workType>", candidateApply.WorkTypeName);
                document.NTSReplaceFirst("<gender>", candidateApply.Gender.Equals('1') ? "Nữ" : "Nam");
                document.NTSReplaceFirst("<phoneNumber>", candidateApply.PhoneNumber);
                document.NTSReplaceFirst("<email>", candidateApply.Email);
                document.NTSReplaceFirst("<address>", candidateApply.Address);
                document.NTSReplaceFirst("<acquaintanceNote>", candidateApply.AcquaintanceNote);
                document.NTSReplaceFirst("<recruitmentChannelName>", candidateApply.RecruitmentChannelName);
                document.NTSReplaceFirst("<dateofbirth>", Convert.ToString(candidateApply.DateOfBirth));
                if (workType != null) {
                    document.NTSReplaceFirst("<minCompanySalary>", workType.MinCompanySalary == null ? "" : Convert.ToString(workType.MinCompanySalary));
                    document.NTSReplaceFirst("<maxCompanySalary>", workType.MaxCompanySalary == null ? "" : Convert.ToString(workType.MaxCompanySalary));
                }
                else
                {
                    document.NTSReplaceFirst("<minCompanySalary>", "");
                    document.NTSReplaceFirst("<maxCompanySalary>", "");
                }
                document.NTSReplaceFirst("<minApplySalary>", candidateApply.MinApplySalary == null ? "" : Convert.ToString(candidateApply.MinApplySalary));
                document.NTSReplaceFirst("<maxApplySalary>", candidateApply.MaxApplySalary == null ? "" : Convert.ToString(candidateApply.MaxApplySalary));
                document.NTSReplaceFirst("<acs1>", candidateApply.AcquaintanceStatus ? "■" : "□");
                document.NTSReplaceFirst("<acs2>", candidateApply.AcquaintanceStatus == false ? "■" : "□");
                document.NTSReplaceImage("<Ảnh>", candidateApply.ImagePath);

                var listCandidateLanguage = (from a in db.CandidateLanguages.AsNoTracking()
                                             where a.CandidateId.Equals(candidateApply.CandidateId)
                                             select new CandidateLanguageModel()
                                             {
                                                 Name = a.Name,
                                                 Level = a.Level
                                             }).ToList();
                if (listCandidateLanguage != null)
                {
                    WTable tableIndex = document.GetTableByFindText("<language>");

                    if (tableIndex != null)
                    {
                        document.NTSReplaceFirst("<language>", string.Empty);
                        WTableRow templateRow = tableIndex.Rows[1].Clone();
                        WTableRow row;
                        int index = 1;
                        foreach (var itv in listCandidateLanguage)
                        {
                            if (index > 1)
                            {
                                tableIndex.Rows.Insert(index, templateRow.Clone());
                            }
                            row = tableIndex.Rows[index];
                            row.Cells[0].Paragraphs[0].Text = itv.Name;
                            row.Cells[1].Paragraphs[0].Text = itv.Level;
                            index++;
                        }
                    }

                }

                var listCandidateEducation = (from a in db.CandidateEducations.AsNoTracking()
                                              where a.CandidateId.Equals(candidateApply.CandidateId)
                                              join b in db.Qualifications.AsNoTracking() on a.QualificationId equals b.Id into ab
                                              from abx in ab.DefaultIfEmpty()
                                              join c in db.Classifications.AsNoTracking() on a.ClassificationId equals c.Id into ac
                                              from acx in ac.DefaultIfEmpty()
                                              select new CandidateEducationModel()
                                              {
                                                  Name = a.Name,
                                                  Major = a.Major,
                                                  QualificationId = abx.Name,
                                                  Type = a.Type,
                                                  ClassificationId = acx.Name,
                                                  Time = a.Time,
                                              }).ToList();


                if (listCandidateEducation != null)
                {
                    WTable tableIndex = document.GetTableByFindText("<education>");

                    if (tableIndex != null)
                    {
                        document.NTSReplaceFirst("<education>", string.Empty);
                        WTableRow templateRow = tableIndex.Rows[1].Clone();
                        WTableRow row;
                        int index = 1;
                        foreach (var itv in listCandidateEducation)
                        {
                            if (index > 1)
                            {
                                tableIndex.Rows.Insert(index, templateRow.Clone());
                            }
                            row = tableIndex.Rows[index];
                            row.Cells[0].Paragraphs[0].Text = itv.Name == null ? "" : itv.Name;
                            row.Cells[1].Paragraphs[0].Text = itv.Major == null ? "" : itv.Major;
                            row.Cells[2].Paragraphs[0].Text = itv.QualificationId == null ? "" : itv.QualificationId;
                            row.Cells[3].Paragraphs[0].Text = itv.Type == null ? "" : itv.Type;
                            row.Cells[4].Paragraphs[0].Text = itv.ClassificationId == null ? "" : itv.ClassificationId;
                            row.Cells[5].Paragraphs[0].Text = itv.Time == null ? "" : itv.Time.ToString();
                            index++;
                        }
                        //if (listCandidateEducation.Count == 0)
                        //{
                        //    tableIndex.Rows.Remove(templateRow);
                        //}
                    }
                }

                var listCandidateWorkHistories = (from a in db.CandidateWorkHistories.AsNoTracking()
                                                  where a.CandidateId.Equals(candidateApply.CandidateId)
                                                  join b in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals b.Id into ab
                                                  from abx in ab.DefaultIfEmpty()
                                                  select new CandidateWorkHistoryModel()
                                                  {
                                                      CompanyName = a.CompanyName,
                                                      TotalTime = a.TotalTime,
                                                      StatusName = a.Status ? "Đã nghỉ việc" : "Đang làm việc",
                                                      Position = a.Position,
                                                      Income = a.Income,
                                                      ReferencePersonPhone = a.ReferencePersonPhone,
                                                      WorkTypeId = abx.Name,
                                                  }).ToList();
                if (listCandidateWorkHistories.Count != null)
                {
                    WTable tableIndex = document.GetTableByFindText("<Work>");

                    if (tableIndex != null)
                    {
                        document.NTSReplaceFirst("<Work>", string.Empty);
                        WTableRow templateRow = tableIndex.Rows[1].Clone();
                        WTableRow row;
                        int index = 1;
                        foreach (var itv in listCandidateWorkHistories)
                        {
                            if (index > 1)
                            {
                                tableIndex.Rows.Insert(index, templateRow.Clone());
                            }
                            row = tableIndex.Rows[index];
                            row.Cells[0].Paragraphs[0].Text = itv.CompanyName;
                            row.Cells[1].Paragraphs[0].Text = itv.TotalTime == null ? "0" : itv.TotalTime.ToString();
                            row.Cells[2].Paragraphs[0].Text = itv.StatusName == null ? "" : itv.StatusName;
                            row.Cells[3].Paragraphs[0].Text = itv.Position == null ? "" : itv.Position;
                            row.Cells[4].Paragraphs[0].Text = itv.Income == null ? "0" : itv.Income.ToString();
                            row.Cells[5].Paragraphs[0].Text = itv.ReferencePersonPhone == null ? "" : itv.ReferencePersonPhone.ToString();
                            row.Cells[6].Paragraphs[0].Text = itv.WorkTypeId == null ? "" : itv.WorkTypeId;
                            index++;

                        }
                        //if (listCandidateWorkHistories.Count == 0) {
                        //    tableIndex.Rows.Remove(templateRow);
                        //}
                    }
                }

                TextSelection textSelection = document.Find("<PhongVan>", false, true);
                WTextRange textRange = textSelection.GetAsOneRange();
                foreach (var item in interviews)
                {
                    using (WordDocument documentInterview = new WordDocument(template))
                    {
                        documentInterview.NTSReplaceFirst("<data>", z.ToString());
                        documentInterview.NTSReplaceFirst("<maininterviewer>", item.InterviewBy);
                        documentInterview.NTSReplaceFirst("<department>", item.DepartmentName);
                        documentInterview.NTSReplaceFirst("<date>", item.InterviewDate.ToString());
                        documentInterview.NTSReplaceFirst("<un1>", item.Status == 0 ? "■" : "□");
                        documentInterview.NTSReplaceFirst("<un2>", item.Status == 2 ? "■" : "□");
                        documentInterview.NTSReplaceFirst("<un3>", item.Status == 1 ? "■" : "□");

                        var listInterviewerUser = (from a in db.InterviewUsers.AsNoTracking()
                                                   where a.InterviewId.Equals(item.Id)
                                                   join b in db.Users.AsNoTracking() on a.UserId equals b.Id
                                                   join e in db.Employees.AsNoTracking() on b.EmployeeId equals e.Id
                                                   join d in db.Departments.AsNoTracking() on e.DepartmentId equals d.Id
                                                   orderby e.Name
                                                   select new ApplyInterviewModel()
                                                   {
                                                       Name = e.Name,
                                                       DepartmentName = d.Name,
                                                   }).ToList();

                        if (listInterviewerUser != null)
                        {
                            WTable tableIndex = documentInterview.GetTableByFindText("<employee>");

                            if (tableIndex != null)
                            {
                                documentInterview.NTSReplaceFirst("<employee>", string.Empty);
                                WTableRow templateRow = tableIndex.Rows[3].Clone();
                                WTableRow row;
                                int index = 2;
                                foreach (var itv in listInterviewerUser)
                                {
                                    if (index > 1)
                                    {
                                        tableIndex.Rows.Insert(index, templateRow.Clone());
                                    }
                                    row = tableIndex.Rows[index];
                                    row.Cells[1].Paragraphs[0].Text = itv.Name;
                                    row.Cells[2].Paragraphs[0].Text = itv.DepartmentName;
                                    index++;
                                }
                            }
                        }

                        var listInterviewerQuestion = (from a in db.InterviewQuestions.AsNoTracking()
                                                       where a.InterviewId.Equals(item.Id)
                                                       select new InterviewQuestionModel()
                                                       {
                                                           QuestionContent = a.QuestionContent,
                                                           QuestionTypeName = a.QuestionType == 1 ? "Câu hỏi Đúng/ sai" : "Câu hỏi mở",
                                                           Answer = a.QuestionType == 2 ? a.Answer : a.Answer.Equals("1") ? "Đúng" : "Sai",
                                                       }).ToList();
                        if (listInterviewerQuestion != null)
                        {
                            WTable tableIndex = documentInterview.GetTableByFindText("<content>");

                            if (tableIndex != null)
                            {
                                documentInterview.NTSReplaceFirst("<content>", string.Empty);
                                WTableRow templateRow = tableIndex.Rows[1].Clone();
                                WTableRow row;
                                int index = 1;
                                foreach (var itv in listInterviewerQuestion)
                                {
                                    if (index > 1)
                                    {
                                        tableIndex.Rows.Insert(index, templateRow.Clone());
                                    }
                                    row = tableIndex.Rows[index];
                                    row.Cells[0].Paragraphs[0].Text = index.ToString();
                                    row.Cells[1].Paragraphs[0].AppendHTML(itv.QuestionContent);
                                    row.Cells[2].Paragraphs[0].Text = itv.QuestionTypeName;
                                    row.Cells[3].Paragraphs[0].Text = itv.Answer;
                                    index++;
                                }
                            }
                            foreach (IWSection sec in documentInterview.Sections)
                            {
                               
                                documentInterviews.Sections.Add(sec.Clone());
                            }
                            documentInterview.Close();
                       }
                    }
                    z++;

                }
                document.Replace("<PhongVan>", documentInterviews, false, true, true);

                foreach (IWSection sec in document.Sections)
                {
                    documentResult.Sections.Add(sec.Clone());
                }
                document.Close();
            }


            string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Kết Quả Phỏng Vấn" + ".docx");

            documentResult.Save(pathFileSave);




            string pathreturn = "Template/" + Constants.FolderExport + "Kết Quả Phỏng Vấn" + ".docx";

            return pathreturn;
        }




        public string ApplyExport(List<ApplySearchResultsModel> model, string idCheck)
        {

            ApplySearchResultsModel newModel = new ApplySearchResultsModel();

            var interviews = model;

            WordDocument documentResult = new WordDocument();

            string templatePath = HttpContext.Current.Server.MapPath("~/Template/ThongTinUngVien.docx");
            var RecruitmentChannel = db.RecruitmentChannels.ToList();
            var listCandidate = db.Candidates.ToList();
            foreach (var item in interviews)
            {
                using (WordDocument document = new WordDocument(templatePath))
                {
                    var now = DateTime.Today;

                    var candidate = listCandidate.Where(r => r.Id.Equals(item.CandidatesId)).FirstOrDefault();
                    var RecruitmentChannelName = RecruitmentChannel.Where(r => r.Id.Equals(candidate.RecruitmentChannelId)).Select(r => r.Name).FirstOrDefault();


                    document.NTSReplaceFirst("<Name>", item.Name.ToString());
                    document.NTSReplaceFirst("<Gender>", candidate.Gender.Equals('1') ? "Nữ" : "Nam");
                    document.NTSReplaceFirst("<DateofBirht>", candidate.DateOfBirth == null ? "" : candidate.DateOfBirth.ToString());
                    document.NTSReplaceFirst("<PhoneNumber>", item.PhoneNumber.ToString());
                    document.NTSReplaceFirst("<Email>", item.Email.ToString());
                    document.NTSReplaceFirst("<Address>", candidate.Address == null ? "" : candidate.Address.ToString());
                    document.NTSReplaceFirst("<recruitment>", RecruitmentChannelName == null ? "" : RecruitmentChannelName.ToString());
                    document.NTSReplaceFirst("<Note>", candidate.AcquaintanceNote);

                    document.NTSReplaceFirst("<un1>", candidate.AcquaintanceStatus ? "■" : "□");
                    document.NTSReplaceFirst("<un2>", !candidate.AcquaintanceStatus ? "■" : "□");

                    document.NTSReplaceFirst("<ut1>", item.Status == 0 ? "■" : "□");
                    document.NTSReplaceFirst("<ut2>", item.Status == 1 ? "■" : "□");
                    document.NTSReplaceFirst("<ut3>", item.Status == 2 ? "■" : "□");

                    document.NTSReplaceFirst("<lh1>", !item.FollowStatus ? "■" : "□");
                    document.NTSReplaceFirst("<lh2>", item.FollowStatus ? "■" : "□");



                    var listCandidateLanguage = (from a in db.CandidateLanguages.AsNoTracking()
                                                 where a.CandidateId.Equals(item.CandidatesId)
                                                 select new CandidateLanguageModel()
                                                 {
                                                     Name = a.Name,
                                                     Level = a.Level
                                                 }).ToList();
                    if (listCandidateLanguage != null)
                    {
                        WTable tableIndex = document.GetTableByFindText("<language>");

                        if (tableIndex != null)
                        {
                            document.NTSReplaceFirst("<language>", string.Empty);
                            WTableRow templateRow = tableIndex.Rows[1].Clone();
                            WTableRow row;
                            int index = 1;
                            foreach (var itv in listCandidateLanguage)
                            {
                                if (index > 1)
                                {
                                    tableIndex.Rows.Insert(index, templateRow.Clone());
                                }
                                row = tableIndex.Rows[index];
                                row.Cells[0].Paragraphs[0].Text = itv.Name;
                                row.Cells[1].Paragraphs[0].Text = itv.Level;
                                index++;
                            }
                            //if (listCandidateLanguage.Count == 0)
                            //{
                            //    tableIndex.Rows.Remove(templateRow);
                            //}
                        }

                    }

                    var listCandidateEducation = (from a in db.CandidateEducations.AsNoTracking()
                                                  where a.CandidateId.Equals(item.CandidatesId)
                                                  join b in db.Qualifications.AsNoTracking() on a.QualificationId equals b.Id into ab
                                                  from abx in ab.DefaultIfEmpty()
                                                  join c in db.Classifications.AsNoTracking() on a.ClassificationId equals c.Id into ac
                                                  from acx in ac.DefaultIfEmpty()
                                                  select new CandidateEducationModel()
                                                  {
                                                      Name = a.Name,
                                                      Major = a.Major,
                                                      QualificationId = abx.Name,
                                                      Type = a.Type,
                                                      ClassificationId = acx.Name,
                                                      Time = a.Time,
                                                  }).ToList();


                    if (listCandidateEducation != null)
                    {
                        WTable tableIndex = document.GetTableByFindText("<education>");

                        if (tableIndex != null)
                        {
                            document.NTSReplaceFirst("<education>", string.Empty);
                            WTableRow templateRow = tableIndex.Rows[1].Clone();
                            WTableRow row;
                            int index = 1;
                            foreach (var itv in listCandidateEducation)
                            {
                                if (index > 1)
                                {
                                    tableIndex.Rows.Insert(index, templateRow.Clone());
                                }
                                row = tableIndex.Rows[index];
                                row.Cells[0].Paragraphs[0].Text = itv.Name == null ? "" : itv.Name;
                                row.Cells[1].Paragraphs[0].Text = itv.Major == null ? "" : itv.Major;
                                row.Cells[2].Paragraphs[0].Text = itv.QualificationId == null ? "" : itv.QualificationId;
                                row.Cells[3].Paragraphs[0].Text = itv.Type == null ? "" : itv.Type;
                                row.Cells[4].Paragraphs[0].Text = itv.ClassificationId == null ? "" : itv.ClassificationId;
                                row.Cells[5].Paragraphs[0].Text = itv.Time == null ? "" : itv.Time.ToString();
                                index++;
                            }
                            //if (listCandidateEducation.Count == 0)
                            //{
                            //    tableIndex.Rows.Remove(templateRow);
                            //}
                        }
                    }

                    var listCandidateWorkHistories = (from a in db.CandidateWorkHistories.AsNoTracking()
                                                      where a.CandidateId.Equals(item.CandidatesId)
                                                      join b in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals b.Id into ab
                                                      from abx in ab.DefaultIfEmpty()
                                                      select new CandidateWorkHistoryModel()
                                                      {
                                                          CompanyName = a.CompanyName,
                                                          TotalTime = a.TotalTime,
                                                          StatusName = a.Status ? "Đã nghỉ việc" : "Đang làm việc",
                                                          Position = a.Position,
                                                          Income = a.Income,
                                                          ReferencePersonPhone = a.ReferencePersonPhone,
                                                          WorkTypeId = abx.Name,
                                                      }).ToList();
                    if (listCandidateWorkHistories.Count != null)
                    {
                        WTable tableIndex = document.GetTableByFindText("<Work>");

                        if (tableIndex != null)
                        {
                            document.NTSReplaceFirst("<Work>", string.Empty);
                            WTableRow templateRow = tableIndex.Rows[1].Clone();
                            WTableRow row;
                            int index = 1;
                            foreach (var itv in listCandidateWorkHistories)
                            {
                                if (index > 1)
                                {
                                    tableIndex.Rows.Insert(index, templateRow.Clone());
                                }
                                row = tableIndex.Rows[index];
                                row.Cells[0].Paragraphs[0].Text = itv.CompanyName;
                                row.Cells[1].Paragraphs[0].Text = itv.TotalTime == null ? "0" : itv.TotalTime.ToString();
                                row.Cells[2].Paragraphs[0].Text = itv.StatusName == null ? "" : itv.StatusName;
                                row.Cells[3].Paragraphs[0].Text = itv.Position == null ? "" : itv.Position;
                                row.Cells[4].Paragraphs[0].Text = itv.Income == null ? "0" : itv.Income.ToString();
                                row.Cells[5].Paragraphs[0].Text = itv.ReferencePersonPhone == null ? "" : itv.ReferencePersonPhone.ToString();
                                row.Cells[6].Paragraphs[0].Text = itv.WorkTypeId == null ? "" : itv.WorkTypeId;
                                index++;

                            }
                            //if (listCandidateWorkHistories.Count == 0) {
                            //    tableIndex.Rows.Remove(templateRow);
                            //}
                        }
                    }

                    var listCandidateWorkTypeFit = (from g in db.CandidateWorkTypeFits.AsNoTracking()
                                                    where g.CandidateId.Equals(item.CandidatesId)
                                                    join x in db.WorkTypes on g.WorkTypeId equals x.Id
                                                    select x.Name).ToList();
                    if (listCandidateWorkTypeFit != null)
                    {
                        WTable tableIndex = document.GetTableByFindText("<workType>");

                        if (tableIndex != null)
                        {
                            document.NTSReplaceFirst("<workType>", string.Empty);
                            WTableRow templateRow = tableIndex.Rows[0].Clone();
                            WTableRow row;
                            int index = 0;
                            foreach (var itv in listCandidateWorkTypeFit)
                            {
                                if (index > 0)
                                {
                                    tableIndex.Rows.Insert(index, templateRow.Clone());
                                }

                                row = tableIndex.Rows[index];
                                row.Cells[0].Paragraphs[0].Text = itv;
                                index++;
                            }


                        }
                    }


                    foreach (IWSection sec in document.Sections)
                    {
                        documentResult.Sections.Add(sec.Clone());
                    }
                    document.Close();
                }

            }
            string pathFileSave = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Thông tin ứng viên" + ".docx");

            documentResult.Save(pathFileSave);




            string pathreturn = "Template/" + Constants.FolderExport + "Thông tin ứng viên" + ".docx";

            return pathreturn;
        }

    }
}
