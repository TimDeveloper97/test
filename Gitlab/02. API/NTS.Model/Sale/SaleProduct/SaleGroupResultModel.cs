using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
    public class SaleGroupResultModel
    {
        /// <summary>
        /// Id nhóm kinh doanh
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tên nhóm kinh doanh
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
    }
}
