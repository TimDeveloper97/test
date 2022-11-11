using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Product
{
    public class ProductResultModel
    {
        public string Id { get; set; }
        public string ProductGroupId { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductGroupCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public int CurentVersion { get; set; }
        public int ProcedureTime { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool IsManualExist { get; set; }
        public bool IsQuoteExist { get; set; }
        public bool IsPracticeExist { get; set; }
        public bool IsManualMaintenance { get; set; }
        public bool IsCatalog { get; set; }
        public bool IsLayoutExist { get; set; }
        public bool IsMaterialExist { get; set; }
        public decimal Pricing { get; set; }
        public string status1 { get; set; }
        public string status2 { get; set; }
        public int IsEnought { get; set; }
        public bool? IsEnoughtSearch { get; set; }
        public int Quantity { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsTestResult { get; set; }
        public int ErrorCount { get; set; }
        public bool IsSendSale { get; set; }
        public DateTime? SyncDate { get; set; }
    }
}
