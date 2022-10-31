using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Meeting
{
    public class MeetingCustomerContactGetInfoModel
    {
        public string Id { get; set; }
        public string CustomerContactId { get; set; }
        public string MeetingId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
