using NTS.Model.Repositories;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Business.Reports
{
    public class Region
    {
        public string Code { get; set; }
        public decimal Sale { get; set; }
        public int Number { get; set; }
    }

    public class KeyValue
    {
        public string Code { get; set; }
        public decimal Value { get; set; }
    }

    public class Sale
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Reality { get; set; }
        public decimal Target { get; set; }
    }

    public class ReportSaleBussiness
    {
        private readonly QLTKEntities db = new QLTKEntities();

        #region Region
        public List<Region> SalesTargetRegion(DateTime f, DateTime t)
        {
            var dataquery = (from customer in db.Customers.AsNoTracking()
                             join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerFinalId
                             join province in db.Provinces.AsNoTracking() on customer.ProvinceId equals province.Id
                             where project.DateFrom >= f && project.DateTo <= t
                             group project by province.Id into table

                             select new Region
                             {
                                 Code = table.Key,
                                 Sale = table.Sum(i => i.SaleNoVAT),
                                 Number = table.Count(),
                             }
                             ).AsQueryable();

            return dataquery.ToList();
        }

        public List<Region> SalesRealityByRegion(DateTime f, DateTime t)
        {
            var dataquery = (from customer in db.Customers.AsNoTracking()
                             join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                             join province in db.Provinces.AsNoTracking() on customer.ProvinceId equals province.Id
                             where project.DateFrom >= f && project.DateTo <= t
                             group project by province.Id into table

                             select new Region
                             {
                                 Code = table.Key,
                                 Sale = table.Sum(i => i.SaleNoVAT),
                                 Number = table.Count(),
                             }
                             ).AsQueryable();

            return dataquery.ToList();
        }


        #endregion

        #region Job - Application - Industry
        public List<Sale> SalesJob(DateTime f, DateTime t)
        {
            var target = (from saletarget in db.SaleTargetments.AsNoTracking()
                             join job in db.Jobs.AsNoTracking() on saletarget.DomainId equals job.Id
                             where saletarget.CreateDate >= f && saletarget.PlanContractDate <= t
                             group saletarget by job.Code into table

                             select new KeyValue
                             {
                                 Code = table.Key,
                                 Value = table.Sum(i => i.SaleTarget ?? 0),
                             }
                             ).AsQueryable().ToList();

            var reality = (from project in db.Projects.AsNoTracking()
                             join projectsolution in db.ProjectSolutions.AsNoTracking() on project.Id equals projectsolution.ProjectId
                             join solution in db.Solutions.AsNoTracking() on projectsolution.SolutionId equals solution.Id
                             join job in db.Jobs.AsNoTracking() on solution.JobId equals job.Id
                             where project.DateFrom >= f && project.DateTo <= t
                             group project by job.Code into table

                             select new KeyValue
                             {
                                 Code = table.Key,
                                 Value = table.Sum(i => i.SaleNoVAT),
                             }
                             ).AsQueryable().ToList();

            var fulljoin = FullJoin(reality, target);

            return fulljoin;
        }

        /// <summary>
        /// API not finish
        /// </summary>
        /// <param name="f"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<Sale> SalesIndustry(DateTime f, DateTime t)
        {
            var target = (from saletarget in db.SaleTargetments.AsNoTracking()
                             join industry in db.Industries.AsNoTracking() on saletarget.IndustryId equals industry.Id
                             where saletarget.CreateDate >= f && saletarget.PlanContractDate <= t
                             group saletarget by industry.Code into table

                             select new KeyValue
                             {
                                 Code = table.Key,
                                 Value = table.Sum(i => i.SaleTarget ?? 0),
                             }
                             ).AsQueryable();

            var reality = new List<KeyValue>();

            var fulljoin = FullJoin(reality, target.ToList());

            return fulljoin;
        }

        public List<Sale> SalesApplication(DateTime f, DateTime t)
        {
            var target = (from saletarget in db.SaleTargetments.AsNoTracking()
                             join application in db.Applications.AsNoTracking() on saletarget.ApplicationTypeId equals application.Id
                             where saletarget.CreateDate >= f && saletarget.PlanContractDate <= t
                             group saletarget by application.Code into table

                             select new KeyValue
                             {
                                 Code = table.Key,
                                 Value = table.Sum(i => i.SaleTarget ?? 0),
                             }
                             ).AsQueryable().ToList();

            var reality = (from project in db.Projects.AsNoTracking()
                             join projectsolution in db.ProjectSolutions.AsNoTracking() on project.Id equals projectsolution.ProjectId
                             join solution in db.Solutions.AsNoTracking() on projectsolution.SolutionId equals solution.Id
                             join application in db.Applications.AsNoTracking() on solution.ApplicationId equals application.Id
                             where project.DateFrom >= f && project.DateTo <= t
                             group project by application.Code into table

                             select new KeyValue
                             {
                                 Code = table.Key,
                                 Value = table.Sum(i => i.SaleNoVAT),
                             }
                             ).AsQueryable().ToList();

            var fulljoin = FullJoin(reality, target);

            return fulljoin;
        }

        #endregion

        #region NV, PB kinh doanh
        public List<string> Departments()
        {
            var result = from department in db.Departments
                         select department.Code;

            return result.ToList();
        }

        public List<string> Employees(string code)
        {
            var result = from Employee in db.Employees
                         where Employee.DepartmentId == code
                         select Employee.Code;

            return result.ToList();
        }

        public List<Sale> SalesEmployeeByDepartment(DateTime f, DateTime t, string codeDepartment)
        {
            var target = from saletargetment in db.SaleTargetments.AsNoTracking()
                         join employee in db.Employees.AsNoTracking() on saletargetment.EmployeeId equals employee.Id
                         where saletargetment.Year >= f.Year && saletargetment.SaleTarget <= t.Year && saletargetment.DepartmentId == codeDepartment
                         group saletargetment by employee.Code into table

                         select new KeyValue
                         {
                             Code = table.Key,
                             Value = table.Sum(i => i.SaleTarget) ?? 0,
                         };

            var reality = (from customer in db.Customers.AsNoTracking()
                           join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                           join employee in db.Employees.AsNoTracking() on customer.EmployeeId equals employee.Id
                           where project.DateFrom >= f && project.DateTo <= t
                           group project by employee.Code into table

                           select new KeyValue
                           {
                               Code = table.Key,
                               Value = table.Sum(i => i.SaleNoVAT),
                           }
                             ).AsQueryable();

            var fulljoin = FullJoin(reality.ToList(), target.ToList());

            return fulljoin;
        }

        public List<Sale> SalesEmployeeByDepartment(DateTime f, DateTime t, string codeDepartment, List<string> codeEmployees)
        {
            var fullemployee = SalesEmployeeByDepartment(f, t, codeDepartment);

            var result = from full in fullemployee
                         join code in codeEmployees on full.Code equals code
                         
                         select full;
            return result.ToList();
        }

        public List<Sale> SalesEmployee(DateTime f, DateTime t)
        {
            var target = from saletargetment in db.SaleTargetments.AsNoTracking()
                         join employee in db.Employees.AsNoTracking() on saletargetment.EmployeeId equals employee.Id
                         where saletargetment.Year >= f.Year && saletargetment.SaleTarget <= t.Year
                         group saletargetment by employee.Code into table

                         select new KeyValue
                         {
                             Code = table.Key,
                             Value = table.Sum(i => i.SaleTarget)??0,
                         };

            var reality = (from customer in db.Customers.AsNoTracking()
                             join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                             join employee in db.Employees.AsNoTracking() on customer.EmployeeId equals employee.Id
                             where project.DateFrom >= f && project.DateTo <= t
                             group project by employee.Code into table

                             select new KeyValue
                             {
                                 Code = table.Key,
                                 Value = table.Sum(i => i.SaleNoVAT),
                             }
                             ).AsQueryable();

            var fulljoin = FullJoin(reality.ToList(), target.ToList());

            return fulljoin;
        }

        public List<Sale> SalesDepartment(DateTime f, DateTime t)
        {
            var target = from saletargetment in db.SaleTargetments.AsNoTracking()
                         join department in db.Departments.AsNoTracking() on saletargetment.DepartmentId equals department.Id
                         where saletargetment.Year >= f.Year && saletargetment.SaleTarget <= t.Year
                         group saletargetment by department.Code into table

                         select new KeyValue
                         {
                             Code = table.Key,
                             Value = table.Sum(i => i.SaleTarget) ?? 0,
                         };

            var reality = (from customer in db.Customers.AsNoTracking()
                             join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                             join employee in db.Employees.AsNoTracking() on customer.EmployeeId equals employee.Id
                             join department in db.Departments.AsNoTracking() on employee.DepartmentId equals department.Id
                             where project.DateFrom >= f && project.DateTo <= t
                             group project by department.Code into table

                             select new KeyValue
                             {
                                 Code = table.Key,
                                 Value = table.Sum(i => i.SaleNoVAT),
                             }
                             ).AsQueryable();

            var fulljoin = FullJoin(reality.ToList(), target.ToList());

            return fulljoin;
        }
        #endregion

        private List<Sale> FullJoin(List<KeyValue> tfirst, List<KeyValue> tlast)
        {
            var leftJoin = from first in tfirst
                           join last in tlast on first.Code equals last.Code into set
                           from last in set.DefaultIfEmpty()
                           select new
                           {
                               Code = first.Code,
                               Reality = first.Value,
                               Target = last != null ? last.Value : 0,
                           };

            var rightJoin = from last in tlast
                            join first in tfirst on last.Code equals first.Code into set
                            from first in set.DefaultIfEmpty()
                            select new
                            {
                                Code = last.Code,
                                Reality = first != null ? first.Value : 0,
                                Target = last.Value,
                            };

            var fullJoin = leftJoin.Union(rightJoin);

            var joinName = fullJoin.Join(db.Jobs, x => x.Code, y => y.Code, (x, y) =>
            new Sale
            {
                Code = x.Code,
                Name = y.Name,
                Reality = x.Reality,
                Target = x.Target,
            }).OrderBy(x => x.Reality);

            return joinName.ToList();
        }

    }
}
