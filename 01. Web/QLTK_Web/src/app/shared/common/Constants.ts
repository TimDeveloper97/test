import { Injectable } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';

import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Injectable({
    providedIn: 'root'
})
export class Constants {
    // Danh sách tình trang chương trình
    PlanStatus: any[] = [
        { Id: 1, Name: 'Chưa thực hiện', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đang thực hiện', BadgeClass: 'badge-yellow', TextClass: 'text-green' },
        { Id: 3, Name: 'Đã hoàn thành', BadgeClass: 'badge-green', TextClass: '' },
        { Id: 4, Name: 'Đóng', BadgeClass: 'badge-secondary', TextClass: '' },
    ];
    
    QCResultStatus: any[]=[
        { Id: 1, Name: 'Đạt', BadgeClass: 'badge-Success', TextClass: 'text-danger' },
        { Id: 2, Name: 'Không đạt', BadgeClass: 'badge-danger', TextClass: 'text-green' },
        { Id: 0, Name: 'Chưa QC', BadgeClass: 'badge-secondary', TextClass: 'text-green' },
    ];

    PlanType: any[] = [
        { Id: 1, Name: 'Dự án',  BadgeClass: 'badge-danger', TextClass: 'text-danger'},
        { Id: 2, Name: 'Bổ sung tính phí', BadgeClass: 'badge-yellow', TextClass: 'text-danger' },
        { Id: 3, Name: 'Bổ sung không tính phí', BadgeClass: 'badge-green', TextClass: 'text-danger'},
    ];

    QCStatus: any[] = [
        { Id: 0, Name: 'Chưa QC',  BadgeClass: 'badge-secondary', TextClass: 'text-danger'},
        { Id: 1, Name: 'Đang QC', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 2, Name: 'QC đạt', BadgeClass: 'badge-Success', TextClass: 'text-danger'},
        { Id: 3, Name: 'Không đạt', BadgeClass: 'badge-danger', TextClass: 'text-danger'},
    ];


    PlanStartDateStatus: any[] = [
        { Id: 1, Name: 'Chưa có', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đã có', BadgeClass: 'badge-yellow', TextClass: 'text-green' },
    ];

    GamePlayApproveStatus: any[] = [
        { Id: '0', Name: 'Chưa duyệt', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: '1', Name: 'Đã duyệt', BadgeClass: 'badge-green', TextClass: '' }
    ];

    Genders: any[] = [
        { Id: '0', Name: 'Khác' },
        { Id: '1', Name: 'Nam' },
        { Id: '2', Name: 'Nữ' }
    ];

    Source: any[] = [
        { Id: 1, Name: 'Thiết bị' },
        { Id: 2, Name: 'Module' },
        { Id: 3, Name: 'Vật tư' },
        { Id: 4, Name: 'Thiết bị nhập khẩu' },
    ]

    exportandkeepstatus: any[] = [
        { Id: 1, Name: 'Đã bán', BadgeClass: 'badge-success' },
        // {Id: 2, Name:'Đang giữ', BadgeClass: 'badge-secondary'},
        { Id: 3, Name: 'Trả về kho', BadgeClass: 'badge-danger' },
        // { Id: 4, Name: 'Quá hạn', BadgeClass: 'badge-warning' },
    ]

    ItemStatus: any[] = [
        { Id: '0', Name: 'Chưa chơi', BadgeClass: 'badge-secondary', TextClass: 'text-danger' },
        { Id: '1', Name: 'Đang chơi', BadgeClass: 'badge-yellow', TextClass: 'text-green' },
        { Id: '2', Name: 'Đã chơi', BadgeClass: 'badge-green', TextClass: '' }
    ];

    //Tình trạng lỗi
    ListError: any[] = [
        { Id: 1, Name: 'Đang tạo', BadgeClass: 'badge-primary', TextClass: 'text-primary' },
        { Id: 2, Name: 'Chờ xác nhận', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 3, Name: 'Chưa có kế hoạch', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        // { Id: 4, Name: 'Đang chờ xử lý' },
        { Id: 5, Name: 'Đang xử lý', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 6, Name: 'Đang QC', BadgeClass: 'badge-secondary', TextClass: 'text-secondary' },
        { Id: 7, Name: 'QC đạt', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 8, Name: 'QC không đạt', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 9, Name: 'Đóng vấn đề của dự án', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 10, Name: 'Đã khắc phục triệt để', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    //Tình trạng lỗi
    ListErrorAlias: any[] = [
        { Id: 1, Name: 'ĐT', BadgeClass: 'badge-primary', TextClass: 'text-primary' },
        { Id: 2, Name: 'CXN', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 3, Name: 'Chưa có kế hoạch', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        // { Id: 4, Name: 'Đang chờ xử lý' },
        { Id: 5, Name: 'ĐXL', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 6, Name: 'Đang QC', BadgeClass: 'badge-secondary', TextClass: 'text-secondary' },
        { Id: 7, Name: 'QC đạt', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 8, Name: 'QC không đạt', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 9, Name: 'Đóng vấn đề của dự án', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 10, Name: 'Đã khắc phục triệt để', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    //TimeStandard
    ListType: any[] = [
        { Id: 1, Name: 'Thiết kế' },
        { Id: 2, Name: 'Tài liệu' },
        { Id: 3, Name: 'Chuyển giao' }
    ];

    //Employee_WorkType
    ListWorkType: any[] = [
        { Id: 1, Name: 'Cơ khí' },
        { Id: 2, Name: 'Điện' },
        { Id: 3, Name: 'Điện tử' }
    ];

    //ProjectProduct_DataType
    ListDataType: any[] = [
        { Id: 1, Name: 'Bài thực hành/công đoạn' },
        { Id: 2, Name: 'Sản phẩm' },
        { Id: 3, Name: 'Mô hình' },
        { Id: 4, Name: 'Module' },
    ];

    //ProjectProduct_ModuleStatus
    ListModuleStatus: any[] = [
        { Id: 1, Name: 'Dự án' },
        { Id: 2, Name: 'Bổ sung tính phí' },
        { Id: 3, Name: 'Bổ sung không tính phí' },
    ];

    //ProjectProduct_DesignStatus
    ListDesignStatus: any[] = [
        { Id: 1, Name: 'Thiết kế mới' },
        { Id: 2, Name: 'Sửa thiết kế cũ' },
        { Id: 3, Name: 'Tận dụng' },
        { Id: 4, Name: 'Hàng bán thẳng' },
    ];

    //ProjectProduct_Cost
    ListCost: any[] = [
        { Id: 1, Name: 'SP/Module vượt quá định mức' },
        { Id: 2, Name: 'Sản phẩm phát sinh chi phí' },
    ];

    LateStatus: any[] = [
        { Id: 1, Name: 'Có CV trễ' },
        { Id: 2, Name: 'Không có CV trễ' },
    ];

    DesignStatus: any = {
        Sale: 4
    }

    FolderTypes: any[] = [
        { Id: 1, Name: 'Thư mục thiết kế tải lên', Type: 1 },
        { Id: 2, Name: 'Thư mục chứa bản cứng CD', Type: 1 },
        { Id: 3, Name: 'Thư mục chứa file CAD', Type: 1 },
        { Id: 4, Name: 'Thư mục MAT', Type: 1 },
        { Id: 5, Name: 'Thư mục chứa bản cứng', Type: 1 },
        { Id: 6, Name: 'Thư mục 3D', Type: 1 },
        { Id: 7, Name: 'Thư mục IGS', Type: 1 },
        { Id: 8, Name: 'Thư mục 3D hàng hãng, đi mượn', Type: 1 },
        { Id: 9, Name: 'Thư mục HMI', Type: 1 },
        { Id: 10, Name: 'Thư mục PLC', Type: 1 },
        { Id: 11, Name: 'Thư mục Phần mềm', Type: 1 }
    ];

    FileTypes: any[] = [
        { Id: 1, Name: 'File danh mục vật tư', Type: 1 },
        { Id: 2, Name: 'File 2D tổng', Type: 1 },
        { Id: 3, Name: 'File Bản vẽ thiết kế điện dạng PDF', Type: 1 },
        { Id: 4, Name: 'File lấy ảnh thiết kế', Type: 1 },
        { Id: 5, Name: 'File Bản giải trình', Type: 1 },
        { Id: 6, Name: 'File FCM', Type: 1 },
        { Id: 7, Name: 'File TSTK', Type: 1 }
    ];
    SendSale: any[] = [
        { Id: false, Name: 'Chưa chuyển', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: true, Name: 'Đã chuyển', BadgeClass: 'badge-green', TextClass: 'text-green' },
    ];

    KeepAndExportPayStatus: any[] = [
        { Id: 1, Name: 'Chưa thanh toán', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đã thanh toán', BadgeClass: 'badge-success', TextClass: 'text-green' },
    ];

    FileCOCQ: any[] = [
        { Id: false, Name: 'Không có' },
        { Id: true, Name: 'Có' },
    ];
    DesignTypes: any[] = [
        { Id: 1, Name: 'Module Cơ khí' },
        { Id: 2, Name: 'Module Điện' },
        { Id: 3, Name: 'Module Điện tử' }
    ];

    DesignObjectTypes: any[] = [
        { Id: 1, Name: 'Module' },
        { Id: 2, Name: 'Thiết bị' },
        { Id: 3, Name: 'Phòng học' },
        { Id: 4, Name: 'Giải pháp' }
    ];

    ListReportProblem: any[] = [
        { Id: 1, Name: 'Đã xử lý' },
        { Id: 2, Name: 'Chưa xử lý' }
    ]

    ScrollConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: false,
        suppressScrollY: false,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

    ScrollXConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: false,
        suppressScrollY: true,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

    ScrollYConfig: PerfectScrollbarConfigInterface = {
        suppressScrollX: true,
        suppressScrollY: false,
        minScrollbarLength: 20,
        wheelPropagation: true
    };

    HttpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    FileHttpOptions = {
        headers: new HttpHeaders({ 'Content-Type': 'multipart/form-data' })
    };

    StatusCode = {
        StatusCodeSuccess: 1,
        StatusCodeError: 2,
    };

    Message: {
        'MSG001': 'Có lỗi phát sinh trong quá trình xử lý. Liên hệ quản trị để hỗ trợ!',
        'MSG002': 'Thêm mới {0} thành công!',
        'MSG003': 'Tên {0} đã được sử dụng. Bạn hãy nhập tên khác!',
        'MSG004': 'Mã {0} đã được sử dụng. Bạn hãy nhập mã khác!',
        'MSG005': '{0} không tồn tại!',
        'MSG006': 'Chỉnh sửa {0} thành công!',
        'MSG007': 'Bạn có chắc muốn xóa {0} này không?',
        'MSG008': 'Xóa {0} thành công!',
        'MSG009': '{0} đang được sử dụng. Không được phép xóa!',
        'MSG010': '',
    };

    ListPageSize = [5, 10, 15, 20, 25, 30];

    validEmailRegEx = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

    SearchDebtTimeTypes: any[] = [
        { Id: '1', Name: 'Hôm nay' },
        { Id: '2', Name: 'Hôm qua' },
        { Id: '3', Name: 'Tuần này' },
        { Id: '4', Name: 'Tuần trước' },
        { Id: '5', Name: '7 ngày gần đây' },
        { Id: '6', Name: 'Tháng này' },
        { Id: '7', Name: 'Tháng trước' },
        { Id: '8', Name: 'Tháng' },
        { Id: '9', Name: 'Quý' },
        { Id: '10', Name: 'Năm nay' },
        { Id: '11', Name: 'Năm trước' },
        { Id: '12', Name: 'Năm' },
        // { Id: '13', Name: 'Khoảng thời gian' }
    ];

    MaterialStatus: any[] = [
        { Id: '0', Name: 'Đang sử dụng' },
        { Id: '1', Name: 'Tạm dừng' },
        { Id: '2', Name: 'Ngưng sản xuất' }
    ];

    MaterialRedundantStatus: any[] = [
        { Id: '0', Name: 'SL thừa = 0' },
        { Id: '1', Name: 'SL thừa > 0' },
    ];

    ErrorPlanStatus: any[] = [
        { Id: '0', Name: 'Trong thời hạn' },
        { Id: '1', Name: 'Trễ hạn' },
    ];


    MaterialSatusIsAllFile: any[] = [
        { Id: 1, Name: 'Đủ tài liệu' },
        { Id: 0, Name: 'Chưa đủ tài liệu' }
    ];

    MaterialType: any[] = [
        { Id: 1, Name: 'Vật tư tiêu chuẩn' },
        { Id: 2, Name: 'Vật tư phi tiêu chuẩn' }
    ];

    MaterialFile3D: any[] = [
        { Id: 1, Name: 'Đủ tài liệu 3D' },
        { Id: 2, Name: 'Không đủ tài liệu 3D' }
    ];

    MaterialFileDatasheet: any[] = [
        { Id: 1, Name: 'Đủ tài liệu Datasheet' },
        { Id: 2, Name: 'Không đủ tài liệu Datasheet' }
    ];

    MaterialImg: any[] = [
        { Id: 1, Name: 'Có hình ảnh' },
        { Id: 2, Name: 'Không có hình ảnh' }
    ];

    ModuleIsEnought: any[] = [
        { Id: 1, Name: 'Đã đủ' },
        { Id: 0, Name: 'Chưa đủ' }
    ];

    ModuleIsDisable: any[] = [
        { Id: 1, Name: 'Đang sử dụng' },
        { Id: 0, Name: 'Không dùng' }
    ];

    ModuleStatus: any[] = [
        { Id: 1, Name: 'Chỉ dùng một lần' },
        { Id: 2, Name: 'Module chuẩn' },
        { Id: 3, Name: 'Module ngừng sử dụng' }
    ];

    EmployeeStatus: any[] = [
        { Id: 1, Name: 'Đang làm việc', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 0, Name: 'Đã nghỉ', BadgeClass: 'badge-danger', TextClass: 'text-danger' }
    ];

    DeparymentStatus: any[] = [
        { Id: 1, Name: 'Dừng hoạt động' },
        { Id: 0, Name: 'Đang hoạt động' }
    ];

    ProductStatus: any[] = [
        { Id: 1, Name: 'Đều đã đủ' },
        { Id: 2, Name: 'Chưa đủ' }
    ];

    ProductIsEnought: any[] = [
        { Id: 0, Name: 'Chưa đủ dữ liệu' },
        { Id: 1, Name: 'Đã đủ dữ liệu' },
        { Id: 2, Name: 'Chưa có HD thực hành' },
        { Id: 3, Name: 'Chưa có báo giá' },
        { Id: 4, Name: 'Chưa có DM BTH' },
        { Id: 5, Name: 'Chưa có BV layout' },
        { Id: 6, Name: 'Chưa có DMVT' }
    ];

    StatusSolution: any[] = [
        { Id: 1, Name: "Đang triển khai" },
        { Id: 2, Name: "Tạm dừng" },
        { Id: 3, Name: "Hủy" },
        { Id: 4, Name: "Đã hoàn thành" }
    ];

    ProjectStatus: any[] = [
        { Id: '1', Name: 'Chưa kickoff', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: '2', Name: 'Sản xuất', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: '5', Name: 'Lắp đặt', BadgeClass: 'badge-info', TextClass: 'text-danger' },
        { Id: '6', Name: 'Hiệu chỉnh', BadgeClass: 'badge-primary', TextClass: 'text-danger' },
        { Id: '7', Name: 'Đưa vào sử dụng', BadgeClass: 'badge-dark', TextClass: 'text-danger' },
        { Id: '4', Name: 'Tạm dừng', BadgeClass: 'badge-secondary', TextClass: 'text-danger' },
        { Id: '3', Name: 'Đóng dự án và thu hồi công nợ', BadgeClass: 'badge-success', TextClass: 'text-danger' },
        { Id: '8', Name: 'Thiết kế', BadgeClass: 'badge-info', TextClass: 'text-danger' },
        { Id: '9', Name: 'Nghiệm thu', BadgeClass: 'badge-success', TextClass: 'text-danger' },
        { Id: '10', Name: 'Đã thanh lý', BadgeClass: 'badge-success', TextClass: 'text-danger' },
        { Id: '11', Name: 'Vật tư', BadgeClass: 'badge-success', TextClass: 'text-danger' },

    ];

    PeEvaluate: any[] = [
        { Id: 1, Name: 'Xuất sắc', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Tốt', BadgeClass: 'badge-info', TextClass: 'text-success' },
        { Id: 3, Name: 'Khá', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 4, Name: 'Trung bình', BadgeClass: 'badge-primary', TextClass: 'text-default' },
        { Id: 5, Name: 'Chưa có đánh giá', BadgeClass: 'badge-primary', TextClass: 'text-default' },
    ];


    listStatus: any[] = [
        { Id: '1', Name: 'Chưa kickoff' },
        { Id: '8', Name: 'Thiết kế' },
        { Id: '2', Name: 'Sản xuất' },
        { Id: '5', Name: 'Lắp đặt' },
        { Id: '6', Name: 'Hiệu chỉnh' },
        { Id: '7', Name: 'Đưa vào sử dụng' },
        { Id: '9', Name: 'Nghiệm thu' },
        { Id: '4', Name: 'Tạm dừng' },
        { Id: '3', Name: 'Đóng dự án và thu hồi công nợ' },
        { Id: '10', Name: 'Đã thanh lý' },
        { Id: '11', Name: 'Vật tư' }
    ]

    Late: any[] = [
        { Id: '1', Name: 'Công việc trễ', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
    ];

    ProjectPriority: any[] = [
        { Id: '1', Name: 'Thấp', BadgeClass: 'badge-info', TextClass: 'text-danger' },
        { Id: '2', Name: 'Trung bình', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: '3', Name: 'Cao', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: '4', Name: 'Quan trọng', BadgeClass: 'badge-primary', TextClass: 'text-danger' },
    ];

    ProjectDocumentStatus: any[] = [
        { Id: 1, Name: 'Chưa khai báo tài liệu', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Chưa đủ tài liệu', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 3, Name: 'Đã đủ tài liệu', BadgeClass: 'badge-success', TextClass: 'text-danger' }
    ];

    ProjectTypes: any[] = [
        { Id: 1, Name: 'Dự án thông thường', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Ban quản lý dự án', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 3, Name: 'Làm giải pháp', BadgeClass: 'badge-success', TextClass: 'text-danger' }
    ];

    ProjectErrorStatus: any[] = [
        { Id: 1, Name: 'Không có vấn đề', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 2, Name: 'Có vấn đề', BadgeClass: 'badge-danger', TextClass: 'text-danger' }
    ];

    CourseStatus: any[] = [
        { Id: 0, Name: 'Chưa đào tạo', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 1, Name: 'Đã đào tạo', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    CourseEmployeeStatus: any[] = [
        { Id: 0, Name: 'Chưa chấm điểm', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 1, Name: 'Đã chấm điểm', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    ProblemType: any[] = [
        { Id: 1, Name: 'Lỗi' },
        { Id: 2, Name: 'Vấn đề' },
    ];

    IsProduct: any [] = [
        { Id: 1, Name: 'Đã đủ' },
        { Id: 2, Name: 'Chưa đủ' },
    ];

    BusinessDomainSolution : any [] =[
        { Id: "FAS", Name: 'FAS' },
        { Id: "LAS", Name: 'LAS' },
        { Id: "ETS", Name: 'ETS' },
    ];
    DesignTypesSolution: any[] = [
        { Id: 4, Name: 'LAS' },
        { Id: 5, Name: 'FAS' },
        { Id: 6, Name: 'ETS' }
    ];

    StatusSBU: any[] = [
        { Id: 0, Name: 'Đang hoạt động' },
        { Id: 1, Name: 'Dừng hoạt động' }
    ];

    StatusTask: any[] = [
        { Id: '1', Name: 'Thiết kế' },
        { Id: '2', Name: 'Tài liệu' },
        { Id: '3', Name: 'Chuyển giao' },
        { Id: '4', Name: 'Giải pháp' },
        { Id: '5', Name: 'Hỗ trợ' },
    ];

    GroupUserStatus: any[] = [
        { Id: '0', Name: 'Không sử dụng' },
        { Id: '1', Name: 'Còn sử dụng' },
    ]

    EmployeeTrainingStatus: any[] = [
        { Id: true, Name: 'Đã đào tạo' },
        { Id: false, Name: 'Chưa đào tạo' }
    ]

    WorkType: any[] = [
        { Id: 1, Name: 'Nhóm cơ khí' },
        { Id: 2, Name: 'Nhóm điện' },
        { Id: 3, Name: 'Nhóm điện tử' }
    ];

    EmployeeTraningStatus: any[] = [
        { Id: 1, Name: 'Đạt' },
        { Id: 2, Name: 'Không đạt' },
    ]

    WorkDiary_Done: any[] = [
        { Id: 0, Name: '0 %' },
        { Id: 10, Name: '10 %' },
        { Id: 20, Name: '20 %' },
        { Id: 30, Name: '30 %' },
        { Id: 40, Name: '40 %' },
        { Id: 50, Name: '50 %' },
        { Id: 60, Name: '60 %' },
        { Id: 70, Name: '70 %' },
        { Id: 80, Name: '80 %' },
        { Id: 90, Name: '90 %' },
        { Id: 100, Name: '100 %' },
    ]

    TypeMerchandise: any[] = [
        { Id: 1, Name: 'Linh kiện' },
        { Id: 2, Name: 'Máy' }
    ]

    // Loại hình thanh toán
    TypePayment_NCC_SX: any[] = [
        { Id: 1, Name: 'Điện chuyển tiền (TT: Telegraphic Transfer Remittance)' },
        { Id: 2, Name: 'Thư chuyển tiền (MTR: Mail Transfer Remittance)' },
        { Id: 3, Name: 'Trả tiền lấy chứng từ (C.A.D: Cash Against Document)' },
        { Id: 4, Name: 'Nhờ thu (Collection)' },
        { Id: 5, Name: 'Thư tín dụng hủy ngang (Revocable L/C)' },
        { Id: 6, Name: 'Thư tín dụng không thể hủy ngang (Irrevocable L/C)' },
        { Id: 7, Name: 'Thư tín dụng trả chậm (Usance Payable L/C) UPAC' },
        { Id: 8, Name: 'Thư tín dụng trả dần (Defered L/C)' },
        { Id: 9, Name: 'Thư tín dụng dự phòng (Standby letter of Credit)' },
        { Id: 10, Name: 'Thư tín dụng tuần hoàn (Revolving Letter of Credit)' },
        { Id: 11, Name: 'Thư tín dụng chuyển nhượng (Transferable Letter of Credit)' },
        { Id: 12, Name: 'Thư tín dụng giáp lưng (Back-to-Back Letter of Credit)' },
        { Id: 13, Name: 'Thư tín dụng đối ứng (Reciprocal L/C)' },
        { Id: 14, Name: 'Bitcoin' },
        { Id: 15, Name: 'Paypal' },
        { Id: 16, Name: 'Visa' },
    ]

    // Điều khoản giao hàng
    RulesPayment_NCC_SX: any[] = [
        { Id: 1, Name: 'EXW (EXWORK) giao hàng tại xưởng (địa điểm quy định...)' },
        { Id: 2, Name: 'FAS (Free alongside ship): giao dọc mạng tàu (...cảng bốc quy định)' },
        { Id: 3, Name: 'FOB (Free on board)(Named port of shipment): giao lên tàu (cảng giao hàng xác định)' },
        { Id: 4, Name: 'FCA (free carrier...named point):(giao hàng cho người chuyên chở)' },
        { Id: 5, Name: 'CFR (Cost and Freight)(Named port of destination): tiền hàng và cước phí (cảng đến xác định)' },
        { Id: 6, Name: 'CPT (Freight or carriage paid to destination) - cước phí trả tới....' },
        { Id: 7, Name: 'CIF (Cost, Insurance and Freight)(Named port of destination): tiền hàng, bảo hiểm và cước phí (cảng đến xác định)' },
        { Id: 8, Name: 'CIP (Carriage, Insurance Paid to)(Named place of destination): cước phí, phí bảo hiểm trả đến (địa điểm đến xác định)' },
        { Id: 9, Name: 'DAT (Delivered at Terminal): giao tại bến (…nơi đến quy định)' },
        { Id: 10, Name: 'DAP (Delivered At Place): giao tại nơi đến (nơi đến quy định)' },
        { Id: 11, Name: 'DDP (Delivered Duty Paid): giao tới đích đã nộp thuế (…đích quy định)' },
    ]

    // Loại tiền tệ
    Currency_NCC_SX: any[] = [
        { Id: 1, Name: 'USD' },
        { Id: 2, Name: 'EUR' },
        { Id: 3, Name: 'CNY' },
        { Id: 4, Name: 'VNĐ' },

        { Id: 5, Name: 'GBP' },
        { Id: 6, Name: 'INR' },
        { Id: 7, Name: 'SGD' },
        { Id: 8, Name: 'CNY' },
        { Id: 9, Name: 'HKD' },
        { Id: 10, Name: 'JPY' },
        { Id: 11, Name: 'KRW' },
    ]

    // Loại tiền tệ
    Currency_Import_Profile: any[] = [
        { Id: 1, Name: 'USD' },
        { Id: 2, Name: 'EUR' },
        { Id: 3, Name: 'VNĐ' },

        { Id: 5, Name: 'GBP' },
        { Id: 6, Name: 'INR' },
        { Id: 7, Name: 'SGD' },
        { Id: 8, Name: 'CNY' },
        { Id: 9, Name: 'HKD' },
        { Id: 10, Name: 'JPY' },
        { Id: 11, Name: 'KRW' },
    ]

    // Phương thức vận chuyển
    MethodVC: any[] = [
        { Id: 1, Name: 'Biển' },
        { Id: 2, Name: 'Bộ' },
        { Id: 3, Name: 'Hàng không' }
    ]

    // Chính sách giá
    PricePolicy: any[] = [
        { Id: '1', Name: 'Exclusive Distributor' },
        { Id: '2', Name: 'Distributor' },
        { Id: '3', Name: 'Wholesalers' },
        { Id: '4', Name: 'Retailers' },
        { Id: '5', Name: 'Dealer' },
        { Id: '6', Name: 'Case by case' },
    ]

    FolderDefinitionBetween: any[] = [
        { Id: 0, Name: '' },
        { Id: 1, Name: 'Mã nhóm sản phẩm' },
        { Id: 2, Name: 'Mã sản phẩm' },
        { Id: 3, Name: 'Mã nhóm cha sản phẩm' },
        { Id: 5, Name: 'Mã module nguồn' },
        { Id: 4, Name: '( )' }
    ]

    FolderDefinitionBetweenIndex: any[] = [
        { Id: 0, Name: '' },
        { Id: 1, Name: 'Từ 01-99' },
        { Id: 2, Name: 'Từ a-z' },
    ]

    SaleProductStatus: any[] = [
        { Id: false, Name: 'Đóng', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: true, Name: 'Mở', BadgeClass: 'badge-green', TextClass: 'text-green' },
    ];

    SaleProductIsSync: any[] = [
        { Id: false, Name: 'Thêm mới', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: true, Name: 'Đồng bộ', BadgeClass: 'badge-green', TextClass: 'text-green' },
    ];

    SaleProductDocStatus: any[] = [
        { Id: false, Name: 'Thiếu file', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: true, Name: 'Đủ file', BadgeClass: 'badge-green', TextClass: 'text-green' },
    ];

    SaleProductIsChoose: any[] = [
        { Id: true, Name: 'Đã chọn', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: false, Name: 'Chưa chọn', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];

    ProplemStatus: any[] = [
        { Id: 1, Name: 'Đã xử lý', BadgeClass: 'badge-green', TextClass: 'text-green' },
        { Id: 2, Name: 'Chưa xử lý', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];

    ImportStep: any[] = [
        { Id: 1, Name: 'Xác định nhà cung cấp' },
        { Id: 2, Name: 'Làm hợp đồng' },
        { Id: 3, Name: 'Thanh toán' },
        { Id: 4, Name: 'Theo dõi tiến độ sản xuất' },
        { Id: 5, Name: 'Lựa chọn nhà vận chuyển' },
        { Id: 6, Name: 'Thủ tục thông quan' },
        { Id: 7, Name: 'Nhập kho' }
    ];

    FinishStatus: any[] = [
        { Id: 1, Name: 'Đúng tiến độ', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 2, Name: 'Chậm tiến độ', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 3, Name: 'Không có tiến độ', BadgeClass: 'badge-danger', TextClass: 'text-warning' },
    ];

    ImportConfigStep: any[] = [
        { Id: 2, Name: 'Làm hợp đồng' },
        { Id: 3, Name: 'Thanh toán' },
        { Id: 4, Name: 'Theo dõi tiến độ sản xuất' },
        { Id: 5, Name: 'Lựa chọn nhà vận chuyển' },
        { Id: 6, Name: 'Thủ tục thông quan' },
        { Id: 7, Name: 'Nhập kho' }
    ];

    ImportPayStatus: any[] = [
        {
            Id: 1, Name: 'Chưa thanh toán',
            BadgeClass: 'badge-danger',
            BadgeClassList: [
                { Id: 0, Class: 'badge-secondary', TextClass: 'text-secondary' },
                { Id: 1, Class: 'badge-warning', TextClass: 'text-warning' },
                { Id: 2, Class: 'badge-danger', TextClass: 'text-danger' },
            ], TextClass: 'text-danger'
        },
        { Id: 2, Name: 'Đã thanh toán', BadgeClass: 'badge-success', TextClass: 'text-success' }
    ];

    ImportPayStatusSearch: any[] = [
        { Id: 1, Name: 'Chưa thanh toán', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đã thanh toán', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 3, Name: 'Sắp đến hạn thanh toán (7 ngày)', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 4, Name: 'Quá hạn thanh toán', BadgeClass: 'badge-success', TextClass: 'text-success' }
    ];

    ImportStatus: any[] = [
        { Id: 1, Name: 'Đúng tiến độ', BadgeClass: 'badge-success', TextClass: 'success' },
        { Id: 2, Name: 'Chậm tiến độ', BadgeClass: 'badge-danger', TextClass: 'danger' }
    ];

    StepStatus: any[] = [
        { Id: 1, Name: 'Đúng tiến độ', BadgeClass: 'badge-success', TextClass: 'success' },
        { Id: 2, Name: 'Săp đến hạn HT', BadgeClass: 'badge-warning', TextClass: 'success' },
        { Id: 3, Name: 'Chậm tiến độ', BadgeClass: 'badge-danger', TextClass: 'danger' },
        { Id: 4, Name: 'Không có tiến độ', BadgeClass: 'badge-danger', TextClass: 'danger' }
    ];

    ProductProgress: any[] = [
        { Id: 0, Name: 'Chưa xác định', BadgeClass: 'badge-warning', TextClass: 'success' },
        { Id: 1, Name: 'Đúng tiến độ', BadgeClass: 'badge-success', TextClass: 'success' },
        { Id: 2, Name: 'Chậm tiến độ', BadgeClass: 'badge-danger', TextClass: 'danger' },
    ];

    ProblemStatus: any[] = [
        { Id: 1, Name: 'Đã xử lý', BadgeClass: 'badge-success', TextClass: 'success' },
        { Id: 2, Name: 'Chưa xử lý', BadgeClass: 'badge-danger', TextClass: 'danger' }
    ];

    ImportPRProductStatus: any[] = [
        { Id: false, Name: 'Chưa tạo hồ sơ', BadgeClass: 'badge-danger', TextClass: 'danger' },
        { Id: true, Name: 'Đã tạo hồ sơ', BadgeClass: 'badge-success', TextClass: 'success' }
    ];

    CustomerContactStatus: any[] = [
        { Id: 1, Name: 'Active', BadgeClass: 'badge-danger', TextClass: 'danger' },
        { Id: 2, Name: 'InActive', BadgeClass: 'badge-success', TextClass: 'success' }
    ];

    SearchDataType = {
        Manuafacture: 1,
        Supplier: 2,
        GroupTPA: 3,
        SBU: 4,
        Department: 5,
        Manager: 6,
        Specialize: 7,
        WorkPlace: 8,
        Degree: 9,
        ClassRoom: 10,
        CustomerType: 11,
        Employee: 12,
        DepartmentProcess: 13,
        Stage: 14,
        Project: 15,
        Job: 16,
        PracaticeGroup: 17,
        ModuleGroup: 18,
        SkillGroup: 19,
        MaterialGroup: 20,
        Task: 21,
        ResponsiblePersion: 22,
        ModuleGroupParentChild: 23,
        Product: 24,
        JobPosition: 25,
        ProjectByUser: 26,
        Module: 27,
        Customer: 28,
        EmployeeByUser: 29,
        Skill: 30,
        ProjectByUserDate: 31,
        ProjectByUserSBU: 32,
        Employees: 33,
        SolutionGroup: 34,
        ProductGroup: 35,
        ProductStandardGroup: 36,
        TestCriteriaGroup: 37,
        WorkType: 38,
        Country: 39,
        SaleProductStatus: 40,
        SaleProductInSync: 41,
        ProductStandTPAType: 42,
        Application: 43,
        SaleProductType: 44,
        DocumentGroup: 45,
        FlowStage: 46,
        ErrorAffect: 47,
        QuestionGroup: 48,
        DocumentType: 49,
        RecruitmentRequest: 50,
        MeetingType: 51,
        ChangePlan: 52,
        Role: 53,
        GroupSalary: 54,
        SalaryType: 55,
        Language: 56,
        productandmodule: 57,
        SupplierInProject: 58,
    }

    SearchExpressionTypes: any[] = [
        { Id: 1, Name: '=' },
        { Id: 2, Name: '>' },
        { Id: 3, Name: '>=' },
        { Id: 4, Name: '<' },
        { Id: 5, Name: '<=' }
    ];

    ImportProfileHasSupplier: any[] = [
        { Id: false, Name: 'Chưa có', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: true, Name: 'Đã có', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    HistoryVersion_Version_Module: number = 1;
    HistoryVersion_Version_Product: number = 2;
    HistoryVersion_Version_Practice: number = 3;

    Type_File_Avatar: number = 1;
    Type_File_Image: number = 2;
    Type_File_Video: number = 3;

    PayStatus: any[] = [
        { Id: 2, Name: 'Đã thanh toán', BadgeClass: 'badge-success', TextClass: 'text-primary' },
        { Id: 1, Name: 'Chưa thanh toán', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];

    DocumentStatus: any[] = [
        { Id: 1, Name: 'Đang sử dụng', BadgeClass: 'badge-success', TextClass: 'text-primary' },
        { Id: 2, Name: 'Hủy', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 3, Name: 'Đang review', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
    ];

    DocumentProblemStatus: any[] = [
        { Id: 0, Name: 'Chưa hoàn thành', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 1, Name: 'Đang thực hiện', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đã hoàn thành', BadgeClass: 'badge-success', TextClass: 'text-danger' },
    ]

    TaskTypes: any[] = [
        { Id: 1, Name: 'Thiết kế', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Tài liệu', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 3, Name: 'Chuyển giao', BadgeClass: 'badge-success', TextClass: 'text-danger' },
    ];

    TaskIsDesignModule: any[] = [
        { Id: true, Name: 'Có', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: false, Name: 'Không', BadgeClass: 'badge-warning', TextClass: 'text-danger' }
    ];

    GroupDocument: any = {
        Module_DocumentHDSD: 'D01.02.01',
        Product_GuidePractive: 'D01.02.02',
        Product_DMBTH: 'D01.02.03',
        Product_GuideMaintenance: 'D01.02.04',
        Product_Catelog: 'D01.02.05',
    }

    LaborContract_Type: any[] = [
        { Id: 1, Name: 'Hợp đồng lao động', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 2, Name: 'Hợp đồng nguyên tắc', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];

    DesignWorkStatus: any[] = [
        { Id: false, Name: 'Đang thiết kế', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: true, Name: 'Đã xong', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];

    ApplyStatus: any[] = [
        { Id: 0, Name: 'Chưa có kết quả', BadgeClass: 'badge-secondary', TextClass: 'text-secondary' },
        { Id: 1, Name: 'Không làm việc', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Làm việc', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    WorkStatus: any[] = [
        { Id: false, Name: 'Đang làm việc' },
        { Id: true, Name: 'Đã nghỉ việc' },
    ];

    ProfileStatus: any[] = [
        { Id: 0, Name: 'Chưa đánh giá', BadgeClass: 'badge-secondary', TextClass: 'text-secondary' },
        { Id: 1, Name: 'Không đạt', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đạt', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    InterviewStatus: any[] = [
        { Id: 0, Name: 'Chưa phỏng vấn', BadgeClass: 'badge-secondary', TextClass: 'text-secondary' },
        { Id: 1, Name: 'Không đạt', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đạt', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    FollowStatus: any[] = [
        { Id: false, Name: 'Không giữ liên hệ' },
        { Id: true, Name: 'Giữ liên hệ' },
    ];

    ChannelStatus: any[] = [
        { Id: true, Name: 'Còn sử dụng', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: false, Name: 'Không sử dụng', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];

    AcquaintanceStatus: any[] = [
        { Id: true, Name: 'Có', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: false, Name: 'Không', BadgeClass: 'badge-warning', TextClass: 'text-danger' }
    ];

    Answers: any[] = [
        { Id: 1, Name: 'Đúng', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Sai', BadgeClass: 'badge-warning', TextClass: 'text-danger' }
    ];

    QuestionTypes: any[] = [
        { Id: 1, Name: 'Câu hỏi Đúng/sai', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Câu hỏi Mở', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    InterviewQuestionStatus: any[] = [
        { Id: true, Name: 'Đã hỏi', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: false, Name: 'Chưa hỏi', BadgeClass: 'badge-warning', TextClass: 'text-danger' }
    ];

    ErrorFixStatus: any[] = [
        { Id: 1, Name: 'Chưa xong', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đã xong', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    FixStatus: any[] = [
        { Id: 1, Name: 'Đang giải quyết', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Bị trễ', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    ChangePlan: any[] = [
        { Id: 1, Name: 'Có thay đổi', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Không thay đổi', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    AffectTypes: any[] = [
        { Id: 1, Name: 'Tiến độ', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Chi phí', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 3, Name: 'Chất lượng', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 4, Name: 'Sự hài lòng của khách hàng', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 5, Name: 'Thu tiền', BadgeClass: 'badge-success', TextClass: 'text-success' }
    ];

    //
    ChangeTypes: any[] = [
        { Id: 1, Name: 'Có thay đổi', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 2, Name: 'Không thay đổi', BadgeClass: 'badge-danger', TextClass: 'text-danger' },

    ];

    RecruitmentRequestType: any[] = [
        { Id: 1, Name: 'Toàn thời gian', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 2, Name: 'Bán thời gian', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];

    CompilationType: any[] = [
        { Id: 1, Name: 'Công ty', BadgeClass: 'badge-success', TextClass: 'text-primary' },
        { Id: 2, Name: 'Nhà cung cấp', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
    ];


    RecruitmentRequestStatus: any[] = [
        { Id: 0, Name: 'Đã phê duyệt', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 1, Name: 'Đã hoàn thành', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    RecruitStatus: any[] = [
        { Id: 0, Name: 'Trễ', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 1, Name: 'Chưa xong', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 2, Name: 'Đã hoàn thành', BadgeClass: 'badge-success', TextClass: 'text-success' },
        { Id: 3, Name: 'Hủy yêu cầu', BadgeClass: 'badge-dark', TextClass: 'text-success' }
    ];


    CustomerRequirementStatus: any[] = [
        { Id: 0, Name: 'Yêu cầu khách hàng ', BadgeClass: 'badge-dark', TextClass: 'text-warning' },
        { Id: 1, Name: 'Phân tích yêu cầu khách hàng', BadgeClass: 'badge-danger', TextClass: 'text-warning' },
        { Id: 2, Name: 'Khảo sát', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 3, Name: 'Phân tích giải pháp', BadgeClass: 'badge-info', TextClass: 'text-warning' },
        { Id: 4, Name: 'Làm giải pháp', BadgeClass: 'badge-primary', TextClass: 'text-warning' },
        { Id: 5, Name: 'Lập dự toán', BadgeClass: 'badge-yellow', TextClass: 'text-warning' },
        // {Id: 6, Name: 'Bảo vệ giải pháp', BadgeClass: 'badge-secondary', TextClass: 'text-warning'},
        // {Id: 7, Name: 'Chốt giải pháp', BadgeClass: 'badge-success', TextClass: 'text-warning'},
        //{Id: 8, Name: 'Hủy giải pháp', BadgeClass: 'badge-danger', TextClass: 'text-warning'},

    ];

    CustomerRequirementStatusManger: any[] = [
        { Id: 0, Name: 'Yêu cầu khách hàng ', BadgeClass: 'badge-dark', TextClass: 'text-warning' },
        { Id: 1, Name: 'Phân tích yêu cầu khách hàng', BadgeClass: 'badge-danger', TextClass: 'text-warning' },
        { Id: 2, Name: 'Khảo sát', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 3, Name: 'Phân tích giải pháp', BadgeClass: 'badge-info', TextClass: 'text-warning' },
        { Id: 4, Name: 'Làm giải pháp', BadgeClass: 'badge-primary', TextClass: 'text-warning' },
        { Id: 5, Name: 'Lập dự toán', BadgeClass: 'badge-yellow', TextClass: 'text-warning' },
        // {Id: 6, Name: 'Bảo vệ giải pháp', BadgeClass: 'badge-secondary', TextClass: 'text-warning'},
        // {Id: 7, Name: 'Chốt giải pháp', BadgeClass: 'badge-success', TextClass: 'text-warning'},
        //{Id: 8, Name: 'Hủy giải pháp', BadgeClass: 'badge-danger', TextClass: 'text-warning'},
        { Id: 9, Name: 'Đang cần bổ sung thêm thông tin', BadgeClass: 'badge-orange', TextClass: 'text-warning' },
    ];

    SelectPaymentName: any[] = [
        { Id: 'Tạm ứng', Name: 'Tạm ứng' },
        { Id: 'Thanh toán trước khi giao hàng', Name: 'Thanh toán trước khi giao hàng' },
        { Id: 'Thanh toán sau khi giao hàng', Name: 'Thanh toán sau khi giao hàng' },
        { Id: 'Thanh toán nghiệm thu/ quyết toán', Name: 'Thanh toán nghiệm thu/ quyết toán' },
    ];

    MeetingType: any[] = [
        { Id: 1, Name: 'Online', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 2, Name: 'Trực tiếp', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    MeetingStatus: any[] = [
        { Id: 1, Name: 'Chưa có kế hoạch', BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 2, Name: 'Đã có kế hoạch', BadgeClass: 'badge-info', TextClass: 'text-info' },
        { Id: 3, Name: 'Hủy', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 4, Name: 'Hoàn thành', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    MeetingStep: any[] = [
        { Id: 0, Name: 'Tạo meeting' },
        { Id: 1, Name: 'Xác nhận lịch Meeting' },
        { Id: 2, Name: 'Thực hiện meeting' },
    ];

    TypeRequestEstimateAttach: any[] = [
        { Id: 1, Name: 'Danh mục vật tư' },
        { Id: 2, Name: 'FCM' },
    ];

    TypeSurveyContentAttach: any[] = [
        { Id: 1, Name: 'Tài liệu' },
        { Id: 2, Name: 'Ảnh' },
    ];

    ProductTypeStatus: any[] = [
        { Id: 1, Name: 'TM' },
        { Id: 2, Name: 'SX' },
        { Id: 3, Name: 'Outsource' },
    ];
    MeetingStatusContent: any[] = [
        { Id: 1, Name: 'Chưa tạo Y/C', BadgeClass: 'badge-danger', TextClass: 'text-danger' },
        { Id: 2, Name: 'Đã tạo Y/C', BadgeClass: 'badge-success', TextClass: 'text-success' },
    ];

    //#region thai
    PaymentStatus: any[] = [
        { Id: true, Name: 'Quá hạn' },
        { Id: false, Name: 'Không' }

    ]

    QuotationStatus: any[] = [
        { Id: 1, Name: 'Đã Chốt' },
        { Id: 0, Name: 'Chưa chốt' },
        { Id: 2, Name: 'Thiếu kế hoạch báo giá'}
    ]
    //#region
    //tình trạng  triển khia sản phẩm của dự án
    ProjectProductStatusPerform: any[] = [
        { Id: 0, Name: 'Thiết kế ', BadgeClass: 'badge-dark', TextClass: 'text-warning' },
        { Id: 1, Name: 'Vật tư', BadgeClass: 'badge-danger', TextClass: 'text-warning' },
        { Id: 2, Name: 'Sản xuất', BadgeClass: 'badge-warning', TextClass: 'text-danger' },
        { Id: 3, Name: 'Lắp đặt tại KH', BadgeClass: 'badge-info', TextClass: 'text-warning' },
        { Id: 4, Name: 'Test', BadgeClass: 'badge-primary', TextClass: 'text-warning' },
        { Id: 5, Name: 'Chuyển giao', BadgeClass: 'badge-yellow', TextClass: 'text-warning' },

    ];

    Plan_Status: any[] = [
        { Id: 1, Name: "Open", BadgeClass: 'badge-primary', TextClass: 'text-danger' },
        { Id: 2, Name: "On-Going", BadgeClass: 'badge-warning', TextClass: 'text-warning' },
        { Id: 3, Name: "Close", BadgeClass: 'badge-info', TextClass: 'text-info' },
        { Id: 4, Name: "Stop", BadgeClass: 'badge-danger', TextClass: 'text-dark' },
        { Id: 5, Name: "Cancel", BadgeClass: 'badge-dark', TextClass: 'text-dark' },
    ];

    StageStatus: any[] = [
        { Id: 1, Name: 'Không TK' },
        { Id: 2, Name: 'Chưa TK' },
        { Id: 3, Name: 'Đang TK' },
        { Id: 4, Name: 'Hoàn Thành' },
      ] 
}
