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
    
    public partial class ProductNeedPrice
    {
        public string Id { get; set; }
        public string CustomerRequirementId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ManufactureName { get; set; }
        public string Specifications { get; set; }
        public int ProductType { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public System.DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
    }
}
