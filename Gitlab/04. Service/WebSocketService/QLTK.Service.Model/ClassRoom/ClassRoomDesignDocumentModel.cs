using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.ClassRoom
{
    public class ClassRoomDesignDocumentModel
    {
        public string Id { get; set; }
        public string ClassRoomId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ServerPath { get; set; }
        public decimal FileSize { get; set; }
        public string FileType { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
