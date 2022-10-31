using NTS.Model.ProductStandards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleProductStandard
{
    public class ModuleProductStandardModel : BaseModel
    {
        public string Id { get; set; }
        public string ProductStandardId { get; set; }
        public string ModuleId { get; set; }
        public List<ModuleProductStandardModel> ListProductStandard { get; set; }
        public List<string> List { get; set; }
        public string ModuleGroupId { get; set; }
    }
}
