using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CourseSkill
{
    public class CourseSkillModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CourseId { get; set; }
        public string WorkSkillId { get; set; }
        public string WorkSkillGroupId { get; set; }
        public decimal Mark { get; set; }
        public decimal Score { get; set; }
        public decimal Grade { get; set; }
        public string ParentId { get; set; }
        public string EmployeeId { get; set; }
    }
}
