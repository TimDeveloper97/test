using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model
{
    public class DesignStructureFileModel
    {
        public string DesignStructureId { get; set; }
        public string Name { get; set; }
        public bool Exist { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsInsertData { get; set; }
        public string Path { get; set; }
    }
}
