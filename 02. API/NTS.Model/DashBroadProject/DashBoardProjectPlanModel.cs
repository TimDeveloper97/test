using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashBroadProject
{
    public class DashBoardProjectPlanModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectProductId { get; set; }
        public int Type { get; set; }
        public DateTime? RealStartDate { get; set; }
        public DateTime? RealEndDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string ResponsiblePersion { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public DateTime? ExpectedDesignFinishDate { get; set; }
        public DateTime? ExpectedMakeFinishDate { get; set; }
        public DateTime? ExpectedTransferDate { get; set; }
        // 1 : trong ke hoach
        //2 : tre ke hoach
        public int Plan { get; set; }
        // 1 : trong kick off
        //2 : tre kick off
        public int KickOff { get; set; }
        public int LateDate { get; set; }
    }
}
