using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class SaleProductExportDetailModel
    {
        public string Id { get; set; }
        public string SaleProductId { get; set; }
        public string SaleProductExportId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Inventory { get; set; }
    }
}
