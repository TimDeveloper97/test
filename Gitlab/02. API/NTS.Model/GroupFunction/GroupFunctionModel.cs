using NTS.Model.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GroupFunction
{
    public class GroupFunctionModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int PermissionTotal { get; set; }
        public int CheckCount { get; set; }
        public bool IsChecked { get; set; }
        public List<PermissionsModel> Permissions { set; get; }

        public GroupFunctionModel()
        {
            Permissions = new List<PermissionsModel>();
        }
    }
}
