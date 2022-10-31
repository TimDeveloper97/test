using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Candidates
{
    public class CandidateEducationModel
    {
        public string Id { get; set; }
        public string CandidateId { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }
        public string QualificationId { get; set; }
        public string Type { get; set; }
        public string ClassificationId { get; set; }
        public string Time { get; set; }
    }
}
