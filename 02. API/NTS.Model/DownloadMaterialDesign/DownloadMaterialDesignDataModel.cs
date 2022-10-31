using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadMaterialDesign
{
    public class DownloadMaterialDesignDataModel
    {
        public string Path { get; set; }

        public List<DownloadMaterialDesignFileModel> Files { get; set; }

        public DownloadMaterialDesignDataModel()
        {
            Files = new List<DownloadMaterialDesignFileModel>();
        }
    }
    public class DownloadMaterialDesignFileModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ServerPath { get; set; }
        public string GoogleDriveId { get; set; }
    }
}
