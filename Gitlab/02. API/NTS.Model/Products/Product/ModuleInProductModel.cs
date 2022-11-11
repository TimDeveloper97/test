﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Product
{
    public class ModuleInProductModel
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Specification { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public int Qty { get; set; }
        public int LeadTime { get; set; }
        public int Version { get; set; }
        public decimal Price { get; set; }
        public bool IsNoPrice { get; set; }
		public int Status { get; set; }
    }
}
