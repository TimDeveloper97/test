using NTS.Common;
using NTS.Common.Logs;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Error;
using NTS.Model.ErrorAttach;
using NTS.Model.ErrorHistory;
using NTS.Model.ErrorImage;
using NTS.Model.ErrorLogHistory;
using NTS.Model.Errors.ErrorChangePlan;
using NTS.Model.Mobile;
using NTS.Model.Mobile.Error;
using NTS.Model.Notify;
using NTS.Model.QLTKMODULE;
using NTS.Model.Repositories;
using QLTK.Business.AutoMappers;
using QLTK.Business.Notify;
using QLTK.Business.Users;
using RabbitMQ.Client.Framing.Impl;
using Syncfusion.XlsIO;
using Syncfusion.XPS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace QLTK.Business.Errors
{
    public class ErrorBusiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private NotifyBussiness _notifyBusiness = new NotifyBussiness();
        public SearchResultModel<ErrorModel> SearchError(ErrorSearchModel modelSearch)
        {
            SearchResultModel<ErrorModel> searchResult = new SearchResultModel<ErrorModel>();

            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
                             from c in ac.DefaultIfEmpty()
                             join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
                             from d in ad.DefaultIfEmpty()
                             join e in db.Departments.AsNoTracking() on a.DepartmentId equals e.Id into ae
                             from e in ae.DefaultIfEmpty()
                             join f in db.Stages.AsNoTracking() on a.StageId equals f.Id into af
                             from f in af.DefaultIfEmpty()
                             join g in db.Modules.AsNoTracking() on a.ObjectId equals g.Id into ag
                             from g in ag.DefaultIfEmpty()
                             join k in db.Products.AsNoTracking() on a.ObjectId equals k.Id into ak
                             from ka in ak.DefaultIfEmpty()
                                 //join i in db.Employees.AsNoTracking() on a.FixBy equals i.Id into ai
                                 //from i in ai.DefaultIfEmpty()
                             join h in db.Employees.AsNoTracking() on a.ErrorBy equals h.Id into ah
                             from h in ah.DefaultIfEmpty()
                                 //join j in db.Departments.AsNoTracking() on a.DepartmentProcessId equals j.Id into aj
                                 //from j in aj.DefaultIfEmpty()
                             where a.Type == 1
                             select new ErrorModel
                             {
                                 Id = a.Id,
                                 Subject = a.Subject,
                                 Code = a.Code,
                                 ErrorGroupId = a.ErrorGroupId,
                                 ErrorGroupName = b.Name,
                                 ErrorGroupCode = b.Code,
                                 AuthorId = a.AuthorId,
                                 AuthorName = c.Name,
                                 PlanStartDate = a.PlanStartDate,
                                 PlanFinishDate = a.PlanFinishDate,
                                 ObjectId = a.ObjectId,
                                 ProjectId = a.ProjectId,
                                 ProjectName = d.Name,
                                 Status = a.Status,
                                 ModuleErrorVisualId = a.ModuleErrorVisualId,
                                 ModuleErrorVisualName = g.Name,
                                 ModuleErrorVisualCode = g.Code,
                                 DepartmentId = a.DepartmentId,
                                 DepartmentName = e.Name,
                                 ErrorBy = a.ErrorBy,
                                 ErrorByName = h.Name,
                                 DepartmentProcessId = a.DepartmentProcessId,
                                 //DepartmentProcessName = j.Name,
                                 StageId = a.StageId,
                                 StageName = f.Name,
                                 FixBy = a.FixBy,
                                 //FixByName = i.Name,
                                 Note = a.Note,
                                 ErrorCost = a.ErrorCost,
                                 Description = a.Description,
                                 CreateDate = a.CreateDate,
                                 CreateBy = a.CreateBy,
                                 UpdateBy = a.UpdateBy,
                                 UpdateDate = a.UpdateDate,
                                 ProductCode = ka != null ? ka.Code : string.Empty,
                                 ProductName = ka != null ? ka.Name : string.Empty
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.ErrorGroupId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.ErrorGroupId.Equals(u.ErrorGroupId));
            }

            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                dataQuery = dataQuery.Where(u => u.Subject.ToUpper().Contains(modelSearch.NameCode.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.NameCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.DepartmentId.Equals(u.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ErrorBy))
            {
                dataQuery = dataQuery.Where(u => modelSearch.ErrorBy.Equals(u.ErrorBy));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentProcessId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.DepartmentProcessId.Equals(u.DepartmentProcessId));
            }

            if (!string.IsNullOrEmpty(modelSearch.FixBy))
            {
                dataQuery = dataQuery.Where(u => modelSearch.FixBy.Equals(u.FixBy));
            }

            if (!string.IsNullOrEmpty(modelSearch.StageId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.StageId.Equals(u.StageId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ObjectId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.ObjectId.Equals(u.ObjectId));
            }

            if (modelSearch.DateOpen != null)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate >= modelSearch.DateOpen);
            }

            if (modelSearch.DateEnd != null)
            {
                dataQuery = dataQuery.Where(r => r.PlanStartDate <= modelSearch.DateEnd);
            }

            if (modelSearch.Status != 0)
            {
                dataQuery = dataQuery.Where(r => r.Status == modelSearch.Status);
            }


            if (!string.IsNullOrEmpty(modelSearch.AuthorId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.AuthorId.Equals(u.AuthorId));
            }

            if (!string.IsNullOrEmpty(modelSearch.Description))
            {
                dataQuery = dataQuery.Where(u => !string.IsNullOrEmpty(u.Description) && u.Description.ToLower().Contains(modelSearch.Description.ToLower()));
            }

            searchResult.TotalItem = dataQuery.Count();
            searchResult.Status1 = dataQuery.Where(r => r.Status.Equals(1)).Count();
            searchResult.Status2 = dataQuery.Where(r => r.Status.Equals(2)).Count();
            searchResult.Status3 = dataQuery.Where(r => r.Status.Equals(3)).Count();
            searchResult.Status4 = dataQuery.Where(r => r.Status.Equals(4)).Count();
            var listResult = SQLHelpper.OrderBy(dataQuery, modelSearch.OrderBy, modelSearch.OrderType).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        /// <summary>
        /// Tìm kiếm vấn đề
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <param name="departmentId">Id phòng ban người đăng nhập</param>
        /// <returns></returns>
        public SearchResultErrorModel SearchProblemExist(ErrorSearchModel modelSearch, string departmentId, bool isAll)
        {
            SearchResultErrorModel searchResult = new SearchResultErrorModel();
            DateTime dateNow = DateTime.Now;
            var dataQuery = this.MakeWhereCondition(modelSearch, departmentId, isAll);

            searchResult.TotalItem = dataQuery.Count();
            searchResult.ErrorStatus1 = dataQuery.Where(r => r.Status == 1 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus2 = dataQuery.Where(r => r.Status == 2 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus3 = dataQuery.Where(r => r.Status == 3 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus4 = dataQuery.Where(r => r.Status == 4 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus5 = dataQuery.Where(r => r.Status == 5 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus6 = dataQuery.Where(r => r.Status == 6 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus7 = dataQuery.Where(r => r.Status == 7 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus8 = dataQuery.Where(r => r.Status == 8 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus9 = dataQuery.Where(r => r.Status == 9 && r.Type == Constants.Error_Type_Error).Count();
            searchResult.ErrorStatus10 = dataQuery.Where(r => r.Status == 10 && r.Type == Constants.Error_Type_Error).Count();

            searchResult.ProblemStatus1 = dataQuery.Where(r => r.Status == 1 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus2 = dataQuery.Where(r => r.Status == 2 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus3 = dataQuery.Where(r => r.Status == 3 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus4 = dataQuery.Where(r => r.Status == 4 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus5 = dataQuery.Where(r => r.Status == 5 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus6 = dataQuery.Where(r => r.Status == 6 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus7 = dataQuery.Where(r => r.Status == 7 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus8 = dataQuery.Where(r => r.Status == 8 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus9 = dataQuery.Where(r => r.Status == 9 && r.Type == Constants.Error_Type_Issue).Count();
            searchResult.ProblemStatus10 = dataQuery.Where(r => r.Status == 10 && r.Type == Constants.Error_Type_Issue).Count();

            searchResult.Type1 = dataQuery.Where(a => a.Type == 1).Count();
            searchResult.Type2 = dataQuery.Where(a => a.Type == 2).Count();

            var listResult = dataQuery.OrderBy(r => r.Status).ThenBy(r => r.PlanFinishDate).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).Select(s => s.Id).ToList();

            searchResult.Errors = (from a in db.Errors.AsNoTracking()
                                   where listResult.Contains(a.Id)
                                   join b in db.ErrorGroups.AsNoTracking() on a.ErrorGroupId equals b.Id into ab
                                   from ba in ab.DefaultIfEmpty()
                                   join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id into ac
                                   from acn in ac.DefaultIfEmpty()
                                   join d in db.Projects.AsNoTracking() on a.ProjectId equals d.Id into ad
                                   from adn in ad.DefaultIfEmpty()
                                   join e in db.Departments.AsNoTracking() on a.DepartmentId equals e.Id into ae
                                   from aen in ae.DefaultIfEmpty()
                                   join f in db.Stages.AsNoTracking() on a.StageId equals f.Id into af
                                   from afn in af.DefaultIfEmpty()
                                   join g in db.Modules.AsNoTracking() on a.ObjectId equals g.Id into ag
                                   from agn in ag.DefaultIfEmpty()
                                       //join i in db.Employees.AsNoTracking() on a.FixBy equals i.Id into ai
                                       //from ain in ai.DefaultIfEmpty()
                                   join h in db.Employees.AsNoTracking() on a.ErrorBy equals h.Id into ah
                                   from ahn in ah.DefaultIfEmpty()
                                       //join j in db.Departments.AsNoTracking() on a.DepartmentProcessId equals j.Id into aj
                                       //from ajn in aj.DefaultIfEmpty()
                                   join k in db.Products.AsNoTracking() on a.ObjectId equals k.Id into ak
                                   from ka in ak.DefaultIfEmpty()
                                   join j in db.Departments.AsNoTracking() on acn.DepartmentId equals j.Id into acnj
                                   from acnjx in acnj.DefaultIfEmpty()
                                   orderby a.Status, a.PlanFinishDate
                                   select new ErrorModel
                                   {
                                       Id = a.Id,
                                       Subject = a.Subject,
                                       Code = a.Code,
                                       //ErrorGroupId = a.ErrorGroupId,
                                       ErrorGroupName = ba != null ? ba.Name : string.Empty,
                                       //AuthorId = a.AuthorId,
                                       AuthorName = acn != null ? acn.Name : string.Empty,
                                       AuthorCode = acn != null ? acn.Code : string.Empty,
                                       PlanStartDate = a.PlanStartDate,
                                       PlanFinishDate = a.PlanFinishDate,
                                       //ObjectId = a.ObjectId,
                                       //ProjectId = a.ProjectId,
                                       ProjectName = adn != null ? adn.Name : string.Empty,
                                       ProjectCode = adn != null ? adn.Code : string.Empty,
                                       Status = a.Status,
                                       ModuleErrorVisualId = a.ModuleErrorVisualId,
                                       ModuleErrorVisualName = agn != null ? agn.Name : string.Empty,
                                       ModuleErrorVisualCode = agn != null ? agn.Code : string.Empty,
                                       DepartmentId = a.DepartmentId,
                                       DepartmentName = aen != null ? aen.Name : string.Empty,
                                       //ErrorBy = a.ErrorBy,
                                       ErrorByName = ahn != null ? ahn.Name : string.Empty,
                                       ErrorByCode = ahn != null ? ahn.Code : string.Empty,
                                       DepartmentProcessId = a.DepartmentProcessId,
                                       //DepartmentProcessName = ajn != null ? ajn.Name : string.Empty,
                                       //StageId = a.StageId,
                                       StageName = afn != null ? afn.Name : string.Empty,
                                       //FixBy = a.FixBy,
                                       //FixByName = ain != null ? ain.Name : string.Empty,
                                       Note = a.Note,
                                       ErrorCost = a.ErrorCost,
                                       Description = a.Description,
                                       Type = a.Type,
                                       TypeName = a.Type == 1 ? "Lỗi" : "Vấn đề",
                                       //CreateDate = a.CreateDate,
                                       //CreateBy = a.CreateBy,
                                       //UpdateBy = a.UpdateBy,
                                       //UpdateDate = a.UpdateDate,
                                       ProductCode = ka != null ? ka.Code : string.Empty,
                                       ProductName = ka != null ? ka.Name : string.Empty,
                                       AuthorDepartmentId = a.AuthorId,
                                       AuthorDepartmentName = acnjx != null ? acnjx.Name : string.Empty,
                                   }).ToList();

            foreach (var item in searchResult.Errors)
            {
                var query = db.ErrorFixs.Where(r => r.ErrorId.Equals(item.Id)).OrderBy(a => a.Solution).Select(a => new ErrorModel
                {
                    Id = a.Id,
                    FinishDate = a.FinishDate,
                    Status = a.Status,
                    Solution = a.Solution,
                    ErrorId = item.Id,
                    DateFrom = a.DateFrom,
                    Done = a.Status == 2 ? "1" : "0"
                }).ToList();
                var querycomplete = db.ErrorFixs.Where(r => r.ErrorId.Equals(item.Id) && r.Status == 2).Count();
                item.Done = $"{querycomplete}/{query.Count()}";
                item.FinishDate = query.Count > 0 ? query.Max(r => r.FinishDate) : null;

            }
            if (modelSearch.UpdateDateOpen != null)
            {
                searchResult.Errors = searchResult.Errors.Where(r => r.UpdateDate >= modelSearch.UpdateDateOpen).ToList();
            }
            if (modelSearch.UpdateDateEnd != null)
            {
                modelSearch.UpdateDateEnd = modelSearch.UpdateDateEnd.Value.ToEndDate();
                searchResult.Errors = searchResult.Errors.Where(r => r.UpdateDate <= modelSearch.UpdateDateEnd).ToList();
            }

            if (modelSearch.ChangePlanId != 0)
            {
                var list = searchResult.Errors.ToList();
                foreach (var item in searchResult.Errors)
                {
                    if (modelSearch.ChangePlanId == 1) // Tìm lỗi thay đổi
                    {

                        var c = 0;
                        var errorChangePlan = db.ErrorChangedPlans.Where(r => r.ErrorId.Equals(item.Id)).ToList();

                        if (errorChangePlan.Count == 0) // Không có thay đổi
                        {
                            list.Remove(item);
                        }
                    }
                    else
                    {
                        var errorChangePlan = db.ErrorFixs.Where(r => r.ErrorId.Equals(item.Id)).ToList();
                        foreach (var ite in errorChangePlan)
                        {
                            var listErrorChangePlan = db.ErrorChangedPlans.Where(r => r.ErrorFixId.Equals(ite.Id)).ToList();
                            if (listErrorChangePlan.Count > 0)
                            {
                                list.Remove(item);
                            }
                        }
                    }
                }

                searchResult.Errors = list.ToList();
            }

            var errorFixResults = (from a in db.ErrorFixs.AsNoTracking()
                                   where listResult.Contains(a.ErrorId)
                                   join b in db.Employees.AsNoTracking() on a.EmployeeFixId equals b.Id into ab
                                   from abn in ab.DefaultIfEmpty()
                                   join c in db.Employees.AsNoTracking() on a.SupportId equals c.Id into ac
                                   from acn in ac.DefaultIfEmpty()
                                   join d in db.Employees.AsNoTracking() on a.ApproveId equals d.Id into ad
                                   from adn in ad.DefaultIfEmpty()
                                   join e in db.Employees.AsNoTracking() on a.AdviseId equals e.Id into ae
                                   from aen in ae.DefaultIfEmpty()
                                   join f in db.Employees.AsNoTracking() on a.NotifyId equals f.Id into af
                                   from afn in af.DefaultIfEmpty()
                                   join j in db.Departments.AsNoTracking() on a.DepartmentId equals j.Id into aj
                                   from ajn in aj.DefaultIfEmpty()
                                   orderby a.DateFrom ascending
                                   select new ErrorFixResultModel
                                   {
                                       AdviseName = aen != null ? aen.Name : string.Empty,
                                       ApproveName = adn != null ? adn.Name : string.Empty,
                                       SupportName = acn != null ? acn.Name : string.Empty,
                                       NotifyName = afn != null ? afn.Name : string.Empty,
                                       FixByName = abn != null ? abn.Name : string.Empty,
                                       DepartmentName = ajn != null ? ajn.Name : string.Empty,
                                       DateFrom = a.DateFrom,
                                       DateTo = a.DateTo,
                                       ErrorId = a.ErrorId,
                                       Id = a.Id,
                                       Solution = a.Solution,
                                       Status = a.Status,
                                       EstimateTime = a.EstimateTime,
                                       CountChangePlan = db.ErrorChangedPlans.Where(r => r.ErrorFixId.Equals(a.Id)).Select(r => r.ErrorFixId).ToList().Count(),
                                       Deadline = a.DateTo.HasValue && dateNow > a.DateTo && a.Status != Constants.ErrorFix_Status_Finish ? DbFunctions.DiffDays(a.DateTo, dateNow).Value : 0,
                                       Done = a.Done
                                   }).ToList();

            List<ErrorFixResultModel> errorFixs;
            foreach (var item in searchResult.Errors)
            {
                errorFixs = errorFixResults.Where(r => r.ErrorId.Equals(item.Id)).Select(a => new ErrorFixResultModel
                {
                    AdviseName = a.AdviseName,
                    ApproveName = a.ApproveName,
                    SupportName = a.SupportName,
                    NotifyName = a.NotifyName,
                    FixByName = a.FixByName,
                    DepartmentName = a.DepartmentName,
                    DateFrom = a.DateFrom,
                    DateTo = a.DateTo,
                    ErrorId = a.ErrorId,
                    Id = a.Id,
                    Solution = a.Solution,
                    Status = a.Status,
                    EstimateTime = a.EstimateTime,
                    ErrorCode = item.Code,
                    Subject = item.Subject,
                    ModuleCode = item.ModuleErrorVisualCode,
                    ModuleName = item.ModuleErrorVisualName,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProjectCode = item.ProjectCode,
                    ProjectName = item.ProjectName,
                    CountChangePlan = a.CountChangePlan,
                    Done = a.Done
                }).ToList();

                searchResult.ErrorFixs.AddRange(errorFixs);

                item.PlanStartDateView = item.PlanStartDate.HasValue ? item.PlanStartDate.Value.ToString("dd/MM/yyyy") : "";
            }


            return searchResult;
        }


        /// <summary>
        /// Tìm kiếm vấn đề
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <param name="departmentId">Id phòng ban người đăng nhập</param>
        /// <returns></returns>
        public ErrorSearchResult SearchErrorsMobile(ErrorSearchCondition modelSearch, string departmentId)
        {
            ErrorSearchResult searchResult = new ErrorSearchResult();
            DateTime dateNow = DateTime.Now;
            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id
                             join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                             orderby a.Status, a.PlanFinishDate
                             select new ErrorResultModel
                             {
                                 Id = a.Id,
                                 Subject = a.Subject,
                                 Code = a.Code,
                                 ErrorGroupId = a.ErrorGroupId,
                                 AuthorId = a.AuthorId,
                                 AuthorName = c.Name,
                                 PlanStartDate = a.PlanStartDate,
                                 ObjectId = a.ObjectId,
                                 Status = a.Status,
                                 DepartmentId = a.DepartmentId,
                                 ErrorBy = a.ErrorBy,
                                 DepartmentProcessId = a.DepartmentProcessId,
                                 StageId = a.StageId,
                                 FixBy = a.FixBy,
                                 Type = a.Type,
                                 ProjectId = a.ProjectId,
                                 DepartmentCreateId = a.DepartmentCreateId,
                                 ProjectName = b.Name,
                                 ProjectCode = b.Code,
                                 DepartmentManageId = b.DepartmentId,
                                 AffectId = a.AffectId,
                                 CreateDate = a.CreateDate,
                                 AuthorDepartmentId = d.Id,
                                 DepartmentFixIds = db.ErrorFixs.Where(r => r.ErrorId.Equals(a.Id)).Select(r => r.DepartmentId).ToList(),
                                 PlanFinishDate = a.PlanFinishDate
                             }).AsQueryable();

            // Trường hợp không có quyền xem tất cả các vấn đề thì tiến hành lọc theo phòng ban
            if (!modelSearch.IsAllPermission)
            {
                // Tìm kiếm theo SBU của người login: Phòng ban người phát hiện || phòng ban người chịu trách nhiệm xử lý || Tìm kiếm theo phòn ban của người thực hiện
                dataQuery = dataQuery.Where(u => u.DepartmentCreateId.Equals(departmentId) || u.DepartmentProcessId.Equals(departmentId) || u.DepartmentFixIds.Contains(departmentId));
            }

            // Tìm kiếm theo Phòng ban quản lý dự án
            if (!string.IsNullOrEmpty(modelSearch.DepartmentManageId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentManageId.Equals(modelSearch.DepartmentManageId));
            }

            // Tìm kiếm theo người chịu trách nhiệm
            if (!string.IsNullOrEmpty(modelSearch.ErrorBy))
            {
                dataQuery = dataQuery.Where(u => u.ErrorBy.Equals(modelSearch.ErrorBy));
            }

            // Tìm kiếm theo dự án
            if (!string.IsNullOrEmpty(modelSearch.ProjectName))
            {
                dataQuery = dataQuery.Where(u => u.ProjectName.ToUpper().Contains(modelSearch.ProjectName.ToUpper()) || u.ProjectCode.ToUpper().Contains(modelSearch.ProjectName.ToUpper()));
            }

            // Tìm kiếm theo mã lỗi / tên lỗi
            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                dataQuery = dataQuery.Where(u => u.Subject.ToUpper().Contains(modelSearch.NameCode.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.NameCode.ToUpper()));
            }

            // Tìm kiếm theo Người phát hiện lỗi
            if (!string.IsNullOrEmpty(modelSearch.AuthorId))
            {
                dataQuery = dataQuery.Where(u => u.AuthorId.Equals(modelSearch.AuthorId));
            }

            // Tìm kiếm theo loại vấn đề
            if (modelSearch.Type != 0)
            {
                // Lỗi
                if (modelSearch.Type == Constants.Error_Type_Error)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Error);
                }
                // Vấn đề
                else if (modelSearch.Type == Constants.Error_Type_Issue)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Issue);
                }
            }

            // Tìm kiếm theo tình trạng
            if (modelSearch.Status != 0)
            {
                dataQuery = dataQuery.Where(u => u.Status == modelSearch.Status);
            }
            else
            {
                dataQuery = dataQuery.Where(r => r.Status != Constants.Problem_Status_Close && r.Status != Constants.Problem_Status_Ok_QC && r.Status != Constants.Problem_Status_Done);
            }

            searchResult.TotalItem = dataQuery.Count();

            searchResult.Errors = dataQuery.ToList();

            foreach (var item in searchResult.Errors)
            {
                item.PlanStartDateView = item.PlanStartDate.HasValue ? item.PlanStartDate.Value.ToString("dd/MM/yyyy") : "";
            }

            return searchResult;
        }

        public SearchResultModel<ErrorHistoryModel> SearchErrorHistory(ErrorHistorySearchModel modelSearch)
        {
            SearchResultModel<ErrorHistoryModel> searchResult = new SearchResultModel<ErrorHistoryModel>();
            var dataQuery = (from a in db.ErrorHistories.AsNoTracking()
                             join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                             join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                             where a.ErrorId.Equals(modelSearch.ErrorId)
                             orderby a.CreateDate descending
                             select new ErrorHistoryModel
                             {
                                 Id = a.Id,
                                 ErrorId = a.ErrorId,
                                 Content = a.Content,
                                 CreateBy = a.CreateBy,
                                 CreateByName = c.Name,
                                 CreateDate = a.CreateDate,
                             }).AsQueryable();

            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;

            return searchResult;
        }

        //
        public SearchResultModel<ErrorHistoryChangePlanModel> SearchErrorHistoryChangePlan(ErrorHistoryChangePlanModel modelSearch)
        {
            SearchResultModel<ErrorHistoryChangePlanModel> searchResult = new SearchResultModel<ErrorHistoryChangePlanModel>();
            var dataQuery = (from a in db.ErrorChangedPlans.AsNoTracking()
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join c in db.Errors.AsNoTracking() on a.ErrorId equals c.Id
                             join d in db.ErrorFixs.AsNoTracking() on a.ErrorFixId equals d.Id
                             join e in db.Users.AsNoTracking() on a.ChangeBy equals e.Id
                             where a.ErrorId.Equals(modelSearch.ErrorId)
                             orderby a.ChangeDate descending
                             select new ErrorHistoryChangePlanModel
                             {
                                 Id = a.Id,
                                 ErrorId = a.ErrorId,
                                 ErrorFixId = a.ErrorFixId,
                                 ProjectId = a.ProjectId,
                                 OldFinishDate = a.OldFinishDate,
                                 NewFinishDate = a.NewFinishDate,
                                 ChangeDate = a.ChangeDate,
                                 ChangeBy = e.UserName,
                                 Reason = a.Reason,
                                 Solution = d.Solution
                             }).AsQueryable();

            var listResultCP = dataQuery.ToList();
            searchResult.ListResult = listResultCP;

            return searchResult;
        }
        //

        public object SearchModule(ModuleSearchModel modelSearch)
        {
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ProjectId
                             join c in db.Modules.AsNoTracking() on b.ModuleId equals c.Id
                             where a.Id.Equals(modelSearch.ProjectId)
                             orderby c.Code
                             select new ModuleModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Code = c.Code
                             }).AsQueryable();
            var list = (from a in dataQuery
                        group a by new { a.Id, a.Name, a.Code } into g
                        select new
                        {
                            g.Key.Id,
                            g.Key.Name,
                            g.Key.Code
                        }).ToList();

            var listResult = list.ToList();
            return listResult;
        }

        public SearchResultModel<ComboboxResult> SearchModuleMobile(string projectId)
        {
            SearchResultModel<ComboboxResult> searchResult = new SearchResultModel<ComboboxResult>();
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ProjectId
                             join c in db.Modules.AsNoTracking() on b.ModuleId equals c.Id
                             where a.Id.Equals(projectId)
                             orderby c.Code
                             select new ComboboxResult
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Code = c.Code
                             }).AsQueryable();
            var list = (from a in dataQuery
                        group a by new { a.Id, a.Name, a.Code } into g
                        select new ComboboxResult
                        {
                            Id = g.Key.Id,
                            Name = g.Key.Name,
                            Code = g.Key.Code
                        }).ToList();

            var listResult = list.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public object SearchProject(ModuleSearchModel modelSearch)
        {
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ProjectId
                             join c in db.Products.AsNoTracking() on b.ProductId equals c.Id
                             where a.Id.Equals(modelSearch.ProjectId)
                             orderby c.Code
                             select new ModuleModel
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Code = c.Code
                             }).AsQueryable();

            var list = (from a in dataQuery
                        group a by new { a.Id, a.Name, a.Code } into g
                        select new
                        {
                            g.Key.Id,
                            g.Key.Name,
                            g.Key.Code
                        }).ToList();

            var listResult = list.ToList();
            return listResult;
        }

        public object SearchProductMobile(string projectId)
        {
            SearchResultModel<ComboboxResult> searchResult = new SearchResultModel<ComboboxResult>();
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ProjectId
                             join c in db.Products.AsNoTracking() on b.ProductId equals c.Id
                             where a.Id.Equals(projectId)
                             orderby c.Code
                             select new ComboboxResult
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Code = c.Code
                             }).AsQueryable();

            var list = (from a in dataQuery
                        group a by new { a.Id, a.Name, a.Code } into g
                        select new ComboboxResult
                        {
                            Id = g.Key.Id,
                            Name = g.Key.Name,
                            Code = g.Key.Code
                        }).ToList();

            var listResult = list.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        public void AddError(ErrorModel model, string userId)
        {
            while (db.Errors.AsNoTracking().FirstOrDefault(a => a.Code.ToUpper().Equals(model.Code.ToUpper())) != null)
            {
                var codeModel = GetCodeProblem(model.Index, model.Type);
                model.Code = codeModel.Code;
                model.Index = codeModel.Index;
            }

            // Xoá ký tự đặc biệt
            model.Code = Util.RemoveSpecialCharacter(model.Code);
            CheckExistedForAdd(model);
            using (var trans = db.Database.BeginTransaction())
            {
                if (model.PlanStartDate > DateTime.Now)
                {
                    throw NTSException.CreateInstance("Ngày phát hiện không được lớn hơn ngày hiện tại!");
                }

                try
                {
                    Error error = new Error
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorGroupId = model.ErrorGroupId,
                        Subject = model.Subject.Trim(),
                        Code = model.Code.Trim(),
                        PlanStartDate = model.PlanStartDate,
                        PlanFinishDate = model.PlanFinishDate,
                        Description = model.Description.Trim(),
                        ProjectId = model.ProjectId,
                        ObjectId = model.ObjectId,
                        AuthorId = model.AuthorId,
                        DepartmentId = model.DepartmentId,
                        ObjectType = model.ObjectType,
                        Status = model.Status,
                        ErrorCost = 0,
                        Type = model.Type,
                        Price = model.Price,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                        Index = model.Index,
                    };

                    var employeeAuthor = db.Employees.FirstOrDefault(r => r.Id.Equals(model.AuthorId));

                    if (employeeAuthor == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
                    }

                    error.DepartmentCreateId = employeeAuthor.DepartmentId;

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = error.Id,
                        Content = model.strHistory,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    if (!string.IsNullOrEmpty(model.ProjectId))
                    {
                        ProjectError projectError = new ProjectError()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ErrorId = error.Id,
                            ProjectId = model.ProjectId
                        };
                        db.ProjectErrors.Add(projectError);
                    }

                    if (model.ListImage != null && model.ListImage.Count > 0)
                    {
                        foreach (var item in model.ListImage)
                        {
                            ErrorImage errorImage = new ErrorImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ErrorId = error.Id,
                                Path = item.Path,
                                ThumbnailPath = item.ThumbnailPath
                            };
                            db.ErrorImages.Add(errorImage);
                        }
                    }

                    if (model.ListFile != null && model.ListFile.Count > 0)
                    {
                        foreach (var item in model.ListFile)
                        {
                            ErrorAttach attach = new ErrorAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ErrorId = error.Id,
                                Path = item.Path,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            };
                            db.ErrorAttaches.Add(attach);
                        }
                    }

                    db.Errors.Add(error);

                    UserLogUtil.LogHistotyAdd(db, userId, error.Code, error.Id, Constants.LOG_Error);

                    db.ErrorHistories.Add(errorHistory);
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

        public ErrorCodeModel GetCodeProblem(int code, int type)
        {
            var yearNow = DateTime.Now.Year;
            if (code == 0)
            {
                code = db.Errors.AsNoTracking().Where(a => a.CreateDate.Year == yearNow && a.Type == type).Select(s => s.Index).DefaultIfEmpty(0).Max();
            }
            code++;

            var trueNumberYear = DateTime.Now.ToString("yy");
            var codeError = "";

            if (type == 1)
            {
                codeError = trueNumberYear + "L." + string.Format("{0:0000}", code);
            }

            if (type == 2)
            {
                codeError = trueNumberYear + "V." + string.Format("{0:0000}", code);
            }

            return new ErrorCodeModel
            {
                Code = codeError,
                Index = code
            };
        }

        public void UpdateErrorConfirm(ErrorModel model, string userId, string departmentId)
        {
            while (db.Errors.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper())) != null)
            {
                var codeModel = GetCodeProblem(model.Index, model.Type);
                model.Code = codeModel.Code;
                model.Index = codeModel.Index;
            }
            using (var trans = db.Database.BeginTransaction())
            {
                var error = db.Errors.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                if (!departmentId.Equals(error.DepartmentCreateId))
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.Error);
                }

                var jsonApter = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                try
                {
                    error.ErrorGroupId = model.ErrorGroupId;
                    error.Code = model.Code;
                    error.Subject = model.Subject.Trim();
                    error.PlanStartDate = model.PlanStartDate;
                    error.PlanFinishDate = model.PlanFinishDate;
                    error.Description = model.Description.Trim();
                    error.ProjectId = model.ProjectId;
                    error.ObjectId = model.ObjectId;
                    error.DepartmentId = model.DepartmentId;
                    error.ErrorBy = model.ErrorBy;
                    error.FixBy = model.FixBy;
                    error.DepartmentProcessId = model.DepartmentProcessId;
                    error.StageId = model.StageId;
                    error.ErrorCost = model.ErrorCost;
                    error.Solution = model.Solution;
                    error.ObjectType = model.ObjectType;
                    error.Note = model.Note;
                    error.ActualStartDate = model.ActualStartDate;
                    error.ActualFinishDate = model.ActualFinishDate;
                    error.Type = model.Type;
                    error.Price = model.Price;
                    error.UpdateBy = userId;
                    error.UpdateDate = DateTime.Now;

                    var employeeAuthor = db.Employees.FirstOrDefault(r => r.Id.Equals(model.AuthorId));

                    if (employeeAuthor == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
                    }

                    error.DepartmentCreateId = employeeAuthor.DepartmentId;

                    var projectErrors = db.ProjectErrors.Where(i => i.ErrorId.Equals(model.Id)).ToList();
                    if (projectErrors.Count > 0)
                    {
                        db.ProjectErrors.RemoveRange(projectErrors);
                    }

                    if (!string.IsNullOrEmpty(model.ProjectId))
                    {
                        ProjectError projectError = new ProjectError()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ErrorId = error.Id,
                            ProjectId = model.ProjectId
                        };

                        db.ProjectErrors.Add(projectError);
                    }

                    string contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0059, TextResourceKey.ProblemExist);

                    if (model.Status != error.Status)
                    {
                        if (model.Status == Constants.Problem_Status_NoPlan)
                        {
                            contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0063, TextResourceKey.ProblemExist);
                        }
                    }

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = error.Id,
                        Content = contentHistory,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

                    error.Status = model.Status;

                    var jsonBefor = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                    UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Error, error.Id, error.Code, jsonBefor, jsonApter);

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

        public void UpdateErrorPlan(ErrorModel model, string userId, string employeeId, string departmentId, bool isUpdateByOtherPermistion)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var error = db.Errors.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                // Trường hợp người chịu trách nhiệm thì được quyền chỉnh sửa kế hoạch của vấn đề
                if (error.Status == Constants.Error_Status_No_Plan)
                {
                    if (!employeeId.Equals(error.ErrorBy))
                    {
                        if (!isUpdateByOtherPermistion)
                        {
                            throw NTSException.CreateInstance("Bạn không phải là người chịu trách nhiệm, không có quyền điều chỉnh kế hoạch");
                        }
                    }
                }

                try
                {
                    UpdateFixs(error.Id, model.Fixs, userId, model.ProjectId);

                    string contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0059, TextResourceKey.ProblemExist);

                    if (model.Status != error.Status)
                    {
                        if (model.Status == Constants.Problem_Status_Processed)
                        {
                            contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0085, TextResourceKey.ProblemExist);
                        }
                    }

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = error.Id,
                        Content = contentHistory,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

                    error.Status = model.Status;
                    error.AffectId = model.AffectId;
                    error.Note = model.Note;

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

        public void UpdateFixs(string errorId, List<ErrorFixModel> fixs, string userId, string projectId)
        {
            if (fixs.Count == 0)
            {
                return;
            }

            ErrorFix errorFix;
            ErrorChangedPlan errorChangedPlan;
            ErrorFixAttach errorFixAttach;
            foreach (var item in fixs)
            {
                if (string.IsNullOrEmpty(item.DepartmentId))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Id))
                {
                    errorFix = db.ErrorFixs.FirstOrDefault(r => r.Id.Equals(item.Id));

                    var startDate = errorFix.DateFrom;
                    var endDate = errorFix.DateTo;

                    if (errorFix != null)
                    {
                        if (item.IsDelete)
                        {
                            var attachs = db.ErrorFixAttachs.Where(r => r.ErrorFixId.Equals(errorFix.Id));

                            db.ErrorFixAttachs.RemoveRange(attachs);

                            var errorChangedPlans = db.ErrorChangedPlans.Where(a => a.ErrorFixId.Equals(errorFix.Id));
                            db.ErrorChangedPlans.RemoveRange(errorChangedPlans);

                            db.ErrorFixs.Remove(errorFix);
                        }
                        else
                        {
                            errorFix.DepartmentId = item.DepartmentId;
                            errorFix.AdviseId = item.AdviseId;
                            errorFix.ApproveId = item.ApproveId;
                            errorFix.DateFrom = item.DateFrom;
                            errorFix.DateTo = item.DateTo;
                            errorFix.Deadline = item.Deadline;
                            errorFix.EmployeeFixId = item.EmployeeFixId;
                            errorFix.NotifyId = item.NotifyId;
                            errorFix.Solution = item.Solution;

                            errorFix.SupportId = item.SupportId;
                            errorFix.EstimateTime = item.EstimateTime != null ? item.EstimateTime : 0;
                            errorFix.Done = item.Done;

                            if (item.Done == 100)
                            {
                                errorFix.Status = 2;
                            }
                            else
                            {
                                errorFix.Status = 1;
                            }

                            if (item.IsChange)
                            {
                                errorChangedPlan = new ErrorChangedPlan();
                                errorChangedPlan.Id = Guid.NewGuid().ToString();
                                errorChangedPlan.ErrorFixId = errorFix.Id;
                                errorChangedPlan.ProjectId = projectId;
                                errorChangedPlan.ErrorId = errorFix.ErrorId;
                                errorChangedPlan.OldStartDate = startDate;
                                errorChangedPlan.OldFinishDate = endDate;
                                errorChangedPlan.Reason = item.Reason;
                                errorChangedPlan.ChangeDate = DateTime.Now;
                                errorChangedPlan.ChangeBy = userId;
                                errorChangedPlan.NewStartDate = errorFix.DateFrom;
                                errorChangedPlan.NewFinishDate = errorFix.DateTo;

                                db.ErrorChangedPlans.Add(errorChangedPlan);
                            }

                        }
                    }
                }
                else
                {
                    errorFix = new ErrorFix()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DepartmentId = item.DepartmentId,
                        AdviseId = item.AdviseId,
                        ApproveId = item.ApproveId,
                        DateFrom = item.DateFrom,
                        DateTo = item.DateTo,
                        Deadline = item.Deadline,
                        EmployeeFixId = item.EmployeeFixId,
                        NotifyId = item.NotifyId,
                        Solution = item.Solution,

                        // Khi thêm mới thì gán tình trạng là chưa xong
                        Status = 1,
                        SupportId = item.SupportId,
                        EstimateTime = item.EstimateTime,
                        ErrorId = errorId,
                        Done = 0,
                    };

                    db.ErrorFixs.Add(errorFix);
                }

                foreach (var file in item.FixAttachs)
                {
                    if (!string.IsNullOrEmpty(file.Id))
                    {
                        errorFixAttach = db.ErrorFixAttachs.FirstOrDefault(r => r.Id.Equals(file.Id));

                        if (errorFixAttach != null)
                        {
                            if (file.IsDelete)
                            {
                                db.ErrorFixAttachs.Remove(errorFixAttach);
                            }
                            else
                            {
                                if (!errorFixAttach.Path.Equals(file.Path))
                                {
                                    errorFixAttach.FileName = file.FileName;
                                    errorFixAttach.FileSize = file.FileSize;
                                    errorFixAttach.Path = file.Path;
                                    errorFixAttach.UpdateBy = userId;
                                }
                                errorFixAttach.UpdateDate = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        errorFixAttach = new ErrorFixAttach()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ErrorFixId = errorFix.Id,
                            FileName = file.FileName,
                            FileSize = file.FileSize,
                            Path = file.Path,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now,
                        };
                        db.ErrorFixAttachs.Add(errorFixAttach);
                    }
                }
            }
        }

        /// <summary>
        /// Update vấn đề đang QC
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void UpdateErrorQC(ErrorModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var error = db.Errors.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                var jsonApter = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                try
                {
                    error.ActualFinishDate = model.ActualFinishDate;
                    error.UpdateBy = userId;
                    error.UpdateDate = DateTime.Now;

                    string contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0059, TextResourceKey.ProblemExist);

                    if (model.Status != error.Status && model.Status == Constants.Problem_Status_Ok_QC)
                    {
                        contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0067, TextResourceKey.ProblemExist);
                    }

                    if (model.Status != error.Status && model.Status == Constants.Problem_Status_NotOk_QC)
                    {
                        contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0068, TextResourceKey.ProblemExist);
                    }

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = error.Id,
                        Content = contentHistory,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

                    error.Status = model.Status;

                    var jsonBefor = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                    UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Error, error.Id, error.Code, jsonBefor, jsonApter);

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
        /// Update vấn đề đang xử lý
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void UpdateErrorProcess(ErrorModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var error = db.Errors.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                //if (!isUpdateByOtherPermistion && !error.CreateBy.Equals(model.UpdateBy))
                //{
                //    throw NTSException.CreateInstance(MessageResourceKey.MSG0032, TextResourceKey.Error);
                //}

                var jsonApter = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                try
                {
                    error.FixBy = model.FixBy;
                    error.StageId = model.StageId;
                    error.ErrorCost = model.ErrorCost;
                    error.Solution = model.Solution;
                    error.Note = model.Note;
                    error.ActualStartDate = model.ActualStartDate;
                    error.UpdateBy = userId;
                    error.UpdateDate = DateTime.Now;

                    var errorAttaches = db.ErrorAttaches.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (errorAttaches.Count > 0)
                    {
                        db.ErrorAttaches.RemoveRange(errorAttaches);
                    }

                    UpdateFixs(error.Id, model.Fixs, userId, model.ProjectId);

                    string contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0059, TextResourceKey.ProblemExist);

                    if (model.Status != error.Status && model.Status == Constants.Problem_Status_Awaiting_QC)
                    {
                        contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0065, TextResourceKey.ProblemExist);
                    }
                    else
                    {
                        var fixNotFinish = model.Fixs.FirstOrDefault(r => !r.IsDelete && r.Status != Constants.ErrorFix_Status_Finish);

                        if (fixNotFinish == null)
                        {
                            model.Status = Constants.Problem_Status_Awaiting_QC;
                        }
                    }

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = error.Id,
                        Content = contentHistory,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

                    error.Status = model.Status;

                    var jsonBefor = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                    UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Error, error.Id, error.Code, jsonBefor, jsonApter);

                    db.SaveChanges();

                    ChangeQC(error.Id);

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

        public void UpdateError(ErrorModel model, bool isUpdateByOtherPermistion, string userId)
        {
            while (db.Errors.AsNoTracking().FirstOrDefault(a => !a.Id.Equals(model.Id) && a.Code.ToUpper().Equals(model.Code.ToUpper())) != null)
            {
                var codeModel = GetCodeProblem(model.Index, model.Type);
                model.Code = codeModel.Code;
                model.Index = codeModel.Index;
            }
            using (var trans = db.Database.BeginTransaction())
            {
                if (model.PlanStartDate > DateTime.Now)
                {
                    throw NTSException.CreateInstance("Ngày phát hiện không được lớn hơn ngày hiện tại!");
                }

                var error = db.Errors.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                if (!error.CreateBy.Equals(userId) && !isUpdateByOtherPermistion)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0032, TextResourceKey.Error);
                }

                var jsonApter = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                try
                {
                    error.ErrorGroupId = model.ErrorGroupId;
                    error.Subject = model.Subject.Trim();
                    error.PlanStartDate = model.PlanStartDate;
                    error.PlanFinishDate = model.PlanFinishDate;
                    error.Description = model.Description.Trim();
                    error.ProjectId = model.ProjectId;
                    error.ObjectId = model.ObjectId;
                    error.DepartmentId = model.DepartmentId;
                    error.ObjectType = model.ObjectType;
                    error.Type = model.Type;
                    error.Price = model.Price;
                    error.UpdateBy = userId;
                    error.UpdateDate = DateTime.Now;

                    var employeeAuthor = db.Employees.FirstOrDefault(r => r.Id.Equals(model.AuthorId));

                    if (employeeAuthor == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
                    }

                    error.DepartmentCreateId = employeeAuthor.DepartmentId;

                    var listImage = db.ErrorImages.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (listImage.Count > 0)
                    {
                        db.ErrorImages.RemoveRange(listImage);
                    }

                    if (model.ListImage != null && model.ListImage.Count > 0)
                    {
                        foreach (var item in model.ListImage)
                        {
                            ErrorImage errorImage = new ErrorImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ErrorId = error.Id,
                                Path = item.Path,
                                ThumbnailPath = item.ThumbnailPath
                            };
                            db.ErrorImages.Add(errorImage);
                        }
                    }

                    var listFile = db.ErrorAttaches.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (listFile.Count > 0)
                    {
                        db.ErrorAttaches.RemoveRange(listFile);
                    }

                    if (model.ListFile != null && model.ListFile.Count > 0)
                    {
                        foreach (var item in model.ListFile)
                        {
                            ErrorAttach attach = new ErrorAttach()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ErrorId = error.Id,
                                Path = item.Path,
                                FileName = item.FileName,
                                FileSize = item.FileSize,
                                CreateBy = userId,
                                CreateDate = DateTime.Now,
                                UpdateBy = userId,
                                UpdateDate = DateTime.Now
                            };
                            db.ErrorAttaches.Add(attach);
                        }
                    }

                    var projectErrors = db.ProjectErrors.Where(i => i.ErrorId.Equals(model.Id)).ToList();
                    if (projectErrors.Count > 0)
                    {
                        db.ProjectErrors.RemoveRange(projectErrors);
                    }

                    if (!string.IsNullOrEmpty(model.ProjectId))
                    {
                        ProjectError projectError = new ProjectError()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ErrorId = error.Id,
                            ProjectId = model.ProjectId
                        };

                        db.ProjectErrors.Add(projectError);
                    }

                    string contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0059, TextResourceKey.ProblemExist);

                    if (model.Status != error.Status && model.Status == Constants.Problem_Status_Awaiting_Confirm)
                    {
                        contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0061, TextResourceKey.ProblemExist);
                    }

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = error.Id,
                        Content = contentHistory,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

                    error.Status = model.Status;

                    var jsonBefor = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                    UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Error, error.Id, error.Code, jsonBefor, jsonApter);

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

        public void DeleteError(ErrorModel model, bool isDeleteByOtherPermistion, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var error = db.Errors.FirstOrDefault(u => u.Id.Equals(model.Id));
                    if (error == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
                    }

                    if (!error.CreateBy.Equals(userId) && !isDeleteByOtherPermistion)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0037, TextResourceKey.Error);
                    }

                    var errorImages = db.ErrorImages.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (errorImages.Count() > 0)
                    {
                        db.ErrorImages.RemoveRange(errorImages);
                    }

                    var errorAttaches = db.ErrorAttaches.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (errorAttaches.Count() > 0)
                    {
                        db.ErrorAttaches.RemoveRange(errorAttaches);
                    }

                    var errorHistories = db.ErrorHistories.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (errorHistories.Count() > 0)
                    {
                        db.ErrorHistories.RemoveRange(errorHistories);
                    }

                    var projectErrors = db.ProjectErrors.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (projectErrors.Count() > 0)
                    {
                        db.ProjectErrors.RemoveRange(projectErrors);
                    }

                    var errorFixAttachs = from a in db.ErrorFixs
                                          where a.ErrorId.Equals(model.Id)
                                          join b in db.ErrorFixAttachs on a.Id equals b.ErrorFixId
                                          select b;

                    if (errorFixAttachs.Count() > 0)
                    {
                        db.ErrorFixAttachs.RemoveRange(errorFixAttachs);
                    }

                    var errorFixs = db.ErrorFixs.Where(a => a.ErrorId.Equals(model.Id)).ToList();
                    if (errorFixs.Count() > 0)
                    {
                        var errorChangedPlans = db.ErrorChangedPlans.Where(a => errorFixs.Select(i => i.Id).Contains(a.ErrorFixId));
                        db.ErrorChangedPlans.RemoveRange(errorChangedPlans);

                        db.ErrorFixs.RemoveRange(errorFixs);
                    }
                    db.Errors.Remove(error);

                    var NameOrCode = error.Code;


                    var jsonBefor = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);
                    UserLogUtil.LogHistotyDelete(db, userId, Constants.LOG_Error, error.Id, NameOrCode, jsonBefor);

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

        public ErrorModel GetErrorInfo(ErrorModel model)
        {
            var resultInfo = db.Errors.AsNoTracking().Where(u => model.Id.Equals(u.Id)).Select(a => new ErrorModel
            {
                Id = a.Id,
                Subject = a.Subject,
                Code = a.Code,
                Index = a.Index,
                ProjectId = a.ProjectId,
                ErrorGroupId = a.ErrorGroupId,
                AuthorId = a.AuthorId,
                PlanStartDate = a.PlanStartDate,
                PlanFinishDate = a.PlanFinishDate,
                ObjectId = a.ObjectId,
                ModuleErrorVisualId = a.ModuleErrorVisualId,
                DepartmentId = a.DepartmentId,
                ErrorBy = a.ErrorBy,
                DepartmentProcessId = a.DepartmentProcessId,
                StageId = a.StageId,
                FixBy = a.FixBy,
                Note = a.Note,
                ErrorCost = a.ErrorCost,
                Description = a.Description,
                Status = a.Status,
                Solution = a.Solution,
                ActualStartDate = a.ActualStartDate,
                ActualFinishDate = a.ActualFinishDate,
                Type = a.Type,
                Price = a.Price,
                ObjectType = a.ObjectType,
                AffectId = a.AffectId
            }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }
            var errorBy = db.Employees.AsNoTracking().Where(a => a.Id.Equals(resultInfo.ErrorBy)).FirstOrDefault();
            if (errorBy != null)
            {
                resultInfo.ErrorByName = errorBy.Name;
            }
            var departmentProcess = db.Departments.AsNoTracking().Where(a => a.Id.Equals(resultInfo.DepartmentProcessId)).FirstOrDefault();
            if (departmentProcess != null)
            {
                resultInfo.DepartmentProcessName = departmentProcess.Name;
            }
            var fixBy = db.Employees.AsNoTracking().Where(a => a.Id.Equals(resultInfo.FixBy)).FirstOrDefault();
            if (fixBy != null)
            {
                resultInfo.FixByName = fixBy.Name;
            }
            var stage = db.Stages.AsNoTracking().FirstOrDefault(a => a.Id.Equals(resultInfo.StageId));
            if (stage != null)
            {
                resultInfo.StageName = stage.Name;
            }
            var listImage = (from a in db.ErrorImages.AsNoTracking()
                             where a.ErrorId.Equals(model.Id)
                             select new ErrorImageModel
                             {
                                 Id = a.Id,
                                 ErrorId = a.ErrorId,
                                 Path = a.Path,
                                 ThumbnailPath = a.ThumbnailPath
                             });
            resultInfo.ListImage = listImage.ToList();

            // Cờ đánh dấu trạng thái có thể chỉnh sửa ngày kế hoạch
            var history = (from r in db.ErrorHistories.AsNoTracking()
                           where r.ErrorId.Equals(model.Id) && r.Content.Equals("Đã có kế hoạch Vấn đề")
                           orderby r.CreateDate descending
                           select r).FirstOrDefault();

            resultInfo.CanChangeDate = false;
            if (history != null)
            {
                var z = (from r in db.ErrorHistories.AsNoTracking()
                         where r.ErrorId.Equals(model.Id) && r.Content.Equals("Hủy Đã có kế hoạch Vấn đề") && r.CreateDate > history.CreateDate
                         select r).FirstOrDefault();
                if (z != null)
                {
                    resultInfo.CanChangeDate = true;
                }
            }

            var listFile = (from a in db.ErrorAttaches.AsNoTracking()
                            where a.ErrorId.Equals(model.Id)
                            select new ErrorAttachModel
                            {
                                Id = a.Id,
                                ErrorId = a.ErrorId,
                                FileName = a.FileName,
                                FileSize = a.FileSize.Value,
                                Path = a.Path,
                            }).ToList();
            resultInfo.ListFile = listFile;
            if (!string.IsNullOrEmpty(resultInfo.ErrorGroupId))
            {
                var errorGroup = db.ErrorGroups.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.ErrorGroupId));
                if (errorGroup != null)
                {
                    resultInfo.ErrorGroupName = errorGroup.Name;
                }
            }

            if (!string.IsNullOrEmpty(resultInfo.DepartmentId))
            {
                var deparment = db.Departments.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.DepartmentId));
                if (deparment != null)
                {
                    resultInfo.DepartmentName = deparment.Name;
                }
            }

            if (!string.IsNullOrEmpty(resultInfo.AuthorId))
            {
                var employee = db.Employees.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.AuthorId));
                if (employee != null)
                {
                    resultInfo.AuthorName = employee.Name;
                }
            }

            if (!string.IsNullOrEmpty(resultInfo.ProjectId))
            {
                var project = db.Projects.AsNoTracking().FirstOrDefault(u => u.Id.Equals(resultInfo.ProjectId));
                if (project != null)
                {
                    resultInfo.ProjectName = project.Name;
                    resultInfo.ProjectCode = project.Code;
                }
            }

            if (!string.IsNullOrEmpty(resultInfo.ObjectId))
            {
                var module = db.Modules.AsNoTracking().FirstOrDefault(a => a.Id.Equals(resultInfo.ObjectId));
                if (module != null)
                {
                    resultInfo.ObjectName = module.Name;
                    resultInfo.ObjectCode = module.Code;
                }
            }
            var dateNow = DateTime.Now;
            resultInfo.Fixs = (from a in db.ErrorFixs.AsNoTracking()
                               where a.ErrorId.Equals(model.Id)
                               join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                               join e in db.Employees.AsNoTracking() on a.EmployeeFixId equals e.Id into cm
                               from cmn in cm.DefaultIfEmpty()
                               select new ErrorFixModel
                               {
                                   AdviseId = a.AdviseId,
                                   Id = a.Id,
                                   DateFrom = a.DateFrom,
                                   DateTo = a.DateTo,
                                   Deadline = a.DateTo.HasValue && dateNow > a.DateTo && a.Status != Constants.ErrorFix_Status_Finish ? DbFunctions.DiffDays(a.DateTo, dateNow).Value : 0,
                                   DepartmentId = a.DepartmentId,
                                   ApproveId = a.ApproveId,
                                   DepartmentName = d.Name,
                                   EmployeeFixId = a.EmployeeFixId,
                                   EmployeeName = cmn != null ? cmn.Name : string.Empty,
                                   NotifyId = a.NotifyId,
                                   Solution = a.Solution,
                                   Status = a.Status,
                                   SupportId = a.SupportId,
                                   EstimateTime = a.EstimateTime,
                                   Done = a.Done,
                               }).OrderBy(t => t.DateFrom).ToList();

            foreach (var item in resultInfo.Fixs)
            {
                item.FixAttachs = (from a in db.ErrorFixAttachs.AsNoTracking()
                                   where a.ErrorFixId.Equals(item.Id)
                                   select new ErrorFixAttachModel
                                   {
                                       FileName = a.FileName,
                                       FileSize = a.FileSize,
                                       Id = a.Id,
                                       Path = a.Path

                                   }).ToList();
            }

            return resultInfo;
        }

        private void CheckExistedForAdd(ErrorModel model)
        {
            if (db.Errors.AsNoTracking().Where(o => o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Error);
            }
        }

        public void CheckExistedForUpdate(ErrorModel model)
        {
            if (db.Errors.AsNoTracking().Where(o => !o.Id.Equals(model.Id) && o.Code.Equals(model.Code)).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Error);
            }
        }

        public string ExportExcel(ErrorSearchModel modelSearch, string departmentId)
        {
            List<ErrorResultModel> listModel = this.MakeWhereCondition(modelSearch, departmentId, true).Where(r =>
            r.Status == Constants.Problem_Status_NoPlan || r.Status == Constants.Problem_Status_Processed).ToList();

            if (listModel.Count == 0)
            {
                throw NTSException.CreateInstance("Không có dữ liệu!");
            }

            List<string> errorIds = listModel.Select(r => r.Id).ToList();

            // Lấy danh sách công việc của từng Vấn đề tồn đọng
            var errorFixs = db.ErrorFixs.Where(r => errorIds.Contains(r.ErrorId)).ToList();

            // Lấy danh sách dự án đang có vấn đề tồn đọng
            var projects = listModel
                        .Select(m => new { m.ProjectId, m.ProjectCode, m.ProjectName, m.Status, m.PriceNoVAT })
                        .Distinct()
                        .ToList();

            List<ErrorExportModel> exportData = new List<ErrorExportModel>();
            ErrorExportModel errorExportModel;
            var employees = db.Employees.AsNoTracking().ToList();
            var departments = db.Departments.AsNoTracking().ToList();
            var products = db.Products.AsNoTracking().ToList();
            var payments = db.Payments.AsNoTracking().ToList();
            CultureInfo ci = new CultureInfo("en-us");

            int index = 1;
            List<ErrorFix> errorFix = new List<ErrorFix>();
            Product product = new Product();
            foreach (var project in projects)
            {
                var errors = listModel.Where(r => r.ProjectId.Equals(project.ProjectId)).ToList();

                // Lấy thông tin thanh toán của dự án
                var payment = payments.Where(r => r.ProjectId.Equals(project.ProjectId)).ToList();
                decimal? tongDaThu = payment == null ? 0 : payment.Sum(r => r.TotalAmount);
                decimal? conPhaiThu = project.PriceNoVAT - tongDaThu;
                decimal tyle = project.PriceNoVAT == 0 ? 0 : (decimal)tongDaThu / (decimal)project.PriceNoVAT;

                foreach (var error in errors)
                {
                    errorFix = errorFixs.Where(r => r.ErrorId.Equals(error.Id)).OrderBy(r => r.DateFrom).ToList();
                    product = products.Where(r => r.Id.Equals(error.ObjectId)).FirstOrDefault();
                    errorExportModel = new ErrorExportModel();
                    errorExportModel.Index = index++;
                    errorExportModel.MaDA = project.ProjectCode;
                    errorExportModel.TenDA = project.ProjectName;
                    errorExportModel.TinhTrangDA = GetProjectStatusName(project.Status);
                    errorExportModel.GiaTriHD = project.PriceNoVAT.ToString();
                    errorExportModel.TongDaThu = tongDaThu == null ? String.Empty : tongDaThu.ToString();
                    errorExportModel.ConPhaiThu = conPhaiThu == null ? String.Empty : conPhaiThu.ToString();
                    errorExportModel.TyLe = tyle.ToString("P", ci);

                    errorExportModel.MaHangMuc = product != null ? product.Code : String.Empty;
                    errorExportModel.TenHangMuc = product != null ? product.Name : String.Empty;
                    errorExportModel.MaVanDe = error.Code;
                    errorExportModel.PhanLoai = error.TypeName;
                    errorExportModel.TenVanDe = error.Subject;
                    errorExportModel.MoTa = error.Description;
                    errorExportModel.NguyenNhan = error.Note;

                    if (errorFix != null)
                    {
                        errorExportModel.NgayKetThuc = errorFix.Select(r => r.DateTo).Max().HasValue ? errorFix.Select(r => r.DateTo).Max().Value.ToString("dd/MM/yyyy") : String.Empty;
                    }

                    errorExportModel.TinhTrang = errorFix.Count() == 0 ? "Chưa có kế hoạch" : (errorFix.Select(r => r.Status == 1).Any() ? "Chưa xong" : "Đã xong");
                    exportData.Add(errorExportModel);

                    foreach (var item in errorFix)
                    {
                        errorExportModel = new ErrorExportModel();
                        errorExportModel.Index = index++;
                        errorExportModel.GiaiPhap = item.Solution;
                        errorExportModel.NguoiThucHien = employees.Where(r => r.Id.Equals(item.EmployeeFixId)).FirstOrDefault() != null ? employees.Where(r => r.Id.Equals(item.EmployeeFixId)).FirstOrDefault().Name : String.Empty;
                        errorExportModel.BoPhanThucHien = departments.Where(r => r.Id.Equals(item.DepartmentId)).FirstOrDefault() != null ? departments.Where(r => r.Id.Equals(item.DepartmentId)).FirstOrDefault().Name : String.Empty;
                        errorExportModel.NgayBatDau = item.DateFrom.HasValue ? item.DateFrom.Value.ToString("dd/MM/yyyy") : String.Empty;
                        errorExportModel.NgayKetThuc = item.DateTo.HasValue ? item.DateTo.Value.ToString("dd/MM/yyyy") : String.Empty;
                        errorExportModel.TinhTrang = item.Status == 1 ? "Chưa xong" : "Đã xong";

                        exportData.Add(errorExportModel);
                    }
                }
            }

            try
            {
                ExcelEngine excelEngine = new ExcelEngine();

                IApplication application = excelEngine.Excel;

                IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ProplemExist.xlsx"));

                IWorksheet sheet = workbook.Worksheets[0];

                var total = exportData.Count;

                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

                sheet.ImportData(exportData, iRangeData.Row, iRangeData.Column, false);
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 21].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 21].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 21].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 21].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 21].Borders.Color = ExcelKnownColors.Black;
                sheet.Range["A" + 2 + ":" + "Z" + (2 + total)].AutofitRows();

                string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách vấn đề tồn đọng" + ".xls");
                workbook.SaveAs(pathExport);
                workbook.Close();
                excelEngine.Dispose();

                //Đường dẫn file lưu trong web client
                string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách vấn đề tồn đọng" + ".xls";

                return resultPathClient;
            }
            catch (Exception ex)
            {
                //Log.LogError(ex);
                throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            }
        }

        private string GetProjectStatusName(int status)
        {
            switch (status)
            {
                case 1:
                    return "Chưa kickoff";
                    break;
                case 2:
                    return "Sản xuất";
                    break;
                case 3:
                    return "Đóng dự án";
                    break;
                case 4:
                    return "Tạm dừng";
                    break;
                case 5:
                    return "Lắp đặt";
                    break;
                case 6:
                    return "Hiệu chỉnh";
                    break;
                case 7:
                    return "Đưa vào sử dụng";
                    break;
                case 8:
                    return "Thiết kế";
                    break;
                case 9:
                    return "Nghiệm thu";
                    break;
                default:
                    return string.Empty;
            }
        }

        private IQueryable<ErrorResultModel> MakeWhereCondition(ErrorSearchModel modelSearch, string departmentId, bool isAll)
        {
            DateTime dateNow = DateTime.Now;

            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join c in db.Employees.AsNoTracking() on a.AuthorId equals c.Id
                             join d in db.Departments.AsNoTracking() on c.DepartmentId equals d.Id
                             select new ErrorResultModel
                             {
                                 Id = a.Id,
                                 Subject = a.Subject,
                                 Code = a.Code,
                                 ErrorGroupId = a.ErrorGroupId,
                                 AuthorId = a.AuthorId,
                                 PlanStartDate = a.PlanStartDate,
                                 ObjectId = a.ObjectId,
                                 Status = a.Status,
                                 DepartmentId = a.DepartmentId,
                                 ErrorBy = a.ErrorBy,
                                 DepartmentProcessId = a.DepartmentProcessId,
                                 StageId = a.StageId,
                                 FixBy = a.FixBy,
                                 Type = a.Type,
                                 ProjectId = a.ProjectId,
                                 DepartmentCreateId = a.DepartmentCreateId,
                                 ProjectName = b.Name,
                                 ProjectCode = b.Code,
                                 DepartmentManageId = b.DepartmentId,
                                 AffectId = a.AffectId,
                                 CreateDate = a.CreateDate,
                                 AuthorDepartmentId = d.Id,
                                 PriceNoVAT = b.SaleNoVAT,
                                 PlanFinishDate = a.PlanFinishDate,
                             }).AsQueryable();

            if (!isAll)
            {
                dataQuery = dataQuery.Where(r => departmentId.Equals(r.DepartmentCreateId) || r.Status > Constants.Problem_Status_Awaiting_Confirm);
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentProcessId) && !string.IsNullOrEmpty(modelSearch.FixBy))
            {
                var departmentIds = db.ErrorFixs.Where(r => modelSearch.DepartmentProcessId.Equals(r.DepartmentId) && modelSearch.FixBy.Equals(r.EmployeeFixId)).Select(s => s.ErrorId).ToList();
                dataQuery = dataQuery.Where(u => departmentIds.Contains(u.Id));
            }
            else if (!string.IsNullOrEmpty(modelSearch.DepartmentProcessId))
            {
                var departmentIds = db.ErrorFixs.Where(r => modelSearch.DepartmentProcessId.Equals(r.DepartmentId)).Select(s => s.ErrorId).ToList();
                dataQuery = dataQuery.Where(u => departmentIds.Contains(u.Id));
            }
            else if (!string.IsNullOrEmpty(modelSearch.FixBy))
            {
                var fixByErrorIds = db.ErrorFixs.Where(r => modelSearch.FixBy.Equals(r.EmployeeFixId)).Select(s => s.ErrorId).ToList();
                dataQuery = dataQuery.Where(u => fixByErrorIds.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(modelSearch.AuthorDepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.AuthorDepartmentId.Equals(modelSearch.AuthorDepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.AuthorId))
            {
                dataQuery = dataQuery.Where(u => u.AuthorId.Equals(modelSearch.AuthorId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ErrorGroupId))
            {
                dataQuery = dataQuery.Where(u => u.ErrorGroupId.Equals(modelSearch.ErrorGroupId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ProjectId))
            {
                dataQuery = dataQuery.Where(u => u.ProjectId.Equals(modelSearch.ProjectId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ProjectName))
            {
                dataQuery = dataQuery.Where(u => u.ProjectName.Contains(modelSearch.ProjectName) || u.ProjectCode.Contains(modelSearch.ProjectName));
            }

            if (modelSearch.Status != 0)
            {
                dataQuery = dataQuery.Where(u => u.Status == modelSearch.Status);
            }
            else
            {
                dataQuery = dataQuery.Where(r => r.Status != Constants.Problem_Status_Close && r.Status != Constants.Problem_Status_Ok_QC && r.Status != Constants.Problem_Status_Done);
            }

            if (!string.IsNullOrEmpty(modelSearch.ErrorGroupId))
            {
                dataQuery = dataQuery.Where(u => u.ErrorGroupId.Equals(modelSearch.ErrorGroupId));
            }

            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                dataQuery = dataQuery.Where(u => u.Subject.ToUpper().Contains(modelSearch.NameCode.ToUpper()) || u.Code.ToUpper().Contains(modelSearch.NameCode.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ErrorBy))
            {
                dataQuery = dataQuery.Where(u => u.ErrorBy.Equals(modelSearch.ErrorBy));
            }

            if (!string.IsNullOrEmpty(modelSearch.StageId))
            {
                dataQuery = dataQuery.Where(u => u.StageId.Equals(modelSearch.StageId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ObjectId))
            {
                dataQuery = dataQuery.Where(u => u.ObjectId.Equals(modelSearch.ObjectId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentManageId))
            {
                dataQuery = dataQuery.Where(u => modelSearch.DepartmentManageId.Equals(u.DepartmentManageId));
            }

            if (modelSearch.ErrorAffectId.HasValue)
            {
                dataQuery = dataQuery.Where(u => modelSearch.ErrorAffectId.Value == u.AffectId);
            }

            if (modelSearch.Type != 0)
            {
                if (modelSearch.Type == Constants.Error_Type_Error)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Error);
                }
                else if (modelSearch.Type == Constants.Error_Type_Issue)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Issue);
                }
            }

            if (modelSearch.FixStatus.HasValue)
            {
                List<string> errorIds = new List<string>();

                if (modelSearch.FixStatus.Value == 1)
                {
                    errorIds = (from a in db.Errors.AsNoTracking()
                                join b in db.ErrorFixs.AsNoTracking() on a.Id equals b.ErrorId
                                where b.Status != Constants.ErrorFix_Status_Finish && a.Status != Constants.Error_Status_Done_QC && a.Status != Constants.Error_Status_Close
                                group a by a.Id into g
                                select g.Key).ToList();
                }
                else if (modelSearch.FixStatus.Value == 2)
                {
                    errorIds = (from a in db.Errors.AsNoTracking()
                                join b in db.ErrorFixs.AsNoTracking() on a.Id equals b.ErrorId
                                where b.Status != Constants.ErrorFix_Status_Finish && a.Status != Constants.Error_Status_Done_QC && a.Status != Constants.Error_Status_Close && dateNow > b.DateTo
                                group a by a.Id into g
                                select g.Key).ToList();
                }

                dataQuery = dataQuery.Where(r => errorIds.Contains(r.Id) && r.Status > Constants.Problem_Status_Awaiting_Confirm);
            }


            if (modelSearch.DateOpen != null)
            {
                dataQuery = dataQuery.Where(r => r.CreateDate >= modelSearch.DateOpen);
            }

            if (modelSearch.DateEnd != null)
            {
                dataQuery = dataQuery.Where(r => r.CreateDate <= modelSearch.DateEnd);
            }

            if (modelSearch.PlanType.HasValue)
            {
                DateTime dateThree = dateNow.AddDays(-3);
                DateTime dateSeven = dateNow.AddDays(-7);

                if (modelSearch.PlanType == 1)
                {
                    dataQuery = dataQuery.Where(r => r.Status == Constants.Error_Status_No_Plan && r.CreateDate >= dateThree && r.CreateDate < dateNow);
                }
                else if (modelSearch.PlanType == 2)
                {
                    dataQuery = dataQuery.Where(r => r.Status == Constants.Error_Status_No_Plan && r.CreateDate >= dateSeven && r.CreateDate < dateThree);
                }
                else if (modelSearch.PlanType == 3)
                {
                    dataQuery = dataQuery.Where(r => r.Status == Constants.Error_Status_No_Plan && r.CreateDate < dateSeven);
                }
            }

            return dataQuery;
        }

        public SearchResultModel<ComboboxResult> GetListProject()
        {
            SearchResultModel<ComboboxResult> searchResult = new SearchResultModel<ComboboxResult>();
            try
            {
                var ListModel = (from a in db.Projects.AsNoTracking()
                                 orderby a.Name ascending
                                 select new ComboboxResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                 }).AsQueryable();
                searchResult.ListResult = ListModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public SearchResultModel<ComboboxResult> GetModuleMobile(string ProjectId)
        {
            SearchResultModel<ComboboxResult> searchResult = new SearchResultModel<ComboboxResult>();
            var dataQuery = (from a in db.Projects.AsNoTracking()
                             join b in db.ProjectProducts.AsNoTracking() on a.Id equals b.ProjectId
                             join c in db.Modules.AsNoTracking() on b.ModuleId equals c.Id
                             where a.Id.Equals(ProjectId)
                             select new ComboboxResult
                             {
                                 Id = c.Id,
                                 Name = c.Name,
                                 Code = c.Code
                             }).AsQueryable();

            var listResult = dataQuery.ToList();
            searchResult.ListResult = listResult;
            return searchResult;
        }

        private void SendNotifyApprove(ErrorModel errorModel)
        {
            try
            {
                var user = (from e in db.Employees.AsNoTracking()
                            join u in db.Users.AsNoTracking() on e.Id equals u.EmployeeId
                            where u.Id == errorModel.UpdateBy
                            select new
                            {
                                e.Name,
                                u.Id
                            }).FirstOrDefault();

                WebNotifyQueueModel notifyModel = new WebNotifyQueueModel
                {
                    Title = "Thay đổi trạng thái mới: " + errorModel.strHistory,
                    Content = "Mã vấn đề: " + errorModel.Code + ". Nhân viên thực hiện: " + user.Name,
                    CreateDate = DateTime.Now,
                    ObjectType = 0,
                    ObjectId = errorModel.Id,
                    Users = new List<string>() { user.Id }
                };

                _notifyBusiness.SendNotify(notifyModel);
            }
            catch (Exception ex)
            {
                NtsLog.LogError(ex);
            }
        }

        /// <summary>
        /// Yêu cầu xác nhận vấn đề
        /// </summary>
        /// <param name="model"></param>
        public void ConfirmRequest(ErrorModel model, bool isUpdateByOtherPermistion, string userId)
        {
            model.Status = Constants.Problem_Status_Awaiting_Confirm;

            if (string.IsNullOrEmpty(model.Id))
            {
                AddError(model, userId);
            }
            else
            {
                UpdateError(model, isUpdateByOtherPermistion, userId);
            }
        }

        /// <summary>
        /// Hủy yêu cầu xác nhận
        /// </summary>
        /// <param name="model"></param>
        public void CancelRequest(ErrorStatusModel model, string userId)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    errorModel.Status = Constants.Problem_Status_Creating;
                    errorModel.UpdateBy = userId;
                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0062, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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
        /// Xác nhận
        /// </summary>
        /// <param name="model"></param>
        public void Confirm(ErrorModel model, string userId, string departmentId)
        {
            model.Status = Constants.Problem_Status_NoPlan;

            UpdateErrorConfirm(model, userId, departmentId);
        }

        /// <summary>
        /// Hủy xác nhận
        /// </summary>
        /// <param name="model"></param>
        public void CancelConfirm(ErrorStatusModel model, string userId, string departmentId)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            if (!departmentId.Equals(errorModel.DepartmentCreateId))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0034, TextResourceKey.Error);
            }

            var errorFixs = db.ErrorFixs.AsNoTracking().Where(r => r.ErrorId.Equals(model.Id)).FirstOrDefault();
            if (errorFixs != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0087);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    errorModel.Status = Constants.Problem_Status_Awaiting_Confirm;
                    errorModel.UpdateBy = userId;
                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0064, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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
        /// Xác nhận
        /// </summary>
        /// <param name="model"></param>
        public void ConfirmPlan(ErrorModel model, string userId, string employeeId, string departmentId, bool isUpdateByOtherPermistion)
        {
            model.Status = Constants.Problem_Status_Processed;

            UpdateErrorPlan(model, userId, employeeId, departmentId, isUpdateByOtherPermistion);
        }

        /// <summary>
        /// Hủy xác nhận
        /// </summary>
        /// <param name="model"></param>
        public void CancelConfirmPlan(ErrorStatusModel model, string userId, string employeeId, string departmentId, bool isUpdateByOtherPermistion)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            // Trường hợp người chịu trách nhiệm thì được quyền chỉnh sửa kế hoạch của vấn đề
            if (!employeeId.Equals(errorModel.ErrorBy))
            {
                if (!isUpdateByOtherPermistion)
                {
                    throw NTSException.CreateInstance("Bạn không phải là người chịu trách nhiệm, không có quyền điều chỉnh kế hoạch");
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    errorModel.Status = Constants.Problem_Status_NoPlan;
                    errorModel.UpdateBy = userId;
                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0086, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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
        /// Đã xử lý
        /// </summary>
        /// <param name="model"></param>
        public void CompleteProccessing(ErrorModel model, string userId)
        {
            model.Status = Constants.Problem_Status_Awaiting_QC;

            UpdateErrorProcess(model, userId);
        }

        /// <summary>
        /// Hủy đã xử lý
        /// </summary>
        /// <param name="model"></param>
        public void CancelCompleteProccessing(ErrorStatusModel model, string userId)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    errorModel.Status = Constants.Problem_Status_Processed;
                    errorModel.UpdateBy = userId;

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0066, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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
        /// QC đạt
        /// </summary>
        /// <param name="model"></param>
        public void QCOK(ErrorModel model, string userId)
        {
            model.Status = Constants.Problem_Status_Ok_QC;
            model.ActualFinishDate = DateTime.Now;

            UpdateErrorQC(model, userId);
        }

        /// <summary>
        /// QC không đạt
        /// </summary>
        /// <param name="model"></param>
        public void QCNG(ErrorModel model, string userId)
        {
            model.Status = Constants.Problem_Status_NotOk_QC;
            model.ActualFinishDate = null;
            UpdateErrorQC(model, userId);
        }

        /// <summary>
        /// Hủy kết quả QC
        /// </summary>
        /// <param name="model"></param>
        public void CancelResultQC(ErrorStatusModel model, string userId)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    errorModel.Status = Constants.Problem_Status_Awaiting_QC;
                    errorModel.ActualFinishDate = null;
                    errorModel.UpdateBy = userId;

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0069, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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
        /// Đóng vấn đề
        /// </summary>
        /// <param name="model"></param>
        public void CloseError(ErrorStatusModel model, string userId)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    errorModel.Status = Constants.Problem_Status_Close;
                    errorModel.UpdateBy = userId;

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0070, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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

        public void CancelCloseError(ErrorStatusModel model, string userId)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    errorModel.Status = Constants.Problem_Status_Creating;
                    errorModel.UpdateBy = userId;
                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0071, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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
        /// Update vấn đề khắc phục triệt để
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        public void UpdateErrorDone(ErrorStatusModel model, string userId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                var error = db.Errors.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();

                //var jsonApter = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                try
                {
                    error.UpdateBy = userId;
                    error.UpdateDate = DateTime.Now;

                    string contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0059, TextResourceKey.ProblemExist);

                    if (model.Status != error.Status && model.Status == Constants.Problem_Status_Ok_QC)
                    {
                        contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0067, TextResourceKey.ProblemExist);
                    }

                    if (model.Status != error.Status && model.Status == Constants.Problem_Status_NotOk_QC)
                    {
                        contentHistory = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0068, TextResourceKey.ProblemExist);
                    }

                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = error.Id,
                        Content = contentHistory,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

                    error.Status = Constants.Problem_Status_Done;
                    error.ActualFinishDate = DateTime.Now;

                    //var jsonBefor = AutoMapperConfig.Mapper.Map<ErrorLogHistoryModel>(error);

                    //UserLogUtil.LogHistotyUpdateInfo(db, userId, Constants.LOG_Error, error.Id, error.Code, jsonBefor, jsonApter);

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

        public void CancelDone(ErrorStatusModel model, string userId)
        {
            var errorModel = db.Errors.Where(a => a.Id.Equals(model.Id)).FirstOrDefault();
            if (errorModel == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Error);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    errorModel.Status = Constants.Problem_Status_Ok_QC;
                    errorModel.UpdateBy = userId;
                    errorModel.ActualFinishDate = null;
                    ErrorHistory errorHistory = new ErrorHistory()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ErrorId = errorModel.Id,
                        Content = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0071, TextResourceKey.ProblemExist),
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                    };

                    db.ErrorHistories.Add(errorHistory);

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

        public SearchResultModel<ErrorFixResultModel> SearchErrorFixMobile(ErrorSearchModel modelSearch, string departmentId)
        {
            SearchResultModel<ErrorFixResultModel> searchResultModel = new SearchResultModel<ErrorFixResultModel>();

            var dataQuery = (from a in db.ErrorFixs.AsNoTracking()
                             join b in db.Errors.AsNoTracking() on a.ErrorId equals b.Id
                             where a.EmployeeFixId.Equals(modelSearch.FixBy)
                             && a.Status == 1 // chỉ tìm kiếm các công việc chưa hoàn thành
                             join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id
                             join d in db.Projects.AsNoTracking() on b.ProjectId equals d.Id
                             select new ErrorFixResultModel
                             {
                                 Id = a.Id,
                                 ErrorId = a.ErrorId,
                                 Subject = b.Subject,
                                 ErrorCode = b.Code,
                                 Deadline = a.Deadline,
                                 DepartmentName = c.Name,
                                 DepartmentId = a.DepartmentId,
                                 ProjectName = d.Name,
                                 ProjectCode = d.Code,
                                 Type = b.Type,
                                 DateFrom = a.DateFrom,
                                 DateTo = a.DateTo,
                                 Status = a.Status
                             }).AsQueryable();


            if (!string.IsNullOrEmpty(modelSearch.ProjectName))
            {
                dataQuery = dataQuery.Where(u => u.ProjectName.Contains(modelSearch.ProjectName) || u.ProjectCode.Contains(modelSearch.ProjectName));
            }

            if (modelSearch.IsLate)
            {
                var currentDate = DateTimeUtils.ConvertDateFrom(DateTime.Now);
                dataQuery = dataQuery.Where(a => a.DateTo.Value < currentDate);
            }

            if (modelSearch.StartDate.HasValue)
            {
                var startdate = DateTimeUtils.ConvertDateFrom(modelSearch.StartDate.Value);
                dataQuery = dataQuery.Where(a => a.DateFrom.HasValue && a.DateFrom.Value >= startdate);
            }

            if (modelSearch.FinishDate.HasValue)
            {
                var finishDate = DateTimeUtils.ConvertDateFrom(modelSearch.FinishDate.Value);
                dataQuery = dataQuery.Where(a => a.DateTo.HasValue && a.DateTo.Value <= finishDate);
            }

            if (modelSearch.Type != 0)
            {
                if (modelSearch.Type == Constants.Error_Type_Error)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Error);
                }
                else if (modelSearch.Type == Constants.Error_Type_Issue)
                {
                    dataQuery = dataQuery.Where(u => u.Type == Constants.Error_Type_Issue);
                }
            }

            if (!string.IsNullOrEmpty(modelSearch.NameCode))
            {
                dataQuery = dataQuery.Where(u => u.Subject.ToUpper().Contains(modelSearch.NameCode.ToUpper()) || u.ErrorCode.ToUpper().Contains(modelSearch.NameCode.ToUpper()));
            }

            searchResultModel.TotalItem = dataQuery.Count();
            var listResult = dataQuery.OrderByDescending(r => r.DateFrom).ThenByDescending(r => r.ErrorCode).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            searchResultModel.ListResult = listResult;

            foreach (var item in searchResultModel.ListResult)
            {
                item.PlanStartDateView = item.DateFrom.HasValue ? item.DateFrom.Value.ToString("dd/MM/yyyy") : "";
                item.Deadline = (item.DateTo.HasValue && item.DateFrom.HasValue) ? (item.DateTo.Value - item.DateFrom.Value).Days : 0;
            }

            return searchResultModel;
        }

        public ErrorStatistic Statistic(string employeeId, bool hasPermitConfirm, bool hasPermitClose)
        {
            ErrorStatistic searchResultModel = new ErrorStatistic();

            var errors = (from a in db.ErrorFixs.AsNoTracking()
                          join b in db.Errors.AsNoTracking() on a.ErrorId equals b.Id
                          where a.Status == 1
                          select new
                          {
                              a.Id,
                              a.EmployeeFixId,
                              a.DateTo,
                              b.Status,
                          }).ToList();

            if (!string.IsNullOrEmpty(employeeId))
            {
                errors = errors.Where(u => !string.IsNullOrEmpty(u.EmployeeFixId) && u.EmployeeFixId.Equals(employeeId)).ToList();
            }

            var currentDate = DateTimeUtils.ConvertDateFrom(DateTime.Now);

            searchResultModel.TotalIssues = errors.Count();
            searchResultModel.TotalLated = errors.Count(r => r.DateTo < currentDate);

            // Bổ sung thống kê số lượng Vấn đề cần xác nhận, số lượng vấn đề cần Đóng
            //searchResultModel.TotalNeedClose = errors.Count();
            //searchResultModel.TotalNeedConfirm = errors.Count();

            return searchResultModel;
        }

        public void UpdateErrorProcessMobile(ErrorFixModel errorFixModel, string userId)
        {
            ErrorFix errorFix = new ErrorFix();

            if (!string.IsNullOrEmpty(errorFixModel.Id))
            {
                errorFix = db.ErrorFixs.FirstOrDefault(r => r.Id.Equals(errorFixModel.Id));

                if (errorFix != null)
                {
                    errorFix.DepartmentId = errorFixModel.DepartmentId;
                    errorFix.AdviseId = errorFixModel.AdviseId;
                    errorFix.ApproveId = errorFixModel.ApproveId;
                    errorFix.DateFrom = errorFixModel.DateFrom;
                    errorFix.DateTo = errorFixModel.DateTo;
                    errorFix.Deadline = errorFixModel.Deadline;
                    errorFix.EmployeeFixId = errorFixModel.EmployeeFixId;
                    errorFix.NotifyId = errorFixModel.NotifyId;
                    errorFix.Solution = errorFixModel.Solution;
                    errorFix.SupportId = errorFixModel.SupportId;
                    errorFix.EstimateTime = errorFixModel.EstimateTime;

                    if (errorFixModel.Status == 2)
                    {
                        errorFix.Done = 100;
                        errorFix.Status = errorFixModel.Status;
                    }
                    else
                    {
                        errorFix.Status = 1;
                    }

                    var attachs = db.ErrorFixAttachs.Where(r => r.ErrorFixId.Equals(errorFix.Id));

                    // Xóa danh sách file đính kèm và thêm mới lại
                    db.ErrorFixAttachs.RemoveRange(attachs);
                    ErrorFixAttach errorFixAttach;

                    foreach (var file in errorFixModel.FixAttachs)
                    {
                        errorFixAttach = new ErrorFixAttach()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ErrorFixId = errorFix.Id,
                            FileName = file.FileName,
                            FileSize = file.FileSize,
                            Path = file.Path,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now,
                        };
                        db.ErrorFixAttachs.Add(errorFixAttach);
                    }
                }
            }
            db.SaveChanges();

            // Update lại trạng thái Chờ QC nếu danh sách công việc đã xử lý xong hết
            ChangeQC(errorFix.ErrorId);

            db.SaveChanges();
        }

        private void ChangeQC(string errorId)
        {
            var remainErrorFix = db.ErrorFixs.Where(r => r.ErrorId.Equals(errorId) && r.Status != Constants.ErrorFix_Status_Finish).Any();

            if (!remainErrorFix)
            {
                var Error = db.Errors.FirstOrDefault(r => r.Id.Equals(errorId));
                Error.Status = Constants.Problem_Status_Awaiting_QC;
            }
        }


        public ErrorFixModel GetErrorFixInfo(ErrorFixModel model)
        {
            var resultInfo = (from a in db.ErrorFixs.AsNoTracking()
                              where a.Id.Equals(model.Id)
                              join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                              select new ErrorFixModel
                              {
                                  AdviseId = a.AdviseId,
                                  Id = a.Id,
                                  DateFrom = a.DateFrom,
                                  DateTo = a.DateTo,
                                  Deadline = a.Deadline,
                                  DepartmentId = a.DepartmentId,
                                  ApproveId = a.ApproveId,
                                  DepartmentName = d.Name,
                                  EmployeeFixId = a.EmployeeFixId,
                                  NotifyId = a.NotifyId,
                                  Solution = a.Solution,
                                  Status = a.Status,
                                  SupportId = a.SupportId,
                                  EstimateTime = a.EstimateTime,
                                  Done = a.Done,
                              }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ErrorFix);
            }

            resultInfo.FixAttachs = (from a in db.ErrorFixAttachs.AsNoTracking()
                                     where a.ErrorFixId.Equals(resultInfo.Id)
                                     select new ErrorFixAttachModel
                                     {
                                         FileName = a.FileName,
                                         FileSize = a.FileSize,
                                         Id = a.Id,
                                         Path = a.Path

                                     }).ToList();

            return resultInfo;
        }
    }
}

