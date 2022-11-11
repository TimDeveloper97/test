using NTS.Common;
using NTS.Model.Combobox;
using NTS.Model.Employees;
using NTS.Model.FuturePersonnelForecast;
using NTS.Model.Plans;
using NTS.Model.Practice;
using NTS.Model.Product;
using NTS.Model.ProjectProducts;
using NTS.Model.Repositories;
using NTS.Model.ScheduleProject;
using NTS.Model.Task;
using NTS.Model.TaskTimeStandardModel;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.FuturePersonnelForecast
{
    public class FuturePersonnelForecastBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public SearchResultModel<FuturePersonnelForecastModel> SearchSelectProject(FuturePersonnelForecastModel modelSearch)
        {
            SearchResultModel<FuturePersonnelForecastModel> searchResult = new SearchResultModel<FuturePersonnelForecastModel>();

            var dataProject = (from a in db.Projects.AsNoTracking()
                               where a.SBUId.Equals(modelSearch.SBUId)
                               select new FuturePersonnelForecastModel
                               {
                                   Id = a.Id,
                                   Name = a.Name,
                                   Code = a.Code
                               }).AsNoTracking();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataProject = dataProject.Where(i => i.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                dataProject = dataProject.Where(i => i.Code.ToUpper().Contains(modelSearch.Code.ToUpper()));
            }

            searchResult.ListResult = dataProject.ToList();
            return searchResult;
        }
        public SearchResultModel<FuturePersonnelForecastModel> Search(FuturePersonnelForecastModel modelSearch)
        {
            SearchResultModel<FuturePersonnelForecastModel> searchResult = new SearchResultModel<FuturePersonnelForecastModel>();
            var dataQuery = (from a in db.Employees.AsNoTracking()
                             join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id into ab
                             from b in ab.DefaultIfEmpty()
                             join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id into bc
                             from c in bc.DefaultIfEmpty()
                             select new FuturePersonnelForecastModel()
                             {
                                 EmployeeName = a.Name,
                                 DepartmantName = b.Name,
                                 EmployeeId = a.Id,
                                 SBUId = c.Id,
                                 DepartmantId = b.Id,
                                 SBUName = c.Name
                             }).AsQueryable();
            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.EmployeeName.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                dataQuery = dataQuery.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }
            if (!string.IsNullOrEmpty(modelSearch.DepartmantId))
            {
                dataQuery = dataQuery.Where(u => u.DepartmantId.Equals(modelSearch.DepartmantId));
            }
            double daysUntilChristmas = 0;
            if (!string.IsNullOrEmpty(modelSearch.DateEnd.ToString()) && !string.IsNullOrEmpty(modelSearch.DateStart.ToString()))
            {
                daysUntilChristmas = Convert.ToInt32(modelSearch.DateStart.Subtract(modelSearch.DateEnd).TotalDays * 8);
            }
            List<FuturePersonnelForecastModel> employees = dataQuery.ToList();

            TaskStandardTypeModel workType;
            List<TaskStandardTypeModel> listWorkType = new List<TaskStandardTypeModel>();
            workType = new TaskStandardTypeModel();
            workType.Type = 1;
            listWorkType.Add(workType);

            workType = new TaskStandardTypeModel();
            workType.Type = 2;
            listWorkType.Add(workType);

            workType = new TaskStandardTypeModel();
            workType.Type = 3;
            listWorkType.Add(workType);

            var taskTimeStandards = db.TaskTimeStandards.AsNoTracking();
            var task = db.Tasks.AsNoTracking();


            foreach (var item in employees)
            {
                var taskEmployee = taskTimeStandards.Where(r => r.EmployeeId.Equals(item.EmployeeId));
                item.ListWorkType = (from t in listWorkType
                                     join h in task on t.Type equals h.Type
                                     join s in taskEmployee on h.Id equals s.TaskId into ts
                                     from stn in ts.DefaultIfEmpty()
                                     select new TaskStandardTypeModel
                                     {
                                         Type = t.Type,
                                         TimeStandard = stn != null ? stn.TimeStandard : 0,
                                     }).ToList();

                List<TaskStandardTypeModel> listRs = new List<TaskStandardTypeModel>();

                var lstRs = item.ListWorkType.GroupBy(t => new { t.Type }).Select(cl => new TaskStandardTypeModel
                {
                    Type = cl.Key.Type,
                    TimeStandard = cl.Sum(c => c.TimeStandard),
                }).ToList();

                item.ListWorkType = lstRs;
                if (!string.IsNullOrEmpty(modelSearch.ModuleGroupId))
                {
                    decimal type1 = 0, type2 = 0, type3 = 0;
                    var dataModuleGroup = (from d in db.TaskModuleGroups.AsNoTracking()
                                           join e in db.Tasks.AsNoTracking() on d.TaskId equals e.Id
                                           join f in db.ModuleGroups.AsNoTracking() on d.ModuleGroupId equals f.Id
                                           where d.ModuleGroupId.Equals(modelSearch.ModuleGroupId)
                                           select new ModuleGroupModel()
                                           {
                                               Id = d.Id,
                                               Type = e.Type,
                                           }).AsQueryable().ToList();
                    foreach (var data in dataModuleGroup)
                    {
                        switch (data.Type)
                        {
                            case 1:
                                type1++;
                                break;
                            case 2:
                                type2++;
                                break;
                            case 3:
                                type3++;
                                break;
                        }
                    }
                    foreach (var it in item.ListWorkType)
                    {
                        switch (it.Type)
                        {
                            case 1:
                                searchResult.Status1 = Convert.ToInt32(it.TimeStandard * type1);
                                break;
                            case 2:
                                searchResult.Status2 = Convert.ToInt32(it.TimeStandard * type2);
                                break;
                            case 3:
                                searchResult.Status3 = Convert.ToInt32(it.TimeStandard * type3);
                                break;
                        }
                    }

                    int total = searchResult.Status1 + searchResult.Status2 + searchResult.Status3;
                    if (total > 0)
                    {
                        item.totalItem = Math.Round((daysUntilChristmas / total), 2);
                    }
                }

                if (!string.IsNullOrEmpty(modelSearch.ProductId))
                {
                    int type1 = 0, type2 = 0, type3 = 0;
                    ProductModel product = new ProductModel();
                    List<ModuleInPracticeModel> listModule = new List<ModuleInPracticeModel>();
                    product.Id = modelSearch.ProductId;
                    var listPractice = (from a in db.PracticInProducts.AsNoTracking()
                                        join b in db.Practices.AsNoTracking() on a.PracticeId equals b.Id
                                        where a.ProductId.Equals(modelSearch.ProductId)
                                        select new PracticeModel
                                        {
                                            Id = a.PracticeId,
                                            Name = b.Name,
                                            ListModuleInPractice = (from c in db.ModuleInPractices.AsNoTracking()
                                                                    join d in db.Modules.AsNoTracking() on c.ModuleId equals d.Id
                                                                    join e in db.ModuleGroups.AsNoTracking() on d.ModuleGroupId equals e.Id
                                                                    where c.PracticeId.Equals(a.PracticeId)
                                                                    select new ModuleInPracticeModel
                                                                    {
                                                                        Id = c.Id,
                                                                        ModuleId = c.ModuleId,
                                                                        PracticeId = c.PracticeId,
                                                                        Qty = c.Qty,
                                                                        ModuleName = d.Name,
                                                                        Specification = d.Specification,
                                                                        Note = d.Note,
                                                                        Code = d.Code,
                                                                        Pricing = d.Pricing,
                                                                        ModuleGroupId = e.Id,
                                                                    }).ToList()
                                        }).ToList();


                    foreach (var practice in listPractice)
                    {

                        foreach (var dataProductModule in practice.ListModuleInPractice)
                        {

                            var dataModuleGroup = (from d in db.TaskModuleGroups.AsNoTracking()
                                                   join e in db.Tasks.AsNoTracking() on d.TaskId equals e.Id
                                                   join f in db.ModuleGroups.AsNoTracking() on d.ModuleGroupId equals f.Id
                                                   where d.ModuleGroupId.Equals(dataProductModule.ModuleGroupId)
                                                   select new ModuleGroupModel()
                                                   {
                                                       Id = d.Id,
                                                       Type = e.Type,
                                                   }).AsQueryable().ToList();
                            foreach (var data in dataModuleGroup)
                            {

                                switch (data.Type)
                                {
                                    case 1:
                                        type1++;
                                        break;
                                    case 2:
                                        type2++;
                                        break;
                                    case 3:
                                        type3++;
                                        break;
                                }
                            }
                            foreach (var it in item.ListWorkType)
                            {
                                switch (it.Type)
                                {
                                    case 1:
                                        searchResult.Status1 = Convert.ToInt32(it.TimeStandard * type1);
                                        break;
                                    case 2:
                                        searchResult.Status2 = Convert.ToInt32(it.TimeStandard * type2);
                                        break;
                                    case 3:
                                        searchResult.Status3 = Convert.ToInt32(it.TimeStandard * type3);
                                        break;
                                }
                            }
                            int totalProduct = searchResult.Status1 + searchResult.Status2 + searchResult.Status3;
                            if (totalProduct > 0)
                            {
                                item.totalItemProduc = Math.Round((daysUntilChristmas / totalProduct), 2);
                            }
                        }
                    }

                }

            }
            searchResult.ListResult = employees;
            return searchResult;
        }

        public object GetListPlanByProjectId(FuturePersonnelForecastSearchModel modelSearch)
        {
            //var query = (from a in db.Plans.AsNoTracking()
            //             join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id
            //             join c in db.Projects.AsNoTracking() on a.ProjectId equals c.Id into ac
            //             from ca in ac.DefaultIfEmpty()
            //             join d in db.Tasks.AsNoTracking() on a.TaskId equals d.Id
            //             //join e in db.Departments.AsNoTracking() on acn.DepartmentId equals e.Id into ae
            //             //from aen in ae.DefaultIfEmpty()
            //             where string.IsNullOrEmpty(a.ResponsiblePersion)
            //             select new ScheduleProjectResultModel()
            //             {
            //                 Id = a.Id,
            //                 PlanId = a.Id,
            //                 ProjectId = b.ProjectId,
            //                 NameView = d.Name,
            //                 PlanName = d.Name,
            //                 ParentId = b.Id,
            //                 ExecutionTime = a.ExecutionTime,
            //                 StartDate = a.StartDate,
            //                 EndDate = a.EndDate,
            //                 RealStartDate = a.RealStartDate,
            //                 RealEndDate = a.RealEndDate,
            //                 DataType = 5,
            //                 KickOffDate = ca.KickOffDate,
            //                 ModuleId = b.ModuleId,
            //                 TaskId = a.TaskId,
            //                 ResponsiblePersion = a.ResponsiblePersion,
            //                 //ResponsiblePersionName = acn != null ? acn.Name : string.Empty,
            //                 DepartmentId = ca != null ? ca.DepartmentId : string.Empty,
            //                 SbuId = ca != null ? ca.SBUId : string.Empty,
            //                 ProjectProductId = a.ProjectProductId,
            //                 StatusView = a.Status == Constants.Plan_Status_DoNot ? "Chưa thực hiện" : a.Status == Constants.Plan_Status_Doing ? "Đang thực hiện" : a.Status == Constants.Plan_Status_Done ? "Đã hoàn thành" : "",
            //             }).AsQueryable();


            var dataQuery = (from a in db.ProjectProducts.AsNoTracking()
                             join c in db.Modules.AsNoTracking() on a.ModuleId equals c.Id into ac
                             from acn in ac.DefaultIfEmpty()
                             join f in db.ModuleGroups.AsNoTracking() on acn.ModuleGroupId equals f.Id into cf
                             from cfn in cf.DefaultIfEmpty()
                             join g in db.Industries.AsNoTracking() on cfn.IndustryId equals g.Id into fg
                             from fgn in fg.DefaultIfEmpty()
                             join d in db.Products.AsNoTracking() on a.ProductId equals d.Id into ad
                             from adn in ad.DefaultIfEmpty()
                             join e in db.Projects.AsNoTracking() on a.ProjectId equals e.Id
                             orderby a.ContractName
                             select new ScheduleProjectResultModel()
                             {
                                 Id = a.Id,
                                 ProjectId = a.ProjectId,
                                 ContractCode = a.ContractCode,
                                 ContractName = a.ContractName,
                                 //ProjectCode = b.Code,
                                 //ProjectName = b.Name,
                                 ParentId = a.ParentId != null ? a.ParentId : a.ProjectId,
                                 ModuleId = a.ModuleId,
                                 ModuleGroupId = cfn.Id,
                                 ProductId = a.ProductId,
                                 //PlanCode = a.ProductId != null ? (d.Code + " - " + d.Name) : (a.ModuleId != null ? (c.Code + " - " + c.Name) : a.ContractCode + " - " + a.ContractName),
                                 DataType = a.DataType,
                                 DatatypeName = a.DataType == 1 ? Constants.DatatypeNamePractice : (a.DataType == 2 ? Constants.DatatypeNameProduct : (a.DataType == 3 ? Constants.DatatypeNameParadigm : Constants.DatatypeNameModule)),
                                 DepartmentId = e.DepartmentId,
                                 SbuId = e.SBUId,
                                 PracticeCode = a.DataType == 1 ? a.ContractCode : "",
                                 PracticeName = a.DataType == 1 ? a.ContractName : "",
                                 DesignCode = acn != null ? acn.Code : adn != null ? adn.Code : string.Empty,
                                 DesignName = acn != null ? acn.Name : adn != null ? adn.Name : string.Empty,
                                 Quantity = a.Quantity,
                                 RealQuantity = a.RealQuantity,
                                 ModuleStatusView = a.ModuleStatus == Constants.ProjectProduct_ModuleStatus_Project ? "Dự án" : "Bổ sung",
                                 DesignStatusView = a.DesignStatus == Constants.ProjectProduct_DesignStatus_NewDesign ? "Thiết kế mới" : (a.DesignStatus == Constants.ProjectProduct_DesignStatus_UpdateDesign ? "Sửa thiết kế cũ" : "Tận dụng"),
                                 KickOffDate = e.KickOffDate,
                                 DesignFinishDate = a.DesignFinishDate,
                                 MakeFinishDate = a.MakeFinishDate,
                                 DeliveryDate = a.DeliveryDate,
                                 TransferDate = a.TransferDate,
                                 ExpectedDesignFinishDate = a.ExpectedDesignFinishDate,
                                 ExpectedMakeFinishDate = a.ExpectedMakeFinishDate,
                                 ExpectedTransferDate = a.ExpectedTransferDate,
                                 PricingModule = acn != null ? acn.Pricing : 0,
                                 IndustryCode = fgn != null ? fgn.Code : "",
                                 NameView = a.ContractName,
                             }
                           ).AsQueryable();

            var queryProject = (from a in db.Projects.AsNoTracking()
                                orderby a.Name
                                select new ScheduleProjectResultModel()
                                {
                                    Id = a.Id,
                                    ProjectId = a.Id,
                                    ParentId = null,
                                    PlanCode = a.Code + " - " + a.Name,
                                    DepartmentId = a.DepartmentId,
                                    SbuId = a.SBUId,
                                    NameView = "DA - " + a.Code + " - " + a.Name,
                                    StatusProject = a.Status
                                }).AsQueryable();

            //var listEmployeeQuery = (from a in db.Employees.AsNoTracking()
            //                         join b in db.Plans.AsNoTracking() on a.Id equals b.ResponsiblePersion into ab
            //                         from ba in ab.DefaultIfEmpty()
            //                         join c in db.Departments.AsNoTracking() on a.DepartmentId equals c.Id
            //                         orderby a.Name
            //                         select new { a.Id, a.Name, ba.EndDate, c.SBUId, a.DepartmentId }).AsQueryable();

            //if (!string.IsNullOrEmpty(modelSearch.ProjectId))
            //{
            //    queryProject = queryProject.Where(u => u.ProjectId.ToUpper().Equals(modelSearch.ProjectId.ToUpper()));
            //    dataQuery = dataQuery.Where(u => u.ProjectId.ToUpper().Equals(modelSearch.ProjectId.ToUpper()));
            //    query = query.Where(u => u.ProjectId.ToUpper().Equals(modelSearch.ProjectId.ToUpper()));
            //    listEmployeeQuery = listEmployeeQuery.Where(i => i.SBUId.Equals(modelSearch.SBUId));
            //}


            //if (!string.IsNullOrEmpty(modelSearch.SBUId))
            //{
            //    queryProject = queryProject.Where(u => u.SbuId.ToUpper().Equals(modelSearch.SBUId.ToUpper()));
            //    dataQuery = dataQuery.Where(u => u.SbuId.ToUpper().Equals(modelSearch.SBUId.ToUpper()));
            //    query = query.Where(u => u.SbuId.ToUpper().Equals(modelSearch.SBUId.ToUpper()));
            //}

            //if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            //{
            //    queryProject = queryProject.Where(u => u.DepartmentId.ToUpper().Equals(modelSearch.DepartmentId.ToUpper()));
            //    dataQuery = dataQuery.Where(u => u.DepartmentId.ToUpper().Equals(modelSearch.DepartmentId.ToUpper()));
            //    query = query.Where(u => u.DepartmentId.ToUpper().Equals(modelSearch.DepartmentId.ToUpper()));
            //    listEmployeeQuery = listEmployeeQuery.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            //}

            //if (!string.IsNullOrEmpty(modelSearch.ResponsiblePersionName))
            //{
            //    query = query.Where(u => u.ResponsiblePersionName.ToUpper().Contains(modelSearch.ResponsiblePersionName.ToUpper()));
            //    if (query != null)
            //    {
            //        List<string> listProjectProđuctId = query.Select(a => a.ProjectProductId).ToList();
            //        dataQuery = dataQuery.Where(u => listProjectProđuctId.Contains(u.Id));
            //        if (dataQuery != null)
            //        {
            //            List<string> listProjectId = dataQuery.Select(a => a.ProjectId).ToList();
            //            queryProject = queryProject.Where(u => listProjectId.Contains(u.Id));
            //        }

            //    }
            //}
            var listId = modelSearch.listBase.Select(i => i.Id);
            if (modelSearch.listBase.Count > 0)
            {
                queryProject = queryProject.Where(u => listId.Contains(u.ProjectId));
                dataQuery = dataQuery.Where(u => listId.Contains(u.ProjectId));
                //query = query.Where(u => modelSearch.listBase.Contains(u.ProjectId));
            }

            List<ScheduleProjectResultModel> listPlan = new List<ScheduleProjectResultModel>();
            //listPlan = query.OrderBy(a => a.PlanName).ToList();
            List<ScheduleProjectResultModel> listDataQuery = new List<ScheduleProjectResultModel>();
            listDataQuery = dataQuery.ToList();

            var listModuleGroup = db.ModuleGroups.AsNoTracking().ToList();
            var listkTimeStandards = db.TaskTimeStandards.AsNoTracking().ToList();
            //var listEmployee = (from a in listEmployeeQuery
            //                    group a by new { a.Id, a.Name, a.DepartmentId } into g
            //                    select new EmployeePlan
            //                    {
            //                        Id = g.Key.Id,
            //                        Name = g.Key.Name,
            //                        DepartmentId = g.Key.DepartmentId,
            //                        DateMax = g.Max(i => i.EndDate) != null ? g.Max(i => i.EndDate) : DateTime.Now,
            //                        DateMaxMax = g.Max(i => i.EndDate) != null ? g.Max(i => i.EndDate) : DateTime.Now,
            //                    }).OrderBy(i => i.DateMax).ToList();
            //foreach (var items in modelSearch.listBase.OrderBy(i => i.Index))
            //{
            //    var listPlanCheck = query.Where(i => i.ProjectId.Equals(items.Id)).OrderBy(i => i.PlanName).ToList();
            //    listEmployee = listEmployee.OrderBy(i => i.DateMax).ToList();
            //    foreach (var item in listPlanCheck)
            //    {
            //        int index = 1;
            //        var listPlanUpdate = listPlan.ToList();
            //        var data = GetPlanByEmployee(index, listEmployee, item, listModuleGroup, listkTimeStandards);
            //        item.ResponsiblePersion = data.Data.Id;
            //        item.ResponsiblePersionName = data.Data.ResponsiblePersionName;
            //        item.StartDate = data.Data.StartDate;
            //        item.EndDate = data.Data.EndDate;
            //        listEmployee = data.ListEmployee;
            //        var check = listEmployee.Where(i => i.DateMax.Value.Subtract(i.DateMaxMax.Value).Days < 7).ToList();
            //        if (check.Count <= 0)
            //        {
            //            foreach (var ite in listEmployee)
            //            {
            //                ite.DateMaxMax.Value.AddDays(7);
            //            }
            //        }
            //    }
            //    listPlan.AddRange(listPlanCheck);
            //    foreach (var item in listEmployee)
            //    {
            //        item.DateMaxMax = item.DateMax.Value;
            //    }
            //}

            List<ScheduleProjectResultModel> listProjectProduct = new List<ScheduleProjectResultModel>();

            foreach (var item in listDataQuery)
            {
                if (item.KickOffDate.HasValue)
                {
                    item.KickOffDateView = DateTimeHelper.ToStringDDMMYY(item.KickOffDate.Value);
                }
                if (item.DesignFinishDate.HasValue)
                {
                    item.DesignFinishDateView = DateTimeHelper.ToStringDDMMYY(item.DesignFinishDate.Value);
                }
                if (item.MakeFinishDate.HasValue)
                {
                    item.MakeFinishDateView = DateTimeHelper.ToStringDDMMYY(item.MakeFinishDate.Value);
                }
                if (item.DeliveryDate.HasValue)
                {
                    item.DeliveryDateView = DateTimeHelper.ToStringDDMMYY(item.DeliveryDate.Value);
                }
                if (item.TransferDate.HasValue)
                {
                    item.TransferDateView = DateTimeHelper.ToStringDDMMYY(item.TransferDate.Value);
                }
                if (item.ExpectedDesignFinishDate.HasValue)
                {
                    item.ExpectedDesignFinishDateView = DateTimeHelper.ToStringDDMMYY(item.ExpectedDesignFinishDate.Value);
                }
                if (item.ExpectedMakeFinishDate.HasValue)
                {
                    item.ExpectedMakeFinishDateView = DateTimeHelper.ToStringDDMMYY(item.ExpectedMakeFinishDate.Value);
                }
                if (item.ExpectedTransferDate.HasValue)
                {
                    item.ExpectedTransferDateView = DateTimeHelper.ToStringDDMMYY(item.ExpectedTransferDate.Value);
                }
            }

            foreach (var plan in listPlan)
            {
                if (plan.EndDate.HasValue)
                {
                    plan.EndDateView = DateTimeHelper.ToStringDDMMYY(plan.EndDate.Value);

                }
                if (plan.StartDate.HasValue)
                {
                    plan.StartDateView = DateTimeHelper.ToStringDDMMYY(plan.StartDate.Value);
                }
                if (plan.RealStartDate.HasValue)
                {
                    plan.RealStartDateView = DateTimeHelper.ToStringDDMMYY(plan.RealStartDate.Value);

                }
                if (plan.RealEndDate.HasValue)
                {
                    plan.RealEndDateView = DateTimeHelper.ToStringDDMMYY(plan.RealEndDate.Value);
                }
            }

            var listProject = queryProject.ToList();
            foreach (var item in listProject)
            {
                var index = modelSearch.listBase.FirstOrDefault(i => i.Id.Equals(item.Id));
                var maxDate = listPlan.Where(i => i.ProjectId.Equals(item.Id) && i.EndDate.HasValue).Max(i => i.EndDate);
                var minDate = listPlan.Where(i => i.ProjectId.Equals(item.Id) && i.StartDate.HasValue).Min(i => i.StartDate);
                if (index != null)
                {
                    item.Index = index.Index;
                    item.EndDateView = maxDate.HasValue ? DateTimeHelper.ToStringDDMMYY(maxDate.Value) : null;
                    item.StartDateView = minDate.HasValue ? DateTimeHelper.ToStringDDMMYY(minDate.Value) : null;
                }
            }

            listProjectProduct.AddRange(listDataQuery);
            listProjectProduct.AddRange(listProject.OrderBy(i => i.Index));
            listProjectProduct.AddRange(listPlan);

            var listProjectProductParent = listProjectProduct.Where(a => string.IsNullOrEmpty(a.ParentId) /*&& a.DepartmentId.Equals(modelSearch.DepartmentId)*/).ToList();
            List<ScheduleProjectResultModel> listResult = new List<ScheduleProjectResultModel>();
            List<ScheduleProjectResultModel> listChild = new List<ScheduleProjectResultModel>();

            foreach (var parent in listProjectProductParent)
            {
                listChild = GetScheduleProjectChild(parent.Id, listProjectProduct);

                listResult.Add(parent);

                listResult.AddRange(listChild);
            }

            return new { listResult };
        }

        private List<ScheduleProjectResultModel> GetScheduleProjectChild(string parentId,
          List<ScheduleProjectResultModel> listSchedulePrject)
        {
            List<ScheduleProjectResultModel> listResult = new List<ScheduleProjectResultModel>();
            var listChild = listSchedulePrject.Where(r => parentId.Equals(r.ParentId)).ToList();

            List<ScheduleProjectResultModel> listChildChild = new List<ScheduleProjectResultModel>();
            foreach (var child in listChild)
            {
                listChildChild = GetScheduleProjectChild(child.Id, listSchedulePrject);

                listResult.Add(child);

                listResult.AddRange(listChildChild);
            }

            return listResult;
        }

        private GetEmployeePlan GetPlanByEmployee(int index, List<EmployeePlan> listEmployee, ScheduleProjectResultModel model, List<ModuleGroup> listModuleGroup, List<TaskTimeStandard> listkTimeStandards)
        {
            string moduleGroupId = string.Empty;
            var module = db.Modules.AsNoTracking().FirstOrDefault(i => i.Id.Equals(model.ModuleId));
            if (module != null)
            {
                moduleGroupId = module.ModuleGroupId;
            }

            foreach (var item in listEmployee)
            {
                var day = item.DateMax.Value.AddDays(1);
                TimeSpan timeSpan = item.DateMax.Value.Subtract(item.DateMaxMax.Value);
                var taskTimeStandards = listkTimeStandards.FirstOrDefault(i => i.EmployeeId.Equals(item.Id) && model.TaskId.Equals(i.TaskId) && moduleGroupId.Equals(i.ModuleGroupId));
                if (day > model.KickOffDate && timeSpan.Days < 7 && taskTimeStandards != null && taskTimeStandards.TimeStandard > 0)
                {
                    item.Index = index;
                    model.ResponsiblePersion = item.Id;
                    model.ResponsiblePersionName = item.Name;
                    model.StartDate = day;
                    model.EndDate = day.AddDays((double)(taskTimeStandards.TimeStandard / 8));
                    item.DateMax = model.EndDate;
                    break;
                }
            }
            return new GetEmployeePlan
            {
                Data = model,
                ListEmployee = listEmployee
            };
        }
    }
}
