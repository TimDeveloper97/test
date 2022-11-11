using NTS.Common;
using NTS.Model.CoefficientEmployee;
using NTS.Model.Combobox;
using NTS.Model.EmployeeCourse;
using NTS.Model.Employees;
using NTS.Model.EmployeeSkillDetails;
using NTS.Model.Error;
using NTS.Model.Plans;
using NTS.Model.QLTKGROUPMODUL;
using NTS.Model.ReportEmployeesPresent;
using NTS.Model.Repositories;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;

namespace QLTK.Business.ReportEmployeesPresent
{
    public class ReportEmployeesPresentBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();
        public ReportEmployeesPresentResultModel searchEmployees(ReportEmployeesPresentSearchModel modelSearch)
        {
            ReportEmployeesPresentResultModel model = new ReportEmployeesPresentResultModel();

            var listEmployee = (from a in db.Employees.AsNoTracking()
                                join d in db.Departments.AsNoTracking() on a.DepartmentId equals d.Id into ad
                                from da in ad.DefaultIfEmpty()
                                join e in db.SBUs.AsNoTracking() on da.SBUId equals e.Id into ae
                                from ea in ae.DefaultIfEmpty()
                                where a.Status == Constants.Employee_Status_Use
                                select new
                                {
                                    Id = a.Id,
                                    Code = a.Code,
                                    Name = a.Name,
                                    SBUId = ea.Id,
                                    DepartmentId = da.Id,
                                    WorkType = a.WorkTypeId,
                                }
                                ).AsQueryable();


            if (!string.IsNullOrEmpty(modelSearch.Code))
            {
                listEmployee = listEmployee.Where(u => u.Code.ToUpper().Contains(modelSearch.Code.ToUpper()) || u.Name.ToUpper().Contains(modelSearch.Code));
            }

            if (!string.IsNullOrEmpty(modelSearch.SBUId))
            {
                listEmployee = listEmployee.Where(u => u.SBUId.Equals(modelSearch.SBUId));
            }

            if (!string.IsNullOrEmpty(modelSearch.DepartmentId))
            {
                listEmployee = listEmployee.Where(u => u.DepartmentId.Equals(modelSearch.DepartmentId));
            }

            // tổng số nv đang làm việc
            model.Total_Employee_Status_Use = listEmployee.Count();

            //var listEmployTemp = listEmployee.ToList();
            var workTypes = db.WorkTypes.AsNoTracking().OrderBy(i => i.Name).ToList();
            decimal total = 0;
            foreach (var item in workTypes)
            {
                var work = listEmployee.Where(i => i.WorkType == item.Id).Count();
                total = item.Quantity - work;
                model.ListWorkType.Add(new WorkTypeEmployee
                {
                    WorkTypeName = item.Name,
                    EmployeePresent = work,
                    EmployeeIncomplete = total
                });
            }

            var list_employee_Header = (from a in listEmployee
                                        join b in db.EmployeeSkills.AsNoTracking() on a.Id equals b.EmployeeId into ab
                                        from ba in ab.DefaultIfEmpty()
                                        join c in db.WorkSkills.AsNoTracking() on ba != null ? ba.WorkSkillId : string.Empty equals c.Id into bac
                                        from cba in bac.DefaultIfEmpty()
                                        select new EmployeeHeader
                                        {
                                            Id = a.Id,
                                            EmployeeName = a.Name,
                                            WorkSkillId = cba != null ? cba.Id : string.Empty,
                                            Mark = ba != null ? ba.Mark : 0,
                                            Grade = ba != null ? ba.Grade : 0,
                                        }).ToList();

            var group = list_employee_Header.GroupBy(a => new { a.Id, a.EmployeeName, a.Mark, a.Grade }).Select(gy => new EmployeeHeader { Id = gy.Key.Id, EmployeeName = gy.Key.EmployeeName, Mark = gy.Key.Mark, Grade = gy.Key.Grade, ListWorkSkillId = gy.ToList() });
            model.ListEmployeeHeader = group.ToList();

            var list_skill = (from a in db.WorkSkills.AsNoTracking()
                              join b in db.EmployeeSkills.AsNoTracking() on a.Id equals b.WorkSkillId into ab
                              from ba in ab.DefaultIfEmpty()
                              select new SkillEmployeeModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Mark = ba != null ? ba.Mark : 0,
                                  Grade = ba != null ? ba.Grade : 0,
                                  EmployeeId = ba.EmployeeId
                              }).GroupBy(t => t.Id).Select(m => new SkillEmployeeModel
                              {
                                  Id = m.Key,
                                  Name = m.FirstOrDefault().Name,
                                  Mark = m.FirstOrDefault().Mark,
                                  Grade = m.FirstOrDefault().Grade,
                                  EmployeeId = m.FirstOrDefault().EmployeeId
                              }).ToList();

