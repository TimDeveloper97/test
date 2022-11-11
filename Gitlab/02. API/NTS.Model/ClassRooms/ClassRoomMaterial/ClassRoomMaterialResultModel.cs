using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomMaterialResultModel
    {
        /// <summary>
        /// Id vật tư phụ
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id phòng học
        /// </summary>
        public string ClassRoomId { get; set; }

        /// <summary>
        /// Object id các bảng liên kết module, vật tư, thiết bị 
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Giá trị lưu theo list 1: vật tư, 2: module, 3: thiết bị
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int Quantity { get; set; }
        public decimal Pricing { get; set; }

        /// <summary>
        /// Mã vật tư
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên vật tư
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        public DateTime? LastBuyDate { get; set; }
        public decimal PriceHistory { get; set; }
        public DateTime? InputPriceDate { get; set; }

    }
}
