using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class ExportAndKeepModel
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
        /// Id khách hàng
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Id người tạo
        /// </summary>
        public string CreateBy { get; set; }
        public string employeeName { get; set; }
        public string employeeCode { get; set; }
        public string email { get; set; }
        public string departmentName { get; set; }
        public string CustomerName { get; set; }
        public string SbuName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        /// <summary>
        /// Hạn xuất giữ
        /// </summary>
        public System.DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Trạng thái thanh toán
        /// </summary>
        public int PayStatus { get; set; }

        /// <summary>
        /// Tiến độ thanh toán
        /// </summary>
        public decimal PaymentPercent { get; set; }

        /// <summary>
        /// Số tiền đã thanh toán
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        public List<SaleProductModel> ListExportAndKeepDetail { get; set; }

        public ExportAndKeepModel()
        {
            ListExportAndKeepDetail = new List<SaleProductModel>();
        }
    }
}
