using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;
using NTS.Model.Common;

namespace NTS.Model.JobPosition
{
    public class JobPositionSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
    }
}
