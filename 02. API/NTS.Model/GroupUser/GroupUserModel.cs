using System;
using System.Collections.Generic;
using System.Linq;

namespace NTS.Model.GroupUser
{
    public class GroupUserModel : BaseModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DepartmentId { get; set; }

        public string IsDisable { get; set; }

        public string HomeURL { get; set; }
        public string SBUId { get; set; }

        public string Description { get; set; }
        public List<PermissionModel> ListPermission{ get; set; }
        public bool IsUpdateUser { get; set; }
        public GroupUserModel()
        {
            ListPermission = new List<PermissionModel>();
        }
    }
}