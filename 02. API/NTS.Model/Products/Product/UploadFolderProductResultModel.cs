using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Product
{
    public class UploadFolderProductResultModel
    {
        public string CreateBy { get; set; }
        public List<ProductDesignDocumentModel> ListResult { get; set; }
        public bool IsUploadSuccess { get; set; }
        public UploadFolderProductResultModel()
        {
            ListResult = new List<ProductDesignDocumentModel>();
        }
    }
}
