using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomTool
{
    public class PracticeAndSkillModel
    {
        public List<string> ListSkillName { get; set; }
        public List<ClassRoomToolPracticeModel> ListPractices { get; set; }
        public PracticeAndSkillModel()
        {
            ListSkillName = new List<string>();
            ListPractices = new List<ClassRoomToolPracticeModel>();
        }
    }
}
