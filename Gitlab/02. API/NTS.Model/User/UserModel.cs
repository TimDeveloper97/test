using NTS.Model.GroupFunction;
using NTS.Model.GroupUser;
using NTS.Model.UserPermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.User
{
    public class UserModel : BaseModel
    {
        public string SecurityKey { get; set; }
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string GroupUserId { get; set; }
        public string UserName { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public string HomeURL { get; set; }
        public int IsDisable { get; set; }
        public bool IsLogin { get; set; }

        public List<UserPermissionModel> ListFunctionParameter { get; set; }
        public List<GroupFunctionModel> ListGroupFunction { get; set; }
        public UserModel()
        {
            ListGroupFunction = new List<GroupFunctionModel>();
            ListFunctionParameter = new List<UserPermissionModel>();
        }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }


    }
}
