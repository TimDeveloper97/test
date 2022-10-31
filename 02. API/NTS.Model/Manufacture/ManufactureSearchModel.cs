using NTS.Model.Combobox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Manufacture
{
    public class ManufactureSearchModel : SearchCommonModel
    {
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public string ManufactureGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string MaterialType { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public List<string> ListIdSelect { get; set; }
        public ManufactureSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
