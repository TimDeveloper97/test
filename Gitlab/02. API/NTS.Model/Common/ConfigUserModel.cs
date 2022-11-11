using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Common
{
  public  class ConfigUserModel
    {
        public int ConfigId { get; set; }
        public string ConfigName { get; set; }
        public string Value { get; set; }
        public string ConfigType { get; set; }
        public string ObjectType { get; set; }
        public string ConfigCode { get; set; }
        public string ManageId { get; set; }
    }
}
