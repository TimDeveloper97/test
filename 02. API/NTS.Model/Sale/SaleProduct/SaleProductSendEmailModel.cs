using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
    public class SaleProductSendEmailModel
    {
        public string Model { get; set; }
        public string EName { get; set; }
        public decimal Inventory { get; set; }
        public decimal ExportQuantity { get; set; }
        public string ExportPerson { get; set; }
    }
}
