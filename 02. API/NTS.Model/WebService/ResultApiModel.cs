using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.WebService
{
    public class ResultApiModel
    {
        public bool SuccessStatus { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
