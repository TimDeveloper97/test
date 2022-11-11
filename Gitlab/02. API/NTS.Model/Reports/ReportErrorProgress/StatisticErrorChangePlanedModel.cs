using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Reports.ReportErrorProgress
{
    public class StatisticErrorChangePlanedModel
    {
        public string DepartmentName { get; set; }
        public string Id { get; set; }

        public int TotalError { get; set; }
        public int TotalPreviousPeriod { get; set; }

        public List<int> NumOfChange { get; set; }
        public StatisticErrorChangePlanedModel()
        {
            NumOfChange = new List<int>();
        }
    }
}
