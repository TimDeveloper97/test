using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomModuleSearchModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ModuleGroupId { get; set; }

        public List<string> ListIdSelect { get; set; }
        public ClassRoomModuleSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
