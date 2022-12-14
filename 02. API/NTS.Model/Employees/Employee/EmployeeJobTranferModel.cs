using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employee
{
    public class EmployeeJobTranferModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime? DateJobTranfer { get; set; }
        public string WorkTypeId { get; set; }
        public string DepartmentId { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
    }
}
