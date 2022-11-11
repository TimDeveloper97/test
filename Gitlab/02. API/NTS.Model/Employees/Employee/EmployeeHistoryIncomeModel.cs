using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employee
{
    public class EmployeeHistoryIncomeModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string ReasonChangeIncomeId { get; set; }
        public decimal NewIncome { get; set; }
        public DateTime? DateChange { get; set; }
    }
}
