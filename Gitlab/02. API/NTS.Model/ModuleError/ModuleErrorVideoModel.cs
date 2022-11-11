using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleError
{
  public  class ModuleErrorVideoModel
    {
        public string Id { get; set; }
        public string ModuleErrorId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Size { get; set; }

    }
}
