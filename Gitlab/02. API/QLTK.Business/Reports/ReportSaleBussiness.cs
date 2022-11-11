using NTS.Model.Repositories;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RabbitMQ.Client.Framing.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

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

    public class DateCode : KeyValue
    {
        public DateTime Date { get; set; }
    }

    public class IdCode
    {
        public string Id { get; set; }
        public string Code { get; set; }
    }

    public class Sale
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Reality { get; set; }
        public decimal Target { get; set; }
    }

    public enum Mode
    {
        Month = 1,
        Quarterly,
        Year,
    }

    public class ModeSale
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<DateCode> Datas { get; set; }
    }

    public class ModeSaleSQL
    {
        public string Code { get; set; }
        public DateTime DateFrom { get; set; }
        public decimal SaleNoVAT { get; set; }
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
                          }).AsQueryable().ToList();

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

            var fulljoin = FullJoin(reality.ToList(), target.ToList());
            var joinName = fulljoin.Join(db.Jobs, x => x.Code, y => y.Code, (x, y) =>
            new Sale
            {
                Code = x.Code,
                Name = y.Name,
                Reality = x.Reality,
                Target = x.Target,
            }).OrderBy(x => x.Reality);

            return joinName.OrderByDescending(x => x.Reality).ToList();
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

            var joinName = fulljoin.Join(db.Industries, x => x.Code, y => y.Code, (x, y) =>
            new Sale
            {
                Code = x.Code,
                Name = y.Name,
                Reality = x.Reality,
                Target = x.Target,
            }).OrderBy(x => x.Reality);

            return joinName.OrderByDescending(x => x.Reality).ToList();
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
                             ).AsQueryable();

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
                             ).AsQueryable();

            var fulljoin = FullJoin(reality.ToList(), target.ToList());
            var joinName = fulljoin.Join(db.Applications, x => x.Code, y => y.Code, (x, y) =>
            new Sale
            {
                Code = x.Code,
                Name = y.Name,
                Reality = x.Reality,
                Target = x.Target,
            }).OrderBy(x => x.Reality);

            return joinName.OrderByDescending(x => x.Reality).ToList();
        }

        #endregion

        #region [Column] NV, PB kinh doanh
        public List<IdCode> Departments()
        {
            var result = (from department in db.Departments
                          select new IdCode
                          {
                              Id = department.Id,
                              Code = department.Code,
                          }).AsQueryable();

            return result.ToList();
        }

        public List<IdCode> Employees(string departmentId)
        {
            var result = (from Employee in db.Employees
                          where Employee.DepartmentId == departmentId
                          select new IdCode
                          {
                              Id = Employee.Id,
                              Code = Employee.Code,
                          }).AsQueryable();

            return result.ToList();
        }

        public List<Sale> SalesEmployeeByDepartment(DateTime f, DateTime t, string departmentId)
        {
            var target = from saletargetment in db.SaleTargetments.AsNoTracking()
                         join employee in db.Employees.AsNoTracking() on saletargetment.EmployeeId equals employee.Id
                         where saletargetment.Year >= f.Year && saletargetment.SaleTarget <= t.Year && saletargetment.DepartmentId == departmentId
                         group saletargetment by employee.Code into table

                         select new KeyValue
                         {
                             Code = table.Key,
                             Value = table.Sum(i => i.SaleTarget) ?? 0,
                         };

            var reality = (from customer in db.Customers.AsNoTracking()
                           join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                           join employee in db.Employees.AsNoTracking() on customer.EmployeeId equals employee.Id 
                           where project.DateFrom >= f && project.DateTo <= t && project.DepartmentId == departmentId
                           group project by employee.Code into table

                           select new KeyValue
                           {
                               Code = table.Key,
                               Value = table.Sum(i => i.SaleNoVAT),
                           }
                             ).AsQueryable();

            var fulljoin = FullJoin(reality.ToList(), target.ToList());
            var joinName = fulljoin.Join(db.Employees, x => x.Code, y => y.Code, (x, y) =>
            new Sale
            {
                Code = x.Code,
                Name = y.Name,
                Reality = x.Reality,
                Target = x.Target,
            }).OrderBy(x => x.Reality);

            return joinName.OrderByDescending(x => x.Reality).ToList();
        }

        public List<Sale> SalesEmployeeByDepartment(DateTime f, DateTime t, string departmentId, string[] employeesCode)
        {
            var fullemployee = SalesEmployeeByDepartment(f, t, departmentId);

            var i = fullemployee.ToList();
            var result = from full in fullemployee
                         join code in employeesCode on full.Code equals code

                         select full;
            var rightJoin = from last in employeesCode
                            join first in fullemployee on last equals first.Code into set
                            from first in set.DefaultIfEmpty()
                            select new Sale
                            {
                                Code = last,
                                Name = first?.Name,
                                Reality = first != null ? first.Reality : 0,
                                Target = first != null ? first.Target : 0,
                            };

            return rightJoin.OrderByDescending(x => x.Reality).ToList();
        }

        public List<Sale> SalesEmployee(DateTime f, DateTime t)
        {
            var target = (from saletargetment in db.SaleTargetments.AsNoTracking()
                          join employee in db.Employees.AsNoTracking() on saletargetment.EmployeeId equals employee.Id
                          where saletargetment.Year >= f.Year && saletargetment.Year <= t.Year
                          group saletargetment by employee.Code into table

                          select new KeyValue
                          {
                              Code = table.Key,
                              Value = table.Sum(i => i.SaleTarget) ?? 0,
                          }).AsQueryable();

            var reality = (from customer in db.Customers.AsNoTracking()
                           join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                           join employee in db.Employees.AsNoTracking() on customer.EmployeeId equals employee.Id
                           where project.DateFrom >= f && project.DateTo <= t
                           group project by employee.Code into table

                           select new KeyValue
                           {
                               Code = table.Key,
                               Value = table.Sum(i => i.SaleNoVAT),
                           }).AsQueryable();

            var fulljoin = FullJoin(reality.ToList(), target.ToList());
            var joinName = fulljoin.Join(db.Employees, x => x.Code, y => y.Code, (x, y) =>
            new Sale
            {
                Code = x.Code,
                Name = y.Name,
                Reality = x.Reality,
                Target = x.Target,
            }).OrderBy(x => x.Reality);

            return joinName.OrderByDescending(x => x.Reality).ToList();
        }

        public List<Sale> SalesDepartment(DateTime f, DateTime t)
        {
            var target = (from saletargetment in db.SaleTargetments.AsNoTracking()
                          join department in db.Departments.AsNoTracking() on saletargetment.DepartmentId equals department.Id
                          where saletargetment.Year >= f.Year && saletargetment.Year <= t.Year
                          group saletargetment by department.Code into table

                          select new KeyValue
                          {
                              Code = table.Key,
                              Value = table.Sum(i => i.SaleTarget) ?? 0,
                          }).AsQueryable();

            var tar = target.ToList();

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
                           }).AsQueryable();

            var rea = reality.ToList();

            var fulljoin = FullJoin(reality.ToList(), target.ToList());
            var joinName = fulljoin.Join(db.Departments, x => x.Code, y => y.Code, (x, y) =>
            new Sale
            {
                Code = x.Code,
                Name = y.Name,
                Reality = x.Reality,
                Target = x.Target,
            }).OrderBy(x => x.Reality);

            return joinName.OrderByDescending(x => x.Reality).ToList();
        }

        public List<Sale> SalesDepartment(DateTime f, DateTime t, string[] departmentsCode)
        {
            var fulldepartment = SalesDepartment(f, t);

            var i = fulldepartment.ToList();
            var result = from full in fulldepartment
                         join code in departmentsCode on full.Code equals code

                         select full;
            var rightJoin = from last in departmentsCode
                            join first in fulldepartment on last equals first.Code into set
                            from first in set.DefaultIfEmpty()
                            select new Sale
                            {
                                Code = last,
                                Name = first?.Name,
                                Reality = first != null ? first.Reality : 0,
                                Target = first != null ? first.Target : 0,
                            };

            return rightJoin.OrderByDescending(x => x.Reality).ToList();
        }
        #endregion

        #region [Line] NV, PB kinh doanh
        public List<ModeSale> SalesEmployeeByDepartment(DateTime f, DateTime t, string departmentId, string[] employeesCode, Mode mode)
        {
            var reality = (from customer in db.Customers.AsNoTracking()
                           join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                           join employee in db.Employees.AsNoTracking() on customer.EmployeeId equals employee.Id
                           where project.DateFrom >= f && project.DateFrom <= t && project.DepartmentId == departmentId
                           orderby project.DateFrom

                           select new ModeSaleSQL
                           {
                               Code = employee.Code,
                               DateFrom = project.DateFrom ?? DateTime.Now,
                               SaleNoVAT = project.SaleNoVAT,
                           }).AsQueryable();

            return SwitchMode(reality, f, t, employeesCode, mode).ToList();
        }

        public List<ModeSale> SalesDepartment(DateTime f, DateTime t, string[] departmentsCode, Mode mode)
        {
            var reality = (from customer in db.Customers.AsNoTracking()
                           join project in db.Projects.AsNoTracking() on customer.Id equals project.CustomerId
                           join employee in db.Employees.AsNoTracking() on customer.EmployeeId equals employee.Id
                           join department in db.Departments.AsNoTracking() on employee.DepartmentId equals department.Id
                           where project.DateFrom >= f && project.DateFrom <= t
                           orderby project.DateFrom

                           select new ModeSaleSQL
                           {
                               Code = department.Code,
                               DateFrom = project.DateFrom ?? DateTime.Now,
                               SaleNoVAT = project.SaleNoVAT,
                           }).AsQueryable();

            return SwitchMode(reality, f, t, departmentsCode, mode).ToList();
        }
        #endregion

        private IEnumerable<Sale> FullJoin(List<KeyValue> tfirst, List<KeyValue> tlast)
        {
            var leftJoin = from first in tfirst
                           join last in tlast on first.Code equals last.Code into set
                           from last in set.DefaultIfEmpty()
                           select new Sale
                           {
                               Code = first.Code,
                               Name = null,
                               Reality = first.Value,
                               Target = last != null ? last.Value : 0,
                           };

            var rightJoin = from last in tlast
                            join first in tfirst on last.Code equals first.Code into set
                            from first in set.DefaultIfEmpty()
                            select new Sale
                            {
                                Code = last.Code,
                                Name = null,
                                Reality = first != null ? first.Value : 0,
                                Target = last.Value,
                            };
            var fullJoin = leftJoin.Union(rightJoin);

            return fullJoin;
        }

        private IEnumerable<ModeSale> SwitchMode(IEnumerable<ModeSaleSQL> reality, DateTime f, DateTime t, string[] departmentsCode, Mode mode)
        {
            var modeSales = new List<ModeSale>();

            foreach (var code in departmentsCode)
            {
                DateTime currentDate = f;
                var item = reality.Where(x => x.Code == code).ToList();
                var m = new ModeSale
                {
                    Code = code,
                    Datas = new List<DateCode>(),
                };

                while (currentDate < t)
                {
                    currentDate = currentDate.AddMonths(1);
                    var month = item?.Where(x => x.DateFrom.Month == currentDate.Month && x.DateFrom.Year == currentDate.Year);

                    decimal money = 0;
                    if (month != null)
                    {
                        money = month.Sum(x => x.SaleNoVAT);
                    }

                    m.Datas.Add(new DateCode { Date = currentDate, Value = money });
                }

                modeSales.Add(m);
            }

            if (mode == Mode.Month)
            {
                foreach (var modeSale in modeSales)
                {
                    foreach (var data in modeSale.Datas)
                    {
                        data.Code = data.Date.ToString("MM/yyyy");
                    }
                    modeSale.Datas = modeSale.Datas.OrderBy(x => x.Date).ToList();
                }
            }
            else if (mode == Mode.Quarterly)
            {
                foreach (var modeSale in modeSales)
                {
                    var datas = new List<DateCode>();
                    datas = modeSale.Datas.GroupBy(x => x.Date.ToQuarterly())
                            .Select(x => new DateCode { Code = x.Key, Value = x.Sum(y => y.Value), Date = x.Key.ToDateTime() })
                            .OrderBy(x => x.Date).ToList();

                    modeSale.Datas = datas;
                }
            }
            else
            {
                foreach (var modeSale in modeSales)
                {
                    var datas = new List<DateCode>();
                    datas = modeSale.Datas.GroupBy(x => x.Date.Year)
                            .Select(x => new DateCode { Code = x.Key.ToString(), Value = x.Sum(y => y.Value), Date = new DateTime(x.Key, 1, 1) })
                            .OrderBy(x => x.Date).ToList();

                    modeSale.Datas = datas;
                }
            }

            return modeSales;
        }
    }

    static class Extention
    {
        public static string ToQuarterly(this DateTime date)
        {
            var month = date.Month;
            if (month >= 1 && month <= 3)
                return "Quy 1/" + date.Year;
            else if (month >= 4 && month <= 6)
                return "Quy 2/" + date.Year;
            else if (month >= 7 && month <= 9)
                return "Quy 3/" + date.Year;
            else 
                return "Quy 4/" + date.Year;
        }

        public static DateTime ToDateTime(this string quarterly)
        {
            var split = quarterly.Split('/');
            if (quarterly == "Quy 1") return new DateTime(int.Parse(split[1]), 1, 1);
            else if (quarterly == "Quy 2") return new DateTime(int.Parse(split[1]), 4, 1);
            else if (quarterly == "Quy 3") return new DateTime(int.Parse(split[1]), 7, 1);
            else return new DateTime(int.Parse(split[1]), 1, 1);
        }
    }
}
