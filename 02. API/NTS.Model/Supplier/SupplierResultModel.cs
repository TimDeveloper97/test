using NTS.Model.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Supplier
{
    public class SupplierResultModel
    {
        public string Id { get; set; }
        public string SupplierGroupId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string SupplierGroupName { get; set; }
        public List<SupplierContactModel> ListSupplierContact { get; set; }
        public string SupplierContactName { get; set; }
        public string SupplierContactPhoneNumber { get; set; }
        public string SupplierContactEmail { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureName { get; set; }
        public string Country { get; set; }
        public string BankPayment { get; set; }
        public int? TypePayment { get; set; }
        public string RulesPayment { get; set; }
        public int? RulesDelivery { get; set; }
        public string DeliveryTime{ get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public List<ManufactureModel> ListManuFacture { get; set; }
    }

    public class SupplierContactResultModel
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
