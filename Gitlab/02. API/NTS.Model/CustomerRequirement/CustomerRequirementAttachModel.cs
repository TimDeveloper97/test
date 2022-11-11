using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirement
{
    public class CustomerRequirementAttachModel : BaseModel
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public bool IsDelete { get; set; }
    }
}
