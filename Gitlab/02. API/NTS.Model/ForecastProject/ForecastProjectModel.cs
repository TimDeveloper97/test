using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ForecastProject
{
    public class ForecastProjectModel
    {
        public string Name { get; set; }

        /// <summary>
        /// Tỷ lệ hoàn thành dự án
        /// </summary>
        public double FinishProject { get; set; }

        /// <summary>
        /// Số đầu việc chưa hoàn thành
        /// </summary>
        public int TotalUnfinishedPlan { get; set; }

        /// <summary>
        /// Số đầu việc chưa có kế hoạch
        /// </summary>
        public int TotalUnplanned { get; set; }

        /// <summary>
        /// Số lượng đầu việc có kế hoạch nhưng chậm so với deadline
        /// </summary>
        public int PlanSlow { get; set; }
    }
}
