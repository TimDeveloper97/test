using NTS.Model.MeetingEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Meeting
{
    public class MeetingPlanEntity
    {
        public string MeetingId { get; set; }
        public string Address { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public object StartTime { get; set; }
        public object EndTime { get; set; }
        public int Time { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public List<MeetingUserInfoModel> ListUser = new List<MeetingUserInfoModel>();

    }
}
