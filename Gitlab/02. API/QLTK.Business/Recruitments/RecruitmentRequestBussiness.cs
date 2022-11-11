using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.RecruitmentRequest;
using NTS.Model.Recruitments.Applys;
using NTS.Model.Repositories;
using NTS.Model.WorkType;
using QLTK.Business.Users;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Recruitments
{
    public class RecruitmentRequestBussiness
    {
        private QLTKEntities db = new QLTKEntities();

        /// <summary>
        /// Tìm kiếm yêu cầu tuyển dụng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public SearchResultModel<RecruitmentRequestSearchResultModel> SearchRecruitmentRequest(RecruitmentRequestSearchModel searchModel)
        {
            SearchResultModel<RecruitmentRequestSearchResultModel> searchResult = new SearchResultModel<RecruitmentRequestSearchResultModel>();
            var dataQuery = (from a in db.RecruitmentRequests.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             join c in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals c.Id
                             select new RecruitmentRequestSearchResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = b.Name,
                                 WorkTypeId = a.WorkTypeId,
                                 WorkTypeName = c.Name,
                                 ApprovalDate = a.ApprovalDate,
                                 RecruitmentDeadline = a.RecruitmentDeadline,
                                 Description = a.Description,
                                 Equipment = a.Equipment,
                                 RecruitmentReason = a.RecruitmentReason,
                                 FinishDate = a.FinishDate,
                                 Quantity = a.Quantity,
                                 Request = a.Request,
                                 RequestDate = a.RequestDate,
                                 MinSalary = a.MinSalary,
                                 MaxSalary = a.MaxSalary,
                                 MinCompanySalary = a.MinCompanySalary,
                                 MaxCompanySalary = a.MaxCompanySalary,
                                 Type = a.Type,
                                 Status = a.Status,
                                 CreateDate = a.CreateDate,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.WorkTypeId))
            {
                dataQuery = dataQuery.Where(u => u.WorkTypeId.Equals(searchModel.WorkTypeId));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status.Value);
            }
            

            //searchResult.TotalItem = dataQuery.Count();
            var list = dataQuery.ToList();
            //var listResult = SQLHelpper.OrderBy(dataQuery, searchModel.OrderBy, searchModel.OrderType).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            foreach (var item in list)
            {
                item.NumberRecruitment = (from a in db.CandidateApplies.AsNoTracking()
                                          where a.RecruitmentRequestId.Equals(item.Id)
                                          select a.Id).ToList().Count();
                item.StatusRecruit = 2;
                if (item.Status != 1 && item.RecruitmentDeadline < DateTime.Now)
                {
                    item.NumberLateDate = (DateTime.Now - (DateTime)item.RecruitmentDeadline).Days;
                    item.StatusRecruit = 0;
                }
                else if (item.Status == 1 && item.RecruitmentDeadline < item.FinishDate)
                {
                    item.NumberLateDate = ((DateTime)item.FinishDate - (DateTime)item.RecruitmentDeadline).Days;
                }
                if (item.Status != 1 && item.RecruitmentDeadline >= DateTime.Now)
                {
                    item.StatusRecruit = 1;
                }
                if (item.Status == 2)
                {
                    item.StatusRecruit = 3;
                }
                item.NumberCandidate = (from a in db.CandidateApplies.AsNoTracking()
                                        where a.RecruitmentRequestId.Equals(item.Id)
                                        && a.ProfileStatus == Constants.CandidateApply_ProfileStatus
                                        select a.Id).ToList().Count();

                item.NumberInterview = (from a in db.Interviews.AsNoTracking()
                                        join b in db.CandidateApplies on a.CandidateApplyId equals b.Id
                                        join c in db.RecruitmentRequests on b.RecruitmentRequestId equals item.Id
                                        group a.Id by a.CandidateApplyId into g
                                        select g.Key).ToList().Count();


                var numberCandidateAcept = (from a in db.Candidates.AsNoTracking()
                                            join b in db.CandidateApplies.AsNoTracking() on a.Id equals b.CandidateId
                                            join t in db.WorkTypes.AsNoTracking() on b.WorkTypeId equals t.Id
                                            where b.ProfileStatus == Constants.CandidateApply_ProfileStatus && b.RecruitmentRequestId == item.Id
                                            && b.Status == 2
                                            select b.Id).ToList().Count();
                item.NumberNeedRecruit = item.Quantity - numberCandidateAcept;
            }

            if (searchModel.StatusRecruit.HasValue)
            {
                if (searchModel.StatusRecruit.Value == 0)
                {
                    //list = list.Where(u => (u.Status == 0 && u.RecruitmentDeadline < DateTime.Now)).ToList();
                    list = list.Where(u => u.StatusRecruit == 0).ToList();
                }
                if (searchModel.StatusRecruit.Value == 1)
                {
                    list = list.Where(u => u.StatusRecruit == 1).ToList();
                }
                if (searchModel.StatusRecruit.Value == 2)
                {
                    list = list.Where(u => u.StatusRecruit == 2).ToList();
                }
                if (searchModel.StatusRecruit.Value == 3)
                {
                    list = list.Where(u => u.StatusRecruit == 3).ToList();
                }
            }
            searchResult.TotalItem = list.Count();
            searchResult.Status1 = list.Where(r => r.StatusRecruit.Equals(0)).Count();
            searchResult.Status2 = list.Where(r => r.StatusRecruit.Equals(1)).Count();
            searchResult.Status3 = list.Where(r => r.StatusRecruit.Equals(2)).Count();
            searchResult.Status4 = list.Where(r => r.StatusRecruit.Equals(3)).Count();

            var listResult = list.OrderByDescending(t => t.Code).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            searchResult.ListResult = listResult;

            return searchResult;
        }

        /// <summary>
        /// Tự động tạo mã xuất giữ
        /// </summary>
        /// <returns></returns>
        public RecruitmentRequestCodeModel GenerateCode()
        {
            var dateNow = DateTime.Now;
            string code = "";
            var maxIndex = db.RecruitmentRequests.AsNoTracking().Select(r => r.Index).DefaultIfEmpty(0).Max();
            maxIndex++;
            code = $"YC.{string.Format("{0:0000}", maxIndex)}";

            return new RecruitmentRequestCodeModel
            {
                Code = code,
                Index = maxIndex
            };
        }

        public WorkTypeSalaryModel GetCompanySalary(string id)
        {
            var workType = (from a in db.WorkTypes.AsNoTracking()
                            join b in db.SalaryLevels.AsNoTracking() on a.SalaryLevelMinId equals b.Id
                            join c in db.SalaryLevels.AsNoTracking() on a.SalaryLevelMaxId equals c.Id
                            where a.Id.Equals(id)
                            select new WorkTypeSalaryModel() {
                                MinCompanySalary = b.Salary,
                                MaxCompanySalary = c.Salary
                            }).FirstOrDefault();
            return workType;
        }

        /// <summary>
        /// Thêm mới yêu cầu tuyển dụng
        /// </summary>
        /// <param name="model"></param>
        public void CreateRecruitmentRequest(RecruitmentRequestCreateModel model)
        {
            while (db.RecruitmentRequests.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper())) != null)
            {
                var codeModel = GenerateCode();
                model.Code = codeModel.Code;
                model.Index = codeModel.Index;
            }

            try
            {
                RecruitmentRequest request = new RecruitmentRequest
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = model.Code,
                    DepartmentId = model.DepartmentId,
                    WorkTypeId = model.WorkTypeId,
                    Quantity = model.Quantity,
                    RecruitmentDeadline = model.RecruitmentDeadline,
                    Type = model.Type,
                    MinSalary = model.MinSalary,
                    MaxSalary = model.MaxSalary,
                    MinCompanySalary = model.MinCompanySalary,
                    MaxCompanySalary = model.MaxCompanySalary,
                    RecruitmentReason = model.RecruitmentReason,
                    Description = model.Description,
                    Request = model.Request,
                    Equipment = model.Equipment,
                    RequestDate = model.RequestDate,
                    ApprovalDate = model.ApprovalDate,
                    Index = model.Index,
                    CreateBy = model.CreateBy,
                    CreateDate = DateTime.Now,
                    UpdateBy = model.CreateBy,
                    UpdateDate = DateTime.Now
                };

                db.RecruitmentRequests.Add(request);

                UserLogUtil.LogHistotyAdd(db, model.CreateBy, request.Code, request.Id, Constants.LOG_RecruitmentRequest);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new NTSLogException(model, ex);
            }
        }

        /// <summary>
        /// Chi tiết yêu cầu tuyển dụng
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public RecruitmentRequestInfoModel GetRecruitmentRequestById(string id)
        {
            var request = (from a in db.RecruitmentRequests.AsNoTracking()
                           where a.Id.Equals(id)
                           select new RecruitmentRequestInfoModel
                           {
                               Id = a.Id,
                               Code = a.Code,
                               DepartmentId = a.DepartmentId,
                               WorkTypeId = a.WorkTypeId,
                               Quantity = a.Quantity,
                               RecruitmentDeadline = a.RecruitmentDeadline,
                               Type = a.Type,
                               MinSalary = a.MinSalary,
                               RecruitmentReason = a.RecruitmentReason,
                               Description = a.Description,
                               Request = a.Request,
                               Equipment = a.Equipment,
                               RequestDate = a.RequestDate,
                               Index = a.Index,
                               Status = a.Status,
                               UpdateBy = a.UpdateBy,
                               ApprovalDate = a.ApprovalDate,
                               MaxSalary = a.MaxSalary,
                               MinCompanySalary = a.MinCompanySalary,
                               MaxCompanySalary = a.MaxCompanySalary,
                           }).FirstOrDefault();

            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            }

            request.ListAttach = (from m in db.RecruitmentRequestAttaches.AsNoTracking()
                                  where m.RecruitmentRequestId.Equals(id)
                                  join u in db.Users.AsNoTracking() on m.CreateBy equals u.Id
                                  join e in db.Employees.AsNoTracking() on u.EmployeeId equals e.Id
                                  select new RecruitmentRequestAttachModel
                                  {
                                      Id = m.Id,
                                      FileName = m.FileName,
                                      FilePath = m.FilePath,
                                      FileSize = m.FileSize,
                                      CreateDate = m.CreateDate,
                                      CreateName = e.Name
                                  }).ToList();

            return request;
        }

        public void UpdateRecruitmentRequest(string id, RecruitmentRequestInfoModel model, string userId)
        {
            var checkCode = db.RecruitmentRequests.AsNoTracking().FirstOrDefault(r => !r.Id.Equals(id) && r.Code.ToLower().Equals(model.Code.ToLower()));

            if (checkCode != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.RecruitmentRequest);
            }

            var request = db.RecruitmentRequests.FirstOrDefault(t => t.Id.Equals(id));

            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    request.Code = model.Code;
                    request.DepartmentId = model.DepartmentId;
                    request.WorkTypeId = model.WorkTypeId;
                    request.Quantity = model.Quantity;
                    request.RecruitmentDeadline = model.RecruitmentDeadline;
                    request.Type = model.Type;
                    request.MinSalary = model.MinSalary;
                    request.MaxSalary = model.MaxSalary;
                    request.MinCompanySalary = model.MinCompanySalary;
                    request.MaxCompanySalary = model.MaxCompanySalary;
                    request.RecruitmentReason = model.RecruitmentReason;
                    request.Description = model.Description;
                    request.Request = model.Request;
                    request.Equipment = model.Equipment;
                    request.RequestDate = model.RequestDate;
                    request.ApprovalDate = model.ApprovalDate;

                    request.UpdateBy = userId;
                    request.UpdateDate = DateTime.Now;

                    RecruitmentRequestAttach attach;
                    foreach (var item in model.ListAttach)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            attach = new RecruitmentRequestAttach()
                            {
                                RecruitmentRequestId = request.Id,
                                FileSize = item.FileSize,
                                Id = Guid.NewGuid().ToString(),
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                FileName = item.FileName,
                                FilePath = item.FilePath,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            };

                            db.RecruitmentRequestAttaches.Add(attach);
                        }
                        else
                        {
                            attach = db.RecruitmentRequestAttaches.FirstOrDefault(r => r.Id.Equals(item.Id));

                            if (attach != null)
                            {
                                if (item.IsDelete)
                                {
                                    db.RecruitmentRequestAttaches.Remove(attach);
                                }
                                else
                                {
                                    attach.FileName = item.FileName;
                                    attach.FilePath = item.FilePath;
                                    attach.FileSize = item.FileSize;
                                    attach.UpdateBy = userId;
                                    attach.UpdateDate = DateTime.Now;
                                }
                            }
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

        public void DeleteRecruitmentRequest(string id, string userId)
        {
            var request = db.RecruitmentRequests.FirstOrDefault(t => t.Id.Equals(id));
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var attachs = db.RecruitmentRequestAttaches.Where(t => t.RecruitmentRequestId.Equals(id));
                    db.RecruitmentRequestAttaches.RemoveRange(attachs);
                    db.RecruitmentRequests.Remove(request);

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

        public void CancelStatus(string id, string userId)
        {
            var request = db.RecruitmentRequests.FirstOrDefault(t => t.Id.Equals(id));

            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    request.Status = 2;
                    request.FinishDate = DateTime.Now;
                    request.UpdateBy = userId;
                    request.UpdateDate = DateTime.Now;

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

        public void NextStatus(string id, string userId)
        {
            var request = db.RecruitmentRequests.FirstOrDefault(t => t.Id.Equals(id));

            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    request.Status++;
                    request.FinishDate = DateTime.Now;
                    request.UpdateBy = userId;
                    request.UpdateDate = DateTime.Now;

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

        public void BackStatus(string id, string userId)
        {
            var request = db.RecruitmentRequests.FirstOrDefault(t => t.Id.Equals(id));

            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    request.Status--;
                    request.FinishDate = null;
                    request.UpdateBy = userId;
                    request.UpdateDate = DateTime.Now;

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

        public string GetWorkTypeByRequestId(string id)
        {
            var request = db.RecruitmentRequests.AsNoTracking().FirstOrDefault(t => t.Id.Equals(id));
            if (request == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            }
            return request.WorkTypeId;
        }

        public RecruitmentRequestSalaryModel GetSalaryByRequestId(string id)
        {
            var salary = db.RecruitmentRequests.AsNoTracking().Where(t => t.Id.Equals(id)).Select(t=> new RecruitmentRequestSalaryModel { 
                MaxSalary = t.MaxSalary,
                MinSalary = t.MinSalary
            }).FirstOrDefault();
            //if (salary == null)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.RecruitmentRequest);
            //}
            return salary;
        }

        ///// <summary>
        ///// Thêm mới yêu cầu tuyển dụng
        ///// </summary>
        ///// <param name="model"></param>
        //public void AddMoreInterviews(MoreInterviewsModel model)
        //{
        //    //var request = db.CandidateApplies.FirstOrDefault(t => t.Id.Equals(model.CandidateApplyId));
        //    //using (var trans = db.Database.BeginTransaction())
        //    //{
        //    //    try
        //    //    {

        //    //        request.InterviewDate = model.InterviewDate;
        //    //        request.InterviewTime = model.InterviewTime;

        //    //        try
        //    //        {
        //    //            Interview interview = new Interview()
        //    //            {
        //    //                Id = Guid.NewGuid().ToString(),
        //    //                CandidateApplyId = model.CandidateApplyId,
        //    //                InterviewDate = model.InterviewDate.Value,
        //    //                SBUId =  model.SBUId,
        //    //                DepartmentId = model.DepartmentId,
        //    //            };
        //    //        }
        //    //        catch (Exception ex)
        //    //        {
        //    //            throw new NTSLogException(model, ex);
        //    //        }


        //    //        db.SaveChanges();
        //    //        trans.Commit();
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        trans.Rollback();
        //    //        throw new NTSLogException(model, ex);
        //    //    }
        //    //}

        //    try
        //    {
        //        Interview interview = new Interview()
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            CandidateApplyId = model.CandidateApplyId,
        //            InterviewDate = model.InterviewDate.Value,
        //            SBUId = model.SBUId,
        //            DepartmentId = model.DepartmentId,
        //        };
        //        db.Interviews.Add(interview);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new NTSLogException(model, ex);
        //    }


        //}

        public string ExportExcel(RecruitmentRequestSearchModel searchModel)
        {
            SearchResultModel<RecruitmentRequestSearchModel> searchResult = new SearchResultModel<RecruitmentRequestSearchModel>();
            var dataQuery = (from a in db.RecruitmentRequests.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                             join c in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals c.Id
                             orderby a.Code
                             select new RecruitmentRequestSearchResultModel
                             {
                                 Id = a.Id,
                                 Code = a.Code,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = b.Name,
                                 WorkTypeId = a.WorkTypeId,
                                 WorkTypeName = c.Name,
                                 ApprovalDate = a.ApprovalDate,
                                 RecruitmentDeadline = a.RecruitmentDeadline,
                                 Description = a.Description,
                                 Equipment = a.Equipment,
                                 RecruitmentReason = a.RecruitmentReason,
                                 FinishDate = a.FinishDate,
                                 Quantity = a.Quantity,
                                 Request = a.Request,
                                 RequestDate = a.RequestDate,
                                 MinSalary = a.MinSalary,
                                 MaxSalary = a.MaxSalary,
                                 MinCompanySalary = a.MinCompanySalary,
                                 MaxCompanySalary = a.MaxCompanySalary,
                                 Type = a.Type,
                                 Status = a.Status,
                                 CreateDate = a.CreateDate,
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                dataQuery = dataQuery.Where(u => u.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.WorkTypeId))
            {
                dataQuery = dataQuery.Where(u => u.WorkTypeId.Equals(searchModel.WorkTypeId));
            }

            if (searchModel.Status.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.Status == searchModel.Status.Value);
            }
            if (searchModel.StatusRecruit.HasValue)
            {
                if (searchModel.StatusRecruit.Value == 0)
                {
                    dataQuery = dataQuery.Where(u => (u.Status == 0 && u.RecruitmentDeadline < DateTime.Now));
                }
                if (searchModel.StatusRecruit.Value == 1)
                {
                    dataQuery = dataQuery.Where(u => u.Status == 0 && u.RecruitmentDeadline >= DateTime.Now);
                }
                if (searchModel.StatusRecruit.Value == 2)
                {
                    dataQuery = dataQuery.Where(u => u.Status == 1 && u.FinishDate < u.RecruitmentDeadline);
                }
                if (searchModel.StatusRecruit.Value == 3)
                {
                    dataQuery = dataQuery.Where(u => u.Status == 2);
                }
            }

            searchResult.TotalItem = dataQuery.Count();
            var list = dataQuery.ToList();
            var listResult = dataQuery.ToList();
            foreach (var item in listResult)
            {
                item.NumberRecruitment = (from a in db.CandidateApplies.AsNoTracking()
                                          where a.RecruitmentRequestId.Equals(item.Id)
                                          select a.Id).ToList().Count();
                item.StatusRecruit = 2;
                if (item.Status != 1 && item.RecruitmentDeadline < DateTime.Now)
                {
                    item.NumberLateDate = (DateTime.Now - (DateTime)item.RecruitmentDeadline).Days;
                    item.StatusRecruit = 0;
                }
                else if (item.Status == 1 && item.RecruitmentDeadline < item.FinishDate)
                {
                    item.NumberLateDate = ((DateTime)item.FinishDate - (DateTime)item.RecruitmentDeadline).Days;
                }
                if (item.Status != 1 && item.RecruitmentDeadline >= DateTime.Now)
                {
                    item.StatusRecruit = 1;
                }
                if (item.Status == 2)
                {
                    item.StatusRecruit = 3;
                }
                item.NumberCandidate = (from a in db.CandidateApplies.AsNoTracking()
                                        where a.RecruitmentRequestId.Equals(item.Id)
                                        && a.ProfileStatus == Constants.CandidateApply_ProfileStatus
                                        select a.Id).ToList().Count();

                item.NumberInterview = (from a in db.Interviews.AsNoTracking()
                                        join b in db.CandidateApplies on a.CandidateApplyId equals b.Id
                                        join c in db.RecruitmentRequests on b.RecruitmentRequestId equals item.Id
                                        group a.Id by a.CandidateApplyId into g
                                        select g.Key).ToList().Count();


                var numberCandidateAcept = (from a in db.Candidates.AsNoTracking()
                                            join b in db.CandidateApplies.AsNoTracking() on a.Id equals b.CandidateId
                                            join t in db.WorkTypes.AsNoTracking() on b.WorkTypeId equals t.Id
                                            where b.ProfileStatus == Constants.CandidateApply_ProfileStatus && b.RecruitmentRequestId == item.Id
                                            && b.Status == 2
                                            select b.Id).ToList().Count();
                item.NumberNeedRecruit = item.Quantity - numberCandidateAcept;
            }
            List<RecruitmentRequestSearchResultModel> listModel = listResult.ToList();


            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0039, TextResourceKey.RecruitmentRequest);
            }
            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/RecruitmentRequestExport.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = listModel.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);


                int index = 1;

                List<RecruitmentRequestExportModel> listExport = new List<RecruitmentRequestExportModel>();
                foreach (var item in listResult)
                {

                    listExport.Add(new RecruitmentRequestExportModel()
                    {
                        Index = index++.ToString(),
                        StatusRecruit = item.StatusRecruit == 0 ? "Trễ": item.StatusRecruit == 1 ? "Chưa xong" : item.StatusRecruit == 2 ? "Đã hoàn thành": "Hủy yêu cầu",
                        Code = item.Code,
                        DepartmentName = item.DepartmentName,
                        WorkTypeName = item.WorkTypeName,
                        Quantity = item.Quantity,
                        RecruitmentDeadline = item.RecruitmentDeadline.ToString("dd/MM/yyyy"),
                        TypeName = item.Type == 1 ? "Toàn thời gian" : "Bán thời gian",
                        MinSalary = item.MinSalary,
                        MaxSalary = item.MaxSalary,
                        RecruitmentReason = item.RecruitmentReason,
                        Description = item.Description,
                        Request = item.Request,
                        Equipment = item.Equipment,
                        ApprovalDate = item.ApprovalDate == null ? "":((DateTime)item.ApprovalDate).ToString("dd/MM/yyy"),
                        RequestDate = item.RequestDate.ToString("dd/MM/yyyy"),
                        FinishDate = item.FinishDate == null? "":((DateTime)item.FinishDate).ToString("dd/MM/yyyy"),
                        NumberRecruitment = item.NumberRecruitment,
                        NumberCandidate = item.NumberCandidate,
                        NumberInterview = item.NumberInterview,
                        NumberNeedRecruit = item.NumberNeedRecruit,
                        NumberLateDate = item.NumberLateDate,

                    });


                }
                if (listExport.Count() > 1)
                {
                    sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
                }
                sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 22].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 22].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 22].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 22].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 22].Borders.Color = ExcelKnownColors.Black;
                //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].CellStyle.WrapText = true;


                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách Yêu cầu tuyển dụng" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách Yêu cầu tuyển dụng" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }
    }
    
}
