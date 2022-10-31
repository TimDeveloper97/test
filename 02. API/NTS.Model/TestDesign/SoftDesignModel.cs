using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestDesign
{
    public class SoftDesignModel
    {
        public string Name { get; set; }
        public string IValue { get; set; }
        public string WValue { get; set; }
        public string Date { get; set; }
        public string SizeCompare { get; set; }

        public string TxtPathSC { get; set; }

        public string NameCompare { get; set; }
        public bool IsExistName { get; set; }

        public string NoteReport { get; set; }
        public string SizeReport { get; set; }
        public string DateReport { get; set; }
    }
}
