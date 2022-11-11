using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleGroupProductStandard
{
    public class ModuleGroupProductStandardSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProductStandardId { get; set; }
        public string ModuleGroupId { get; set; }
    }
}
