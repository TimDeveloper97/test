using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class ExportAndKeepResultModel
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
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Id người tạo
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string SaleProductName { get; set; }

        /// <summary>
        /// Tên người tạo
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// Id người tạo
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Id người tạo
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public System.DateTime CreateDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public System.DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Trạng thái
        /// 1: Đã bán
        /// 2: Đang giữ
        /// 3: Trả về kho
        /// 4: Quá hạn
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Tình trạng thanh toán
        /// </summary>
        public int PayStatus { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public decimal Quantity { get; set; }
    }
}
