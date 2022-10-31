using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ErrorHistory
{
    public class ErrorHistoryModel : BaseModel
    {
        public string Id { get; set; }
        public string ErrorId { get; set; }
        public string Content { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
