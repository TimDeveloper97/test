using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
    public class EmailModel
    { 
        public string Username { get; set; } // Username
        public string Password { get; set; } // Password 
        public string Host { get; set; } // Host
        public int? Port { get; set; } // Port  
    }
}
