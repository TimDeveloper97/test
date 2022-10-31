using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentPromulgate
{
    public class DocumentPromulgateFileModel
    {
        public string Id { get; set; }
        public string DocumentPromulgateId { get; set; }
        public string DocumentId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
    }
}
