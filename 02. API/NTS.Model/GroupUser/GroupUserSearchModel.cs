using NTS.Model.Combobox;
using System;
using System.Linq;

namespace NTS.Model.GroupUser
{
    public class GroupUserSearchModel : SearchCommonModel
    {

        public string Name { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }

        public int? IsDisable { get; set; }

    }
}