using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GeneralTemplate
{
    public class GeneralCheckListModel : BaseModel
    {
        public string ProductName { get; set; }
        // mã module
        public string ProductCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string UserCheck { get; set; }
        public bool IsExport { get; set; }

    }
}
