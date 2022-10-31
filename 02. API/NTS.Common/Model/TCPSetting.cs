using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
   public class TCPSetting
    {
        public string IPServer { get; set; }

        public int Port { get; set; }

        public bool IsRS485 { get; set; }
    }
}
