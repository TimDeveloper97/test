using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.QLTKMP
{
    public class QLTKMPModel : BaseModel
    {
        public string Id { get; set; }
        public string MaterialGroupId { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string MaterialParameterId { get; set; }
        public List<string> MaterialParameterValueList { get; set; }
    }
}
