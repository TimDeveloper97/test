using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Notify
{
    public class WebNotifyQueueModel
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }
        public int ObjectType { get; set; }
        public object ObjectId { get; set; }

        public List<string> Users { get; set; }
    }
}
