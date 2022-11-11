using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Error
{
    public class ErrorStatusModel
    {
        public int Status { get; set; }
        public string Id { get; set; }
        public string strHistory { get; set; }
        public string UpdateBy { get; set; }
    }
}
