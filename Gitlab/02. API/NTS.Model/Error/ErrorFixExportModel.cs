using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Error
{
    public class ErrorFixExportModel
    {
        public string MaVanDe { get; set; }
        public string MaHangMuc { get; set; }
        public string TenHangMuc { get; set; }
        public string PhanLoai { get; set; }
        public string TenVanDe { get; set; }
        public string MoTa { get; set; }

        public string NguyenNhan { get; set; }
        public string GiaiPhap { get; set; }
        public string NguoiThucHien { get; set; }
        public string BoPhanThucHien { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public string DanhGia { get; set; }
        public string TinhTrang { get; set; }
        public int Done { get; set; }
    }
}
