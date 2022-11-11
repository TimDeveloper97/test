using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusMaterial
{
    public class ReportStatusMaterialModuleModel
    {
        public string ModuleId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ModuleGroupName { get; set; }
        public decimal Quantity { get; set; }
        public decimal RealQuantityInProject { get; set; }
        public int Status { get; set; }
    }
}
