using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
    public class RadarViewMapModel
    {
        public string Id { get; set; }
        /// <summary>
        /// Id thiết bị
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Mã thiết bị
        /// </summary>
        public string SerialNumber { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Tốc đọ trung bình xe máy
        /// </summary>
        public decimal AvqSpeedBike { get; set; }

        /// <summary>
        /// Tốc độ TB xe ô tô
        /// </summary>
        public decimal AvqSpeedCar { get; set; }

        /// <summary>
        /// Tốc độ TB xe tải
        /// </summary>
        public decimal AvqSpeedTruck { get; set; }

        /// <summary>
        /// Màu tốc độ
        /// </summary>
        public string SpeedBikeColor { get; set; }
        public string SpeedCarColor { get; set; }
        public string SpeedTruckColor { get; set; }
        public DateTime ReceiptTime { get; set; }
    }
}
