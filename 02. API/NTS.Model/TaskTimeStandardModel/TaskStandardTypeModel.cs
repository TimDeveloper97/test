using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.TaskTimeStandardModel
{
    public class TaskStandardTypeModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string TaskId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal TimeStandard { get; set; }

        public List<string> ListType = new List<string>();
}
}
