using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.MAT
{
   public class MATModel
    {
        public string ModuleCode { get; set; }
        public string NamePath { get; set; }

        public List<MATFileResultModel> FileLocals { get; set; }

        public MATModel()
        {
            FileLocals = new List<MATFileResultModel>();
        }
    }
}
