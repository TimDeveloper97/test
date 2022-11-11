using NTS.Model.CustomerRequirement;
using NTS.Model.ProjectSolution;
using NTS.Model.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Solution
{
    public class SolutionModel : BaseModel
    {
        public string Id { get; set; }
        public string SolutionGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SBUName { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string EndCustomerId { get; set; }
        public string EndCustomerName { get; set; }
        public string SBUBusinessId { get; set; }
        public string DepartmentBusinessId { get; set; }
        public string BusinessUserId { get; set; }
        public string SBUId { get; set; }
        public string TPAUName { get; set; }
        public string SolutionMaker { get; set; }
        public string SolutionMakerName { get; set; }
        public string SBUSolutionMakerId { get; set; }
        public string DepartmentSolutionMakerId { get; set; }
        public string Phone { get; set; }
        public decimal? Price { get; set; }
        public DateTime? FinishDate { get; set; }
        public string SolutionGroupName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string Path { get; set; }
        public bool Has3DSolution { get; set; }
        public bool Has2D { get; set; }
        public bool HasExplan { get; set; }
        public bool HasDMVT { get; set; }
        public bool HasFCM { get; set; }
        public bool HasTSKT { get; set; }
        public bool Design3DExist { get; set; }
        public bool Design2DExist { get; set; }
        public bool ExplanExist { get; set; }
        public bool DMVTExist { get; set; }
        public bool FCMExist { get; set; }
        public bool TSTKExist { get; set; }
        public string CreateByName { get; set; }
        public int Status { get; set; }
        public int CountData { get; set; }
        public int ProjectSolution { get; set; }
        public decimal SaleNoVat { get; set; }
        public DateTime? StartDate { get; set; }
        public string DepartmentId { get; set; }
        public int Index { get; set; }
        public bool IsDelete { get; set; }
        public string ProjectSolutionId { get; set; }
        public string ProjectProductSolutionId { get; set; }
        public int CurentVersion { get; set; }
        public string EditContent { get; set; }
        public int? EstimateState { get; set; }
        public int NumberAttach { get; set; }
        public List<string> SolutionTechnologies { get; set; }
        public string StatusRequirementSolution { get; set; }
        /// <summary>
        /// người thiết kế cơ khí
        /// </summary>
        public string MechanicalMaker { get; set; }

        /// <summary>
        /// người thiết kế điện
        /// </summary>
        public string ElectricMaker { get; set; }

        /// <summary>
        /// người thiết kế điện tử
        /// </summary>
        public string ElectronicMaker { get; set; }
        public List<SolutionAttachModel> ListFile { get; set; }
        public List<ProjectSolutionModel> ListProjectSolution { get; set; }

        public List<SolutionImageModel> ListImage { get; set; }
        public string BusinessDomain { get; set; }
        public string CustomerRequirementId { get; set; }
        public bool IsEnoughDocument { get; set; }
        public int? SurveyState { get; set; }
        public string ProjectPhaseId { get; set; }
        public List<SurveyCreateModel> ListSurvey { get; set; }
        public int? DoSolutionAnalysisState { get; set; }
        public decimal TotalPrice { get; set; }
        public string TradeConditions { get; set; }
        public List<CustomerRequirementAttachModel> ListRequireEstimateMaterialAttach { get; set; }
        public string CustomerRequirementCode { get; set; }
        public bool IsFAS { get; set; }
        public bool IsLAS { get; set; }
        public bool IsEST { get; set; }
        public string JobId { get; set; }
        public string ApplicationId { get; set; }

        public SolutionModel()
        {
            ListFile = new List<SolutionAttachModel>();
            ListProjectSolution = new List<ProjectSolutionModel>();
            ListImage = new List<SolutionImageModel>();
            SolutionTechnologies = new List<string>();
            ListSurvey = new List<SurveyCreateModel>();
            ListRequireEstimateMaterialAttach = new List<CustomerRequirementAttachModel>();
        }


    }
    public class SolutionImageModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string Note { get; set; }
        public int Type { get; set; }
    }
}
