import { DatePipe } from '@angular/common';
import { Component, Directive, Input, OnChanges, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, PermissionService, Constants, AppSetting, ComboboxService, DateUtils, Configuration } from 'src/app/shared';
import { PlanService } from '../../service/plan.service';
import { ScheduleProjectService } from '../../service/schedule-project.service';
import Gantt from "devextreme/ui/gantt";
import { GanttComponent, VirtualScroll  } from '@syncfusion/ej2-angular-gantt';

declare var $: any;

@Component({
  selector: 'app-gantt-chart',
  templateUrl: './gantt-chart.component.html',
  styleUrls: ['./gantt-chart.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class GanttChartComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private permissionService: PermissionService,
    public constant: Constants,
    private router: Router,
    private scheduleProjectService: ScheduleProjectService,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private service: PlanService,
    private routeA: ActivatedRoute,
    private config: Configuration,
    private comboboxService: ComboboxService,
    private dateUtils: DateUtils,
    public datepipe: DatePipe
  ) { }

  @ViewChild('gantt') public ganttObj: GanttComponent;
  startDateRange: Date;
  endDateRange: Date;

  projectId: string;
  height: string;
  listData: any[] = [];
  listHoliday: any[] = [];
  dateTime = new Date();

  dateFrom = null;
  dateTo = null;
  searchModel: any = {
    projectId: '',
    dateFrom: null,
    dateTo: null,
    NameProduct: '',
    SupplierId:'',
    PlanStatus:0,
    WorkStatus:0,
    StageId:'',
    Operator :0,
    Percen :''
  }
  PlanStatus = [
    { Id: 1, Name: "OK", Code: "KH" },
    { Id: 2, Name: "Quá hạn HĐ", Code: "PC" },
    { Id: 3, Name: "Quá hạn HT", Code: "PS" },
    { Id: 4, Name: "Trống bắt đầu HĐ", Code: "PS" },
    { Id: 5, Name: "Trống kết thúc HĐ", Code: "PS" },
    { Id: 6, Name: "Thiếu ngày TK", Code: "PS" },
  ]
  WorkStatus = [
    { Id: 1, Name: 'Open', BadgeClass: 'badge-danger', TextClass: '' },
    { Id: 2, Name: 'On Going', BadgeClass: 'badge-yellow', TextClass: '' },
    { Id: 3, Name: 'Close', BadgeClass: 'badge-green', TextClass: '' },
    { Id: 4, Name: 'Stop', BadgeClass: 'badge-secondary', TextClass: '' },
    { Id: 5, Name: 'Cancel', BadgeClass: 'badge-secondary', TextClass: '' },
  ]

  searchOptions: any = {
    FieldContentName: 'NameProduct',
    Placeholder: 'Tìm kiếm theo tên sản phẩm ...',
    Items: [
      {
        Name: 'Nhà thầu',
        FieldName: 'SupplierId',
        Placeholder: 'Nhà thầu',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.SupplierInProject,
        DisplayName: 'Name',
        ValueName: 'Id',
        Columns: [{ Name: 'Code', Title: 'Mã nhà thầu' }, { Name: 'Name', Title: 'Tên nhà thầu' }],
      },
      {
        Name: 'Tình trạng công việc',
        FieldName: 'WorkStatus',
        Placeholder: 'Tình trạng sử dụng',
        Type: 'select',
        Data: this.WorkStatus,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Công đoạn',
        FieldName: 'StageId',
        Placeholder: 'công đoạn',
        Type: 'select',
        DataType: this.constant.SearchDataType.Stage,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Placeholder: '% HT',
        Name: 'Tiến độ',
        FieldName: 'Percen',
        FieldNameType: 'Operator',
        Type: 'number',
      },
      {
        Name: 'Tình trạng công việc nội bộ',
        FieldName: 'PlanStatus',
        Placeholder: 'tình trạng công việc nội  bộ',
        Type: 'select',
        Data: this.PlanStatus,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 215 + "px";
    this.projectId = this.routeA.snapshot.paramMap.get('Id');

    this.ganttChart();
   // this.getListHoliday();
  }
  search() {
    if (this.dateFrom) {
      this.searchModel.dateFrom = this.dateUtils.convertObjectToDate(this.dateFrom);
    } else {
      this.searchModel.dateFrom = null;
    }

    if (this.dateTo) {
      this.searchModel.dateTo = this.dateUtils.convertObjectToDate(this.dateTo);
    } else {
      this.searchModel.dateTo = null;
    }

    this.ganttChart();
  }

  ganttChart() {
    this.searchModel.projectId = this.projectId;

    this.scheduleProjectService.ganttChart(this.searchModel).subscribe((data: any) => {
      if (data) {
        if (data.DateFrom) {
          this.startDateRange = data.DateFrom;
          this.dateFrom = this.dateUtils.convertDateToObject(data.DateFrom);
        }

        if (data.DateTo) {
          this.endDateRange = data.DateTo;
          this.dateTo = this.dateUtils.convertDateToObject(data.DateTo);
        }

        this.listData = data.Schedules;
        this.eventMarkers = [
          {
            day: new Date(),
          }
        ];
        this.selectionSettings = {
          mode: 'Row',
          type: 'Single',
          enableToggle: false
        };

        this.ganttObj.setProperties({ height: this.height });
        console.log(this.ganttObj);
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getListHoliday() {
    this.scheduleProjectService.ganttChartHoliday().subscribe((data: any) => {
      if (data) {
        this.listHoliday = data;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  //-- -- --------------------------------------------------------
  dayNames = ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7']
  workWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
  gridLines = 'Both';
  eventMarkers: object[];
  taskSettings = {
    id: 'Id',
    name: 'Name',
    startDate: 'PlanStartDate',
    endDate: 'PlanDueDate',
    progress: 'DoneRatio',
    child: 'ListChild',
    // resourceInfo: 'resources'
  };
  splitterSettings = {
    columnIndex: 3
  };

  tooltipSettings: any = {
    showTooltip: true
  }

  timelineSettings = {
    showTooltip: true,
    timelineViewMode: 'Week',
    topTier: {
      unit: 'Week',
      format: 'dd/MM/yyyy',
      formatter: (date: Date) => {
        return this.datepipe.transform(date, 'dd/MM/yyyy');
      }
    },
    bottomTier: {
      unit: 'Day',
      format: 'dd',
      // formatter: (date: Date) => {
      //   return this.datepipe.transform(date, 'dd/MM/yyyy');
      // }
    }
  };

  selectionSettings: any = {};

  queryTaskbarInfo(event: any) {
    event.taskbarBgColor = event.data.taskData.Color;
    event.progressBarBgColor = 'green';
  }

  rowDataBound(event: any) {
    if (!event.data.taskData.IsPlan) {
      event.row.style.background = event.data.taskData.Color;
    }
  }

  clear(){
    this.searchModel = {
      NameProduct: '',
      SupplierId:'',
      PlanStatus:0,
      WorkStatus:0,
      StageId:'',
      Operator :0,
      Percen :''
    }
    this.search()
  }
}