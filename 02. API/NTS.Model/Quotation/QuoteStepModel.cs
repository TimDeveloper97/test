using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class QuoteStepModel : BaseModel
    {
        public string QuotesId { get; set; }
        public int SuccessRatio { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string StepInQuotationId { get; set; }
        public string ParentId { get; set; }
        public string QuotesCode { get; set; }

        public string QuotationCode { get; set; }

        public List<QuotationPlanModel> Listchild { get; set; }
        public List<QuotationPlanModel> ListchildInProgress { get; set; }
        public QuoteStepModel()
        {
            Listchild = new List<QuotationPlanModel>();
            ListchildInProgress = new List<QuotationPlanModel>();
        }
    }
}
