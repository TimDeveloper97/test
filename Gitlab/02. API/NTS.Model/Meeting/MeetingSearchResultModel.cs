using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Meeting
{
    public class MeetingSearchResultModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string MeetingTypeId { get; set; }
        public string MeetingTypeName { get; set; }
        public string Request { get; set; }
        public DateTime? RequestDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public string StrRealStartTime { get; set; }
        public string StrRealEndTime { get; set; }
        public object StartTime { get; set; }
        public object EndTime { get; set; }
        public object RealStartTime { get; set; }
        public object RealEndTime { get; set; }
        public int Time { get; set; }
        public int Status { get; set; }
        public int Step { get; set; }
        public string EmployeeId { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
