using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class DowloadMaterialDocument
    {
        public List<MaterialDocumentDownloadModel> Material { get; set; }
        public string ApiUrl { get; set; }
        public string DownloadPath { get; set; }
        public string Token { get; set; }
        public string ApiFileUrl { get; set; }
        public List<FileErrorModel> ListError { get; set; }
    }
}
