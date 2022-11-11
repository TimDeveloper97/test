using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
    public class MessageRadarLane
    {
        public string RadarIP { get; set; }
        public int LaneId { get; set; }
        public string SerialNo { get; set; }

        public decimal VolumeUndefine { get; set; }
        public decimal VolumePedestrian { get; set; }
        public decimal VolumeBike { get; set; }
        public decimal VolumeCar { get; set; }
        public decimal VolumeTruck { get; set; }

        public decimal OccupancyUndefine { get; set; }
        public decimal OccupancyPedestrian { get; set; }
        public decimal OccupancyBike { get; set; }
        public decimal OccupancyCar { get; set; }
        public decimal OccupancyTruck { get; set; }

        public decimal AvqSpeedUndefine { get; set; }
        public decimal AvqSpeedPedestrian { get; set; }
        public decimal AvqSpeedBike { get; set; }
        public decimal AvqSpeedCar { get; set; }
        public decimal AvqSpeedTruck { get; set; }

        public decimal P85SpeedUndefine { get; set; }
        public decimal P85SpeedPedestrian { get; set; }
        public decimal P85SpeedBike { get; set; }
        public decimal P85SpeedCar { get; set; }
        public decimal P85SpeedTruck { get; set; }

        public DateTime DateTimeLog { get; set; }
    }
}
