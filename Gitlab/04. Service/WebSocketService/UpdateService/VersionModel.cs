using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateService
{
  public  class VersionModel
    {
        public string VersionNew { get; set; }
        public bool IsUpdate { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
