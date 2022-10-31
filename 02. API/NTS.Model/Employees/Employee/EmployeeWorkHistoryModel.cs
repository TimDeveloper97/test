using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employee
{
    public class EmployeeWorkHistoryModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string TotalTime { get; set; }
        public string WorkTypeId { get; set; }
        public string ReferencePerson { get; set; }
        public string ReferencePersonPhone { get; set; }
        public int NumberOfManage { get; set; }
        public decimal Income { get; set; }
    }
}
