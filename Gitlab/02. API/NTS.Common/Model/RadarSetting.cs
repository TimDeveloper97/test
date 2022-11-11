using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraCommon.Model
{
   public class RadarSetting
    {
        public string IPServer { get; set; }

        public int Port { get; set; }

        public bool IsRS485 { get; set; }
    }
}
