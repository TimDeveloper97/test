using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomResultModel : BaseModel
    {
        public string Id { get; set; }
        public string RoomTypeId { get; set; }
        public string Name { get; set; }
        public decimal Pricing { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public string RoomTypeName { get; set; }
        public string ObjectId { get; set; }
        public string MaterialName { get; set; }
    }
}
