using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomPracticeModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PracticeGroupName { get; set; }
        public string ClassRoomId { get; set; }
        public string PracticeId { get; set; }
        public int Quantity { get; set; }
        public decimal HardwarePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal LessonPrice { get; set; }
    }
}
