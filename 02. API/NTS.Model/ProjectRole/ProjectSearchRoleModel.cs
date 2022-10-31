using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectRole
{
    public class ProjectSearchRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NamePermission { get; set; }
        public string Code { get; set; }
        public int Index { get; set; }
        public string Descipton { get; set; }
        public bool IsDisable { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
