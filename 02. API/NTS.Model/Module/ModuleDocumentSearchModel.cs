using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Module
{
    public class ModuleDocumentSearchModel
    {
        public string DocumentGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string GroupCode { get; set; }
        public List<string> ListIdSelect { get; set; }
        public ModuleDocumentSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
