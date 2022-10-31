using NTS.Model.JobAttach;
using NTS.Model.JobSupjects;
using NTS.Model.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.HistoryVersion
{
    public class JobHistoryModel
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
        public JobHistoryModel()
        {
            ListJobSubject = new List<JobSubjectsModel>();
            ListJobAttach = new List<JobAttachModel>();
            ListSubject = new List<SubjectsModel>();
        }
    }
}
