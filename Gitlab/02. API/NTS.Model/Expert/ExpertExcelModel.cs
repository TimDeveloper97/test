using NTS.Model.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Expert
{
    public class ExpertExcelModel
    {
        public string Index { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string WorkPlaceName { get; set; }
        public string DegreeName { get; set; }
        public string SpecializeName { get; set; }
        public string PhoneNumber { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountName { get; set; }
        public List<BankModel> ListBank { get; set; }
    }
}
