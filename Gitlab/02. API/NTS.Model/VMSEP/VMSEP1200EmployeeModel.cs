using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.VMSEP
{
  public class VMSEP1200EmployeeModel : SearchCommonModel
    { 
        public string UserId { set; get; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PositionName { get; set; }
    }
}
