using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Errors.ErrorChangePlan
{
    public class ErrorHistoryChangePlanModel : BaseModel
    {
        public string Id { get; set; }
        public string Solution { get; set; }
        public string ErrorId { get; set; }
        public string ProjectId { get; set; }
        public string ErrorFixId { get; set; }
        public DateTime? OldtartDate { get; set; }
        public DateTime? OldFinishDate { get; set; }
        public DateTime? NewStartDate { get; set; }
        public DateTime? NewFinishDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Reason { get; set; }
        public string ChangeBy { get; set; }
    }
}
