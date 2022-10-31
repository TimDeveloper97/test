using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.Specialize
{
    public class SpecializeSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public List<string> ListIdSelect { get; set; }

        public SpecializeSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
