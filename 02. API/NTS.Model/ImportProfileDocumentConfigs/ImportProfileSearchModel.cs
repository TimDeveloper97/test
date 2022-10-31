using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.ImportProfileDocumentConfigs
{
    public class ImportProfileSearchModel: SearchCommonModel
    {
        public string Code { get; set; }
        public string EmployeeId { get; set; }
        public int? Step { get; set; }
        public int? StepSearch { get; set; }
        public int? FinishStatus { get; set; }
        public int? Status { get; set; }
        public int? WorkStatus { get; set; }
        public string ProjectCode { get; set; }
        public string PRCode { get; set; }
        public string ProductCode { get; set; }
        public string SupplierId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public DateTime? PayDateFrom { get; set; }
        public DateTime? PayDateTo { get; set; }
        public int? PayStatus { get; set; }
        public DateTime? DeliveryDateFrom { get; set; }
        public DateTime? DeliveryDateTo { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
        public string TimeType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Quarter { get; set; }
        public bool IsSearch7Day { get; set; }
        public DateTime? SearchDateFrom { get; set; }
        public DateTime? SearchDateTo { get; set; }
        public DateTime? ProductionExpectedDateFrom { get; set; }
        public DateTime? ProductionExpectedDateTo{ get; set; }
        public bool IsProductionExpiredOnWeek { get; set; }
        public bool IsProductionExpired { get; set; }
        public bool IsPayExpired { get; set; }
        public bool IsPayWarning { get; set; }
    }
}