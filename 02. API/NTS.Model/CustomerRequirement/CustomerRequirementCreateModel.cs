using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.CustomerRequirementSurey;
using NTS.Model.Solution;
using NTS.Model.SolutionAnalysisEstimate;
using NTS.Model.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirement
{
    public class CustomerRequirementCreateModel : BaseModel
    {
        public string Id { get; set; }
        public decimal Budget { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int RequestType { get; set; }
        //public string Request { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public int Version { get; set; }
        public string CreateBy { get; set; }
        public string Petitioner { get; set; }
        public string DepartmentRequest { get; set; }
        public string Reciever { get; set; }
        public string DepartmentReceive { get; set; }
        public int RequestSource { get; set; }
        public string ProjectPhaseId { get; set; }
        public string Competitor { get; set; }
        public string CustomerSupplier { get; set; }
        public int PriorityLevel { get; set; }
        //public DateTime FinishDate { get; set; }
        public DateTime? RealFinishDate { get; set; }
        public int Index { get; set; }
        public string TradeConditions { get; set; }
        public decimal TotalPrice { get; set; }
        public string Content1 { get; set; }
        public bool Conclude1 { get; set; }
        public string Person1 { get; set; }
        public string Content2 { get; set; }
        public bool Conclude2 { get; set; }
        public string Person2 { get; set; }
        public string Content3 { get; set; }
        public bool Conclude3 { get; set; }
        public string Person3 { get; set; }
        public DateTime? PlanFinishDate { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Duration { get; set; }
        public int? CustomerRequirementState { get; set; }
        public int? CustomerRequirementAnalysisState { get; set; }
        public int? SurveyState { get; set; }
        public int? SolutionAnalysisState { get; set; }
        public int? EstimateState { get; set; }
        public int? DoSolutionAnalysisState { get; set; }
        public string MeetingCode { get; set; }
        public string MeetingId { get; set; }

        public List<SurveyUserModel> ListUser { get; set; }
        public List<SurveyToolsModel> ListMaterial { get; set; }
        public List<SurveyCreateModel> ListSurvey { get; set; }
        public List<SolutionAnalysisEstimateModel> ListSolutionAnalysisEstimate { get; set; }

        public List<CustomerRequirementAttachModel> ListAttach { get; set; }

        public List<CustomerRequirementContentModel> ListContent { get; set; }

        public List<SolutionAnalysisRiskModel> ListSolutionAnalysisRisk { get; set; }
        public List<CustomerRequireEstimateAttachModel> ListRequireEstimateMaterialAttach { get; set; }
        public List<CustomerRequireEstimateAttachModel> ListRequireEstimateFCMlAttach { get; set; }
        public string DomainId { get; set; }

        public CustomerRequirementCreateModel()
        {
            ListAttach = new List<CustomerRequirementAttachModel>();
            ListUser = new List<SurveyUserModel>();
            ListMaterial = new List<SurveyToolsModel>();
            ListSurvey = new List<SurveyCreateModel>();
            ListSolutionAnalysisEstimate = new List<SolutionAnalysisEstimateModel>();
            ListRequireEstimateMaterialAttach = new List<CustomerRequireEstimateAttachModel>();
            ListRequireEstimateFCMlAttach = new List<CustomerRequireEstimateAttachModel>();
        }

    }
}
