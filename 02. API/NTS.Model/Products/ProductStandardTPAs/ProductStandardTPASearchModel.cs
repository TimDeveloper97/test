using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAs
{
    public class ProductStandardTPASearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Manufacture { get; set; }
        public string ProductStandardTPATypeId { get; set; }
        public string Supplier { get; set; }
        public bool IsExport { get; set; }
        public bool IsSendSale { get; set; }
        public bool? IsCOCQ { get; set; }
    }
}
