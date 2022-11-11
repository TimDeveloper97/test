﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.Combobox;

namespace NTS.Model.EmployeeGroups
{
    public class EmployeeGroupSearchModel : SearchCommonModel
    {
        public string EmployeeGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
}
