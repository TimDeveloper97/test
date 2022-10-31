using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CodeRule
{
    public class CodeRuleData
    {
        public List<CodeRuleModel> ListModel { get; set; }
    }

    public class SearchCodeRuleModel
    {
        public string Code { get; set; }
    }
}
