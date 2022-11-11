using NTS.Model.MeetingAttach;
using NTS.Model.MeetingCustomerContact;
using NTS.Model.MeetingEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Meeting
{
    public class MeetingPerformingEntity
    {
        public string MeetingId { get; set; }
        public string StrRealStartTime { get; set; }
        public string StrRealEndTime { get; set; }
        public object RealStartTime { get; set; }
        public object RealEndTime { get; set; }

        public List<MeetingCustomerContactInfoModel> ListCustomerContact = new List<MeetingCustomerContactInfoModel>();
        public List<MeetingAttachModel> ListAttachPerformStep = new List<MeetingAttachModel>();
    }
}
