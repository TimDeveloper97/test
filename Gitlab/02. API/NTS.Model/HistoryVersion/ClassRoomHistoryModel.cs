using NTS.Model.ClassRoom;
using NTS.Model.ClassRoomAttach;
using NTS.Model.ClassRoomProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class ClassRoomHistoryModel
    {
        public string Id { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string Name { get; set; }
        public decimal Pricing { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public string ObjectId { get; set; }
        public string MaterialName { get; set; }

        public List<ClassRoomMaterialResultModel> ListMaterial { get; set; }
        public List<ClassRoomModuleModel> ListModule { get; set; }
        public List<ClassRoomPracticeModel> ListPractice { get; set; }
        public List<ClassRoomProductModel> ListProduct { get; set; }

        public List<ClassRoomAttachModel> ListFile { get; set; }
        public ClassRoomHistoryModel()
        {
            ListFile = new List<ClassRoomAttachModel>();
        }
    }
}
