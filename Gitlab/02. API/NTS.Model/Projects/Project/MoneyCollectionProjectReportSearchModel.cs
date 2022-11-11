using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Projects.Project
{
    public class MoneyCollectionProjectReportSearchModel
    {
        public string ProjectId { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public bool IsExport { get; set; }
    }
}
