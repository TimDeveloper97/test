import { Component, OnInit, ViewEncapsulation, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { AppSetting, Constants, MessageService, DateUtils, Configuration } from 'src/app/shared';
import { ImportProfileService } from '../services/import-profile.service';

@Component({
  selector: 'app-import-profile-history',
  templateUrl: './import-profile-history.component.html',
  styleUrls: ['./import-profile-history.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ImportProfileHistoryComponent implements OnInit {

  startIndex: number = 1;

  exportListFileModel = {
    Id: '',
    Step: 0
  }

  listFileModel: any = { ListDatashet: [] }

  searchModel:any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'PRDueDate',
    OrderType: true,
    Code: '',
    PayStatus: '',
    Status: '',
    WorkStatus: 0,
    DueDateFrom: null,
    DueDateTo: null,
    PayDateFrom: null,
    PayDateTo: null,
    DeliveryDateFrom: null,
    DeliveryDateTo: null,
    DueDateFromV: null,
    DueDateToV: null,
    PayDateFromV: null,
    PayDateToV: null,
    DeliveryDateFromV: null,
    DeliveryDateToV: null,
    EmployeeId: null,
    FinishStatus: null,
    StepSearch: 0
  };

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên/mã',
    Items: [
      {
        Name: 'Nhân viên',
        FieldName: 'EmployeeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employees,
        Columns: [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn nhân viên',
        Permission: ['F120804'],
      },
      {
        Name: 'Quy trình',
        FieldName: 'StepSearch',
        Placeholder: 'Chọn quy trình',
        Type: 'select',
        Data: this.constant.ImportStep,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tiến độ vật tư theo PR Due Date',
        FieldName: 'Status',
        Placeholder: 'Chọn tình trạng',
        Type: 'select',
        Data: this.constant.ImportStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },     
      {
        Name: 'Mã dự án',
        FieldName: 'ProjectCode',
        Placeholder: 'Nhập mã dự án',
        Type: 'text',
      },     
      {
        Name: 'Tiến độ',
        FieldName: 'FinishStatus',
        Placeholder: 'Chọn tiến độ',
        Type: 'select',
        Data: this.constant.FinishStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      // {
      //   Name: 'Mã sản phẩm',
      //   FieldName: 'ProductCode',
      //   Placeholder: 'Nhập mã sản phẩm',
      //   Type: 'text',
      // },
      {
        Name: 'Nhà cung cấp',
        FieldName: 'SupplierId',
        Placeholder: 'Chọn Nhà cung cấp',
        Type: 'dropdown', SelectMode: 'single',
        DataType: this.constant.SearchDataType.Supplier,
        Columns: [{ Name: 'Code', Title: 'Mã NCC' }, { Name: 'Name', Title: 'Tên NCC' }],
        DisplayName: 'Code',
        ValueName: 'Id'
      },
      {
        Name: 'PR Due date',
        FieldNameFrom: 'DueDateFromV',
        FieldNameTo: 'DueDateToV',
        Type: 'date',
      },
      {
        Name: 'Tình trạng thanh toán',
        FieldName: 'PayStatus',
        Placeholder: 'Chọn tình trạng thanh toán',
        Type: 'select',
        Data: this.constant.ImportPayStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  listData: any[] = [];

  listDataKanban: any[] = [];

  isShowTable = true;
  isShowKanban = false;
  missionListHeight = 400;

  StartIndex = 1;

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private router: Router,
    private importProfileService: ImportProfileService,
    private dateUtils: DateUtils,
    private config: Configuration,
  ) { }

  ngOnInit(): void {
    this.resize();
    this.appSetting.PageTitle = 'Quản lý hồ sơ nhập khẩu đã kết thúc';
    this.searchModel.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    this.searchImportProfile();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.resize();
  }

  resize() {
    this.missionListHeight = window.innerHeight - 250;
  }

  searchImportProfile() {
    this.searchModel.DueDateFrom = null;
    if (this.searchModel.DueDateFromV) {
      this.searchModel.DueDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DueDateFromV);
    }
    this.searchModel.DueDateTo = null;
    if (this.searchModel.DueDateToV) {
      this.searchModel.DueDateTo = this.dateUtils.convertObjectToDate(this.searchModel.DueDateToV);
    }

    this.searchModel.PayDateTo = null;
    if (this.searchModel.PayDateToV) {
      this.searchModel.PayDateTo = this.dateUtils.convertObjectToDate(this.searchModel.PayDateToV);
    }

    this.searchModel.DeliveryDateFrom = null;
    if (this.searchModel.DeliveryDateFromV) {
      this.searchModel.DeliveryDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DeliveryDateFromV);
    }

    this.searchModel.DeliveryDateTo = null;
    if (this.searchModel.DeliveryDateToV) {
      this.searchModel.DeliveryDateTo = this.dateUtils.convertObjectToDate(this.searchModel.DeliveryDateToV);
    }

    this.importProfileService.searchImportProfileFinish(this.searchModel).subscribe((result: any) => {
      this.listData = result.ListResult;
      this.searchModel.TotalItems = result.TotalItem;
      this.StartIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
    }, error => {

      this.messageService.showError(error);
    });

  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'DueDate',
      OrderType: true,
      Code: '',
      PayStatus: '',
      Status: '',
      WorkStatus: 0,
      DueDateFrom: null,
      DueDateTo: null,
      PayDateFrom: null,
      PayDateTo: null,
      DeliveryDateFrom: null,
      DeliveryDateTo: null,
      DueDateFromV: null,
      DueDateToV: null,
      PayDateFromV: null,
      PayDateToV: null,
      DeliveryDateFromV: null,
      DeliveryDateToV: null,
      EmployeeId: null,
      FinishStatus: null,
      StepSearch: 0
    };
    this.searchModel.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    this.searchImportProfile();
  }

  downListFile(id) {
    this.exportListFileModel.Id = id;
    this.exportListFileModel.Step = 0;
    this.importProfileService.getListFile(this.exportListFileModel).subscribe(data => {
      if (data.length <= 0) {
        this.messageService.showMessage("Không có file để tải");
        return;
      }
      var listFilePath: any[] = [];
      data.forEach(element => {
        listFilePath.push({
          Path: element.Path,
          FileName: element.FileName
        });
      });
      this.listFileModel.Name = "DanhSachChungTu_";
      this.listFileModel.ListDatashet = listFilePath;
      this.importProfileService.downloadListFile(this.listFileModel).subscribe(data => {
        var link = document.createElement('a');
        link.setAttribute("type", "hidden");
        link.href = this.config.ServerFileApi + data.PathZip;
        link.download = 'Download.zip';
        document.body.appendChild(link);
        // link.focus();
        link.click();
        document.body.removeChild(link);
      }, e => {
        this.messageService.showError(e);
      });
    }, e => {
      this.messageService.showError(e);
    });
    
  }

}
