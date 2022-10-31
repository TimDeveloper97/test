using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SaleGroups
{
    public class SaleGroupCreateModel
    {
        /// <summary>
        /// Tên nhóm kinh doanh
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ghi chú nhóm kinh doanh
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        public List<SaleGroupUserModel> ListEmployee { get; set; }

        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        public List<SaleGroupProductModel> ListGroupProduct { get; set; }

        public SaleGroupCreateModel()
        {
            ListEmployee = new List<SaleGroupUserModel>();
            ListGroupProduct = new List<SaleGroupProductModel>();
        }
    }
}
