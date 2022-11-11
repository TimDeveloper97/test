using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.NSMaterialParameterValue
{
    public class NSMaterialParameterValueModel
    {
        public string Id { get; set; }
        public string NSMaterialParameterId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
    }
}
