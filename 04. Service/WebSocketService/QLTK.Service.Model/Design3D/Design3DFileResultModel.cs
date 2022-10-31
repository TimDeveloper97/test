using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Design3D
{
    public class Design3DFileResultModel
    {
        public List<string> FilePartDifferentSize { get; set; }
        public List<string> FileModuleDifferentSize { get; set; }

        public List<string> FileShareDifferentSize { get; set; }
        public List<string> FileShareNotExist { get; set; }
        public List<DocumentFileSizeModel> ListDocumentFileSize { get; set; }

        public Design3DFileResultModel()
        {
            FilePartDifferentSize = new List<string>();
            FileModuleDifferentSize = new List<string>();
            FileShareDifferentSize = new List<string>();
            FileShareNotExist = new List<string>();
            ListDocumentFileSize = new List<DocumentFileSizeModel>();
        }
    }
}
