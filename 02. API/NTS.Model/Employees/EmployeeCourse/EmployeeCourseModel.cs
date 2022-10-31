using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EmployeeCourse
{
    public class EmployeeCourseModel: BaseModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Rate { get; set; }
        public bool? Status { get; set; }
        public System.DateTime CourseDate { get; set; }
    }
}
