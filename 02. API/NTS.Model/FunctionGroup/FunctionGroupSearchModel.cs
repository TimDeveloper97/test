using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FunctionGroup
{
    public class FunctionGroupSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
}
