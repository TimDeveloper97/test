using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class ExportAndKeepViewModel
    {
        /// <summary>
        /// Id xuất giữ
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Mã xuất giữ
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Id người tạo
        /// </summary>
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string SbuName { get; set; }

        /// <summary>
        /// Hạn xuất giữ
        /// </summary>
        public System.DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Trạng thái tiến độ
        /// </summary>
        public int PayStatus { get; set; }

        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        public List<SaleProductModel> ListExportAndKeepDetail { get; set; }

        public ExportAndKeepViewModel()
        {
            ListExportAndKeepDetail = new List<SaleProductModel>();
        }
    }
}
