using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CoefficientEmployee
{
    public class CoefficientEmployeeResultModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int Month { get; set; }
        public int? Year { get; set; }
        public decimal Coefficient { get; set; }
        public List<CoefficientModel> ListCoefficient { get; set; }
        public CoefficientEmployeeResultModel()
        {
            ListCoefficient = new List<CoefficientModel>();
        }
    }

    public class CoefficientModel
    {
        public decimal Coefficient { get; set; }
    }
}
