using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SaleGroups
{
    /// <summary>
    /// Model dữ liệu tìm kiếm
    /// </summary>
    public class SearchSaleGroupModel : SearchCommonModel
    {
        /// <summary>
        /// Tìm kiếm theo tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tìm kiếm theo ghi chú
        /// </summary>
        public string Note { get; set; }
    }
}
