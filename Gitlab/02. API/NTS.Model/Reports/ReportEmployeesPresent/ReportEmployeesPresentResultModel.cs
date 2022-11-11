using NTS.Model.EmployeeCourse;
using NTS.Model.Employees;
using NTS.Model.EmployeeSkillDetails;
using NTS.Model.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportEmployeesPresent
{
    public class ReportEmployeesPresentResultModel : BaseModel
    {
        public decimal Total_Employee_Status_Use { get; set; }
        /// <summary>
        /// nhân sự thiếu theo nhóm Cơ khí
        /// </summary>
        public decimal Total_Employee_Incomplete_CK { get; set; }
        /// <summary>
        /// nhân sự thiếu theo nhóm Điện
        /// </summary>
        public decimal Total_Employee_Incomplete_Dn { get; set; }
        /// <summary>
        /// nhân sự thiếu theo nhóm Điện tử
        /// </summary>
        public decimal Total_Employee_Incomplete_Dt { get; set; }
        /// <summary>
        /// Tống số lỗi của nv 
        /// </summary>
        public decimal Total_Error_Employee { get; set; }
        /// <summary>
        /// Số khóa học chưa đào tạo
        /// </summary>
        public decimal Total_Course_Not_Start { get; set; }
        /// <summary>
        /// Số khóa học trễ
        /// </summary>
        public decimal Total_Course_Delay { get; set; }
        public List<ErrorModel> List_Error_Employee { get; set; }
        public List<EmployeeModel> ListEmployeeSkill { get; set; }
        public List<EmployeeCourseModel> ListEmployeeCourse { get; set; }
        public List<PerformanceEmployee> ListPerformanceEmployee { get; set; }
        public List<Performance> ListPerformance { get; set; }
        public List<EmployeeHeader> ListEmployeeHeader { get; set; }
        public List<SkillEmployeeModel> ListSkillEmployee { get; set; }
        public List<ErrorProjectByEmployee> ListErrorByEmployee { get; set; }
        public List<EmployeeProduct> ListErrorEmployeeByProduct { get; set; }
        public List<WorkTypeEmployee> ListWorkType { get; set; }
        public ReportEmployeesPresentResultModel()
        {
            ListEmployeeSkill = new List<EmployeeModel>();
            ListEmployeeCourse = new List<EmployeeCourseModel>();
            List_Error_Employee = new List<ErrorModel>();
            ListPerformanceEmployee = new List<PerformanceEmployee>();
            ListPerformance = new List<Performance>();
            ListEmployeeHeader = new List<EmployeeHeader>();
            ListSkillEmployee = new List<SkillEmployeeModel>();
            ListErrorByEmployee = new List<ErrorProjectByEmployee>();
            ListWorkType = new List<WorkTypeEmployee>();
        }

    }
    public class PerformanceEmployee
    {
        public string PlanId { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleGroupId { get; set; }
        public string ModuleGroupName { get; set; }
        public string ModuleGroupCode { get; set; }
        public DateTime? RealEndDate { get; set; }
        public DateTime? RealStartDate { get; set; }
        /// <summary>
        /// người thực hiện
        /// </summary>
        public string ResponsiblePersion { get; set; }
        public string ResponsiblePersionName { get; set; }
        public int Status { get; set; }
        public decimal ExecutionTime { get; set; }
    }

    public class Performance
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal performance { get; set; }
    }

    public class EmployeeHeader
    {
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public string WorkSkillId { get; set; }
        public decimal Mark { get; set; }
        public decimal Grade { get; set; }
        public List<EmployeeHeader> ListWorkSkillId { get; set; }

        public EmployeeHeader()
    {
            ListWorkSkillId = new List<EmployeeHeader>();
    }
}

    public class SkillEmployeeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Mark { get; set; }
        public decimal Grade { get; set; }
        public string EmployeeId { get; set; }
    }

    public class ProjectModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DepartmentId { get; set; }
        public string SBUId { get; set; }
    }

    public class ErrorProjectByEmployee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public int Count { get; set; }
    }
    public class EmployeeProduct
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public int Count { get; set; }
    }

    public class GroupProduct
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string IndexView { get; set; }

        public string Index { get; set; }

    }

    public class WorkTypeEmployee
    {
        public string WorkTypeName { get; set; }
        public int EmployeePresent { get; set; }
        public decimal EmployeeIncomplete { get; set; }
    }
}
