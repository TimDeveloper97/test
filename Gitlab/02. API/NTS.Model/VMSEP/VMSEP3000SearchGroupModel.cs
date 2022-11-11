using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.VMSEP
{
  public  class VMSEP3000SearchGroupModel : SearchCommonModel
    {
        public string CustomerId { set; get; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}