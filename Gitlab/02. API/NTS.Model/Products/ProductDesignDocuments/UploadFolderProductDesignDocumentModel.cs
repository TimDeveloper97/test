using NTS.Model.DesignDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductDesignDocuments
{
  public  class UploadFolderProductDesignDocumentModel
    {
        public string ProductId { get; set; }
        public List<FolderUploadModel> DesignDocuments { get; set; }
        public string CreateBy { get; set; }
        public int DesignType { get; set; }
        public UploadFolderProductDesignDocumentModel()
        {
            DesignDocuments = new List<FolderUploadModel>();
        }
    }
}
