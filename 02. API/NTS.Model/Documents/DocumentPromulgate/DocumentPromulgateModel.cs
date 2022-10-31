using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentPromulgate
{
    public class DocumentPromulgateModel
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string Reason { get; set; }
        public string Content { get; set; }
        public System.DateTime PromulgateDate { get; set; }
        public System.DateTime ReviewDateFrom { get; set; }
        public System.DateTime ReviewDateTo { get; set; }
        public System.DateTime ApproveDate { get; set; }
    }
}
