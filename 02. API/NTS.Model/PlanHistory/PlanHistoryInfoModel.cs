using NTS.Model.Download;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PlanHistory
{
    public class PlanHistoryInfoModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public int Version { get; set; }
        public bool Status { get; set; }
        public DateTime? AcceptDate { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDate { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public List<DownloadModel> ListDatashet { get; set; }
        public PlanHistoryInfoModel()
        {
            ListDatashet = new List<DownloadModel>();
        }
    }
}
