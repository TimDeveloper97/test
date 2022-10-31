import { Component, OnInit } from '@angular/core';
import { AppSetting, MessageService, Constants, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { UserHistoryService } from '../../service/user-history.service';

@Component({
  selector: 'app-user-history-managa',
  templateUrl: './user-history-managa.component.html',
  styleUrls: ['./user-history-managa.component.scss']
})
export class UserHistoryManagaComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public dateUtils: DateUtils,
    private userHistoryService: UserHistoryService,
  ) { }

  sbuid = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
  departmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;

  StartIndex = 0;
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Date',
    OrderType: false,
    
    Content: '',
    SBUId: this.sbuid,
    DepartmentId: this.departmentId,
    EmployeeName: ''
  }

  listData : any[] = [];
  // Tool search.
  searchOptions: any = {
    FieldContentName: 'Content',
    Placeholder: 'Tìm kiếm theo nội dung',
    Items: [
      {
        Name: 'Tên nhân viên',
        FieldName: 'EmployeeName',
        Placeholder: 'Nhập tên nhân viên',
        Type: 'text'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 2,
        Permission: ['F110502'],
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 1,
        Permission: ['F110502'],
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ] 
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Lịch sử thao tác";
    this.searchUserHistory();
  }

  searchUserHistory() {

    if (this.model.DateFromV) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    else{
      this.model.DateFrom = null;
    }
    if (this.model.DateToV) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    }
    else{
      this.model.DateTo = null;
    }
    this.userHistoryService.searchUserHistory(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Date',
    OrderType: false,
    
    Content: '',
    SBUId: '',
    DepartmentId: '',
    EmployeeId: ''
    }
    this.searchUserHistory();
  }


}
