using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusModule
{
    public class ReportStatusModuleSearchModel : SearchCommonModel
    {
        public string ProductId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCode { get; set; }
        public string ProjectId { get; set; }
        public bool IsExcel { get; set; }
    }
}
