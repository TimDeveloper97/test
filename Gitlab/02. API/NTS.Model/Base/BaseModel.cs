using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model
{
    public class BaseModel
    {
        /// <summary>
        /// Id người tạo
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Id người cập nhật
        /// </summary>
        public string UpdateBy { get; set; }
    }
}
