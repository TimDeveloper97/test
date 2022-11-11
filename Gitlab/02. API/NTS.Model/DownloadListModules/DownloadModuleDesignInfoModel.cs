using NTS.Model.ModuleDesignDocument;
using NTS.Model.ModuleMaterials;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadListModules
{
    public class DownloadModuleDesignInfoModel
    {
        public string ModuleCode { get; set; }
        public string ModuleGroupCode { get; set; }
        public string ModuleGroupParentCode { get; set; }
        public bool IsFullDesign { get; set; }
        public List<ModuleMaterialResultModel> Materials { get; set; }
        public List<ModuleDesignDocumentDownloadModel> ModuleDesignDocuments { get; set; }

        public List<DownloadModuleDesignInfoModel> ModuleSubs { get; set; }

        public DownloadModuleDesignInfoModel()
        {
            Materials = new List<ModuleMaterialResultModel>();
            ModuleDesignDocuments = new List<ModuleDesignDocumentDownloadModel>();
            ModuleSubs = new List<DownloadModuleDesignInfoModel>();
        }
    }

    public class DownloadModuleDesignSubInfoModel
    {
        public List<ModuleMaterialResultModel> Materials { get; set; }
        public List<DownloadModuleDesignFileModel> Files { get; set; }

        public DownloadModuleDesignSubInfoModel()
        {
            Materials = new List<ModuleMaterialResultModel>();
            Files = new List<DownloadModuleDesignFileModel>();
        }
    }
}
