//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NTS.Model.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class ImportProfilePayment
    {
        public string Id { get; set; }
        public string ImportProfileId { get; set; }
        public string Content { get; set; }
        public int Index { get; set; }
        public int PercentPayment { get; set; }
        public decimal Money { get; set; }
        public System.DateTime Duedate { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public string MoneyTransferPath { get; set; }
        public string MoneyTransferName { get; set; }
        public int CurrencyUnit { get; set; }
    
        public virtual ImportProfile ImportProfile { get; set; }
    }
}
