using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public string Address { get; set; }
        public bool IsExport { get; set; }

        public List<string> ListIdSelect { get; set; }

        public ClassRoomSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
