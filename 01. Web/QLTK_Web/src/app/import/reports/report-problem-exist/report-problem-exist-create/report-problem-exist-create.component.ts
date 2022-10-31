import { Component, HostListener, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import * as moment from 'moment';
import { Label } from 'ng2-charts';
import { ReportProblemExistService } from 'src/app/import/services/report-problem-exist.service';
import { Constants, DateUtils, Configuration, MessageService, ComboboxService, AppSetting } from 'src/app/shared';

@Component({
  selector: 'app-report-problem-exist-create',
  templateUrl: './report-problem-exist-create.component.html',
  styleUrls: ['./report-problem-exist-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportProblemExistCreateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private dateUtils: DateUtils,
    private config: Configuration,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ReportProblemExistService,
    private comboboxService: ComboboxService,
  ) { }

  id: string;
  isAction: boolean = false;
  createReportDate = null;
  listEmployee: any[] = [];
  listSupplier: any[] = [];
  listMonth: any[] = [];
  listYear: any[] = [];
  listProblem: any[] = [];
  listReportEmployee: any[] = [
    { Id: '1', Name: 'Nguyễn Văn A', Total: 30, SupplierQuantity: 2, ContractQuantity: 5, PayQuantity: 8, ProductionQuantity: 6, TranportQuantity: 5, CustomsQuantity: 4, WarehouseQuantity: 0 },
    { Id: '2', Name: 'Đỗ Thị Ngọc Anh', Total: 91, SupplierQuantity: 7, ContractQuantity: 13, PayQuantity: 45, ProductionQuantity: 0, TranportQuantity: 2, CustomsQuantity: 23, WarehouseQuantity: 1 },
    { Id: '3', Name: 'Đào Xuân Đại', Total: 13, SupplierQuantity: 2, ContractQuantity: 1, PayQuantity: 2, ProductionQuantity: 0, TranportQuantity: 0, CustomsQuantity: 8, WarehouseQuantity: 0 },
    { Id: '4', Name: 'Lê Công Tùng', Total: 60, SupplierQuantity: 12, ContractQuantity: 7, PayQuantity: 22, ProductionQuantity: 0, TranportQuantity: 0, CustomsQuantity: 18, WarehouseQuantity: 1 },
    { Id: '5', Name: 'Lê Thanh Lâm', Total: 32, SupplierQuantity: 3, ContractQuantity: 0, PayQuantity: 9, ProductionQuantity: 1, TranportQuantity: 5, CustomsQuantity: 14, WarehouseQuantity: 0 },
  ];
  listReportSupplier: any[] = [
    { Id: '1', Name: 'COGNEX IRELAND LTD.', Total: 30, SupplierQuantity: 2, ContractQuantity: 5, PayQuantity: 8, ProductionQuantity: 6, TranportQuantity: 5, CustomsQuantity: 4, WarehouseQuantity: 0 },
    { Id: '2', Name: 'CÔNG TY TNHH TM & DV CÔNG NGHIỆP AN PHÚ', Total: 91, SupplierQuantity: 7, ContractQuantity: 13, PayQuantity: 45, ProductionQuantity: 0, TranportQuantity: 2, CustomsQuantity: 23, WarehouseQuantity: 1 },
    { Id: '3', Name: 'CÔNG TY TNHH NHÔM ĐÔNG Á', Total: 13, SupplierQuantity: 2, ContractQuantity: 1, PayQuantity: 2, ProductionQuantity: 0, TranportQuantity: 0, CustomsQuantity: 8, WarehouseQuantity: 0 },
    { Id: '4', Name: 'CN CÔNG TY TNHH SMC CORPORATION (VIỆT NAM) TẠI HÀ NỘI', Total: 60, SupplierQuantity: 12, ContractQuantity: 7, PayQuantity: 22, ProductionQuantity: 0, TranportQuantity: 0, CustomsQuantity: 18, WarehouseQuantity: 1 },
    { Id: '5', Name: 'FLEXLINK ENGINEERING SDN. BHD', Total: 32, SupplierQuantity: 3, ContractQuantity: 0, PayQuantity: 9, ProductionQuantity: 1, TranportQuantity: 5, CustomsQuantity: 14, WarehouseQuantity: 0 },
  ];
  listReportProject: any[] = [
    { Id: '1', Name: 'S139.2001', Total: 30, SupplierQuantity: 2, ContractQuantity: 5, PayQuantity: 8, ProductionQuantity: 6, TranportQuantity: 5, CustomsQuantity: 4, WarehouseQuantity: 0 },
    { Id: '2', Name: 'T020.1906', Total: 91, SupplierQuantity: 7, ContractQuantity: 13, PayQuantity: 45, ProductionQuantity: 0, TranportQuantity: 2, CustomsQuantity: 23, WarehouseQuantity: 1 },
    { Id: '3', Name: 'T093.2010', Total: 13, SupplierQuantity: 2, ContractQuantity: 1, PayQuantity: 2, ProductionQuantity: 0, TranportQuantity: 0, CustomsQuantity: 8, WarehouseQuantity: 0 },
    { Id: '4', Name: 'T094.2010', Total: 60, SupplierQuantity: 12, ContractQuantity: 7, PayQuantity: 22, ProductionQuantity: 0, TranportQuantity: 0, CustomsQuantity: 18, WarehouseQuantity: 1 },
    { Id: '5', Name: 'T095.2010', Total: 32, SupplierQuantity: 3, ContractQuantity: 0, PayQuantity: 9, ProductionQuantity: 1, TranportQuantity: 5, CustomsQuantity: 14, WarehouseQuantity: 0 },
  ];
  dateFrom = null;
  dateTo = null;
  totalItems = 0;
  totalProcessed = 0;
  totalNoProcessed = 0;
  modalInfo = {
    Title: 'Báo cáo vấn đề tồn đọng',
  };

  model: any = {
    ProjectCode: '',
    PRCode: '',
    TimeType: '5',
    Year: (new Date).getFullYear(),
    Month: 1,
    Quarter: 1,
    EmployeeId: '',
    SupplierId: '',
    Status: 2,
  }

  modelData: any = {
    totalItem: 0,
    totalProcessed: 0,
    totalNoProcessed: 0,
    supplierQuantity: 0,
    contractQuantity: 0,
    payQuantity: 0,
    productionQuantity: 0,
    tranportQuantity: 0,
    customsQuantity: 0,
    warehouseQuantity: 0,
    listEmployee: [],
    listSupplier: [],
    listReportProjectCode: [],
    listResult: []
  }

  columnSupplier: any[] = [{ Name: 'Code', Title: 'Mã nhà cung cấp' }, { Name: 'Name', Title: 'Tên nhà cung cấp' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];

  public barChartOptions: ChartOptions = {
    responsive: true,
    scales: {
      yAxes: [
        {
          ticks: {
            callback: function (label, index, labels) {
              return label.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            },
            precision: 0,
          },
          scaleLabel: {
            display: true,
            //labelString: '1k = 100.000'
          }
        }
      ]
    },
    tooltips: {
      //enabled: true,
      mode: 'single',
      callbacks: {
        label: function (tooltipItem, data) {
          var datasetLabel = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
          return data.datasets[tooltipItem.datasetIndex].label + ': ' + datasetLabel;
        }
      }
    }
  };
  public barChartLabels: Label[] = ['Xác định NCC', 'Hợp đồng', 'Thanh toán', 'Sản xuất', 'Vận chuyển', 'Hải quan', 'Nhập kho'];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartData: ChartDataSets[] = [
    { data: [], label: 'Số lượng vấn đề tồn đọng' }
  ];

  public doughnutChartData: number[] = [];
  public doughnutChartType: ChartType = 'doughnut';
  public pieChartType: ChartType = 'pie';
  public pieChartColors = [
    {
      backgroundColor: ['#ff9933', '#007bff', '#ff99cc', '#26c6da', '#cc99ff', '#66bb6a', '#737373'],
    },
  ];

  ngOnInit(): void {
    this.appSetting.PageTitle = "Báo cáo vấn đề tồn đọng";
    this.getListEmployee();
    this.getListSupplier();
    this.loadYear();
    this.loadMonth();
    this.searchData();
    this.resize();
    if (this.id) {
      this.modalInfo.Title = 'Cập nhật báo cáo vấn đề tồn đọng';
    } else {
      this.modalInfo.Title = 'Thêm mới báo cáo vấn đề tồn đọng';
    }
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

  getListEmployee() {
    this.comboboxService.getCbbEmployee().subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListSupplier() {
    this.comboboxService.getCBBSupplier().subscribe(
      data => {
        this.listSupplier = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  loadMonth() {
    for (var month = 1; month < 13; month++)
      this.listMonth.push(month);
  }

  loadYear() {
    for (var year = 2017; year <= new Date().getFullYear(); year++) {
      this.listYear.push(year);
    }
  }

  searchData() {
    this.model.DateTo = null;
    if (this.dateTo) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.dateTo);
    }
    this.model.DateFrom = null;
    if (this.dateFrom) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.dateFrom);
    }

    this.service.getListImportProfileProblemExist(this.model).subscribe((data: any) => {
      if (data) {
        this.listProblem = data.ListResult;
        this.totalItems = data.TotalItem;
        this.totalProcessed = data.Status1;
        this.totalNoProcessed = data.Status2;
        this.modelData = data;
        this.barChartData[0].data = [
          this.modelData.supplierQuantity,
          this.modelData.contractQuantity,
          this.modelData.payQuantity,
          this.modelData.productionQuantity,
          this.modelData.tranportQuantity,
          this.modelData.customsQuantity,
          this.modelData.warehouseQuantity
        ];
        this.doughnutChartData = [
          this.modelData.supplierQuantity,
          this.modelData.contractQuantity,
          this.modelData.payQuantity,
          this.modelData.productionQuantity,
          this.modelData.tranportQuantity,
          this.modelData.customsQuantity,
          this.modelData.warehouseQuantity
        ];
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  showConfirmDelete(index: number) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vấn đề này không?").then(
      data => {
        this.listProblem.splice(index, 1);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  clear() {
    this.model = {
      ProjectCode: '',
      PRCode: '',
      TimeType: '5',
      Year: (new Date).getFullYear(),
      Month: 1,
      Quarter: 1,
      EmployeeId: '',
      SupplierId: '',
      Status: null,
    };
    this.searchData();
  }

  exportExcel() {
    this.service.excel(this.modelData).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
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
