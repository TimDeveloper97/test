using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
    public class SaleProductTypeModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ParentId { get; set; }
        public decimal EXWRate { get; set; }
        public decimal PublicRate { get; set; }
        public int Index { get; set; }
        public string Note { get; set; }
        public string SBUId { get; set; }

        public SaleProductTypeModel()
        {
        }
    }
}
