using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Notify
{
    public class NotifySearchModel : SearchCommonModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        // public string UserName { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public bool IsOperate { get; set; }
    }
}
