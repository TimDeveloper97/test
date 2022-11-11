using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EMSST
{
    public class EMSST1200UserInfoModel
    {
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeId { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public int IsDisable { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string HomeUrl { get; set; }
        public int Status { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }

    }
}
