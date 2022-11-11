import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, MessageService } from 'src/app/shared';
import { SaleProductService } from '../sale-product.service';

@Component({
  selector: 'app-choose-group-product-sale',
  templateUrl: './choose-group-product-sale.component.html',
  styleUrls: ['./choose-group-product-sale.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseGroupProductSaleComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private modalService: NgbModal,
    private activeModal: NgbActiveModal,
    public config: Configuration,
    public saleProductService: SaleProductService,
  ) { }
  listGroupProduct: any[]=[];

  listGroupSelect = [];
  listGroupProductId: any = [];
  checkedTop = false;
  checkedBot = false;
  isAction: boolean = false;

  modelSearch: any ={
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Name: '',
    Note: '',
    ListIdSelect:[],
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhóm',
    Items: [
      {
        Name: 'Ghi chú',
        FieldName: 'Note',
        Placeholder: 'Nhập nội dung ghi chú',
        Type: 'text'
      },
    ]
  };

  ngOnInit() {
    this.modelSearch.ListIdSelect = this.listGroupProductId;
    this.searchGroupProduct();
  }

  imagePath: string;
  searchGroupProduct() {
    this.saleProductService.SearchGroupProduct(this.modelSearch).subscribe((data: any) => {
      if (data) {
        this.listGroupProduct = data.ListResult;
        this.listGroupProduct.forEach((element, index) => {
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
      this.listGroupProduct.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listGroupSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }

  addRow() {
    this.listGroupProduct.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listGroupSelect.push(element);
      }
    });
    this.listGroupSelect.forEach(element => {
      var index = this.listGroupProduct.indexOf(element);
      if (index > -1) {
        this.listGroupProduct.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listGroupSelect.forEach(element => {
      if (element.Checked) {
        this.listGroupProduct.push(element);
      }
    });
    this.listGroupProduct.forEach(element => {
      var index = this.listGroupSelect.indexOf(element);
      if (index > -1) {
        this.listGroupSelect.splice(index, 1);
      }
    });
  }

  choose() {
    this.activeModal.close(this.listGroupSelect);
  }

  clear() {
    this.modelSearch = {
      Name: '',
      ListIdSelectL:[],
    }
    this.modelSearch.ListIdSelect = this.listGroupProductId;
    this.searchGroupProduct();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
