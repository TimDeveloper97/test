using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomProductSearchModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductGroupId { get; set; }
        public List<string> ListIdSelect { get; set; }
        public ClassRoomProductSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
