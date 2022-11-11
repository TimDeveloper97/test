import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, Constants } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductMaterialsService } from '../services/product-materials.service';

@Component({
  selector: 'app-product-materials-choose-tab',
  templateUrl: './product-materials-choose-tab.component.html',
  styleUrls: ['./product-materials-choose-tab.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductMaterialsChooseTabComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private service: ProductMaterialsService
  ) { }
  ProductId: string;
  editField: string;
  isAction: boolean = false;
  listData: any = [];
  modelSearch: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    ProductId: '',
    ManufactureName: '',
    RawMaterialCode: '',
    Quantity: '',
    Pricing: '',
    Note: '',
    listSelect: [],
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }

  listBase: any = [];
  listSelect: any = [];
  IsRequest: boolean;
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];

  ngOnInit() {
    this.modelSearch.ProductId = this.ProductId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchMaterial();
  }

  searchMaterial() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchMaterial(this.modelSearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear() {
    this.modelSearch = {
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Name: '',
      Code: '',
      Note: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.modelSearch.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.ListIdSelectRequest.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    } else {
      this.ListIdSelect.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    }
    this.searchMaterial();
  }
  addRow() {
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listBase.indexOf(element);
      if (index > -1) {
        this.listBase.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listBase.push(element);
      }
    });

    this.listBase.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  save() {
    this.modelSearch.listSelect = this.listSelect;
    this.service.addProductMaterials(this.modelSearch).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới vật tư  thành công!');
        this.activeModal.close(this.listData);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  CloseModal() {
    this.activeModal.close(false);
  }

}
