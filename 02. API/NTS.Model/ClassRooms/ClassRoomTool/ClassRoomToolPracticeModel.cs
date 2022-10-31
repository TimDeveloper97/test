using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomTool
{
    public class ClassRoomToolPracticeModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ClassRoomToolPracticeSkillModel> ListCheckPracticeSkill { get; set; }
        public ClassRoomToolPracticeModel()
        {
            ListCheckPracticeSkill = new List<ClassRoomToolPracticeSkillModel>();
        }
    }

    public class ClassRoomToolPracticeSkillModel
    {
        public string SkillId { get; set; }
        public string PracticeId { get; set; }
        public bool Check { get; set; }
    }
}
