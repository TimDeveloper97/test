using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ImportPR
{
    public class ImportPRUpdateModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public string UnitName { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureCode { get; set; }
        public int Quantity { get; set; }
        public DateTime RequireDate { get; set; }
        public DateTime? PODueDate { get; set; }
        public string SalesBy { get; set; }
        public string SalesName { get; set; }
        public string ProjectCode { get; set; }
        public string PurchaseRequestCode { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public DateTime? LeadTime { get; set; }
        public int CurrencyUnit { get; set; }
        public decimal ImportTax { get; set; }
        public decimal ImportTaxValue { get; set; }
        public decimal VATTax { get; set; }
        public decimal VATTaxValue { get; set; }
        public decimal OtherTax { get; set; }
        public decimal OtherTaxValue { get; set; }
        public decimal Amount { get; set; }
        public string HSCode { get; set; }
        public decimal QuotaPrice { get; set; }
        public decimal InternationalShippingCost { get; set; }
        public decimal InlandShippingCost { get; set; }
        public decimal OtherCosts { get; set; }
        public decimal RealPrice { get; set; }
        public string ProductionDescription { get; set; }
    }
}
