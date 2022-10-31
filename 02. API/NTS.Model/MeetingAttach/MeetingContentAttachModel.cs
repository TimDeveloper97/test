using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MeetingAttach
{
    public class MeetingContentAttachModel
    {
        public string Id { get; set; }
        public string MeetingId { get; set; }
        public string MeetingContentId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateName { get; set; } 
    }
}
