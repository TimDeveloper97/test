using NTS.Model.ProjectAttch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Project
{
    public class ProjectResultModel: BaseModel
    {
        public string Id { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ManageId { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerTypeId { get; set; }
        public string CustomerType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Status { get; set; }
        public string Parameter { get; set; }
        public string Note { get; set; }
        public DateTime? search { get; set; }
        public bool IsExport { get; set; }
        public DateTime? KickOffDate { get; set; }
        public DateTime DateMax { get; set; }
        public decimal TotalTime { get; set; }
        public decimal TakeTime { get; set; }
        public decimal Symmetrical { get; set; }
        public bool IsSolution { get; set; }
        public bool IsTransfer { get; set; }
        public decimal SaleNoVAT { get; set; }
        public decimal Price { get; set; }
        public decimal DesignPrice { get; set; }
        public string CustomerFinalId { get; set; }
        public decimal FCMPrice { get; set; }
        public bool IsBadDebt { get; set; }
        public DateTime? BadDebtDate { get; set; }
        public int ErorrTotalDone { get; set; }
        public int ErorrTotal { get; set; }
        public int IssueTotalDone { get; set; }
        public int IssueTotal { get; set; }
        public int Priority { get; set; }
        public string StatusProject { get; set; }
        /// <summary>
        /// Mã kho
        /// </summary>
        public string WarehouseCode { get; set; }
        public int DocumentStatus { get; set; }
        public int? PaymentStatus { get; set; }
        public string Progress { get; set; }

        public int Type { get; set; }
        public IQueryable<bool> IsImages { get; set; }
        public IQueryable<bool> IsVideos { get; set; }
        public IQueryable<bool> IsCatalogs { get; set; }
        public IQueryable<bool> IsWeb { get; set; }
        public IQueryable<bool> IsOther { get; set; }
        public bool IsCatalog { get; set; }
        public bool IsPracticeExist { get; set; }
        public bool IsManualMaintenance { get; set; }
        public bool IsManualExist { get; set; }

        public ProjectResultModel()
        {
            IsSolution = false;
            IsTransfer = false;
        }
    }
}
