using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ProjectRole
{
    public class SearchRoleModel : SearchCommonModel
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public int? IsDisable { get; set; }


    }
}
