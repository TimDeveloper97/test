using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomTool
{
    public class ClassRoomToolModel
    {
        public List<ClassRoomToolSkillModel> ListSkill { get; set; }
        public List<ClassRoomToolPracticeModel> ListPractice { get; set; }
        public List<ClassRoomToolProductModel> ListProduct { get; set; }
        public ClassRoomToolModel()
        {
            ListSkill = new List<ClassRoomToolSkillModel>();
            ListPractice = new List<ClassRoomToolPracticeModel>();
            ListProduct = new List<ClassRoomToolProductModel>();
        }
    }
}
