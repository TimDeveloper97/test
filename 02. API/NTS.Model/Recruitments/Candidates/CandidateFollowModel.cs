using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Candidates
{
    public class CandidateFollowModel
    {
        public string Id { get; set; }
        public string CandidateId { get; set; }
        public string Content { get; set; }
        public DateTime FollowDate { get; set; }
    }
}
