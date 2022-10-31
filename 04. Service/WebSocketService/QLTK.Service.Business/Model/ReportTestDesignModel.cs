using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class ReportTestDesignModel
    {
        public string ApiUrl { get; set; }
        public string PathDownload { get; set; }
        public string PathLocal { get; set; }
        public string ModuleCode { get; set; }
        public List<ErrorDesignStructureModel> DatatbleCT { get; set; }
    }
}
