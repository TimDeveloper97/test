using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Function
{
    public class FunctionSearchModel : SearchCommonModel
    {
        public string FunctionGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public List<string> ListIdSelect { get; set; }
        public List<string> ListIdChecked { get; set; }
        public string FunctionGroupName { get; set; }
    }
}
