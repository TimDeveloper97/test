using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
    public class SaleProductDetailModel
    {
        public string Id { get; set; }
        public string EName { get; set; }
        public string VName { get; set; }
        public string Model { get; set; }
        public string GroupCode { get; set; }
        public string ChildGroupCode { get; set; }
        public string ManufactureId { get; set; }
        public string CountryName { get; set; }
        public string Specifications { get; set; }
        public string CustomerSpecifications { get; set; }
        public string SaleProductTypeId { get; set; }
        public Nullable<System.DateTime> SpecificationDate { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> MaterialPrice { get; set; }
        public Nullable<decimal> EXWTPAPrice { get; set; }
        public Nullable<System.DateTime> EXWTPADate { get; set; }
        public decimal PublicPrice { get; set; }
        public Nullable<System.DateTime> ExpireDateFrom { get; set; }
        public Nullable<System.DateTime> ExpireDateTo { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<decimal> Inventory { get; set; }
        public Nullable<System.DateTime> InventoryDate { get; set; }
        public Nullable<decimal> ExportQuantity { get; set; }
        public string ExportPerson { get; set; }
        public List<SaleProductMediaModel> ListImage { get; set; }
        public List<SaleProductMediaModel> ListMedia { get; set; }
        public List<string> ListJob { get; set; }
        public List<string> ListApp { get; set; }
        public List<string> ListAccessory { get; set; }
        public List<SaleProductDocumentModel> ListDocument { get; set; }
        public SaleProductDetailModel ()
        {
            ListImage = new List<SaleProductMediaModel>();
            ListMedia = new List<SaleProductMediaModel>();
            ListDocument = new List<SaleProductDocumentModel>();
        }
    }
}
