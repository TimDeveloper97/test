using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.NSMaterialGroup
{
    public class NSMaterialGroupSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ManufactureId { get; set; }
    }
}
