using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectEmloyee
{
    public class ProjectEmployeeGroupModel
    {
        public string Code { get; set; }
        public string EmployeeId { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string RoleName { get; set; }
        public int Evaluate { get; set; }
        public string DepartmentName { get; set; }
        public string JobDescription { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Status { get; set; }
        public int TotalItem { get; set; }
        public string StatusProject { get; set; }
        public List<ProjectEmployeeGroupModel> ListProjectEmployeeGroup { get; set; }
    }
}
