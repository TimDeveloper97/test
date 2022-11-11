using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
    public class CbbModelDeviceModel
    {
        public string DeviceModelId { get; set; }
        public string ModelName { get; set; }
        public string Note { get; set; }
        public int Floor { get; set; }
        public int SlotOfFloor { get; set; }
    }
}
