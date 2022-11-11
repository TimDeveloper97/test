using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace NTS.Model.HistoryVersion
{
    public class HistoryVersionModel
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public int Version { get; set; }
        public string Content { get; set; }
        public string EditContent { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
