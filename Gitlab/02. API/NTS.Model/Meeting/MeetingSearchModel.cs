using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Meeting
{
    public class MeetingSearchModel: SearchCommonModel
    {
        public string MeetingTypeId { get; set; }
        public string Code { get; set; }
        public int? Type { get; set; }
        public int? Status { get; set; }
        public string UserId { get; set; }
        public string DepartmentId { get; set; }

        public string Customer { get; set; }
        public string Request { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EmployeeId { get; set; }

        public bool IsBGĐ { get; set; }
        public bool IsTrPhong { get; set; }

    }
}
