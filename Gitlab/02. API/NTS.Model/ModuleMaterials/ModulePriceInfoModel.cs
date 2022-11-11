using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleMaterials
{
   public class ModulePriceInfoModel
    {
        public string ModuleId { get; set; }
        public decimal Price { get; set; }
        public bool IsNoPrice { get; set; }
        public bool IsModuleExist { get; set; }
    }
}
