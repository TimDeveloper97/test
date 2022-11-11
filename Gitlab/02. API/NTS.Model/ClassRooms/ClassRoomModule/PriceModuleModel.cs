using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomModule
{
    public class PriceModuleModel
    {
        public string ModuleId { get; set; }
        public Nullable<System.DateTime> LastBuyDate { get; set; }
        public Nullable<System.DateTime> InputPriceDate { get; set; }
        public decimal Pricing { get; set; }
        public decimal PriceHistory { get; set; }
        public int Quantity { get; set; }
    }
}
