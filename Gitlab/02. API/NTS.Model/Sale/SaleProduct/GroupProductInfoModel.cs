using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Sale.SaleProduct
{
    public class GroupProductInfoModel
    {
        public string Id{ get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
    public class EmployeeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentId { get; set; }
    }
}
