using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.Employees;
using NTS.Model.MeetingEmployee;
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
    public class CustomerRequirementInfoModel 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public string CustomerId { get; set; }
        public string CustomerContactId { get; set; }
        public string Code { get; set; }
        public string MeetingCode { get; set; }
        public int RequestType { get; set; }
        public string Petitioner { get; set; }
        public string DepartmentRequest { get; set; }
        public string Reciever { get; set; }
        public string DepartmentReceive { get; set; }
        //public DateTime FinishDate { get; set; }
        public DateTime? RealFinishDate { get; set; }
        public int RequestSource { get; set; }
        //public string Request { get; set; }
        public string Note { get; set; }
        public int Status { get; set; }
        public int Version { get; set; }
        public string UpdateBy { get; set; }
        public int Index { get; set; }
        public string ProjectPhaseId { get; set; }
        public string Competitor { get; set; }
        public string CustomerSupplier { get; set; }
        public int PriorityLevel { get; set; }
        public int Step { get; set; }
        public string TradeConditions { get; set; }
        public decimal TotalPrice { get; set; }
        public string Content1 { get; set; }
        public bool? Conclude1 { get; set; }
        public string Person1 { get; set; }
        public string Content2 { get; set; }
        public bool? Conclude2 { get; set; }
        public string Person2 { get; set; }
        public string Content3 { get; set; }
        public bool? Conclude3 { get; set; }
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


        public List<CustomerRequirementAttachModel> ListAttach { get; set; }
        public List<CustomerRequirementUserInfoModel> ListUser { get; set; }
        public List<CustomerRequirementMaterialInfoModel> ListMaterial { get; set; }
        public List<SurveyCreateModel> ListSurvey { get; set; }
        public List<CustomerRequirementContentModel> ListContent { get; set; }
        public List<SolutionAnalysisRiskModel> ListSolutionAnalysisRisk { get; set; }
        public List<CustomerRequirementAttachModel> ListRequireEstimateMaterialAttach { get; set; }
        public List<CustomerRequirementAttachModel> ListRequireEstimateFCMlAttach { get; set; }
        public List<SolutionAnalysisEstimateModel> ListEstimate { get; set; }
        public List<SolutionAnalysisSupplierModel> ListSupplier { get; set; }
        public List<SolutionAnalysisProductModel> ListProduct { get; set; }
        public List<ProductNeedSolutionModel> ListProductNeedSolution { get; set; }
        public List<ProductNeedPriceModel> ListProductNeedPrice { get; set; }
        public List<SolutionModel> ListSolution { get; set; }
        public List<ProtectSolutionModel> ListProtectSolution { get; set; }
        public List<EmployeeModel> listEmployee { get; set; }
        public string DomainId { get; set; }

        public CustomerRequirementInfoModel()
        {
            ListAttach = new List<CustomerRequirementAttachModel>();
            ListUser = new List<CustomerRequirementUserInfoModel>();
            ListMaterial = new List<CustomerRequirementMaterialInfoModel>();
            ListSurvey = new List<SurveyCreateModel>();
            ListContent = new List<CustomerRequirementContentModel>();
            ListRequireEstimateMaterialAttach = new List<CustomerRequirementAttachModel>();
            ListRequireEstimateFCMlAttach = new List<CustomerRequirementAttachModel>();
            ListSolutionAnalysisRisk = new List<SolutionAnalysisRiskModel>();
            ListEstimate = new List<SolutionAnalysisEstimateModel>();
            ListSupplier = new List<SolutionAnalysisSupplierModel>();
            ListProduct = new List<SolutionAnalysisProductModel>();
            ListProductNeedPrice = new List<ProductNeedPriceModel>();
            ListProductNeedSolution = new List<ProductNeedSolutionModel>();
            ListSolution = new List<SolutionModel>();
            ListProtectSolution = new List<ProtectSolutionModel>();
        }

    }
}
