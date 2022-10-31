using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTS.Model.Solution.SolutionsSupplier
{
    public class SolutionSupplierSearchModel
    {
        /// <summary>
        /// Danh sách id bản ghi đã được chọn
        /// </summary>
        public List<string> ListSupplierId { get; set; }

        /// <summary>
        /// Tên nhà cung cấp
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã nhà cung cấp
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Email nhà cung cấp
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// số điện thoại nhà cung cấp
        /// </summary>
        public int PhoneNumber { get; set; }

        public SolutionSupplierSearchModel()
        {
            ListSupplierId = new List<string>();
        }
    }
}