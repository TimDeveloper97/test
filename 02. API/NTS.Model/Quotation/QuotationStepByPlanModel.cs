using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class QuotationStepByPlanModel
    {
        public string QuotesId { get; set; }
        public string QuotesName { get; set; }
        public string QuotesCode { get; set; }
        public int SuccessRatio { get; set; }
        public DateTime? PlanDueDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
    }
}
