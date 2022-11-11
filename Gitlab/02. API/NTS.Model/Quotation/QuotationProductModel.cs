using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class QuotationProductModel
    {
        public string Id { get; set; }
        public string QuotationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ObjectId { get; set; }
        public int ObjectType { get; set; }
        public string IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public string ManufactureId { get; set; }
        public string ManufactureName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}
