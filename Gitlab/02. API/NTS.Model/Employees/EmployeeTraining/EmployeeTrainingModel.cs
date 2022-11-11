using NTS.Model.Course;
using NTS.Model.Employees.EmployeeTraining;
using NTS.Model.EmployeeSkills;
using NTS.Model.WorkSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EmployeeTraining
{
    public class EmployeeTrainingModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CreateBy { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Status { get; set; }
        public decimal StudyTime { get; set; }
        public int EmployeeInEmployeeTraining { get; set; }
        public int CourseInEmployeeTraining { get; set; }
        public List<EmployeeTrainingCourseModel> ListCourse { get; set; }
        public List<EmployeeTrainingFileAttachModel> ListAttachs { get; set; }
        public EmployeeTrainingModel()
        {
            ListCourse = new List<EmployeeTrainingCourseModel>();
        }
    }

    public class EmployeeTrainingCourseModel
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal StudyTime { get; set; }
        public string DeviceForCourse { get; set; }
        public bool IsDelete { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<EmployeeTrainingEmployeeModel> ListEmployees { get; set; }
        public EmployeeTrainingCourseModel()
        {
            ListEmployees = new List<EmployeeTrainingEmployeeModel>();
        }
    }

    public class EmployeeTrainingEmployeeModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public bool IsDelete { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string SBUName { get; set; }
        public string SBUId { get; set; }
        public int IsReach { get; set; }
        public string QualificationId { get; set; }
        public string QualificationName { get; set; }
        public string JobPositionId { get; set; }
        public string JobPositionName { get; set; }
        public EmployeeTrainingEmployeeModel()
        {
        }
    }

    public class EmployeeTrainingUpdatePointModel
    {
        public string EmployeeCourseTrainingId { get; set; }
        public string EmployeeId { get; set; }
        public string CourseTrainingId { get; set; }

        public bool CheckSave { get; set; }

        public List<WorkSkillResultModel> ListWorkSkill { get; set; }

        public EmployeeTrainingUpdatePointModel()
        {
            ListWorkSkill = new List<WorkSkillResultModel>();
        }
    }

    public class EmployeeTrainingUpdatePointResultModel
    {
        public int EmployeeStatus { get; set; }
        public int CourseStatus { get; set; }
    }

        public class EmployeeTrainingWorkSkillSearchModel
    {
        public string EmployeeId { get; set; }
        public string CourseId { get; set; }
    }
}
