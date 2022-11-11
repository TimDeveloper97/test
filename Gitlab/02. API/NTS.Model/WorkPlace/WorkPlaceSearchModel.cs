using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.WorkPlace
{
    public class WorkPlaceSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<string> ListIdSelect { get; set; }

        public WorkPlaceSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
