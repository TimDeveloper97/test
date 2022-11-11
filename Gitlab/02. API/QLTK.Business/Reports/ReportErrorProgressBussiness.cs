using NTS.Common;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Report;
using NTS.Model.Reports.ReportError;
using NTS.Model.Reports.ReportErrorProgress;
using NTS.Model.ReportStatusModule;
using NTS.Model.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using static NTS.Model.Reports.ReportError.ReportErorrWorkChangePlanModel;

namespace QLTK.Business.ReportStatusModule
{
    public class ReportErrorProgressBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        public ReportErrorProgressResultModel Report(ReportErrorProgressSearchModel searchModel)
        {
            ReportErrorProgressResultModel result = new ReportErrorProgressResultModel();

            int weekStart = 8;
            int weekTotal = 12;
            int dayStart = 1;
            int dayTotal = 14;
            ReportErrorProgressResultLabelDayModel dayLabel;
            for (int i = dayTotal; i > 0; i--)
            {
                dayLabel = new ReportErrorProgressResultLabelDayModel()
                {
                    IsToDayECP = i == dayStart,
                    TitleECP = i == dayStart? "Ngày HT":  $"Ngày HT - { i - dayStart}"
                };

                result.Days.Add(dayLabel);
            }
            DateTime dateNowMonday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);

            DateTime weekStartDate = dateNowMonday.AddDays((1 - weekStart) * 7);
            DateTime weekEndDate = weekStartDate.AddDays(weekTotal * 7 - 1);

            DateTime dayStartDate = dateNowMonday.AddDays((1 - dayStart) * 7);
            DateTime dayEndDate = dayStartDate.AddDays(1 - dayTotal);

            ReportErrorProgressResultLabelModel weekLabel;
            for (int i = 1; i <= weekTotal; i++)
            {
                weekLabel = new ReportErrorProgressResultLabelModel()
                {
                    IsToDay = i == weekStart,
                    Title = i == weekStart? "Tuần HT" :(i < weekStart? $"Tuần HT - {weekStart - i}" : $"Tuần HT + {i  - weekStart}")
                };

                result.Weeks.Add(weekLabel);
            }

            DateTime dateNow = DateTime.Now.Date;

