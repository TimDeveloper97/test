using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.ProductCompareSource
{
    public class ModuleCompareSourceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ModuleGroupCode { get; set; }
        public decimal Pricing { get; set; }
        public string Specification { get; set; }
        public string ParentId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
