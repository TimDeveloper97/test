using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model
{
    public class UserEntity
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string HomeUrl { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsLogin { get; set; }
        public int Status { get; set; }
        public string Password { get; set; }
        public string ImageLink { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public string UnitId { get; set; }
        public int Level { get; set; }
        public string GroupId { get; set; }
        public string DoctorLocation { get; set; }
        public bool IsAdmin { get; set; }

        public string CustomerId { get; set; }

        public string ManageId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }

        public List<string> ListPermission { get; set; }
        public List<string> ListSaleGroups { get; set; }

        public string AuthorizeString { get; set; }
        public string AuthorizeItemString { get; set; }
        public UserEntity ListUser { get; set; }

        public string SecurityKey { get; set; }

        public UserEntity()
        {
            ListSaleGroups = new List<string>();
            ListPermission = new List<string>();
        }

    }
}
