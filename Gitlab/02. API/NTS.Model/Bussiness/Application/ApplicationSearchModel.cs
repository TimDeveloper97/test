using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Bussiness.Application
{
   public class ApplicationSearchModel: SearchCommonModel
    {
        public string Name { get; set; }
        public List<string> ListIdSelect { get; set; }

        public ApplicationSearchModel ()
        {
            ListIdSelect = new List<string>();
        }
    }
}
