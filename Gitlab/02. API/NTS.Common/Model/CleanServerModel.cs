using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
    public class CleanServerModel
    {
        /// <summary>
        /// Thư mục lưu ảnh cần dọn dẹp
        /// </summary>
        public string PathFoldelImage { get; set;}
        /// <summary>
        /// Xóa file tạo trước bao nhiêu h
        /// </summary>
        public double TotalHour { get; set; }
    }
}
