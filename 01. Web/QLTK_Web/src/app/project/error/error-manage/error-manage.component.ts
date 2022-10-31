import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import { DxTreeListComponent } from 'devextreme-angular';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AppSetting, MessageService, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { ErrorGroupCreateComponent } from '../../error-group/error-group-create/error-group-create.component';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-error-manage',
  templateUrl: './error-manage.component.html',
  styleUrls: ['./error-manage.component.scss']
})
export class ErrorManageComponent implements OnInit {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    public dateUtils: DateUtils,
    private service: ErrorService,
    private combobox: ComboboxService,
    private router: Router,
  ) {
    this.items = [
      { Id: 1, text: 'Sửa', icon: 'fa fa-edit' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }

  startIndex = 0;
  totalItems = 0;
  height = 0;
  items: any;
  listData: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listStage: any[] = [];
  listErrorGroup: any[] = [];
  listErrorGroupId = [];
  selectedErrorGroupId = '';
  errorGroupId: '';
  dateOpen: any;
  dateEnd: any;
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Status1: '',
    Status2: '',
    Status3: '',
    Status4: '',
    Id: '',
    Subject: '',
    Code: '',
    Status: 0,
    ErrorGroupId: '',
    DepartmentId: '',
    ErrorBy: '',
    DepartmentProcessId: '',
    ObjectId: '',
    StageId: '',
    FixBy: '',
    DateOpen: '',
    DateEnd: '',
    DateToV: '',
    DateFromV: '',
  }

  errorGroupModel: any = {
    Id: '',
  }

  allModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'NameCode',
    Placeholder: 'Tên lỗi/ Mã lỗi',
    Items: [
      {
        Name: 'Bộ phận gây lỗi',
        FieldName: 'DepartmentId',
        Placeholder: 'Bộ phận gây lỗi',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Người gây lỗi',
        FieldName: 'ErrorBy',
        Placeholder: 'Người gây lỗi',
        Type: 'select',
        DataType: this.constant.SearchDataType.Employee,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Bộ phận khắc phục',
        FieldName: 'DepartmentProcessId',
        Placeholder: 'Bộ phận khắc phục',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Người khắc phục',
        FieldName: 'FixBy',
        Placeholder: 'Người khắc phục',
        Type: 'select',
        DataType: this.constant.SearchDataType.Employee,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Mã Module',
        FieldName: 'ObjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Module,
        Columns: [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn mã Module'
      },
      {
        Name: 'Công đoạn',
        FieldName: 'StageId',
        Placeholder: 'Công đoạn',
        Type: 'select',
        DataType: this.constant.SearchDataType.Stage,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.ListError,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit() {
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Quản lý lỗi";
    this.searchErrorGroup();
    this.searchError("");
    this.getCbbDepartment();
    this.getCbbEmployee();
    this.getCbbStage();
    this.selectedErrorGroupId = localStorage.getItem("selectedErrorGroupId");
    localStorage.removeItem("selectedErrorGroupId");
  }

  itemClick(e) {
    if (this.errorGroupId == '' || this.errorGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm lỗi!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdateErrorGroup(this.errorGroupId);
      }
      else if (e.itemData.Id == 2) {
        this.showConfirmDeleteErrorGroup(this.errorGroupId);
      }
    }
  }

  onSelectionChanged(e) {
    this.selectedErrorGroupId = e.selectedRowKeys[0];
    this.searchError(e.selectedRowKeys[0]);
    this.errorGroupId = e.selectedRowKeys[0];
  }

  searchErrorGroup() {
    this.service.searchErrorGroup(this.errorGroupModel).subscribe((data: any) => {
      if (data) {
        this.listErrorGroup = data;
        this.totalItems = data.length;
        this.listErrorGroup.unshift(this.allModel);

        if (this.selectedErrorGroupId == null) {
          this.selectedErrorGroupId = this.listErrorGroup[0].Id;
        }
        this.treeView.selectedRowKeys = [this.selectedErrorGroupId];
        for (var item of this.listErrorGroup) {
          this.listErrorGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchError(ErrorGroupId: string) {
    this.model.ErrorGroupId = ErrorGroupId;
    if (this.model.DateFromV) {
      this.model.DateOpen = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    if (this.model.DateToV) {
      this.model.DateEnd = this.dateUtils.convertObjectToDate(this.model.DateToV);
    }
    this.service.searchError(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        this.model.Status1 = data.Status1;
        this.model.Status2 = data.Status2;
        this.model.Status3 = data.Status3;
        this.model.Status4 = data.Status4;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbbDepartment() {
    this.combobox.getCbbDepartment().subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbEmployee() {
    this.combobox.getCbbEmployee().subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbStage() {
    this.combobox.getCbbStage().subscribe(
      data => {
        this.listStage = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear(errorGroupId: string) {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Id: '',
      Subject: '',
      Code: '',
      Status: 0,
      ErrorGroupId: '',
      DepartmentId: '',
      ErrorBy: '',
      DepartmentProcessId: '',
      StageId: '',
      FixBy: '',
      DateOpen: '',
      DateEnd: '',
      DateToV: '',
      DateFromV: '',
    }
    this.listErrorGroup = [];
    this.listErrorGroupId = [];
    this.selectedErrorGroupId = '';
    this.errorGroupId = '';
    this.dateOpen = '';
    this.dateEnd = '';
    this.searchError(errorGroupId);
    this.searchErrorGroup();
  }

  showConfirmDeleteErrorGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm lỗi này không?").then(
      data => {
        this.deleteErrorGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteErrorGroup(Id: string) {
    this.service.deleteErrorGroup({ Id: Id }).subscribe(
      data => {
        this.searchErrorGroup();
        this.searchError("");
        this.messageService.showSuccess('Xóa nhóm lỗi thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteError(Id: string, ErrorGroupId: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá lỗi này không?").then(
      data => {
        this.deleteError(Id, ErrorGroupId);
      },
      error => {
        
      }
    );
  }

  deleteError(Id: string, ErrorGroupId: string) {
    this.service.deleteError({ Id: Id }).subscribe(
      data => {
        this.searchError(ErrorGroupId);
        this.messageService.showSuccess('Xóa lỗi thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirm(Status: number, Id: string) {
    if (Status != 1) {
      this.router.navigate(['du-an/quan-ly-loi/xac-nhan-loi/', Id]);
    }
    else {
      this.messageService.showMessage("Lỗi chưa được yêu cầu xác nhận")
    }
  }

  showCreateUpdate(Id: string) {
    this.router.navigate(['du-an/quan-ly-loi/loi/', Id]);
  }

  showCreateUpdateErrorGroup(Id: string) {
    let activeModal = this.modalService.open(ErrorGroupCreateComponent, { container: 'body', windowClass: 'error-group-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchErrorGroup();
      }
    }, (reason) => {
    });
  }

}
