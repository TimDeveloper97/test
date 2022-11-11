using NTS.Model.Employees;
using NTS.Model.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportEmployeesPresent
{
    public class ReportEmployeesPresentModel : BaseModel
    {
        public List<EmployeeModel> ListEmployee { get; set; }
        public List<PlanResultModel> ListPlan { get; set; }
        public int Total { get; set; }
        public ReportEmployeesPresentModel()
        {
            ListEmployee = new List<EmployeeModel>();
            ListPlan = new List<PlanResultModel>();
        }
    }
}
