using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SBU
{
    public class SBUSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public int? Status { get; set; }
    }
}
