using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RecruitmentRequest
{
    public class RecruitmentRequestSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public string WorkTypeId { get; set; }
        public int? Status { get; set; }
        public int? StatusRecruit { get; set; }
    }
}
