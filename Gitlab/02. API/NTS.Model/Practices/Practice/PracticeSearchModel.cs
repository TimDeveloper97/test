using NTS.Model.Combobox;
using NTS.Model.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Practice
{
    public class PracticeSearchModel: SearchCommonModel
    {
        public string Id { get; set; }
        public string PracticeGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public List<string> SkillName { get; set; }   
        public List<string> SkillId { get; set; }
        public List<string> ListIdSelect { get; set; }
        public List<string> PracaticeId { get; set; }
    }
}
