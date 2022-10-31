using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Supplier
{
    public class SupplierContractModel
    {
        public string Id { get; set; }
        public string LaborContractId { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
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
    }
}
