using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Recruitments.Interviews
{
    public class InterviewSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string WorkTypeId { get; set; }
        public string Email { get; set; }
        public int? ProfileStatus { get; set; }
        public int? Status { get; set; }
        public int? InterviewStatus { get; set; }
        public DateTime? ApplyDateTo { get; set; }
        public DateTime? ApplyDateFrom { get; set; }
        public DateTime? InterviewDateTo { get; set; }
        public DateTime? InterviewDateFrom { get; set; }
        public string EmployeeId { get; set; }
        public List<InterviewModel> Interviews { get; set; }
        public int Count { get; set; }
    }
}
