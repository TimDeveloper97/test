using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
   public class DbConnection
    {
        /// <summary>
        /// Tên/ IP server
        /// </summary>
        public string Server;

        /// <summary>
        /// Tên database
        /// </summary>
        public string Database;

        /// <summary>
        /// Tên User
        /// </summary>
        public string User;
        
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string Password;
    }
}
