using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialPartModel: BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public int Qty { get; set; }
        public string MaterialItemId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }

        public string ModuleGroupName { get; set; }
    }
}
