using NTS.Model.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectGeneralDesignMaterials
{
    public class ProjectGeneralDesignMaterialsModel : MaterialModel
    {
        public string ProjectGeneralDesignId { get; set; }
        public decimal Inventoty { get; set; }
        public decimal ContractPrice { get; set; }
        public string Manafacture { get; set; }
        public bool IsDelete { get; set; }
        public decimal OldQuantity { get; set; }
        public int ModuleStatusUse { get; set; }
        public int CreateIndex { get; set; }
    }
}
