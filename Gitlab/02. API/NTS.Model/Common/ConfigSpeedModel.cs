using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
    public class ConfigSpeedModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int SpeedFrom { get; set; }
        public int SpeedTo { get; set; }
    }
}
