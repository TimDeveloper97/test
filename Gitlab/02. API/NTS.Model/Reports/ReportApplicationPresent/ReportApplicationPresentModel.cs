using NTS.Model.Project;
using NTS.Model.Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportApplicationPresent
{
    public class ReportApplicationPresentModel
    {   
        /// <summary>
        /// Dự án chưa hoàn thành
        /// </summary>
        public decimal TotalProjectToNotFinish { get; set; }
        /// <summary>
        /// Dự án đã hoàn thành
        /// </summary>
        public decimal TotalProjectFinish { get; set; }
        /// <summary>
        /// Giải pháp đang triển khai 
        /// </summary>
        public decimal TotalSolutionUse { get; set; }
        /// <summary>
        /// Giải pháp thành dự án
        /// </summary>
        public decimal TotalSolutionToProject { get; set; }
        /// <summary>
        /// Giải pháp không thành dự án
        /// </summary>
        public decimal TotalSolutionNotToProject { get; set; }
        /// <summary>
        /// Giải pháp tạm dừng
        /// </summary>
        public decimal TotalSolutionStop { get; set; }
        /// <summary>
        /// Giải pháp bị hủy
        /// </summary>
        public decimal TotalSolutionCancel { get; set; }

        public List<SolutionReport> Solutions { get; set; }
        public List<ProjectReport> Projects { get; set; }

        public ReportApplicationPresentModel()
        {
            Projects = new List<ProjectReport>();
            Solutions = new List<SolutionReport>();
        }

    }
    public class SolutionReport
    {
        public int Status { get; set; }
        public decimal TotalCostSolution { get; set; }
        public List<SolutionModel> ListSolution { get; set; }

    }

    public class ProjectReport
    {
        public string Status { get; set; }
        public decimal TotalCostProject { get; set; }
        public List<ProjectResultModel> ListProject { get; set; }
    }
}
