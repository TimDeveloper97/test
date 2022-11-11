import { Component, OnInit, ViewEncapsulation, HostListener } from '@angular/core';
import { AppSetting, Constants, MessageService, Configuration } from 'src/app/shared';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label, MultiDataSet } from 'ng2-charts';
import { ImportProfileReportService } from '../../services/import-profile-report.service';

@Component({
  selector: 'app-report-ongoing',
  templateUrl: './report-ongoing.component.html',
  styleUrls: ['./report-ongoing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportOngoingComponent implements OnInit {

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private importProfileReportService: ImportProfileReportService,
    private messageService: MessageService,
    private config: Configuration) { }

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
    DeliveryDateToV: null
  };

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo tên/mã',
    Items: [
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
        Name: 'Due date',
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
        Data: this.constant.ImportPayStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Ngày giao hàng dự kiến',
        FieldNameFrom: 'DeliveryDateFromV',
        FieldNameTo: 'DeliveryDateToV',
        Type: 'date',
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
  public barChartLabels: Label[] = ['Xác định NCC', 'Hợp đồng', 'Thanh toán', 'Sản xuất', 'Vận chuyển', 'Hải quản', 'Nhập kho'];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;

  public barChartData: ChartDataSets[] = [
    { data: [], label: 'Số lượng hồ sơ' }
  ];

  public doughnutChartData: number[] = [];
  public doughnutChartType: ChartType = 'doughnut';
  public pieChartType: ChartType = 'pie';
  public pieChartColors = [
    {
      backgroundColor: ['#ff9933', '#007bff', '#ff99cc', '#26c6da', '#cc99ff', '#66bb6a', '#737373'],
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
  ngOnInit(): void {
    this.appSetting.PageTitle = 'Báo cáo Hồ sơ đang thực hiện';
    this.resize();
    this.getReport();
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
    this.importProfileReportService.getReport(this.searchModel).subscribe((result: any) => {
      this.barChartData[0].data = result.BarChartData;
      this.doughnutChartData = result.PieChartData;
      this.importProfile = result.ImportProfile;
      this.importProfileEmployee = result.ImportProfileEmployee;
      this.importProfileCustoms = result.ImportProfileCustoms;
      this.importProfileSupplier = result.ImportProfileSupplier;
      this.importProfileTransport = result.ImportProfileTransport;
    }, error => {
      this.messageService.showError(error);
    });
  }

  exportExcel() {
    this.importProfileReportService.exportExcel(this.searchModel).subscribe((result: any) => {
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
