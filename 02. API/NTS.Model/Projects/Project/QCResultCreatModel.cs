using NTS.Model.Combobox;
using NTS.Model.Quotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Project
{
    public class QCResultCreatModel
    {
        public string Id { get; set; }
        public string QCCheckListId { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public string ProductItemId { get; set; }
        public string UserId { get; set; }
        public List<QuoteDocumentModel> Attachment { get; set; }
    }
}
