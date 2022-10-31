using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestCriteriaGroup
{
    public class TestCriteriaGroupSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Code { set; get; }
        public string Name { set; get; }
       
    }
}
