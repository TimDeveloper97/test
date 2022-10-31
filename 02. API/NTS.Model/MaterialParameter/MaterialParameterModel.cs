
using NTS.Model.MaterialParameterValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.MaterialParameter
{
    public class MaterialParameterModel
    {
        public string Id { get; set; }
        public string MaterialGroupId { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public bool IsDelete { get; set; }
        public List<MaterialParameterValueModel> ListValue { get; set; }

        public MaterialParameterModel()
        {
            ListValue = new List<MaterialParameterValueModel>();
        }
    }
}
