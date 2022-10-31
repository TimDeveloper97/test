using NTS.Model.MeetingAttach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MeetingContent
{
    public class MeetingContentModel
    {
        public string Id { get; set; }
        public string MeetingId { get; set; }
        public string Request { get; set; }
        public string Solution { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Note { get; set; }
        public string Descripton { get; set; }
        public string CreateBy { get; set; }   
        public DateTime? CreateDate { get; set; }
        public string Code { get; set; }
        public bool? Checked { get; set; }
        public string EmployeeName { get; set; }
        public string RequestBy { get; set; }
        public string NameFiles { get; set; }

        public bool IsDeleted { get; set; }
        public List<MeetingContentAttachModel> MeetingContentAttaches { get; set; }
    }
}
