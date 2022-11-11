using NTS.Model.DownloadMaterialDesign;
using NTS.Model.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadListModules
{
    public class DownloadMarterialDesignModel
    {
        public string ModuleCode { get; set; }
        public string MaterialCode { get; set; }
        public string ApiUrl { get; set; }
        public string DownloadPath { get; set; }

        public string Token { get; set; }
        public List<DownloadMarterialModel> ListMaterialModule { get; set; }
    }
}
