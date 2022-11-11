using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Function
{
    public class FunctionModel : BaseModel
    {
        public string Id { get; set; }
        public string FunctionGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TechnicalRequire { get; set; }
        public string Note { get; set; }

        public List<string> ListIdSelect { get; set; }
        public List<string> ListIdChecked { get; set; }
        public string FunctionGroupName { get; set; }
    }
}
