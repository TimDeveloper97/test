using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateService
{
    public class ResultApiModel<T>
    {
        public bool SuccessStatus { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public T Data { get; set; }
    }

    public class ResultApiModel
    {
        public bool SuccessStatus { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public object Data { get; set; }
    }
}
