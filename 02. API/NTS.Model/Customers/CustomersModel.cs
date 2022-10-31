using NTS.Model.CustomerContact;
using NTS.Model.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Customers
{
    public class CustomersModel : BaseModel
    {
        public string Index { get; set; }
        public string Id { get; set; }
        public string Tax { get; set; }
        public string ProvinceId { get; set; }
        public string EmployeeId { get; set; }
        public string CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Contact { get; set; }
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public List<string> ListJobGroupId { get; set; }
        public List<CustomerContactModel> ListCustomerContact { get; set; }
        public string SBUId { get; set; }
        public string SBUCode { get; set; }
        public string SBUName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public decimal Acreage { get; set; }
        public decimal Capital { get; set; }
        public int EmployeeQuantity { get; set; }
        public string Field { get; set; }


        public int Type { get; set; }
        public string CodeChar { get; set; }
        public List<CustomerOfCustomerModel> CustomerOfCustomer { get; set; }
        public List<MeetingInfoModel> Meetings { get; set; }
    }

    public class ImportCustomer
    {
        public List<CustomersModel> ListExist { get; set; }
        public string Message { get; set; }
        public ImportCustomer()
        {
            ListExist = new List<CustomersModel>();
        }
    }
}
