using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CoefficientEmployee
{
    public class CoefficientEmployeeCreateModel
    {
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<CoefficientEmployeeResultModel> ListData { get; set; }
        public CoefficientEmployeeCreateModel()
        {
            ListData = new List<CoefficientEmployeeResultModel>();
        }
    }
}
