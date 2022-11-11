import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy, ViewEncapsulation, HostListener } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TiviReportErrorService } from '../services/tivi-report-error.service';
import { ActivatedRoute } from '@angular/router';
import { timeStamp } from 'console';
import { Observable, interval, Subscription } from 'rxjs';
import { TiviReportTKService } from '../services/tivi-report-tk.service';
import { PerfectScrollbarComponent } from 'ngx-perfect-scrollbar';

@Component({
  selector: 'app-tivi-report-tk',
  templateUrl: './tivi-report-tk.component.html',
  styleUrls: ['./tivi-report-tk.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TiviReportTKComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public reportErrorService: TiviReportErrorService,
    private config: Configuration,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    private routeA: ActivatedRoute,
    private tiviReportTKService: TiviReportTKService,
  ) { }

  @ViewChild('scrollDepartmentManage', { static: false }) scrollDepartmentManage: ElementRef;
  @ViewChild('scrollDepartmentManageHeader', { static: false }) scrollDepartmentManageHeader: ElementRef;

  @ViewChild('scrollDepartment', { static: false }) scrollDepartment: ElementRef;
  @ViewChild('scrollDepartmentHeader', { static: false }) scrollDepartmentHeader: ElementRef;

  @ViewChild('scrollEmployee', { static: false }) scrollEmployee: ElementRef;
  @ViewChild('scrollEmployeeHeader', { static: false }) scrollEmployeeHeader: ElementRef;

  @ViewChild('scrollWorkingTime', { static: false }) scrollWorkingTime: ElementRef;
  @ViewChild('scrollWorkingTimeHeader', { static: false }) scrollWorkingTimeHeader: ElementRef;

  @ViewChild('scrollProject', { static: false }) scrollProject: ElementRef;
  @ViewChild('scrollProjectHeader', { static: false }) scrollProjectHeader: ElementRef;

  @ViewChild('workingTimeScroll') workingTimeScroll: PerfectScrollbarComponent;
  @ViewChild('projectScroll') projectScroll: PerfectScrollbarComponent;
  depMinWidth = 250;
  empMinWidth = 250;
  departmentSelectIndex = -1;
  employeeSelectIndex = -1;
  minHeight = 700;
  worktimeHeight = 700;
  projectHeight = 700;
  private updateSubscription: Subscription;
  private scrollSubscription: Subscription;

  errorFixs: any[] = [];
  errorFixBys: any[] = [];
  errorFixBys2: any[] = [];
  departments: any[] = [];
  errorProjects: any[] = [];
  stages: any[] = [];
  workTimes: any[] = [];
  days: any[] = [];
  tems: any[] = [];
  projectInPlans: any[] = [];
  activeTabId = 1;

  model: any = {
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
    CustomerId: '',
    CustomerFinalId: ''
  }

  workingTimeSearchModel: any = {
    DepartmentId: '',
    SBUId: '',
    NameCode: '',
    DateToV: this.dateUtils.getFiscalYearEnd(),
    DateFromV: this.dateUtils.getFiscalYearStart(),
    WorkType: ''
  }

  projectSearchModel: any = {
    ProjectId: '',
    TotalProject: '', //tổng số dự án đang triển khai
    TotalProjectFinish: '', // Dự án hoàn thành
    TotalProjectDelayDeadline: '', //Dự án dự kiến bị chậm
    TotalProjectNotPlan: '', // Dự án không có kế hoạch
    TotalProjectOnSchedule: '',//Dự án đúng tiến độ
    listTaskType: [],
    Total_Task_Delay: '',
    DateFrom: '',
    DateTo: '',
    DateToV: '',
    DateFromV: '',
    ProjCode: '',
    SBUId: '',
    DepartmentId: ''
  }

  dateStartV = this.dateUtils.getDateNowStart();
  dateEndV = this.dateUtils.getDateNowEnd();
  scrollY = 0;
  isScrollDown = true;
  projectScrollY = 0;
  isProjectScrollDown = true;
  nextTabCount = 0;
  timeCountDown = 15000;

  ngOnInit() {

    this.model.FixDepartmentId = 'b706d52b-1dc3-4f03-8c92-2a52474662cc';
    this.workingTimeSearchModel.DepartmentId = 'b706d52b-1dc3-4f03-8c92-2a52474662cc';
    this.projectSearchModel.DepartmentId = 'b706d52b-1dc3-4f03-8c92-2a52474662cc';

    this.appSetting.PageTitle = "Báo cáo phòng thiết kế";
    this.resize();

    this.report();
    this.searchWorkingTime();
    this.searchProjects();

    this.updateSubscription = interval(300000).subscribe(
      (val) => {
        this.report();
        this.searchWorkingTime();
        this.searchProjects();
      });

    this.scrollSubscription = interval(this.timeCountDown).subscribe(
      (val) => {
        if (this.activeTabId == 2) {
          if (this.isScrollDown) {
            this.scrollY += 400;
          }
          else {
            this.scrollY -= 400;
          }

          if (this.scrollY > this.workingTimeScroll.directiveRef['instance'].contentHeight - this.worktimeHeight) {
            this.isScrollDown = false;
          }
          else if (this.scrollY <= 0) {
            this.isScrollDown = true;
          }

          this.workingTimeScroll.directiveRef.scrollToY(this.scrollY)
        }
        else if (this.activeTabId == 3) {
          if (this.isScrollDown) {
            this.projectScrollY += 400;
          }
          else {
            this.projectScrollY -= 400;
          }

          if (this.projectScrollY > this.projectScroll.directiveRef['instance'].contentHeight - this.projectHeight) {
            this.isProjectScrollDown = false;
          }
          else if (this.projectScrollY <= 0) {
            this.isProjectScrollDown = true;
          }

          this.projectScroll.directiveRef.scrollToY(this.projectScrollY)
        }

        this.nextTabCount++;

        if (this.nextTabCount * this.timeCountDown > 90000) {
          this.nextTabCount = 0;
          if (this.activeTabId < 3) {
            this.activeTabId++;
          } else {
            this.activeTabId = 1;
          }
        }
      });
  }

  ngAfterViewInit() {
    this.scrollDepartmentManage.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollDepartmentManageHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollDepartment.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollDepartmentHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollEmployee.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollEmployeeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollWorkingTime.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollWorkingTimeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollDepartmentManage.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollDepartment.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollEmployee.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollWorkingTime.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollProject.nativeElement.removeEventListener('ps-scroll-x', null);
    this.updateSubscription.unsubscribe();
    this.scrollSubscription.unsubscribe();
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.resize();
  }

  resize() {
    this.minHeight = window.innerHeight - 80;
    this.worktimeHeight = window.innerHeight - 200;
    this.projectHeight = window.innerHeight - 260;
  }

  report() {
    this.model.DateFrom = null;
    if (this.model.DateFromV != null) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }

    this.model.DateTo = null;
    if (this.model.DateToV != null) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    }

    this.reportErrorService.report(this.model).subscribe(
      data => {
        this.errorFixs = data.ErrorFixs;
        // this.errorFixBys = data.ErrorFixBys;

        let byCount = data.ErrorFixBys.length / 2;

        this.errorFixBys = [];
        this.errorFixBys2 = [];
        for (let index = 0; index < data.ErrorFixBys.length; index++) {
          if (index < byCount) {
            this.errorFixBys.push(data.ErrorFixBys[index]);
          }
          else {
            this.errorFixBys2.push(data.ErrorFixBys[index]);
          }
        }

        this.departments = data.Departments;
        this.stages = data.Stages;
        this.errorProjects = data.ErrorProjects;

        // if (this.departments.length > 0) {
        //   this.appSetting.PageTitle = "Báo cáo Vấn đề tồn đọng - " + this.departments[0];
        // }

        this.empMinWidth = 520;
        this.depMinWidth = 470;
      },
      error => {
        this.messageService.showError(error, true);
      }
    );
  }

  searchWorkingTime() {
    if (this.workingTimeSearchModel.DateFromV != null) {
      this.workingTimeSearchModel.DateFrom = this.dateUtils.convertObjectToDate(this.workingTimeSearchModel.DateFromV);
    }
    if (this.workingTimeSearchModel.DateFromV == null) {
      this.workingTimeSearchModel.DateFrom = null;
    }
    if (this.workingTimeSearchModel.DateToV != null) {
      this.workingTimeSearchModel.DateTo = this.dateUtils.convertObjectToDate(this.workingTimeSearchModel.DateToV)
    }
    if (this.workingTimeSearchModel.DateToV == null) {
      this.workingTimeSearchModel.DateTo = null;
    }
    if (this.workingTimeSearchModel != null) {
      this.workingTimeSearchModel.DateStart = this.dateUtils.convertObjectToDate(this.dateStartV);
    } else {
      this.workingTimeSearchModel.DateStart = null;
    }
    if (this.dateEndV != null) {
      this.workingTimeSearchModel.DateEnd = this.dateUtils.convertObjectToDate(this.dateEndV)
    } else {
      this.workingTimeSearchModel.DateEnd = null;
    }
    this.tiviReportTKService.search(this.workingTimeSearchModel).subscribe((data: any) => {
      if (data) {
        this.workTimes = data.ListResult;
        this.days = data.DayofMonths;
        this.tems = data.DayOfWeeks;
        // this.daysOfMonth = data.holiday;
        this.workTimes.forEach(element => {
          element.ListWorkingTime.forEach(item => {
            if (item.IsHoliday) {
              item.EstimateTime = null;
            }
          });
        });
      }
    },
      error => {
        this.messageService.showError(error, true);
      });
  }

  searchProjects() {
    this.tiviReportTKService.searchProjects(this.projectSearchModel).subscribe((data: any) => {
      this.projectInPlans = data.ListProjectInPlan;
    },
      error => {
        this.messageService.showError(error, true);
      });
  }

  changeTab(event) {
    if (event.nextId == '1') {

    }
    else if (event.nextId == '2') {
      // this.scrollWorkingTime.nativeElement.removeEventListener('ps-scroll-x', null);
      // this.scrollWorkingTime.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      //   this.scrollWorkingTimeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      // }, true);
    }
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
