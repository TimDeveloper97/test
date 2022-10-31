using NTS.Model.Combobox;
using NTS.Model.Quotation;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Project
{
    public class QCCheckListModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProductStandardGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public string ProductStandardGroupName { get; set; }
        public string ProjectProductId { get; set; }
        public string NG_Images { get; set; }
        public string OK_Images { get; set; }
        public string Content { get; set; }
        public DateTime? QCDate { get; set; }
        public string QCByName { get; set; }
        public string QCResultId { get;set; }
        public string ProductItemId { get; set; }
        public string UserId { get; set; }
        public string Result { get; set; }
        public string QCCheckListId { get; set; }
        public QCResultModel QCResult { get; set; }
        public List<QuoteDocumentModel> Attachment { get; set; }
        public List<String> NGImg { get; set; }
        public List<String> OKImg { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public List<Attachment> ListFile { get; set; }
        public string Target { get; set; }
        public int DataType { get; set; }
    }
}
