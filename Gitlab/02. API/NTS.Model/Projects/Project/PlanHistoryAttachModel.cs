using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.Project
{
    public class PlanHistoryAttachModel
    {
        public string Id { get; set; }
        public string PlanHistoryId { get; set; }
        public int Type { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string FilePath { get; set; }
        public string CreateBy { get; set; }
        public String UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
