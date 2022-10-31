using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EmployeeEducation
{
    public class EmployeeEducationModel : BaseModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string EducationId { get; set; }
        public string Rate { get; set; }
        public string Status { get; set; }
        public System.DateTime EducationDate { get; set; }
    }
}
