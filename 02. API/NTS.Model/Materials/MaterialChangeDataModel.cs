using NTS.Model.ProjectGeneralDesignModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Materials
{
    public class MaterialChangeDataModel
    {
        public string ProjectProductId { get; set; }
        public string ModuleId { get; set; }
        public List<ProjectGeneralDesignModuleModel> ModuleIdProjectProducts { get; set; }
        public List<MaterialChangeModel> oldMaterialChangeModels { get; set; }
        public List<MaterialChangeModel> newMaterialChangeModels { get; set; }
        public List<MaterialChangeModel> allMaterialOfModules { get; set; }

        public bool IsExit { get; set; }
        public bool Confirm { get; set; }
        public string Content { get; set; }
    }
}
