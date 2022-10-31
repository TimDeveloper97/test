using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistory
{
    public class CourseHistoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal StudyTime { get; set; }
        public Nullable<bool> Status { get; set; }
        public string DeviceForCourse { get; set; }
    }
}
