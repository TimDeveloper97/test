using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Bank
{
    public class BankResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string AccountName { get; set; } // chi nhánh ngân hàng
        public List<BankModel> ListBank { get; set; }
    }
}
