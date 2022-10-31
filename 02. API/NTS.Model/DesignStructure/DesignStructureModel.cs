using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DesignStructure
{
    public class DesignStructureModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int ObjectType { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }
        public string Contain { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string ParentPath { get; set; }
        public bool IsOpen { get; set; }
        public string DepartmentId { get; set; }
        public List<DesignStructureFileModel> ListFile { get; set; }

        public DesignStructureModel()
        {
            ListFile = new List<DesignStructureFileModel>();
        }
    }
}
