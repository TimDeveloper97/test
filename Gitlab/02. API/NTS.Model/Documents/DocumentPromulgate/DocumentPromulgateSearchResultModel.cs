using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentPromulgate
{
    public class DocumentPromulgateSearchResultModel
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string Reason { get; set; }
        public string Content { get; set; }
        public DateTime PromulgateDate { get; set; }
        public List<DocumentPromulgateFileModel> ListFile { get; set; }

        public DocumentPromulgateSearchResultModel()
        {
            ListFile = new List<DocumentPromulgateFileModel>();
        }
    }
}
