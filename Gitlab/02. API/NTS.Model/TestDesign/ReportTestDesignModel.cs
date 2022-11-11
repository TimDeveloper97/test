using NTS.Model.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestDesign
{
    public class ReportTestDesignModel
    {
        public string ModuleCode { get; set; }
        public List<SoftDesignModel> ListSoftDesign { get; set; }
        public List<HardDesignModel> ListHardDesign { get; set; }
        public List<ErrorDesignStructureModel> DatatbleCT { get; set; }

        public string FileName { get; set; }

        public string Designer { get; set; }
    }
}
