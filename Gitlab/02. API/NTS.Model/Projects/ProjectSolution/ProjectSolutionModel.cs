using NTS.Model.Solution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectSolution
{
    public class ProjectSolutionModel
    {
        public int TotalItems { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string Customer { get; set; }
        public string SBUName { get; set; }
        public string DepartmentName { get; set; }
        public string Id { get; set; }
        public string SolutionId { get; set; }
        public List<SolutionModel> ListResult { get; set; }
        public ProjectSolutionModel()
        {
            ListResult = new List<SolutionModel>();
            ListProduct = new List<ProjectProductToSolutionModel>();
        }
        public string ProjectProductSolutionId { get; set; }
        public List<ProjectProductToSolutionModel> ListProduct { get; set; }
    }
}
