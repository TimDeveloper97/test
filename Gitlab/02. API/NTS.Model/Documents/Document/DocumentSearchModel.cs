using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Document
{
    public class DocumentSearchModel : SearchCommonModel
    {
        public string DocumentGroupId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Keyword { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentUseId { get; set; }
        public string DocumentTypeId { get; set; }
        public string CompilationSuppliserId { get; set; }
        public int? Status { get; set; }
        public int? CompilationType { get; set; }
        public DateTime? PromulgateDateFrom { get; set; }
        public DateTime? PromulgateDateTo { get; set; }
        public DateTime? PromulgateLastDateFrom { get; set; }
        public DateTime? PromulgateLastDateTo { get; set; }

        public Nullable<System.DateTime> EffectiveDateFrom { get; set; }
        public Nullable<System.DateTime> EffectiveDateTo { get; set; }

        public List<string> ListIdSelect { get; set; }

        public DocumentSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
