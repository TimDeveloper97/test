using NTS.Model.ClassRoom;
using NTS.Model.ClassRoomProduct;
using NTS.Model.JobAttach;
using NTS.Model.JobSupjects;
using NTS.Model.Product;
using NTS.Model.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Job
{
    public class JobModel : BaseModel
    {
        public string Id { get; set; }
        public string DegreeId { get; set; }
        public string DegreeName { get; set; }
        public string JobGroupId { get; set; }
        public string JobGroupName { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Documents { get; set; }
        public string Description { get; set; }
        public List<JobSubjectsModel> ListJobSubject { get; set; }
        public List<JobAttachModel> ListJobAttach { get; set; }
        public List<SubjectsModel> ListSubject { get; set; }
        public List<ClassRoomResultModel> ListClassRoom { get; set; }
        public List<ClassRoomProductModel> ListClassRoomProductModel { get; set; }
        public List<ModuleInProductModel> ListModuleProduct { get; set; }
        public JobModel()
        {
            ListJobSubject = new List<JobSubjectsModel>();
            ListJobAttach = new List<JobAttachModel>();
            ListSubject = new List<SubjectsModel>();
        }
    }
    
}
