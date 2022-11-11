import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, ComboboxService, Constants, DateUtils, MessageService } from 'src/app/shared';
import { ReportProgressProjectService } from '../service/report-progress-project.service';

@Component({
  selector: 'app-report-progress-project',
  templateUrl: './report-progress-project.component.html',
  styleUrls: ['./report-progress-project.component.scss'],
  encapsulation: ViewEncapsulation.None,

})
export class ReportProgressProjectComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    public dateUtils: DateUtils,
    private reportProgressProjectSevice: ReportProgressProjectService,
    private comboboxService: ComboboxService,
    private messageService: MessageService,
  ) { }

  Stages: any[] = [];
  ReportProgressProjects: any[] = [];
  statIndex = 1;
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'MaDuAn',
    OrderType: true,
    SBUId: '',
    DateStartTo: '',
    DateStartFrom: '',
    PlanDueDateTo: '',
    PlanDueDateFrom: '',
    PlanKICKOFFTo: '',
    PlanKICKOFFFrom: '',
    Type: 0,
    ManageId: '',
    Status: [],
    PaymentStatus: '',
    Delay: '',
    DelayType: 0,
    StageStatus: 0,
    Stage: '',
    Priority: 0,
    DepartmentId: '',

  }
  CountVT: number;
  CountTK: number;
  CountDDA: number;
  CountCKO: number;
  CountDVSD: number;
  CountHC: number;
  CountLD: number;
  CountSx: number;
  CountTD: number;
  CountNT: number;
  CountDTL: number;
  CountDTK: number;
  TongTienHopDong: number;
  TongSoTienDaThu: number;

  //#region thai
  VDTDs: any[] = [
    { Id: 1, Name: 'Đang có vấn đề' },
    { Id: 2, Name: 'Đã xử lý xong' },
    { Id: 3, Name: 'Không có vấn đề' },
  ];
  StageStatus: any[] = [
    { Id: 1, Name: 'Không TK' },
    { Id: 2, Name: 'Chưa TK' },
    { Id: 3, Name: 'Đang TK' },
    { Id: 4, Name: 'Hoàn Thành' },
  ];
  //#region thai
  PaymentStatus: any[] = [
    { Id: 1, Name: 'Quá hạn' },
    { Id: 0, Name: 'Không' }

  ]
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên hoặc mã dự án',
    Items: [
      {
        Name: 'SBU phụ trách',
        FieldName: 'SBUId',
        Placeholder: 'SBU người phụ trách',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Phòng QLDA',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng QLDA',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
      },
      {
        Name: 'Loại dự án',
        FieldName: 'Priority',
        Placeholder: 'Loại dự án',
        Type: 'select',
        Data: this.constant.ProjectTypes,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
      },
      {
        Name: 'Ngày bắt đầu',
        FieldNameTo: 'DateStartToV',
        FieldNameFrom: 'DateStartFromV',
        Type: 'date'
      },
      {
        Name: 'Mức độ ưu tiên',
        FieldName: 'Type',
        Placeholder: 'mức độ ưu tiên',
        Type: 'select',
        Data: this.constant.ProjectPriority,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
      },
      {
        Name: 'Người quản lý phụ trách',
        FieldName: 'ManageId',
        Placeholder: 'người phụ trách',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Employee,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        Columns: [{ Name: 'Code', Title: 'Mã người phụ trách' }, { Name: 'Name', Title: 'Tên người phụ trách' }],
      },
      {
        Name: 'Trạng thái dự án',
        FieldName: 'Status',
        Placeholder: 'trạng thái dự án',
        Type: 'dropdown',
        Data: this.constant.ProjectStatus,
        DisplayName: 'Name',
        ValueName: 'Id',
        SelectMode: 'multiple',
        Columns: [{ Name: 'Name', Title: 'Trạng thái dự án' }],
      },
      {
        Name: 'Tình trạng công nợ',
        FieldName: 'PaymentStatus',
        Placeholder: 'tình trạng công nợ',
        Type: 'select',
        Data: this.PaymentStatus,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
      },
      {                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        Name: 'Vấn đề tồn đọng',
        FieldName: 'VDTD',
        Placeholder: 'vấn đề tồn đọng',
        Type: 'select',
        Data: this.VDTDs,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
      },
      {
        Name: 'Kế hoạch triển khai',
        FieldNameTo: 'PlanDueDateToV',
        FieldNameFrom: 'PlanDueDateFromV',
        Type: 'date'
      },
      {
        Name: 'Kế hoạch xác nhận KH',
        FieldNameTo: 'PlanKICKOFFToV',
        FieldNameFrom: 'PlanKICKOFFFromV',
        Type: 'date'
      },
      {
        Placeholder: 'số ngày lệch',
        Name: 'Số ngày lệch',
        FieldName: 'Delay',
        FieldNameType: 'DelayType',
        Type: 'number',
      },
      {
        Placeholder: 'công đoạn',
        Name: 'Công đoạn',
        FieldName: 'Stage',
        FieldNameType: 'StageStatus',
        Type: 'StageStatus',
        DataType: this.constant.SearchDataType.Stage,
        DisplayName: 'Name',
        ValueName: 'Id',
      },


    ]
  };


  ngOnInit(): void {
    this.appSetting.PageTitle = "Báo cáo tiến độ triển khai dự án";
    this.GetReportProgressProjects();
  }

  GetReportProgressProjects() {
    this.reportProgressProjectSevice.GetReportProgressProject(this.model).subscribe((data) => {
      if (data) {
        this.statIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.ReportProgressProjects = data.ReportProgressProjectModels.ListResult;
        this.Stages = data.Stages;
        this.model.TotalItems = data.ReportProgressProjectModels.TotalItem;
        this.CountVT = data.CountVT;
        this.CountTK = data.CountTK;
        this.CountDDA = data.CountDDA;
        this.CountCKO = data.CountCKO;
        this.CountDVSD = data.CountDVSD;
        this.CountHC = data.CountHC;
        this.CountLD = data.CountLD;
        this.CountSx = data.CountSx;
        this.CountTD = data.CountTD;
        this.CountNT = data.CountNT;
        this.CountDTL = data.CountDTL;
        this.CountDTK = this.model.TotalItems - this.CountDTL - this.CountTD;
        this.TongTienHopDong = data.TongTienHopDong;
        this.TongSoTienDaThu = data.TongSoTienDaThu;


      }
    });
  }
  search() {
    if (this.model.DateStartFromV != null) {
      this.model.DateStartFrom = this.dateUtils.convertObjectToDate(this.model.DateStartFromV);
    }
    if (this.model.DateStartFromV == null) {
      this.model.DateStartFrom = null;
    }
    if (this.model.DateStartToV != null) {
      this.model.DateStartTo = this.dateUtils.convertObjectToDate(this.model.DateStartToV)
    }
    if (this.model.DateStartToV == null) {
      this.model.DateStartTo = null;
    }
    if (this.model.PlanDueDateFromV != null) {
      this.model.PlanDueDateFrom = this.dateUtils.convertObjectToDate(this.model.PlanDueDateFromV);
    }
    if (this.model.PlanDueDateFromV == null) {
      this.model.PlanDueDateFrom = null;
    }
    if (this.model.PlanDueDateToV != null) {
      this.model.PlanDueDateTo = this.dateUtils.convertObjectToDate(this.model.PlanDueDateToV)
    }
    if (this.model.PlanDueDateToV == null) {
      this.model.PlanDueDateTo = null;
    }
    if (this.model.PlanKICKOFFFromV != null) {
      this.model.PlanKICKOFFFrom = this.dateUtils.convertObjectToDate(this.model.PlanKICKOFFFromV);
    }
    if (this.model.PlanKICKOFFFromV == null) {
      this.model.PlanKICKOFFFrom = null;
    }
    if (this.model.PlanKICKOFFToV != null) {
      this.model.PlanKICKOFFTo = this.dateUtils.convertObjectToDate(this.model.PlanKICKOFFToV)
    }
    if (this.model.PlanKICKOFFToV == null) {
      this.model.PlanKICKOFFTo = null;
    }
    this.model.IsSynchronized = false;
    this.GetReportProgressProjects();
  }
  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'MaDuAn',
      OrderType: true,
      SBUId: '',
      DateStartTo: '',
      DateStartFrom: '',
      PlanDueDateTo: '',
      PlanDueDateFrom: '',
      PlanKICKOFFTo: '',
      PlanKICKOFFFrom: '',
      Type: 0,
      ManageId: '',
      Status: [],
      PaymentStatus: '',
      Delay: '',
      DelayType: 0,
      StageStatus: 0,
      Stage: '',
      Priority: 0,
      DepartmentId: '',
    }
    this.search();
  }
  reload(){
    this.model.IsSynchronized = true;
    this.GetReportProgressProjects();
  }
}
