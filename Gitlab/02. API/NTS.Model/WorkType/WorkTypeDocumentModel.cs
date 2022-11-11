using NTS.Model.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorkType
{
 public   class WorkTypeDocumentModel
    {
        public string Id { get; set; }
        public string DocumentGroupName { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentObjectName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDocumentOfTask { get; set; }
        public string TaskName { get; set; }
        public List<DocumentFileModel> ListFile { get; set; }

        public WorkTypeDocumentModel()
        {
            ListFile = new List<DocumentFileModel>();
        }
    }
}
