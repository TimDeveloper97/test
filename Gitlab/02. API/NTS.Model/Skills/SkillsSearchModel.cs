using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Skills
{
    public class SkillsSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SkillGroupId { get; set; }
        public string PracticeId { get; set; }
        public List<string> ListIdSelect { get; set; }

        public SkillsSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
