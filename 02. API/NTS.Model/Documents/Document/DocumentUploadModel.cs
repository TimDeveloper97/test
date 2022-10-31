using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Document
{
    public class DocumentUploadModel
    {
        public string DocumentId { get; set; }
        public List<DocumentFileModel> DocumentFiles { get; set; }
        public List<DocumentSearchResultModel> DocumentReferences { get; set; }

        public DocumentUploadModel()
        {
            DocumentFiles = new List<DocumentFileModel>();
            DocumentReferences = new List<DocumentSearchResultModel>();
        }
    }
}
