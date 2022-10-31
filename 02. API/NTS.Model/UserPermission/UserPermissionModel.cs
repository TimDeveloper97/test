using NTS.Model.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserPermission
{
    public class UserPermissionModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PermissionId { get; set; }
        public int dem { get; set; }
        public string Name { get; set; }
        public List<PermissionsModel> ListFunctionParameter { set; get; }
        public List<PermissionsModel> ListValue { set; get; }

        public UserPermissionModel()
        {
            ListValue = new List<PermissionsModel>();
        }
    }
}
