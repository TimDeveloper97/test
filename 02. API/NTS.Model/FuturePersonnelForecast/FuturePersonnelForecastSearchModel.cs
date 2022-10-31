using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FuturePersonnelForecast
{
    public class FuturePersonnelForecastSearchModel
    {
        public string ProjectId { get; set; }
        public DateTime Date { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
        public string ResponsiblePersionName { get; set; }

        public List<ReportProjectSearchModel> listBase { get; set; }

        public FuturePersonnelForecastSearchModel()
        {
            listBase = new List<ReportProjectSearchModel>();
        }
    }

    public class ReportProjectSearchModel
    {
        public string Id { get; set; }
        public int Index { get; set; }
    }
}
