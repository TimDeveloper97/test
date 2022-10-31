using NTS.Model.Combobox;
using NTS.Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Designer
{
    public class DesignerSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string EmployeeId { get; set; }
        public string Type { get; set; }
        public List<EmployeeModel> ListEmployee { get; set; }
    }
}
