using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CoefficientEmployee
{
    public class CoefficientEmployeeModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Coefficient { get; set; }
    }
}
