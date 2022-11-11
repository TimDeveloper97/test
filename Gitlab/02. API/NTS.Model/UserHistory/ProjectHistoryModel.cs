using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectHistory
{
    public class ProjectHistoryModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string Status { get; set; }
        public string Parameter { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> KickOffDate { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public decimal Price { get; set; }
        public decimal DesignPrice { get; set; }
        public string WarehouseCode { get; set; }
        public string CustomerFinalId { get; set; }
        public decimal SaleNoVAT { get; set; }
        public decimal FCMPrice { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
