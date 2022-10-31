using NTS.Model.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportQualityPresent
{
    public class ReportQualityPresentModel
    {
        public decimal Total_Project_Use { get; set; }
        public decimal Total_Problem_Project { get; set; }
        public List<ErrorModel> List_Problem_Project { get; set; }
        public decimal Total_Error_Project { get; set; }
        public decimal Total_Error_ProjectProduct { get; set; }
        public List<ProjectProductErrorModel> List_Error_Project { get; set; }
        public List<ProjectProductErrorModel> List_Error_ProjectProduct { get; set; }

        public ReportQualityPresentModel()
        {
            List_Problem_Project = new List<ErrorModel>();
            List_Error_Project = new List<ProjectProductErrorModel>();
            List_Error_ProjectProduct = new List<ProjectProductErrorModel>();
        }
    }

    public class ProjectProductErrorModel
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
        public string ErrorId { get; set; }
        public string ErrorName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleGroupName { get; set; }
        public int TotalError { get; set; }

    }
}
