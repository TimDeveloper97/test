using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.IGS
{
    public class IGSFileResultModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public decimal FileSize { get; set; }
        public decimal FileServerSize { get; set; }
        public string Status { get; set; }
    }
}
