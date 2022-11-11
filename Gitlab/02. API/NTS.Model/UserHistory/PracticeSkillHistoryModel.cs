using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.PracticeSkillHistory
{
    public class PracticeSkillHistoryModel
    {
        public string Id { get; set; }
        public string SkillGroupId { get; set; }
        public string DegreeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string Description { get; set; }
    }
}
