using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadListModules
{
   public class DownloadModuleDesignModel
    {
        public string ProjectId { get; set; }
        public string ModuleCode { get; set; }
        public List<string> ModuleIds { get; set; }

        public DownloadModuleDesignModel()
        {
            ModuleIds = new List<string>();
        }
    }
}
