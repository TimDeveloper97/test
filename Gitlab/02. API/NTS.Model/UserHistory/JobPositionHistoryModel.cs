using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.UserHistory
{
    public class JobPositionHistoryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public bool IsOnlyOne { get; set; }
    }
}
