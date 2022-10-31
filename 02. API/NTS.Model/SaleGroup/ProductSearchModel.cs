using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SaleGroups
{
    public class ProductSearchModel
    {
        /// <summary>
        /// Danh sách id bản ghi đã được chọn
        /// </summary>
        public List<string> ListProductId { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Nhóm kinh doanh
        /// </summary>
        public string SaleProductTypeId { get; set; }

        public bool? IsChoose { get; set; }

        public ProductSearchModel()
        {
            ListProductId = new List<string>();
        }
    }
}
