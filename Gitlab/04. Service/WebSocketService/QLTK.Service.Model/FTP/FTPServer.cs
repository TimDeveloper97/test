using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Model.FTP
{
    public class FTPServer
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string ServerIP { get; set; }
        public int Port { get; set; }
    }
}
