import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as moment from 'moment';
import { WorkDiaryService } from '../service/work-diary.service';

@Component({
  selector: 'app-total-time-perform',
  templateUrl: './total-time-perform.component.html',
  styleUrls: ['./total-time-perform.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TotalTimePerformComponent implements OnInit, AfterViewInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    public constant: Constants,
    private combobox: ComboboxService,
    private service: WorkDiaryService,
    private dateUtils: DateUtils) { }

  @ViewChild('scrollWorkingTime',{static:false}) scrollWorkingTime: ElementRef;
  @ViewChild('scrollWorkingTimeHeader',{static:false}) scrollWorkingTimeHeader: ElementRef;

  @ViewChild('scrollTime',{static:false}) scrollTime: ElementRef;
  @ViewChild('scrollTimeHeader',{static:false}) scrollTimeHeader: ElementRef;

  StartIndex = 0;
  listData: any[] = [];

  currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));

  searchModel: any = {
    DepartmentId: '',
    SBUId: '',
    EmployeeCode: '',
    EmployeeId: '',
    DateToV: this.dateUtils.getFiscalYearEnd(),
    DateFromV: this.dateUtils.getFiscalYearStart(),
    DateStartV: this.dateUtils.getDateNowStart(),
    DateEndV: this.dateUtils.getDateNowEnd()
  }

  columnName: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  columnDepaterment: any[] = [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }];
  columnEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  listSBU = [];
  listDepartment = [];
  listEmployee = [];

  listTem: any = [];
  listDays: any = [];
  daysOfMonth: any = [];
  TotalMonth: 0;
  height = 100;
  isTrue = 0;
  sbuids: string;

  ngOnInit() {

    this.height = window.innerHeight - 350;
    this.searchModel.PlanDate = new Date();
    this.appSetting.PageTitle = "Thời gian làm việc";

    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.searchModel.SBUId = currentUser.sbuId;
      this.sbuids = currentUser.sbuId;
      this.searchModel.DepartmentId = currentUser.departmentId;
      this.searchModel.EmployeeId = currentUser.employeeId;
    }

    this.getListSBU();

    this.searchWorkingTime();
  }
  
  ngAfterViewInit(){
    this.scrollTime.nativeElement.addEventListener('ps-scroll-y', (event: any) => {
      this.scrollWorkingTimeHeader.nativeElement.scrollTop = event.target.scrollTop;
    }, true);
    this.scrollTime.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollTimeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollTime.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollTime.nativeElement.removeEventListener('ps-scroll-y', null);
  }

  searchWorkingTime() {

    if (this.searchModel.DateFromV != null) {
      this.searchModel.DateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DateFromV);
    }
    else {
      this.searchModel.DateFrom = null;
    }
    if (this.searchModel.DateToV != null) {
      this.searchModel.DateTo = this.dateUtils.convertObjectToDate(this.searchModel.DateToV)
    }
    else {
      this.searchModel.DateTo = null;
    }
    if (this.searchModel.DateStartV != null) {
      this.searchModel.DateStart = this.dateUtils.convertObjectToDate(this.searchModel.DateStartV);
    }
    else {
      this.searchModel.DateStart = null;
    }
    if (this.searchModel.DateEndV != null) {
      this.searchModel.DateEnd = this.dateUtils.convertObjectToDate(this.searchModel.DateEndV)
    }
    else {
      this.searchModel.DateEnd = null;
    }

    this.service.searchWorkingTime(this.searchModel).subscribe((data: any) => {
      if (data) {
        this.listData = data.ListResult;
        this.listTem = data.DayOfWeeks;
        this.daysOfMonth = data.holiday;
        this.TotalMonth = data.TotalMonth;

        if (this.searchModel.EmployeeId == null || this.searchModel.EmployeeId == '') {
          this.listData = [];
          this.TotalMonth = null;
          data.DayofMonths.forEach(element => {
            element.TotalTimeDay = null;
          });
        }
        this.listDays = data.DayofMonths;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getListSBU() {
    this.combobox.getCbbSBU().subscribe((data: any) => {
      if (data) {
        this.listSBU = data;
        this.getListDepartmentBySBUId(this.searchModel.SBUId);
        this.listEmployee = [];
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getListDepartmentBySBUId(sbuId) {
    this.combobox.getCbbDepartmentBySBU(sbuId).subscribe((data: any) => {
      if (data) {
        this.listDepartment = data;
        if (this.sbuids != sbuId) {
          this.searchModel.EmployeeId = '';
        }
        if (sbuId == null) {
          this.searchModel.DepartmentId = '';
        }
        this.getListEmployeeBydepartment(this.searchModel.DepartmentId);
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getListEmployeeBydepartment(departmentId) {
    this.combobox.getEmployeeByDepartment(departmentId).subscribe((data: any) => {
      if (data) {
        this.listEmployee = data;
        if (this.sbuids != this.searchModel.SBUId) {
          this.searchModel.EmployeeId = '';
          this.listData = [];
          this.listDays.forEach(element => {
            element.TotalTimeDay = null;
          });
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  changeDate() {
    var dateEnd = moment(this.searchModel.DateStartV).add(-1, "month").add(1, "month");
    var dateNow = new Date();
    var dateNext = new Date(this.searchModel.DateStartV.year, this.searchModel.DateStartV.month, 0);

    if (this.searchModel.DateStartV.day == 1) {
      this.searchModel.DateEndV = { year: dateEnd.year(), month: dateEnd.month(), day: dateNext.getDate() };
    }
    else {
      var monthday = new Date(this.searchModel.DateStartV.year, this.searchModel.DateStartV.month +1, 0);
      if(this.searchModel.DateStartV.day == 31){
        this.searchModel.DateEndV = { year: monthday.getFullYear(), month: monthday.getMonth() + 1, day: monthday.getDate() };
      }
      else{
        this.searchModel.DateEndV = { year: dateEnd.year(), month: dateEnd.month() + 1, day: dateEnd.date() };
      }
      
    }

    this.searchWorkingTime();
  }

  clear() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));

    if (currentUser) {
      this.searchModel.SBUId = currentUser.sbuId;
      this.searchModel.DepartmentId = currentUser.departmentId;
      this.searchModel.EmployeeId = currentUser.employeeId;
    }
    this.searchModel = {
      DepartmentId: currentUser.departmentId,
      SBUId: currentUser.sbuId,
      EmployeeCode: '',
      EmployeeId: currentUser.employeeId,
      DateToV: this.dateUtils.getFiscalYearEnd(),
      DateFromV: this.dateUtils.getFiscalYearStart(),
      DateStartV: this.dateUtils.getDateNowStart(),
      DateEndV: this.dateUtils.getDateNowEnd()
    }

    this.searchWorkingTime();
  }

}
