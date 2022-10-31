using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DesignStructure
{
    public class DesignStructureSearchModel
    {
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int ObjectType { get; set; }
    }

    public class DesignStructureSearchParentModel
    {
        public int Type { get; set; }
        public int ObjectType { get; set; }
        public string DesignStructureId { get; set; }
        public string DepartmentId { get; set; }
    }
}
