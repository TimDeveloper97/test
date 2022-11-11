using NTS.Model.MeetingAttach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Meeting
{
    public class MeetingInfoEntity
    {
        public string MeetingId { get; set; }
        public string MeetingTypeId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public string Request { get; set; }
        public DateTime? RequestDate { get; set; }
        public string CodeChar { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public List<MeetingAttachModel> ListAttach = new List<MeetingAttachModel>();

    }
}
