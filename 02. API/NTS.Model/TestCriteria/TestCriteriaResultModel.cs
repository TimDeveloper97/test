using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestCriteria
{
    public class TestCriteriaResultModel : BaseModel
    {
        public string Id { get; set; }
        public string TestCriteriaGroupId { get; set; }
        public int Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ModuleCode { get; set; }
        public string TechnicalRequirements { get; set; }
        public string Note { get; set; }
        public string TestCriteriaGroupName { get; set; }
        public bool ResuldStatusTest { get; set; }
        public int DataType { get; set; }
    }
}
