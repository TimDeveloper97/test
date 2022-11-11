using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EMSEP
{
   public class EMSEP1000:SearchCommonModel
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Email { get; set; } 
        public string Status { get; set; }
        public string PositionId { get; set; } 
        public string PositionName { get; set; }
        public string Account { get; set; }
    }
}
