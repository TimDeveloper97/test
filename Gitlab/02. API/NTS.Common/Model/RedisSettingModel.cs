using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
    public class RedisSettingModel
    {
        /// <summary>
        /// Tên/ IP server
        /// </summary>
        public string Server { get; set; }

        public string Port { get; set; }

        public string Pass { get; set; }
    }
}
