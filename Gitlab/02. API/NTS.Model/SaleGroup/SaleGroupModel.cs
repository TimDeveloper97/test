using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SaleGroups
{
    public class SaleGroupModel
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
        /// Ghi chú nhóm kinh doanh
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Danh sách nhân viên
        /// </summary>
        public List<EmployeeModel> ListEmployee { get; set; }

        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        public List<SaleProductModel> ListGroupProduct { get; set; }

        public SaleGroupModel()
        {
            ListEmployee = new List<EmployeeModel>();
            ListGroupProduct = new List<SaleProductModel>();
        }
    }
}
