using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DesignDocuments
{
   public class FolderUploadModel
    {
        public string LocalPath { get; set; }
        public string ServerPath { get; set; }
        public string Name { get; set; }
        public List<FileUploadModel> Files { get; set; }
        public string Id { get; set; }
        public string DBId { get; set; }
        public string ParentId { get; set; }
    }
}
