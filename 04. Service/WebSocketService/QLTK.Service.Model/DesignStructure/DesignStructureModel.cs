using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model
{
    public class DesignStructureModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string ParentPath { get; set; }
        public bool IsOpen { get; set; }

        public List<DesignStructureFileModel> listFile { get; set; }

        public DesignStructureModel()
        {
            listFile = new List<DesignStructureFileModel>();
        }
    }
}
