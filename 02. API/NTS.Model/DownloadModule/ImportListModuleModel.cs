using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DownloadModule
{
    public class ImportListModuleModel
    {
        public List<string> ListId { get; set; }
        public List<DownloadModuleSearchResultModel> ListNoSelect { get; set; }
        public List<DownloadModuleSearchResultModel> ListSelect { get; set; }
        public int TotalNoSelect { get; set; }
        public int TotalSelect { get; set; }
        public ImportListModuleModel()
        {
            ListId = new List<string>();
            ListSelect = new List<DownloadModuleSearchResultModel>();
            ListNoSelect = new List<DownloadModuleSearchResultModel>();
        }
    }
}
