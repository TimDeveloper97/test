using NTS.Model.ScheduleProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FuturePersonnelForecast
{
    public class EmployeePlan
    {
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public DateTime? DateMax { get; set; }
        public DateTime? DateMaxMax { get; set; }
    }

    public class GetEmployeePlan
    {
        public ScheduleProjectResultModel Data { get; set; }
        public List<EmployeePlan> ListEmployee { get; set; }
        public GetEmployeePlan()
        {
            ListEmployee = new List<EmployeePlan>();
            Data = new ScheduleProjectResultModel();
        }
    }
}