            model.ListSkillEmployee = list_skill;
            // số khóa học phải học theo từng nhóm, từng nv 

            var employee_course = (from a in listEmployee
                                   join b in db.EmployeeCourseTrainings.AsNoTracking() on a.Id equals b.EmployeeId
                                   join c in db.CourseTrainings.AsNoTracking() on b.CourseTrainingId equals c.Id
                                   join d in db.Courses.AsNoTracking() on c.CourseId equals d.Id
                                   select new EmployeeCourseModel
                                   {
                                       Id = a.Id,
                                       CourseId = d.Id,
                                       CourseName = d.Name,
                                       EmployeeId = a.Id,
                                       EmployeeName = a.Name,
                                       Rate = b.Rate,
                                       Status = d.Status,
                                   }).ToList();

            model.ListEmployeeCourse = employee_course;



            // số khóa học chưa đào tạo                      
            model.Total_Course_Not_Start = employee_course.Where(a => a.Status.Equals(Constants.Course_Training_Status_Not_Learn)).Count();

            // số khóa học đã trễ kế hoạch

            var Total_Cousrse_Delay = (from a in db.EmployeeCourseTrainings.AsNoTracking()
                                       join b in db.CourseTrainings.AsNoTracking() on a.CourseTrainingId equals b.Id
                                       where b.Status.Equals(Constants.Course_Training_Status_Not_Learn) && b.EndDate < DateTime.Now
                                       select new EmployeeCourseModel
                                       {
                                           Id = a.Id,
                                       }).AsQueryable();
            model.Total_Course_Delay = Total_Cousrse_Delay.Count();

            // số lỗi nv theo dự án, theo nhóm sp
            //số lỗi nv theo dự án
            if (!string.IsNullOrEmpty(modelSearch.ProjectId))
            {
                var employeeDoProject = (from a in listEmployee
                                         join b in db.Errors.AsNoTracking() on a.Id equals b.ErrorBy into ab
                                         from ba in ab.DefaultIfEmpty()
                                         where ba == null || (ba.ProjectId.Equals(modelSearch.ProjectId) && ba.Type == 1)
                                         //where modelSearch.ProjectId.Equals(ba != null ? ba.ProjectId : string.Empty)
                                         select new
                                         {
                                             Id = a.Id,
                                             Name = a.Name,
                                             ProjectId = ba != null ? ba.ProjectId : string.Empty,
                                             ErrorBy = ba != null ? ba.ErrorBy : string.Empty,
                                             PlanStartDate = ba != null ? ba.PlanStartDate : null,
                                         }).ToList();

                if (modelSearch.DateFrom.HasValue)
                {
                    employeeDoProject = employeeDoProject.Where(i => i.PlanStartDate >= modelSearch.DateFrom).ToList();
                }

                if (modelSearch.DateTo.HasValue)
                {
                    employeeDoProject = employeeDoProject.Where(i => i.PlanStartDate <= modelSearch.DateTo).ToList();
                }

                var grouped_em = employeeDoProject.GroupBy(t => new { t.Id, t.Name, t.ProjectId }).Select(g => new ErrorProjectByEmployee
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    ProjectId = g.Key.ProjectId,
                    Count = g.Count(a => !string.IsNullOrEmpty(a.ErrorBy))
                });

                var listgrouped_em = grouped_em.ToList();

                model.ListErrorByEmployee = listgrouped_em;



