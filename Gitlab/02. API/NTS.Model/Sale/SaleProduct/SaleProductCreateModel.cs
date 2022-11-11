using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
    public class SaleProductCreateModel
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
        public Nullable<System.DateTime> SpecificationDate { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> MaterialPrice { get; set; }
        public Nullable<decimal> EXWTPAPrice { get; set; }
        public Nullable<System.DateTime> EXWTPADate { get; set; }
        public decimal PublicPrice { get; set; }
        public Nullable<System.DateTime> ExpireDateFrom { get; set; }
        public Nullable<System.DateTime> ExpireDateTo { get; set; }
        public string DeliveryTime { get; set; }
        public string SaleProductTypeId { get; set; }
        public List<string> SaleProductJobModels { get; set; }
        public List<string> SaleProductAppModels { get; set; }
        public List<SaleProductDocumentModel> SaleProductDocumnetModels { get; set; }
        public List<SaleProductMediaModel> SaleProductMediaModels { get; set; }
        public List<string> SaleProductAccessoryModels { get; set; }
        public List<string> SaleGroupProduct { get; set; }
        public SaleProductCreateModel ()
        {
            SaleProductMediaModels = new List<SaleProductMediaModel>();
            SaleProductDocumnetModels = new List<SaleProductDocumentModel>();
            SaleProductAccessoryModels=new List<string>();
            SaleGroupProduct = new List<string>();
            SaleProductJobModels = new List<string>();
            SaleProductAppModels = new List<string>();
        }
        
    }
    public class SaleProductMediaModel
    {
        public string Id { get; set; }
        public string SaleProductId { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal FileSize { get; set; }
        public int Type { get; set; }
    }

    public class SaleProductDocumentModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal FileSize { get; set; }
        public int Type { get; set; }
      
    }
}
