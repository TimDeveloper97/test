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
    
    public partial class ImportProfileProduct
    {
        public string Id { get; set; }
        public string PurchaseRequestProductId { get; set; }
        public string ImportProfileId { get; set; }
        public decimal Price { get; set; }
        public decimal OtherCosts { get; set; }
        public decimal RealPrice { get; set; }
        public Nullable<System.DateTime> Leadtime { get; set; }
        public int CurrencyUnit { get; set; }
        public decimal VATTax { get; set; }
        public decimal ImportTax { get; set; }
        public decimal OtherTax { get; set; }
        public decimal Amount { get; set; }
        public string HSCode { get; set; }
        public decimal ImportTaxValue { get; set; }
        public decimal VATTaxValue { get; set; }
        public decimal OtherTaxValue { get; set; }
        public decimal InternationalShippingCost { get; set; }
        public decimal InlandShippingCost { get; set; }
        public string ProductionDescription { get; set; }
    
        public virtual ImportProfile ImportProfile { get; set; }
        public virtual PurchaseRequestProduct PurchaseRequestProduct { get; set; }
    }
}