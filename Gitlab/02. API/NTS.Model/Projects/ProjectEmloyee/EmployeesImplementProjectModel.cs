using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectEmloyee
{
    public class EmployeesImplementProjectModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ImagePath { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeImg { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhone { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string JobDescription { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public string ProjectId { get; set; }
        public int MaxLateTime { get; set; }
        public int SumInProgressPlan { get; set; }
        public int LateStatus { get; set; }
        public long TotalProduct { get; set; }
        public int LateProduct { get; set; }
        public int LatePlan { get; set; }

    }
}
