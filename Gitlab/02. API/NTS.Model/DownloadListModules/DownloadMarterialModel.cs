using NTS.Model.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadListModules
{
    public class DownloadMarterialModel
    {
        public List<DownloadMarterialDetailsModel> ListMaterials { get; set; }
        public string ModuleCode { get; set; }

        public DownloadMarterialModel()
        {
            ListMaterials = new List<DownloadMarterialDetailsModel>();
        }

    }

    public class DownloadMarterialDetailsModel
    {
        public string MaterialCode { get; set; }
        public string Specification { get; set; }
    }
}
