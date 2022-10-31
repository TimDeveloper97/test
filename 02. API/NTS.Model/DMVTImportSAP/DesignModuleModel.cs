using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DMVTImportSAP
{
    public class DesignModuleModel
    {
        public string Id { get; set; }
        public string Index { get; set; }
        public string ModuleCode { get; set; }
        public string ParentId { get; set; }
        public string MaterialCode { get; set; }
        public string ModuleName { get; set; }
        public string ProjectCode { get; set; }
        public string Parameter { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal RealQuantity { get; set; }
        public string Manufacturer { get; set; }
        public string MaterialPath { get; set; }
        public string WarehouseCode { get; set; }

        public string ProjectProductId { get; set; }
        public List<DesignMaterialModel> Materials { get; set; }

        public DesignModuleModel()
        {
            Materials = new List<DesignMaterialModel>();
        }
    }
}
