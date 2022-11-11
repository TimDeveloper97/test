using NTS.Model.CustomerRequirementEmployee;
using NTS.Model.CustomerRequirementMaterial;
using NTS.Model.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerRequirement
{
    public class CustomerRequirementSearchResultModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public decimal Budget { get; set; }
        public string CustomerContactId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int RequestType { get; set; }
        public string Petitioner { get; set; }
        public string DepartmentRequest { get; set; }
        public string Reciever { get; set; }
        public string DepartmentReceive { get; set; }
        public int RequestSource { get; set; }
        //public string Request { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }
        public int? Step { get; set; }
        public int Version { get; set; }
        public string ProjectPhaseId { get; set; }
        public string Competitor { get; set; }
        public string CustomerSupplier { get; set; }
        public int PriorityLevel { get; set; }
        //public DateTime FinishDate { get; set; }
        public DateTime? RealFinishDate { get; set; }
        public string DepartmentRequestName { get; set; }
        public string DepartmentReceiveName { get; set; }
        public string PetitionerName { get; set; }
        public string RecieverName { get; set; }
        public int? CustomerRequirementState { get; set; }
        public int? CustomerRequirementAnalysisState { get; set; }
        public int? SurveyState { get; set; }
        public int? SolutionAnalysisState { get; set; }
        public int? EstimateState { get; set; }
        public int? DoSolutionAnalysisState { get; set; }
        public string EmployeeId { get; set; }

        public List<SurveyCreateModel> ListSurvey { get; set; }
        public List<CustomerRequirementUserInfoModel> ListUser { get; set; }
        public List<CustomerRequirementMaterialInfoModel> ListMaterial { get; set; }
        public string CustomerName { get; set; }
    }
}
