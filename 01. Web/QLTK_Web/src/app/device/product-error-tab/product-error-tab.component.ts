import { Component, OnInit, Input } from '@angular/core';
import { MessageService, FileProcess, Constants, AppSetting, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductService } from '../services/product.service';
import { ShowImageErrorComponent } from 'src/app/module/Module/show-image-error/show-image-error.component';
import { ShowErrorComponent } from 'src/app/module/Module/show-error/show-error.component';

@Component({
  selector: 'app-product-error-tab',
  templateUrl: './product-error-tab.component.html',
  styleUrls: ['./product-error-tab.component.scss']
})
export class ProductErrorTabComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    public fileProcess: FileProcess,
    private modalService: NgbModal,
    public constants: Constants,
    public appset: AppSetting,
    public dateUtils: DateUtils,
    private service: ProductService,
  ) { }

  @Input() Id: string;
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
    this.model.Id = this.Id;
    this.searchProductError();
  }

  searchProductError() {
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
    this.service.searchProductError(this.model).subscribe(data => {
      this.listData = data.ListResult;
      this.model.TotalItems = data.TotalItem;
      this.model.Status1 = data.Status1;
      this.model.Status2 = data.Status2;
      this.model.Status3 = data.Status3;
      this.model.Status4 = data.Status4;
      this.model.Status5 = data.Status5;
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
    this.model.Id = this.Id;
    this.searchProductError();
  }

}
