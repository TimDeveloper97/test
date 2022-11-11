using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SubsidyHistory
{
    public class SubsidyHistoryModel
    {
        public string Id { get; set; }
        public string ProjectEmployeeId { get; set; }
        public decimal Subsidy { get; set; }
        public DateTime SubsidyStartTime { get; set; }
        public DateTime SubsidyEndTime { get; set; }
    }
}
