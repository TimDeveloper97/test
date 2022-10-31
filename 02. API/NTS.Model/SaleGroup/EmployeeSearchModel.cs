using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.SaleGroups
{
    public class EmployeeSearchModel
    {
        /// <summary>
        /// Danh sách id bản ghi đã được chọn
        /// </summary>
        public List<string> ListEmployeeId { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Email nhân viên
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Tình trạng làm việc
        /// </summary>
        public int? Status { get; set; }
        public string DepartermentId { get; set; }

        public EmployeeSearchModel()
        {
            ListEmployeeId = new List<string>();
        }
    }
}
