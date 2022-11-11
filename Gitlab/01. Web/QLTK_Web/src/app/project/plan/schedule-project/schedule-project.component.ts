import { Component, OnInit, ViewChild, ViewEncapsulation, ElementRef, OnDestroy, HostListener, Input, AfterViewInit, Output, EventEmitter } from '@angular/core';

import {
  DxTreeListComponent
} from 'devextreme-angular';
import { PerfectScrollbarConfigInterface, PerfectScrollbarComponent } from 'ngx-perfect-scrollbar';

import { MessageService, Constants, AppSetting, ComboboxService, DateUtils, Configuration, ComponentService, PermissionService } from 'src/app/shared';
import { ScheduleProjectService } from '../../service/schedule-project.service';
import * as moment from 'moment';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanService } from '../../service/plan.service';
import { SplitComponent, SplitAreaDirective } from 'angular-split';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { CreateTimePerformComponent } from '../create-time-perform/create-time-perform.component';
import { ChooseStageModalComponent } from '../choose-stage-modal/choose-stage-modal.component';
import { WorkingReportComponent } from '../working-report/working-report.component'
import { OverallProjectComponent } from '../overall-project/overall-project.component'
import { DashboardPlanComponent } from '../dashboard-plan/dashboard-plan.component';
import { PopupSearchComponent } from '../popup-search/popup-search.component';
import { RiskProblemProjectComponent } from '../../risk-problem-project/risk-problem-project.component'
import { PlanAdjustmentComponent } from '../plan-adjustment/plan-adjustment.component';
import { PlanProjectCreateComponent } from '../plan-project-create/plan-project-create.component';
import { PlanEmployeeComponent } from '../plan-employee/plan-employee.component';
import { PlanCopyComponent } from '../plan-copy/plan-copy.component';
import { PlanHistoryComponent } from '../plan-history/plan-history.component';
import { ReportPlanByDateComponent } from '../report-plan-by-date/report-plan-by-date.component';
import { PlanCreateComponent } from '../plan-create/plan-create.component';
import { CreateUpdatePlanSaleTargetmentComponent } from '../create-update-plan-sale-targetment/create-update-plan-sale-targetment.component';
import { ProjectEmployeeService } from '../../service/project-employee.service';
import { element } from 'protractor';
import { ProjectPaymentService } from '../../service/project-payment.service';


declare var $: any;

