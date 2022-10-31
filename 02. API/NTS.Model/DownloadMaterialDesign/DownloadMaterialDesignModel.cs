using NTS.Model.ModuleMaterials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadMaterialDesign
{
    public class DownloadMaterialDesignModel
    {
        public List<string> Listmaterial { get; set; }

        public string ModuleCode { get; set; }
        public string MaterialCode { get; set; }
        public DownloadMaterialDesignModel()
        {
            Listmaterial = new List<string>();
        }
    }
}
