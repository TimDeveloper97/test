using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomTool
{
    public class PracticeAndProductModel
    {
        public List<string> ListPraticeName { get; set; }
        public List<ClassRoomToolProductModel> ListProducts { get; set; }
        public PracticeAndProductModel()
        {
            ListPraticeName = new List<string>();
            ListProducts = new List<ClassRoomToolProductModel>();
        }
    }
}
