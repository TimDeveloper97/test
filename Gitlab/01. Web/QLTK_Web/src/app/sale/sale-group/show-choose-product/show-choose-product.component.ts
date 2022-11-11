import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService } from 'src/app/shared';
import { SaleGroupService } from '../service/sale-group.service';

@Component({
  selector: 'app-show-choose-product',
  templateUrl: './show-choose-product.component.html',
  styleUrls: ['./show-choose-product.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChooseProductComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private saleGroupService: SaleGroupService,
    private activeModal: NgbActiveModal,
    public constant: Constants
  ) { }

  listProduct = [];
  listProductId = [];
  listProductSelect = [];
  isAction: boolean = false;
  checkedTop = false;
  checkedBot = false;

  modelSearch: any = {
    Code: '',
    Name: '',
    SaleProductTypeId:'',
    IsChoose: null
  }

  searchOptions = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên sản phẩm/ mã sản phẩm...',
    Items: [
      {
        Placeholder: 'Chọn chủng loại',
        Name: 'Chủng loại',
        FieldName: 'SaleProductTypeId',
        Type: 'dropdowntree',
        SelectMode: 'single',
        ParentId: 'ParentId',
        DataType: this.constant.SearchDataType.SaleProductType,
        Columns: [ { Name: 'Name', Title: 'Tên chủng loại' }],
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Tình trạng chọn',
        FieldName: 'IsChoose',
        Placeholder: 'Chọn tình trạng',
        Type: 'select',
        Data: this.constant.SaleProductIsChoose,
        DisplayName: 'Name',
        ValueName: 'Id'
      }
    ]
  }

  ngOnInit() {
    this.modelSearch.ListProductId = this.listProductId;
    this.searchProduct();
  }

  searchProduct() {
    this.listProductSelect.forEach(item => {
      this.modelSearch.ListProductId.push(item.Id);
    })
    this.saleGroupService.getListProduct(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listProduct = data;
        this.listProduct.forEach((element, index) => {
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
      this.listProduct.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listProductSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow() {
    this.listProduct.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listProductSelect.push(element);
      }
    });
    this.listProductSelect.forEach(element => {
      var index = this.listProduct.indexOf(element);
      if (index > -1) {
        this.listProduct.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listProductSelect.forEach(element => {
      if (element.Checked) {
        this.listProduct.push(element);
      }
    });
    this.listProduct.forEach(element => {
      var index = this.listProductSelect.indexOf(element);
      if (index > -1) {
        this.listProductSelect.splice(index, 1);
      }
    });
  }

  choose() {
    this.activeModal.close(this.listProductSelect);
  }

  clear() {
    this.modelSearch = {
      Name: '',
      Code: '',
      Email: '',
      ListProductId: [],
    }
    this.modelSearch.ListProductId = this.listProductId;
    this.searchProduct();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
