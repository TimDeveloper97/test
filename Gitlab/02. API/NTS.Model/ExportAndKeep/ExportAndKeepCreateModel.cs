using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ExportAndKeep
{
    public class ExportAndKeepCreateModel
    {
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
        /// SĐT khách hàng
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Địa chỉ khách hàng
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Id người tạo
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// Hạn xuất giữ
        /// </summary>
        public System.DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Tình trạng thanh toán
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
        public List<SaleProductExportDetailModel> ListExportAndKeepDetail { get; set; }

        public ExportAndKeepCreateModel()
        {
            ListExportAndKeepDetail = new List<SaleProductExportDetailModel>();
        }
    }
}
