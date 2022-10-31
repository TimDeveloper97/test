using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.CustomerContact
{
    public class CustomerContactSearchModel: SearchCommonModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public List<string> ListCustomerContactId { get; set; }
        public CustomerContactSearchModel()
        {
            ListCustomerContactId = new List<string>();
        }
    }
}
