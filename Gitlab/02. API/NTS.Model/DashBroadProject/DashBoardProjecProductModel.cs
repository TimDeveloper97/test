using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashBroadProject
{
    public class DashBoardProjecProductModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public int DataType { get; set; }
        public int DesignStatus { get; set; }
        public DateTime? DesignFinishDate { get; set; }
        public DateTime? ExpectedMakeFinishDate { get; set; }
        public DateTime? ExpectedTransferDate { get; set; }
        public DateTime? ExpectedDesignFinishDate { get; set; }
        public DateTime? MakeFinishDate { get; set; }
        public DateTime? KickOffDate { get; set; }
    }
}
