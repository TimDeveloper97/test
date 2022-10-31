using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAs
{
    public class ProductStandardTPATypeSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Index { get; set; }
        public string Note { get; set; }
    }
}