            //Danh sách thông tin chi tiết của công việc
            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join t in db.ErrorChangedPlans.AsNoTracking() on a.Id equals t.ErrorId
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                             join d in db.Employees.AsNoTracking() on c.EmployeeFixId equals d.Id into cd
                             from cdn in cd.DefaultIfEmpty()
                             join e in db.Departments.AsNoTracking() on c.DepartmentId equals e.Id into ce
                             from cen in ce.DefaultIfEmpty()
                             where a.Status != Constants.Error_Status_Close && a.Status > Constants.Problem_Status_Awaiting_Confirm
                             && ((c.DateFrom >= weekStartDate && c.DateFrom <= weekEndDate) || (c.DateFrom < weekStartDate && c.DateTo >= weekStartDate) || (c.DateTo < weekStartDate && c.Status != Constants.ErrorFix_Status_Finish))
                             select new ReportErrorInfoModel
                             {
                                 Id = a.Id,
                                 PlanFinishDate = a.PlanFinishDate,
                                 PlanStartDate = a.PlanStartDate,
                                 StageId = a.StageId,
                                 ProjectId = a.ProjectId,
                                 Status = a.Status,
                                 FixStatus = c.Status,
                                 Deadline = c.DateTo.HasValue && dateNow > c.DateTo && c.Status != Constants.ErrorFix_Status_Finish ? DbFunctions.DiffDays(c.DateTo, dateNow).Value : 0,
                                 ProjectStatus = b.Status,
                                 EmployeeName = cdn != null ? cdn.Name : string.Empty,
                                 EmployeeFixId = c.EmployeeFixId,
                                 FixDepartmentId = c.DepartmentId,
                                 FixDepartmentName = cen != null ? cen.Name : string.Empty,
                                 DepartmentManageId = b.DepartmentId,
                                 ProjectAmount = b.SaleNoVAT,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 FinishDate = c.FinishDate,
                                 CustomerFinalId = b.CustomerFinalId,
                                 CustomerId = b.CustomerId,
                                 DepartmentId = a.DepartmentId,
                                 ErrorChangePlanId = t.Id,
                                 ErrorId = a.Id,
                                 ErrorFixId = c.Id
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ProjectId))
            {
                dataQuery = dataQuery.Where(u => u.ProjectId.Equals(searchModel.ProjectId));
            }

            if (!string.IsNullOrEmpty(searchModel.ErrorPlanStatus))
            {
                // Thống kê công việc Trong thời hạn
                if (searchModel.ErrorPlanStatus.Equals("0"))
                {
                    dataQuery = dataQuery.Where(r => (r.DateTo > dateNow) && (r.FixStatus != Constants.ErrorFix_Status_Finish));
                }
                else
                {
                    // Thống kê công việc Trễ hạn
                    dataQuery = dataQuery.Where(r => (r.DateTo < dateNow) && (r.FixStatus != Constants.ErrorFix_Status_Finish));
                }
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectStatus))
            {
                dataQuery = dataQuery.Where(u => u.ProjectStatus.Equals(searchModel.ProjectStatus));
            }

            if (!string.IsNullOrEmpty(searchModel.FixDepartmentId))
            {
                dataQuery = dataQuery.Where(u => searchModel.FixDepartmentId.Equals(u.FixDepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentId.Equals(u.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => searchModel.EmployeeId.Equals(u.EmployeeFixId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentManageId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentManageId.Equals(u.DepartmentManageId));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerFinalId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerFinalId.Equals(u.CustomerFinalId));
            }

            var errors = dataQuery.ToList();

            var errorDeparments = errors.Where(r => !string.IsNullOrEmpty(r.FixDepartmentId)).GroupBy(
                                               g => new { g.FixDepartmentId, g.FixDepartmentName }).Select(
                                               s => new { s.Key.FixDepartmentName, s.Key.FixDepartmentId }).OrderBy(
                                               o => o.FixDepartmentName).ToList();
            var errorEmployees = errors.Where(r => !string.IsNullOrEmpty(r.EmployeeFixId)).GroupBy(
                                               g => new { g.EmployeeFixId, g.EmployeeName }).Select(
                                               s => new { s.Key.EmployeeName, s.Key.EmployeeFixId }).ToList();

            ReportErrorProgressResultObjectModel objectModel;
            ReportErrorProgressResultValueModel valueModel;
            List<ReportErrorInfoModel> errorWeeks;
            List<ReportErrorInfoModel> errorBefore;
            List<ReportErrorInfoModel> errorDep;
            List<ReportErrorInfoModel> errorIn;
            DateTime weekStartSearchDate;
            DateTime weekEndSearchDate;
            foreach (var department in errorDeparments)
            {
                objectModel = new ReportErrorProgressResultObjectModel()
                {
                    Id = department.FixDepartmentId,
                    Name = !string.IsNullOrEmpty(department.FixDepartmentName) ? department.FixDepartmentName : "Chưa có bộ phận khắc phục",
                    DateFrom = weekStartDate,
                    DateTo = weekEndDate
                };

                for (int i = 1; i <= weekTotal; i++)
                {
                    weekStartSearchDate = weekStartDate.AddDays((i - 1) * 7);
                    weekEndSearchDate = weekStartSearchDate.AddDays(6);

                    errorWeeks = errors.Where(r => r.DateTo >= weekStartSearchDate && r.DateFrom <= weekEndSearchDate && department.FixDepartmentId.Equals(r.FixDepartmentId)).ToList();
                    valueModel = new ReportErrorProgressResultValueModel()
                    {
                        IsToDay = i == weekStart,
                        Value = errorWeeks.Count(c => c.Deadline > 0 && c.DateTo <= weekEndSearchDate),
                        DateFrom = weekStartSearchDate,
                        DateTo = weekEndSearchDate
                    };
                    objectModel.DelayValues.Add(valueModel);

                    valueModel = new ReportErrorProgressResultValueModel()
                    {
                        IsToDay = i == weekStart,
                        IsLessToDay = i < weekStart,
                        Value = errorWeeks.Count(c => !c.FinishDate.HasValue || c.FinishDate > weekEndSearchDate || (c.FinishDate >= weekStartSearchDate && c.FinishDate <= weekEndSearchDate)),
                        DateFrom = weekStartSearchDate,
                        DateTo = weekEndSearchDate
                    };

                    objectModel.PlanValues.Add(valueModel);
                }

                errorDep = errors.Where(r => department.FixDepartmentId.Equals(r.FixDepartmentId)).ToList();

                errorBefore = errorDep.Where(r => r.DateTo < weekStartDate).ToList();
                objectModel.OpeningTotal = errorBefore.Count;

                objectModel.DelayTotal = objectModel.DelayValues.Sum(r => r.Value);

                errorIn = errorDep.Where(r => (r.DateFrom >= weekStartDate && r.DateFrom <= weekEndDate) || (r.DateFrom < weekStartDate && r.DateTo >= weekStartDate)).ToList();

                objectModel.Total = errorIn.Count;
                objectModel.FinishTotal = errorIn.Count(r => r.FixStatus == Constants.ErrorFix_Status_Finish);
                objectModel.FinishPercent = objectModel.Total > 0 ? Math.Round((objectModel.FinishTotal / objectModel.Total) * 100) : 0;

                if (objectModel.DelayTotal > 0 || objectModel.Total > 0 || objectModel.OpeningTotal > 0)
                {
                    result.ErrorFixs.Add(objectModel);
                }
            }

            foreach (var employee in errorEmployees)
            {
                objectModel = new ReportErrorProgressResultObjectModel()
                {
                    Id = employee.EmployeeFixId,
                    Name = !string.IsNullOrEmpty(employee.EmployeeName) ? employee.EmployeeName : "Chưa có người khắc phục",
                    DateFrom = weekStartDate,
                    DateTo = weekEndDate
                };

                for (int i = 1; i <= weekTotal; i++)
                {
                    weekStartSearchDate = weekStartDate.AddDays((i - 1) * 7);
                    weekEndSearchDate = weekStartSearchDate.AddDays(6);

                    errorWeeks = errors.Where(r => r.DateTo >= weekStartSearchDate && r.DateFrom <= weekEndSearchDate && employee.EmployeeFixId.Equals(r.EmployeeFixId)).ToList();
                    valueModel = new ReportErrorProgressResultValueModel()
                    {
                        IsToDay = i == weekStart,
                        Value = errorWeeks.Count(c => c.Deadline > 0 && c.DateTo <= weekEndSearchDate),
                        DateFrom = weekStartSearchDate,
                        DateTo = weekEndSearchDate
                    };
                    objectModel.DelayValues.Add(valueModel);

                    valueModel = new ReportErrorProgressResultValueModel()
                    {
                        IsToDay = i == weekStart,
                        IsLessToDay = i < weekStart,
                        Value = errorWeeks.Count(c => !c.FinishDate.HasValue || c.FinishDate > weekEndSearchDate || (c.FinishDate >= weekStartSearchDate && c.FinishDate <= weekEndSearchDate)),
                        DateFrom = weekStartSearchDate,
                        DateTo = weekEndSearchDate
                    };

                    objectModel.PlanValues.Add(valueModel);
                }

                errorDep = errors.Where(r => employee.EmployeeFixId.Equals(r.EmployeeFixId)).ToList();

                errorBefore = errorDep.Where(r => r.DateTo < weekStartDate).ToList();
                objectModel.OpeningTotal = errorBefore.Count;

                objectModel.DelayTotal = objectModel.DelayValues.Sum(r => r.Value);

                errorIn = errorDep.Where(r => (r.DateFrom >= weekStartDate && r.DateFrom <= weekEndDate) || (r.DateFrom < weekStartDate && r.DateTo >= weekStartDate)).ToList();

                objectModel.Total = errorIn.Count;
                objectModel.FinishTotal = errorIn.Count(r => r.FixStatus == Constants.ErrorFix_Status_Finish);
                objectModel.FinishPercent = objectModel.Total > 0 ? Math.Round((objectModel.FinishTotal / objectModel.Total) * 100) : 0;

                if (objectModel.DelayTotal > 0 || objectModel.Total > 0 || objectModel.OpeningTotal > 0)
                {
                    result.ErrorFixBys.Add(objectModel);
                }
            }

            result.ErrorFixs = result.ErrorFixs.OrderByDescending(r => r.DelayTotal).ThenByDescending(r => r.Total).ToList();
            result.ErrorFixBys = result.ErrorFixBys.OrderByDescending(r => r.DelayTotal).ThenByDescending(r => r.Total).ToList();

            return result;
        }


        public List<StatisticErrorChangePlanedModel> reportErrorChangePlan(StatisticErrorChangePlanedModel searchModel)
        {

            List<string> departmentIds = new List<string>();
            DateTime today = DateTime.Now;
            DateTime searchDate;
            DateTime dateFrom;
            DateTime dateTo;
            var errorChangePlansx = db.ErrorChangedPlans.AsNoTracking();

            // Danh sách phòng ban có điều chỉnh
            var listDepartment = (from r in db.Errors.AsNoTracking()
                                  where r.Status == Constants.Error_Status_Processing || r.Status == Constants.Error_Status_Waiting_QC || r.Status == Constants.Error_Status_Fail_QC
                                  join a in db.Projects.AsNoTracking() on r.ProjectId equals a.Id into ra
                                  from rac in ra.DefaultIfEmpty()
                                  join b in db.Departments.AsNoTracking() on rac.DepartmentId equals b.Id into ab
                                  from abc in ab.DefaultIfEmpty()
                                  join c in db.ErrorChangedPlans.AsNoTracking() on r.ProjectId equals c.ProjectId
                                  group r by new { abc.Id, abc.Name } into g // Group phòng ban trùng thông tin
                                  select new { g.Key.Id, g.Key.Name }).ToList();

            var error = (from r in db.Errors.AsNoTracking()
                         join a in db.Projects.AsNoTracking() on r.ProjectId equals a.Id
                         select new { r.ProjectId, r.Id, a.DepartmentId }).ToList();


            List<StatisticErrorChangePlanedModel> report = new List<StatisticErrorChangePlanedModel>();
            StatisticErrorChangePlanedModel statisticErrorChangePlanedModel;

            foreach (var item in listDepartment)
            {
                statisticErrorChangePlanedModel = new StatisticErrorChangePlanedModel();

                var errorIds = error.Where(r => r.DepartmentId.Equals(item.Id)).Select(r => r.Id).ToList();


                // Add số công việc có điều chỉnh kế hoạch

                for (int i = 13; i >= 0; i--)
                {
                    searchDate = today.AddDays(-i);
                    dateFrom = DateTimeUtils.ConvertDateFrom(searchDate);
                    dateTo = DateTimeUtils.ConvertDateTo(searchDate);

                    var ad = errorChangePlansx.Where(r => r.ChangeDate >= dateFrom && r.ChangeDate <= dateTo && errorIds.Contains(r.ErrorId)).ToList();

                    statisticErrorChangePlanedModel.NumOfChange.Add(errorChangePlansx.Where(r => r.ChangeDate >= dateFrom && r.ChangeDate <= dateTo && errorIds.Contains(r.ErrorId)).GroupBy(a => a.ErrorId).Count());
                    
                }


                // Lấy số lượng vấn đề tồn của kỳ trước
                dateFrom = DateTimeUtils.ConvertDateFrom(today.AddDays(-13));
                statisticErrorChangePlanedModel.TotalPreviousPeriod = errorChangePlansx.Where(r => r.ChangeDate < dateFrom && errorIds.Contains(r.ErrorId)).GroupBy(a => a.ErrorId).Count();
                statisticErrorChangePlanedModel.TotalError = statisticErrorChangePlanedModel.NumOfChange.Sum() + statisticErrorChangePlanedModel.TotalPreviousPeriod;

                statisticErrorChangePlanedModel.DepartmentName = item.Name;
                statisticErrorChangePlanedModel.Id = item.Id;

                report.Add(statisticErrorChangePlanedModel);
            }

            return report;

        }


        public SearchResultModel<ReportErrorWorkModel> GetWork(ReportErrorProgressSearchModel searchModel)
        {
            DateTime dateNow = DateTime.Now.Date;
            var dataQuery = (from a in db.Errors.AsNoTracking()
                             join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                             join c in db.ErrorFixs.AsNoTracking() on a.Id equals c.ErrorId
                             join d in db.Modules.AsNoTracking() on a.ObjectId equals d.Id into ad
                             from adn in ad.DefaultIfEmpty()
                             join e in db.Products.AsNoTracking() on a.ObjectId equals e.Id into ae
                             from aen in ae.DefaultIfEmpty()
                             join m in db.Departments.AsNoTracking() on c.DepartmentId equals m.Id into cm
                             from cmn in cm.DefaultIfEmpty()
                             join s in db.Stages.AsNoTracking() on a.StageId equals s.Id into ast
                             from astn in ast.DefaultIfEmpty()
                             where a.Status != Constants.Error_Status_Close
                             orderby c.DateFrom
                             select new ReportErrorWorkModel
                             {
                                 Id = a.Id,
                                 PlanFinishDate = a.PlanFinishDate,
                                 PlanStartDate = a.PlanStartDate,
                                 DateFrom = c.DateFrom,
                                 DateTo = c.DateTo,
                                 FinishDate = c.FinishDate,
                                 StageId = a.StageId,
                                 ProjectId = a.ProjectId,
                                 Status = c.Status,
                                 Deadline = c.DateTo.HasValue && dateNow > c.DateTo && c.Status != Constants.ErrorFix_Status_Finish ? DbFunctions.DiffDays(c.DateTo, dateNow).Value : 0,
                                 ProjectStatus = b.Status,
                                 EmployeeFixId = c.EmployeeFixId,
                                 DepartmentId = c.DepartmentId,
                                 DepartmentName = cmn != null ? cmn.Name : string.Empty,
                                 AdviseId = c.AdviseId,
                                 ApproveId = c.ApproveId,
                                 SupportId = c.SupportId,
                                 NotifyId = c.NotifyId,
                                 ErrorCode = a.Code,
                                 Subject = a.Subject,
                                 ModuleCode = adn != null ? adn.Code : string.Empty,
                                 ModuleName = adn != null ? adn.Name : string.Empty,
                                 ProductCode = aen != null ? aen.Code : string.Empty,
                                 ProductName = aen != null ? aen.Name : string.Empty,
                                 ProjectCode = b.Code,
                                 ProjectName = b.Name,
                                 EstimateTime = c.EstimateTime,
                                 Solution = c.Solution,
                                 DepartmentManageId = b.DepartmentId,
                                 CustomerFinalId = b.CustomerFinalId,
                                 CustomerId = b.CustomerId,
                                 StageName = astn != null ? astn.Name : string.Empty
                             }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.ProjectId))
            {
                dataQuery = dataQuery.Where(u => u.ProjectId.Equals(searchModel.ProjectId));
            }

            if (!string.IsNullOrEmpty(searchModel.ProjectStatus))
            {
                dataQuery = dataQuery.Where(u => u.ProjectStatus.Equals(searchModel.ProjectStatus));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentId.Equals(u.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.EmployeeId))
            {
                dataQuery = dataQuery.Where(u => searchModel.EmployeeId.Equals(u.EmployeeFixId));
            }

            if (!string.IsNullOrEmpty(searchModel.StageId))
            {
                dataQuery = dataQuery.Where(u => searchModel.StageId.Equals(u.StageId));
            }

            if (searchModel.IsOpening)
            {
                dataQuery = dataQuery.Where(r => r.DateTo < searchModel.DateFrom && r.Status != Constants.ErrorFix_Status_Finish);
            }
            else
            {
                if (searchModel.DateFrom.HasValue && searchModel.DateTo.HasValue)
                {
                    dataQuery = dataQuery.Where(r => r.DateTo >= searchModel.DateFrom && r.DateFrom <= searchModel.DateTo);
                }
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerId.Equals(u.CustomerId));
            }

            if (!string.IsNullOrEmpty(searchModel.CustomerFinalId))
            {
                dataQuery = dataQuery.Where(u => searchModel.CustomerFinalId.Equals(u.CustomerFinalId));
            }

            if (!string.IsNullOrEmpty(searchModel.DepartmentManageId))
            {
                dataQuery = dataQuery.Where(u => searchModel.DepartmentManageId.Equals(u.DepartmentManageId));
            }

            if (searchModel.WorkType == 1)
            {
                dataQuery = dataQuery.Where(r => r.Deadline > 0 && r.Status != Constants.ErrorFix_Status_Finish && r.DateTo <= searchModel.DateTo);
            }
            else if (searchModel.WorkType == 2)
            {
                dataQuery = dataQuery.Where(r => !r.FinishDate.HasValue || r.FinishDate > searchModel.DateTo || (r.FinishDate >= searchModel.DateFrom && r.FinishDate <= searchModel.DateTo));
            }

            SearchResultModel<ReportErrorWorkModel> result = new SearchResultModel<ReportErrorWorkModel>();

            result.TotalItem = dataQuery.Count();
            result.ListResult = dataQuery.Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            return result;
        }

        public object GetWorkChangePlan(ReportErrorProgressSearchModelWorkChangePlanModel searchModel)
        {
            DateTime searchDate = DateTime.Now.AddDays(-(13 - searchModel.Index));
            DateTime searchFrom = DateTimeUtils.ConvertDateFrom(searchDate);
            DateTime searchTo = DateTimeUtils.ConvertDateTo(searchDate);

            if(searchModel.Index == -9999)
            {
                searchDate = DateTime.Now.AddDays(-14);
                searchFrom = DateTimeUtils.ConvertDateFrom(new DateTime(2022, 1,1));
                searchTo = DateTimeUtils.ConvertDateTo(searchDate);
            }

            DateTime dateNow = DateTime.Now.Date;

            var data = (from r in db.ErrorFixs.AsNoTracking()
                        join error in db.Errors.AsNoTracking() on r.ErrorId equals error.Id
                        join proj in db.Projects.AsNoTracking() on error.ProjectId equals proj.Id
                        join errorChanged in db.ErrorChangedPlans on r.Id equals errorChanged.ErrorFixId
                        join module in db.Modules.AsNoTracking() on error.ObjectId equals module.Id into errormodule
                        from adn in errormodule.DefaultIfEmpty()
                        join product in db.Products.AsNoTracking() on error.ObjectId equals product.Id into errorproduct
                        from aen in errorproduct.DefaultIfEmpty()
                        join department in db.Departments.AsNoTracking() on proj.DepartmentId equals department.Id
                        join employee in db.Employees.AsNoTracking() on r.EmployeeFixId equals employee.Id
                        select new ReportErorrWorkChangePlanModel()
                        {
                            ErrorId = error.Id,
                            ErrorFixId = r.Id,
                            ProjectCode = proj.Code,
                            ProjectName = proj.Name,
                            ModuleCode = adn != null ? adn.Code : string.Empty,
                            ModuleName = adn != null ? adn.Name : string.Empty,
                            ProductCode = aen != null ? aen.Code : string.Empty,
                            ProductName = aen != null ? aen.Name : string.Empty,
                            ErrorCode = error.Code,
                            Subject = error.Subject,
                            ChangeDate = errorChanged.ChangeDate,
                            DepartmentName = department.Name,
                            FixByName = employee.Name,
                            DateFrom = r.DateFrom,
                            DateTo = r.DateTo,
                            DepartmentId = department.Id,
                            Deadline = r.DateTo.HasValue && dateNow > r.DateTo && r.Status != Constants.ErrorFix_Status_Finish ? DbFunctions.DiffDays(r.DateTo, dateNow).Value : 0,
                            NewStartDate = errorChanged.NewStartDate,
                            NewFinishDate = errorChanged.NewFinishDate,
                            Reason = errorChanged.Reason,
                            Solution = r.Solution
                        }).AsQueryable();


            data = data.Where(u => u.ChangeDate >= searchFrom && u.ChangeDate <= searchTo);

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                data = data.Where(u => searchModel.DepartmentId.Equals(u.DepartmentId));
            }

            var dataQuery = data.ToList();
            var list = (from a in dataQuery
                        group a by new
                        {
                            ErrorId = a.ErrorId,
                            ErrorFixId = a.ErrorFixId,
                            ProjectCode = a.ProjectCode,
                            ProjectName = a.ProjectName,
                            ErrorCode = a.ErrorCode,
                            Subject = a.Subject,
                            ModuleCode = a.ModuleCode,
                            ModuleName = a.ModuleName,
                            ProductCode = a.ProductCode,
                            ProductName = a.ProductName,
                            DepartmentName = a.DepartmentName,
                            FixByName = a.FixByName,
                            Solution = a.Solution,
                            DateFrom = a.DateFrom,
                            DateTo = a.DateTo,
                            DepartmentId = a.DepartmentId,
                            Deadline = a.Deadline,
                        } into g
                        select new ReportErorrWorkChangePlanModel
                        {
                            ErrorId = g.Key.ErrorId,
                            ErrorFixId = g.Key.ErrorFixId,
                            ProjectCode = g.Key.ProjectCode,
                            ProjectName = g.Key.ProjectName,
                            ErrorCode = g.Key.ErrorCode,
                            Subject = g.Key.Subject,
                            ModuleCode = g.Key.ModuleCode,
                            ModuleName = g.Key.ModuleName,
                            ProductCode = g.Key.ProductCode,
                            ProductName = g.Key.ProductName,
                            DepartmentName = g.Key.DepartmentName,
                            FixByName = g.Key.FixByName,
                            Solution = g.Key.Solution,
                            DateFrom = g.Key.DateFrom,
                            DateTo = g.Key.DateTo,
                            DepartmentId = g.Key.DepartmentId,
                            Deadline = g.Key.Deadline,
                            TotalChange = g.Where(i => i.ChangeDate.HasValue).Count()
                        }).ToList();

            int max = list.Count > 0 ? list.Max(a => a.TotalChange): 0;
            foreach (var item in list)
            {
                item.ListChange = (from a in dataQuery
                                   where a.ErrorFixId.Equals(item.ErrorFixId)
                                   select new InforChange
                                   {
                                       NewStartDate = a.NewStartDate,
                                       NewFinishDate = a.NewFinishDate,
                                       Reason = a.Reason,
                                   }).ToList();

                if (item.ListChange.Count < max)
                {
                    for (int i = 0; i <= max - item.ListChange.Count; i++)
                    {
                        item.ListChange.Add(new InforChange
                        {
                            Id = i.ToString(),
                            Reason = "-",
                        });
                    }
                }
            }

            List<ReportErorrWorkChangePlanModel> result = new List<ReportErorrWorkChangePlanModel>();
            result = list.ToList();

            List<DateChange> listHead = new List<DateChange>();

            if (max > 0)
            {
                for (int i = 1; i <= max; i++)
                {
                    listHead.Add(new DateChange
                    {
                        Title = $"Ngày điều chỉnh lần {string.Format("{0:00}", i)}"
                    });
                }
            }

            return new
            {
                result,
                listHead
            };
        }

        public string ExportExcel(ReportErrorSearchModel searchModel)
        {
            //var data = Report(searchModel);
            //List<ReportStatusModuleModel> listModel = data.ListModule;

            //if (listModel.Count == 0)
            //{
            //    throw NTSException.CreateInstance("Không có dữ liệu!");
            //}
            //try
            //{
            //    ExcelEngine excelEngine = new ExcelEngine();

            //    IApplication application = excelEngine.Excel;

            //    IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/ReportStatusModule.xlsx"));

            //    IWorksheet sheet = workbook.Worksheets[0];

            //    var total = listModel.Count;

            //    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            //    iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            //    var listExport = listModel.Select((a, i) => new
            //    {
            //        Index = i + 1,
            //        a.ModuleName,
            //        a.ModuleCode,
            //        a.TotalModule,
            //        a.TotalModuleInProject,
            //        ViewStatus = a.Status == 1 ? "Chỉ sử dụng một lần" : a.Status == 2 ? "Module chuẩn" : a.Status == 3 ? "Module ngừng sử dụng" : "",
            //    });

            //    if (listExport.Count() > 1)
            //    {
            //        sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            //    }
            //    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            //    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 6].Borders.Color = ExcelKnownColors.Black;
            //    //sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].CellStyle.WrapText = true;

            //    string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Báo cáo tình trạng module" + ".xls");
            //    workbook.SaveAs(pathExport);
            //    workbook.Close();
            //    excelEngine.Dispose();

            //    //Đường dẫn file lưu trong web client
            //    string resultPathClient = "Template/" + Constants.FolderExport + "Báo cáo tình trạng module" + ".xls";

            //    return resultPathClient;
            //}
            //catch (Exception ex)
            //{
            //    //Log.LogError(ex);
            //    throw new Exception("Có lỗi trong quá trình xử lý" + ex.Message);
            //}

            return string.Empty;
        }
    }
}
