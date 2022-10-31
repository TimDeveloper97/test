import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { ApplyService } from '../../services/apply.service';
import { InterviewService } from '../../services/interview.service';

@Component({
  selector: 'app-interview-manage',
  templateUrl: './interview-manage.component.html',
  styleUrls: ['./interview-manage.component.scss']
})
export class InterviewManageComponent implements OnInit {

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,

    Name: '',
    PhoneNumber: "",
    WorkTypeId: '',
    Email: '',
    ApplyDateTo: null,
    ApplyDateFrom: null,
    ApplyDateToV: null,
    ApplyDateFromV: null,
    Status:'0'
  };

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo mã/tên ứng viên',
    Items: [
      {
        Name: 'Số điện thoại',
        FieldName: 'PhoneNumber',
        Placeholder: 'Nhập số điện thoại',
        Type: 'text'
      },
      {
        Name: 'Email',
        FieldName: 'Email',
        Placeholder: 'Email',
        Type: 'text'
      },
      {
        Name: 'Vị trí ứng tuyển',
        FieldName: 'WorkTypeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.WorkType,
        Columns: [{ Name: 'Name', Title: 'Tên vị trí ứng tuyển' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn Vị trí ứng tuyển',
      },
      {
        Name: 'Thời gian ứng tuyển',
        FieldNameTo: 'ApplyDateToV',
        FieldNameFrom: 'ApplyDateFromV',
        Type: 'date'
      },
      {
        Name: 'Kết quả phỏng vấn',
        FieldName: 'Status',
        Placeholder: 'Kết quả phỏng vấn',
        Type: 'select',
        Data: this.constant.InterviewStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Ngày phỏng vấn',
        FieldNameTo: 'InterviewDateToV',
        FieldNameFrom: 'InterviewDateFromV',
        Type: 'date'
      },
      {
        Name: 'Người phỏng vấn ',
        FieldName: 'EmployeeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employees,
        Columns: [{ Name: 'Name', Title: 'Người phỏng vấn' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn Người phỏng vấn',
      },
    ]
  };

  interviews: any[] = []
  startIndex: number = 1;

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,
    private modalService: NgbModal,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private interviewService: InterviewService,
    private dateUtils: DateUtils) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = "Danh sách phỏng vấn";
    this.searchApplys();
  }

  searchApplys() {
    this.searchModel.ApplyDateTo = null;
    if (this.searchModel.ApplyDateToV) {
      this.searchModel.ApplyDateTo = this.dateUtils.convertObjectToDate(this.searchModel.ApplyDateToV);
    }

    this.searchModel.ApplyDateFrom = null;
    if (this.searchModel.ApplyDateFromV) {
      this.searchModel.ApplyDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.ApplyDateFromV);
    }

    //this.searchModel.InterviewDateTo = null;
    if (this.searchModel.InterviewDateToV) {
      this.searchModel.InterviewDateTo = this.dateUtils.convertObjectToDate(this.searchModel.InterviewDateToV);
    }

    //this.searchModel.InterviewDateFrom = null;
    if (this.searchModel.InterviewDateFromV) {
      this.searchModel.InterviewDateFrom = this.dateUtils.convertObjectToDate(this.searchModel.InterviewDateFromV);
    }

    this.interviewService.searchInterviews(this.searchModel).subscribe(
      data => {
        this.interviews = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,

      Name: null,
      PhoneNumber: null,
      WorkTypeId: null,
      Email: null,
      ApplyDateTo: null,
      ApplyDateFrom: null,
      InterviewDateTo: null,
      InterviewDateFrom: null,
      ApplyDateToV: null,
      ApplyDateFromV: null,
      InterviewDateToV: null,
      InterviewDateFromV: null
    };
    this.searchApplys();
  } 
  
  showConfirmDelete(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn hủy buổi phỏng vấn này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.interviewService.delete(Id).subscribe(
      data => {
        this.searchApplys();
        this.messageService.showSuccess('Hủy lần phỏng vấn thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }
}
