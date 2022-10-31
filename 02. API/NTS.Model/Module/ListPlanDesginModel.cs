using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Module
{
    public class ListPlanDesginModel
    {
        public string Id { get; set; }
        public string NameWork { get; set; }
        public string CodeProject { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public int Status { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
