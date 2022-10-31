using NTS.Model.NSMaterialParameterValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.NSMaterialParameter
{
    public class NSMaterialParameterModel
    {
        public string Id { get; set; }
        public string NSMaterialGroupId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string ConnectCharacter { get; set; }
        public string Unit { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<NSMaterialParameterValueModel> ListValue { get; set; }

        public string Value { get; set; }

        public NSMaterialParameterModel()
        {
            ListValue = new List<NSMaterialParameterValueModel>();
        }
    }
}
