using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.Project
{
    public class PlanAdjustmentModel
    {
        public string Id { get; set; }
        public string PlanId { get; set; }
        public int Version { get; set; }
        public bool Status { get; set; }
        public DateTime? AcceptDate { get; set; }
        public string Content { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public List<PlanHistoryAttachModel> ListAttach { get; set; }
        public PlanAdjustmentModel()
        {
            ListAttach = new List<PlanHistoryAttachModel>();
        }

    }
}
