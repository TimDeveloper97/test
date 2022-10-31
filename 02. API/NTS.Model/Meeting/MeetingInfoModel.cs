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
    public class MeetingInfoModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerContactId { get; set; }
        public string CustomerContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string MeetingTypeId { get; set; }
        public string MeetingTypeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string StrStartTime { get; set; }
        public string StrEndTime { get; set; }
        public object StartTime { get; set; }
        public object EndTime { get; set; }
        public string StrRealStartTime { get; set; }
        public string StrRealEndTime { get; set; }
        public object RealStartTime { get; set; }
        public object RealEndTime { get; set; }
        public int Time { get; set; }
        public string CodeChar { get; set; }
        public int Index { get; set; }
        public int Status { get; set; }
        public int Step { get; set; }
        public string ReasonCancel { get; set; }
        public string Request { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        


        public List<MeetingCustomerContact.MeetingCustomerContactInfoModel> ListCustomerContact { get; set; }
        public List<MeetingUserInfoModel> ListUser { get; set; }
        public List<MeetingContentModel> ListContent { get; set; }
        public List<MeetingAttachModel> ListAttach { get; set; }
        public List<MeetingAttachModel> ListAttachPerformStep { get; set; }

        public MeetingInfoModel()
        {
            ListCustomerContact = new List<MeetingCustomerContact.MeetingCustomerContactInfoModel>();
            ListUser = new List<MeetingUserInfoModel>();
            ListContent = new List<MeetingContentModel>();
            ListAttach = new List<MeetingAttachModel>();
            ListAttachPerformStep = new List<MeetingAttachModel>();
        }
    }
}
