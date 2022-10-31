using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectGeneralDesignModule
{
    public class ProjectGeneralDesignModuleResultModel
    {
        public string Id { get; set; }
        public string ProjectGeneralDesignId { get; set; }
        public string ProjectProductId { get; set; }
        public int Quantity { get; set; }
        public int ErrorQuantity { get; set; }
        public decimal ContractPrice { get; set; }
        public decimal ModulePrice { get; set; }
        public string Note { get; set; }
        public int ModuleStatus { get; set; }
    }
}
