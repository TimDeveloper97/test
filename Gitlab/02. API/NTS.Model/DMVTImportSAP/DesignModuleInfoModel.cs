using NTS.Model.ProjectGeneralDesignMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DMVTImportSAP
{
    public class DesignModuleInfoModel
    {
        public string ProjectId { get; set; }
        public string ProjectProductId { get; set; }
        public decimal Quantity { get; set; }
        public int CreateIndex { get; set; }
        public List<DesignModuleModel> Modules { get; set; }
        public List<DesignModuleModel> ModulesFalse { get; set; }
        public List<ProjectGeneralDesignMaterialsModel> ListMaterial { get; set; }

        public List<string> ListIdSelect { get; set; }

        public DesignModuleInfoModel()
        {
            Modules = new List<DesignModuleModel>();
            ModulesFalse = new List<DesignModuleModel>();
            ListMaterial = new List<ProjectGeneralDesignMaterialsModel>();
        }
    }
}
