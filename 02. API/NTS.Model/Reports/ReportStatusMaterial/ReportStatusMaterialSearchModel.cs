using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusMaterial
{
    public class ReportStatusMaterialSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string ProjectId { get; set; }
        public string ModuleId { get; set; }
    }
}
