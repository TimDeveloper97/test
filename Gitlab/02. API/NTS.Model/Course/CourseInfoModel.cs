using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Course
{
    public class CourseInfoModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string CourseTrainingId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal StudyTime { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int Status { get; set; }
        public string DeviceForCourse { get; set; }
        public int ListEmployeeCourseModel { get; set; }
        public int ListEmployeeCourse { get; set; }
        public string EmployeeTraningName { get; set; }
        public string EmployeeTraningCode { get; set; }
    }
}
