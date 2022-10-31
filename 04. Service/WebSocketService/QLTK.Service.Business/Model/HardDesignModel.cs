using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class HardDesignModel
    {
        public string ModuleCode { get; set; }
        public string Code { get; set; }
        public string Size { get; set; }
        public string Date { get; set; }
        public string FullDate { get; set; }
        public string FilePath { get; set; }

        public string NameCompare { get; set; }
        public string SizeCompare { get; set; }
        public string DateCompare { get; set; }

        public bool IsExistName { get; set; }
        public bool IsDifferentSize { get; set; }
        public bool IsDifferentDate { get; set; }
    }
}
