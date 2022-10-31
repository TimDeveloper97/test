import { AfterViewInit, Component, ElementRef, Input, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SplitComponent, SplitAreaDirective } from 'angular-split';
import { DxTreeListComponent } from 'devextreme-angular';
import * as moment from 'moment';
import { PerfectScrollbarComponent } from 'ngx-perfect-scrollbar';
import { ComboboxService, MessageService, Constants, AppSetting, ComponentService, DateUtils, Configuration } from 'src/app/shared';
import { RiskProblemProjectComponent } from '../../risk-problem-project/risk-problem-project.component';
import { PlanService } from '../../service/plan.service';
import { ProjectProductService } from '../../service/project-product.service';
import { ScheduleProjectService } from '../../service/schedule-project.service';
import { ChooseStageModalComponent } from '../choose-stage-modal/choose-stage-modal.component';
import { CreateTimePerformComponent } from '../create-time-perform/create-time-perform.component';
import { DashboardPlanComponent } from '../dashboard-plan/dashboard-plan.component';
import { OTPlanTimeComponent } from '../otplan-time/otplan-time.component';
import { OverallProjectComponent } from '../overall-project/overall-project.component';
import { PlanAdjustmentComponent } from '../plan-adjustment/plan-adjustment.component';
import { PlanCopyComponent } from '../plan-copy/plan-copy.component';
import { PlanEmployeeComponent } from '../plan-employee/plan-employee.component';
import { PlanHistoryComponent } from '../plan-history/plan-history.component';
import { PlanProjectCreateComponent } from '../plan-project-create/plan-project-create.component';
import { PopupSearchComponent } from '../popup-search/popup-search.component';
import { WorkingReportComponent } from '../working-report/working-report.component';

declare var $: any;

