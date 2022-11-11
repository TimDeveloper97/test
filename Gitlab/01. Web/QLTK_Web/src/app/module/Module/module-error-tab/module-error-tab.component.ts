import { Component, OnInit, Input } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { FileProcess, Constants, MessageService, AppSetting, DateUtils } from 'src/app/shared';
import { ModuleServiceService } from '../../services/module-service.service';
import { ShowErrorComponent } from '../show-error/show-error.component';
import { ShowImageErrorComponent } from '../show-image-error/show-image-error.component';

@Component({
  selector: 'app-module-error-tab',
  templateUrl: './module-error-tab.component.html',
  styleUrls: ['./module-error-tab.component.scss']
})
export class ModuleErrorTabComponent implements OnInit {
  @Input() Id: string;
  constructor(
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private modalService: NgbModal,
    public constants: Constants,
    public appset: AppSetting,
    public dateUtils: DateUtils,
    private service: ModuleServiceService,
  ) { }

  startIndex = 1;
  status5: number;
  listData: any[] = [];
  listYear: any[] = [];
  listMonth: any[] = [];

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã lỗi',
    Items: [
      {
        Name: 'Tên lỗi',
        FieldName: 'Subject',
        Placeholder: 'Nhập tên lỗi',
        Type: 'text'
      },
      {
        Name: 'Trạng thái lỗi',
        FieldName: 'Status',
        Placeholder: 'Trạng thái lỗi',
        Type: 'select',
        Data: this.constants.ListError,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ]
  };

  model: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    Status1: '',
    Status2: '',
    Status3: '',
    Status4: '',
    Status5: '',
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Code: '',
    TimeType: '0',
    ModuleErrorVisualId: '',
    ObjectId: '',
    Subject: '',
    Status: '',
    CreateDate: '',
    DateFromV: null,
    DateToV: null,
    DateFrom: null,
    DateTo: null,
    Year: null,
    Month: 1,
  }

  ngOnInit() {
    this.model.ModuleErrorVisualId = this.Id;
    this.searchModuleErrors();
  }

  searchModuleErrors() {
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
    this.service.searchModuleErrors(this.model).subscribe(data => {
      this.listData = data.ListResult;
      this.model.TotalItems = data.TotalItem;
        // trạng thái đang xử lý
        this.model.Status4 = data.Status4;
        // trạng thái đang qc
        this.model.Status6 = data.Status6;
        // trạng thái qc đạt
        this.model.Status7 = data.Status7;
        // trạng thái qc không đạt
        this.model.Status8 = data.Status8;

        // trang thái đóng vấn đề
        this.model.Status9 = data.Status9;
      this.status5 = data.MaxDeliveryDay;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Code: '',
      TimeType: '0',
      ModuleErrorVisualId: '',
      ObjectId: '',
      Subject: '',
      Status: '',
      CreateDate: '',
      DateFromV: null,
      DateToV: null,
      DateFrom: null,
      DateTo: null,
      Month: 1,
    };
    this.model.ModuleErrorVisualId = this.Id;
    this.searchModuleErrors();
  }

  showImage(Id: string) {
    let activeModal = this.modalService.open(ShowImageErrorComponent, { container: 'body', windowClass: 'show-image', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchModuleErrors();
      }
    }, (reason) => {
    });
  }

  showError(Id: string) {
    let activeModal = this.modalService.open(ShowErrorComponent, { container: 'body', windowClass: 'showerror-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchModuleErrors();
      }
    }, (reason) => {
    });
  }

}
