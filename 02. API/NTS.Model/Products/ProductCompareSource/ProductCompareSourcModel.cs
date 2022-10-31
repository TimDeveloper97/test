using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductCompareSource
{
    public class ProductCompareSourcModel
    {
        public string Id { get; set; }
        public string EName { get; set; }
        public string VName { get; set; }
        public string Model { get; set; }
        public string GroupCode { get; set; }
        public string ChildGroupCode { get; set; }
        public string ChildGroupId { get; set; }
        public string ManufactureName { get; set; }
        public decimal PriceHistory { get; set; }
        public decimal SourceId { get; set; }
        public System.Nullable<DateTime> InputPriceDate { get; set; }

        public List<string> MaterialParameterValue { get; set; }
        public string ManufactureId { get; set; }
        public string CountryName { get; set; }
        public string Specifications { get; set; }
        public string CustomerSpecifications { get; set; }
        public Nullable<System.DateTime> SpecificationDate { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> MaterialPrice { get; set; }
        public Nullable<decimal> EXWTPAPrice { get; set; }
        public Nullable<System.DateTime> EXWTPADate { get; set; }
        public decimal PublicPrice { get; set; }
        public Nullable<System.DateTime> ExpireDateFrom { get; set; }
        public Nullable<System.DateTime> ExpireDateTo { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<int> Inventory { get; set; }
        public Nullable<System.DateTime> InventoryDate { get; set; }
        public Nullable<System.DateTime> LastBuyDate { get; set; }
        public Nullable<int> ExportQuantity { get; set; }
        public string ExportPerson { get; set; }
        public string ProductStandTPATypeName { get; set; }
        public int Type { get; set; }
    }
}
