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
  columnName: any[] = [{ Name: 'Code', Title: 'M?? kh??ch h??ng' }, { Name: 'Name', Title: 'T??n kh??ch h??ng' }];

  searchOptions: any = {
    FieldContentName: 'NameCode',
    Placeholder: 'T??m ki???m theo m??/t??n thi???t b???/th??ng s???',
    Items: [
      {
        Name: 'S??? l?????ng t???n',
        FieldName: 'Inventory',
        Placeholder: 'S??? l?????ng t???n',
        FieldNameType: 'InventoryType',
        Type: 'number',
      },
      {
        Name: 'S??? l?????ng kh??? d???ng',
        FieldName: 'AvailableQuantity',
        Placeholder: 'S??? l?????ng kh??? d???ng',
        FieldNameType: 'AvailableQuantityType',
        Type: 'number',
      },
      {
        Name: 'Xu???t x???',
        FieldName: 'CountryName',
        Placeholder: 'Xu???t x???',
        Type: 'select',
        DataType: this.constant.SearchDataType.Country,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'H??ng s???n xu???t',
        FieldName: 'ManufactureId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manuafacture,
        Columns: [{ Name: 'Code', Title: 'M?? h??ng s???n xu???t' }, { Name: 'Name', Title: 'T??n h??ng s???n xu???t' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Ch???n h??ng s???n xu???t'
      },
      {
        Name: 'Gi??',
        FieldName: 'EXWTPAPrice',
        Placeholder: 'Nh???p gi??',
        FieldNameType: 'EXWTPAPriceType',
        Type: 'number',
      },
      // {
      //   Name: 'T??nh tr???ng',
      //   FieldName: 'Status',
      //   Placeholder: 'T??nh tr???ng',
      //   Type: 'select',
      //   Data: this.constant.ProductStatus,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      // {
      //   Name: 'T??nh tr???ng d??? li???u',
      //   FieldName: 'IsEnought',
      //   Placeholder: 'T??nh tr???ng d??? li???u',
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
      //   Name: 'Ph??ng ban',
      //   FieldName: 'DepartmentId',
      //   Placeholder: 'Ph??ng ban',
      //   Type: 'select',
      //   DataType: this.constant.SearchDataType.Department,
      //   DisplayName: 'Name',
      //   ValueName: 'Id',
      //   RelationIndexFrom: 2,
      //   Permission: ['F030405'],
      // },
      // {
      //   Name: 'Chuy???n th?? vi???n',
      //   FieldName: 'IsSendSale',
      //   Placeholder: 'Ch???n t??nh tr???ng chuy???n th?? vi???n',
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
    this.appSetting.PageTitle = "Th??m m???i xu???t gi???";
    this.user = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    this.id = this.routeA.snapshot.paramMap.get('id');
    this.calculateHeight();

    this.searchProductStandTPAType();
    if (this.id) {
      this.isLoad = true;

      this.appSetting.PageTitle = "Ch???nh s???a xu???t gi???";
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
        this.messageService.showMessage("S??? l?????ng xu???t gi??? kh??ng ???????c l???n h??n s??? l?????ng t???n kho!");
        item.Quantity = null;
      }
    })
  }

  searchProductStandTPAType() {
    this.combobox.getCBBSaleProductType().subscribe(item => {
      this.listProductStandTPAType = item;
      this.listProductStandTPAType.unshift({ Id: '', Name: 'T???t c???' })
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
    this.messageService.showConfirm("B???n c?? ch???c mu???n xo?? s???n ph???m n??y kh??ng?").then(
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
        this.messageService.showSuccess('Ch???nh s???a xu???t gi??? th??nh c??ng!');
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
        this.messageService.showSuccess('Th??m m???i xu???t gi??? th??nh c??ng!');

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
        this.messageService.showMessage("Ng??y h???t h???n ph???i l???n h??n ng??y hi???n t???i!");
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
    this.messageService.showConfirm("B???n c?? ch???c gi???i ph??ng xu???t gi??? n??y kh??ng?").then(
      data => {
        this.manumitExportAndKeep();
      }
    );
  }

  manumitExportAndKeep() {
    this.service.ManumitExportAndKeep(this.id).subscribe(
      data => {
        this.messageService.showSuccess('Gi???i ph??ng xu???t gi??? th??nh c??ng!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showComfirSoldExportAndKeep() {
    this.messageService.showConfirm("B???n c?? ch???c c???p nh???t tr???ng th??i sang ???? b??n kh??ng?").then(
      data => {
        this.SoldExportAndKeep();
      }
    );
  }

  SoldExportAndKeep() {
    this.service.SoldExportAndKeep(this.id).subscribe(
      data => {
        this.messageService.showSuccess('C???p nh???t tr???ng th??i xu???t gi??? ???? b??n th??nh c??ng!');
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
