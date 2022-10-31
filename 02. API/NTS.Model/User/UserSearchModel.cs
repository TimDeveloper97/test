using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;


namespace NTS.Model.User
{
    public class UserSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string GroupUserId { get; set; }
    }
}
