using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ClassRoom
{
    public class ClassRoomSkillModel
    {
        /// <summary>
        /// Id liên kết
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id phòng học
        /// </summary>
        public string ClassRoomId { get; set; }

        /// <summary>
        /// Id kĩ năng
        /// </summary>
        public string SkillId { get; set; }

        /// <summary>
        /// Mã kĩ năng
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên kĩ năng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên nhóm kĩ năng
        /// </summary>
        public string SkillGroupName { get; set; }

        /// <summary>
        /// Mô tả kĩ năng
        /// </summary>
        public string Description { get; set; }
    }
}
