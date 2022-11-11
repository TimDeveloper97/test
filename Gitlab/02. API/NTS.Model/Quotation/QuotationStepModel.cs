using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class ListQuotationStep
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string QuotesId { get; set; }
        public string QuotationStepId { get; set; }
        public int maxData { get; set; }
        public int Status { get; set; }
        
    }

    public class ListStatusQuotationProcess
    {
        public int TotalWork { get; set; } //Tổng lượng CV phải làm
        public int WorkInProgress { get; set; } //SL còn phải làm
        public int WorkLate { get; set; } //SL trễ
    }

    public class ListQuotationDocument
    {
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public decimal Size { get; set; }
        public string Extention { get; set; }
        public string Thumbnail { get; set; }
        public string HashValue { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
