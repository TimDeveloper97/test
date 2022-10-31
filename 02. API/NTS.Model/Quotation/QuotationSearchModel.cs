using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.Quotation
{
    public class QuotationSearchModel : SearchCommonModel
    {
        public string Code { get; set; }//So YCKH=YCGP
        public string QuotationId { get; set; }
        public string CustomerName { get; set; } //Ten KH
        public string CustomerCode { get; set; } //Ma KH
        public string CustomerType { get; set; } //Loai KH
        public string JobName { get; set; } //Tên lĩnh vực
        public string Province { get; set; } //Tinh TP
        public string NumberQuotation { get; set; } //So BG
        public DateTime QuotationDate { get; set; } //Ngay BG
        public decimal ExpectedPrice { get; set; } //Giá trị dự kiến mua
        public decimal QuotationPrice { get; set; } //Giá trị BG
        public DateTime? ImplementationDate { get; set; } //Thời gian triển khai
        public int AdvanceRate { get; set; } //Tỷ lệ tạm ứng
        public int SuccessRate { get; set; } //Tỷ lệ thành công
        public int QuotationStatus { get; set; } //Tỷ lệ thành công
        public string SBUName { get; set; } //Tên SBU
        public string PetitionerName { get; set; } //NV phụ trách BG
        public string DepartmentRequestName { get; set; } //Phòng phụ trách BG
        public string RecieverName { get; set; } //NV tiếp nhận BG
        public DateTime CreateDate { get; set; } //Ngày tạo

        public int EffectiveLength { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Delivery { get; set; }
        public string Warranty { get; set; }
        public string PaymentMethod { get; set; }
        public string SBUId { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string StepInQuotationId { get; set; }
        public int Status { get; set; }
        public int QuotationPriceType { get; set; }
        public string CustomerAddress { get; set; } 
        public string CustomerTel { get; set; }
        public string CustomerTax { get; set; }

        public string EmployeeChargeName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsNotFullSchedule { get; set; }

        public List<ListQuotationDocument> ListQuotationDocument { get; set; }
        public List<ListQuotationStep> ListQuotationStep { get; set; }
        public List<ListStatusQuotationProcess> ListStatusQuotationProcess { get; set; }
        public QuotationSearchModel()
        {
            ListQuotationStep = new List<ListQuotationStep>();
            ListQuotationDocument = new List<ListQuotationDocument>();
            ListStatusQuotationProcess = new List<ListStatusQuotationProcess>();
        }

    }
}
