using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportQualityPresent
{
    public class ReportQualityPresentSearchModel : SearchCommonModel
    {
        public string ProjectId { get; set; }
        public string ModuleGroupId { get; set; }
        public string ErrorGroupId { get; set; }
        // Loại thời gian
        public string TimeType { get; set; }
        /// <summary>
        /// Tháng
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Năm
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Quý
        /// </summary>
        public int Quarter { get; set; }
    }
}
