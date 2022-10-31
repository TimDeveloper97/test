using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestCriteria
{
    public class TestCriteriaModel : BaseModel
    {
        public string Id { get; set; }
        public string TestCriteriaGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TechnicalRequirements { get; set; }
        public string Note { get; set; }
        public string TestCriteriaGroupName { get; set; }
        public bool ResuldStatusTest { get; set; }
        public bool IsExport { get; set; }
        public string NoteResuld { get; set; }
        public int DataType { get; set; }
        public bool IsSendSale { get; set; }
    }
}
