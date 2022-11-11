using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestCriteria
{
   public class TestCriterSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string TestCriteriaGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TechnicalRequirements { get; set; }
        public string Note { get; set; }
        public int? DataType { get; set; }
        public bool IsExport { get; set; }

        public List<string> ListIdSelect { get; set; }
    }
}
