using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistoryManage
{
    public class UserHistoryModel
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SBUId { get; set; }
        public string SBUName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public string ObjectId { get; set; }
        public string Content { get; set; }
    }
}
