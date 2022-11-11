import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, Constants } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductAccessoriesService } from '../services/product-accessories.service';

@Component({
  selector: 'app-productaccessorieschoose',
  templateUrl: './productaccessorieschoose.component.html',
  styleUrls: ['./productaccessorieschoose.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductaccessorieschooseComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private service: ProductAccessoriesService,
    private constant: Constants,

  ) { }

  start = 1;
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  editField: string;
  isAction: boolean = false;
  ProductId: string;

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

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã phụ kiện',
    Items: [
      {
        Name: 'Tên phụ kiện',
        FieldName: 'Name',
        Placeholder: 'Nhập tên phụ kiện',
        Type: 'text'
      },
      {
        Name: 'Mã hãng sản xuất',
        FieldName: 'ManufactureId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manuafacture,
        Columns: [{ Name: 'Code', Title: 'Mã hãng sản xuất' }, { Name: 'Name', Title: 'Tên hãng sản xuất' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn mã hãng sản xuất'
      },
    ]
  };

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
      TestCriteriaGroupId: '',
      TechnicalRequirements: '',
      Note: '',
      ListTestCriteriaModule: [],

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
    // this.modelSearch.listSelect = this.listSelect;
    // this.service.addProductAccessories(this.modelSearch).subscribe(
    //   data => {
    //     this.messageService.showSuccess('Thêm mới phụ kiện thành công!');
    //     this.activeModal.close();
    //   },
    //   error => {
    //     this.messageService.showError(error);
    //   }
    // );
    this.activeModal.close(this.listSelect);
  }

  CloseModal() {
    this.activeModal.close(false);
  }

  checkAll(isCheck) {
    if (isCheck) {
      this.listBase.forEach(element => {
        if (this.checkedTop) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    } else {
      this.listSelect.forEach(element => {
        if (this.checkedBot) {
          element.Checked = true;
        } else {
          element.Checked = false;
        }
      });
    }
  }
}
