using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Task
{
    public class TaskWorkTypeModel
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string WorkTypeRId { get; set; }
        public string WorkTypeAId { get; set; }
        public string WorkTypeSId { get; set; }
        public string WorkTypeCId { get; set; }
        public string WorkTypeIId { get; set; }
        public bool checkDepartment { get; set; } = false;
        public List<ComboboxResult> DepartmentIds { get; set; }
        public int Index { get; set; }
    }
}
