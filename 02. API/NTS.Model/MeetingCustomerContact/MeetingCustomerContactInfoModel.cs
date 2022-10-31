using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MeetingCustomerContact
{
    public class MeetingCustomerContactInfoModel
    {
        public string Id { get; set; }
        public string MeetingCustomerContactId { get; set; }
        public string CustomerContactId { get; set; }
        public string MeetingId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string Avatar { get; set; }
        public bool IsNew { get; set; }
    }
}