@Component({
  selector: 'app-plan-history-view',
  templateUrl: './plan-history-view.component.html',
  styleUrls: ['./plan-history-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PlanHistoryViewComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild('split', { static: false }) split: SplitComponent;
  @ViewChild('area1', { static: false }) area1: SplitAreaDirective;
  @ViewChild('area2', { static: false }) area2: SplitAreaDirective;
  @ViewChild(DxTreeListComponent) treeView;
  @ViewChild('scrollScheduleDate', { static: false }) scrollScheduleDate: ElementRef;
  @ViewChild('scrollScheduleDateHeader', { static: false }) scrollScheduleDateHeader: ElementRef;
  @ViewChild('scrollScheduleDateContent', { static: false }) perfectScroll: PerfectScrollbarComponent;
  @ViewChild('scrollTree', { static: false }) scrollTree: ElementRef;

  constructor(
    private combobox: ComboboxService,
    private messageService: MessageService,
    public constant: Constants,
    private router: Router,
    private scheduleProjectService: ScheduleProjectService,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private service: PlanService,
    private routeA: ActivatedRoute,
    private config: Configuration,
    private componentService: ComponentService,
    private comboboxService: ComboboxService,
    private projectProductService: ProjectProductService,
    private dateUtils: DateUtils) {
    this.items = [
      // { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      // { Id: 2, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      // { Id: 3, text: 'Xóa công đoạn', icon: 'fas fa-times text-danger' },
      // { Id: 8, text: 'Xóa kế hoạch', icon: 'fas fa-times text-danger' },
      // // { Id: 5, text: 'Log time', icon: 'fas fa-stopwatch text-success' },
      // { Id: 6, text: 'Copy', icon: 'far fa-copy text-danger' },
      // { Id: 7, text: 'Paste', icon: 'fas fa-paste text-success' },
      { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows text-warning'},
    ];
  }

  itemsProduct: any[] = [
    // { Id: 6, text: 'Copy', icon: 'far fa-copy text-danger' },
    // { Id: 7, text: 'Paste', icon: 'fas fa-paste text-success', disabled: true },
    { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows text-warning'},
  ];

  itemsStage: any[] = [
    // { Id: 3, text: 'Xóa công đoạn', icon: 'fas fa-times text-danger' },
    // { Id: 10, text: 'Copy công đoạn', icon: 'far fa-copy text-danger' },
    // { Id: 6, text: 'Copy', icon: 'far fa-copy text-danger' },
    // { Id: 7, text: 'Paste', icon: 'fas fa-paste text-success', disabled: true },
    { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows text-warning'},
  ];

  itemsPlan = [
    // { Id: 9, text: 'Sửa kế hoạch', icon: 'fa fa-edit text-warning' },
    // { Id: 8, text: 'Xóa kế hoạch', icon: 'fas fa-times text-danger' },
    // { Id: 6, text: 'Copy', icon: 'far fa-copy text-danger' },
    { Id: 4, text: 'Thu gọn/Mở rộng', icon: '' },
  ];

  Type = [
    { Id: 1, Name: "Theo kế hoạch", Code: "KH" },
    { Id: 2, Name: "Phát sinh - tính phí (PC)", Code: "PC" },
    { Id: 3, Name: "Phát sinh không tính phí", Code: "PS" },
  ]

  StatusPlan = [
    { Id: 1, Name: "Chậm tiến độ" },
  ]

  direction: string = 'horizontal'
  
  listProject: any = [];

  ListStageOfProject: any[] = [];
  DoneRatioOfProject: string;
  ItemSearch: any[] = [];
  modelSearchProject: any = {
    ProjectId: this.routeA.snapshot.paramMap.get('id'),
    
  }

  workModel: any = {
    Id: '',
    ProjectId: null,
    ProjectProductId: null,
    ParentId: null,
    StageName: '',
    PlanName: '',
    BackgroundColor: '',
    StageId: null,
    ContractStartDate: null,
    ContractDueDate: null,
    PlanStartDate: null,
    PlanDueDate: null,
    Done: 0,
    Color: '',
    Weight: 0,
    IsPlan: false,
    EstimateTime: 0,
    Status: null,
    SupplierId: null,
    Type: null,
    Index: null,
    CreateDate: null
  };

  listPlan: any = [];
  totalDay: any;
  daysOfMonth: any = [];
  selectDateId = -1;
  dayOfWeek: any = [];
  listProjectProduct: any = [];
  listExplanedId: any = [];
  projectIdSelect: any;
  items: any;
  suppliers: any = [];
  isEdit: string = 'row';
  listUserId: any[] = [];


  sizes = {
    percent: {
      area1: 40,
      area2: 60,
    },
    pixel: {
      area1: 120,
      area2: '*',
      area3: 160,
    },
  }
  isSearchConditon: boolean;
  StatisticalProjectModel: any = {
    TotalProject: 0,
    TotalProjectDone: 0,
    TotalProjectDoing: 0
  };

  scheduleHeight = 100;
  listHoliday: any = [];
  mode = 'single';
  ListData: any[] = [];
  ListResult: any[] = [];
  IsCollapsed = false;
  id: string;
  version: number;
  createDate = null;

  ngOnInit() {
    this.id = this.routeA.snapshot.paramMap.get('Id');
    this.IsCollapsed = true;
    this.scheduleHeight = window.innerHeight - 145;
    this.modelSearchProject.Date = new Date();
    this.searchProject();
  }

  ngAfterViewInit() {
    if (this.scrollScheduleDate) {
      this.scrollScheduleDate.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollScheduleDateHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
      this.scrollScheduleDate.nativeElement.addEventListener('ps-scroll-y', (event: any) => {
        var a = this.treeView.instance.getScrollable();
        a.scrollTo({ left: a.scrollLeft(), top: event.target.scrollTop })
      }, true);
    }
  }

  onRowPrepared(event: any) {
    if (event.rowType == "data" && !event.data.IsPlan && event.data.StageId) {
      event.rowElement.style.backgroundColor = event.data.Color;
    } else if (event.rowType == "data" && !event.data.IsPlan && !event.data.StageId && !event.data.ParentId) {
      event.rowElement.style.backgroundColor = "#A5CDFF";
    } else if (event.rowType == "data" && !event.data.IsPlan && !event.data.StageId && event.data.ParentId) {
      event.rowElement.style.backgroundColor = "#FEFFDC";
    }
  }

  onContentReady(event) {
    let that: any = this;
    event.component.getScrollable().off("scroll");
    event.component.getScrollable().on("scroll", e => {
      this.perfectScroll.directiveRef.scrollToTop(this.treeView.instance.getScrollable().scrollTop());
    })
  }

  ngOnDestroy() {
    this.scrollScheduleDate.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollScheduleDate.nativeElement.removeEventListener('ps-scroll-y', null);
    this.treeView.instance.getScrollable().off("scroll");
  }

  searchProject() {   
    this.scheduleProjectService.getPlanHistoryInfo(this.id).subscribe((data: any) => {
      if (data) {
        this.daysOfMonth = data.Result.holidays;
        this.dayOfWeek = data.Result.dayOfWeek;
        this.listProjectProduct = data.Result.listResult;
        this.appSetting.PageTitle = "Kế hoạch dự án - " + data.ProjectName;
        this.version = data.Version;
        this.createDate = data.CreateDate;

        if (this.IsCollapsed) {
          this.collapsed();

          this.IsCollapsed = false;
        } else {
          this.listExplanedId.forEach(id => {
            this.updateExpanded(id, false);
          });
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  updateScrollDate() {
    var dateSelect = $("#" + this.selectDateId);

    if (dateSelect && dateSelect.length > 0) {
      this.perfectScroll.directiveRef.scrollToLeft((dateSelect[0].offsetParent.offsetLeft / 3) * 2);
    }
  }

  onRowExpanded(event) {
    this.updateExpanded(event.key, false);
  }

  updateExpanded(key: string, isAll: boolean) {
    for (let index = 0; index < this.listProjectProduct.length; index++) {
      if (this.listProjectProduct[index].Id == key) {
        this.listProjectProduct[index].IsExpanded = true;
        this.listProjectProduct[index].IsShow = true;
        index = this.listProjectProduct.length;
      }
    }

    this.updateExpandedChild(key, isAll);
  }

  updateExpandedChild(parentKey: string, isAll: boolean) {
    for (let index = 0; index < this.listProjectProduct.length; index++) {
      if (this.listProjectProduct[index].ParentId == parentKey) {
        this.listProjectProduct[index].IsShow = true;
        if (isAll) {
          this.listProjectProduct[index].IsExpanded = true;
        }

        if (this.listProjectProduct[index].IsExpanded) {
          this.updateExpandedChild(this.listProjectProduct[index].Id, isAll);
        }
      }
    }
  }

  onRowCollapsed(event) {
    this.updateCollapsed(event.key, false);
  }

  updateCollapsed(key: string, isAll: boolean) {
    for (let index = 0; index < this.listProjectProduct.length; index++) {
      if (this.listProjectProduct[index].Id == key) {
        this.listProjectProduct[index].IsExpanded = false;
        index = this.listProjectProduct.length;
      }
    }

    this.updateCollapsedChild(key, isAll);
  }

  updateCollapsedChild(parentKey: string, isAll: boolean) {
    for (let index = 0; index < this.listProjectProduct.length; index++) {
      if (this.listProjectProduct[index].ParentId == parentKey) {
        this.listProjectProduct[index].IsShow = false;
        if (isAll) {
          this.listProjectProduct[index].IsExpanded = false;
        }
        this.updateCollapsedChild(this.listProjectProduct[index].Id, isAll);
      }
    }
  }

  collapsed() {
    if (this.listExplanedId.length > 0) {
      this.listExplanedId = [];
      this.listProjectProduct.forEach(element => {
        if (!element.ParentId) {
          this.updateCollapsed(element.Id, true);
        }
      });
    } else {
      this.listExplanedId = [];
      this.listProjectProduct.forEach(element => {
        this.listExplanedId.push(element.Id);
        if (!element.ParentId) {
          this.updateExpanded(element.Id, true);
        }
      });
    }
  }

  exportExcel() {
    this.scheduleProjectService.exportExcel(this.modelSearchProject).subscribe(d => {
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

  exportExcelProjectSchedule() {
    this.scheduleProjectService.exportExcelProjectSchedule(this.modelSearchProject).subscribe(d => {
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

  exportSummaryExcel(){
    // let activeModal = this.modalService.open(ReportPlanByDateComponent, { container: 'body', windowClass: 'report-plan-by-date', backdrop: 'static' })
    // activeModal.componentInstance.projectId = this.idProject;
    // activeModal.result.then((result) => {
    //   if (result && result.length > 0) {

    //   }
    // }, (reason) => {
    // });
  }
  
  exportExcelProjectPlan() {
    this.scheduleProjectService.exportExcelProjectPlan(this.modelSearchProject).subscribe(d => {
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

  dataModel: any = {
    ScheduleProject: {},
    ListCopy: [],
    ListCopyStage: []
  }


  closeModal() {
    this.router.navigate(['du-an/quan-ly-du-an']);
  }

  orderByPipe(array: any, field: string, type: any) {
    if (type == 1) {
      let newarr = array.sort((a: any, b: any) => a[field] - b[field]);
      return newarr;
    } else {
      let newarr = array.sort((a: any, b: any) => b[field] - a[field]);
      return newarr;
    }
  }

}
