using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class CADResultModel
    {
        public bool IsSuccess { get; set; }
        public List<CADModel> ListHardCAD { get; set; }
        public List<CADModel> ListSoftCAD { get; set; }
    }
}
