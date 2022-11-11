import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, DateUtils, MessageService } from 'src/app/shared';
import { ExportAndKeepService } from '../service/export-and-keep.service';

@Component({
  selector: 'app-export-and-keep-manage',
  templateUrl: './export-and-keep-manage.component.html',
  styleUrls: ['./export-and-keep-manage.component.scss']
})
export class ExportAndKeepManageComponent implements OnInit {

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
  isAction: boolean = false;

  listStatus = [
    { Id: null, Name: "Tất cả" },
    { Id: 2, Name: "Quá hạn" },
    { Id: 1, Name: "Đúng hạn" }
  ]

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
    EmployeeId: null,
    PayStatus: null,
    // SupplierId: null
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
        Data: this.listStatus,
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
      // {
      //   Name: 'Nhà cung cấp',
      //   FieldName: 'SupplierId',
      //   Placeholder: 'Chọn Nhà cung cấp',
      //   Type: 'dropdown', SelectMode: 'single',
      //   DataType: this.constant.SearchDataType.Supplier,
      //   Columns: [{ Name: 'Code', Title: 'Mã NCC' }, { Name: 'Name', Title: 'Tên NCC' }],
      //   DisplayName: 'Code',
      //   ValueName: 'Id'
      // },
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
        Permission: ['F120120'],
      },
      {
        Name: 'Tình trạng thanh toán',
        FieldName: 'PayStatus',
        Placeholder: 'Tình trạng thanh toán',
        Type: 'select',
        Data: this.constant.KeepAndExportPayStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  user: any;

  ngOnInit() {
    this.appSetting.PageTitle = "Danh sách xuất giữ";
    this.user = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.model.EmployeeId = this.user.employeeId;
    this.searchExportAndKeep();
  }

  showComfirmDelete(id) {
    this.messageService.showConfirm("Bạn có chắc xóa xuất giữ này không?").then(
      data => {
        this.delete(id);
      },
      error => {

      }
    );
  }

  delete(id) {
    this.service.deleteExportAndKeep(id).subscribe(
      data => {
        this.searchExportAndKeep();
        this.messageService.showSuccess('Xóa xuất giữ thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
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

    this.service.SearchExportAndKeep(this.model).subscribe((data: any) => {
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

  createExpoerAndKeep() {
    this.router.navigate(['kinh-doanh/danh-sach-xuat-giu/them-moi']);
  }

  updateExpoerAndKeep(id: string) {
    this.router.navigate(['kinh-doanh/danh-sach-xuat-giu/chinh-sua/' + id]);
  }

  showExportAndKeep(id: string) {
    this.router.navigate(['kinh-doanh/danh-sach-xuat-giu/xem/' + id]);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  showComfirmmanumitExportAndKeep(id: string) {
    this.messageService.showConfirm("Bạn có chắc giải phóng xuất giữ này không?").then(
      data => {
        this.manumitExportAndKeep(id);
      }
    );
  }

  manumitExportAndKeep(id: string) {
    this.service.ManumitExportAndKeep(id).subscribe(
      data => {
        this.messageService.showSuccess('Giải phóng xuất giữ thành công!');
        this.searchExportAndKeep();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showComfirSoldExportAndKeep(id: string) {
    this.messageService.showConfirm("Bạn có chắc cập nhật trạng thái sang đã bán không?").then(
      data => {
        this.SoldExportAndKeep(id);
      }
    );
  }

  SoldExportAndKeep(id: string) {
    this.service.SoldExportAndKeep(id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật trạng thái xuất giữ đã bán thành công!');
        this.searchExportAndKeep();

      },
      error => {
        this.messageService.showError(error);
      }
    );
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
      CreateDateToV: null,
      CreateDateFromV: null,
      ExpiredDateFromV: null,
      ExpiredDateToV: null,
      Quantity: '',
      QuantityType: 1,
      EmployeeId: null,
      // SupplierId: null
    }

    this.searchExportAndKeep();
  }
}
