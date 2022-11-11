using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Supplier
{
    public class SupplierSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string ManufactureId { get; set; }
        public string SupplierGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public List<SupplierContactModel> ListSupplierContact { get; set; }
    }

    public class SupplierContactSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
