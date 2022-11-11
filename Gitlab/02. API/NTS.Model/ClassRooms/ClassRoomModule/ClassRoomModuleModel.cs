using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomModuleModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ModuleGroupName { get; set; }
        public string ClassRoomId { get; set; }
        public string ModuleId { get; set; }
        public int Quantity { get; set; }
        public decimal Pricing { get; set; }
    }
}
