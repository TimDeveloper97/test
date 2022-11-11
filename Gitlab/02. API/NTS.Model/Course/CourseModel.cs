using NTS.Model.CourseAttach;
using NTS.Model.CourseSkill;
using NTS.Model.EmployeeCourse;
using NTS.Model.Employees;
using NTS.Model.WorkSkill;
using NTS.Model.WorldSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Course
{
    public class CourseModel : BaseModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string CourseTrainingId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal StudyTime { get; set; }
        public string ParentId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public int Status1 { get; set; }
        public string DeviceForCourse { get; set; }
        public int ListEmployeeCourseModel { get; set; }
        public int ListEmployeeCourse { get; set; }
        public List<CourseAttachModel> ListFile { get; set; }
        public List<CourseSkillModel> ListCourseSkill { get; set; }
        public List<EmployeeModel> ListEmployees { get; set; }
        public List<string> ListId { get; set; }
        //public List<WorkSkillResultModel> ListWorkSkill { get; set; }
        public CourseModel()
        {
            ListEmployees = new List<EmployeeModel>();
            ListId = new List<string>();
            //ListWorkSkill = new List<WorkSkillResultModel>();
        }

    }
}
