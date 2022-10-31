using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Document
{
    public class DocumentSearchResultModel
    {
        public string Id { get; set; }
        public string DocumentGroupId { get; set; }
        public string DocumentGroupName { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeId { get; set; }
        public string CompilationSuppliserId { get; set; }
        public int Status { get; set; }
        public int CompilationType { get; set; }
        public string DocumentObjectName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public DateTime? PromulgateDate { get; set; }
        public DateTime? PromulgateLastDate { get; set; }
        public List<DocumentObjectModel> DocumentObjects { get; set; }
        public bool IsDocumentOfTask { get; set; }
        public string Version { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<System.DateTime> ReviewDateFrom { get; set; }
        public Nullable<System.DateTime> ReviewDateTo { get; set; }


        public List<DocumentFileModel> ListFile { get; set; }
        public DocumentSearchResultModel()
        {
            DocumentObjects = new List<DocumentObjectModel>();
            ListFile = new List<DocumentFileModel>();
        }
    }
}
