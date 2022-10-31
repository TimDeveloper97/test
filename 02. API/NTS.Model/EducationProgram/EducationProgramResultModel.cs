using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.EducationProgram
{
    public class EducationProgramResultModel : BaseModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Documents { get; set; }

        public string JobId { get; set; }
        public string JobName { get; set; }
    }
}
