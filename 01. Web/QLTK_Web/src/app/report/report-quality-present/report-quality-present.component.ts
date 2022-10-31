import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportQualityPresentService } from '../service/report-quality-present.service';
import { ObjectDate } from 'src/app/shared/common/ObjectDate';

@Component({
  selector: 'app-report-quality-present',
  templateUrl: './report-quality-present.component.html',
  styleUrls: ['./report-quality-present.component.scss']
})
export class ReportQualityPresentComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    public constant: Constants,
    public dateUtils: DateUtils,
    public comboboxService: ComboboxService,
    public serviceReportEmployee: ReportQualityPresentService
  ) { }

  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo chất lượng hiện tại";
    this.errorGroup();
    this.getListProject();
    this.loadMonth();
    this.loadYear();
    this.initModel();
    this.errorRatio();
  }

  DateNow = new Date();
  listMonth: any[] = [];
  listYear: any[] = [];
  StartIndex = 1;
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    ProjectId: '',
    ModuleGroupId: '',
    DateFrom: '',
    DateTo: '',
    DateToV: null,
    DateFromV: null,
  }

  errorGroupModel: any = {
    ProjectId: '',
    ModuleGroupId: '',
    DateFrom: '',
    DateTo: '',
    DateToV: null,
    DateFromV: null,
  }

  errorRatioModel: any = {
    TimeType: '6',
    DateFromV: null,
    DateToV: null,
    DateFrom: null,
    DateTo: null,
    Year: null,
    Quarter: 1,
    Month: this.DateNow.getMonth() + 1,
  }

  searchOptions: any = {
    FieldContentName: 'ModuleCode',
    Placeholder: 'Tìm kiếm',
    Items: [
      {
        Placeholder: 'Chọn nhóm module',
        Name: 'Nhóm module',
        FieldName: 'ModuleGroupId',
        Type: 'dropdowntree',
        SelectMode: 'single',
        ParentId: 'ParentId',
        DataType: this.constant.SearchDataType.ModuleGroup,
        Columns: [{ Name: 'Code', Title: 'Mã nhóm module' }, { Name: 'Name', Title: 'Tên nhóm module' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ]
  };

  searchOptionsErrorGroup: any = {
    FieldContentName: 'ModuleCode',
    Placeholder: 'Tìm kiếm',
    Items: [
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ]
  };

  searchErrorRatio: any = {
    FieldContentName: '',
    Placeholder: 'Tìm kiếm',
    Items: [
      {
        Name: 'Thời gian',
        FieldName: 'TimeType',
        Placeholder: 'Nhóm',
        Type: 'select',
        Data: this.constant.SearchDebtTimeTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  public textViewChartErrorWithLineProduct: string = 'Biểu đồ thể hiện số lỗi, vấn đề theo dòng sản phẩm';
  public barListViewErrorWithLineProduct: string[];
  public barChartTypeErrorWithLineProduct: string = 'bar';
  public barChartLegendErrorWithLineProduct: boolean = true;
  public barChartOptionsErrorWithLineProduct: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    yAxes: [
      {
        ticks: {
          callback: function (label, index, labels) {
            return (label).toString().replace(/\B(?=(\d{0})+(?!\d))/g, ".");
          }
        },
        scaleLabel: {
          display: true,
          //labelString: '1k = 1000'
        }
      }
    ],
    tooltips: {
      //enabled: true,
      mode: 'single',
      callbacks: {
        label: function (tooltipItem, data) {
          var datasetLabel = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
          return data.datasets[tooltipItem.datasetIndex].label + ': ' + datasetLabel;
        }
      }
    },

  };
  public barChartDataErrorWithLineProduct: any[] = [
    { data: [], label: 'Tổng module' },
    { data: [], label: 'Lỗi' },
    { data: [], label: 'Vấn đề' }
  ];
  public chartColorsErrorWithLineProduct: any[] = [
    { backgroundColor: '#6495ed' },
    { backgroundColor: 'red' },
    { backgroundColor: 'yellow' }
  ];

  public textViewChartErrorGroup: string = 'Biểu đồ thể hiện số lỗi, vấn đề theo nhóm vấn đề';
  public barListViewErrorGroup: string[];
  public barChartTypeErrorGroup: string = 'bar';
  public barChartLegendErrorGroup: boolean = true;
  public barChartOptionsErrorGroup: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    yAxes: [
      {
        ticks: {
          callback: function (label, index, labels) {
            return (label).toString().replace(/\B(?=(\d{0})+(?!\d))/g, ".");
          }
        },
        scaleLabel: {
          display: true,
          //labelString: '1k = 1000'
        }
      }
    ],
    tooltips: {
      //enabled: true,
      mode: 'single',
      callbacks: {
        label: function (tooltipItem, data) {
          var datasetLabel = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
          return data.datasets[tooltipItem.datasetIndex].label + ': ' + datasetLabel;
        }
      }
    },

  };
  public barChartDataErrorGroup: any[] = [
    { data: [], label: 'Lỗi' },
    { data: [], label: 'Vấn đề' }
  ];
  public chartColorsErrorGroup: any[] = [
    { backgroundColor: 'red' },
    { backgroundColor: 'yellow' }
  ];

  public textViewChartErrorRatio: string = 'Biểu đồ thể hiện tỷ lệ lỗi, vấn đề';
  public barListViewErrorRatio: string[];
  public barChartTypeErrorRatio: string = 'line';
  public barChartLegendErrorRatio: boolean = true;
  public barChartOptionsErrorRatio: any = {
    scaleShowVerticalLines: false,
    responsive: true,
    yAxes: [
      {
        ticks: {
          callback: function (label, index, labels) {
            return (label).toString().replace(/\B(?=(\d{0})+(?!\d))/g, ".");
          }
        },
        scaleLabel: {
          display: true,
          //labelString: '1k = 1000'
        }
      }
    ],
    tooltips: {
      //enabled: true,
      mode: 'single',
      callbacks: {
        label: function (tooltipItem, data) {
          var datasetLabel = data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index].toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
          return data.datasets[tooltipItem.datasetIndex].label + ': ' + datasetLabel + " %";
        }
      }
    },

  };
  public barChartDataErrorRatio: any[] = [
    { data: [], label: 'Lỗi' },
    { data: [], label: 'Vấn đề' }
  ];
  public chartColorsErrorRatio: any[] = [
    { backgroundColor: 'red' },
    { backgroundColor: 'yellow' }
  ];

  errorWithLineProduct() {
    if (this.model.DateFromV != null) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    } else {
      this.model.DateFrom = null
    }

    if (this.model.DateToV != null) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    } else {
      this.model.DateTo = null;
    }

    this.serviceReportEmployee.errorWithLineProduct(this.model).subscribe((data: any) => {
      if (data) {
        this.barChartDataErrorWithLineProduct = [
          {
            data: data.listTotalModule,
            label: 'Tổng module',
            //yAxisID: 'yAxis1',
            borderColor: '#6495ed',
            fill: false,
          },
          {
            data: data.listError,
            label: 'Lỗi',
            //yAxisID: 'yAxis2',
            borderColor: 'red',
            fill: false,
          },
          {
            data: data.listProblem,
            label: 'Vấn đề',
            //yAxisID: 'yAxis2',
            borderColor: 'yellow',
            fill: false,
          }
        ];
        this.barListViewErrorWithLineProduct = data.listName;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  errorGroup() {
    if (this.errorGroupModel.DateFromV != null) {
      this.errorGroupModel.DateFrom = this.dateUtils.convertObjectToDate(this.errorGroupModel.DateFromV);
    } else {
      this.errorGroupModel.DateFrom = null
    }

    if (this.errorGroupModel.DateToV != null) {
      this.errorGroupModel.DateTo = this.dateUtils.convertObjectToDate(this.errorGroupModel.DateToV)
    } else {
      this.errorGroupModel.DateTo = null;
    }

    this.serviceReportEmployee.errorGroup(this.errorGroupModel).subscribe((data: any) => {
      if (data) {
        this.barChartDataErrorGroup = [
          {
            data: data.listError,
            label: 'Lỗi',
            //yAxisID: 'yAxis2',
            borderColor: 'red',
            fill: false,
          },
          {
            data: data.listProblem,
            label: 'Vấn đề',
            //yAxisID: 'yAxis2',
            borderColor: 'yellow',
            fill: false,
          }
        ];
        this.barListViewErrorGroup = data.listName;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  errorRatio() {
    if (this.errorRatioModel.DateFromV != null) {
      this.errorRatioModel.DateFrom = this.dateUtils.convertObjectToDate(this.errorRatioModel.DateFromV);
    } else {
      this.errorRatioModel.DateFrom = null
    }

    if (this.errorRatioModel.DateToV != null) {
      this.errorRatioModel.DateTo = this.dateUtils.convertObjectToDate(this.errorRatioModel.DateToV)
    } else {
      this.errorRatioModel.DateTo = null;
    }

    this.serviceReportEmployee.errorRatio(this.errorRatioModel).subscribe((data: any) => {
      if (data) {
        this.barChartDataErrorRatio = [
          {
            data: data.listError,
            label: 'Lỗi',
            //yAxisID: 'yAxis2',
            borderColor: 'red',
            fill: false,
            lineTension: 0,
          },
          {
            data: data.listProblem,
            label: 'Vấn đề',
            //yAxisID: 'yAxis2',
            borderColor: 'yellow',
            fill: false,
            lineTension: 0,
          }
        ];
        this.barListViewErrorRatio = data.listName;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  listProject = [];
  getListProject() {
    this.comboboxService.getListProject().subscribe(
      data => {
        this.listProject = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      ProjectId: '',
    }
  }

  clearErrorGroup() {
    this.errorGroupModel = {
      ProjectId: '',
      ModuleGroupId: '',
      DateFrom: '',
      DateTo: '',
      DateToV: null,
      DateFromV: null,
    }
  }

  clearErrorRadio() {
    this.errorRatioModel = {
      TimeType: '6',
      DateFromV: null,
      DateToV: null,
      DateFrom: null,
      DateTo: null,
      Year: null,
      Quarter: 1,
      Month: this.DateNow.getMonth() + 1,
    }
    this.loadMonth();
    this.loadYear();
    this.initModel();
    this.errorRatio();
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

  createObjectDate(year: number, month: number, day: number) {
    var objectDate = new ObjectDate();
    objectDate.day = day;
    objectDate.month = month;
    objectDate.year = year;
    return objectDate;
  }

  initModel() {
    this.errorRatioModel.Year = new Date().getFullYear();
    this.errorRatioModel.DateFromV = new ObjectDate();

    // Set ngày từ
    this.errorRatioModel.DateFromV = this.createObjectDate(parseInt(this.errorRatioModel.Year), 1, 1);

    // Set ngày đến
    this.errorRatioModel.DateToV = this.createObjectDate(parseInt(this.errorRatioModel.Year), 12, 31);
  }
}
