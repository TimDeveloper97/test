using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MaterialParameterValue
{
    public class MaterialParameterValueModel
    {
        public string Id { get; set; }
        public string MaterialParameterId { get; set; }
        public string Value { get; set; }
        public string CreateBy { get; set; }
        public bool IsDelete { get; set; }


        public bool IsChecked { get; set; } 
    }
}
