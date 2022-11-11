using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WebService
{
    public class ErrorDesignStructureModel
    {
        public string Name { get; set; }
        public string NameVT { get; set; }
        public string Hang { get; set; }
        public long Size { get; set; }
        public string PathLocal { get; set; }
        public string Path3D { get; set; }
        public string Type { get; set; }

        public string KLOld { get; set; }
        public string KLNew { get; set; }
    }
}
