
using NTS.Model.Practice;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeSkill
{
    public class PracticeSkillModel:BaseModel
    {
        public string Id { get; set; }
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public string PracticeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public bool IsExport { get; set; }
        public List<PracticeModel> ListPractice { get; set; }
    }
}
