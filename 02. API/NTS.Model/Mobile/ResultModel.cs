using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Mobile
{
    public class ResultModel<T>
    {
        //[JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        //[JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        //[JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        //[JsonProperty(PropertyName = "totalrecord")]
        public int TotalRecord { get; set; }

        //[JsonProperty(PropertyName = "total")]
        public Dictionary<string, long> Total { get; set; }
    }
}
