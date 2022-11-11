using NTS.Common;
using NTS.Common.Resource;
using NTS.Model;
using NTS.Model.Combobox;
using NTS.Model.Download;
using NTS.Model.Employees;
using NTS.Model.HistoryVersion;
using NTS.Model.PlanHistory;
using NTS.Model.Plans;
using NTS.Model.ProjectProducts;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using NTS.Model.SolutionOldVersion;
using NTS.Model.Task;
using NTS.Model.TaskFlowStage;
using NTS.Model.TaskModuleGroup;
using NTS.Model.WorkDiary;
using NTS.Utils;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using QLTK.Business.AutoMappers;
using QLTK.Business.ScheduleProject;
using QLTK.Business.TaskModule;
using QLTK.Business.Users;
using QLTK.Business.WorkDiarys;
using RabbitMQ.Client.Framing.Impl;
using Syncfusion.DocIO.DLS;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;
using static log4net.Appender.RollingFileAppender;
using DateTimeUtils = NTS.Utils.DateTimeUtils;

namespace QLTK.Business.Plans
{
    public class PlanBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        private readonly WorkDiaryBussiness workDiaryBussiness = new WorkDiaryBussiness();

        public SearchResultModel<PlanResultModel> SearchPlans(PlanSearchModel modelSearch)
        {
            SearchResultModel<PlanResultModel> searchResult = new SearchResultModel<PlanResultModel>();

            var plans = (from a in db.Plans.AsNoTracking()
                         where a.IsPlan
                         join x in db.PlanAssignments.AsNoTracking() on a.Id equals x.PlanId
                         join d in db.Employees.AsNoTracking() on x.EmployeeId equals d.Id
                         join f in db.Departments.AsNoTracking() on d.DepartmentId equals f.Id
                         select new PlanResultModel()
                         {
                             Id = a.Id,
                             ProjectId = a.ProjectId,
                             DepartmentId = d.DepartmentId,
                             SBUId = f.SBUId,
                             ResponsiblePersion = x.EmployeeId,
                             ResponsiblePersionName = d.Name,
                             StartDate = a.ContractStartDate,
                             EndDate = a.ContractDueDate,
                             PlanStartDate = a.PlanStartDate,
                             PlanDueDate = a.PlanDueDate,
                             ActualStartDate = a.ActualStartDate,
                             ActualEndDate = a.ActualEndDate,
                             EmployeeCode = d.Code,
                             EmployeeId = d.Id,
                             Done = a.DoneRatio,
                             Description = a.Description,
                             Types = 1,
                             ProjectProductId = a.ProjectProductId,
                             TaskName = a.Name,
                             statusDones = a.Status,
                             EstimateTimes = a.EstimateTime,
                             Duration = a.Duration,
                         }).AsQueryable();

            var errorfix = (from r in db.ErrorFixs.AsNoTracking()
                            join j in db.Errors.AsNoTracking() on r.ErrorId equals j.Id
                            where j.Status > 3
                            join e in db.Employees.AsNoTracking() on r.EmployeeFixId equals e.Id
                            join d in db.Departments.AsNoTracking() on r.DepartmentId equals d.Id
                            select new PlanResultModel()
                            {
                                Id = r.Id,
                                ProjectId = j.ProjectId,
                                DepartmentId = e.DepartmentId,
                                SBUId = d.SBUId,
                                ResponsiblePersion = r.EmployeeFixId,
                                ResponsiblePersionName = e.Name,
                                StartDate = r.DateFrom,
                                EndDate = r.DateTo,
                                PlanStartDate = r.DateFrom,
                                PlanDueDate = r.DateTo,
                                ActualStartDate = r.ActualStartDate,
                                ActualEndDate = r.FinishDate,
                                EmployeeCode = e.Code,
                                EmployeeId = e.Id,
                                Done = r.Done,
                                Description = j.Description,
                                Types = 2,
                                ProjectProductId = "",
                                TaskName = j.Code,
                                statusDones = r.Done == 100 ? 3 : r.Done > 0 && r.Done < 100 ? 2 : 1,
                                EstimateTimes = r.EstimateTime.HasValue ? (decimal)r.EstimateTime : 0,
                                Duration = 0,
                            }).AsQueryable();

            var quotationPlan = (from r in db.QuotationPlans.AsNoTracking()
                                join e in db.Employees.AsNoTracking() on r.EmployeeId equals e.Id
                                join d in db.Departments.AsNoTracking() on e.DepartmentId equals d.Id
                                join u in db.SBUs.AsNoTracking() on d.SBUId equals u.Id
                                select new PlanResultModel()
                                {
                                    Id = r.Id,
                                    ProjectId = null,
                                    DepartmentId = d.Id,
                                    SBUId = u.Id,
                                    ResponsiblePersion = r.EmployeeId,
                                    ResponsiblePersionName = e.Name,
                                    StartDate = r.UpdateDate,
                                    EndDate = null,
                                    PlanStartDate = r.PlanStartDate,
                                    PlanDueDate = r.PlanDueDate,
                                    ActualStartDate = r.ActualStartDate,
                                    ActualEndDate = r.ActualEndDate,
                                    EmployeeCode = null,
                                    EmployeeId = null,
                                    Done = r.DoneRatio,
                                    Description = r.Descripton,
                                    Types = 3,
                                    ProjectProductId = null,
                                    TaskName = r.Name,
                                    statusDones = r.DoneRatio == 100 ? 3 : r.DoneRatio > 0 && r.DoneRatio < 100 ? 2 : 1,
                                    EstimateTimes = r.EstimateTime,
                                    Duration = 0,
                                }).AsQueryable();

            if (modelSearch.Status > 0)
            {
                plans = plans.Where(u => u.statusDones == modelSearch.Status);
                errorfix = errorfix.Where(u => u.statusDones == modelSearch.Status);
                quotationPlan = quotationPlan.Where(u => u.statusDones == modelSearch.Status);
            }

            if (!string.IsNullOrEmpty(modelSearch.EmployeeCode))
            {
                plans = plans.Where(u => u.EmployeeCode.ToUpper().Contains(modelSearch.EmployeeCode.ToUpper()) || u.ResponsiblePersionName.ToUpper().Contains(modelSearch.EmployeeCode.ToUpper()));
            }
            if (modelSearch.Types != 0)
            {
                plans = plans.Where(u => u.Types == modelSearch.Types);
                errorfix = errorfix.Where(u => u.Types == modelSearch.Types);
                quotationPlan = quotationPlan.Where(u => u.Types == modelSearch.Types);
            }

            if (!string.IsNullOrEmpty(modelSearch.TaskName))
            {
                plans = plans.Where(u => u.TaskName.ToUpper().Contains(modelSearch.TaskName.Trim().ToUpper()));
                errorfix = errorfix.Where(u => u.TaskName.ToUpper().Contains(modelSearch.TaskName.Trim().ToUpper()));
                quotationPlan = quotationPlan.Where(u => u.TaskName.ToUpper().Contains(modelSearch.TaskName.Trim().ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.ProjectId))
            {
                plans = plans.Where(u => u.ProjectId.Equals(modelSearch.ProjectId));
                errorfix = errorfix.Where(u => u.ProjectId.Equals(modelSearch.ProjectId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                plans = plans.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
                errorfix = errorfix.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
                quotationPlan = quotationPlan.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }
            if (!string.IsNullOrEmpty(modelSearch.WorkTypeId))
            {
                plans = plans.Where(u => u.WorkTypeId.Equals(modelSearch.WorkTypeId));
                errorfix = errorfix.Where(u => u.WorkTypeId.Equals(modelSearch.WorkTypeId));
            }
            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                plans = plans.Where(u => u.SBUId.Equals(modelSearch.SBUId));
                errorfix = errorfix.Where(u => u.SBUId.Equals(modelSearch.SBUId));
                quotationPlan = quotationPlan.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DesignCode))
            {
                var projectProductIds = (from r in db.ProjectProducts.AsNoTracking()
                                         join x in db.Modules.AsNoTracking() on r.ModuleId equals x.Id
                                         where x.Code.ToLower().Contains(modelSearch.DesignCode.ToLower()) || x.Name.ToLower().Contains(modelSearch.DesignCode.ToLower())
                                         select r.Id).ToList();

                plans = plans.Where(u => projectProductIds.Contains(u.ProjectId));
                errorfix = errorfix.Where(u => projectProductIds.Contains(u.ProjectId));
            }

            if (!string.IsNullOrEmpty(modelSearch.ContractCode))
            {
                var projectProductIds = (db.ProjectProducts.AsNoTracking().Where(u => u.ContractCode.ToLower().Contains(modelSearch.ContractCode.ToLower()) || u.ContractName.ToLower().Contains(modelSearch.ContractCode.ToLower())).Select(r => r.Id)).ToList();
                plans = plans.Where(u => projectProductIds.Contains(u.ProjectProductId));
                errorfix = errorfix.Where(u => projectProductIds.Contains(u.ProjectProductId));
            }

            if (!string.IsNullOrEmpty(modelSearch.EmployeeId))
            {
                plans = plans.Where(u => u.ResponsiblePersion.Equals(modelSearch.EmployeeId));
                errorfix = errorfix.Where(u => u.ResponsiblePersion.Equals(modelSearch.EmployeeId));
                quotationPlan = quotationPlan.Where(u => u.ResponsiblePersion.Equals(modelSearch.EmployeeId));
            }

            if (modelSearch.DoneType > 0)
            {
                switch (modelSearch.DoneType)
                {
                    case 1:
                        plans = plans.Where(u => u.Done == modelSearch.Done);
                        errorfix = errorfix.Where(u => u.Done == modelSearch.Done);
                        quotationPlan = quotationPlan.Where(u => u.Done == modelSearch.Done);
                        break;
                    case 2:
                        plans = plans.Where(u => u.Done > modelSearch.Done);
                        errorfix = errorfix.Where(u => u.Done > modelSearch.Done);
                        quotationPlan = quotationPlan.Where(u => u.Done > modelSearch.Done);
                        break;
                    case 3:
                        plans = plans.Where(u => u.Done >= modelSearch.Done);
                        errorfix = errorfix.Where(u => u.Done >= modelSearch.Done);
                        quotationPlan = quotationPlan.Where(u => u.Done >= modelSearch.Done);
                        break;
                    case 4:
                        plans = plans.Where(u => u.Done < modelSearch.Done);
                        errorfix = errorfix.Where(u => u.Done < modelSearch.Done);
                        quotationPlan = quotationPlan.Where(u => u.Done < modelSearch.Done);
                        break;
                    case 5:
                        plans = plans.Where(u => u.Done <= modelSearch.Done);
                        errorfix = errorfix.Where(u => u.Done <= modelSearch.Done);
                        quotationPlan = quotationPlan.Where(u => u.Done <= modelSearch.Done);
                        break;
                    default:
                        break;
                }
            }

            if (modelSearch.PlanStartDateFrom.HasValue)
            {
                modelSearch.PlanStartDateFrom = DateTimeUtils.ConvertDateFrom(modelSearch.PlanStartDateFrom);

                plans = plans.Where(u => u.PlanStartDate >= modelSearch.PlanStartDateFrom);
                errorfix = errorfix.Where(u => u.PlanStartDate >= modelSearch.PlanStartDateFrom);
                quotationPlan = quotationPlan.Where(u => u.PlanStartDate >= modelSearch.PlanStartDateFrom);
            }

            if (modelSearch.PlanStartDateTo.HasValue)
            {
                modelSearch.PlanStartDateTo = DateTimeUtils.ConvertDateTo(modelSearch.PlanStartDateTo);

                plans = plans.Where(u => u.PlanStartDate <= modelSearch.PlanStartDateTo);
                errorfix = errorfix.Where(u => u.PlanStartDate <= modelSearch.PlanStartDateTo);
                quotationPlan = quotationPlan.Where(u => u.PlanStartDate <= modelSearch.PlanStartDateFrom);
            }

            if (modelSearch.PlanDueDateFrom.HasValue)
            {
                modelSearch.PlanDueDateFrom = DateTimeUtils.ConvertDateFrom(modelSearch.PlanDueDateFrom);

                plans = plans.Where(u => u.PlanDueDate >= modelSearch.PlanDueDateFrom);
                errorfix = errorfix.Where(u => u.PlanDueDate >= modelSearch.PlanDueDateFrom);
                quotationPlan = quotationPlan.Where(u => u.PlanDueDate >= modelSearch.PlanDueDateFrom);
            }

            if (modelSearch.PlanDueDateTo.HasValue)
            {
                modelSearch.PlanDueDateTo = DateTimeUtils.ConvertDateTo(modelSearch.PlanDueDateTo);

                plans = plans.Where(u => u.PlanDueDate <= modelSearch.PlanDueDateTo);
                errorfix = errorfix.Where(u => u.PlanDueDate <= modelSearch.PlanDueDateTo);
                quotationPlan = quotationPlan.Where(u => u.PlanDueDate <= modelSearch.PlanDueDateTo);
            }

            plans = plans.Union(errorfix);
            plans = plans.Union(quotationPlan);

            searchResult.TotalItem = plans.Count();
            List<PlanResultModel> listResult = new List<PlanResultModel>();

            if(modelSearch.PageNumber == -1)
            {
                listResult = plans.OrderBy(r => r.PlanDueDate).ToList();
            }else
            {
                listResult = plans.OrderBy(r => r.PlanDueDate).Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToList();
            }

            if (listResult.Count > 0)
            {
                var projects = db.Projects.AsNoTracking().ToList();
                var projectProducts = db.ProjectProducts.AsNoTracking().ToList();
                var modules = db.Modules.AsNoTracking().ToList();
                ProjectProduct projectProduct = new ProjectProduct();
                Module module = new Module();
                var listHolidayDataBase = db.Holidays.AsNoTracking().ToList();
                foreach (var item in listResult)
                {
                    if(item.Types != 3)
                    {
                        item.ProjectName = projects.Where(r => r.Id.Equals(item.ProjectId)).FirstOrDefault().Name;
                        item.ProjectCode = projects.Where(r => r.Id.Equals(item.ProjectId)).FirstOrDefault().Code;
                        item.ExecutionTime = item.EstimateTimes * item.Duration;

                        item.Duration = (item.PlanStartDate.HasValue && item.PlanDueDate.HasValue) ? (int)(item.PlanDueDate - item.PlanStartDate).Value.TotalDays + 1 : 0;

                        projectProduct = projectProducts.Where(r => r.Id.Equals(item.ProjectProductId)).FirstOrDefault();
                        if (projectProduct != null)
                        {
                            item.ContractCode = projectProduct.ContractCode;
                            item.ContractName = projectProduct.ContractName;

                            module = modules.Where(r => r.Id.Equals(projectProduct.ModuleId)).FirstOrDefault();
                            if (module != null)
                            {
                                item.DesignCode = module.Code;
                                item.DesignName = module.Name;
                            }
                        }
                        else
                        {
                            module = modules.Where(r => r.Id.Equals(item.ModuleId)).FirstOrDefault();
                            if (module != null)
                            {
                                item.DesignCode = module.Code;
                                item.DesignName = module.Name;
                            }
                        }
                    }  
                    if(item.Types == 3)
                    {
                        int a = 0;
                        if (item.PlanDueDate != null && item.PlanStartDate != null)
                        {
                            TimeSpan time = (TimeSpan)(item.PlanDueDate - item.PlanStartDate);
                            int number = time.Days;

                            foreach (var ite in listHolidayDataBase)
                            {
                                if (item.PlanStartDate <= ite.HolidayDate && item.PlanDueDate >= ite.HolidayDate)
                                {
                                    a++;
                                }
                            }
                            item.EstimateTime = number - a + 1;
                        }
                        else
                        {
                            item.EstimateTime = 0;
                        }

                        //var workDiaries = db.WorkDiaries.Where(r => r.EmployeeId.Equals(item.EmployeeId) && r.ObjectId.Equals(item.Id)).ToList();
                        //item.ExecutionTime = workDiaries.Sum(r => r.TotalTime);

                        item.ExecutionTime = item.EstimateTime * item.EstimateTimes;
                    }    
                    
                }
            }

            searchResult.ListResult = listResult;
            return searchResult;
        }

        public List<TaskAssign> GetWorkEmployeeByDate(string employeeId, DateTime date)
        {
            List<TaskAssign> taskAssigns = new List<TaskAssign>();
            var taskOfPlan = (from a in db.Plans.AsNoTracking()
                              join b in db.PlanAssignments.AsNoTracking() on a.Id equals b.PlanId
                              join c in db.Users.AsNoTracking() on b.UserId equals c.Id
                              join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                              join e in db.WorkDiaries.AsNoTracking() on a.Id equals e.ObjectId into ea
                              from ae in ea.DefaultIfEmpty()
                              where ae.EmployeeId.Equals(employeeId) && date == ae.WorkDate
                              select new TaskAssign()
                              {
                                  Id = ae == null ? null : ae.Id,
                                  ProjectId = a.ProjectId,
                                  TaskName = a.Name,
                                  NumberTime = ae == null ? 0 : ae.TotalTime,
                                  ObjectId = a.Id,
                                  ObjectType = 1,
                                  Note = ae == null ? null : ae.Note,
                                  EmpoyeeId = employeeId,
                                  TimeEnd = ae == null ? null : ae.EndTime,
                                  TimeStart = ae == null ? null : ae.StartTime
                              }
                            ).ToList();
            taskAssigns.AddRange(taskOfPlan);
            //error assign of plan
            var errorFix = (from a in db.ErrorFixs.AsNoTracking()
                            join b in db.Employees.AsNoTracking() on a.EmployeeFixId equals b.Id
                            join j in db.Errors.AsNoTracking() on a.ErrorId equals j.Id
                            join e in db.WorkDiaries.AsNoTracking() on a.Id equals e.ObjectId into ea
                            from ae in ea.DefaultIfEmpty()
                            where a.EmployeeFixId.Equals(employeeId) && j.Status > 3 && date == ae.WorkDate
                            select new TaskAssign()
                            {
                                Id = ae == null ? null : ae.Id,
                                ProjectId = db.Errors.FirstOrDefault(ax => ax.Id.Equals(a.ErrorId)).ProjectId,
                                TaskName = a.Solution,
                                NumberTime = ae == null ? 0 : ae.TotalTime,
                                ObjectId = a.Id,
                                ObjectType = 0,
                                Note = ae == null ? null : ae.Note,
                                EmpoyeeId = employeeId,
                                TimeEnd = ae == null ? null : ae.EndTime,
                                TimeStart = ae == null ? null : ae.StartTime
                            }
                            ).ToList();
            taskAssigns.AddRange(errorFix);


            List<TaskAssign> taskAssignsDefault = new List<TaskAssign>();
            //task assign of plan
            var taskOfPlanDefault = (from a in db.Plans.AsNoTracking()
                                     join b in db.PlanAssignments.AsNoTracking() on a.Id equals b.PlanId
                                     join c in db.Users.AsNoTracking() on b.UserId equals c.Id
                                     join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                                     where d.Id.Equals(employeeId) && a.ActualStartDate <= date && (a.ActualEndDate != null && a.ActualEndDate >= date || a.ActualEndDate == null)
                                     select new TaskAssign()
                                     {
                                         EmpoyeeId = employeeId,
                                         ProjectId = a.ProjectId,
                                         TaskName = a.Name,
                                         NumberTime = 0,
                                         ObjectId = a.Id,
                                         ObjectType = 1,
                                     }
                            ).ToList();
            taskAssignsDefault.AddRange(taskOfPlanDefault);
            //error assign of plan
            var errorFixDefault = (from a in db.ErrorFixs.AsNoTracking()
                                   join b in db.Employees.AsNoTracking() on a.EmployeeFixId equals b.Id
                                   join j in db.Errors.AsNoTracking() on a.ErrorId equals j.Id
                                   where a.EmployeeFixId.Equals(employeeId) && j.Status > 3 && a.ActualStartDate <= date && (a.FinishDate != null && a.FinishDate >= date || a.FinishDate == null)
                                   select new TaskAssign()
                                   {
                                       EmpoyeeId = employeeId,
                                       ProjectId = db.Errors.FirstOrDefault(ax => ax.Id.Equals(a.ErrorId)).ProjectId,
                                       TaskName = a.Solution,
                                       NumberTime = 0,
                                       ObjectId = a.Id,
                                       ObjectType = 0,
                                   }
                            ).ToList();
            taskAssignsDefault.AddRange(errorFixDefault);
            foreach (var item in taskAssignsDefault)
            {
                foreach (var item1 in taskAssigns)
                {
                    if (item.ObjectId.Equals(item1.ObjectId) && item.EmpoyeeId.Equals(item1.EmpoyeeId))
                    {
                        item.Id = item1.Id;
                        item.Note = item1.Note;
                        item.NumberTime = item1.NumberTime;
                        item.TimeStart = item1.TimeStart;
                        item.TimeEnd = item1.TimeEnd;
                    }
                }
            }
            return taskAssignsDefault;
        }

        public void UpdateWorkTime(TaskAssignModel taskAssignModel, string createBy)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var employee = db.Employees.FirstOrDefault(a => a.Id.Equals(taskAssignModel.employeeId));
                    var sbuId = db.SBUs.FirstOrDefault(a => a.Id.Equals(db.Departments.FirstOrDefault(b => b.Id.Equals(employee.DepartmentId)).SBUId)).Id;

                    foreach (var item in taskAssignModel.TaskAssign)
                    {
                        var work = db.WorkDiaries.FirstOrDefault(a => a.Id.Equals(item.Id));
                        if (work != null && work.WorkDate == item.WorkDate)
                        {
                            if (item.NumberTime == 0)
                            {
                                db.WorkDiaries.Remove(work);
                            }
                            else
                            {
                                work.TotalTime = item.NumberTime;
                                work.Note = item.Note;
                                work.StartTime = item.TimeStart;
                                work.EndTime = item.TimeEnd;
                            }
                        }
                        else if ((work == null && item.NumberTime > 0) || (work != null && work.WorkDate != item.WorkDate))
                        {
                            WorkDiary newWorkSkill = new WorkDiary()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectId = item.ProjectId,
                                Name = item.TaskName,
                                SBUId = sbuId,
                                DepartmentId = employee.DepartmentId,
                                ObjectId = item.ObjectId,
                                ObjectType = item.ObjectType,
                                WorkDate = item.WorkDate,
                                TotalTime = item.NumberTime,
                                Address = Constants.Manufacture_TPA,
                                Note = item.Note,
                                EmployeeId = employee.Id,
                                StartTime = item.TimeStart,
                                EndTime = item.TimeEnd,
                                CreateBy = createBy,
                                UpdateBy = createBy,
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                            };
                            db.WorkDiaries.Add(newWorkSkill);
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(taskAssignModel, ex);
                }

            }
        }

        public PlanWorkEmpoyeeModel GetEmployeeInfor(string employeeId, int month, int year)
        {
            PlanWorkEmpoyeeModel planWorkEmpoyee = new PlanWorkEmpoyeeModel();
            var employee = db.Employees.FirstOrDefault(e => e.Id.Equals(employeeId));
            //get date of month
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            Monthly monthly = new Monthly();
            monthly.TimeRange = firstDayOfMonth.ToString("dd/MM/yyyy") + " - " + lastDayOfMonth.ToString("dd/MM/yyyy");

            DateTime dateLogWork = DateTime.Now;
            var listDate = GetDates(year, month);
            //check task assigment
            List<WorkDiary> workDiariesDefault = new List<WorkDiary>();
            List<WorkDiary> workDiariesSetting = new List<WorkDiary>();
            var PlanInfo = (from a in db.WorkDiaries.AsNoTracking()
                            join b in db.Plans.AsNoTracking() on a.ObjectId equals b.Id
                            where a.EmployeeId.Equals(employeeId)
                            select a).ToList();
            workDiariesSetting.AddRange(PlanInfo);
            var ErrorInfo = (from a in db.WorkDiaries.AsNoTracking()
                             join b in db.ErrorFixs.AsNoTracking() on a.ObjectId equals b.Id
                             where a.EmployeeId.Equals(employeeId)
                             select a).ToList();
            workDiariesSetting.AddRange(ErrorInfo);
            foreach (var item in workDiariesSetting)
            {
                var taskInfo = workDiariesDefault.FirstOrDefault(a => a.ProjectId.Equals(item.ProjectId) && a.ObjectId.Equals(item.ObjectId) && a.EmployeeId.Equals(employeeId));
                if (taskInfo == null)
                {
                    workDiariesDefault.Add(item);
                }
            }
            foreach (var item in workDiariesDefault)
            {
                TaskWorkTimeModel workTimeModel = new TaskWorkTimeModel();
                workTimeModel.TaskName = item.Name;
                workTimeModel.TaskWorkTime = GetDates(year, month);
                foreach (var date in workTimeModel.TaskWorkTime)
                {
                    foreach (var item1 in workDiariesSetting)
                    {
                        if (date.DateTime == item1.WorkDate && item1.ObjectId.Equals(item.ObjectId) && item1.ProjectId.Equals(item.ProjectId) && item1.EmployeeId.Equals(employeeId))
                        {
                            date.LogTime = date.LogTime + item1.TotalTime;
                        }
                    }
                }
                planWorkEmpoyee.ListWorkTime.Add(workTimeModel);
            }
            foreach (var item in planWorkEmpoyee.ListWorkTime.ToList())
            {
                var count = 0;
                foreach (var item1 in item.TaskWorkTime)
                {
                    if (item1.LogTime == 0)
                    {
                        count++;
                    }
                }
                if (item.TaskWorkTime.Count == count)
                {
                    planWorkEmpoyee.ListWorkTime.Remove(item);
                }
            }
            //task assign of plan
            var taskOfPlan = (from a in db.Plans.AsNoTracking()
                              join b in db.PlanAssignments.AsNoTracking() on a.Id equals b.PlanId
                              join c in db.Users.AsNoTracking() on b.UserId equals c.Id
                              join d in db.Employees.AsNoTracking() on c.EmployeeId equals d.Id
                              where d.Id.Equals(employee.Id) && a.ActualStartDate <= firstDayOfMonth && (a.ActualEndDate != null && a.ActualEndDate >= firstDayOfMonth || a.ActualEndDate == null)
                              select new TaskAssign()
                              {
                                  ProjectId = a.ProjectId,
                                  TaskName = a.Name,
                                  NumberTime = 0,
                                  ObjectId = a.Id,
                                  ObjectType = 1,
                              }
                            ).ToList();
            planWorkEmpoyee.taskAssigns.AddRange(taskOfPlan);
            //error assign of plan
            var errorFix = (from a in db.ErrorFixs.AsNoTracking()
                            join b in db.Employees.AsNoTracking() on a.EmployeeFixId equals b.Id
                            join j in db.Errors.AsNoTracking() on a.ErrorId equals j.Id
                            where a.EmployeeFixId.Equals(employee.Id) && j.Status > 3 && a.ActualStartDate <= firstDayOfMonth && (a.FinishDate != null && a.FinishDate >= firstDayOfMonth || a.FinishDate == null)
                            select new TaskAssign()
                            {
                                ProjectId = db.Errors.FirstOrDefault(ax => ax.Id.Equals(a.ErrorId)).ProjectId,
                                TaskName = a.Solution,
                                NumberTime = 0,
                                ObjectId = a.Id,
                                ObjectType = 0,
                            }
                            ).ToList();
            planWorkEmpoyee.taskAssigns.AddRange(errorFix);
            //
            monthly.DataMonthlys = listDate;
            planWorkEmpoyee.EmployeeName = employee.Name;
            planWorkEmpoyee.EmployeeId = employee.Id;
            planWorkEmpoyee.NowDate = DateTime.Now.ToString("dd/MM/yyyy");
            planWorkEmpoyee.Monthly = monthly;
            return planWorkEmpoyee;
        }
        public List<DataMonthlys> GetDates(int year, int month)
        {
            var list = Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list

            List<DataMonthlys> listDateOfMonth = new List<DataMonthlys>();
            var count = 1;
            foreach (var date in list)
            {
                DataMonthlys dataMonthlys = new DataMonthlys();
                dataMonthlys.Date = count;
                dataMonthlys.DateTime = date;
                dataMonthlys.DayOfWeek = date.GetDayOfWWeek().ToString();
                count++;
                listDateOfMonth.Add(dataMonthlys);
            }
            return listDateOfMonth;
        }

        public PlanResultModel GetPlanView(string id)
        {
            PlanResultModel resultInfo = new PlanResultModel();
            resultInfo = (from a in db.Plans.AsNoTracking()
                          join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id

                          join e in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals e.Id
                          join m in db.Modules.AsNoTracking() on e.ModuleId equals m.Id into em
                          from emn in em.DefaultIfEmpty()
                          join p in db.Products.AsNoTracking() on e.ProductId equals p.Id into ep
                          from epn in ep.DefaultIfEmpty()
                          join x in db.PlanAssignments.AsNoTracking() on a.Id equals x.PlanId into ax
                          from axn in ax.DefaultIfEmpty()
                          where axn.IsMain == true && a.Id == id
                          join c in db.Users.AsNoTracking() on axn.UserId equals c.Id into ac
                          from ca in ac.DefaultIfEmpty()
                          join d in db.Employees.AsNoTracking() on ca.EmployeeId equals d.Id into ad
                          from da in ad.DefaultIfEmpty()
                          join f in db.Departments.AsNoTracking() on da.DepartmentId equals f.Id into fd
                          from df in fd.DefaultIfEmpty()
                          join cc in db.SBUs.AsNoTracking() on df.SBUId equals cc.Id
                          select new PlanResultModel()
                          {
                              Id = a.Id,
                              ProjectId = b.Id,
                              ProjectName = b.Name,
                              ProjectCode = b.Code,
                              DepartmentId = da.DepartmentId,
                              DepartmentName = df.Name,
                              SBUName = cc.Name,
                              Status = a.Status,
                              Index = a.Index,
                              ProjectProductId = a.ProjectProductId,
                              ResponsiblePersionName = da != null ? da.Name : string.Empty,
                              Description = a.Description,
                              Types = 1,
                              TaskName = a.Name,
                              DesignFinishDate = e.DesignFinishDate,
                              MakeFinishDate = e.MakeFinishDate,
                              TransferDate = e.TransferDate,
                          }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
            }

            return resultInfo;
        }
        public PlanResultModel GetQuotationPlanView(string id)
        {
            PlanResultModel resultInfo = new PlanResultModel();
            // Lấy dữ liệu của bảng plan
            resultInfo = db.QuotationPlans.AsNoTracking().Where(u => u.Id.Equals(id)).Select(p => new PlanResultModel()
            {
                Id = p.Id,
                Done = p.DoneRatio,
                PlanStartDate = p.PlanStartDate,
                PlanDueDate = p.PlanDueDate,
                ActualStartDate = p.ActualStartDate,
                ActualEndDate = p.ActualEndDate,
                Description = p.Descripton,
                Name = p.Name,
                Types = 3,
            }).FirstOrDefault();


            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.QuotationPlan);
            }


            var planEmployee = (from a in db.QuotationPlans.AsNoTracking()
                                join c in db.Employees.AsNoTracking() on a.EmployeeId equals c.Id
                                where a.Id.Equals(id)
                                select new { c.Name }).FirstOrDefault();
            if (planEmployee != null)
            {
                resultInfo.ResponsiblePersion = planEmployee.Name;
            }

            return resultInfo;
        }
        public PlanResultModel GetErrorFixView(string id)
        {
            PlanResultModel resultInfo = new PlanResultModel();

            resultInfo = (from r in db.ErrorFixs.AsNoTracking()
                          where r.Id == id
                          join j in db.Errors.AsNoTracking() on r.ErrorId equals j.Id
                          join t in db.Projects.AsNoTracking() on j.ProjectId equals t.Id into jt
                          from jtn in jt.DefaultIfEmpty()
                          join q in db.ProjectProducts.AsNoTracking() on j.ProjectId equals q.Id into jq
                          from jqn in jq.DefaultIfEmpty()
                          join p in db.Modules.AsNoTracking() on jqn.ProductId equals p.Id into qp
                          from qpn in qp.DefaultIfEmpty()
                          join e in db.Employees.AsNoTracking() on r.EmployeeFixId equals e.Id
                          join d in db.Departments.AsNoTracking() on r.DepartmentId equals d.Id
                          select new PlanResultModel()
                          {
                              Id = r.Id,
                              ProjectId = j.ProjectId,
                              ProjectName = jtn != null ? jtn.Name : string.Empty,
                              ProjectCode = jtn != null ? jtn.Code : string.Empty,
                              DepartmentId = e.DepartmentId,
                              DepartmentName = d.Name,
                              SBUName = d.Name,
                              Status = r.Status,
                              ProjectProductId = jqn != null ? jqn.Id : string.Empty,
                              ResponsiblePersionName = e.Name,
                              Description = j.Description,
                              Types = 2,
                              TaskName = r.Solution,
                              DesignFinishDate = jqn.DesignFinishDate,
                              MakeFinishDate = jqn.MakeFinishDate,
                              TransferDate = jqn.TransferDate,
                          }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ErrorFix);
            }

            return resultInfo;
        }

        public PlanInfoModel GetPlanInfo(string id)
        {
            var resultInfo = (from r in db.Plans.AsNoTracking()
                              where r.Id.Equals(id)
                              join a in db.Projects.AsNoTracking() on r.ProjectId equals a.Id
                              select new PlanInfoModel()
                              {
                                  Id = r.Id,
                                  DoneRatio = r.DoneRatio,
                                  PlanStartDate = r.PlanStartDate,
                                  PlanDueDate = r.PlanDueDate,
                                  ActualStartDate = r.ActualStartDate,
                                  ActualEndDate = r.ActualEndDate,
                                  ProjectName = a.Name,
                                  Index = r.Index,
                                  Description = r.Description,
                                  Name = r.Name,
                              }).FirstOrDefault();

            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
            }

            var mainPIC = db.PlanAssignments.AsNoTracking().Where(r => r.PlanId.Equals(resultInfo.Id) && r.IsMain).FirstOrDefault();
            if (mainPIC != null)
            {
                resultInfo.PersonInCharge = (from em in db.Employees.AsNoTracking()
                                             where em.Id.Equals(mainPIC.EmployeeId)
                                             select em.Name).FirstOrDefault();
            }

            return resultInfo;
        }
        public PlanInfoModel GetQuotationPlanInfo(string id)
        {
            PlanInfoModel resultInfo = new PlanInfoModel();
            // Lấy dữ liệu của bảng plan
            resultInfo = db.QuotationPlans.AsNoTracking().Where(u => u.Id.Equals(id)).Select(p => new PlanInfoModel()
            {
                Id = p.Id,
                DoneRatio = p.DoneRatio,
                ActualStartDate = p.ActualStartDate,
                ActualEndDate = p.ActualEndDate,
                Description = p.Descripton,
                Name = p.Name,

            }).FirstOrDefault();


            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.QuotationPlan);
            }


            var planEmployee = (from a in db.QuotationPlans.AsNoTracking()
                                join c in db.Employees.AsNoTracking() on a.EmployeeId equals c.Id
                                where a.Id.Equals(id)
                                select new { c.Name }).FirstOrDefault();
            if (planEmployee != null)
            {
                resultInfo.ResponsiblePersion = planEmployee.Name;
            }

            return resultInfo;
        }

        public PlanInfoModel GetErrorFixsInfo(string id)
        {
            var resultInfo = (from r in db.ErrorFixs.AsNoTracking()
                              where r.Id.Equals(id)
                              join a in db.Errors.AsNoTracking() on r.ErrorId equals a.Id
                              join b in db.Projects.AsNoTracking() on a.ProjectId equals b.Id
                              join c in db.Employees.AsNoTracking() on r.EmployeeFixId equals c.Id
                              select new PlanInfoModel()
                              {
                                  Id = r.Id,
                                  DoneRatio = r.Done,
                                  PlanStartDate = r.DateFrom,
                                  PlanDueDate = r.DateTo,
                                  ActualStartDate = r.ActualStartDate,
                                  ActualEndDate = r.FinishDate,
                                  ProjectName = b.Name,
                                  Description = r.Solution,
                                  PersonInCharge = c.Name,
                              }).FirstOrDefault();

            return resultInfo;
        }

        public void UpdateQuotationPlan(PlanInfoModel model, string userLoginId)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var newQuotationPlan = db.QuotationPlans.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    if (newQuotationPlan == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.QuotationPlan);
                    }
                    if (model.ActualStartDate > model.ActualEndDate)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0012, TextResourceKey.QuotationPlan);
                    }

                    DateTime date = DateTime.Now;
                    newQuotationPlan.DoneRatio = model.DoneRatio;
                    newQuotationPlan.Name = model.Name;
                    newQuotationPlan.Descripton = model.Description;
                    newQuotationPlan.ActualStartDate = model.ActualStartDate;
                    newQuotationPlan.ActualEndDate = model.ActualEndDate;

                    if (model.ActualEndDate.HasValue)
                    {
                        newQuotationPlan.Status = 3;
                    }
                    newQuotationPlan.UpdateBy = userLoginId;
                    newQuotationPlan.UpdateDate = DateTime.Now;

                    //Cập nhật trạng thái bước báo giá
                    var quotationStep = db.StepInQuotations.FirstOrDefault(a => a.Id.Equals(newQuotationPlan.StepInQuotationId));
                    var listPlanInStep = db.QuotationPlans.Where(a => a.StepInQuotationId.Equals(newQuotationPlan.StepInQuotationId)).ToList();
                    int countPlanInStep = listPlanInStep.Count();
                    int count = 0;
                    bool checkLate = false;
                    foreach (var a in listPlanInStep)
                    {
                        if (a.Status == 3)
                        {
                            count = count + 1;
                        }
                        if (a.Status == 4)
                        {
                            checkLate = true;
                        }
                    }
                    if (count == countPlanInStep)
                    {
                        quotationStep.Status = 3;
                    }
                    if (checkLate)
                    {
                        quotationStep.Status = 4;
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

        public ScheduleEntity UpdatePlan(PlanInfoModel model, string userLoginId)
        {
            var plan = db.Plans.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

            if (plan == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
            }

            if (db.PlanAssignments.FirstOrDefault(a => a.PlanId.Equals(model.Id) && a.IsMain && a.UserId.Equals(userLoginId)) == null)
            {
                throw NTSException.CreateInstance("Bạn không có quyền thao tác. Liên hệ với người phụ trách chính để xác nhận!");
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    bool isChangeDone = plan.DoneRatio != model.DoneRatio;
                    plan.DoneRatio = model.DoneRatio;

                    // Trường hợp công việc đang Stop thì không update lại trạng thái
                    if (plan.Status != (int)Constants.ScheduleStatus.Stop && plan.Status != (int)Constants.ScheduleStatus.Cancel)
                    {
                        if (model.DoneRatio > 0)
                        {
                            plan.Status = model.DoneRatio == 100 ? (int)Constants.ScheduleStatus.Closed : (int)Constants.ScheduleStatus.Ongoing;
                        }
                    }

                    plan.ActualStartDate = model.ActualStartDate;
                    plan.ActualEndDate = model.ActualEndDate;

                    db.SaveChanges();

                    if (isChangeDone)
                    {
                        this.ReCalculateDoneRatio(plan.ParentId);
                    }

                    trans.Commit();

                    return new ScheduleEntity()
                    {
                        Id = plan.Id,
                        ProjectProductId = plan.ProjectProductId,
                        StageId = plan.StageId,
                        ParentId = plan.ParentId,
                        PlanName = plan.Name,
                        IsPlan = plan.IsPlan,
                        Status = plan.Status,
                        Weight = plan.Weight,
                        Type = plan.Type,
                        DoneRatio = plan.DoneRatio,
                    };
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }
            }
        }

        private void ReCalculateDoneRatio(string stageId)
        {
            // Lấy thông tin Stage
            var stage = db.Plans.FirstOrDefault(a => a.Id.Equals(stageId));

            if (stage != null)
            {
                var plans = db.Plans.AsNoTracking().Where(r => r.ParentId.Equals(stage.Id)).ToList();
                int numOfPlan = plans.Count();
                int numOfOpen = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count();
                int numOfOngoing = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count();
                int numOfClosed = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count();
                int numOfStop = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count();
                int numOfCancel = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count();

                // Update thông tin Stage
                stage.DoneRatio = plans.Where(r => r.Status != 4 & r.Status != 5).Count() == 0 ? 0 : plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum() / plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum();

                // Trường hợp chưa có công việc thì là Open (phòng trường hợp xóa công việc)
                if (numOfPlan == 0)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp có bất kỳ công việc nào đang Ongoing thì là Ongoing
                else if (numOfOngoing > 0)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Ongoing;
                }
                // Trường hợp tất cả công việc là Open
                else if (numOfPlan == numOfOpen)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp tất cả công việc là Close
                else if (numOfPlan == numOfClosed)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Closed;
                }
                // Trường hợp tất cả công việc là Stop
                else if (numOfPlan == numOfStop)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Stop;
                }
                // Trường hợp tất cả công việc là Cancel
                else if (numOfPlan == numOfCancel)
                {
                    stage.Status = (int)Constants.ScheduleStatus.Cancel;
                }
                else
                {
                    if (numOfOpen > 0)
                    {
                        if (numOfClosed > 0)
                        {
                            stage.Status = (int)Constants.ScheduleStatus.Ongoing;
                        }
                        else
                        {
                            stage.Status = (int)Constants.ScheduleStatus.Open;
                        }

                    }
                    else
                    {
                        if (numOfClosed > 0)
                        {
                            stage.Status = (int)Constants.ScheduleStatus.Closed;
                        }
                        else
                        {
                            if (numOfStop > 0)
                            {
                                stage.Status = (int)Constants.ScheduleStatus.Stop;
                            }
                        }
                    }
                }

                db.SaveChanges();

                ReCalculateDoneRatioProjectProduct(stage.ProjectProductId);
            }
        }

        private void ReCalculateDoneRatioProjectProduct(string projectProductId)
        {
            // Thông tin của sản phẩm cha cần update
            var product = db.ProjectProducts.FirstOrDefault(a => a.Id.Equals(projectProductId));

            // Danh sách tất cả công đoạn của sản phẩm
            var plans = db.Plans.AsNoTracking().Where(r => r.ProjectProductId.Equals(projectProductId) && r.IsPlan == false).ToList();

            if (product != null)
            {
                int numOfPlan;
                int numOfOpen;
                int numOfOngoing;
                int numOfClosed;
                int numOfStop;
                int numOfCancel;

                // Trường hợp có công đoạn
                if (plans.Count > 0)
                {
                    // Danh sách tất cả sản phẩm con
                    var products = db.ProjectProducts.Where(a => a.ParentId.Equals(projectProductId)).ToList();

                    product.DoneRatio = (plans.Where(r => r.Status != 4 & r.Status != 5).Count() == 0 && products.Where(r => r.Status != 4 & r.Status != 5).Count() == 0)
                        ? 0
                        : (plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum() + products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum()) / (plans.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum() + products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum());

                    numOfPlan = plans.Count() + products.Count();
                    numOfOpen = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count();
                    numOfOngoing = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count();
                    numOfClosed = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count();
                    numOfStop = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count();
                    numOfCancel = plans.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count() + products.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count();

                }
                else
                {
                    // Danh sách tất cả sản phẩm con
                    var products = db.ProjectProducts.Where(a => a.ParentId.Equals(projectProductId)).ToList();

                    product.DoneRatio = products.Where(r => r.Status != 4 & r.Status != 5).Count() == 0 ? 0 : products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.DoneRatio * r.Weight).Sum() / products.Where(r => r.Status != 4 & r.Status != 5).Select(r => r.Weight).Sum();

                    numOfPlan = products.Count();
                    numOfOpen = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Open).Count();
                    numOfOngoing = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Ongoing).Count();
                    numOfClosed = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Closed).Count();
                    numOfStop = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Stop).Count();
                    numOfCancel = products.Where(r => r.Status == (int)Constants.ScheduleStatus.Cancel).Count();
                }

                // Trường hợp chưa có công việc thì là Open (phòng trường hợp xóa công việc)
                if (numOfPlan == 0)
                {
                    product.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp có bất kỳ công việc nào đang Ongoing thì là Ongoing
                else if (numOfOngoing > 0)
                {
                    product.Status = (int)Constants.ScheduleStatus.Ongoing;
                }
                // Trường hợp tất cả công việc là Open
                else if (numOfPlan == numOfOpen)
                {
                    product.Status = (int)Constants.ScheduleStatus.Open;
                }
                // Trường hợp tất cả công việc là Close
                else if (numOfPlan == numOfClosed)
                {
                    product.Status = (int)Constants.ScheduleStatus.Closed;
                }
                // Trường hợp tất cả công việc là Stop
                else if (numOfPlan == numOfStop)
                {
                    product.Status = (int)Constants.ScheduleStatus.Stop;
                }
                // Trường hợp tất cả công việc là Cancel
                else if (numOfPlan == numOfCancel)
                {
                    product.Status = (int)Constants.ScheduleStatus.Cancel;
                }
                else
                {
                    if (numOfOpen > 0)
                    {
                        if (numOfClosed > 0)
                        {
                            product.Status = (int)Constants.ScheduleStatus.Ongoing;
                        }
                        else
                        {
                            product.Status = (int)Constants.ScheduleStatus.Open;
                        }

                    }
                    else
                    {
                        if (numOfClosed > 0)
                        {
                            product.Status = (int)Constants.ScheduleStatus.Closed;
                        }
                        else
                        {
                            if (numOfStop > 0)
                            {
                                product.Status = (int)Constants.ScheduleStatus.Stop;
                            }
                        }
                    }
                }

                db.SaveChanges();

                if (!string.IsNullOrEmpty(product.ParentId))
                {
                    this.ReCalculateDoneRatioProjectProduct(product.ParentId);
                }
            }
        }


        public void UpdateErrorFix(PlanInfoModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var errorFix = db.ErrorFixs.Where(u => u.Id.Equals(model.Id)).FirstOrDefault();

                    if (errorFix == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ErrorFix);
                    }

                    errorFix.Done = model.DoneRatio;
                    if (model.DoneRatio == 100)
                    {
                        errorFix.Status = 2;
                    }
                    else
                    {
                        errorFix.Status = 1;
                    }
                    errorFix.FinishDate = model.ActualEndDate;
                    errorFix.ActualStartDate = model.ActualStartDate;

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

        public string ExportExcel(PlanSearchModel model)
        {
            model.PageNumber = -1;

            SearchResultModel<PlanResultModel> searchResult = this.SearchPlans(model);

            if (searchResult.ListResult.Count == 0)
            {
                throw new Exception("Không có dữ liệu!");
            }

            ExcelEngine excelEngine = new ExcelEngine();

            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Open(HostingEnvironment.MapPath("~/Template/Plan.xlsx"));

            IWorksheet sheet = workbook.Worksheets[0];

            var total = searchResult.ListResult.Count;

            IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            iRangeData.Text = iRangeData.Text.Replace("<Data>", string.Empty);

            var listExport = searchResult.ListResult.Select((a, i) => new
            {
                Index = i + 1,
                a.ProjectCode,
                a.ProjectName,
                a.ContractCode,
                a.ContractName,
                a.DesignCode,
                a.DesignName,
                a.Done,
                a.TaskName,
                a.ResponsiblePersionName,
                a.ExecutionTime,
                a.PlanStartDate,
                a.PlanDueDate,
                a.ActualStartDate,
                a.ActualEndDate,
            });

            if (listExport.Count() > 1)
            {
                sheet.InsertRow(iRangeData.Row + 1, listExport.Count() - 1, ExcelInsertOptions.FormatAsBefore);
            }
            sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 10].Borders.Color = ExcelKnownColors.Black;
            sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 10].CellStyle.WrapText = true;

            string pathExport = HttpContext.Current.Server.MapPath("~/Template/" + Constants.FolderExport + "Danh sách kế hoạch" + ".xls");
            workbook.SaveAs(pathExport);
            workbook.Close();
            excelEngine.Dispose();

            //Đường dẫn file lưu trong web client
            string resultPathClient = "Template/" + Constants.FolderExport + "Danh sách kế hoạch" + ".xls";

            return resultPathClient;

        }

        public List<ScheduleProjectResultModel> CreatePlanStage(string userId, ProduceStageModel model)
        {
            List<ScheduleProjectResultModel> listResult = new List<ScheduleProjectResultModel>();
            List<Plan> listPlan = new List<Plan>();
            if (model.ListIdSelect.Count > 0)
            {
                foreach (var item in model.ListIdSelect)
                {
                    var stage = db.Stages.FirstOrDefault(t => t.Id.Equals(item));
                    if (stage != null)
                    {
                        Plan plan = new Plan()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ProjectId = model.ProjectId,
                            StageId = item,
                            ProjectProductId = model.Id,
                            TrackerType = 1,
                            Weight = 1,
                            Name = stage.Name,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        listResult.Add(new ScheduleProjectResultModel
                        {
                            Id = plan.Id,
                            ProjectId = plan.ProjectId,
                            ProjectProductId = plan.ProjectProductId,
                            ParentId = plan.ProjectProductId,
                            StageName = plan.Name,
                            BackgroundColor = stage.Color,
                            StageId = plan.StageId,
                            ContractStartDate = plan.ContractStartDate,
                            ContractDueDate = plan.ContractDueDate,
                            PlanStartDate = plan.PlanStartDate,
                            PlanDueDate = plan.PlanDueDate,
                            DoneRatio = plan.DoneRatio,
                            Color = stage.Color,
                            Weight = plan.Weight,
                            IsPlan = plan.IsPlan,
                            EstimateTime = (plan.EstimateTime == 0 && plan.IsPlan) ? 8 : (plan.EstimateTime > 0 && plan.IsPlan) ? plan.EstimateTime : 0,
                            Status = plan.Status,
                            SupplierId = plan.SupplierId,
                            Type = plan.Type,
                            Index = stage.index,
                            CreateDate = plan.CreateDate
                        });

                        listPlan.Add(plan);
                    }
                }
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Plans.AddRange(listPlan);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new NTSLogException(model, ex);
                }

            }

            return listResult;
        }

        public void CreatePlanWork(string userId, PlanWorkModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    Plan plan = new Plan()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParentId = model.ParentId,
                        TrackerType = 2,
                        Name = model.Name,
                        ContractStartDate = model.ContractStartDate,
                        ContractDueDate = model.ContractDueDate,
                        PlanStartDate = model.PlanStartDate,
                        PlanDueDate = model.PlanDueDate,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };

                    db.Plans.Add(plan);
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

        public object SearchWorkingTime(WorkingTimeSearchModel searchModel)
        {
            SearchResultModel<WorkingTime> searchResult = new SearchResultModel<WorkingTime>();

            var employeeQuery = (from a in db.Employees.AsNoTracking()
                                 join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id
                                 join e in db.WorkTypes.AsNoTracking() on a.WorkTypeId equals e.Id
                                 where a.Status == Constants.Employee_Status_Use
                                 orderby a.Code, e.Name
                                 select new
                                 {
                                     a.Id,
                                     a.Name,
                                     a.Code,
                                     DepartmentId = d.Id,
                                     WorkType = e.Name,
                                     WorkTypeId = e.Id,
                                     d.SBUId
                                 }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.DepartmentId))
            {
                employeeQuery = employeeQuery.Where(r => r.DepartmentId.Equals(searchModel.DepartmentId));
            }

            if (!string.IsNullOrEmpty(searchModel.SBUId))
            {
                employeeQuery = employeeQuery.Where(r => r.SBUId.Equals(searchModel.SBUId));
            }

            if (!string.IsNullOrEmpty(searchModel.NameCode))
            {
                employeeQuery = employeeQuery.Where(r => r.Name.ToLower().Contains(searchModel.NameCode.ToLower()) || r.Code.ToLower().Contains(searchModel.NameCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchModel.WorkType))
            {
                employeeQuery = employeeQuery.Where(r => searchModel.WorkType.Equals(r.WorkTypeId));
            }

            var employees = employeeQuery.ToList();
            var employeeIds = employees.Select(r => r.Id).ToList();

            searchModel.DateStart = DateTimeUtils.ConvertDateFrom(searchModel.DateStart);
            searchModel.DateEnd = DateTimeUtils.ConvertDateTo(searchModel.DateEnd);

            // Lấy danh sách công việc trong kế hoạch của vấn đề tồn đọng
            var errorFixs = (from c in db.ErrorFixs.AsNoTracking()
                             where employeeIds.Contains(c.EmployeeFixId) && c.Status != Constants.ErrorFix_Status_Finish &&
                             ((c.DateFrom.Value > searchModel.DateStart && c.DateTo.Value < searchModel.DateEnd) // Trường hợp công việc nằm trọn vẹn trong tháng
                             || (c.DateFrom.Value < searchModel.DateStart && c.DateTo.Value > searchModel.DateEnd) // Trường hợp công việc nằm ngoài tháng
                             || (c.DateFrom.Value > searchModel.DateStart && c.DateFrom.Value < searchModel.DateEnd) // Trường hợp công việc bắt đầu trong tháng, kết thúc tháng sau
                             || (c.DateTo.Value > searchModel.DateStart && c.DateTo.Value < searchModel.DateEnd) // Trường hợp công việc bắt đầu trước, kết thúc trong tháng
                             )
                             select new PlanErrorFixByEmployeeModel
                             {
                                 PlanId = c.Id,
                                 EmployeeId = c.EmployeeFixId,
                                 EstimateTime = c.EstimateTime.HasValue ? c.EstimateTime.Value : 0,
                                 PlanStartDate = c.DateFrom < searchModel.DateStart ? searchModel.DateStart : c.DateFrom,
                                 PlanDueDate = c.DateTo > searchModel.DateTo ? searchModel.DateTo : c.DateTo,
                                 Type = (int)Constants.PLanType.Error
                             }).ToList();

            // Lấy danh sách công việc trong kế hoạch dự án
            var plans = (from b in db.PlanAssignments.AsNoTracking()
                         join c in db.Plans.AsNoTracking() on b.PlanId equals c.Id
                         where employeeIds.Contains(b.EmployeeId) && c.Status != (int)Constants.ScheduleStatus.Closed && c.Status != (int)Constants.ScheduleStatus.Stop && c.Status != (int)Constants.ScheduleStatus.Cancel &&
                            ((c.PlanStartDate.Value >= searchModel.DateStart && c.PlanDueDate.Value <= searchModel.DateEnd) // Trường hợp công việc nằm trọn vẹn trong tháng
                             || (c.PlanStartDate.Value <= searchModel.DateStart && c.PlanDueDate.Value >= searchModel.DateEnd) // Trường hợp công việc nằm ngoài tháng
                             || (c.PlanStartDate.Value >= searchModel.DateStart && c.PlanStartDate.Value <= searchModel.DateEnd) // Trường hợp công việc bắt đầu trong tháng, kết thúc tháng sau
                             || (c.PlanDueDate.Value >= searchModel.DateStart && c.PlanDueDate.Value <= searchModel.DateEnd) // Trường hợp công việc bắt đầu trước, kết thúc trong tháng
                             )
                         select new PlanErrorFixByEmployeeModel
                         {
                             PlanId = c.Id,
                             EmployeeId = b.EmployeeId,
                             EstimateTime = c.EstimateTime,
                             PlanStartDate = c.PlanStartDate <= searchModel.DateStart ? searchModel.DateStart : c.PlanStartDate,
                             PlanDueDate = c.PlanDueDate >= searchModel.DateTo ? searchModel.DateTo : c.PlanDueDate,
                             Type = (int)Constants.PLanType.Plan
                         }).ToList();

            plans.AddRange(errorFixs);

            // Danh sách ngày nghỉ trong khoảng kế hoạch
            var listHolidayPlan = db.Holidays.AsNoTracking().Where(x => x.HolidayDate >= searchModel.DateStart && x.HolidayDate <= searchModel.DateEnd).ToList();

            List<WorkTimeModel> dayofMonths = new List<WorkTimeModel>();

            int totalDays = (searchModel.DateEnd - searchModel.DateStart).Days;

            for (int i = 0; i <= totalDays; i++)
            {
                dayofMonths.Add(new WorkTimeModel
                {
                    DateTime = searchModel.DateStart.AddDays(i),
                    EstimateTime = 0,
                });
            }

            // Danh sách ngày trong tuần
            List<int> dayofweeks = new List<int>();
            foreach (var item in dayofMonths)
            {
                dayofweeks.Add((int)item.DateTime.DayOfWeek + 1);
            }

            List<WorkingTime> workingTimes = new List<WorkingTime>();

            WorkingTime workingTime;

            List<PlanErrorFixByEmployeeModel> temp = new List<PlanErrorFixByEmployeeModel>();
            List<PlanErrorFixByEmployeeModel> planOfEmployees = new List<PlanErrorFixByEmployeeModel>();
            List<PlanErrorFixByEmployeeModel> planOfEmp = new List<PlanErrorFixByEmployeeModel>();

            DateTime startDate;
            WorkTimeModel workTimeModel;
            bool isHoliday = false;
            foreach (var item in employees)
            {
                workingTime = new WorkingTime();

                // Danh sách công việc của từng nhân viên
                planOfEmployees = plans.Where(r => r.EmployeeId.Equals(item.Id)).ToList();

                for (int i = 0; i <= totalDays; i++)
                {
                    workTimeModel = new WorkTimeModel();
                    startDate = searchModel.DateStart.AddDays(i);
                    planOfEmp = planOfEmployees.Where(r => r.PlanStartDate <= startDate && startDate <= r.PlanDueDate).ToList();

                    isHoliday = listHolidayPlan.Where(r => r.HolidayDate >= DateTimeUtils.ConvertDateFrom(startDate) && r.HolidayDate <= DateTimeUtils.ConvertDateTo(startDate)).Any();
                    if (!isHoliday)
                    {
                        if (planOfEmp.Count > 0)
                        {
                            workTimeModel = new WorkTimeModel()
                            {
                                DateTime = startDate,
                                EstimateTime = planOfEmp.Sum(r => r.EstimateTime),
                                IsHoliday = isHoliday,
                                Plans = planOfEmp,
                                PlanId = planOfEmp.Select(r => r.PlanId).ToList(),
                            };
                        }
                        workingTime.ListWorkingTime.Add(workTimeModel);
                    }
                    else
                    {
                        workTimeModel = new WorkTimeModel()
                        {
                            IsHoliday = true,
                        };
                        workingTime.ListWorkingTime.Add(workTimeModel);
                    }
                }

                workingTime.EmployeeId = item.Id;
                workingTime.WorkType = item.WorkType;
                workingTime.EmployeeCode = item.Code;
                workingTime.EmployeeName = item.Name;
                workingTimes.Add(workingTime);
            }

            return new
            {
                ListResult = workingTimes,
                DayofMonths = dayofMonths,
                DayOfWeeks = dayofweeks
            };
        }

        private static DateTime GetDayInMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Date.Year, dateTime.Date.Month, dateTime.Date.Day);
        }

        public List<ComboboxResult> GetListTask()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            string moduleGroupId = string.Empty;
            try
            {
                searchResult = (from a in db.Tasks.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    ObjectId = a.DepartmentId
                                }).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        public InforEmployee GetEmployee(string DepartmentId)
        {
            InforEmployee result = new InforEmployee();
            //var data = (from a in db.Employees.AsNoTracking()
            //            join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
            //            join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
            //            join d in db.Users.AsNoTracking() on a.Id equals d.EmployeeId
            //            select new InforEmployee
            //            {
            //                EmployeeId = a.Id,
            //                EmployeeCode = a.Code,
            //                EmployeeName = d.UserName,
            //                DepartmentId = b.Id,
            //                DepartmentName = b.Name,
            //                SubId = c.Id,
            //                SubName = c.Name,
            //                UserId = d.Id,
            //            }).AsQueryable();
            //if (!string.IsNullOrEmpty(DepartmentId))
            //{
            //    data = data.Where(a => a.DepartmentId.Equals(DepartmentId));
            //}
            var data = (from a in db.Employees.AsNoTracking()
                        select new InforEmployee
                        {
                            EmployeeId = a.Id,
                            EmployeeCode = a.Code,
                            EmployeeName = a.Name,
                        }).AsQueryable();
            result.ListInforEmployee = data.ToList();

            return result;
        }

        public ProjectProductsModel GetProjectProductInfo(string projectProductId)
        {
            ProjectProductsModel result = new ProjectProductsModel();
            result = (from a in db.ProjectProducts.AsNoTracking()
                      where a.Id.Equals(projectProductId)
                      select new ProjectProductsModel
                      {
                          DataType = a.DataType,
                          ModuleId = a.ModuleId,
                          ProductId = a.ProductId
                      }).FirstOrDefault();

            if (result == null)
            {
                result = new ProjectProductsModel();
            }
            return result;
        }

        public void AddWorkDiary(WorkDiaryModel model, string employeeId, bool outOfDate)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var day = workDiaryBussiness.GetConfigDayWorkDiary();
                    DateTime check = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                    DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    if (day > 0 && (startMonth.AddMonths(-1).Date > model.WorkDate.Date || (DateTime.Now.Date > check.Date && model.WorkDate.Date < startMonth.Date && !outOfDate)))
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0074, check.ToStringDDMMYY());
                    }

                    WorkDiary newWorkSkill = new WorkDiary()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = model.ProjectId,
                        Name = model.Name,
                        SBUId = model.SBUId,
                        DepartmentId = model.DepartmentId,
                        ObjectId = model.ObjectId,
                        ObjectType = model.ObjectType,
                        WorkDate = model.WorkDate,
                        TotalTime = model.TotalTime,
                        Done = model.Done,
                        Address = Constants.Manufacture_TPA,
                        Note = model.Note,
                        EmployeeId = employeeId,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        CreateBy = model.CreateBy,
                        UpdateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                    };

                    var plan = db.Plans.FirstOrDefault(i => i.Id.Equals(model.ObjectId));
                    if (plan != null)
                    {
                        //plan.Done = model.Done;
                        //plan.ExecutionTime = plan.ExecutionTime + model.TotalTime;

                        //var listPlanReference = db.Plans.Where(t => plan.Id.Equals(t.ReferenceId));
                        //if (listPlanReference.Count() > 0)
                        //{
                        //    foreach (var item in listPlanReference)
                        //    {
                        //        item.Done = model.Done;
                        //        item.ExecutionTime = plan.ExecutionTime;
                        //    }
                        //}
                    }
                    else
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Plan);
                    }

                    db.WorkDiaries.Add(newWorkSkill);
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
        /// Lấy số ngày kết thúc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int GetDayEndDate(PlanEndDateModel model)
        {
            decimal ot = 0;
            // Lấy số giờ % OT của plan
            if (model.OT > 0)
            {
                //ot = (decimal)(plan.EstimateTime * plan.OT / 100);
                ot = (decimal)(8 * (decimal)model.OT / 100);
            }

            decimal timeInDay = 8 + ot;

            int day = 0;
            if (model.EstimateTime > timeInDay)
            {
                day = (int)Math.Floor(model.EstimateTime / timeInDay);
            }

            if (model.EstimateTime % timeInDay == 0 && day > 1)
            {
                day--;
            }

            var holidays = db.Holidays.AsNoTracking().Where(i => i.HolidayDate >= model.StartDate).ToList();
            DateTime endate;
            Holiday holiday;
            for (int i = 0; i <= day; i++)
            {
                endate = model.StartDate.AddDays(i);

                holiday = holidays.FirstOrDefault(r => r.HolidayDate.Date == endate.Date);

                if (holiday != null)
                {
                    day++;
                }
            }

            return day;
        }

        public int CheckDay(SearchCommonModel model, double day, double dayTo)
        {
            DateTime dateTime = model.DateTo.Value.AddDays(day);
            TimeSpan timeSpan = dateTime.Subtract(model.DateFrom.Value);
            int dayCheck = db.Holidays.AsNoTracking().Where(i => i.HolidayDate >= model.DateFrom.Value && i.HolidayDate <= dateTime).Count();
            if (dayCheck > 0 && timeSpan.Days - dayCheck < dayTo)
            {
                dayCheck = CheckDay(model, day + 1, dayTo);
            }
            else
            {
                dayCheck = timeSpan.Days;
            }
            return dayCheck;
        }

        public object GetListPlan(List<string> plansId, string EmployeeId)
        {
                    var plans = (from a in db.Plans.AsNoTracking().Where(r => plansId.Contains(r.Id))
                         join d in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals d.Id
                         join b in db.Projects.AsNoTracking() on d.ProjectId equals b.Id
                         join t in db.PlanAssignments.AsNoTracking() on a.Id equals t.PlanId
                         join o in db.Users.AsNoTracking() on t.UserId equals o.Id
                         join e in db.Employees.AsNoTracking() on o.EmployeeId equals e.Id into ae
                         from ea in ae.DefaultIfEmpty()
                         join k in db.Modules.AsNoTracking() on d.ModuleId equals k.Id into dk
                         from kd in dk.DefaultIfEmpty()
                         join m in db.Products.AsNoTracking() on d.ProductId equals m.Id into md
                         from dm in md.DefaultIfEmpty()
                         join i in db.ModuleGroups.AsNoTracking() on kd.ModuleGroupId equals i.Id into kdi
                         from idk in kdi.DefaultIfEmpty()
                         join q in db.Industries.AsNoTracking() on idk.IndustryId equals q.Id into qidk
                         from idkq in qidk.DefaultIfEmpty()
                         join j in db.Users.AsNoTracking() on a.CreateBy equals j.Id
                         join y in db.Employees.AsNoTracking() on j.EmployeeId equals y.Id into ee
                         from y in ee.DefaultIfEmpty()
                         select new PlanErrorFixResultModel()
                         {
                             Id = a.Id,
                             Type = (int)Constants.PLanType.Plan,
                             ResponsiblePersionId = ea != null ? ea.Code : string.Empty, 
                             ResponsiblePersion = ea != null ? ea.Name : string.Empty, // Người phụ trách
                             //EmployeeName = o.UserName, // Người tạo
                             EmployeeName = y.Name, //Người tạo kế hoạch
                             EstimateTime = a.EstimateTime, //  Thời gian thực hiện (h)
                             ActualStartDate = a.ActualStartDate, // Thời gian bđ thực tế
                             ActualEndDate = a.ActualEndDate, // Thời gian kết thúc thực tế
                             Status = a.Status,// Tình trạng công việc
                             PlanStartDate = a.PlanStartDate, // Ngày bắt đầu dự kiến 
                             PlanDueDate = a.PlanDueDate, // Ngày kết thúc dự kiến 
                             ObjectName = a.Name, // là công việc
                             ContractCode = d.ContractCode, // Mã theo hợp đồng
                             ContractName = d.ContractName, // Tên theo hợp đồng
                             DesignCode = kd != null ? kd.Code : dm != null ? dm.Code : string.Empty, // Mã theo thiết kế 
                             DesignName = kd != null ? kd.Name : dm != null ? dm.Name : string.Empty, // Tên theo thiết kế
                             IndustryCode = idkq != null ? idkq.Code : string.Empty,  // mã nghành hàng
                             Quantity = d.Quantity, // số lượng
                             PricingModule = kd != null ? kd.Pricing : 0, // Giá tổng hợp thiết kế
                             DataType = d.DataType == Constants.ProjectProduct_DataType_Module ? "Module" : (d.DataType == Constants.ProjectProduct_DataType_Paradigm ? "Mô hình" : (d.DataType == Constants.ProjectProduct_DataType_Practice ? "Bài thực hành" : "Sản phẩm")), // Kiểu dữ liệu
                             ModuleStatusView = d.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung", // Tình trạng bổ sung ngoài hợp đồng
                             DesignStatusView = d.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : (d.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : "Tận dụng"), // TÌnh trạng thiết kế
                             KickOffDate = b.KickOffDate, // ngày kick offf
                             ExpectedDesignFinishDate = d.ExpectedDesignFinishDate,
                             ExpectedMakeFinishDate = d.ExpectedMakeFinishDate,
                             ExpectedTransferDate = d.ExpectedTransferDate,
                             PlanId = a.Id, // planId
                             ProjectId = a.ProjectId,
                             ProjectName = b.Name,
                             ProjectCode = b.Code,
                             ProjectProductId = a.ProjectProductId,
                             EpCode = ea != null ? ea.Code : string.Empty
                         }).AsQueryable()
                            .Union(from a in db.ErrorFixs.AsNoTracking()
                                   where plansId.Contains(a.Id)
                                   join g in db.Errors.AsNoTracking() on a.ErrorId equals g.Id
                                   join e in db.Employees.AsNoTracking() on a.EmployeeFixId equals e.Id into ae
                                   from ea in ae.DefaultIfEmpty()
                                   join t in db.Projects.AsNoTracking() on g.ProjectId equals t.Id
                                   join k in db.Modules.AsNoTracking() on g.ObjectId equals k.Id into dk
                                   from kd in dk.DefaultIfEmpty()
                                   join m in db.Products.AsNoTracking() on g.ObjectId equals m.Id into md
                                   from dm in md.DefaultIfEmpty()
                                   join i in db.ModuleGroups.AsNoTracking() on kd.ModuleGroupId equals i.Id into kdi
                                   from idk in kdi.DefaultIfEmpty()
                                   join q in db.Industries.AsNoTracking() on idk.IndustryId equals q.Id into qidk
                                   from idkq in qidk.DefaultIfEmpty()
                                   select new PlanErrorFixResultModel()
                                   {
                                       Id = a.Id,
                                       Type = (int)Constants.PLanType.Error,
                                       ResponsiblePersionId = ea != null ? ea.Code : string.Empty,
                                       ResponsiblePersion = ea != null ? ea.Name : string.Empty, // Người phụ trách
                                       EmployeeName = ea.Name, // Người tạo
                                       EstimateTime = a.EstimateTime, //  Thời gian thực hiện (h)
                                       ActualStartDate = a.DateFrom, // Thời gian bđ thực tế
                                       ActualEndDate = a.DateTo, // Thời gian kết thúc thực tế
                                       Status = a.Status,// Tình trạng công việc
                                       PlanStartDate = null, // Ngày bắt đầu dự kiến 
                                       PlanDueDate = null, // Ngày kết thúc dự kiến 
                                       ObjectName = a.Solution, // là công việc lỗi
                                       ContractCode = null, // Mã theo hợp đồng
                                       ContractName = null, // Tên theo hợp đồng
                                       DesignCode = kd != null ? kd.Code : dm != null ? dm.Code : string.Empty, // Mã theo thiết kế 
                                       DesignName = kd != null ? kd.Name : dm != null ? dm.Name : string.Empty, // Tên theo thiết kế
                                       IndustryCode = idkq != null ? idkq.Code : string.Empty,  // mã nghành hàng
                                       Quantity = 0, // số lượng
                                       PricingModule = kd != null ? kd.Pricing : 0, // Giá tổng hợp thiết kế
                                       DataType = null, // Kiểu dữ liệu
                                       ModuleStatusView = null, // Tình trạng bổ sung ngoài hợp đồng
                                       DesignStatusView = null, // TÌnh trạng thiết kế
                                       KickOffDate = t.KickOffDate, // ngày kick offf
                                       ExpectedDesignFinishDate = null,
                                       ExpectedMakeFinishDate = null,
                                       ExpectedTransferDate = null,
                                       PlanId = a.Id, // errorFixId
                                       ProjectId = g.ProjectId,
                                       ProjectName = t.Name,
                                       ProjectCode = t.Code,
                                       ProjectProductId = null,
                                       EpCode = ea != null ? ea.Code : string.Empty
                                   }).AsQueryable();

            //if (!string.IsNullOrEmpty(EmployeeCode) && !EmployeeCode.Equals("null"))
            //{
            //    listPlan = listPlan.Where(a => a.EpCode.Equals(EmployeeCode));
            //}
            if (!string.IsNullOrEmpty(EmployeeId))
            {
                plans = plans.Where(a => a.ResponsiblePersionId.Equals(EmployeeId));
            }

            var listPlanResult = plans.ToList();

            return listPlanResult;
        }

        public List<TaskFlowStageSearchResultModel> GetListTask(string deparmentid)
        {
            var dataQuery = (from a in db.Tasks.AsNoTracking()
                             join b in db.FlowStages.AsNoTracking() on a.FlowStageId equals b.Id into ab
                             from abv in ab.DefaultIfEmpty()
                             join c in db.SBUs.AsNoTracking() on a.SBUId equals c.Id into ac
                             from acv in ac.DefaultIfEmpty()
                             join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                             from adv in ad.DefaultIfEmpty()
                             orderby a.Name
                             select new TaskFlowStageSearchResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 Code = a.Code,
                                 IsDesignModule = a.IsDesignModule.HasValue ? a.IsDesignModule.Value : false,
                                 FlowStageId = a.FlowStageId,
                                 FlowStageName = abv != null ? abv.Name : "",
                                 Type = a.Type,
                                 CreateDate = a.CreateDate,
                                 SBUId = a.SBUId,
                                 DepartmentId = a.DepartmentId,
                             }).AsQueryable();

            var listResult = dataQuery.ToList();
            foreach (var item in listResult)
            {
                item.Departments = db.TaskWorkTypes.AsNoTracking().Where(a => a.TaskId.Equals(item.Id)).Select(a => a.DepartmentId).ToList();
            }

            if (!string.IsNullOrEmpty(deparmentid) && !deparmentid.Equals("null"))
            {
                listResult = listResult.Where(a => a.Departments.Contains(deparmentid)).ToList();
            }

            return listResult;
        }

        public List<PlanHistoryInfoModel> GetPlanHistory(string projectId)
        {
            var listPlanHistory = (from a in db.PlanHistories.AsNoTracking()
                                   join b in db.Users.AsNoTracking() on a.CreateBy equals b.Id
                                   join c in db.Employees.AsNoTracking() on b.EmployeeId equals c.Id
                                   where a.ProjectId.Equals(projectId)
                                   orderby a.Version descending
                                   select new PlanHistoryInfoModel()
                                   {
                                       Id = a.Id,
                                       ProjectId = a.ProjectId,
                                       Version = a.Version,
                                       Status = a.Status,
                                       AcceptDate = a.AcceptDate,
                                       CreateBy = a.CreateBy,
                                       CreateByName = c.Name,
                                       CreateDate = a.CreateDate,
                                       Description = a.Description
                                   }).ToList();

            foreach (var item in listPlanHistory)
            {
                item.ListDatashet = (from a in db.PlanHistoryAttaches.AsNoTracking()
                                     where a.PlanHistoryId.Equals(item.Id)
                                     select new DownloadModel
                                     {
                                         FileName = a.FileName,
                                         Path = a.Path
                                     }).ToList();

                item.FileName = string.Join(", \r\n", item.ListDatashet.Select(a => a.FileName).ToList());
            }

            return listPlanHistory;
        }
    }
}
