using QLTK.Service.Model.Design3D;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Downloads
{
    public class DownloadFileDesignDocumentModel
    {
        public string FilePath { get; set; }
        public int Type { get; set; }
        public string ApiUrl { get; set; }
        public string Token { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleGroupCode { get; set; }
        public List<DocumentFileSizeModel> ListDocumentFileSize { get; set; }
        public DownloadFileDesignDocumentModel()
        {
            ListDocumentFileSize = new List<DocumentFileSizeModel>();
        }
    }
}
