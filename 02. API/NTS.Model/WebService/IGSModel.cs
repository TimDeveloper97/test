using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WebService
{
    public class IGSModel
    {
        public string ModuleCode { get; set; }
        public string NamePath { get; set; }

        public List<IGSFileResultModel> FileLocals { get; set; }

        public IGSModel()
        {
            FileLocals = new List<IGSFileResultModel>();
        }
    }
}
