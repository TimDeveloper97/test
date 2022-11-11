using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleDesignDocument
{
 public   class ModuleDesignDocumentDownloadModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ServerPath { get; set; }
        public string FileType { get; set; }
        public string ModuleId { get; set; }
    }
}
