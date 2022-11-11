using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class CADModel
    {
        public string IdGoogleApi { get; set; }
        public string FTPFilePath { get; set; }
        public string FilePath { get; set; }
        public string SourceFileName { get; set; }
        public string FileName { get; set; }

        public bool IsExist { get; set; }

    }
}
