using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class QuotationPlanModel
    {
        public string QuotationId { get; set; }
        public string StepInQuotationId { get; set; }
        public string Name { get; set; }
        public string EmployeeId { get; set; }
        public string Description { get; set; }
        public int EstimateTime { get; set; }
        public int DoneRatio { get; set; }
        public int Status { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public DateTime? PlanStartDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string QuotationPlanName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? ActualEndDate { get; set; }

        public string DepartmentName { get; set; }
    }
}
