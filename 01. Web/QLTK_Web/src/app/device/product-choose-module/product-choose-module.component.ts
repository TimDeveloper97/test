import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductCreatesService } from '../services/product-create.service';
import { MessageService, Constants, ComboboxService } from 'src/app/shared';

@Component({
  selector: 'app-product-choose-module',
  templateUrl: './product-choose-module.component.html',
  styleUrls: ['./product-choose-module.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductChooseModuleComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private productCreatesService: ProductCreatesService,
    private messageService: MessageService,
    public constant: Constants,
    public comboboxService: ComboboxService,
  ) { }

  modelsearch: any = {
    Page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Code: '',
    Name: '',
    ListIdSelect: [],
    ListIdChecked: [],
  }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  listBase: any = [];
  listSelect: any = [];
  listIdSelect: any = [];
  listModuleGroup: any[] = [];
  ngOnInit() {
    this.listIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });
    this.searchModule();
  }

  searchModule() {
    this.listSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelsearch.ListIdChecked.push(element.Id);
      }
    });
    this.productCreatesService.searchModule(this.modelsearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
        element.Qty = 0;
      });
      this.modelsearch.TotalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên module',
        FieldName: 'Name',
        Placeholder: 'Nhập tên module ...',
        Type: 'text'
      },
      {
        Name: 'Nhóm module',
        FieldName: 'ModuleGroupId',
        Placeholder: 'Nhóm module',
        Type: 'select',
        DataType: this.constant.SearchDataType.ModuleGroup,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  clear() {
    this.modelsearch = {
      page: 1,
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Code: '',
      Name: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }

    this.listIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });

    this.searchModule();
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
        element.Qty = 0;
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

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal() {
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
