using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Files
{
    public class FolderUploadModel
    {
        public string LocalPath { get; set; }
        public string ServerPath { get; set; }
        public string Name { get; set; }
        public List<FileUploadModel> Files { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
    }

    public class FileUploadModel
    {
        public string Name { get; set; }
        public string LocalPath { get; set; }
        public string ServerPath { get; set; }
        public decimal Size { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string HashValue { get; set; }
        public int RowState { get; set; }
    }
}
