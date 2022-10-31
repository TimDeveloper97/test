using NTS.Model.Employees;
using NTS.Model.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashboardEmployee
{
    public class DashboardEmployeeModel : BaseModel
    {
        public double TotalItems { get; set; }
        public List<EmployeeModel> ListEmployee { get; set; }
        public List<PlanResultModel> ListPlan { get; set; }
        public List<ObjectEmployeeModel> ListDashboard { get; set; }
        public DashboardEmployeeModel()
        {
            ListDashboard = new List<ObjectEmployeeModel>();
            ListEmployee = new List<EmployeeModel>();
            ListPlan = new List<PlanResultModel>();
        }
    }
}
