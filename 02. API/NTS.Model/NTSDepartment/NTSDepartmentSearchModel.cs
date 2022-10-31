using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NTS.Model.NTSDepartment
{
    public class DepartmentSearchModel : SearchCommonModel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int? Status { get; set; }

        public string ManagerId { get; set; }
        public string SBUId { get; set; }

        public string ManagerName { get; set; }

        public List<string> ListIdSelect { get; set; }

        public DepartmentSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}