using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Bank
{
    public class BankModel : BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameBank { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; } // chi nhánh ngân hàng
        public string ExpertId { get; set; }
        public string ExpertName { get; set; }
    }
}
