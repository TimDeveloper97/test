using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TestCriteriaGroup
{
    public class TestCriteriaGroupModel: BaseModel
    {
        public string Id { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string Note { set; get; }
    }
}
