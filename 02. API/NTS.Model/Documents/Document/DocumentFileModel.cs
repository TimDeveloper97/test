using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Document
{
    public class DocumentFileModel
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
        public string PDFPath { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDelete { get; set; }

    }
}
