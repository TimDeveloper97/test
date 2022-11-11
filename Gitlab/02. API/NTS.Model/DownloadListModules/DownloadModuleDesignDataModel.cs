using NTS.Model.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadListModules
{
    public class DownloadModuleDesignDataModel
    {
        public string Path { get; set; }
        public string ProjectCode { get; set; }
        public bool IsExportMaterial { get; set; }
        public List<ModuleMaterialResultModel> ListMaterial { get; set; }
        public List<DownloadModuleDesignFileModel> Files { get; set; }

        public DownloadModuleDesignDataModel()
        {
            ListMaterial = new List<ModuleMaterialResultModel>();
            Files = new List<DownloadModuleDesignFileModel>();
        }
    }

    public class DownloadModuleDesignFileModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ServerPath { get; set; }
        public string GoogleDriveId { get; set; }
    }
}
