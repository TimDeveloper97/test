import { Component, OnInit, ViewEncapsulation, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { AppSetting, Constants, MessageService, DateUtils, Configuration } from 'src/app/shared';
import { ImportProfileService } from '../services/import-profile.service';

@Component({
  selector: 'app-import-profile-manage',
  templateUrl: './import-profile-manage.component.html',
  styleUrls: ['./import-profile-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ImportProfileManageComponent implements OnInit {

  startIndex: number = 1;
  exportListFileModel = {
    Id: '',
    Step: 0
  }

  listFileModel: any = { ListDatashet: [] }

  fromDateSearch: any = null;
  toDateSearch: any = null;

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PayWarningTotal: 0,
    PayExpiredTotal: 0,
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
    IsSearch7Day: false,
    SearchDateFrom: null,
    SearchDateTo: null,
    ProductionExpectedDateFrom: null,
    ProductionExpectedDateTo: null,
    ProductionExpectedDateFromV: null,
    ProductionExpectedDateToV: null,
    ProductionExpiredTotal: 0,
    IsProductionExpiredOnWeek: false,
    IsProductionExpired: false,
  };

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên/mã hồ sơ',
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
        FieldName: 'Step',
        Placeholder: 'Chọn quy trình',
        Type: 'select',
        Data: this.constant.ImportStep,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tiến độ nhập khẩu hàng hóa',
        FieldName: 'FinishStatus',
        Placeholder: 'Chọn tiến độ',
        Type: 'select',
        Data: this.constant.FinishStatus,
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
        Name: 'Hạn thanh toán',
        FieldNameFrom: 'PayDateFromV',
        FieldNameTo: 'PayDateToV',
        Type: 'date',
      },
      {
        Name: 'Tình trạng thanh toán',
        FieldName: 'PayStatus',
        Placeholder: 'Chọn tình trạng thanh toán',
        Type: 'select',
        Data: this.constant.ImportPayStatusSearch,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'PO Due date',
        FieldNameFrom: 'DeliveryDateFromV',
        FieldNameTo: 'DeliveryDateToV',
        Type: 'date',
      },
      {
        Name: 'Ngày giao hàng dự kiến',
        FieldNameFrom: 'ProductionExpectedDateFromV',
        FieldNameTo: 'ProductionExpectedDateToV',
        Type: 'date',
      },
    ]
  };

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  listData: any[] = [];

  listDataKanban: any[] = [];

  isShowTable = true;
  isShowKanban = false;
  isSearch7Day = false;
  isSearchByDate = false;
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
    this.appSetting.PageTitle = 'Quản lý hồ sơ nhập khẩu';
    this.searchModel.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    this.searchImportProfile();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.resize();
  }

  resize() {
    this.missionListHeight = window.innerHeight - 280;
  }

  searchImportProfile() {
    this.searchModel.IsSearch7Day = this.isSearch7Day;
    if (this.searchModel.DueDateFromV) {
      this.searchModel.DueDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DueDateFromV);
    }

    if (this.searchModel.DueDateToV) {
      this.searchModel.DueDateTo = this.dateUtils.convertObjectToDate(this.searchModel.DueDateToV);
    }

    if (this.searchModel.PayDateFromV) {
      this.searchModel.PayDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.PayDateFromV);
    }

    if (this.searchModel.PayDateToV) {
      this.searchModel.PayDateTo = this.dateUtils.convertObjectToDate(this.searchModel.PayDateToV);
    }

    if (this.searchModel.DeliveryDateFromV) {
      this.searchModel.DeliveryDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DeliveryDateFromV);
    }

    if (this.searchModel.DeliveryDateToV) {
      this.searchModel.DeliveryDateTo = this.dateUtils.convertObjectToDate(this.searchModel.DeliveryDateToV);
    }

    if (this.searchModel.ProductionExpectedDateFromV) {
      this.searchModel.ProductionExpectedDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.ProductionExpectedDateFromV);
    }

    if (this.searchModel.ProductionExpectedDateToV) {
      this.searchModel.ProductionExpectedDateTo = this.dateUtils.convertObjectToDate(this.searchModel.ProductionExpectedDateToV);
    }

    if (this.isShowTable) {
      this.importProfileService.searchImportProfile(this.searchModel).subscribe((result: any) => {
        this.listData = result.ListResult;
        this.searchModel.TotalItems = result.TotalItem;
        this.searchModel.PayWarningTotal = result.PayWarningTotal;
        this.searchModel.PayExpiredTotal = result.PayExpiredTotal;
        this.searchModel.ProductionExpiredTotal = result.ProductionExpiredTotal;
        this.searchModel.ProductionExpiredWeekTotal = result.ProductionExpiredWeekTotal;
        this.StartIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      }, error => {

        this.messageService.showError(error);
      });
    } else {
      this.importProfileService.searchImportProfileKanban(this.searchModel).subscribe((result: any) => {
        this.listDataKanban = result.ListResult;
        this.searchModel.TotalItems = result.TotalItem;
        this.searchModel.PayWarningTotal = result.PayWarningTotal;
        this.searchModel.PayExpiredTotal = result.PayExpiredTotal;
        this.searchModel.ProductionExpiredTotal = result.ProductionExpiredTotal;
        this.StartIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      }, error => {

        this.messageService.showError(error);
      });
    }

    this.searchModel.IsProductionExpiredOnWeek = false;
    this.searchModel.IsProductionExpired = false;
    this.searchModel.IsPayExpired = false;
    this.searchModel.IsPayWarning = false;
  }

  searchProductionExpiredOnWeek() {
    this.searchModel.IsProductionExpiredOnWeek = true;

    this.searchImportProfile();
  }

  searchProductionExpired() {
    this.searchModel.IsProductionExpired = true;

    this.searchImportProfile();
  }

  searchPayExpired() {
    this.searchModel.IsPayExpired = true;

    this.searchImportProfile();
  }

  searchPayWarning() {
    this.searchModel.IsPayWarning = true;

    this.searchImportProfile();
  }

  showCreate() {
    this.router.navigate(['nhap-khau/ho-so-nhap-khau/them-moi']);
  }

  exportExcel() {

  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PayWarningTotal: 0,
      PayExpiredTotal: 0,
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
      IsSearch7Day: false,
      SearchDateFrom: null,
      SearchDateTo: null,
      ProductionExpectedDateFrom: null,
      ProductionExpectedDateTo: null,
      ProductionExpectedDateFromV: null,
      ProductionExpectedDateToV: null,
      ProductionExpiredTotal: 0,
      IsProductionExpiredOnWeek: false,
      IsProductionExpired: false
    };
    this.isSearch7Day = false;
    this.isSearchByDate = false;
    this.fromDateSearch = null;
    this.toDateSearch = null;
    this.searchModel.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    this.searchImportProfile();
  }

  showConfirmDelete(id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá hồ sơ này không?").then(
      data => {
        this.importProfileService.deleteImportProfile({ Id: id }).subscribe((result: any) => {
          this.messageService.showSuccess('Xóa hồ sơ thành công!');
          this.searchImportProfile();
        }, error => {
          this.messageService.showError(error);
        });
      }
    );
  }

  showTable() {
    this.isShowTable = true;
    this.searchImportProfile();
  }

  showKanban() {
    this.isShowTable = false;
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

  search7Day() {
    this.isSearch7Day = !this.isSearch7Day;
    if (this.isSearchByDate) {
      this.isSearchByDate = false;
      this.fromDateSearch = null;
      this.toDateSearch = null;
      this.searchModel.SearchDateFrom = null;
      this.searchModel.SearchDateTo = null;
    }
    this.searchImportProfile();
  }

  searchByDate() {
    this.isSearchByDate = true;
    if (this.isSearch7Day) {
      this.isSearch7Day = false;
    }
    if (this.fromDateSearch) {
      this.searchModel.SearchDateFrom = this.dateUtils.convertObjectToDate(this.fromDateSearch);
    }

    if (this.toDateSearch) {
      this.searchModel.SearchDateTo = this.dateUtils.convertObjectToDate(this.toDateSearch);
    }
    this.searchImportProfile();
  }

  clearSearchByDate() {
    this.isSearchByDate = false;
    this.fromDateSearch = null;
    this.toDateSearch = null;
    this.searchModel.SearchDateFrom = null;
    this.searchModel.SearchDateTo = null;
    this.searchImportProfile();
  }

  
}
