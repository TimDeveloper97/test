import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, MessageService, Constants, DateUtils } from 'src/app/shared';
import { SaleProductService } from '../sale-product.service';

@Component({
  selector: 'app-choose-accessory',
  templateUrl: './choose-accessory.component.html',
  styleUrls: ['./choose-accessory.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseAccessoryComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public constant: Constants,
    public saleProductService: SaleProductService,
    public dateUtils: DateUtils,
  ) { }
  listAccessory: any[] = [];

  listAccessorySelect = [];
  listAccessoryId: any = [];
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;

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
        FieldNameTo: 'EXWTPADateToV',
        FieldNameFrom: 'EXWTPADateFromV',
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
        Name: 'Hiệu lực của giá từ ngày',
        FieldNameTo: 'ExpiredDateFromToV',
        FieldNameFrom: 'ExpiredDateFromFromV',
        Type: 'date',
      },
      {
        Name: 'Hiệu lực của giá đến ngày',
        FieldNameTo: 'ExpiredDateToToV',
        FieldNameFrom: 'ExpiredDateToFromV',
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
        FieldNameTo: 'InventoryDateToV',
        FieldNameFrom: 'InventoryDateFromV',
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
  modelProduct: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'VName',
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
    ListIdSelect: [],
    EXWTPADateToV: null,
    EXWTPADateFromV: null,
    ExpiredDateFromToV: null,
    ExpiredDateFromFromV: null,
    ExpiredDateToToV: null,
    ExpiredDateToFromV: null,
    InventoryDateToV: null,
    InventoryDateFromV: null
  };
  ModalInfo = {
    Title: 'Chọn phụ kiện',
    SaveText: 'Lưu',
  };
  ngOnInit() {
    this.modelProduct.ListIdSelect = this.listAccessoryId;
    this.searchAccessory();
  }

  searchAccessory() {
    this.saleProductService.getAllSaleProduct(this.modelProduct).subscribe((data: any) => {
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
      if (data) {
        this.listAccessory = data.ListResult;
        this.modelProduct.totalItems = data.TotalItem;
        this.listAccessory.forEach((element, index) => {
          element.Index = index + 1;
        });
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  checkAll(isCheck: any) {
    if (isCheck) {
      this.listAccessory.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listAccessorySelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow() {
    this.listAccessory.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listAccessorySelect.push(element);
      }
    });
    this.listAccessorySelect.forEach(element => {
      var index = this.listAccessory.indexOf(element);
      if (index > -1) {
        this.listAccessory.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listAccessorySelect.forEach(element => {
      if (element.Checked) {
        this.listAccessory.push(element);
      }
    });
    this.listAccessory.forEach(element => {
      var index = this.listAccessorySelect.indexOf(element);
      if (index > -1) {
        this.listAccessorySelect.splice(index, 1);
      }
    });
  }

  choose() {
    this.activeModal.close(this.listAccessorySelect);
  }

  clear() {
    this.modelProduct = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'VName',
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
      ListIdSelect: [],
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
    this.modelProduct.ListIdSelect = this.listAccessoryId;
    this.searchAccessory();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
