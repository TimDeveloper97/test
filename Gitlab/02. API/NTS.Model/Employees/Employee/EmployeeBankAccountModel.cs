using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employee
{
    public class EmployeeBankAccountModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string BankAccountId { get; set; }
        public string AccountNumber { get; set; }
    }
}
