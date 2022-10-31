using NTS.Model.ClassRoom;
using NTS.Model.NTSDepartment;
using NTS.Model.QLTKMODULE;
using NTS.Model.TaskFlowStage;
using NTS.Model.WorkType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Document
{
    public class DocumentModel
    {
        public string Id { get; set; }
        public string DocumentGroupId { get; set; }
        public string DocumentTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
        public int CompilationType { get; set; }
        public string CompilationEmployeeId { get; set; }
        public string CompilationSuppliserId { get; set; }
        public Nullable<System.DateTime> PromulgateDate { get; set; }
        public Nullable<System.DateTime> PromulgateLastDate { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> ReviewDateFrom { get; set; }
        public Nullable<System.DateTime> ReviewDateTo { get; set; }
        public decimal Price { get; set; }
        public string Keyword { get; set; }
        public List<string> Keywords { get; set; }
        public string Description { get; set; }
        public string ApproveWorkTypeId { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }

        public List<ClassRoomResultProductModel> Devices { get; set; }
        public List<ModuleModel> Modules { get; set; }
        public List<WorkTypeModel> WorkTypes { get; set; }
        public List<DepartmentResultModel> Departments { get; set; }
        public List<TaskFlowStageSearchResultModel> Works { get; set; }


        public List<string> DocumentTags { get; set; }

        public DocumentModel()
        {
            Devices = new List<ClassRoomResultProductModel>();
            Modules = new List<ModuleModel>();
            WorkTypes = new List<WorkTypeModel>();
            Departments = new List<DepartmentResultModel>();
            Works = new List<TaskFlowStageSearchResultModel>();
            Keywords = new List<string>();
            DocumentTags = new List<string>();
        }
    }
}
