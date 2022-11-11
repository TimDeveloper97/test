using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Manufacture
{
    public class ManufactureResultModel
    {
        public string Id { get; set; }
        public string ManufactureGroupId { get; set; }
        public string MaterialGroupCode { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Origination { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string MaterialType { get; set; }
    }
}
