using System;
using System.Linq;

namespace NTS.Model.GroupUser
{
    public class PermissionModel
    {
        public string Index { get; set; }
        public string FunctionId { get; set; }
        public string GroupFunctionId { get; set; }
        public string GroupFunctionName { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public bool Checked { get; set; }

    }
}