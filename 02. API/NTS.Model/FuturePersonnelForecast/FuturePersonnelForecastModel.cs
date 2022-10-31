using NTS.Model.Employees;
using NTS.Model.ProjectProducts;
using NTS.Model.Task;
using NTS.Model.TaskTimeStandardModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FuturePersonnelForecast
{
    public class FuturePersonnelForecastModel
    {
        public string Id { get; set; }
        public string ModuleGroupId { get; set; }
        public string ProjectProductId { get; set; }
        public string ProjectProductName { get; set; }
        public string ModulesId { get; set; }
        public string TaskId { get; set; }
        public string TaskTimeStandardId { get; set; }
        public string ProductId { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TaskName { get; set; }
        public string SBUName { get; set; }
        public string SBUId { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public string DepartmantName { get; set; }
        public string ProjectName { get; set; }
        public string DepartmantId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int TimeStandard { get; set; }
        public int Type { get; set; }
        public double totalItem { get; set; }
        public double totalItemProduc { get; set; }
        public List<TasksModel> ListTask { get; set; }
        public List<ProjectProductsModel> ListProjectProduct { get; set; }
        public List<EmployeeModel> listEmployee { get; set; }
        public List<int> ListType { get; set; }
        public List<TaskStandardTypeModel> ListWorkType { get; set; }
        public List<ModuleGroupModel> ListModuleGroup { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
