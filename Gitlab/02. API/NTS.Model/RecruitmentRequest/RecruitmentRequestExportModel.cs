using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Model.RecruitmentRequest
{
    public class RecruitmentRequestExportModel
    {
        public string Index { get; set; } // stt
        public string StatusRecruit { get; set; } //tình trạng
        public string Code { get; set; } //Số YCTD
        public string DepartmentName { get; set; } //Phòng ban đề xuất
        public string WorkTypeName { get; set; } //Vị trí tuyển dụng
        public int Quantity { get; set; } //Số lượng
        public string RecruitmentDeadline { get; set; } //Thời gian cần
        public string TypeName { get; set; } //Loại hình
        public decimal MinSalary { get; set; } //Mức lương dự kiến tối thiểu
        public decimal MaxSalary { get; set; } //Mức lương dự kiến tối đa
        public string RecruitmentReason { get; set; } //Lý do tuyển
        public string Description { get; set; } //Mô tả công việc
        public string Request { get; set; } //Yêu cầu
        public string Equipment { get; set; } // Trang thiết bị cần
        public string ApprovalDate { get; set; } //Ngày phê duyệt
        public string RequestDate { get; set; } // Ngày nhận yêu cầu
        public string FinishDate { get; set; } // Ngày hoàn thành
        public int NumberRecruitment { get; set; } //Số lượng ứng tuyển
        public int NumberCandidate { get; set; } //Số lượng hồ sơ đạt
        public int NumberInterview { get; set; } //Số lượng phỏng vấn
        public int NumberNeedRecruit { get; set; } //Số lượng còn phải tuyển
        public int NumberLateDate { get; set; } // Số ngày trễ
    }
}
