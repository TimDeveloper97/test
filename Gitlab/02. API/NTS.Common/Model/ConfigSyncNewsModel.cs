using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common.Model
{
    public class ConfigSyncNewsModel
    {
        /// <summary>
        /// Link danh mục
        /// </summary>
        public string LinkCategory { get; set; }
        /// <summary>
        /// Tên parameter phân trang danh mục
        /// </summary>
        public string ParameterPage { get; set; }
        /// <summary>
        /// Từ trang
        /// </summary>
        public decimal PageFrom { get; set; }
        /// <summary>
        /// Đến trang
        /// </summary>
        public decimal PageTo { get; set; }
        /// <summary>
        /// Thẻ chứa tin tức mới
        /// </summary>
        public string TagNewNews { get; set; }
        /// <summary>
        /// Thẻ chứa tin tức
        /// </summary>
        public string TagContainNews { get; set; }

        /// <summary>
        /// Thẻ tin tức trong danh mục
        /// </summary>
        public string TagNewsInCategory { get; set; }

        /// <summary>
        /// Danh mục đồng bộ vào
        /// </summary>
        public string CategorySync { get; set; }

        /// <summary>
        /// Trạng thái publish bài viết sau khi đồng bộ
        /// </summary>
        public string IsPublish { get; set; }

        /// <summary>
        /// Thẻ chứa tin tức
        /// </summary>
        public string TagNewsDetails { get; set; }

        /// <summary>
        /// Thẻ chứa tiêu đề
        /// </summary>
        public string TagNewsDate { get; set; }

        /// <summary>
        /// Thẻ chứa tiêu đề
        /// </summary>
        public string TagNewsDescription { get; set; }

        /// <summary>
        /// Thẻ chứa nội dung
        /// </summary>
        public string TagNewsContent { get; set; }

        /// <summary>
        /// Thẻ chứ nội dung cần loại bỏ
        /// </summary>
        public string TagRemove { get; set; }
    }
}
