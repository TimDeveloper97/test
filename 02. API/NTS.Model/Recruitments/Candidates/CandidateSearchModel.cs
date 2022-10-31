using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Candidates
{
    public class CandidateSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkTypeId { get; set; }
        public string Email { get; set; }
        public DateTime? ApplyDateTo { get; set; }
        public DateTime? ApplyDateFrom { get; set; }
    }
}
