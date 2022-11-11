using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MasterLibrary
{
    public class MasterLibraryModel: SearchCommonModel
    {
        public string SubjectsId { set; get; }
        public string SubjectsName { set; get; }
        public string SubjectsCode { set; get; }
        public string SkillId { set; get; }
        public string SkillName { set; get; }
        public string SkillCode { set; get; }
        public string PracticeId { set; get; }
        public string PracticeName { set; get; }
        public string PracticeCode { set; get; }
        public string ModuleId { set; get; }
        public string ModuleName { set; get; }
        public string ModuleCode { set; get; }
    }
}
