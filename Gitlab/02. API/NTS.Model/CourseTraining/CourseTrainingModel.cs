using NTS.Model.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CourseTraining
{
    public class CourseTrainingModel
    {
        public string Id { get; set; }
        public string EmployeeTrainingId { get; set; }
        public string ListEmployeeId { get; set; }
        public List<EmployeeModel> ListEmployees { get; set; }

        public int Status { get; set; }
    }
}
