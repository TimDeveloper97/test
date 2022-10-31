using System;
using System.Linq;

namespace NTS.Model.GroupUser
{
    public class GroupUserResultModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int IsDisable { get; set; }

        public string HomeURL { get; set; }

        public string Description { get; set; }
    }
}