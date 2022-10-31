import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService, AppSetting, DateUtils } from 'src/app/shared';
import { ExportAndKeepService } from '../service/export-and-keep.service';

@Component({
  selector: 'app-export-and-keep-history',
  templateUrl: './export-and-keep-history.component.html',
  styleUrls: ['./export-and-keep-history.component.scss']
})
export class ExportAndKeepHistoryComponent implements OnInit {

  constructor(
    public constant: Constants,
    private router: Router,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public appSetting: AppSetting,
    public service: ExportAndKeepService,
    public dateUtil: DateUtils
  ) { }

  listData = [];
  startIndex = 1;

  model: any = {
    PageSize: 20,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'ExpiredDate',
    OrderType: true,

    Status: '',
    Code: '',
    Customer: '',
    Product: '',
    CreateDateTo: '',
    CreateDateFrom: '',
    ExpiredDateFrom: '',
    ExpiredDateTo: '',
    CreateDateToV: null,
    CreateDateFromV: null,
    ExpiredDateFromV: null,
    ExpiredDateToV: null,
    Quantity: '',
    QuantityType: 1,
    EmployeeId: null
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã xuất giữ',
    Items: [
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Tình trạng',
        Type: 'select',
        Data: this.constant.exportandkeepstatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Khách hàng',
        FieldName: 'Customer',
        Placeholder: 'Nhập khách hàng',
        Type: 'text'
      },
      {
        Name: 'Sản phẩm',
        FieldName: 'Product',
        Placeholder: 'Nhập sản phẩm',
        Type: 'text'
      },
      {
        Name: 'Ngày tạo',
        FieldNameTo: 'CreateDateToV',
        FieldNameFrom: 'CreateDateFromV',
        Type: 'date'
      },
      {
        Name: 'Hạn xuất giữ',
        FieldNameTo: 'ExpiredDateToV',
        FieldNameFrom: 'ExpiredDateFromV',
        Type: 'date'
      },
      {
        Name: 'Số lượng sản phẩm',
        FieldName: 'Quantity',
        FieldNameType: 'QuantityType',
        Placeholder: 'Nhập số lượng sản phẩm...',
        Type: 'number'
      },
      {
        Name: 'Nhân viên',
        FieldName: 'EmployeeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employees,
        Columns: [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn nhân viên',
        Permission: ['F120202'],
      },
    ]
  };

  ngOnInit() {
    this.appSetting.PageTitle = "Lịch sử xuất giữ"
    this.model.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    this.searchExportAndKeep();
  }

  searchExportAndKeep() {
    if (this.model.CreateDateToV != null) {
      this.model.CreateDateTo = this.dateUtil.convertObjectToDate(this.model.CreateDateToV);
    }
    else {
      this.model.CreateDateTo = null;
    }

    if (this.model.CreateDateFromV != null) {
      this.model.CreateDateFrom = this.dateUtil.convertObjectToDate(this.model.CreateDateFromV);
    }
    else {
      this.model.CreateDateFrom = null;
    }

    if (this.model.ExpiredDateToV != null) {
      this.model.ExpiredDateTo = this.dateUtil.convertObjectToDate(this.model.ExpiredDateToV);
    }
    else {
      this.model.ExpiredDateTo = null;
    }

    if (this.model.ExpiredDateFromV != null) {
      this.model.ExpiredDateFrom = this.dateUtil.convertObjectToDate(this.model.ExpiredDateFromV);
    }
    else {
      this.model.ExpiredDateFrom = null;
    }

    this.service.SearchExportAndKeepHistory(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showExportAndKeep(id: string) {
    this.router.navigate(['kinh-doanh/lich-su-xuat-giu/xem/' + id]);
  }


  clear() {
    this.model = {
      PageSize: 20,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'ExpiredDate',
      OrderType: true,

      Status: '',
      Code: '',
      Customer: '',
      Product: '',
      CreateDateTo: '',
      CreateDateFrom: '',
      ExpiredDateFrom: '',
      ExpiredDateTo: '',
      Quantity: '',
      QuantityType: 1,
      EmployeeId: null
    }

    this.searchExportAndKeep();
  }

}
