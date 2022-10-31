using NTS.Model.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.DashBroadProject
{
    public class DashBroadProjectResultModel
    {
        /// <summary>
        /// Tổng số dự án đang triển khai
        /// </summary>
        public int TotalProject { get; set; }
        /// <summary>
        /// Dự án hoàn thành
        /// </summary>
        public int TotalProjectFinish { get; set; }
        /// <summary>
        /// Dự án dự kiến bị chậm
        /// </summary>
        public int TotalProjectDelayDeadline { get; set; }
        /// <summary>
        /// Dự án đúng tiến độ
        /// </summary>
        public int TotalProjectOnSchedule { get; set; }
        /// <summary>
        /// Số lượng dự án đamg có hạng mục - chưa lập kế hoạch
        /// </summary>
        public int TotalProjectNotPlan { get; set; }

        ///<summary>
        /// Tổng số project trong bảng plan
        /// </summary>
        public List<DashBroadProjectModel> ListProjectInPlan { get; set; }
        /// <summary>
        /// Danh sách dự án, tình trạng dự án
        /// </summary>
        public List<ProjectResultModel> Projects { get; set; }
    }

}
