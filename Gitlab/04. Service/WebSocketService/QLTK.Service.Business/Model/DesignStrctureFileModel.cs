using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class DesignStrctureFileModel
    {
        public string Id { get; set; }
        public string DesignStructureId { get; set; }
        public string Name { get; set; }
        public bool Exist { get; set; }
    }
}
