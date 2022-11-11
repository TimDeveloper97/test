using NTS.Model.Job;
using NTS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Model.EducationProgramAttachModel;
namespace NTS.Model.EducationProgram
{
    public class EducationProgramModel :BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Documents { get; set; }

        public string JobId { get; set; }
        public string JobName { get; set; }

        public List<JobModel> ListJob { get; set; }
        public List<NTS.Model.EducationProgramAttachModel.EducationProgramAttachModel> ListFile { get; set; }
    }
}
