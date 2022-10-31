using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Customers
{
    public class CustomerMeetingSearchResultModel : SearchCommonModel
    {
        public string MeetingTypeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public int Status { get; set; }
        public string Request { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Address { get; set; }
        public DateTime? MeetingDate { get; set; }
        public object StartTime { get; set; }
        public object EndTime { get; set; }

        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
    }
}
