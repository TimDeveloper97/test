using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.ModuleRoomType
{
    public class RoomTypeSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<string> ListIdSelect { get; set; }

        public RoomTypeSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
