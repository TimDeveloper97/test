using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectRole.ExportExcelModel
{
    public class ExportExcelRoleResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string Descipton { get; set; }
        public bool IsDisable { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<PlanFunctionExportExcelResultModel> PlanFunctions { get; set; }
        public List<string>  PlanfunctionNames { get; set; }

    }
}
