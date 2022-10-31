using NTS.Model.Combobox;
using NTS.Model.Quotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Project
{
    public class QCResultModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string QCCheckListId { get; set; }
        public int Status { get; set; }
        public DateTime QCDate { get; set; }
        public string QCByName { get; set; }
        public string QCByImagePath { get; set; }
        public string Reason { get; set; }
        public string ProductItemId { get; set; }
        public string UserId { get; set; }
        public string Result { get; set; }
        public List<QuoteDocumentModel> Attachment { get; set; }
        public string QCBy { get; set; }
    }
}
