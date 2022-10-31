using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Modules
{
    public class ModuleDesignDocumentModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ServerPath { get; set; }
        public decimal FileSize { get; set; }
        public string HashValue { get; set; }

        public string FileType { get; set; }
        public string DesignType { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string StatusError { get; set; }
    }
}
