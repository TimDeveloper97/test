using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MaterialBuyHistory
{
    public class MaterialBuyHistoryModel
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string MaterialId { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string PriceUnit { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        //lấy thêm thông tin khi xem giá
        public decimal Total { get; set; }
        public int DateCount { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
    }

    public class ImportMaterialBuyHistoryResult
    {
        public List<MaterialBuyHistoryModel> ListExist { get; set; }
        public string Message { get; set; }
        public ImportMaterialBuyHistoryResult()
        {
            ListExist = new List<MaterialBuyHistoryModel>();
        }
    }
}
