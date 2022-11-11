using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RecruitmentChannels
{
    public class RecruitmentChannelSearchResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
        public int CandidateNumber { get; set; }
        public int RecruitmentNumber { get; set; }
        public int RecruitmentReceive { get; set; }
    }
}
