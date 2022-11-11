using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
    public class RadarModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal EndLatitude { get; set; }
        public decimal EndLongitude { get; set; }
        public decimal StartLatitude { get; set; }
        public decimal StartLongitude { get; set; }
        public string Note { get; set; }
        public string IpAddress { get; set; }
        public string Address { get; set; }
        public string SerialNumber { get; set; }
    }
}
