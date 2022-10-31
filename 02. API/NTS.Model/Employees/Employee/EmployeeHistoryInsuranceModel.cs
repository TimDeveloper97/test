using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employee
{
    public class EmployeeHistoryInsuranceModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string ReasonChangeInsuranceId { get; set; }
        public decimal InsuranceMoney { get; set; }
        public DateTime? DateChange { get; set; }
        public string InsuranceLevelId { get; set; }
    }
}
