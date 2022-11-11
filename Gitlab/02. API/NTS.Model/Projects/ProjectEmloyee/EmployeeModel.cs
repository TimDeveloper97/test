using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.ProjectEmloyee
{
    public class EmployeeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public string RoleId { get; set; }
    }
}
