using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.DesignStructure
{
    public class DesignStructureResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int Type { get; set; }
        public List<DesignStructureFileModel> ListFile { get; set; }
        public string Path { get; set; }
        public DesignStructureResultModel()
        {
            ListFile = new List<DesignStructureFileModel>();
        }
    }
}
