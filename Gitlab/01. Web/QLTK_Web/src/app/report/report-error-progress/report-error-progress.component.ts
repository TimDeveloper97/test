import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy, ViewEncapsulation } from '@angular/core';

import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label, Color } from 'ng2-charts';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';

import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportErrorProgressService } from '../service/report-error-progress.service';
import { ReportErrorWorkComponent } from '../report-error-work/report-error-work.component';
import { ReportErrorChangePlanComponent } from '../report-error-change-plan/report-error-change-plan.component';
@Component({
  selector: 'app-report-error-progress',
  templateUrl: './report-error-progress.component.html',
  styleUrls: ['./report-error-progress.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportErrorProgressComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public reportErrorProgressService: ReportErrorProgressService,
    private config: Configuration,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
  ) { }

  @ViewChild('scrollDepartment', { static: false }) scrollDepartment: ElementRef;
  @ViewChild('scrollDepartmentHeader', { static: false }) scrollDepartmentHeader: ElementRef;

  @ViewChild('scrollEmployee', { static: false }) scrollEmployee: ElementRef;
  @ViewChild('scrollEmployeeHeader', { static: false }) scrollEmployeeHeader: ElementRef;

  @ViewChild('scrollErrorChangedPlan', { static: false }) scrollErrorChangedPlan: ElementRef;
  @ViewChild('scrollErrorChangedPlanHeader', { static: false }) scrollErrorChangedPlanHeader: ElementRef;

  depMinWidth = 250;
  empMinWidth = 250;
  departmentSelectIndex = -1;
  employeeSelectIndex = -1;

  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo Vấn đề tồn đọng";
    this.report();
    this.reportErrorChangePlan();
  }

  ngAfterViewInit() {

    this.scrollDepartment.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollDepartmentHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollEmployee.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollEmployeeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollErrorChangedPlan.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollErrorChangedPlanHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollDepartment.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollEmployee.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollErrorChangedPlan.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  //
  days: any[] = [];
  //
  errorFixs: any[] = [];
  errorFixBys: any[] = [];
  errorChangePlans: any[] = [];

  weeks: any[] = [];

  public barChartData: ChartDataSets[] = [
    { data: [], label: 'Tỉ lệ % hoàn thành' }
  ];
  public barChartLabels: Label[] = [];
  public barChartPlugins = [pluginDataLabels];
  public barChartOptions: ChartOptions = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{ ticks: { stepSize: 10, max: 100, } }] },
    tooltips: {
      enabled: true,
      mode: 'single',
      callbacks: {
        label: (tooltipItem, data) => {
          var datasetLabel = this.errorFixs[tooltipItem.index].FinishTotal;
          return 'Số lượng hoàn thành' + ': ' + datasetLabel;
        },

      }
    },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
        formatter: (value, ctx) => {
          const label = this.errorFixs[ctx.dataIndex].FinishTotal;
          return label;
        },
        padding: {
          bottom: -30
        },
      }
    },
  };
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartColors: Color[] = [
    { backgroundColor: 'rgba(54, 162, 235, 0.8)' }
  ]

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'ModuleCode',
    OrderType: true,
    ModuleName: '',
    ModuleCode: '',
    ProjectId: '',
    CustomerId: '',
    CustomerFinalId: '',
    ErrorPlanStatus:''
  }

  searchOptions: any = {
    FieldContentName: 'ModuleCode',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên module',
        FieldName: 'ModuleName',
        Placeholder: 'Nhập tên module ...',
        Type: 'text'
      },
      {
        Placeholder: 'Chọn dự án',
        Name: 'Mã dự án',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Project,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn khách hàng',
        Name: 'Khách hàng thương mại',
        FieldName: 'CustomerId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn khách hàng',
        Name: 'Khách hàng cuối',
        FieldName: 'CustomerFinalId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn TT dự án',
        Name: 'Tình trạng dự án',
        FieldName: 'ProjectStatus',
        Type: 'select',
        DisplayName: 'Name',
        Data: this.constant.ProjectStatus,
        ValueName: 'Id'
      },
      {
        Placeholder: 'Chọn nhân viên',
        Name: 'Người khắc phục',
        FieldName: 'EmployeeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employees,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn bộ phận',
        Name: 'Dự án theo phòng kinh doanh',
        FieldName: 'DepartmentManageId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn bộ phận',
        Name: 'Bộ phận chịu trách nhiệm',
        FieldName: 'DepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn bộ phận',
        Name: 'Bộ phận khắc phục',
        FieldName: 'FixDepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Name: 'Tình trạng công việc',
        FieldName: 'ErrorPlanStatus',
        Placeholder: 'Tình trạng công việc',
        Type: 'select',
        Data: this.constant.ErrorPlanStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  report() {
    this.reportErrorProgressService.report(this.model).subscribe(
      data => {
        this.errorFixs = data.ErrorFixs;
        this.errorFixBys = data.ErrorFixBys;
        this.errorChangePlans = data.ErrorChangePlans;
        this.days = data.Days;
        this.weeks = data.Weeks;
        let barChartData: any[] = [];
        this.barChartLabels = [];
        data.ErrorFixs.forEach(element => {
          barChartData.push(element.FinishPercent);
          this.barChartLabels.push(element.Name);
        });

        this.barChartData[0].data = barChartData;
        this.empMinWidth = 660 + this.weeks.length * 60;
        this.depMinWidth = 660 + this.weeks.length * 60;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  reportErrorChangePlan() {
    this.reportErrorProgressService.reportErrorChangePlan(this.model).subscribe(
      data => {
        this.errorChangePlans = data;
        // console.log(data);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'ModuleCode',
      OrderType: true,
      ModuleName: '',
      ModuleCode: '',
      ProjectId: '',
      DateFrom: null,
      DateTo: null,
      ErrorPlanStatus:''
    }
    let now = new Date();
    // let dateFromV = { day: 1, month: 4, year: now.getFullYear() };
    // this.model.DateFromV = dateFromV;
    // let dateToV = { day: 31, month: 3, year: now.getFullYear() + 1 };
    // this.model.DateToV = dateToV;

    this.report();
  }

  exportExcel() {
    this.reportErrorProgressService.exportExcel(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showErrorWork(workType, objectId, object, objectType, isOpening) {
    let activeModal = this.modalService.open(ReportErrorWorkComponent, { container: 'body', windowClass: 'report-error-work-modal', backdrop: 'static' })
    activeModal.componentInstance.searchModel = Object.assign({}, this.model);
    if (objectType == 1) {
      activeModal.componentInstance.searchModel.DepartmentId = objectId;
    }
    else {
      activeModal.componentInstance.searchModel.EmployeeId = objectId;
    }

    activeModal.componentInstance.searchModel.DateFrom = object.DateFrom;
    activeModal.componentInstance.searchModel.DateTo = object.DateTo;
    activeModal.componentInstance.searchModel.WorkType = workType;
    activeModal.componentInstance.searchModel.IsOpening = isOpening;
    activeModal.componentInstance.isProgress = true;

    activeModal.result.then((result) => {
      if (result) {

      }
    }, (reason) => {
    });
  }

  showErrorChangePlan(departmentId, index){
    let activeModal = this.modalService.open(ReportErrorChangePlanComponent, { container: 'body', windowClass: 'report-error-change-plan-modal', backdrop: 'static' })
    activeModal.componentInstance.searchModel.index = index;
    activeModal.componentInstance.searchModel.departmentId= departmentId;

    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {
    });
  }

  selectDepartment(index) {
    if (this.departmentSelectIndex != index) {
      this.departmentSelectIndex = index
    }
  }

  selectEmployee(index) {
    if (this.employeeSelectIndex != index) {
      this.employeeSelectIndex = index
    }
  }
}
