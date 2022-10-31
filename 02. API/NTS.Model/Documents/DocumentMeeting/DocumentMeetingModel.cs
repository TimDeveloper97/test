using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentMeeting
{
    public class DocumentMeetingModel
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
        public Nullable<decimal> PDFPath { get; set; }
        public string Note { get; set; }
        public System.DateTime MeetingDate { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
