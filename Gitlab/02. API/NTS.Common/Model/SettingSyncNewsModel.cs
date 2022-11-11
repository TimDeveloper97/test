using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
    public class SettingSyncNewsModel
    {
        /// <summary>
        /// Liên tục
        /// </summary>
        public bool Continuity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Hàng ngày
        /// </summary>
        public bool Daily { get; set; }
        public string Time { get; set; }
    }
}
