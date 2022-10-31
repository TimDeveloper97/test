using NTS.Model.TestCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ModuleTestCriteria
{
    public class ModuleTestCriteriaModule
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string TestCriteriaId { get; set; }
        public List<TestCriteriaModel> ListTestCriteria { get; set; }
    }
}
