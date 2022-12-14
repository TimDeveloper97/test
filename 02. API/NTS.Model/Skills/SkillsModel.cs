using NTS.Model.Practice;
using NTS.Model.PracticeSkill;
using NTS.Model.SkillAttach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Skills
{
    public class SkillsModel : BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SkillGroupId { get; set; }
        public string SkillGroupName { get; set; }
        public string DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string PracticeId { get; set; }
        public List<SkillsModel> listSelect { get; set; }
        public List<SkillAttachModel> ListFile { get; set; }
        public List<PracticeModel> ListData { get; set; }
        public List<PracticeSkillModel> ListPracticeSkill { get; set; }
        public SkillsModel()
        {
            listSelect = new List<SkillsModel>();
            ListFile = new List<SkillAttachModel>();
            ListData = new List<PracticeModel>();
            ListPracticeSkill = new List<PracticeSkillModel>();
        }
    }
}
