using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class FolderModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public string ParentId { get; set; }
        public string Extension { get; set; }
        public decimal FileSize { get; set; }
        public bool IsRoot { get; set; }
    }

    public class DataFolderModel
    {
        public string Id { get; set; }
        public List<FolderModel> ListFolder { get; set; }
    }
}
