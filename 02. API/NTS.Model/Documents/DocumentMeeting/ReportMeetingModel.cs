using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentMeeting
{
    public class ReportMeetingModel
    {
        public string DocumentId { get; set; }
        public DateTime? MeetingDate { get; set; }
        public List<DocumentMeetingModel> MeetingFiles { get; set; }
        public ReportMeetingModel()
        {
            MeetingFiles = new List<DocumentMeetingModel>();
        }
    }
}
