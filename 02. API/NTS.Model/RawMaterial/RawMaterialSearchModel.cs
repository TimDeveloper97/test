using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RawMaterial
{
    public class RawMaterialSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string MaterialId { get; set; }
    }
}
