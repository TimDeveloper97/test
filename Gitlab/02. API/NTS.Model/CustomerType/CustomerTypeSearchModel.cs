using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerType
{
    public class CustomerTypeSearchModel: SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
