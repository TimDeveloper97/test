using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProductStandardTPAs
{
    public class SupplierModel
    {
        public string Id { get; set; }
        public string Supplier_NCC_SX { get; set; }
        public string Code { get; set; }
        public string Country_NCC_SX { get; set; }
        public string Website_NCC_SX { get; set; }
        public string Name_NCC_SX { get; set; }
        public string Address_NCC_SX { get; set; }
        public string PIC_NCC_SX { get; set; }
        public string PhoneNumber_NCC_SX { get; set; }
        public string Email_NCC_SX { get; set; }
        public string Title_NCC_SX { get; set; }
        public string BankPayment_NCC_SX { get; set; }
        public int? TypePayment_NCC_SX { get; set; }
        public string RulesPayment_NCC_SX { get; set; }
        public int? RulesDelivery_NCC_SX { get; set; }
        public string DeliveryTime { get; set; }
    }
}
