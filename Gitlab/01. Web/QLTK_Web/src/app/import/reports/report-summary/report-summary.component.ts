import { Component, OnInit, ViewEncapsulation, HostListener, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { AppSetting, Constants, MessageService, Configuration, DateUtils } from 'src/app/shared';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label, MultiDataSet } from 'ng2-charts';
import { ImportProfileReportService } from '../../services/import-profile-report.service';

@Component({
  selector: 'app-report-summary',
  templateUrl: './report-summary.component.html',
  styleUrls: ['./report-summary.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportSummaryComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private importProfileReportService: ImportProfileReportService,
    private messageService: MessageService,
    private config: Configuration,
    private dateUtils: DateUtils,
  ) { }

  searchModel = {
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
    TimeType: null,
    DateFromV: null,
    DateToV: null,
    DateFrom: null,
    DateTo: null
  };

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên/mã',
    Items: [
      // {
      //   Name: 'Thời gian',
      //   FieldName: 'TimeType',
      //   Placeholder: 'Nhóm',
      //   Type: 'select',
      //   Data: this.constant.SearchDebtTimeTypes,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      {
        Name: 'Khoảng thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
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
        // Permission: ['F060705', 'F110702'],
      },
      {
        Name: 'Tình trạng',
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
        Name: 'Nhà cung cấp',
        FieldName: 'SupplierId',
        Placeholder: 'Chọn Nhà cung cấp',
        Type: 'dropdown', SelectMode: 'single',
        DataType: this.constant.SearchDataType.Supplier,
        Columns: [{ Name: 'Code', Title: 'Mã NCC' }, { Name: 'Name', Title: 'Tên NCC' }],
        DisplayName: 'Code',
        ValueName: 'Id'
      },
    ]
  };

  public barChartOptions: ChartOptions = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{ ticks: { stepSize: 1 } }] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public barChartLabels: Label[] = [];
  public barChartStepLabels: Label[] = [];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;

  public barChartData: ChartDataSets[] = [
    { data: [], label: 'HS đúng tiến độ' },
    { data: [], label: 'HS chậm tiến độ' }
  ];

  public barChartStepData: ChartDataSets[] = [
    { data: [], label: 'HS đúng tiến độ' },
    { data: [], label: 'HS chậm tiến độ' }
  ];

  public doughnutChartData: number[] = [];
  public doughnutChartType: ChartType = 'doughnut';
  public pieChartType: ChartType = 'pie';

  public pieChartLabels: Label[] = ['Đúng tiến độ', 'Chậm tiến độ'];
  public pieChartColors = [
    {
      backgroundColor: ['#007bff', '#ff99cc'],
    },
  ];

  importProfileEmployee: any[] = [];
  importProfileSupplier: any[] = [];
  importProfileTransport: any[] = [];
  importProfileCustoms: any[] = [];
  importProfile: any = {
    Total: 0,
    OngoingQuantity: 0,
    SlowQuantity: 0,
    SupplierQuantity: 0,
    ContractQuantity: 0,
    PayQuantity: 0,
    ProductionQuantity: 0,
    TranportQuantity: 0,
    CustomsQuantity: 0,
    WarehouseQuantity: 0
  };

  @ViewChild('scrollByEmployee', { static: false }) scrollByEmployee: ElementRef;
  @ViewChild('scrollByEmployeeHeader', { static: false }) scrollByEmployeeHeader: ElementRef;

  @ViewChild('scrollBySupplier', { static: false }) scrollBySupplier: ElementRef;
  @ViewChild('scrollBySupplierHeader', { static: false }) scrollBySupplierHeader: ElementRef;

  @ViewChild('scrollByTransport', { static: false }) scrollByTransport: ElementRef;
  @ViewChild('scrollByTransportHeader', { static: false }) scrollByTransportHeader: ElementRef;

  @ViewChild('scrollByCustom', { static: false }) scrollByCustom: ElementRef;
  @ViewChild('scrollByCustomHeader', { static: false }) scrollByCustomHeader: ElementRef;

  ngAfterViewInit() {
    this.scrollByEmployee.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollByEmployeeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollBySupplier.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollBySupplierHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollByTransport.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollByTransportHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollByCustom.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollByCustomHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnInit(): void {
    this.appSetting.PageTitle = 'Báo cáo Hồ sơ đã kết thúc';
    this.resize();
    this.getReport();
  }

  ngOnDestroy() {
    this.scrollByEmployee.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollBySupplier.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollByTransport.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollByCustom.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.resize();

  }

  resize() {
    setTimeout(function () {
      let right = document.getElementById('rightdiv');

      let left = document.getElementById('leftdiv');


      if (right && left) {
        left.style.height = right.offsetHeight + 'px';
      }

      let rightPrice = document.getElementById('rightPricediv');

      let leftPrice = document.getElementById('leftPricediv');


      if (rightPrice && leftPrice) {

        if (rightPrice.offsetHeight > leftPrice.offsetHeight) {
          leftPrice.style.height = rightPrice.offsetHeight + 'px';
        }
        else {
          rightPrice.style.height = leftPrice.offsetHeight + 'px';
        }
      }
    }, 100);

  }

  getReport() {
    this.searchModel.DateFrom = null;
    if (this.searchModel.DateFromV) {
      this.searchModel.DateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DateFromV);
    }

    this.searchModel.DateTo = null;
    if (this.searchModel.DateToV) {
      this.searchModel.DateTo = this.dateUtils.convertObjectToDate(this.searchModel.DateToV);
    }

    this.importProfileReportService.getReportSummary(this.searchModel).subscribe((result: any) => {
      this.barChartData[0].data = result.BarChartData;
      this.barChartData[1].data = result.BarChartDataSlow;
      this.barChartStepData[0].data = result.BarChartStepData;
      this.barChartStepData[1].data = result.BarChartStepDataSlow;
      this.barChartLabels = result.BarChartLabels;
      this.barChartStepLabels = ['Xác định NCC', 'Hợp đồng', 'Thanh toán', 'Sản xuất', 'Vận chuyển', 'Hải quản', 'Nhập kho'];
      this.doughnutChartData = result.PieChartData;
      this.importProfile = result.ImportProfile;
      this.importProfileEmployee = result.ImportProfileEmployee;
      this.importProfileCustoms = result.ImportProfileCustoms;
      this.importProfileSupplier = result.ImportProfileSupplier;
      this.importProfileTransport = result.ImportProfileTransport;
      this.resize();
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
      TimeType: '6',
      DateFromV: null,
      DateToV: null,
      DateFrom: null,
      DateTo: null
    };
  }

  exportExcel() {
    this.importProfileReportService.exportExcelSummary(this.searchModel).subscribe((result: any) => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + result;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, error => {
      this.messageService.showError(error);
    });
  }
}
