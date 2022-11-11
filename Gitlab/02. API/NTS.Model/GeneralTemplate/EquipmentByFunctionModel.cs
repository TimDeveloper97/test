using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.GeneralTemplate
{
    public class EquipmentByFunctionModel : BaseModel
    {
        public string ProductName { get; set; }
        // mã module
        public string ProductCode { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        //public string MainResponsible { get; set; }
        public string UserName { get; set; }
        //public string Parametter { get; set; }
        public bool IsExport { get; set; }
        public string Designer { get; set; }
        public string Specification { get; set; }

    }
}
