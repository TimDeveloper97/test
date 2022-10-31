using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.Modules
{
    public class DownloadMarterialModel
    {
        public List<DownloadMarterialModel> ListMaterials { get; set; }
        public string ModuleCode { get; set; }
        public string MaterialCode { get; set; }
        public string Specification { get; set; }

        public DownloadMarterialModel()
        {
            ListMaterials = new List<DownloadMarterialModel>();
        }
    }
}
