using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
   public class ApplicationInfoModel
    {
        public string Id{ get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }

    public class CareeInfoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class MediaInfoModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime? CreatDate { get; set; }
        public decimal FileSize { get; set; }
        public int Type { get; set; }
    }
    public class DocumentInfoModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime CreatDate { get; set; }
        public decimal FileSize { get; set; }
        public int Type { get; set; }
    }
    public class AccessoryInfoModel
    {
        public string Id { get; set; }
        public string EName { get; set; }
        public string VName { get; set; }
        public string Model { get; set; }
        public string GroupCode { get; set; }
        public string ChildGroupCode { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureName { get; set; }
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
        public Nullable<decimal> Inventory { get; set; }
        public Nullable<System.DateTime> InventoryDate { get; set; }
        public Nullable<decimal> ExportQuantity { get; set; }
        public string ExportPerson { get; set; }
        public string ImagePath { get; set; }
        public string ProductType { get; set; }
        public string ProductTypeName { get; set; }
    }
}
