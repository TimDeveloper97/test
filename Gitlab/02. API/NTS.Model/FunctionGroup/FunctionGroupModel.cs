﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FunctionGroup
{
    public class FunctionGroupModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }
}
