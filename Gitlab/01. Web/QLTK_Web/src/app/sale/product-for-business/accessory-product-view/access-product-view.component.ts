import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { Router } from '@angular/router';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { DxTreeListComponent } from 'devextreme-angular';
import { Configuration, MessageService, AppSetting, Constants, DateUtils } from 'src/app/shared';
import { SaleProductService } from '../sale-product.service';
@Component({
  selector: 'app-access-product-view',
  templateUrl: './access-product-view.component.html',
  styleUrls: ['./access-product-view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccessProductViewComponent implements OnInit {

  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public appSetting: AppSetting,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    public saleProductService: SaleProductService,
    public dateUtils: DateUtils,
  ) { }
  id: string;
  modalInfo = {
    Title: 'Danh sách phụ kiện',

  };
  startIndex = 0;
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên tiếng việt/tên tiếng anh',
    Items: [
      {
        Name: 'Model',
        FieldName: 'Model',
        Placeholder: 'Model',
        Type: 'text',
      },
      {
        Name: 'Mã nhóm thiết bị',
        FieldName: 'GroupCode',
        Placeholder: 'Mã nhóm thiết bị',
        Type: 'text',
      },
      {
        Name: 'Mã nhóm thiết bị con',
        FieldName: 'ChildGroupCode',
        Placeholder: 'Mã nhóm thiết bị con',
        Type: 'text',
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
        Placeholder: 'Chọn mã hãng sản xuất'
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
      // {
      //   Name: 'Ứng dụng',
      //   FieldName: 'Application',
      //   Placeholder: 'Chọn ứng dụng',
      //   Type: 'select',
      //   Data: this.constant.SendSale,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      // {
      //   Name: 'Ngành nghề',
      //   FieldName: 'Caree',
      //   Placeholder: 'Chọn ngành nghề',
      //   Type: 'select',
      //   Data: this.constant.SendSale,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      {
        Name: 'Thông số kỹ thuật',
        FieldName: 'Specifications',
        Placeholder: 'Thông số kỹ thuật',
        Type: 'text',
      },
      {
        Name: 'Giá vật tư',
        FieldName: 'MaterialPrice',
        Placeholder: 'Giá vật tư',
        FieldNameType: 'MaterialPriceType',
        Type: 'number',
      },
      {
        Name: 'VAT',
        FieldName: 'VAT',
        Placeholder: 'VAT',
        FieldNameType: 'MaterialPriceType',
        Type: 'number',
      },
      {
        Name: 'Giá bán EXW TPA',
        FieldName: 'EXWTPAPrice',
        Placeholder: 'Giá bán EXW TPA',
        FieldNameType: 'MaterialPriceType',
        Type: 'number',
      },
      {
        Name: 'Ngày cập nhập EXW TPA',
        FieldName: 'EXWTPADate',
        Placeholder: 'Ngày cập nhập giá EXW TPA',
        Type: 'date',
      },
      {
        Name: 'Giá trên Web',
        FieldName: 'PublicPrice',
        Placeholder: 'Giá trên Web',
        FieldNameType: 'MaterialPriceType',
        Type: 'number',
      },
      {
        Name: 'SL tồn kho',
        FieldName: 'Inventory',
        Placeholder: 'Số lượng tồn kho',
        Type: 'number',
      },
      {
        Name: 'Hiệu lực của gia từ ngày',
        FieldName: 'ExpireDateFrom',
        Placeholder: 'Hiệu lực của gia từ ngày',
        Type: 'date',
      },
      {
        Name: 'Hiệu lực của gia đến ngày',
        FieldName: 'ExpireDateTo',
        Placeholder: 'Hiệu lực của gia đến ngày',
        Type: 'date',
      },
      {
        Name: 'Thời gian đặt hàng',
        FieldName: 'DeliveryTime',
        Placeholder: 'Nhập thời gian đặt hàng',
        Type: 'number',
        FieldNameType: 'LastDeliveryType',
      },
      {
        Name: 'Ngày cập nhập tồn kho',
        FieldName: 'InventoryDate',
        Placeholder: 'Ngày cập nhập tồn kho',
        Type: 'date'
      },
      {
        Name: 'Số lượng xuất giữ',
        FieldName: 'ExportQuantity',
        Placeholder: 'số lượng xuất giữ',
        Type: 'number'
      },
    ]
  };
  ngOnInit() {
    this.getListAccessory();
  }

  getListAccessory() {
    this.saleProductService.searchAccessory(this.id, this.modelProduct).subscribe((data: any) => {
      if (this.modelProduct.EXWTPADateToV != null) {
        this.modelProduct.EXWTPADateTo = this.dateUtils.convertObjectToDate(this.modelProduct.EXWTPADateToV);
      }
      else{
        this.modelProduct.EXWTPADateTo = null;
      }
      if (this.modelProduct.EXWTPADateFromV != null) {
        this.modelProduct.EXWTPADateFrom = this.dateUtils.convertObjectToDate(this.modelProduct.EXWTPADateFromV)
      }
      else{
        this.modelProduct.EXWTPADateFrom = null;
      }

      if (this.modelProduct.ExpiredDateFromToV != null) {
        this.modelProduct.ExpiredDateFromTo = this.dateUtils.convertObjectToDate(this.modelProduct.ExpiredDateFromToV);
      }
      else{
        this.modelProduct.ExpiredDateFromTo = null;
      }
      if (this.modelProduct.ExpiredDateFromFromV != null) {
        this.modelProduct.ExpiredDateFromFrom = this.dateUtils.convertObjectToDate(this.modelProduct.ExpiredDateFromFromV)
      }
      else{
        this.modelProduct.ExpiredDateFromFrom = null;
      }

      if (this.modelProduct.ExpiredDateToToV != null) {
        this.modelProduct.ExpiredDateToTo = this.dateUtils.convertObjectToDate(this.modelProduct.ExpiredDateToToV);
      }
      else{
        this.modelProduct.ExpiredDateToTo = null;
      }
      if (this.modelProduct.ExpiredDateToFromV != null) {
        this.modelProduct.ExpiredDateToFrom = this.dateUtils.convertObjectToDate(this.modelProduct.ExpiredDateToFromV)
      }
      else{
        this.modelProduct.ExpiredDateToFrom = null;
      }

      if (this.modelProduct.InventoryDateToV != null) {
        this.modelProduct.InventoryDateTo = this.dateUtils.convertObjectToDate(this.modelProduct.InventoryDateToV);
      }
      else{
        this.modelProduct.InventoryDateTo = null;
      }
      if (this.modelProduct.InventoryDateFromV != null) {
        this.modelProduct.InventoryDateFrom = this.dateUtils.convertObjectToDate(this.modelProduct.InventoryDateFromV)
      }
      else{
        this.modelProduct.InventoryDateFrom = null;
      }
      if (data.ListResult) {
        this.startIndex = ((this.modelProduct.PageNumber - 1) * this.modelProduct.PageSize + 1);
        this.listProduct = data.ListResult;
        this.modelProduct.totalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }
  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  height = 0;
  pagination;
  modelProduct: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'EName',
    OrderType: true,
    ProductStandardTPATypeId: '',
    Name: '',
    Model: '',
    GroupCode: '',
    ChildGroupCode: '',
    ManufactureId: '',
    CountryName: '',
    Specifications: '',
    CustomerSpecifications: '',
    SpecificationDate: null,
    VAT: null,
    MaterialPrice: null,
    EXWTPAPrice: null,
    EXWTPADate: null,
    PublicPrice: null,
    ExpireDateFrom: null,
    ExpireDateTo: null,
    DeliveryTime: null,
    Inventory: null,
    InventoryDate: null,
    ExportQuantity: null,
    EXWTPADateToV: null,
    EXWTPADateFromV: null,
    ExpiredDateFromToV: null,
    ExpiredDateFromFromV: null,
    ExpiredDateToToV: null,
    ExpiredDateToFromV: null,
    InventoryDateToV: null,
    InventoryDateFromV: null
  };
  listProduct: any[] = []

  clear() {
    this.modelProduct = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'EName',
      OrderType: true,
      ProductStandardTPATypeId: '',
      Name: '',
      Model: '',
      GroupCode: '',
      ChildGroupCode: '',
      ManufactureId: '',
      CountryName: '',
      Specifications: '',
      CustomerSpecifications: '',
      SpecificationDate: null,
      VAT: null,
      MaterialPrice: null,
      EXWTPAPrice: null,
      EXWTPADate: null,
      PublicPrice: null,
      ExpireDateFrom: null,
      ExpireDateTo: null,
      DeliveryTime: null,
      Inventory: null,
      InventoryDate: null,
      ExportQuantity: null,
      EXWTPADateToV: null,
      EXWTPADateFromV: null,
      ExpiredDateFromToV: null,
      ExpiredDateFromFromV: null,
      ExpiredDateToToV: null,
      ExpiredDateToFromV: null,
      InventoryDateToV: null,
      InventoryDateFromV: null,
      EXWTPADateTo: null,
      EXWTPADateFrom: null,
      ExpiredDateFromTo: null,
      ExpiredDateFromFrom: null,
      ExpiredDateToTo: null,
      ExpiredDateToFrom: null,
      InventoryDateTo: null,
      InventoryDateFrom: null
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : false);
  }

}
