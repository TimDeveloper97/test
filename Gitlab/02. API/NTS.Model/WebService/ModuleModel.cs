using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WebService
{
    public class ModuleModel
    {
        public string Specification { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ModuleGroupId { get; set; }
        public string ParentGroupId { get; set; }
        public string ModuleGroupCode { get; set; }
        public string ParentGroupCode { get; set; }
        public int CurrentVersion { get; set; }
    }
}
