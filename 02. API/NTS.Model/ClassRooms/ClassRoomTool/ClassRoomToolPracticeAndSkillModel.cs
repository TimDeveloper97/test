using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomTool
{
    public class ClassRoomToolPracticeAndSkillModel
    {
        public List<ClassRoomToolSkillModel> ListSkill { get; set; }
        public List<ClassRoomToolPracticeModel> ListPractice { get; set; }
        public ClassRoomToolPracticeAndSkillModel()
        {
            ListSkill = new List<ClassRoomToolSkillModel>();
            ListPractice = new List<ClassRoomToolPracticeModel>();
        }
    }
}
