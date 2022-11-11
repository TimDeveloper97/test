import { Component, OnInit, ViewChild, ElementRef, OnDestroy, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanService } from '../../service/plan.service';
import * as moment from 'moment';
import { ViewPlanDetailComponent } from '../view-plan-detail/view-plan-detail.component';


@Component({
  selector: 'app-working-time',
  templateUrl: './working-time.component.html',
  styleUrls: ['./working-time.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WorkingTimeComponent implements OnInit, OnDestroy, AfterViewInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    private service: PlanService,
    public constant: Constants,
    private combobox: ComboboxService,
    private dateUtils: DateUtils
  ) { }

  @ViewChild('scrollWorkingTime',{static:false}) scrollWorkingTime: ElementRef;
  @ViewChild('scrollWorkingTimeHeader',{static:false}) scrollWorkingTimeHeader: ElementRef;

  StartIndex = 0;
  listData: any[] = [];
  searchModel: any = {
    DepartmentId: '',
    SBUId: '',
    NameCode: '',
    DateToV: this.dateUtils.getFiscalYearEnd(),
    DateFromV: this.dateUtils.getFiscalYearStart(),
    WorkType:''
  }

  dateStartV = this.dateUtils.getDateNowStart();
  dateEndV = this.dateUtils.getDateNowEnd();
  searchOptions: any = {
    Placeholder: 'Tìm kiếm theo mã hoặc tên nhân viên ...',
    FieldContentName: 'NameCode',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        Permission: ['F060802'],
        RelationIndexTo: 1
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Permission: ['F060802'],
        RelationIndexFrom: 0
      },
      {
        Name: 'Vị trí công việc',
        FieldName: 'WorkType',
        Placeholder: 'Vị trí công việc',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.WorkType,
        Columns: [{ Name: 'Name', Title: 'Vị trí công việc' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  listTem: any = [];
  listDays: any = [];
  daysOfMonth: any = [];
  height = 100;
  isTrue = 0;

  ngOnInit() {
    this.height = window.innerHeight - 320;
    this.searchModel.PlanDate = new Date();
    this.appSetting.PageTitle = "Thời gian làm việc";

    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.searchModel.SBUId = currentUser.sbuId;
      this.searchModel.DepartmentId = currentUser.departmentId;
    }

    this.searchWorkingTime();
  }

  ngAfterViewInit(){
    this.scrollWorkingTime.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollWorkingTimeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollWorkingTime.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  searchWorkingTime() {
    if (this.searchModel.DateFromV != null) {
      this.searchModel.DateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DateFromV);
    }
    if (this.searchModel.DateFromV == null) {
      this.searchModel.DateFrom = null;
    }
    if (this.searchModel.DateToV != null) {
      this.searchModel.DateTo = this.dateUtils.convertObjectToDate(this.searchModel.DateToV)
    }
    if (this.searchModel.DateToV == null) {
      this.searchModel.DateTo = null;
    }
    if (this.dateStartV != null) {
      this.searchModel.DateStart = this.dateUtils.convertObjectToDate(this.dateStartV);
    }else {
      this.searchModel.DateStart = null;
    }
    if (this.dateEndV != null) {
      this.searchModel.DateEnd = this.dateUtils.convertObjectToDate(this.dateEndV)
    }else {
      this.searchModel.DateEnd = null;
    }
    this.service.searchWorkingTime(this.searchModel).subscribe((data: any) => {
      if (data) {
        this.listData = data.ListResult;
        this.listDays = data.DayofMonths;
        this.listTem = data.DayOfWeeks;
        this.daysOfMonth = data.holiday;
        this.listData.forEach(element => {
          element.ListWorkingTime.forEach(item => {
            if (item.IsHoliday) {
              item.EstimateTime = null;
            }
          });
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  changeDate() {
    var dateEnd = moment(this.dateStartV).add(-1, "month").add(1, "month");
    var dateNow = new Date();
    var dateNext = new Date(this.dateStartV.year, this.dateStartV.month, 0);

    if (this.dateStartV.day == 1) {
      this.dateEndV = { year: dateEnd.year(), month: dateEnd.month(), day: dateNext.getDate() };
    }
    else {
      var monthday = new Date(this.dateStartV.year, this.dateStartV.month + 1, 0);
      if (this.dateStartV.day == 31) {
        this.dateEndV = { year: monthday.getFullYear(), month: monthday.getMonth() + 1, day: monthday.getDate() };
      }
      else {
        this.dateEndV = { year: dateEnd.year(), month: dateEnd.month() + 1, day: dateEnd.date() };
      }

    }
    this.searchWorkingTime();
  }

  selectIndex = -1;
  listTemp: any[] = [];
  loadParam(event, EmployeeCode) {
    let activeModal = this.modalService.open(ViewPlanDetailComponent, { container: 'body', windowClass: 'view-plan-model', backdrop: 'static' })
    activeModal.componentInstance.listPlanId = event.PlanId;
    activeModal.componentInstance.EmployeeCode = EmployeeCode;
    activeModal.result.then((result) => {
      if (result) {
        this.searchWorkingTime();
      }
    }, (reason) => {
    });
  }
}