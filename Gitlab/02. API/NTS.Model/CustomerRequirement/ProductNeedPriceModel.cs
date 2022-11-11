using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirement
{
    public class ProductNeedPriceModel
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ManufactureName { get; set; }
        public string Specifications { get; set; }
        public int ProductType { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
    }
}
