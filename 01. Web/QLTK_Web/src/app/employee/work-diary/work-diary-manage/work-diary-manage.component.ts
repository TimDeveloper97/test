import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants, Configuration, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { WorkDiaryCreateComponent } from '../work-diary-create/work-diary-create.component';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { WorkDiaryService } from '../../service/work-diary.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-work-diary-manage',
  templateUrl: './work-diary-manage.component.html',
  styleUrls: ['./work-diary-manage.component.scss']
})
export class WorkDiaryManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private workDiaryService: WorkDiaryService,
    public constant: Constants,
    private config: Configuration,
    public dateUtils: DateUtils,
    private router: Router,
  ) { }

  startIndex = 0;
  pagination;
  lstpageSize = [5, 10, 15, 20, 25, 30];
  listDA: any[] = [];

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'WorkDate',
    OrderType: false,
    DepartmentId: '',
    SBUId: '',
    Id: '',
    WorkTimeId: '',
    EmployeeId: '',
    ProjectId: '',
    ModuleId: '',
    WorkDate: '',
    TotalTime: '',
    PercentFinish: '',
    Address: '',
    DateToV: '',
    DateFromV: '',
  }

  searchOptions: any = {
    FieldContentName: 'EmployeeName',
    Permission: ['F080805'],
    Placeholder: 'Tìm kiếm theo mã hoặc tên nhân viên ...',
    Items: [
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 2,
        Permission: ['F080805'],
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Id: 'Tên SBU' }]
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexTo: 3,
        RelationIndexFrom: 1,
        IsRelation: true,
        Permission: ['F080805'],
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Id: 'Tên phòng ban' }]
      },
      {
        Name: 'Nhân viên',
        FieldName: 'EmployeeId',
        Placeholder: 'Nhân viên',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Employee,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 2,
        Permission: ['F080805'],
        Columns: [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Id: 'Tên nhân viên' }]
      },
    ]
  };

  employeeLoginId = '';

  ngOnInit() {
    this.appSetting.PageTitle = "Nhật kí công việc";

    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.employeeLoginId = currentUser.employeeId;
      this.model.EmployeeId = currentUser.employeeId;
      this.model.DepartmentId = currentUser.departmentId;
      this.model.SBUId = currentUser.sbuId;
    }

    this.searchWorkDiary();
  }
  searchWorkDiary() {
    if (this.model.DateFromV) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    if (this.model.DateToV) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    }
    this.workDiaryService.searchWorkDiary(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listDA = data.ListResult;
        this.model.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'EmployeeName',
      OrderType: true,

      Id: '',
      WorkTimeId: '',
      EmployeeId: '',
      ProjectId: '',
      ModuleId: '',
      WorkDate: '',
      TotalTime: '',
      PercentFinish: '',
      Address: '',
    }
    this.searchWorkDiary();
  }

  showConfirmDeleteWorkDiary(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhật kí công việc này không?").then(
      data => {
        this.deleteWorkDiary(Id);
      },
      error => {
        
      }
    );
  }

  deleteWorkDiary(Id: string) {
    this.workDiaryService.deleteWorkDiary({ Id: Id }).subscribe(
      data => {
        this.searchWorkDiary();
        this.messageService.showSuccess('Xóa nhật kí công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  viewWorkDiary(Id: string) {
    if (Id) {
      this.router.navigate(['nhan-vien/nhat-ky-cong-viec/xem/', Id]);
    }
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(WorkDiaryCreateComponent, { container: 'body', windowClass: 'work-diary-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchWorkDiary();
      }
    }, (reason) => {
    });

  }

  exportExcel() {
    this.workDiaryService.ExcelWorkDiary(this.model).subscribe(d => {
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

}
