using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Device
{
     public class InfoSendMail
    {
        public string CustomerName { get; set; }
        public string EmailReceive { get; set; }
        public string ObjectType { get; set; }  
        public string ErrorCode { get; set; }
        public string FixErrorCode { get; set; }

    }
}
