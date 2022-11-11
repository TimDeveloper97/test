using NTS.Model.MeetingAttach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirement
{
    public class CustomerRequirementContentModel
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string Code { get; set; }
        public string MeetingContentId { get; set; }
        public string Request { get; set; }
        public string Solution { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Note { get; set; }
        public string RequestBy { get; set; }
        public string RequestName { get; set; }
        
        public List<MeetingContentAttachModel> MeetingContentAttaches { get; set; }

    }
}
