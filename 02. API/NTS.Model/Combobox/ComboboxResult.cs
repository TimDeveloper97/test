using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Combobox
{
    public class ComboboxResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Exten { get; set; }
        public int Index { get; set; }
        public string ObjectId { get; set; }
        public int? LeadTime { get; set; }
    }

    public class ComboboxMultilevelResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ParentId { get; set; }
        public string Exten { get; set; }
        public int Index { get; set; }
        public int Type { get; set; }
        public bool IsPending { get; set; }
    }

    public class ForgotpassModel
    {
        public string UserId { get; set; }
        public string SecurityStamp { get; set; }
        public string PassNew { get; set; }
    }

    public class ComboboxResultModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Exten { get; set; }
        public int Index { get; set; }
    }
}
