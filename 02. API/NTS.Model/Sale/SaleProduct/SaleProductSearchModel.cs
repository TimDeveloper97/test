using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
    public class SaleProductSearchModel: SearchCommonModel
    {
        /// <summary>
        /// Chủng loại sản phẩm
        /// </summary>
        public string SaleProductTypeId { get; set; }
        /// <summary>
        /// Tìm kiếm theo tên tiếng anh và tiên tiếng việt
        /// </summary>
        public string Name { get; set; }
        public string Model { get; set; }
        public string GroupCode { get; set; }
        public string ChildGroupCode { get; set; }
        public string ManufactureId { get; set; }
        public string ApplicationId { get; set; }
        public string JobId { get; set; }
        public string CountryName { get; set; }
        public string Specifications { get; set; }
        public Nullable<System.DateTime> SpecificationDate { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> MaterialPrice { get; set; }
        public Nullable<decimal> EXWTPAPrice { get; set; }
        public Nullable<System.DateTime> EXWTPADate { get; set; }
        public decimal? PublicPrice { get; set; }
        public Nullable<System.DateTime> ExpireDateFrom { get; set; }
        public Nullable<System.DateTime> ExpireDateTo { get; set; }
        public string DeliveryTime { get; set; }
        public Nullable<int> Inventory { get; set; }
        public Nullable<System.DateTime> InventoryDate { get; set; }
        public Nullable<int> ExportQuantity { get; set; }
        public bool IsExport { get; set; }
        public int MaterialPriceType { get; set; }
        public int InventoryType { get; set; }
        public int ExportQuantityType { get; set; }
        public int DeliveryDaysType { get; set; }
        public int PublicPriceType { get; set; }
        public int VATType { get; set; }
        public int EXWTPAPriceType { get; set; }
        public List<string> ListIdSelect { get; set; }
        public bool? IsSync { get; set; }
        public bool? Status { get; set; }
        public bool? DocStatus { get; set; }
        public DateTime? EXWTPADateTo { get; set; }
        public DateTime? EXWTPADateFrom { get; set; }
        public DateTime? ExpiredDateFromTo { get; set; }
        public DateTime? ExpiredDateFromFrom { get; set; }
        public DateTime? ExpiredDateToTo { get; set; }
        public DateTime? ExpiredDateToFrom { get; set; }
        public DateTime? InventoryDateTo { get; set; }
        public DateTime? InventoryDateFrom { get; set; }
        public SaleProductSearchModel ()
        {
            ListIdSelect = new List<string>();
        }
    }
}
