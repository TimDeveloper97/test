using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.VMSMF
{
    public class VMSMF1000ManuafacturerModel : Common.CommonModel
    {
        public string ManuafacturerId { set; get; }
        public string LogUserId { set; get; }
        public string ManuafacturerName { set; get; }
        public string Note { set; get; }
        public string CustomerId { set; get; }
        public string ObjectType { set; get; }
        public string Status { set; get; }
        public int TotalRows { get; set; }
    }
}
