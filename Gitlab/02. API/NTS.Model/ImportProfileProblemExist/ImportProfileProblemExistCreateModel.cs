using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfileProblemExist
{
    public class ImportProfileProblemExistCreateModel
    {
        public string Id { get; set; }
        public string ImportProfileId { get; set; }
        public string Note { get; set; }
        public string Plan { get; set; }
        public int Step { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
