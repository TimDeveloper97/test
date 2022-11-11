import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

import { DxTreeListComponent } from 'devextreme-angular';
import { AppSetting, MessageService, Configuration, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { ErrorService } from '../../service/error.service';
import { ErrorGroupCreateComponent } from '../../error-group/error-group-create/error-group-create.component';
import { ThisReceiver } from '@angular/compiler';

@Component({
  selector: 'app-problem-exist-manage',
  templateUrl: './problem-exist-manage.component.html',
  styleUrls: ['./problem-exist-manage.component.scss']
})
export class ProblemExistManageComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private config: Configuration,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    public dateUtils: DateUtils,
    private service: ErrorService,
    private combobox: ComboboxService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.items = [
      { Id: 1, text: 'Sửa', icon: 'fa fa-edit' },
      { Id: 2, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }

  startIndex = 0;
  totalItems = 0;
  height = 0;
  status5 = 0;
  items: any;
  dateOpen: any;
  dateEnd: any;
  updateDate: any;
  errors: any[] = [];
  errorFixs: any[] = [];
  errorFixDisplays: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listStage: any[] = [];
  listErrorGroup: any[] = [];
  listErrorGroupId = [];
  selectedErrorGroupId = '';
  errorGroupId: '';

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'PlanStartDate',
    OrderType: false,
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
    Type: 0,
    DateToV: '',
    UpdateDateToV: '',
    UpdateDateFromV: '',
    DateFromV: '',
    SBUId: '',
    ProjectId: '',
    AuthorId: '',
    Description: '',
    FixStatus: null,
    ChangePlan: null,
    DepartmentManageId: null,
    ErrorAffectId: null,
    AuthorDepartmentId: null
  }

  errorGroupModel: any = {
    Id: '',
    Check: 1
  }

  allModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'PlanStartDate',
    OrderType: false,
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'NameCode',
    Placeholder: 'Tên vấn đề/ Mã vấn đề',
    Items: [
      {
        Name: 'Dự án',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Project,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn dự án',
        IsRelation: true,
        RelationIndexTo: 6
      },
      {
        Name: 'Người phát hiện',
        FieldName: 'AuthorId',
        Placeholder: 'Người phát hiện',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Employee,
        Columns: [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Bộ phận phát hiện',
        FieldName: 'AuthorDepartmentId',
        Placeholder: 'Bộ phận phát hiện',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        // RelationIndexFrom: 2
      },
      {
        Name: 'Bộ phận chịu trách nhiệm',
        FieldName: 'DepartmentId',
        Placeholder: 'Bộ phận chịu trách nhiệm',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        // RelationIndexFrom: 2
      },
      {
        Name: 'Người chịu trách nhiệm',
        FieldName: 'ErrorBy',
        Placeholder: 'Người chịu trách nhiệm',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Employee,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Bộ phận khắc phục',
        FieldName: 'DepartmentProcessId',
        Placeholder: 'Bộ phận khắc phục',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Người khắc phục',
        FieldName: 'FixBy',
        Placeholder: 'Người khắc phục',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Employee,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
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
        Placeholder: 'Chọn mã Module',
        RelationIndexFrom: 0,
      },
      {
        Name: 'Bộ phận quản lý',
        FieldName: 'DepartmentManageId',
        Placeholder: 'Bộ phận quản lý',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id'
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
        Name: 'Yếu tố ảnh hưởng',
        FieldName: 'ErrorAffectId',
        Placeholder: 'Yếu tố ảnh hưởng',
        Type: 'select',
        DataType: this.constant.SearchDataType.ErrorAffect,
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
        Name: 'Loại vấn đề',
        FieldName: 'Type',
        Placeholder: 'Loại vấn đề',
        Type: 'select',
        Data: this.constant.ProblemType,
        DisplayName: 'Name',
        ValueName: 'Id'
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
      {
        Name: 'Mô tả',
        FieldName: 'Description',
        Placeholder: 'Mô tả',
        Type: 'text',
      },
      {
        Name: 'Tình trạng xử lý',
        FieldName: 'FixStatus',
        Placeholder: 'Tình trạng xử lý',
        Type: 'select',
        Data: this.constant.FixStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thay đổi kế hoạch',
        FieldName: 'ChangePlanId',
        Placeholder: 'Thay đổi kế hoạch',
        Type: 'select',
        Data: this.constant.ChangePlan,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Ngày cập nhật công việc xử lý',
        FieldNameTo: 'UpdateDateToV',
        FieldNameFrom: 'UpdateDateFromV',
        Type: 'date'
      },
    ]
  };

  TotalError = 0;
  TotoalProblem = 0;
  ErrorStatus1 = 0;
  ErrorStatus2 = 0;
  ErrorStatus3 = 0;
  ErrorStatus4 = 0;
  ErrorStatus5 = 0;
  ErrorStatus6 = 0;
  ErrorStatus7 = 0;
  ErrorStatus8 = 0;
  ErrorStatus9 = 0;
  ErrorStatus10 = 0;


  ProblemStatus1 = 0;
  ProblemStatus2 = 0;
  ProblemStatus3 = 0;
  ProblemStatus4 = 0;
  ProblemStatus5 = 0;
  ProblemStatus6 = 0;
  ProblemStatus7 = 0;
  ProblemStatus8 = 0;
  ProblemStatus9 = 0;
  ProblemStatus10 = 0;

  selectIndex = -1;

  @ViewChild('scrollPlan', { static: false }) scrollPlan: ElementRef;
  @ViewChild('scrollPlanHeader', { static: false }) scrollPlanHeader: ElementRef;

  ngOnInit() {
    // this.model.SBUId = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;

    this.appSetting.PageTitle = "Quản lý vấn đề tồn đọng";

    this.selectedErrorGroupId = localStorage.getItem("selectedErrorGroupId");
    localStorage.removeItem("selectedErrorGroupId");
    this.searchErrorGroup();
    this.height = window.innerHeight - 140;


    this.route.queryParams
      .subscribe(params => {
        console.log(params);
        this.model.StageId = params.stageId;
        this.model.ErrorAffectId = params.affectId;
        this.model.DepartmentProcessId = params.departmentId;
        this.model.FixBy = params.employeeId;
        this.model.FixStatus = params.fixStatus;
        this.model.ChangePlan = params.ChangePlan;
        this.model.DepartmentManageId = params.departmentManageId;        
        this.model.PlanType = params.planType;
        if (params.dateFrom) {
          this.model.DateFromV = this.dateUtils.convertDateToObject(params.dateFrom);
        }
        if (params.dateTo) {
          this.model.DateToV = this.dateUtils.convertDateToObject(params.dateTo);
        }
        if (params.updateDateFrom) {
          this.model.UpdateDateFromV = this.dateUtils.convertDateToObject(params.updateDateFrom);
        }
        if (params.updateDateTo) {
          this.model.UpdateDateToV = this.dateUtils.convertDateToObject(params.updateDateTo);
        }
        
        this.model.ProjectId = params.projectId;

        let now = new Date();
        this.searchProblemExist(this.errorGroupId);
      });
  }

  ngAfterViewInit() {
    this.scrollPlan.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPlanHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPlan.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  itemClick(e) {
    if (this.errorGroupId == '' || this.errorGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm vấn đề!")
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
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedErrorGroupId) {
      this.selectedErrorGroupId = e.selectedRowKeys[0];
      this.searchProblemExist(e.selectedRowKeys[0]);
      this.errorGroupId = e.selectedRowKeys[0];
    }
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

  searchProblemExist(ErrorGroupId: string) {
    this.model.ErrorGroupId = ErrorGroupId;
    if (this.model.DateFromV) {
      this.model.DateOpen = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    } else {
      this.model.DateOpen = null;
    }
    if (this.model.DateToV) {
      this.model.DateEnd = this.dateUtils.convertObjectToDate(this.model.DateToV);
    } else {
      this.model.DateEnd = null;
    }

    if (this.model.UpdateDateFromV) {
      this.model.UpdateDateOpen = this.dateUtils.convertObjectToDate(this.model.UpdateDateFromV);
    } 
    if (this.model.UpdateDateToV) {
      this.model.UpdateDateEnd = this.dateUtils.convertObjectToDate(this.model.UpdateDateToV );
    } 
    this.service.searchProblemExist(this.model).subscribe((data: any) => {
      if (data.Errors) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.errors = data.Errors;
        this.errorFixs = data.ErrorFixs;
        this.errorFixDisplays = [...this.errorFixs];
        this.model.TotalItems = data.TotalItem;
        this.ErrorStatus1 = data.ErrorStatus1;
        this.ErrorStatus2 = data.ErrorStatus2;
        this.ErrorStatus3 = data.ErrorStatus3;
        this.ErrorStatus4 = data.ErrorStatus4;
        this.ErrorStatus5 = data.ErrorStatus5;
        this.ErrorStatus6 = data.ErrorStatus6;
        this.ErrorStatus7 = data.ErrorStatus7;
        this.ErrorStatus8 = data.ErrorStatus8;
        this.ErrorStatus9 = data.ErrorStatus9;
        this.ErrorStatus10 = data.ErrorStatus10;

        this.ProblemStatus1 = data.ProblemStatus1;
        this.ProblemStatus2 = data.ProblemStatus2;
        this.ProblemStatus3 = data.ProblemStatus3;
        this.ProblemStatus4 = data.ProblemStatus4;
        this.ProblemStatus5 = data.ProblemStatus5;
        this.ProblemStatus6 = data.ProblemStatus6;
        this.ProblemStatus7 = data.ProblemStatus7;
        this.ProblemStatus8 = data.ProblemStatus8;
        this.ProblemStatus9 = data.ProblemStatus9;
        this.ProblemStatus10 = data.ProblemStatus10;

        this.TotalError = data.Type1;
        this.TotoalProblem = data.Type2;

        this.status5 = data.MaxDeliveryDay;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }


  clear(errorGroupId: string) {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'PlanStartDate',
      OrderType: false,
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
      UpdateDateToV: '',
      UpdateDateFromV: '',
      Type: 0,
      DateToV: '',
      DateFromV: '',
    }
    this.listErrorGroup = [];
    this.listErrorGroupId = [];
    this.selectedErrorGroupId = '';
    this.errorGroupId = '';
    this.dateOpen = '';
    this.dateEnd = '';
    this.updateDate = '';
    this.searchProblemExist(errorGroupId);
    this.searchErrorGroup();
  }

  showConfirmDeleteErrorGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhóm vấn đề này không?").then(
      data => {
        this.deleteErrorGroup(Id);
      },
      error => {

      }
    );
  }

  deleteErrorGroup(Id: string) {
    this.errorGroupModel.Id = Id;
    this.service.deleteErrorGroup(this.errorGroupModel).subscribe(
      data => {
        this.searchErrorGroup();
        this.searchProblemExist(this.errorGroupId);
        this.messageService.showSuccess('Xóa nhóm vấn đề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteError(Id: string, ErrorGroupId: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vấn đề này không?").then(
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
        this.searchProblemExist(ErrorGroupId);
        this.messageService.showSuccess('Xóa vấn đề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirm(Status: number, Id: string) {
    if (Status != 1) {
      this.router.navigate(['du-an/quan-ly-van-de/xac-nhan-van-de/', Id]);
    }
    else {
      this.messageService.showMessage("Vấn đề chưa được yêu cầu xác nhận")
    }
  }

  showCreateUpdate(Id: string) {
    this.router.navigate(['du-an/quan-ly-van-de/van-de/', Id]);
  }

  showCreateUpdateErrorGroup(Id: string) {
    let activeModal = this.modalService.open(ErrorGroupCreateComponent, { container: 'body', windowClass: 'error-group-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.check = "1";
    activeModal.result.then((result) => {
      if (result) {
        this.searchErrorGroup();
      }
    }, (reason) => {
    });
  }

  exportExcel() {
    this.service.exportExcel(this.model).subscribe(d => {
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

  selectError(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.errorFixDisplays = [];
      this.errorFixs.forEach(element => {
        if (element.ErrorId == this.errors[this.selectIndex].Id) {
          this.errorFixDisplays.push(element);
        }
      });
    }
    else {
      this.selectIndex = -1;
      this.errorFixDisplays = [...this.errorFixs];
    }
  }
}
