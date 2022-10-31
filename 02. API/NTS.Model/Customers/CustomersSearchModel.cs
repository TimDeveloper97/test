using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Customers
{
    public class CustomersSearchModel : SearchCommonModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerTypeId { get; set; }
        public string SBUId { get; set; }
        public decimal Acreage { get; set; }
        public decimal Capital { get; set; }
        public int EmployeeQuantity { get; set; }
        public string Field { get; set; }
        public string DepartmentId { get; set; }
        
    }
}
