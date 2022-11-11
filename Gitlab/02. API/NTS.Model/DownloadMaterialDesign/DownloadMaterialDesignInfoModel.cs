using NTS.Model.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadMaterialDesign
{
    public class DownloadMaterialDesignInfoModel
    {
        public string ModuleCode { get; set; }
        public string ModuleGroupCode { get; set; }
        public string ModuleGroupParentCode { get; set; }
        public bool IsFullDesign { get; set; }
        public List<ModuleMaterialResultModel> Materials { get; set; }
        public List<NTS.Model.Repositories.ModuleDesignDocument> ModuleDesignDocuments { get; set; }

        public DownloadMaterialDesignInfoModel()
        {
            Materials = new List<ModuleMaterialResultModel>();
            ModuleDesignDocuments = new List<Repositories.ModuleDesignDocument>();
        }
    }
}
