import { Component, OnInit, Input, ViewChild, ElementRef, AfterViewInit, OnDestroy, ViewEncapsulation } from '@angular/core';

import { Constants, MessageService, DateUtils, Configuration } from 'src/app/shared';
import * as moment from 'moment';
import { ProjectErrorService } from '../../../service/project-error.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { InformationErrorComponent } from '../../information-error/information-error.component';
import { Router } from '@angular/router';
import { ProjectErrorCreateComponent } from '../project-error-create/project-error-create.component';
import { ProjectErrorConfirmComponent } from '../project-error-confirm/project-error-confirm.component';
import { ErrorService } from 'src/app/project/service/error.service';
import { ProblemExistConfirmModalComponent } from 'src/app/project/problem-exist/problem-exist-confirm-modal/problem-exist-confirm-modal.component';

@Component({
  selector: 'app-project-error',
  templateUrl: './project-error.component.html',
  styleUrls: ['./project-error.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProjectErrorComponent implements OnInit, AfterViewInit, OnDestroy {

  @Input() Id: string;
  constructor(
    private router: Router,
    public constants: Constants,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
    private service: ProjectErrorService,
    private serviceError: ErrorService,
    private messageService: MessageService,
    public config: Configuration,
  ) {
    this.model.ProjectId = this.Id;
  }

  totalError: number;
  status5: number;
  listData: any[] = [];
  errorFixDisplays: any[] = [];
  errorFixs: any[] = [];
  selectIndex = -1;
  model: any = {
    Id: '',
    ProjectId: '',
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
    DateFromV: ''
  }

  searchOptions: any = {};

  @ViewChild('scrollPlan', { static: false }) scrollPlan: ElementRef;
  @ViewChild('scrollPlanHeader', { static: false }) scrollPlanHeader: ElementRef;

  ngOnInit() {
    this.searchOptions = {
      FieldContentName: 'NameCode',
      Placeholder: '/ Mã vấn đề',
      Items: [
        {
          Name: 'Bộ phận chịu trách nhiệm',
          FieldName: 'DepartmentId',
          Placeholder: 'Bộ phận chịu trách nhiệm',
          Type: 'select',
          DataType: this.constants.SearchDataType.Department,
          DisplayName: 'Name',
          ValueName: 'Id'
        },
        {
          Name: 'Người chịu trách nhiệm',
          FieldName: 'ErrorBy',
          Placeholder: 'Người chịu trách nhiệm',
          Type: 'select',
          DataType: this.constants.SearchDataType.Employee,
          DisplayName: 'Name',
          ValueName: 'Id'
        },
        {
          Name: 'Bộ phận khắc phục',
          FieldName: 'DepartmentProcessId',
          Placeholder: 'Bộ phận khắc phục',
          Type: 'select',
          DataType: this.constants.SearchDataType.Department,
          DisplayName: 'Name',
          ValueName: 'Id'
        },
        {
          Name: 'Người khắc phục',
          FieldName: 'FixBy',
          Placeholder: 'Người khắc phục',
          Type: 'select',
          DataType: this.constants.SearchDataType.Employee,
          DisplayName: 'Name',
          ValueName: 'Id'
        },
        {
          Name: 'Mã Module',
          FieldName: 'ObjectId',
          Type: 'dropdown',
          SelectMode: 'single',
          DataType: this.constants.SearchDataType.Module,
          Columns: [{ Name: 'Code', Title: 'Mã Module' }, { Name: 'Name', Title: 'Tên Module' }],
          DisplayName: 'Code',
          ValueName: 'Id',
          Placeholder: 'Chọn mã Module',
          ObjectId: this.Id
        },
        {
          Name: 'Công đoạn',
          FieldName: 'StageId',
          Placeholder: 'Công đoạn',
          Type: 'select',
          DataType: this.constants.SearchDataType.Stage,
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
          Data: this.constants.ProblemType,
          DisplayName: 'Name',
          ValueName: 'Id'
        },
        {
          Name: 'Tình trạng',
          FieldName: 'Status',
          Placeholder: 'Tình trạng',
          Type: 'select',
          Data: this.constants.ListError,
          DisplayName: 'Name',
          ValueName: 'Id'
        },
      ]
    };
    this.model.ProjectId = this.Id;
    this.searchModuleErrors();
  }

  ngAfterViewInit() {
    this.scrollPlan.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPlanHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPlan.nativeElement.removeEventListener('ps-scroll-x', null);
  }


  searchModuleErrors() {
    if (this.model.DateFromV) {
      this.model.DateOpen = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }
    if (this.model.DateToV) {
      this.model.DateEnd = this.dateUtils.convertObjectToDate(this.model.DateToV);
    }
    this.service.searchModuleErrors(this.model).subscribe(
      data => {
        this.listData = data.Errors;
        this.errorFixs = data.ErrorFixs;
        //this.errorFixDisplays = [...this.errorFixs];

        this.model.TotalItems = data.TotalItem;
        this.totalError = data.TotalError;
        // trạng thái đang tạo
        this.model.Status1 = data.Status1;
        // trạng thái đang chờ xử lý
        this.model.Status2 = data.Status2;
        // trạng thái đã xác nhận
        this.model.Status3 = data.Status3;
        // trạng thái đang xử lý
        this.model.Status4 = data.Status4;
        this.model.Status5 = data.Status5;
        // trạng thái đang qc
        this.model.Status6 = data.Status6;
        // trạng thái qc đạt
        this.model.Status7 = data.Status7;
        // trạng thái qc không đạt
        this.model.Status8 = data.Status8;
        // trang thái đóng vấn đề
        this.model.Status9 = data.Status9;
        //Tổng số công việc
        this.model.Status10 = data.Status10;
        //Tổng số công việc hoàn thành
        this.model.Status11 = data.Status11;
        //Tổng số công việc đã triển khai
        this.model.Status12 = data.Status12;
        //Tổng số công việc chưa chiển khai
        this.model.Status13 = data.Status13;
        this.status5 = data.MaxDeliveryDay;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      ProjectId: this.Id,
    }
    this.searchModuleErrors();
  }

  showConfirmDeleteError(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vấn đề này không?").then(
      data => {
        this.deleteError(Id);
      },
      error => {

      }
    );
  }

  deleteError(Id: string) {
    this.serviceError.deleteError({ Id: Id }).subscribe(
      data => {
        this.searchModuleErrors();
        this.messageService.showSuccess('Xóa vấn đề thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirm(Status: number, Id: string) {

    if (Status != 1) {
      this.showConfirmProjectError(Id);
    }
    else {
      this.messageService.showMessage("Vấn đề chưa được yêu cầu xác nhận")
    }
  }

  showConfirmProjectError(Id: string) {
    let activeModal = this.modalService.open(ProblemExistConfirmModalComponent, { container: 'body', windowClass: 'problem-exist-confirm-modal', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchModuleErrors();
      }
    }, (reason) => {
    });
  }

  showClick(Id: string) {
    let activeModal = this.modalService.open(InformationErrorComponent, { container: 'body', windowClass: 'information-error', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {
    });
  }

  showCreateUpdate(Id: string) {
    if (Id) {
      this.listData.forEach(element => {
        if (element.Id == Id) {
          if (element.Code != null) {
            let activeModal = this.modalService.open(ProjectErrorCreateComponent, { container: 'body', windowClass: 'project-error-create', backdrop: 'static' })
            activeModal.componentInstance.Id = Id;
            activeModal.componentInstance.projectId = this.model.ProjectId;
            activeModal.result.then((result) => {
              if (result) {
                this.searchModuleErrors();
              }
            }, (reason) => {
            });
          } else {
            this.messageService.showSuccess('Thêm mới vấn đề thành công!');
          }
        }
      });
    } else {
      let activeModal = this.modalService.open(ProjectErrorCreateComponent, { container: 'body', windowClass: 'project-error-create', backdrop: 'static' })
      activeModal.componentInstance.Id = Id;
      activeModal.componentInstance.projectId = this.model.ProjectId;
      activeModal.result.then((result) => {
        if (result) {
          this.searchModuleErrors();
        }
      }, (reason) => {
      });
    }
  }

  selectedRowKey: any[] = [];
  onSelectionChanged(e) {
    this.errorFixDisplays = [];
    if (this.selectedRowKey[0] == e.selectedRowKeys[0]) {
      this.errorFixs.forEach(element => {
        if (element.ErrorId == e.selectedRowKeys[0]) {
          this.errorFixDisplays.push(element);
        }
      });
    } else {
      e.selectedRowKeys = [];

      this.errorFixs.forEach(element => {
        this.errorFixDisplays.push(element);
      });
    }
  }

  select(e) {
    if (e.data.Id == this.selectedRowKey[0]) {
      this.selectedRowKey = [];

      this.errorFixs.forEach(element => {
        this.errorFixDisplays.push(element);
      });
    } else {
      this.selectedRowKey = [];
      this.selectedRowKey.push(e.data.Id);
    }
  }


  selectError(index) {
    if (this.selectIndex != index) {
      this.selectIndex = index;
      this.errorFixDisplays = [];
      this.errorFixs.forEach(element => {
        if (element.ErrorId == this.listData[this.selectIndex].Id) {
          this.errorFixDisplays.push(element);
        }
      });
    }
    else {
      this.selectIndex = -1;
      this.errorFixDisplays = [...this.errorFixs];
    }
  }

  exportExcel() {
    this.service.exportExcelError(this.Id).subscribe(d => {
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