@Component({
  selector: 'app-schedule-project',
  templateUrl: './schedule-project.component.html',
  styleUrls: ['./schedule-project.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ScheduleProjectComponent implements OnInit, OnDestroy, AfterViewInit {

  @ViewChild('split', { static: false }) split: SplitComponent;
  @ViewChild('area1', { static: false }) area1: SplitAreaDirective;
  @ViewChild('area2', { static: false }) area2: SplitAreaDirective;
  @ViewChild(DxTreeListComponent) treeView;
  @ViewChild('scrollScheduleDate', { static: false }) scrollScheduleDate: ElementRef;
  @ViewChild('scrollScheduleDateHeader', { static: false }) scrollScheduleDateHeader: ElementRef;
  @ViewChild('scrollScheduleDateContent', { static: false }) perfectScroll: PerfectScrollbarComponent;
  @ViewChild('scrollTree', { static: false }) scrollTree: ElementRef;

  constructor(
    private messageService: MessageService,
    private permissionService: PermissionService,
    public constant: Constants,
    private router: Router,
    private scheduleProjectService: ScheduleProjectService,
    public appSetting: AppSetting,
    private modalService: NgbModal,
    private service: PlanService,
    private projectEmployeeService: ProjectEmployeeService,
    private routeA: ActivatedRoute,
    private config: Configuration,
    private comboboxService: ComboboxService,
    private dateUtils: DateUtils,
    private projectPayment : ProjectPaymentService) {
    this.items = [
      { Id: 4, text: 'Thu gọn/Mở rộng', icon: '' },
    ];
  }

  itemsProduct: any[] = [
    { Id: 6, text: 'Copy công việc', icon: 'far fa-copy text-danger' },
    { Id: 7, text: 'Paste công việc', icon: 'fas fa-paste text-success', disabled: true },
    { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows-alt text-success' },
    { Id: 15, text: 'Paste công đoạn', icon: 'fas fa-paste text-success', disabled: true },
  ];

  itemsStage: any[] = [
    { Id: 3, text: 'Xóa công đoạn', icon: 'fas fa-times text-danger' },
    { Id: 10, text: 'Copy công đoạn', icon: 'far fa-copy text-danger' },
    { Id: 6, text: 'Copy công việc', icon: 'far fa-copy text-danger' },
    { Id: 7, text: 'Paste công việc', icon: 'fas fa-paste text-success', disabled: true },
    { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows-alt text-success' },
    { Id: 15, text: 'Paste công đoạn', icon: 'fas fa-paste text-success', disabled: true },
  ];

  itemsPlan = [
    { Id: 9, text: 'Sửa kế hoạch', icon: 'fa fa-edit text-warning' },
    { Id: 8, text: 'Xóa kế hoạch', icon: 'fas fa-times text-danger' },
    { Id: 6, text: 'Copy công việc', icon: 'far fa-copy text-danger' },
    { Id: 10, text: 'Copy công đoạn', icon: 'far fa-copy text-danger' },
    { Id: 11, text: 'Tạm dừng', icon: 'far fa-pause-circle text-info' },
    { Id: 14, text: 'Hủy', icon: 'fa fa-trash text-danger' },
    { Id: 12, text: 'Khôi phục', icon: 'far fa-play-circle text-success' },
    { Id: 13, text: 'Cập nhật tiến độ', icon: 'fa fa-percent text-warning' },
    { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows-alt text-success' },
    { Id: 15, text: 'Paste công đoạn', icon: 'fas fa-paste text-success', disabled: true },
    { Id: 16, text: 'Thêm kế hoạch thu tiền', icon: 'fas fa-dollar-sign text-success', disabled: false },
  ];

  Type = [
    { Id: 1, Name: "Theo kế hoạch", Code: "KH" },
    { Id: 2, Name: "Phát sinh - tính phí (PC)", Code: "PC" },
    { Id: 3, Name: "Phát sinh không tính phí", Code: "PS" },
  ]

  StatusPlan = [
    { Id: 1, Name: "Chậm tiến độ" },
  ]

  @Input() idProject;
  @Input() hide;
  @Input() Name;
  @Input() idStageSelect;
  @Input() IsChange;
  @Input() IsChangeDate;
  @Input() PlanId :string;
  @Output() change: EventEmitter<string> = new EventEmitter();


  direction: string = 'horizontal'

  ListStageOfProject: any[] = [];
  DoneRatioOfProject: string;
  ItemSearch: any[] = [];

  modelSearchProject: any = {
    ProjectId: this.routeA.snapshot.paramMap.get('id'),
    Date: '',
    SbuId: '',
    DepartmentId: '',
    EmployeeCode: '',
    DateToV: '',
    DateFromV: '',
    DateStartV: '',
    DateEndV: '',
    ModuleCode: '',
    ModuleContactCode: '',
    StartDateStatus: null,
    DoneRatio: null,
    Status: '',
    DoneType: 1,
    ContractStartDateToV: null,
    ContractStartDateFromV: null,
    ContractDueDateToV: null,
    ContractDueDateFromV: null,
    PlanStartDateToV: null,
    PlanStartDateFromV: null,
    PlanDueDateToV: null,
    PlanDueDateFromV: null,
    EmployeeId: '',
    WorkProgress: '',
    StageId: '',
    ImplementingAgenciesCode: '',
    WorkClassify: '',
    PlanStatus: '',
    WorkStatus: '',
    DateFrom: null,
    DateTo: null,
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
    DoneRatio: 0,
    Color: '',
    Weight: 1,
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
  ListCopyStage : any[] =[];

  currentUserId: '';

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

  ResponsiblePersionId: string;
  ResPersionId: string;

  scheduleHeight = 100;
  mode = 'single';
  ListData: any[] = [];
  ListResult: any[] = [];
  IsCollapsed = false;
  editorOptions: object;

  ngOnInit() {
    this.editorOptions = {
      itemTemplate: 'statusTemplate',
      width: 400
    };
    if(this.PlanId && typeof this.PlanId === 'string'){
      this.modelSearchProject.StageDelayId = this.PlanId;
    }

    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.currentUserId = currentUser.userid;
    }

    this.IsCollapsed = true;
    this.scheduleHeight = window.innerHeight - 200;
    this.modelSearchProject.Date = new Date();
    this.searchSchedules();
    this.getListSupplier();
    this.getCBBDepartment();
    this.getListTask();
  }

  listTask: any = [];

  listDepartment: any = [];

  getCBBDepartment() {
    this.comboboxService.getCbbDepartment().subscribe((data: any) => {
      if (data) {
        this.listDepartment = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getListTask() {
    this.scheduleProjectService.getListTask().subscribe(
      data => {
        this.listTask = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }


  getDisplayExpr(item) {
    if (!item) {
      return '';
    }

    return `${item.Alias} ${item.Name}`;
  }

  ngAfterViewInit() {
    // this.scrollScheduleDate.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
    //   this.scrollScheduleDateHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    // }, true);
    // this.scrollScheduleDate.nativeElement.addEventListener('ps-scroll-y', (event: any) => {
    //   var a = this.treeView.instance.getScrollable();
    //   a.scrollTo({ left: a.scrollLeft(), top: event.target.scrollTop })
    // }, true);
  }

  dataEditing: any;

  onEditingStart(e) {
    if (e.column.dataField === "PlanName" && !e.data.IsPlan) {
      e.cancel = true;
    } else if (e.column.dataField === "Type" && !e.data.IsPlan) {
      e.cancel = true;
    } else if (e.column.dataField === "SupplierId" && !e.data.IsPlan) {
      e.cancel = true;
    } else if (e.column.dataField === "EstimateTime" && !e.data.IsPlan) {
      e.cancel = true;
    } else if (e.column.dataField === "PlanStartDate" && !e.data.IsPlan) {
      e.cancel = true;
    } else if (e.column.dataField === "PlanDueDate" && !e.data.IsPlan) {
      e.cancel = true;
    } else if (e.column.dataField === "ContractStartDate" && !e.data.StageId) {
      e.cancel = true;
    } else if (e.column.dataField === "ContractDueDate" && !e.data.StageId) {
      e.cancel = true;
    } else {
      var planModel = Object.assign({}, this.workModel);
      planModel.Id = e.data.Id;
      planModel.ProjectId = e.data.ProjectId;
      planModel.ProjectProductId = e.data.ProjectProductId;
      planModel.ParentId = e.data.ParentId;
      planModel.StageName = e.data.StageName;
      planModel.PlanName = e.data.PlanName;
      planModel.BackgroundColor = e.data.BackgroundColor;
      planModel.StageId = e.data.StageId;
      planModel.ContractStartDate = e.data.ContractStartDate;
      planModel.ContractDueDate = e.data.ContractDueDate;
      planModel.PlanStartDate = e.data.PlanStartDate;
      planModel.PlanDueDate = e.data.PlanDueDate;
      planModel.DoneRatio = e.data.DoneRatio;
      planModel.Color = e.data.Color;
      planModel.Weight = e.data.Weight;
      planModel.IsPlan = e.data.IsPlan;
      planModel.EstimateTime = e.data.EstimateTime;
      planModel.Status = e.data.Status;
      planModel.SupplierId = e.data.SupplierId;
      planModel.Type = e.data.Type;
      planModel.Index = e.data.Index;
      planModel.CreateDate = e.data.CreateDate;
      this.dataEditing = planModel;
    }
    // if (e.column.dataField === "Type" && e.data.IsPlan) {
    //   window.setTimeout(function () {
    //     $('.dx-dropdownlist-popup-wrapper .dx-popup-content').css({ 'background-color': '#DDEEED', 'color': 'black' });
    //   }, 0);
    // }
  }



  onContentReady(event) {
    // let that: any = this;
    // event.component.getScrollable().off("scroll");
    // event.component.getScrollable().on("scroll", e => {
    //   this.perfectScroll.directiveRef.scrollToTop(this.treeView.instance.getScrollable().scrollTop());
    // })
  }

  ngOnDestroy() {
    // this.scrollScheduleDate.nativeElement.removeEventListener('ps-scroll-x', null);
    // this.scrollScheduleDate.nativeElement.removeEventListener('ps-scroll-y', null);
    // this.treeView.instance.getScrollable().off("scroll");
  }

  getListSupplier() {
    this.scheduleProjectService.getListSupplier().subscribe(
      data => {
        this.suppliers = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  listChild: any[] = [];
  listRemove: any[] = [];

  // Chọn công đoạn
  chooseStage(row: any, rowId: string) {
    let activeModal = this.modalService.open(ChooseStageModalComponent, { container: 'body', windowClass: 'choose-stage-modal', backdrop: 'static' })
    activeModal.componentInstance.Id = rowId;
    activeModal.componentInstance.ProjectId = this.modelSearchProject.ProjectId;
    activeModal.componentInstance.Data = row;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        this.listChild = this.listProjectProduct.filter(a => a.ProjectProductId === rowId && a.StageId);
        let indexParent = this.listProjectProduct.indexOf(row);
        if (this.listChild.length > 0) {
          this.listRemove = this.listChild;

          this.listRemove.forEach(element => {
            const index: number = this.listProjectProduct.indexOf(element);
            if (index !== -1) {
              this.listProjectProduct.splice(index, 1);
            }
          });

          result.forEach((element: any) => {
            this.listChild.push(element);
          });

          this.listChild = this.orderByPipe(this.listChild, 'Index', 2);
          this.listChild.forEach(element => {
            this.listProjectProduct.splice(indexParent, 0, element);
            indexParent = this.listProjectProduct.indexOf(element);
            //this.listProjectProduct.push(element);
          });

        } else {
          result.forEach(element => {
            this.listProjectProduct.splice(indexParent, 0, element);
            indexParent++;
            //this.listProjectProduct.push(element);
            this.updateExpandedChild(element.Id, true);
          });
        }

        this.recalculateParentDoneRatio(rowId);
        this.updateProjectProductDate(rowId);
      }
    }, (reason) => {
    });
  }

  /**
   * Lấy thông tin kế hoạch của dự án
   */
  searchSchedules() {
    this.modelSearchProject.ProjectId = this.idProject;
    if (this.modelSearchProject.DepartmentId || this.modelSearchProject.SbuId || this.modelSearchProject.ProjectId) {
      this.isSearchConditon = true;
    } else {
      this.isSearchConditon = false;
    }
    this.projectIdSelect = this.modelSearchProject.ProjectId;
    if (this.modelSearchProject.DateStartV != null) {
      this.modelSearchProject.DateStart = this.dateUtils.convertObjectToDate(this.modelSearchProject.DateStartV);
    }
    if (this.modelSearchProject.DateStartV == null) {
      this.modelSearchProject.DateStart = null;
    }
    if (this.modelSearchProject.DateEndV != null) {
      this.modelSearchProject.DateEnd = this.dateUtils.convertObjectToDate(this.modelSearchProject.DateEndV)
    }
    if (this.modelSearchProject.DateEndV == null) {
      this.modelSearchProject.DateEnd = null;
    }
    this.modelSearchProject.ContractStartDate = null;
    if (this.modelSearchProject.ContractStartDateToV) {
      this.modelSearchProject.ContractStartDateTo = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractStartDateToV);
    }

    this.modelSearchProject.ContractStartDate = null;
    if (this.modelSearchProject.ContractStartDateFromV) {
      this.modelSearchProject.ContractStartDateFrom = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractStartDateFromV);
    }

    this.modelSearchProject.ContractDueDate = null;
    if (this.modelSearchProject.ContractDueDateToV) {
      this.modelSearchProject.ContractDueDateTo = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractDueDateToV);
    }

    this.modelSearchProject.ContractDueDate = null;
    if (this.modelSearchProject.ContractDueDateFromV) {
      this.modelSearchProject.ContractDueDateFrom = this.dateUtils.convertObjectToDate(this.modelSearchProject.ContractDueDateFromV);
    }

    this.modelSearchProject.PlanStartDate = null;
    if (this.modelSearchProject.PlanStartDateToV) {
      this.modelSearchProject.PlanStartDateTo = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanStartDateToV);
    }

    this.modelSearchProject.PlanStartDate = null;
    if (this.modelSearchProject.PlanStartDateFromV) {
      this.modelSearchProject.PlanStartDateFrom = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanStartDateFromV);
    }

    this.modelSearchProject.PlanDueDate = null;
    if (this.modelSearchProject.PlanDueDateToV) {
      this.modelSearchProject.PlanDueDateTo = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanDueDateToV);
    }

    this.modelSearchProject.PlanDueDate = null;
    if (this.modelSearchProject.PlanDueDatefromV) {
      this.modelSearchProject.PlanDueDateFrom = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanDueDateFromV);
    }

    if (this.modelSearchProject.DateFromV) {
      this.modelSearchProject.DateFrom = this.dateUtils.convertObjectToDate(this.modelSearchProject.DateFromV);
    } else {
      this.modelSearchProject.DateFrom = null;
    }

    if (this.modelSearchProject.DateToV) {
      this.modelSearchProject.DateTo = this.dateUtils.convertObjectToDate(this.modelSearchProject.DateToV);
    } else {
      this.modelSearchProject.DateTo = null;
    }

    if (this.modelSearchProject.ProjectId != "" && this.modelSearchProject.ProjectId != null) {
      this.scheduleProjectService.getListPlanByProjectId(this.modelSearchProject).subscribe((data: any) => {
        if (data) {
          this.daysOfMonth = data.holidays;
          this.listProjectProduct = data.listResult;
          if(this.modelSearchProject.StageDelayId){
            this.change.emit(null);
          }
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
    } else {
      if (this.modelSearchProject.ProjectId == null) {
        this.listProjectProduct = [];
      }
    }
  }

  getListPlanByMonth() {
    if (this.modelSearchProject.DepartmentId || this.modelSearchProject.SbuId || this.modelSearchProject.ProjectId) {
      this.isSearchConditon = true;
    } else {
      this.isSearchConditon = false;
    }

    if (this.modelSearchProject.DateStartV != null) {
      this.modelSearchProject.DateStart = this.dateUtils.convertObjectToDate(this.modelSearchProject.DateStartV);
    }
    if (this.modelSearchProject.DateStartV == null) {
      this.modelSearchProject.DateStart = null;
    }
    if (this.modelSearchProject.DateEndV != null) {
      this.modelSearchProject.DateEnd = this.dateUtils.convertObjectToDate(this.modelSearchProject.DateEndV)
    }
    if (this.modelSearchProject.DateEndV == null) {
      this.modelSearchProject.DateEnd = null;
    }

    this.projectIdSelect = this.modelSearchProject.ProjectId;
    if (this.modelSearchProject.ProjectId != "" && this.modelSearchProject.ProjectId != null) {
      this.scheduleProjectService.getListPlanByMonth(this.modelSearchProject).subscribe((data: any) => {
        if (data) {
          this.daysOfMonth = data.holidays;
          this.dayOfWeek = data.dayOfWeek;

          setTimeout(() => {
            this.updateScrollDate();
          }, 300);
        }
      },
        error => {
          this.messageService.showError(error);
        });
    } else {
      if (this.modelSearchProject.ProjectId == null) {
        this.listProjectProduct = [];
      }
    }
  }

  onSelectionChanged(e) {
    this.selectDateId = e.selectedRowKeys[0];
    this.idSelected = e.selectedRowKeys[0];

    let planSelect;
    for (let index = 0; index < this.listProjectProduct.length; index++) {
      if (this.listProjectProduct[index].Id == this.idSelected) {
        planSelect = this.listProjectProduct[index]
        index = this.listProjectProduct.length;
      }
    }

    if (planSelect && planSelect.StartDate) {
      if (moment(planSelect.StartDate).month() != moment(this.modelSearchProject.DateStart).month() || moment(planSelect.StartDate) < moment(this.modelSearchProject.DateStart)) {
        // this.modelSearchProject.Date = ;
        this.modelSearchProject.DateStartV = this.dateUtils.getObjectDateByDate(moment(planSelect.StartDate).toDate());
        var dateEnd = moment(planSelect.StartDate).add(1, "month");
        this.modelSearchProject.DateEndV = { year: dateEnd.year(), month: dateEnd.month() + 1, day: dateEnd.date() };
        this.getListPlanByMonth();
      }
      else {
        this.updateScrollDate();
      }
    }

    if (e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.dataModel.ScheduleProject = e.selectedRowsData[0];

      if (!this.dataModel.ScheduleProject.StageId) {
        this.items = this.itemsProduct;
        if(!this.IsChange){
          this.items = [
            { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows-alt text-success' },
          ];
        }
      } else if (!this.dataModel.ScheduleProject.IsPlan) {
        this.items = this.itemsStage;
        if(!this.IsChange){
          this.items = [
            { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows-alt text-success' },
          ];
        }
      } else if (this.dataModel.ScheduleProject.IsPlan) {
        this.items = this.itemsPlan;
        if(!this.IsChange){
          this.items = [
            { Id: 4, text: 'Thu gọn/Mở rộng', icon: 'fa fa-arrows-alt text-success' },
            { Id: 13, text: 'Cập nhật tiến độ', icon: 'fa fa-percent text-warning' },
          ];
        }
      }
    }
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

  onSelectedPlan(plan) {
    this.selectDateId = plan.Id;
    this.treeView.instance.selectRows([plan.Id], false);

    if (plan.StartDate) {
      if (moment(plan.StartDate).month() != moment(this.modelSearchProject.Date).month()) {
        this.modelSearchProject.Date = moment(plan.StartDate);
        this.getListPlanByMonth();
      }
    }
  }


  showConfirmDeleteStage(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá Công đoạn này không?").then(
      data => {
        this.deleteStage(Id);
      },
      error => {

      }
    );
  }

  deleteStage(Id: string) {
    this.scheduleProjectService.deleteStage({ Id: Id }).subscribe(
      data => {

        // Xóa công việc khỏi danh sách
        this.listProjectProduct.forEach(element => {
          if (element.Id === Id) {
            const index: number = this.listProjectProduct.indexOf(element);
            if (index !== -1) {
              this.listProjectProduct.splice(index, 1);
              this.updateStageDate(element.ParentId);
              this.updateProjectProductDate(element.ParentId);
              this.recalculateParentDoneRatio(element.ProjectProductId);
            }
          }
        });
      },
      error => {
        this.messageService.showError(error);
      });
  }


  showConfirmDeletePlan(Id: string) {

    let plan = this.listProjectProduct.filter(a => a.Id === Id);
    if (plan !== null) {
      let userId = '';
      let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
      if (currentUser) {
        userId = currentUser.userid;
      }

      if (plan[0].MainUserId !== '' && plan[0].MainUserId !== null && userId !== plan[0].MainUserId) {
        this.messageService.showConfirm("Bạn không phải là người chịu trách nhiệm chính, bạn có muốn xóa?").then(
          data => {
            this.deletePlan(Id);
          },
          error => {

          }
        );
      } else {
        this.deletePlan(Id);
      }
    }
  }

  deletePlan(Id: string) {

    this.scheduleProjectService.deletePlan({ Id: Id }).subscribe(
      data => {
        // Xóa công việc khỏi danh sách
        this.listProjectProduct.forEach(element => {
          if (element.Id === Id) {
            const index: number = this.listProjectProduct.indexOf(element);
            if (index !== -1) {
              this.listProjectProduct.splice(index, 1);
              this.updateStageDate(element.ParentId);
              this.reCalculateDoneRatio(element.ParentId);
            }
          }
        });
        this.UpdatePlanPayment();
      },
      error => {
        this.messageService.showError(error);
      });
  }

  deleteMultiPlan() {
    let listCheck = [];
    this.listProjectProduct.forEach((element: any) => {
      if (element.Checked) {
        listCheck.push(element);
      }
    });

    let isExisted = false;

    if (listCheck.length > 0) {

      let userId = '';
      let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
      if (currentUser) {
        userId = currentUser.userid;
      }

      listCheck.forEach(element => {
        let plan = this.listProjectProduct.filter(a => a.Id === element.Id);
        if (plan[0].MainUserId !== '' && plan[0].MainUserId !== null && userId !== plan[0].MainUserId) {
          isExisted = true;
        }
      });

      if (isExisted) {
        this.messageService.showConfirm("Bạn không phải là người chịu trách nhiệm chính, bạn có muốn xóa?").then(
          data => {
            this.scheduleProjectService.deleteMultiPlan({ ListCopy: listCheck }).subscribe(
              data => {
                listCheck.forEach(item => {
                  // Xóa công việc khỏi danh sách
                  this.listProjectProduct.forEach(element => {
                    if (element.Id === item.Id) {
                      const index: number = this.listProjectProduct.indexOf(element);
                      if (index !== -1) {
                        this.listProjectProduct.splice(index, 1);
                        this.updateStageDate(element.ParentId);
                        this.reCalculateDoneRatio(element.ParentId);
                      }
                    }
                  });

                });

              },
              error => {
                this.messageService.showError(error);
              });
          },
          error => {
          }
        );
      } else {
        this.scheduleProjectService.deleteMultiPlan({ ListCopy: listCheck }).subscribe(
          data => {
            listCheck.forEach(item => {
              // Xóa công việc khỏi danh sách
              this.listProjectProduct.forEach(element => {
                if (element.Id === item.Id) {
                  const index: number = this.listProjectProduct.indexOf(element);
                  if (index !== -1) {
                    this.listProjectProduct.splice(index, 1);
                    this.updateStageDate(element.ParentId);
                    this.reCalculateDoneRatio(element.ParentId);
                  }
                }
              });

            });

          },
          error => {
            this.messageService.showError(error);
          });
      }
    } else if (this.dataModel.ScheduleProject.IsPlan) {
      this.showConfirmDeletePlan(this.idSelected);
    }
  }

  idSelected: string;

  itemClick(e) {
    if (e.itemData.Id == 4) {
      this.collapsed();
    } else {
      let projectProductSelected;
      if (e.itemData.Id == 3) {
        this.showConfirmDeleteStage(this.dataModel.ScheduleProject.Id);
      } else if (e.itemData.Id == 8) {
        this.deleteMultiPlan();
      }
      else if (e.itemData.Id == 9) {
        this.showCreateUpdatePlan(this.dataModel.ScheduleProject, 2);
      }
      else if (e.itemData.Id == 6) {
        this.copyPlan();
      }
      else if (e.itemData.Id == 7) {
        this.pastePlan();
      }
      else if (e.itemData.Id == 10) {
        this.copyStage();
      } else if (e.itemData.Id == 11) {
        this.Pending();
      } else if (e.itemData.Id == 12) {
        this.Resume();
      } else if (e.itemData.Id == 13) {
        this.UpdateProgress();
      } else if (e.itemData.Id == 14) {
        this.Cancel();
      }
      else if (e.itemData.Id == 15) {
        this.pasteStage();
      }
      else if (e.itemData.Id == 16) {
        this.addPlanSaleTargetment(this.dataModel.ScheduleProject.Id);
      }
      {
        if (!this.idSelected) {
          this.messageService.showMessage("Chưa chọn kế hoạch!")
        } else {
          this.listProjectProduct.forEach(element => {
            if (element.Id == this.idSelected) {
              projectProductSelected = element;
            }
          });
        }
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

  dragEnd(unit, { sizes }) {
    if (unit === 'percent') {
      this.sizes.percent.area1 = sizes[0];
      this.sizes.percent.area2 = sizes[1];
    }
    else if (unit === 'pixel') {
      this.sizes.pixel.area1 = sizes[0];
      this.sizes.pixel.area2 = sizes[1];
      this.sizes.pixel.area3 = sizes[2];
    }

    this.updateScrollDate();
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
    this.scheduleProjectService.searchOverallProject(this.idProject).subscribe(d => {
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

  /**
   * Sự kiện update thông tin khi nhập dữ liệu trên Row
   * @param event 
   * @returns 
   */
  onRowUpdated(event: any) {
    if (event.data) {
      let isValidDate = true;
      let startDate: any;
      let endDate: any;

      if (event.data.ContractStartDate) {
        startDate = moment(event.data.ContractStartDate).format('YYYY-MM-DD');
        event.data.ContractStartDate = moment(event.data.ContractStartDate).format('YYYY-MM-DD');
      }

      if (event.data.ContractDueDate) {
        endDate = moment(event.data.ContractDueDate).format('YYYY-MM-DD');
        event.data.ContractDueDate = moment(event.data.ContractDueDate).format('YYYY-MM-DD');
      }
      if (startDate && endDate) {
        if (startDate > endDate) {
          this.messageService.showMessage("Ngày bắt đầu hợp đồng phải nhỏ hơn ngày kết thúc hợp đồng!");
          event.data.ContractStartDate = this.dataEditing.ContractStartDate;
          event.data.ContractDueDate = this.dataEditing.ContractDueDate;
          isValidDate = false;
          return;
        }
      }

      startDate = null;
      endDate = null;
      if (event.data.PlanStartDate) {
        startDate = moment(event.data.PlanStartDate).format('YYYY-MM-DD');
        event.data.PlanStartDate = moment(event.data.PlanStartDate).format('YYYY-MM-DD');

      }
      if (event.data.PlanDueDate) {
        endDate = moment(event.data.PlanDueDate).format('YYYY-MM-DD');
        event.data.PlanDueDate = moment(event.data.PlanDueDate).format('YYYY-MM-DD');

      }
      if (startDate && endDate) {
        if (startDate > endDate) {
          this.messageService.showMessage("Ngày bắt đầu triển khai phải nhỏ hơn ngày kết thúc triển khai!");
          event.data.PlanStartDate = this.dataEditing.PlanStartDate;
          event.data.PlanDueDate = this.dataEditing.PlanDueDate;
          isValidDate = false;
          return;
        }
      }

      if (event.data.PlanStartDate && event.data.PlanDueDate) {
        let date1 = moment(event.data.PlanStartDate);
        let date2 = moment(event.data.PlanDueDate);
        let diffDay = date2.diff(date1, 'day');
        event.data.Duration = diffDay + 1;
      } else {
        event.data.Duration = 0;
      }

      // Cập nhật thông tin cha
      if (isValidDate &&
        event.data.PlanStartDate != this.dataEditing.PlanStartDate ||
        event.data.PlanDueDate != this.dataEditing.PlanDueDate ||
        event.data.ContractStartDate != this.dataEditing.ContractStartDate ||
        event.data.ContractDueDate != this.dataEditing.ContractDueDate) {

        let listParent = this.listProjectProduct.filter(a => a.Id === event.data.ParentId);
        let listChild = this.listProjectProduct.filter(a => a.ParentId === event.data.ParentId);

        // Công việc
        if (event.data.IsPlan) {
          if (listParent.length > 0) {
            listParent[0].PlanStartDate = this.minDate(listChild, "PlanStartDate");
            listParent[0].PlanDueDate = this.maxDate(listChild, "PlanDueDate");
            if (listParent[0].PlanStartDate && listParent[0].PlanDueDate) {
              let date1 = moment(listParent[0].PlanStartDate);
              let date2 = moment(listParent[0].PlanDueDate);
              let diffDay = date2.diff(date1, 'day');
              listParent[0].Duration = diffDay + 1;
            }

            this.updateStageDate(listParent[0].Id);
          }
        } else if (!event.data.IsPlan && event.data.StageId) // Công đoạn
        {
          if (event.data.PlanDueDate && event.data.ContractDueDate && event.data.PlanDueDate > event.data.ContractDueDate) {
            event.data.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
          } else {
            event.data.InternalStatus = "";
          }
          if (listParent.length > 0) {
            listParent[0].ContractStartDate = this.minDate(listChild, "ContractStartDate");
            listParent[0].ContractDueDate = this.maxDate(listChild, "ContractDueDate");

            this.updateProjectProductDate(listParent[0].ParentId);
          }
        }
      }

      // Trường hợp thay đổi giá trị của Trọng số (Weight)
      if (event.data.Weight != this.dataEditing.Weight) {

        this.reCalculateDoneRatio(event.data.ParentId);
      }

      if (isValidDate && (
        event.data.Weight != this.dataEditing.Weight ||
        event.data.PlanName != this.dataEditing.PlanName ||
        event.data.Type != this.dataEditing.Type ||
        event.data.ContractStartDate != this.dataEditing.ContractStartDate ||
        event.data.ContractDueDate != this.dataEditing.ContractDueDate ||
        event.data.PlanStartDate != this.dataEditing.PlanStartDate ||
        event.data.PlanDueDate != this.dataEditing.PlanDueDate ||
        event.data.EstimateTime != this.dataEditing.EstimateTime ||
        event.data.SupplierId != this.dataEditing.SupplierId)) {

        let listUpdate = [event.data];

        this.scheduleProjectService.modifyPlan(listUpdate).subscribe(
          data=>{
            this.UpdatePlanPayment();
          }
        );
      }

      this.updateInternalStatus(event.data);
    }
  }

  updateInternalStatus(plan: any) {
    var dateTime = new Date();
    dateTime.setDate(dateTime.getDate());

    var dateTime2 = moment(dateTime).format('YYYY-MM-DD');

    var children = this.listProjectProduct.filter(a => a.ParentId === plan.Id);

    var lessContractStartDate = children.filter(r => !r.IsPlan && r.ContractStartDate === null).length;
    var lessContractDueDate = children.filter(r => !r.IsPlan && r.ContractDueDate === null).length;
    var lessPlanDate = children.filter(r => r.PlanStartDate === null || r.PlanDueDate === null).length;

    if(!plan.IsPlan){
      if(plan.StageId === null){
        if(plan.Status == 3){
          plan.InternalStatus = "OK";
        }else if (lessContractStartDate > 0) {
          plan.InternalStatus = "TRỐNG BẮT ĐẦU HĐ";
        }else if (lessContractDueDate > 0) {
          plan.InternalStatus = "TRỐNG KẾT THÚC HĐ";
        }
        else if (lessPlanDate > 0) {
          plan.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
        }
        else if (dateTime2 > plan.PlanDueDate && plan.DoneRatio < 100) {
          plan.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
        }else if (plan.PlanDueDate > plan.ContractDueDate) {
          plan.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
        }
        else {
          plan.InternalStatus = "OK";
        }
      }else {
        if(plan.Status == 3){
          plan.InternalStatus = "OK";
        }else if (plan.ContractStartDate === null) {
          plan.InternalStatus = "TRỐNG BẮT ĐẦU HĐ";
        }else if (plan.ContractDueDate === null) {
          plan.InternalStatus = "TRỐNG KẾT THÚC HĐ";
        }
        else if (lessPlanDate > 0 || plan.PlanStartDate === null || plan.PlanDueDate === null) {
          plan.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
        }
        else if (dateTime2 > plan.PlanDueDate && plan.DoneRatio < 100) {
          plan.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
        }else if (plan.PlanDueDate > plan.ContractDueDate) {
          plan.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
        }
        else {
          plan.InternalStatus = "OK";
        }
      }
    }else {
      if(plan.Status == 3){
        plan.InternalStatus = "OK";
      }
      else if (!plan.PlanStartDate || !plan.PlanDueDate) {
        plan.InternalStatus = "THIẾU NGÀY TRIỂN KHAI";
      }
      else if (dateTime2 > plan.PlanDueDate && plan.DoneRatio < 100) {
        plan.InternalStatus = "QUÁ HẠN HOÀN THÀNH";
      }else if (plan.PlanDueDate > plan.ContractDueDate) {
        plan.InternalStatus = "QUÁ HẠN HỢP ĐỒNG";
      }
      else {
        plan.InternalStatus = "OK";
      }
    }

    let parent = this.listProjectProduct.filter(a => a.Id === plan.ParentId);
    if (parent.length > 0) {
      this.updateInternalStatus(parent[0])
    }
  }

  /**
   * Tính lại tỷ lệ hoàn thành, tình trạng của Công đoạn
   * @param parentId 
   */
  reCalculateDoneRatio(parentId: string) {
    let stage = this.listProjectProduct.filter(a => a.Id === parentId);

    if (stage.length > 0) {
      let plans = this.listProjectProduct.filter(a => a.ParentId === stage[0].Id);

      let DoneRation = 0;
      let Weight = 0;

      plans.forEach(element => {
        if (element.Status !== 4 && element.Status !== 5) {
          DoneRation = DoneRation + (element.DoneRatio * element.Weight);
          Weight = Weight + element.Weight;
        }
      })

      stage[0].DoneRatio = Weight === 0 ? 0 : Math.floor(DoneRation / Weight);

      this.listProjectProduct.forEach(element => {
        if (element.IsPlan == true && element.ListIdUserId.length > 0) {
          element.ListIdUserId.forEach(item => {
            if (this.listUserId.indexOf(item) == -1) {
              this.listUserId.push(item);
            }
          })
        }
      })

      let numOfPlan = plans.length;
      let numOfOpen = plans.filter(r => r.Status === 1).length;
      let numOfOngoing = plans.filter(r => r.Status === 2).length;
      let numOfClosed = plans.filter(r => r.Status === 3).length;
      let numOfStop = plans.filter(r => r.Status === 4).length;
      let numOfCancel = plans.filter(r => r.Status === 5).length;


      if (numOfPlan == 0) {
        stage[0].Status = 1;
      } else if (numOfOngoing > 0) {
        stage[0].Status = 2;
      } else if (numOfPlan == numOfOpen) {
        stage[0].Status = 1;
      } else if (numOfPlan == numOfClosed) {
        stage[0].Status = 3;
      } else if (numOfPlan == numOfStop) {
        stage[0].Status = 4;
      } else if (numOfPlan == numOfCancel) {
        stage[0].Status = 5;
      } else {
        if (numOfOpen > 0) {
          if (numOfClosed > 0) {
            stage[0].Status = 2;
          }
          else {
            stage[0].Status = 1;
          }
        } else {
          if (numOfClosed > 0) {
            stage[0].Status = 3;
          }
          else {
            if (numOfStop > 0) {
              stage[0].Status = 4;
            }
          }
        }
      }

      this.recalculateParentDoneRatio(stage[0].ProjectProductId)
    }

  }

  /**
   * Tính toán lại tỷ lệ % hoàn thành, tình trạng công việc của sản phẩm, module
   * @param projectProductId 
   */
  recalculateParentDoneRatio(projectProductId: string) {
    let product = this.listProjectProduct.filter(a => a.Id === projectProductId);

    if (product.length > 0) {
      let plans = this.listProjectProduct.filter(a => a.ProjectProductId === projectProductId && a.IsPlan === false);

      var numOfPlan = 0;
      var numOfOpen = 0;
      var numOfOngoing = 0;
      var numOfClosed = 0;
      var numOfStop = 0;
      var numOfCancel = 0;

      if (plans.length > 0) {

        let DoneRationPlan = 0;
        let WeightPlan = 0;
        plans.forEach(element => {
          if (element.Status !== 4 && element.Status !== 5) {
            DoneRationPlan = DoneRationPlan + (element.DoneRatio * element.Weight);
            WeightPlan = WeightPlan + element.Weight;
          }
        })

        // Do gán chung vào 1 model nên  lúc này lấy ra cả công đoạn và sản phẩm, do đó cần loại bỏ công đoạn để tính toán
        var products = this.listProjectProduct.filter(a => a.ParentId === projectProductId && a.StageId === null);
        let DoneRation = 0;
        let Weight = 0;
        products.forEach(element => {
          if (element.Status !== 4 && element.Status !== 5) {
            DoneRation = DoneRation + (element.DoneRatio * element.Weight);
            Weight = Weight + element.Weight;
          }
        })

        product[0].DoneRatio = WeightPlan === 0 && Weight === 0 ? 0 : Math.floor((DoneRationPlan + DoneRation) / (WeightPlan + Weight));

        numOfPlan = plans.length + products.length;
        numOfOpen = plans.filter(r => r.Status === 1).length + products.filter(r => r.Status === 1).length;
        numOfOngoing = plans.filter(r => r.Status === 2).length + products.filter(r => r.Status === 2).length;
        numOfClosed = plans.filter(r => r.Status === 3).length + products.filter(r => r.Status === 3).length;
        numOfStop = plans.filter(r => r.Status === 4).length + products.filter(r => r.Status === 4).length;
        numOfCancel = plans.filter(r => r.Status === 5).length + products.filter(r => r.Status === 5).length;

      } else {

        // Do gán chung vào 1 model nên  lúc này lấy ra cả công đoạn và sản phẩm, do đó cần loại bỏ công đoạn để tính toán
        var products = this.listProjectProduct.filter(a => a.ParentId === projectProductId && a.StageId === null);
        let DoneRation = 0;
        let Weight = 0;
        products.forEach(element => {
          if (element.Status !== 4 && element.Status !== 5) {
            DoneRation = DoneRation + (element.DoneRatio * element.Weight);
            Weight = Weight + element.Weight;
          }
        })

        product[0].DoneRatio = Weight === 0 ? 0 : Math.floor(DoneRation / Weight)

        numOfPlan = products.length;
        numOfOpen = products.filter(r => r.Status === 1).length;
        numOfOngoing = products.filter(r => r.Status === 2).length;
        numOfClosed = products.filter(r => r.Status === 3).length;
        numOfStop = products.filter(r => r.Status === 4).length;
        numOfCancel = products.filter(r => r.Status === 5).length;
      }

      if (numOfPlan == 0) {
        product[0].Status = 1;
      } else if (numOfOngoing > 0) {
        product[0].Status = 2;
      } else if (numOfPlan == numOfOpen) {
        product[0].Status = 1;
      } else if (numOfPlan == numOfClosed) {
        product[0].Status = 3;
      } else if (numOfPlan == numOfStop) {
        product[0].Status = 4;
      } else if (numOfPlan == numOfCancel) {
        product[0].Status = 5;
      } else {
        if (numOfOpen > 0) {
          if (numOfClosed > 0) {
            product[0].Status = 2;
          }
          else {
            product[0].Status = 1;
          }
        } else {
          if (numOfClosed > 0) {
            product[0].Status = 3;
          }
          else {
            if (numOfStop > 0) {
              product[0].Status = 4;
            }
          }
        }
      }

      if (product[0].ParentId !== null) {
        this.recalculateParentDoneRatio(product[0].ParentId);
      }
    }
  }

  updateStageDate(stageId: string) {
    let listParent = this.listProjectProduct.filter(a => a.Id === stageId);
    let listChild = this.listProjectProduct.filter(a => a.ParentId === stageId);
    if (listParent.length > 0) {
      listParent[0].PlanStartDate = listChild.length > 0 ? this.minDate(listChild, "PlanStartDate") : null;
      listParent[0].PlanDueDate = listChild.length > 0 ? this.maxDate(listChild, "PlanDueDate") : null;
      if (listParent[0].PlanStartDate && listParent[0].PlanDueDate) {
        let date1 = moment(listParent[0].PlanStartDate);
        let date2 = moment(listParent[0].PlanDueDate);
        let diffDay = date2.diff(date1, 'day');
        listParent[0].Duration = diffDay + 1;
      } else {
        listParent[0].Duration = 0;
      }

      this.updateProjectProductDate(listParent[0].ParentId);
    }
  }

  updateProjectProductDate(projectProductId: string) {
    let listParent = this.listProjectProduct.filter(a => a.Id === projectProductId);
    let listChild = this.listProjectProduct.filter(a => a.ParentId === projectProductId);

    if (listParent.length > 0) {
      listParent[0].PlanStartDate = listChild.length > 0 ? this.minDate(listChild, "PlanStartDate") : null;
      listParent[0].PlanDueDate = listChild.length > 0 ? this.maxDate(listChild, "PlanDueDate") : null;
      listParent[0].ContractStartDate = listChild.length > 0 ? this.minDate(listChild, "ContractStartDate") : null;
      listParent[0].ContractDueDate = listChild.length > 0 ? this.maxDate(listChild, "ContractDueDate") : null;

      if (listParent[0].PlanStartDate && listParent[0].PlanDueDate) {
        let date1 = moment(listParent[0].PlanStartDate);
        let date2 = moment(listParent[0].PlanDueDate);
        let diffDay = date2.diff(date1, 'day');
        listParent[0].Duration = diffDay + 1;
      } else {
        listParent[0].Duration = 0;
      }

      if (listParent[0].ParentId) {
        this.updateProjectProductDate(listParent[0].ParentId);
      }
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

  workingReport() {
    let activeModal = this.modalService.open(WorkingReportComponent, { container: 'body', windowClass: 'working-report-model', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.idProject;
    activeModal.result.then((result) => {
    }, (reason) => {
    });
  }

  unHideDashboard() {
    let activeModal = this.modalService.open(DashboardPlanComponent, { container: 'body', windowClass: 'dashboard-plan-model', backdrop: 'static' })
    activeModal.componentInstance.idProject = this.idProject;
    activeModal.componentInstance.Name = this.Name;
    activeModal.result.then((result) => {
    }, (reason) => {
    });
  }

  //lọc
  popupSearch() {
    let activeModal = this.modalService.open(PopupSearchComponent, { container: 'body', windowClass: 'popup-search-model', backdrop: 'static' })
    activeModal.componentInstance.idProject = this.idProject;
    activeModal.componentInstance.modelSearchProject = this.modelSearchProject;
    activeModal.componentInstance.ItemSearch = this.ItemSearch;


    this.listProjectProduct.forEach(element => {
      if (element.IsPlan == true && element.ListIdUserId.length > 0) {
        element.ListIdUserId.forEach(item => {
          if (this.listUserId.indexOf(item) == -1) {
            this.listUserId.push(item);
          }
        })
      }
    })



    activeModal.componentInstance.listUserId = this.listUserId;

    activeModal.result.then((result) => {
      if (result) {
        this.ItemSearch = activeModal.componentInstance.ItemSearch;

        this.searchSchedules();
      }
    }, (reason) => {
    });
  }

  overallProject() {
    let activeModal = this.modalService.open(OverallProjectComponent, { container: 'body', windowClass: 'overall-project-model', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.idProject;
    activeModal.result.then((result) => {
    }, (reason) => {
    });
  }

  riskProject() {
    let activeModal = this.modalService.open(RiskProblemProjectComponent, { container: 'body', windowClass: 'risk-problem-project-model', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.idProject;
    activeModal.result.then((result) => {
      if (result) {
        this.searchSchedules();
      }
    }, (reason) => {
    });
  }

  planAdjustment() {
    let activeModal = this.modalService.open(PlanAdjustmentComponent, { container: 'body', windowClass: 'plan-adjustment-model', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.idProject;
    activeModal.result.then((result) => {
    }, (reason) => {
    });
  }

  /**
   * Thực hiện tạo mới công việc. Kết quả trả về sẽ dùng để update lại giao diện danh sách công việc
   * @param row 
   * @param type 
   */
  showCreateUpdatePlan(row: any, type: number) {
    let activeModal = this.modalService.open(PlanProjectCreateComponent, { container: 'body', windowClass: 'plan-project-create-model', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.modelSearchProject.ProjectId;
    activeModal.componentInstance.projectProductId = row.ProjectProductId;
    activeModal.componentInstance.stageId = row.StageId;
    activeModal.componentInstance.parentId = row.Id;
    activeModal.componentInstance.Id = row.Id;
    activeModal.componentInstance.type = type;
    activeModal.componentInstance.tasks = this.listTask;
    activeModal.componentInstance.listDepartment = this.listDepartment;

    activeModal.result.then((result) => {
      if (result !== '') {

        // Lấy ra danh sách các công việc cùng công đoạn
        this.listChild = this.listProjectProduct.filter(a => a.StageId === row.StageId && a.IsPlan === true);

        // Kiểm tra tồn tại của công việc
        var existed = this.listChild.filter(a => a.Id === result.Id);
        var oldIndexPlan = row.IndexPlan;
        // Trường hợp thêm mới
        if (existed.length === 0) {
          this.listChild.push(result);
        } else {
          // Trường hợp update
          for (var i in this.listChild) {
            if (this.listChild[i].Id == result.Id) {
              this.listChild[i].PlanName = result.PlanName;
              this.listChild[i].IndexPlan = result.IndexPlan;
              this.listChild[i].Description = result.Description;
            } else {
              // Update lại thứ tự Order của các công việc
              // Trường hợp tăng Index
              if (result.IndexPlan > oldIndexPlan) {
                // Trường hợp 
                if (this.listChild[i].IndexPlan > oldIndexPlan && this.listChild[i].IndexPlan <= result.IndexPlan) {
                  this.listChild[i].IndexPlan--;
                }
              } else {
                if (this.listChild[i].IndexPlan < oldIndexPlan && this.listChild[i].IndexPlan >= result.IndexPlan) {
                  this.listChild[i].IndexPlan++;
                }
              }
            }
          }
        }

        if (this.listChild.length > 0) {
          this.listRemove = this.listChild;

          // Xóa khỏi danh sách công việc
          this.listRemove.forEach(element => {
            const index: number = this.listProjectProduct.indexOf(element);
            if (index !== -1) {
              this.listProjectProduct.splice(index, 1);
            }
          });
        }

        this.listChild = this.orderByPipe(this.listChild, 'IndexPlan', 1);

        this.listChild.forEach(element => {
          this.listProjectProduct.push(element);
        });

        this.reCalculateDoneRatio(result.ParentId);
      }
    },
      (reason) => {
      });
  }

  upDatePlan() {
    this.isEdit = "cell";
  }

  showChooseEmployee(row: any) {
    let activeModal = this.modalService.open(PlanEmployeeComponent, { container: 'body', windowClass: 'plan-show-employee-model', backdrop: 'static' })
    activeModal.componentInstance.planId = row.Id;
    activeModal.componentInstance.listIdUserId = row.ListIdUserId;
    activeModal.result.then((result) => {
      if (result) {
        this.scheduleProjectService.getPlanAdjustment(row.Id).subscribe((data: any) => {
          if (data) {
            row.ResponsiblePersionName = data.ResponsiblePersionName;
            row.ListIdUserId = data.ListIdUserId;
            row.MainUserId = data.MainUserId;
          }
        }, error => {
          this.messageService.showError(error);
        });
      }
    }, (reason) => {
    });
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

  copyPlan() {
    this.dataModel.ListCopy = [];

    this.listProjectProduct.forEach((element: any) => {
      if (element.Checked && element.IsPlan==true) {
        this.dataModel.ListCopy.push(element);
      }
    });

    if (this.dataModel.ListCopy.length > 0) {
      this.itemsStage[3].disabled = false;
    }
  }

  copyStage() {
    this.ListCopyStage=[];
    this.dataModel.ListCopyStage = [];
    // this.dataModel.ListCopyStage.push(this.dataModel.ScheduleProject);
    this.listProjectProduct.forEach((element: any) => {
      if (element.Checked ) {
        this.ListCopyStage.push(element);
      }
    });
    this.checkStages();
    if (this.ListCopyStage.length > 0) {
      this.itemsProduct[3].disabled = false;
    }
  }


  Pending() {

    // Gán tình trạng công việc là Stop
    this.listProjectProduct.forEach(element => {
      if (element.Id === this.idSelected && element.IsPlan === true) {
        element.Status = 4;

        this.reCalculateDoneRatio(element.ParentId);
      }
    });

    this.scheduleProjectService.pending(this.idSelected).subscribe((data: any) => {
      if (data) {

      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  Cancel() {

    // Gán tình trạng công việc là Stop
    this.listProjectProduct.forEach(element => {
      if (element.Id === this.idSelected && element.IsPlan === true) {
        element.Status = 5;

        this.reCalculateDoneRatio(element.ParentId);
      }
    });

    this.scheduleProjectService.cancel(this.idSelected).subscribe((data: any) => {
      if (data) {

      }
    }, error => {
      this.messageService.showError(error);
    });

  }

  Resume() {

    // Gán tình trạng công việc
    this.listProjectProduct.forEach(element => {
      if (element.Id === this.idSelected && element.IsPlan === true) {
        element.Status = element.DoneRatio == 100 ? 3 : (element.DoneRatio == 0 ? 1 : 2);
        this.reCalculateDoneRatio(element.ParentId);
      }
    });


    this.scheduleProjectService.resume(this.idSelected).subscribe((data: any) => {
      if (data) {

      }
    }, error => {
      this.messageService.showError(error);
    });
  }


  /**
   * Cập nhật tiến độ thực hiện công việc
   */
  UpdateProgress() {
    let activeModal = this.modalService.open(PlanCreateComponent, { container: 'body', windowClass: 'plan-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = this.idSelected;
    activeModal.componentInstance.Types = 1;
    activeModal.result.then((result) => {
      if (result) {
        // Gán tình trạng công việc
        this.listProjectProduct.forEach(element => {
          if (element.Id === this.idSelected && element.IsPlan === true) {
            element.DoneRatio = result.DoneRatio;
            element.Status = result.Status;
            this.reCalculateDoneRatio(element.ParentId);
          }
        });
      }
    }, (error) => {
      this.messageService.showError(error);
    });
  }

  pastePlan() {
    this.dataModel.ScheduleProject.ProjectId = this.modelSearchProject.ProjectId;
      this.scheduleProjectService.createPlanCopy(this.dataModel).subscribe(
        result => {
          if (result !== '') {
            result.forEach(element => {
              this.listProjectProduct.push(element);
              this.reCalculateDoneRatio(element.ParentId);
            });
          }

        }, error => {
          this.messageService.showError(error);
        }
      );
  }
  pasteStage() {
    this.dataModel.ScheduleProject.ProjectId = this.modelSearchProject.ProjectId;
    var listStage =[];
    this.dataModel.ListCopyStage =[];
      var dupplicateStage = [];
      this.listProjectProduct.filter(a => a.ParentId == this.dataModel.ScheduleProject.Id).forEach(element =>{
        this.ListCopyStage.filter(a => a.StageName!='').forEach(stage =>{
          if(element.StageName === stage.StageName){
            dupplicateStage.push(stage);
          }
        });
      });
      listStage =this.ListCopyStage.filter(a => a.StageName!='');
      if (dupplicateStage.length == listStage.length) {
        this.messageService.showMessage("Bạn chỉ có thể Copy công đoạn cho sản phẩm chưa có công đoạn này!");
        return;
      }
      if(dupplicateStage.length >0){
        this.dataModel.ListCopyStage = this.ListCopyStage.filter(a => !dupplicateStage.includes(a));
      }else{
        this.dataModel.ListCopyStage = this.ListCopyStage;
      }
      this.scheduleProjectService.createStageCopy(this.dataModel).subscribe(
        result => {
          if (result !== '') {
            var plans = this.listProjectProduct.filter(a => a.ProjectProductId == this.dataModel.ScheduleProject.Id);
            this.listProjectProduct = this.listProjectProduct.filter(item => !plans.includes(item));
            result.forEach(element => {
              this.listProjectProduct.push(element);
              this.updateStageDate(element.ParentId);
              this.updateProjectProductDate(element.ParentId);
              this.reCalculateDoneRatio(element.ParentId);
            });
            if(dupplicateStage.length >0){
              var stageName ='';
              dupplicateStage.forEach( s =>{
                stageName = stageName ==''? s.StageName :stageName +', '+s.StageName;
              })
                  this.messageService.showMessage("Công đoạn đã tồn tại : "+stageName);
            }
          }
        }, error => {
          this.messageService.showError(error);
        }
      );
  }

  closeModal() {
    this.router.navigate(['du-an/quan-ly-du-an']);
  }

  orderByPipe(array: any, field: string, type: any) {
    if (type == 1) // asc 
    {
      let newarr = array.sort((a: any, b: any) => a[field] - b[field]);
      return newarr;
    } else // dsc
    {
      let newarr = array.sort((a: any, b: any) => b[field] - a[field]);
      return newarr;
    }
  }

  maxDate(arrays: any, field: string) {
    let maxDueDate = arrays.sort((a: any, b: any) =>
      new Date(b[field]).getTime() - new Date(a[field]).getTime())[0][field];
    return maxDueDate;
  }

  minDate(arrays: any, field: string) {

    // Loại khỏi danh sách những phần tử không có ngày trước khi tính Min
    var temps: any[] = [];
    arrays.forEach(element => {
      if (element[field] !== null) {
        temps.push(element);
      }
    });

    if (temps.length > 0) {
      let maxDueDate = temps.sort((a: any, b: any) =>
        new Date(a[field]).getTime() - new Date(b[field]).getTime())[0][field];
      return maxDueDate;
    } else {
      return null;
    }
  }

  history() {
    let activeModal = this.modalService.open(PlanHistoryComponent, { container: 'body', windowClass: 'plan-history-modal', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.idProject;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {

      }
    }, (reason) => {
    });
  }

  ganttChart() {
   // this.router.navigate(['du-an/quan-ly-du-an/gantt-chart', this.idProject]);

   this.router.navigate(['du-an/quan-ly-du-an/gantt-chart', this.idProject]).then(result => {
    window.open();
 });

  }

  exportSummaryExcel() {
    let activeModal = this.modalService.open(ReportPlanByDateComponent, { container: 'body', windowClass: 'report-plan-by-date', backdrop: 'static' })
    activeModal.componentInstance.projectId = this.idProject;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {

      }
    }, (reason) => {
    });
  }

  checkStages(){
    var Stages =[];
    var Plans =[];
    var ListCopy=[];
    this.ListCopyStage.forEach(element =>{
      if(element.IsPlan ==true){
        Plans.push(element);
      }else{
        Stages.push(element);
      }
    });
    ListCopy =Stages;
    Stages.forEach(stage =>{
      Plans.forEach(plan =>{
        if(stage.Id == plan.ParentId){
          ListCopy.push(plan);
        }
      });
    });
    this.ListCopyStage =ListCopy;
  }
  setPlanOfStage(id :any){
    this.listProjectProduct.forEach((stage: any) => {
      if (stage.Id == id) {
        this.listProjectProduct.forEach((element: any) => {
          if (element.ParentId == id) {
            if(stage.Checked ==null){
              element.Checked = false;
            }else{
              element.Checked = stage.Checked;
            }
          }
        });
      }
    });
  }
  addPlanSaleTargetment(planId : string){
    let activeModal = this.modalService.open(CreateUpdatePlanSaleTargetmentComponent, { container: 'body', windowClass: 'create-update-plan-sale-targetment', backdrop: 'static' })
    activeModal.componentInstance.PlanId = planId;
    activeModal.componentInstance.ProjectId = this.modelSearchProject.ProjectId;

    activeModal.result.then((result) => {
      if (result && result.length > 0) {

      }
    }, (reason) => {
    });
  }
  UpdatePlanPayment(){
    this.projectPayment.UpdatePlanPaymentDate(this.idProject).subscribe(
      result => {
      }, error => {
        this.messageService.showError(error);
      }
    );
  }
}