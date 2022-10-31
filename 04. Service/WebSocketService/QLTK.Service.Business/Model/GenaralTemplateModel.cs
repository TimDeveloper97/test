using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTK.Service.Business.Model
{
    public class GenaralTemplateModel
    {
        public string PathTempalte { get; set; }
        public string NameTemplate { get; set; }
        public string ModuleCode { get; set; }
        public string GroupCode { get; set; }
        public string parentGroupCode { get; set; }
        public string ApiUrl { get; set; }
        public List<string> ListFolderDefinition { get; set; }
        public List<string> ListFileDefinition { get; set; }
        public int Type { get; set; }
    }
}
