using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomTool
{
    public class ClassRoomToolProductModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<ClassRoomToolPracticeProductModel> ListCheckPracticeProduct { get; set; }
        public ClassRoomToolProductModel()
        {
            ListCheckPracticeProduct = new List<ClassRoomToolPracticeProductModel>();
        }
    }

    public class ClassRoomToolPracticeProductModel
    {
        public string ProductId { get; set; }
        public string PracticeId { get; set; }
        public bool Check { get; set; }
    }
}
