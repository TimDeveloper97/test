using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomMaterial
{
    public class ClassRoomMaterialSearch
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string MaterialGroupId { get; set; }

        public List<string> ListIdSelect { get; set; }
        public ClassRoomMaterialSearch()
        {
            ListIdSelect = new List<string>();
        }
    }
}
