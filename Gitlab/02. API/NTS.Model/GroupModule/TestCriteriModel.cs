using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GroupModule
{
    public class TestCriteriModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string TestCriteriasId { get; set; }
        public List<TestCriteriModel> ListTestCriteri { get; set; }
    }
}
