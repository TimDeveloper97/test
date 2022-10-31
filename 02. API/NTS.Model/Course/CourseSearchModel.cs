using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Course
{
    public class CourseSearchModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal StudyTime { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string DeviceForCourse { get; set; }
        public List<string> ListIdSelect { get; set; }
    }
}
