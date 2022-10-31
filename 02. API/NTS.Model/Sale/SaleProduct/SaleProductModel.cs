using System;

namespace NTS.Model.Sale.SaleProduct
{
    public class SaleProductModel
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
        public string ImagePath { get; set; }
        public string ProductType { get; set; }
        public string ProductTypeName { get; set; }
        public bool Status { get; set; }
        public bool IsSync { get; set; }
        public decimal ExportQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public bool ExistSolution { get; set; }
        public bool ExistCatalog { get; set; }
        public bool ExistTrainingTechnique { get; set; }
        public bool ExistTrainingSale { get; set; }
        public bool ExistUserManual { get; set; }
        public bool ExistFixBug { get; set; }
        public string SourceId { get; set; }
        public string AvatarPath { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string SaleProductTypeId { get; set; }
    }
}
