using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class SaleProductModel
    {
        public string Id { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SaleProductTypeId { get; set; }
        public string SaleProductId { get; set; }
        public decimal ExportQuantity { get; set; }
        public decimal Inventory { get; set; }
        public decimal AvailableQuantity { get; set; }
        public decimal Quantity { get; set; }
        public int Type { get; set; }
        public string Specifications { get; set; }
        public string CountryName { get; set; }
        public string ManufactureId { get; set; }
        public decimal EXWTPAPrice { get; set; }
    }
}
