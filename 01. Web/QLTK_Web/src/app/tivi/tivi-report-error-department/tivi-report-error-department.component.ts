import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TiviReportErrorService } from '../services/tivi-report-error.service';
import { ActivatedRoute } from '@angular/router';
import { timeStamp } from 'console';
import { Observable, interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-tivi-report-error-department',
  templateUrl: './tivi-report-error-department.component.html',
  styleUrls: ['./tivi-report-error-department.component.scss']
})
export class TiviReportErrorDepartmentComponent implements OnInit, AfterViewInit, OnDestroy {

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
  ) { }

  @ViewChild('scrollDepartmentManage', { static: false }) scrollDepartmentManage: ElementRef;
  @ViewChild('scrollDepartmentManageHeader', { static: false }) scrollDepartmentManageHeader: ElementRef;

  @ViewChild('scrollDepartment', { static: false }) scrollDepartment: ElementRef;
  @ViewChild('scrollDepartmentHeader', { static: false }) scrollDepartmentHeader: ElementRef;

  @ViewChild('scrollEmployee', { static: false }) scrollEmployee: ElementRef;
  @ViewChild('scrollEmployeeHeader', { static: false }) scrollEmployeeHeader: ElementRef;
  depMinWidth = 250;
  empMinWidth = 250;
  departmentSelectIndex = -1;
  employeeSelectIndex = -1;
  minHeight = 700;
  private updateSubscription: Subscription;

  ngOnInit() {

    this.model.FixDepartmentId = this.routeA.snapshot.paramMap.get('id');

    this.appSetting.PageTitle = "Báo cáo Vấn đề tồn đọng phòng ban";

    this.minHeight = window.innerHeight - 80;
    this.report();

    this.updateSubscription = interval(300000).subscribe(
      (val) => {
        this.report();
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
  }

  ngOnDestroy() {
    this.scrollDepartmentManage.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollDepartment.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollEmployee.nativeElement.removeEventListener('ps-scroll-x', null);
    this.updateSubscription.unsubscribe();
  }

  errorFixs: any[] = [];
  errorFixBys: any[] = [];
  errorFixBys2: any[] = [];
  departments: any[] = [];
  errorProjects: any[] = [];
  stages: any[] = [];

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

        if (this.departments.length > 0) {
          this.appSetting.PageTitle = "Báo cáo Vấn đề tồn đọng - " + this.departments[0];
        }

        this.empMinWidth = 520;
        this.depMinWidth = 470;
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
    }
    let now = new Date();
    // let dateFromV = { day: 1, month: 4, year: now.getFullYear() };
    // this.model.DateFromV = dateFromV;
    // let dateToV = { day: 31, month: 3, year: now.getFullYear() + 1 };
    // this.model.DateToV = dateToV;

    this.report();
  }

  exportExcel() {
    this.reportErrorService.exportExcel(this.model).subscribe(d => {
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
