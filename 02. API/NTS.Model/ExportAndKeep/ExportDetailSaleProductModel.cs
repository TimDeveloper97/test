using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class ExportDetailSaleProductModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CreateBy { get; set; }
        public string Customer { get; set; }
        public decimal TotalProduct { get; set; }
        public decimal AvailableQuantity { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Status { get; set; }
    }
}
