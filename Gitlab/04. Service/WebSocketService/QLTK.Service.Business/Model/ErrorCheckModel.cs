using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ErrorCheckModel
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Type { get; set; }
        public int Index { get; set; }
    }
}
