using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.Permission
{
    public class PermissionsModel
    {
        public string Id { get; set; }
        public string GroupFunctionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ScreenCode { get; set; }
        public bool IsChecked { get; set; }
    }
}
