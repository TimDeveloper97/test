using NTS.Common;
using NTS.Common.Helpers;
using NTS.Model.Combobox;
using NTS.Model.DashboardEmployee;
using NTS.Model.Employees;
using NTS.Model.Plans;
using NTS.Model.Repositories;
using NTS.Model.TaskTimeStandardModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.DashboardEmployee
{
    public class DashboardEmployeeBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public DashboardEmployeeModel SearchListEmployee(DashboardEmployeeSearchModel modelSearch)
        {
            DashboardEmployeeModel dashboard = new DashboardEmployeeModel();
            List<EmployeeModel> ListEmployee = new List<EmployeeModel>();
            List<PlanResultModel> ListPlan = new List<PlanResultModel>();

            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            if (string.IsNullOrEmpty(modelSearch.TimeType))
            {
                modelSearch.TimeType = "0";
            }
            if (!modelSearch.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(modelSearch.TimeType, modelSearch.Year, modelSearch.Month, modelSearch.Quarter, ref dateFrom, ref dateTo);
            }
            else
            {
                if (!modelSearch.DateFrom.HasValue || !modelSearch.DateTo.HasValue)
                {
                    return dashboard;
                }

                dateFrom = modelSearch.DateFrom.Value.ToStartDate();
                dateTo = modelSearch.DateTo.Value.ToEndDate();
            }

            var listEmployee = (from a in db.Employees.AsNoTracking()
                                join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                //where a.WorkType == modelSearch.WorkType
                                where a.Status == Constants.Employee_Status_Use
                                orderby a.Code
                                select new EmployeeModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    DepartmentId = a.DepartmentId,
                                    SBUID = c.Id,
                                    WorkType = a.WorkTypeId
                                }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.WorkType))
            {
                listEmployee = listEmployee.Where(i => i.WorkType.Equals(modelSearch.WorkType));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                listEmployee = listEmployee.Where(i => i.SBUID.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                listEmployee = listEmployee.Where(i => i.DepartmentId.Equals(modelSearch.DepartmentId));
            }
            ListEmployee = listEmployee.ToList();
            List<ObjectEmployeeModel> objectEmployees = new List<ObjectEmployeeModel>();
            double TotalItems = 0;

            //var planQuery = (from a in db.Plans.AsNoTracking()
            //                 join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id
            //                 where string.IsNullOrEmpty(a.ReferenceId)
            //                 select new PlanResultModel
            //                 {
            //                     Id = a.Id,
            //                     Name = a.Name,
            //                     RealEndDate = a.RealEndDate,
            //                     DataType = b.DataType,
            //                     DesignStatus = b.DesignStatus,
            //                     StartDate = a.StartDate,
            //                     ExecutionTime = a.ExecutionTime,
            //                     ResponsiblePersion = a.ResponsiblePersion,
            //                     Status = a.Status
            //                 }).AsQueryable();

            //if (!modelSearch.TimeType.Equals("0"))
            //{
            //    planQuery = planQuery.Where(i => i.StartDate.Value >= dateFrom && i.StartDate.Value <= dateTo);
            //}

            //var plans = planQuery.ToList();

            List<PlanResultModel> planEmployee;
            foreach (var item in ListEmployee)
            {
                //- Lấy công việc của nhân viên
                //planEmployee = plans.Where(r => r.ResponsiblePersion.Equals(item.Id)).ToList();

                //TotalItems += planEmployee.Count;

                //+ Số công việc hoàn thành
                //var realEndDateTrue = planEmployee.Where(i => i.Status == 3).ToList();
                //ListPlan.AddRange(realEndDateTrue);

                //* Số lượng mô hình
                //var dataTypeParadigmTrue = realEndDateTrue.Where(i => i.DataType == Constants.ProjectProduct_DataType_Paradigm).ToList();

                //$ Thiết kế mới (mô hình)
                //var paradigmNewDesignTrue = dataTypeParadigmTrue.Where(i => Constants.ProjectProduct_DesignStatus_NewDesign == i.DesignStatus).Count();
                //$ Thiết kế sửa đổi(mô hình) 
                //var updateDesignTrue = dataTypeParadigmTrue.Where(i => Constants.ProjectProduct_DesignStatus_UpdateDesign == i.DesignStatus).Count();
                //$ Thiết kế tận dụng(mô hình) 
                //var useDesignUseTrue = dataTypeParadigmTrue.Where(i => Constants.ProjectProduct_DesignStatus_Use == i.DesignStatus).Count();

                //* Số lượng module
                //var check = realEndDateTrue.Where(i => i.DataType != Constants.ProjectProduct_DataType_Module).ToList();
                //var dataTypeModuleTrue = realEndDateTrue.Where(i => i.DataType == Constants.ProjectProduct_DataType_Module).ToList();

                //$ Thiết kế mới (module)
                //var paradigmNewDesign = dataTypeModuleTrue.Where(i => Constants.ProjectProduct_DesignStatus_NewDesign == i.DesignStatus).Count();
                ////$ Thiết kế sửa đổi(module) 
                //var updateDesign = dataTypeModuleTrue.Where(i => Constants.ProjectProduct_DesignStatus_UpdateDesign == i.DesignStatus).Count();
                ////$ Thiết kế tận dụng(module) 
                //var useDesign = dataTypeModuleTrue.Where(i => Constants.ProjectProduct_DesignStatus_Use == i.DesignStatus).Count();

                ////+ Số công việc đang triển khai
                ////var realEndDateFalse = plans.Where(i => i.Status == 2).ToList();

                ////* Số lượng mô hình
                //var dataTypeParadigmFalse = realEndDateFalse.Where(i => i.DataType == Constants.ProjectProduct_DataType_Paradigm).ToList();

                ////$ Thiết kế mới (mô hình)
                //var paradigmNewDesignFalse = dataTypeParadigmFalse.Where(i => Constants.ProjectProduct_DesignStatus_NewDesign == i.DesignStatus).Count();
                ////$ Thiết kế sửa đổi(mô hình) 
                //var updateDesignFalse = dataTypeParadigmFalse.Where(i => Constants.ProjectProduct_DesignStatus_UpdateDesign == i.DesignStatus).Count();
                ////$ Thiết kế tận dụng(mô hình) 
                //var useDesignUseFalse = dataTypeParadigmFalse.Where(i => Constants.ProjectProduct_DesignStatus_Use == i.DesignStatus).Count();

                ////* Số lượng module
                //var dataTypeModuleFals = realEndDateFalse.Where(i => i.DataType == Constants.ProjectProduct_DataType_Module).ToList();

                ////$ Thiết kế mới (module)
                //var moduleNewDesign = dataTypeModuleFals.Where(i => Constants.ProjectProduct_DesignStatus_NewDesign == i.DesignStatus).Count();
                ////$ Thiết kế sửa đổi(module) 
                //var moduleUpdateDesign = dataTypeModuleFals.Where(i => Constants.ProjectProduct_DesignStatus_UpdateDesign == i.DesignStatus).Count();
                ////$ Thiết kế tận dụng(module) 
                //var moduleUseDesign = dataTypeModuleFals.Where(i => Constants.ProjectProduct_DesignStatus_Use == i.DesignStatus).Count();

                ////Add thông tin
                //objectEmployees.Add(new ObjectEmployeeModel
                //{
                //    Name = item.Name,

                //    FinishParadigmNewDesign = paradigmNewDesignTrue,
                //    FinishParadigmUpdateDesign = updateDesignTrue,
                //    FinishParadigmUseDesign = useDesignUseTrue,
                //    FinishModuleNewDesign = paradigmNewDesign,
                //    FinishModuleUpdateDesign = updateDesign,
                //    FinishModuleUseDesign = useDesign,

                //    MakeParadigmNewDesign = paradigmNewDesignFalse,
                //    MakeParadigmUpdateDesign = updateDesignFalse,
                //    MakeParadigmUseDesign = useDesignUseFalse,
                //    MakeModuleNewDesign = moduleNewDesign,
                //    MakeModuleUpdateDesign = moduleUpdateDesign,
                //    MakeModuleUseDesign = moduleUseDesign,
                //});
            }

            dashboard.TotalItems = TotalItems;
            dashboard.ListEmployee = ListEmployee;
            dashboard.ListPlan = ListPlan;
            dashboard.ListDashboard = objectEmployees;
            return dashboard;
        }

        public object GetGeneralDashboardEmployee(DashboardEmployeeSearchModel searchModel)
        {
            List<double> ListMediumType = new List<double>();
            List<ObjectEmployeeModel> ListData = new List<ObjectEmployeeModel>();
            List<ListTime> times = new List<ListTime>();
            if (searchModel.ListModuleGroupId.Count > 0)
            {
                //- Lấy công việc của nhân viên
                //var plans = (from a in db.Plans.AsNoTracking()
                //             join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id
                //             join c in db.Tasks.AsNoTracking() on a.TaskId equals c.Id
                //             join d in db.Modules.AsNoTracking() on b.ModuleId equals d.Id
                //             //join e in searchModel.ListModuleGroupId on d.ModuleGroupId equals e
                //             where a.ResponsiblePersion.Equals(searchModel.EmployeeId) || searchModel.ListModuleGroupId.Contains(d.ModuleGroupId)
                //             select new
                //             {
                //                 PlanId = a.Id,
                //                 PlanName = a.Name,
                //                 TaskId = c.Id,
                //                 TaskName = c.Name,
                //                 a.ExecutionTime,
                //                 d.ModuleGroupId,
                //             }).ToList();
                ////var listPlan = plans.ToList();
                //var ListTimeStand = db.TaskTimeStandards.AsNoTracking().ToList();
                //var group = plans.GroupBy(a => new { a.TaskName, a.TaskId }).Select(g => new { g.Key.TaskId, g.Key.TaskName, ListPlan = g.ToList() }).ToList();

                //string taskName = "";
                //string header = "";
                //decimal TimeStandard = 0;
                //decimal avg = 0;
                //ObjectEmployeeModel model = new ObjectEmployeeModel();
                //foreach (var item in group)
                //{
                //    model = new ObjectEmployeeModel();
                //    taskName = item.TaskName;
                //    model.TaskName = taskName;
                //    foreach (var ite in searchModel.ListModuleGroupId)
                //    {
                //        //foreach (var itm in moduleGroupId)
                //        //{
                //        header = db.ModuleGroups.AsNoTracking().FirstOrDefault(a => a.Id.Equals(ite)).Code;
                //        var planIngroup = item.ListPlan.Where(a => a.ModuleGroupId.Equals(ite)).ToList();
                //        //if (ite.Equals(moduleGroupId))
                //        if (planIngroup.Count > 0)
                //        {
                //            var tem = ListTimeStand.FirstOrDefault(a => a.TaskId.Equals(item.TaskId) && a.EmployeeId.Equals(searchModel.EmployeeId));
                //            if (tem != null)
                //            {
                //                TimeStandard = tem.TimeStandard;
                //            }
                //            else
                //            {
                //                TimeStandard = 0;
                //            }

                //            var temp = planIngroup.Sum(v => v.ExecutionTime);
                //            var totalplan = planIngroup.Count;
                //            avg = temp / totalplan;
                //            model.ListTime.Add(new ListTime { ModuleGroupCode = header, Avg = avg, TimeStandard = TimeStandard });
                //        }
                //        else
                //        {
                //            model.ListTime.Add(new ListTime { ModuleGroupCode = header, Avg = 0, TimeStandard = 0 });
                //        }
                //        //}
                //    }
                //    ListData.Add(model);
                //}
            }
            return ListData;

        }
        public object GetEmployee(DashboardEmployeeSearchModel modelSearch)
        {
            var listEmployee = (from a in db.Employees.AsNoTracking()
                                join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                                join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                                where a.Status == Constants.Employee_Status_Use
                                orderby a.Code
                                select new EmployeeModel
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    Name = a.Name,
                                    DepartmentId = a.DepartmentId,
                                    SBUID = c.Id,
                                }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                listEmployee = listEmployee.Where(i => i.SBUID.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                listEmployee = listEmployee.Where(i => i.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            return listEmployee;
        }
    }
}
