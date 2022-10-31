using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.VMSMF
{
    public class VMSMF1000ManuafacturerSearchModel : SearchCommonModel
    {
        public string ManuafacturerName { set; get; }
        public string Status { set; get; }
        public string CustomerId { set; get; }
    }
}
