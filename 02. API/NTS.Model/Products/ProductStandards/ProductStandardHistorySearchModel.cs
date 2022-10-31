using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandards
{
    public class ProductStandardHistorySearchModel: SearchCommonModel
    {
        public string Id { get; set; }
        public string ProductStandardId { get; set; }
        public string Version { get; set; }
    }
}
