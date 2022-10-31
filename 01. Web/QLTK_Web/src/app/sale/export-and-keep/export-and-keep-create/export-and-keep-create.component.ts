import { CdkDragDrop, CdkDragStart, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { ParsedEvent } from '@angular/compiler';
import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, OnDestroy, AfterViewInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as printJS from 'print-js';
import { forkJoin } from 'rxjs';
import { timeout } from 'rxjs/operators';
import { AppSetting, ComboboxService, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { ProductService } from '../../../device/services/product.service';
import { SaleCustomerCreateComponent } from '../customer-create/customer-create.component';
import { ExportAndKeepService } from '../service/export-and-keep.service';

@Component({
  selector: 'app-export-and-keep-create',
  templateUrl: './export-and-keep-create.component.html',
  styleUrls: ['./export-and-keep-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ExportAndKeepCreateComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    public constant: Constants,
    public config: Configuration,
    private productService: ProductService,
    private messageService: MessageService,
    public combobox: ComboboxService,
    private activeModal: NgbActiveModal,
    public service: ExportAndKeepService,
    private modalService: NgbModal,
    public dateUtil: DateUtils,
    public router: Router,
    private routeA: ActivatedRoute,
    public appSetting: AppSetting,

  ) { }

  model: any = {
    PayStatus: 1,
    PaymentPercent: 0,
    PaymentAmount: 0
  }

  isAction: boolean = false;
  ExpiredDateV: any;
  minDateNotificationV: any;

  modelSearchProduct: any = {
    PageSize: 20,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    SaleProductTypeId: null,
    NameCode: '',
    Inventory: null,
    InventoryType: null,
    AvailableQuantity: null,
    AvailableQuantityType: null,
    CountryName: null,
    ManufactureId: null,
    EXWTPAPrice: null,
    EXWTPAPriceType: null,

    ListIdSelect: []
  }
  saleProductTypeId = null;
  startIndex = 1;
  startIndexCheck = 1;
  listProduct = [];
  listProductCheck = [];
  listEmployee = [];
  listCustomer = [];
  height = 0;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }];

  searchOptions: any = {
    FieldContentName: 'NameCode',
    Placeholder: 'Tìm kiếm theo mã/tên thiết bị/thông số',
    Items: [
      {
        Name: 'Số lượng tồn',
        FieldName: 'Inventory',
        Placeholder: 'Số lượng tồn',
        FieldNameType: 'InventoryType',
        Type: 'number',
      },
      {
        Name: 'Số lượng khả dụng',
        FieldName: 'AvailableQuantity',
        Placeholder: 'Số lượng khả dụng',
        FieldNameType: 'AvailableQuantityType',
        Type: 'number',
      },
      {
        Name: 'Xuất xứ',
        FieldName: 'CountryName',
        Placeholder: 'Xuất xứ',
        Type: 'select',
        DataType: this.constant.SearchDataType.Country,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Hãng sản xuất',
        FieldName: 'ManufactureId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manuafacture,
        Columns: [{ Name: 'Code', Title: 'Mã hãng sản xuất' }, { Name: 'Name', Title: 'Tên hãng sản xuất' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn hãng sản xuất'
      },
      {
        Name: 'Giá',
        FieldName: 'EXWTPAPrice',
        Placeholder: 'Nhập giá',
        FieldNameType: 'EXWTPAPriceType',
        Type: 'number',
      },
      // {
      //   Name: 'Tình trạng',
      //   FieldName: 'Status',
      //   Placeholder: 'Tình trạng',
      //   Type: 'select',
      //   Data: this.constant.ProductStatus,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      // {
      //   Name: 'Tình trạng dữ liệu',
      //   FieldName: 'IsEnought',
      //   Placeholder: 'Tình trạng dữ liệu',
      //   Type: 'select',
      //   Data: this.constant.ProductIsEnought,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      // {
      //   Name: 'SBU',
      //   FieldName: 'SBUId',
      //   Placeholder: 'SBU',
      //   Type: 'select',
      //   DataType: this.constant.SearchDataType.SBU,
      //   DisplayName: 'Name',
      //   ValueName: 'Id',
      //   IsRelation: true,
      //   RelationIndexTo: 3,
      //   Permission: ['F030405'],
      // },
      // {
      //   Name: 'Phòng ban',
      //   FieldName: 'DepartmentId',
      //   Placeholder: 'Phòng ban',
      //   Type: 'select',
      //   DataType: this.constant.SearchDataType.Department,
      //   DisplayName: 'Name',
      //   ValueName: 'Id',
      //   RelationIndexFrom: 2,
      //   Permission: ['F030405'],
      // },
      // {
      //   Name: 'Chuyển thư viện',
      //   FieldName: 'IsSendSale',
      //   Placeholder: 'Chọn tình trạng chuyển thư viện',
      //   Type: 'select',
      //   Data: this.constant.SendSale,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
    ]
  };

  modelCustomer: any = {
    Id: ''
  }

  listType = 1;
  listProductStandTPAType = [];

  user: any;
  id: string;
  idProduct: string;
  productChooseHeight = 250;
  productHeight = 350;

  @ViewChild('scrollProducTable', { static: false }) scrollProducTable: ElementRef;
  @ViewChild('scrollProducTableHeader', { static: false }) scrollProducTableHeader: ElementRef;
  @ViewChild('scrollProducChose', { static: false }) scrollProducChose: ElementRef;
  @ViewChild('scrollProducChoseHeader', { static: false }) scrollProducChoseHeader: ElementRef;
  isLoad = false;
  ngOnInit() {
    this.appSetting.PageTitle = "Thêm mới xuất giữ";
    this.user = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.id = this.routeA.snapshot.paramMap.get('id');
    this.calculateHeight();

    this.searchProductStandTPAType();
    if (this.id) {
      this.isLoad = true;

      this.appSetting.PageTitle = "Chỉnh sửa xuất giữ";
      forkJoin([
        this.service.GetListCustomers(),
        this.service.GetInfoByIdExportAndKeep(this.id),
      ]).subscribe(([res1, res2]) => {
        this.user = null;
        this.listCustomer = res1;
        if (res2) {
          this.listProductCheck = res2.ListExportAndKeepDetail;
          this.user = res2;
          var index = 1;
          this.listProductCheck.forEach(element => {
            element.index = index++;
          });
          this.model = res2;
          this.ExpiredDateV = this.dateUtil.convertDateToObject(this.model.ExpiredDate);
          this.changeCustomer();
          this.searchProduct();
          this.isLoad = false;
        }
      });
    }
    else {
      this.getListCustomers(false);
      this.GenerateCode();
      this.searchProduct();
      this.idProduct = this.routeA.snapshot.paramMap.get('idProduct');
      if (this.idProduct) {
        this.getProductById();
      }
    }
  }

  ngAfterViewInit() {
    this.scrollProducChose.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollProducChoseHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    if (this.scrollProducTable) {
      this.scrollProducTable.nativeElement.removeEventListener('ps-scroll-x', null);
    }
    if (this.scrollProducChose) {
      this.scrollProducChose.nativeElement.removeEventListener('ps-scroll-x', null);
    }
  }

  calculateHeight() {
    if (window.innerHeight <= 768) {
      this.productChooseHeight = 170;
    }

    this.height = window.innerHeight - this.productChooseHeight - 230;
    this.productHeight = window.innerHeight - this.productChooseHeight - 310;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event) {
    this.calculateHeight();
  }

  getInfoExportAnhKeep() {
    this.service.GetInfoByIdExportAndKeep(this.id).subscribe(item => {
      if (item) {
        this.listProductCheck = item.ListExportAndKeepDetail;
        this.model = item;
        this.user = item;
        this.ExpiredDateV = this.dateUtil.convertDateToObject(this.model.ExpiredDate);
        this.changeCustomer();
      }
    });
  }

  GenerateCode() {
    this.service.GenerateCode().subscribe((data: any) => {
      if (data) {
        this.model.Code = data.Code;
      }
    });
  }

  changeListType(value) {
    if (value == 2 && this.listType != value) {
      this.listType = value;
      setTimeout(() => {
        if (this.scrollProducTable) {
          this.scrollProducTable.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
            this.scrollProducTableHeader.nativeElement.scrollLeft = event.target.scrollLeft;
          }, true);
        }
      }, 500);

    } else {
      this.listType = value;
      if (this.scrollProducTable) {
        this.scrollProducTable.nativeElement.removeEventListener('ps-scroll-x', null);
      }
    }

  }

  changeTotal() {
    this.listProductCheck.forEach(item => {
      if (parseInt(item.Quantity) > parseInt(item.Inventory)) {
        this.messageService.showMessage("Số lượng xuất giữ không được lớn hơn số lượng tồn kho!");
        item.Quantity = null;
      }
    })
  }

  searchProductStandTPAType() {
    this.combobox.getCBBSaleProductType().subscribe(item => {
      this.listProductStandTPAType = item;
      this.listProductStandTPAType.unshift({ Id: '', Name: 'Tất cả' })
    })
  }

  searchProduct() {
    this.modelSearchProduct.SaleProductTypeId = this.saleProductTypeId;
    this.modelSearchProduct.ListIdSelect = [];
    if (this.idProduct) {
      this.modelSearchProduct.ListIdSelect.push(this.idProduct);
    }

    this.listProductCheck.forEach(item => {
      this.modelSearchProduct.ListIdSelect.push(item.Id)
    })

    this.service.GetSaleProducts(this.modelSearchProduct).subscribe((data: any) => {
      if (data) {
        this.startIndex = ((this.modelSearchProduct.PageNumber - 1) * this.modelSearchProduct.PageSize + 1);
        this.listProduct = data.ListResult;
        this.modelSearchProduct.TotalItems = data.TotalItem;
      }
    }, error => {
    });
  }

  getProductById() {
    this.service.GetSaleProductById(this.idProduct).subscribe((data: any) => {
      if (data) {
        this.listProductCheck.push(data);
      }
    }, error => {
    });
  }

  changeCustomer() {
    this.listCustomer.forEach(item => {
      if (item.Id == this.model.CustomerId) {
        this.modelCustomer = item;
        if (!this.isLoad) {
          this.model.PhoneNumber = item.PhoneNumber;
          this.model.Address = item.Address;
        }
        return;
      }
    })
  }

  getListCustomers(isChange) {
    this.service.GetListCustomers().subscribe(item => {
      this.listCustomer = item;

      if (isChange) {
        this.changeCustomer();
      }
    })
  }

  showConfirmDelete(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá sản phẩm này không?").then(
      data => {
        this.delete(row);
      },
      error => {

      }
    );
  }

  delete(row) {
    var index = this.listProductCheck.indexOf(row);
    if (index > -1) {
      this.listProductCheck.splice(index, 1);
      this.listProduct.push(row);
      // this.modelSearchProduct.TotalItems = this.listProduct.length;
      this.searchProduct();
    }
  }

  clearProductCheck() {
    this.modelSearchProduct = {
      PageSize: 21,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      SaleProductTypeId: null,
      NameCode: '',
      Inventory: null,
      InventoryType: null,
      AvailableQuantity: null,
      AvailableQuantityType: null,
      CountryName: null,
      ManufactureId: null,
      EXWTPAPrice: null,
      EXWTPAPriceType: null,

      ListIdSelect: []
    }
    this.searchProduct();
  }

  showCustomerCreate() {
    let activeModal = this.modalService.open(SaleCustomerCreateComponent, { container: 'body', windowClass: 'customer-create-model', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result) {
        this.model.CustomerId = result;
        this.getListCustomers(true);
      }
    }, (reason) => {
    });
  }

  changpushlistproducrcheck(item: any) {
    item.index = this.listProductCheck.length + 1;
    this.listProductCheck.push(item);
    // var vaule = this.listProduct.indexOf(item);
    // this.listProduct.splice(vaule, 1);
    // this.modelSearchProduct.TotalItems = this.listProduct.length;
    this.searchProduct();
  }

  update() {
    this.service.UpdateExportAndKeep(this.id, this.model).subscribe(
      data => {
        this.messageService.showSuccess('Chỉnh sửa xuất giữ thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {

    this.service.CreateExportAndKeep(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới xuất giữ thành công!');

        if (!isContinue) {
          this.closeModal(true);
        }
        else {
          this.model = {

          }
          this.ExpiredDateV = null;
          this.listProductCheck = [];
          this.modelCustomer = {};
          this.searchProduct();
          this.GenerateCode();
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue) {

    if (this.ExpiredDateV) {
      this.model.ExpiredDate = this.dateUtil.convertObjectToDate(this.ExpiredDateV);
    }

    this.model.ListExportAndKeepDetail = this.listProductCheck;

    var dateNow = new Date();

    var expiredDate = new Date(this.model.ExpiredDate);
    expiredDate.setHours(23, 59, 59, 999);

    if (this.id) {
      this.update();
    }
    else {
      if (expiredDate <= dateNow) {
        this.messageService.showMessage("Ngày hết hạn phải lớn hơn ngày hiện tại!");
      }
      else {
        this.create(isContinue);
      }
    }

  }

  closeModal(isOK: boolean) {
    this.router.navigate(['kinh-doanh/danh-sach-xuat-giu']);
  }

  productStandTPATypeSelectId: string;

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.productStandTPATypeSelectId) {
      this.productStandTPATypeSelectId = e.selectedRowKeys[0];
      this.saleProductTypeId = e.selectedRowKeys[0]
      this.searchProduct();
    }
  }

  showComfirmmanumitExportAndKeep() {
    this.messageService.showConfirm("Bạn có chắc giải phóng xuất giữ này không?").then(
      data => {
        this.manumitExportAndKeep();
      }
    );
  }

  manumitExportAndKeep() {
    this.service.ManumitExportAndKeep(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Giải phóng xuất giữ thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showComfirSoldExportAndKeep() {
    this.messageService.showConfirm("Bạn có chắc cập nhật trạng thái sang đã bán không?").then(
      data => {
        this.SoldExportAndKeep();
      }
    );
  }

  SoldExportAndKeep() {
    this.service.SoldExportAndKeep(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật trạng thái xuất giữ đã bán thành công!');
        this.closeModal(true);

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  printPdf() {
    this.service.PrintCustomer(this.id).subscribe((data) => {
      var blob = new Blob([data], { type: "application/pdf" });
      var url = window.URL.createObjectURL(blob);
      this.appSetting.PrintjsPdf(url);
    }, error => {
      this.messageService.showMessageErrorBlob(error);
    });
  }
}
