using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerContact
{
    public class CustomerContactModel : BaseModel
    {
        public string  Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Note { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Status { get; set; }
        public string CustomerCode { get; set; }
        public string Avatar { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
    }
}
