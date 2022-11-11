using QLTK.Service.Model.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class GoogleUploadFolderProductResultModel
    {
        public string CreateBy { get; set; }
        public List<ProductDesignDocumentModel> ListResult { get; set; }
        public bool IsUploadSuccess { get; set; }
        public GoogleUploadFolderProductResultModel()
        {
            ListResult = new List<ProductDesignDocumentModel>();
        }
    }
}
