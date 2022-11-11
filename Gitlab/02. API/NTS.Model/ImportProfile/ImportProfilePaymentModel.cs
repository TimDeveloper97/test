using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportProfile
{
    public class ImportProfilePaymentModel
    {
        public string Id { get; set; }
        public string ImportProfileId { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }
        public int PercentPayment { get; set; }
        public decimal Money { get; set; }
        public DateTime Duedate { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public string MoneyTransferPath { get; set; }
        public string MoneyTransferName { get; set; }
        public int CurrencyUnit { get; set; }
    }
}
