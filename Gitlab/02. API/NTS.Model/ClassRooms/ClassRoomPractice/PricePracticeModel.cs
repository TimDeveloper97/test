using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoomPractice
{
    public class PricePracticeModel
    {
        public decimal HardwarePrice { get; set; }
        public string PracticeId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal LessonPrice { get; set; }
        public int Quantity { get; set; }
    }
}
