using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model
{
    public class DesignStructureCreateModel
    {
        public int Type { get; set; }
        public int ObjectType { get; set; }
        public string ObjectCode { get; set; }
        public string ApiUrl { get; set; }
        public string ObjectName { get; set; }
        public string CreateBy { get; set; }
        public string ObjectGroupCode { get; set; }
        public string ParentGroupCode { get; set; }
        public List<DesignStructureModel> ListDesignStructure { get; set; }
    }
}
