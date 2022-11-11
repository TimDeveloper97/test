using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Modules
{
    public class DownloadMaterialDesignModel
    {
        public string ModuleCode { get; set; }
        public string MaterialCode{ get; set; }
        public string ApiUrl { get; set; }
        public string DownloadPath { get; set; }
        public string MaterialPath { get; set; }

        public string Token { get; set; }
        public List<DownloadMarterialModel> ListMaterialModule { get; set; }

        public DownloadMaterialDesignModel()
        {
            ListMaterialModule = new List<DownloadMarterialModel>();
        }
    }
}
