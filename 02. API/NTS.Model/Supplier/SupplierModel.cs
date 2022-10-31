using NTS.Model.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Supplier
{
    public class SupplierModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string ManufactureId { get; set; }
        public string Country { get; set; }
        public string BankPayment { get; set; }
        public int? TypePayment { get; set; }
        public string RulesPayment { get; set; }
        public int? RulesDelivery { get; set; }
        public string DeliveryTime { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }

        public List<string> ListSupplierGroupId = new List<string>();
        public List<SupplierContactModel> ListSupplierContact { get; set; }
        public List<ManufactureResultModel> ListManfacture { get; set; }
        public List<SuppilerContractModel> Contracts { get; set; }

    }

    public class SuppilerContractModel : BaseModel
    {
        public string Id { get; set; }
        public string LaborContractId { get; set; }
        public string LaborContractName { get; set; }
        public string SupplierId { get; set; }
        public string FileName { get; set; }
        public decimal FileSize { get; set; }
        public string Path { get; set; }
        public string CreateByName { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? SignDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }

    public class SupplierContactModel : BaseModel
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class ImportSupplier
    {
        public List<SupplierModel> ListExist { get; set; }
        public string Message { get; set; }
        public ImportSupplier()
        {
            ListExist = new List<SupplierModel>();
        }
    }
}
