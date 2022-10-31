using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.VMSEP
{
    public class VMSEP2100GroupModel
    {
        public string GroupId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Default { get; set; }
        public string Description { get; set; }
        public string GroupType { get; set; }
        public string ManageId { get; set; }
        public string CreateBy { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public string ObjectType { get; set; }

        //List id quyền
        public string StrListFunctionId { get; set; }
    }
}
