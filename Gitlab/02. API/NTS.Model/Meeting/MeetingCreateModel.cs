using NTS.Model.MeetingAttach;
using NTS.Model.MeetingContent;
using NTS.Model.MeetingCustomerContact;
using NTS.Model.MeetingEmployee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Meeting
{
    public class MeetingCreateModel
    {
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public int Type { get; set; }
        public string MeetingTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; } 
        public DateTime? MeetingDate { get; set; }
        public object StartTime { get; set; }
        public object EndTime { get; set; }
        public object RealStartTime { get; set; }
        public object RealEndTime { get; set; }
        public int Time { get; set; }
        public string CodeChar { get; set; }
        public int Index { get; set; }
        public int Status { get; set; }
        public int Step { get; set; }
        public string MeetingId { get; set; }

        public string Request { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public List<MeetingCustomerContactInfoModel> ListCustomerContact { get; set; }
        public List<MeetingUserModel> ListUser { get; set; }
        public List<MeetingContentModel> ListContent { get; set; }
        public List<MeetingAttachModel> ListAttach { get; set; }

        public MeetingCreateModel()
        {
            ListCustomerContact = new List<MeetingCustomerContactInfoModel>();
            ListUser = new List<MeetingUserModel>();
            ListContent = new List<MeetingContentModel>();
            ListAttach = new List<MeetingAttachModel>();
        }
    }
}
