using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectEmloyee
{
    public class ProjectEmployeeModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public int Type { get; set; }
        public string ProjectId { get; set; }
        public string RoleId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string JobDescription { get; set; }
        public int Evaluate { get; set; }
        public bool Status { get; set; }
        public decimal Subsidy { get; set; }
        public DateTime? subsidyStartTime { get; set; }
        public DateTime? subsidyEndTime { get; set; }
    }
}
