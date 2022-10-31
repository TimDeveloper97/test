using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class ExportAndKeepSearchModel : SearchCommonModel
    {
        /// <summary>
        /// Mã xuất giữ
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Ngày tạo đến
        /// </summary>
        public System.DateTime? CreateDateTo { get; set; }

        /// <summary>
        /// Ngày tạo từ
        /// </summary>
        public System.DateTime? CreateDateFrom { get; set; }

        /// <summary>
        /// Hến hạn đến
        /// </summary>
        public System.DateTime? ExpiredDateTo { get; set; }

        /// <summary>
        /// Hết hạn từ
        /// </summary>
        public System.DateTime? ExpiredDateFrom { get; set; }

        /// <summary>
        /// Trạng thái
        /// 1: Đã bán
        /// 2: Đang giữ
        /// 3: Trả về kho
        /// 4: Quá hạn
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// Loại tìm kiếm
        /// </summary>
        public int? QuantityType { get; set; }

        /// <summary>
        ///Người tạo
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Nhà cung cấp
        /// </summary>
        public string SupplierId { get; set; } 

        /// <summary>
        /// Tình trạng thanh toán
        /// </summary>
        public int? PayStatus { get; set; }
    }
}
