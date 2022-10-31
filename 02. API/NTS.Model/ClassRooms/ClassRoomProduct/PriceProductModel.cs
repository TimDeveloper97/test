using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomProduct
{
    public class PriceProductModel
    {
        public string Id { get; set; }
        public decimal Pricing { get; set; }
        public string ModuleId { get; set; }
        public int Quantity { get; set; }
    }
}
