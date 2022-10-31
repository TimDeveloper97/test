using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EMSEP
{
    public class EMSEP1100EmployeeModel
    {
        
        public string Account { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string  UserStatus { get; set; }

        //employee
        public string EmployeeType { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string PositionId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }

        public string CustomerId { get; set; } 


        public DateTime? UpdateDate { get; set; }

        public string GroupId { get; set; }

        public List<EMSEP1100UserPermissionModel> listUserPermission;
        public EMSEP1100EmployeeModel()
        {
            listUserPermission = new List<EMSEP1100UserPermissionModel>(); 
        }

    }
}
