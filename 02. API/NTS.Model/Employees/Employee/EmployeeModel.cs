using NTS.Model.Employee;
using NTS.Model.Families;
using NTS.Model.GroupUser;
using NTS.Model.TaskTimeStandardModel;
using NTS.Model.WorkSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Employees
{
    public class EmployeeModel : BaseModel
    {
        public string Id { get; set; }
        public string ModuleCode { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public string QualificationId { get; set; }
        public string QualificationName { get; set; }
        public string JobPositionId { get; set; }
        public string JobPositionName { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmantName { get; set; }
        public string EmployeeId { get; set; }
        public string SBUID { get; set; }
        public string DepartmantId { get; set; }
        public string SBUName { set; get; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public int TimeStandard { get; set; }
        public int Type { get; set; }
        public string BankAccount { get; set; }
        public string SocialInsurrance { get; set; }
        public string HealtInsurrance { get; set; }
        public string TaxCode { get; set; }
        public string IdentifyNum { get; set; }
        public Nullable<System.DateTime> StartWorking { get; set; }
        public Nullable<System.DateTime> EndWorking { get; set; }
        public string Carrer { get; set; }
        public int Status { get; set; }
        public string WorkType { get; set; }
        public bool IsExport { get; set; }
        public decimal Index { get; set; }
        //thông tin user
        public string HomeURL { get; set; }
        public int IsDisable { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string GroupUserId { get; set; }
        public string GroupUserIdOld { get; set; }
        public List<string> ListPermission { get; set; }
        public List<DegreeModel> ListDegreeModel { get; set; }
        // list gia đình
        public List<FamiliesModel> ListFamilies { get; set; }
        public List<TaskTimeStandardResultModel> ListTaskTime { get; set; }
        public string SkillName { get; set; }
        public List<EmployeeModel> ListEmployeSub { get; set; }
        public List<WorkSkillResultModel> ListWorkSkill { get; set; }

        #region Các thông tin bổ sung

        public int IsOfficial { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime? IdentifyDate { get; set; }
        public string IdentifyAddress { get; set; }
        public string AddressProvinceId { get; set; }
        public string AddressDistrictId { get; set; }
        public string AddressWardId { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentAddressProvinceId { get; set; }
        public string PermanentAddressDistrictId { get; set; }
        public string PermanentAddressWardId { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentAddressProvinceId { get; set; }
        public string CurrentAddressDistrictId { get; set; }
        public string CurrentAddressWardId { get; set; }
        public DateTime? StartInsurance { get; set; }
        public string BookNumberInsurance { get; set; }
        public string CardNumberInsurance { get; set; }
        public string Kcb { get; set; }
        public string InsuranceLevelId { get; set; }
        public string Forte { get; set; }
        public int MaritalStatus { get; set; }
        public string Seniority { get; set; }
        public string ReasonEndWorkingId { get; set; }
        public string WorkLocationId { get; set; }
        public string AppliedPositionId { get; set; }
        public string EmployeeGroupId { get; set; }

        public List<EmployeeBankAccountModel> BankAccounts { get; set; }
        public List<EmployeeWorkHistoryModel> WorkHistories { get; set; }
        public List<EmployeeJobTranferModel> HistoryJobTranfer { get; set; }
        public List<EmployeeHistoryAppointModel> HistoryAppoint { get; set; }
        public List<EmployeeHistoryIncomeModel> HistoriesIncome { get; set; }
        public List<EmployeeHistoryInsuranceModel> HistoriesInsurance { get; set; }
        public List<EmployeeHistoryLaborContractModel> HistoriesLaborContract { get; set; }
        #endregion

        public EmployeeModel()
        {
            ListEmployeSub = new List<EmployeeModel>();
            ListWorkSkill = new List<WorkSkillResultModel>();
            BankAccounts = new List<EmployeeBankAccountModel>();
            WorkHistories = new List<EmployeeWorkHistoryModel>();
            HistoryJobTranfer = new List<EmployeeJobTranferModel>();
            HistoryAppoint = new List<EmployeeHistoryAppointModel>();
            HistoriesIncome = new List<EmployeeHistoryIncomeModel>();
            HistoriesInsurance = new List<EmployeeHistoryInsuranceModel>();
            HistoriesLaborContract = new List<EmployeeHistoryLaborContractModel>();
        }
    }

    public class FamiliesContactModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Relationship { get; set; }
        public string Gender { get; set; }
    }

    public class ImportEmployee
    {
        public List<EmployeeModel> ListExist { get; set; }
        public string Message { get; set; }
        public ImportEmployee()
        {
            ListExist = new List<EmployeeModel>();
        }
    }
}