                if (!string.IsNullOrEmpty(modelSearch.ModuleGroupId))
                {
                    // Số lỗi nv theo nhóm sp
                    var error_employee_product = (from a in listEmployee
                                                  join d in db.Errors.AsNoTracking() on a.Id equals d.ErrorBy into ad
                                                  from da in ad.DefaultIfEmpty()
                                                  join c in db.Modules.AsNoTracking() on da != null ? da.ObjectId : string.Empty equals c.Id into dac
                                                  from acd in dac.DefaultIfEmpty()
                                                  join e in db.ModuleGroups.AsNoTracking() on acd != null ? acd.ModuleGroupId : string.Empty equals e.Id into acde
                                                  from eacd in acde.DefaultIfEmpty()
                                                  where da == null || (da.ProjectId.Equals(modelSearch.ProjectId) && da.Type == 1) && (eacd == null || eacd.Id.Equals(modelSearch.ModuleGroupId))
                                                  select new
                                                  {
                                                      Id = a.Id,
                                                      Name = a.Name,
                                                      ModuleGroupName = eacd != null ? eacd.Code : string.Empty,
                                                      ProjectId = da != null ? da.ProjectId : string.Empty,
                                                      ErrorBy = da != null ? da.ErrorBy : string.Empty,
                                                      PlanStartDate = da != null ? da.PlanStartDate : null,
                                                  }).ToList();

                    if (modelSearch.DateFrom.HasValue)
                    {
                        error_employee_product = error_employee_product.Where(i => i.PlanStartDate >= modelSearch.DateFrom).ToList();
                    }

                    if (modelSearch.DateTo.HasValue)
                    {
                        error_employee_product = error_employee_product.Where(i => i.PlanStartDate <= modelSearch.DateTo).ToList();
                    }

                    var listErrorproduct = error_employee_product;

                    var grByEmployeeProduct = error_employee_product.GroupBy(t => new { t.Id, t.ProjectId, t.Name, t.ModuleGroupName }).Select((g) => new EmployeeProduct
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        ProjectId = g.Key.ProjectId,
                        Count = g.Count(a => !string.IsNullOrEmpty(a.ErrorBy))
                    });

