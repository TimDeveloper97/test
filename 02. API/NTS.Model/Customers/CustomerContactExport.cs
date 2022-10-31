using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Customers
{
    public class CustomerContactExport
    {
        public string Index { get; set; } // stt
        public string Code { get; set; } // mã KH
        public string Name { get; set; } // tên KH
        public string CustomerTypeName { get; set; } //Nhóm KH
        public string SBU { get; set; } //SBU
        public decimal Acreage { get; set; } // diện tích
        public int EmployeeQuantity { get; set; } //Số lượng NV
        public decimal Capital { get; set; } // Vốn
        public string NameContact { get; set; } // người liên hệ
        public string PhoneNumberContact { get; set; } //SĐT
        public string AddressContact { get; set; } //Dchi
        public string Field { get; set; } //Lĩnh vực
        public string Note { get; set; } //Ghi chú
    }
}
