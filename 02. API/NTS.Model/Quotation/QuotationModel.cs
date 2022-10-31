using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class QuotationModel
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime QuotationDate { get; set; }
        public int EffectiveLength { get; set; }
        public string EmployeeId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public string Warranty { get; set; }
        public string Delivery { get; set; }
        public int AdvanceRate { get; set; }
        public int SuccessRate { get; set; }
        public decimal ExpectedPrice { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public int Status { get; set; }
        public string SBUid { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public bool isShowChosse { get; set; }

        public List<QuoteDocumentModel> ListQuoteDocument { get; set; }
        public List<QuoteStepModel> ListQuotesStep { get; set; }
        
    }
}
