using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectEmloyee
{
    public class ProjectProductToEmployeeModel
    {
        public string Id { get; set; }
        public string ImagePath { get; set; }
        public string EmployeeId { get; set; }  
        public string EmployeeName { get; set; }
        public string EmployeePhone { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string JobDescription { get; set; }
        public DateTime TimeNow { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Subsidy { get; set; }
        public DateTime? SubsidyStartTime { get; set; }
        public DateTime? SubsidyEndTime { get; set; }
        public int Evaluate { get; set; }
        public bool Status { get; set; }
        public string Code { get; set; }
        public string ExternalEmployeeId { get; set; }
        public bool Checked { get; set; }

    }
}
