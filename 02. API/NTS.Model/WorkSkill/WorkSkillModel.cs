using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WorldSkill
{
    public class WorkSkillModel : BaseModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string WorkSkillId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WorkTypeSkillId { get; set; }
        public string WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public List<string> ListIdSelect { get; set; }
        public string WorkSkillGroupId { get; set; }
        public string WorkSkillGroupName { get; set; }
        public decimal Mark { get; set; }
        public decimal Score { get; set; }
        public WorkSkillModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