                    model.ListErrorEmployeeByProduct = grByEmployeeProduct.ToList();
                }
            }


            // năng suát nv làm việc theo tháng
            //var listPlanINMonth = (from a in db.Plans.AsNoTracking()
            //                       join b in db.ProjectProducts.AsNoTracking() on a.ProjectProductId equals b.Id
            //                       join c in db.Modules.AsNoTracking() on b.ModuleId equals c.Id
            //                       join d in db.ModuleGroups.AsNoTracking() on c.ModuleGroupId equals d.Id
            //                       //join e in db.TaskTimeStandards.AsNoTracking() on a.TaskId equals e.TaskId into ae
            //                       //from ea in ae.DefaultIfEmpty()
            //                       where a.RealEndDate.Value.Month == modelSearch.Date.Month && a.RealEndDate.Value.Year == modelSearch.Date.Year && a.Status == Constants.Plan_Status_Done
            //                       //&&(ea == null || (ea.ModuleGroupId.Equals(d.Id) && ea.EmployeeId.Equals(a.ResponsiblePersion)) )
            //                       select new PerformanceEmployee
            //                       {
            //                           PlanId = a.Id,
            //                           ModuleId = c.Id,
            //                           ModuleName = c.Name,
            //                           ModuleGroupId = d.Id,
            //                           ModuleGroupCode = d.Code,
            //                           ModuleGroupName = d.Name,
            //                           TaskId = a.TaskId,
            //                           ResponsiblePersion = a.ResponsiblePersion,
            //                           Status = a.Status,
            //                           ExecutionTime = a.ExecutionTime,
            //                           //TimeStandard = ea != null ? ea.TimeStandard : 0,
            //                       }).ToList();

            //var listTimeStandard = db.TaskTimeStandards.ToList();

            //var employeeTimeStand = (from a in listEmployee
            //                         join b in db.TaskTimeStandards on a.Id equals b.EmployeeId
            //                         //from ba in ab.DefaultIfEmpty()
            //                         select new
            //                         {
            //                             EmployeeId = a.Id,
            //                             EmployeeName = a.Name,
            //                             ModuleGroupId = b.ModuleGroupId,
            //                             TaskId = b.TaskId,
            //                             TimeStandard = b.TimeStandard,
            //                         }).ToList();

            var coefficientEmployees = db.CoefficientEmployees.AsNoTracking().ToList();
            var listEmployeePerformance = listEmployee.GroupBy(a => new { a.Id, a.Name }).Select(b => new Performance { Id = b.Key.Id, Name = b.Key.Name }).ToList();
            var listEmployeeByTimeStand = (from a in db.TaskModuleGroups.AsNoTracking()
                                           join b in db.TaskModuleGroupTimeStandards.AsNoTracking() on a.Id equals b.TaskModuleGroupId
                                           where b.Year == modelSearch.Date.Year
                                           select new
                                           {
                                               a.TaskId,
                                               a.ModuleGroupId,
                                               b.TimeStandard
                                           });

            foreach (var item in listEmployeePerformance)
            {
                //var temw = listPlanINMonth.Where(a => item.Id.Equals(a.ResponsiblePersion)).ToList();
                //var listEmployeeByTimeStand = employeeTimeStand.Where(a => item.Id.Equals(a.EmployeeId)).Select(b => new { b.EmployeeId, b.TimeStandard, b.TaskId, b.ModuleGroupId }).ToList();
                //var tam = (from a in temw
                //           join b in listEmployeeByTimeStand on a.TaskId equals b.TaskId
                //           join c in coefficientEmployees on a.ResponsiblePersion equals c.EmployeeId
                //           where a.ModuleGroupId.Equals(b.ModuleGroupId) && c.Month == modelSearch.Date.Month && c.Year == modelSearch.Date.Year
                //           select new
                //           {
                //               TaskId = a.TaskId,
                //               ExecutionTime = a.ExecutionTime,
                //               TimeStandard = b.TimeStandard,
                //               Coefficient = a.ExecutionTime > 0 && c.Coefficient > 0 ? Math.Round(b.TimeStandard / (a.ExecutionTime * c.Coefficient), 2) : 0
                //           }).ToList();

                //item.performance = tam.Sum(i => i.Coefficient);

                //model.ListPerformance.Add(item);
            }

            return model;
        }

        // lấy project theo pb và sbu
        public List<ProjectModel> GetCbbProjectBySBUId_DepartmentId(string SBUId, string DepartmentId)
        {
            List<ProjectModel> searchResult = new List<ProjectModel>();
            try
            {
                var ListProject = (from a in db.Projects.AsNoTracking()
                                   orderby a.Name ascending
                                   select new ProjectModel()
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       Code = a.Code,
                                       DepartmentId = a.DepartmentId,
                                       SBUId = a.SBUId,
                                   }).AsQueryable();
                if (!string.IsNullOrEmpty(SBUId) && SBUId != "null")
                {
                    ListProject = ListProject.Where(a => a.SBUId.Equals(SBUId));
                }
                if (!string.IsNullOrEmpty(DepartmentId) && DepartmentId != "null")
                {
                    ListProject = ListProject.Where(a => a.DepartmentId.Equals(DepartmentId));
                }

                searchResult = ListProject.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }
        // Lấy ra sp 
        public List<GroupProduct> GetGroupProducts(string ProjectId)
        {
            List<GroupProduct> searchResult = new List<GroupProduct>();

            try
            {
                var ListGroupProduct = (from a in db.ModuleGroups.AsNoTracking()
                                        join b in db.Modules.AsNoTracking() on a.Id equals b.ModuleGroupId
                                        join c in db.ProjectProducts.AsNoTracking() on b.Id equals c.ModuleId
                                        where c.ProjectId.Equals(ProjectId)
                                        orderby a.Name ascending
                                        select new GroupProduct()
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Code = a.Code,
                                        }).ToList();

                //List<GroupProduct> listResult = new List<GroupProduct>();
                var grouped = ListGroupProduct.GroupBy(t => new { t.Id, t.Name, t.Code }).Select(a => new GroupProduct { Id = a.Key.Id, Name = a.Key.Name, Code = a.Key.Code }).ToList();
                //var listParent = grouped.Where(r => string.IsNullOrEmpty(r.ParentId)).ToList();
                //bool isSearch = false;
                //int index = 1;

                //List<GroupProduct> listChild = new List<GroupProduct>();

                //foreach (var parent in listParent)
                //{
                //    isSearch = true;


                //    listChild = GetModuleGroupChild(parent.Id, grouped, index.ToString());
                //    if (isSearch || listChild.Count > 0)
                //    {
                //        parent.Index = index.ToString();
                //        listResult.Add(parent);
                //        index++;
                //    }

                //    listResult.AddRange(listChild);
                //}

                //foreach (var item in listResult)
                //{
                //    item.IndexView = item.Index + " | " + item.Code + " | " + item.Name;
                //}


                //searchResult = listResult.OrderBy(t => t.Code).ToList();
                searchResult = grouped;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }
            return searchResult;
        }

        //private List<GroupProduct> GetModuleGroupChild(string parentId,List<GroupProduct> listModuleGroup, string index)
        //{
        //    List<GroupProduct> listResult = new List<GroupProduct>();
        //    var listChild = listModuleGroup.Where(r => parentId.Equals(r.ParentId)).ToList();

        //    bool isSearch = false;
        //    int indexChild = 1;
        //    List<GroupProduct> listChildChild = new List<GroupProduct>();
        //    foreach (var child in listChild)
        //    {
        //        isSearch = true;

        //        listChildChild = GetModuleGroupChild(child.Id, listModuleGroup, index + "." + indexChild);
        //        if (isSearch || listChildChild.Count > 0)
        //        {
        //            child.Index = index + "." + indexChild;
        //            listResult.Add(child);
        //            indexChild++;
        //        }

        //        listResult.AddRange(listChildChild);
        //    }

        //    return listResult;
        //}

        public object CoefficientEmployees(ReportEmployeesPresentSearchModel model)
        {
            if (!model.Year.HasValue)
            {
                return new { };
            }
            var listData = (from a in db.Employees.AsNoTracking()
                            join b in db.Departments.AsNoTracking() on a.DepartmentId equals b.Id
                            join c in db.SBUs.AsNoTracking() on b.SBUId equals c.Id
                            where a.Status==Constants.Employee_Status_Use
                            orderby a.Name
                            select new EmployeeModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Code = a.Code,
                                DepartmentId = a.DepartmentId,
                                SBUID = c.Id
                            }).AsQueryable();
            if (!string.IsNullOrEmpty(model.SBUId))
            {
                listData = listData.Where(i => i.SBUID.Equals(model.SBUId));
            }
            if (!string.IsNullOrEmpty(model.DepartmentId))
            {
                listData = listData.Where(i => i.DepartmentId.Equals(model.DepartmentId));
            }

            List<CoefficientEmployeeResultModel> listCoefficientEmployee = new List<CoefficientEmployeeResultModel>();
            List<CoefficientModel> listDecimal;

            var listEmployee = listData.ToList();
            var listCoefficientEmployees = db.CoefficientEmployees.AsNoTracking().ToList();
            foreach (var item in listEmployee)
            {
                listDecimal = new List<CoefficientModel>();
                for (int a = 1; a <= 12; a++)
                {
                    var data = listCoefficientEmployees.FirstOrDefault(i => i.EmployeeId.Equals(item.Id) && i.Month == a && i.Year == model.Year);
                    if (data != null)
                    {
                        listDecimal.Add(new CoefficientModel
                        {
                            Coefficient = data.Coefficient
                        });
                    }
                    else
                    {
                        listDecimal.Add(new CoefficientModel
                        {
                            Coefficient = 0
                        });
                    }
                }
                listCoefficientEmployee.Add(new CoefficientEmployeeResultModel
                {
                    EmployeeId = item.Id,
                    EmployeeName = item.Name,
                    EmployeeCode = item.Code,
                    ListCoefficient = listDecimal
                });
            }

            return new
            {
                ListData = listCoefficientEmployee
            };
        }

        public void UpdateCoefficientEmployee(CoefficientEmployeeCreateModel model)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var listEmployeeId = model.ListData.Select(i => i.EmployeeId).ToList();
                    var listRemove = db.CoefficientEmployees.Where(i => i.Year == model.Year && listEmployeeId.Contains(i.EmployeeId)).ToList();

                    if (listRemove.Count > 0)
                    {
                        db.CoefficientEmployees.RemoveRange(listRemove);
                    }

                    List<CoefficientEmployee> listAdd = new List<CoefficientEmployee>();

                    int index = 0;
                    foreach (var item in model.ListData)
                    {
                        foreach (var items in item.ListCoefficient)
                        {
                            index++;
                            CoefficientEmployee coefficient = new CoefficientEmployee()
                            {
                                Id = Guid.NewGuid().ToString(),
                                EmployeeId = item.EmployeeId,
                                Month = index,
                                Year = model.Year,
                                Coefficient = items.Coefficient
                            };
                            listAdd.Add(coefficient);
                        }
                        index = 0;
                    }

                    db.CoefficientEmployees.AddRange(listAdd);
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
