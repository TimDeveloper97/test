using QLTK.Service.Model.Commons;
using QLTK.Service.Model.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Products
{
    public class UploadFolderProductResultModel
    {
        public List<CheckUploadEntity> ListError { get; set; }
        public List<string> LstError { get; set; }
        public List<FolderUploadModel> DesignDocuments { get; set; }
        public bool Status { get; set; }
        public bool IsUploadSuccess { get; set; }
        public UploadFolderProductResultModel()
        {
            LstError = new List<string>();
            DesignDocuments = new List<FolderUploadModel>();
        }
    }
}
