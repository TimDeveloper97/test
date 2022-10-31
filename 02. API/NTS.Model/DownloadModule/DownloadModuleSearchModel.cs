using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadModule
{
    public class DownloadModuleSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Code { get; set; }
        // Extent
        public List<string> ListIdSelect { get; set; }

        public DownloadModuleSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
