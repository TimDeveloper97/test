using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomTool
{
    public class ClassRoomToolPracticeAndProductModel
    {
        public List<ClassRoomToolPracticeModel> ListPractice { get; set; }
        public List<ClassRoomToolProductModel> ListProduct { get; set; }
        public ClassRoomToolPracticeAndProductModel()
        {
            ListPractice = new List<ClassRoomToolPracticeModel>();
            ListProduct = new List<ClassRoomToolProductModel>();
        }
    }
}
