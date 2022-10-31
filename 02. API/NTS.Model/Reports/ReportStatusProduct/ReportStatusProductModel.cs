using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.ReportStatusProduct
{
    public class ReportStatusProductModel
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string ProjectId { get; set; }
        public string ProductId { get; set; }
        public decimal Quatity { get; set; }
        public int TotalProduct { get; set; }
        public decimal TotalProductInProject { get; set; }

        public List<ReportStatusProductModel> ListProductUse { get; set; }
        public List<ReportStatusProductModel> ListProduct { get; set; }
        public ReportStatusProductModel()
        {
            ListProductUse = new List<ReportStatusProductModel>();
            ListProduct = new List<ReportStatusProductModel>();
        }
    }
}
