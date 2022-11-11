using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusMaterial
{
    public class ReportStatusMaterialModuleMaterialModel
    {
        public string ModuleId { get; set; }
        public string MaterialId { get; set; }
        public string Specification { get; set; }
        public decimal Quantity { get; set; }
        public decimal ReadQuantity { get; set; }
        public decimal ReadQuantityInProject { get; set; }
        public string Index { get; set; }
    }
}
