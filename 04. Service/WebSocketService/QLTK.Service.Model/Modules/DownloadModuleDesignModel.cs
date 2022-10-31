using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Modules
{
    /// <summary>
    /// Thong tin download tài liệu thiết kế module
    /// </summary>
    public class DownloadModuleDesignModel
    {
        public List<string> ModuleIds { get; set; }
        public string ProjectId { get; set; }
        public string ApiUrl { get; set; }
        public string DownloadPath { get; set; }

        public string Token { get; set; }

        public DownloadModuleDesignModel()
        {
            ModuleIds = new List<string>();
        }
    }
}
