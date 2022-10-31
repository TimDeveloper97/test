using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Skills
{
    public class SkillResultModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SkillGroupId { get; set; }
        public string ParentId { get; set; }
        public string SkillGroupName { get; set; }
        public string SkillGroupName1 { get; set; }
        public string SkillGroupName2 { get; set; }
        public string SkillGroupName3 { get; set; }
        public string SkillGroupName4 { get; set; }
        public string SkillGroupName5 { get; set; }
        public string SkillGroupName6 { get; set; }
        public string DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public List<SkillsModel> listSelect { get; set; }
    }
}
