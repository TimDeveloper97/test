using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.FuturePersonnelForecast
{
    public class SelectProjectModel
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Code { set; get; }
        public List<string> ListIdSelect { get; set; }
        public SelectProjectModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
