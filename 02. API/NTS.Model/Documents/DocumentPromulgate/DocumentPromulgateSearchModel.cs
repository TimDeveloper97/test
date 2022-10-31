using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DocumentPromulgate
{
    public class DocumentPromulgateSearchModel: SearchCommonModel
    {
        public string Reason { get; set; }
        public string DocumentId { get; set; }
        public string Content { get; set; }
    }
}
