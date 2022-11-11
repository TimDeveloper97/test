import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, Constants, AppSetting } from 'src/app/shared';
import { ClassRoomService } from '../../service/class-room.service';

@Component({
  selector: 'app-show-select-product',
  templateUrl: './show-select-product.component.html',
  styleUrls: ['./show-select-product.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowSelectProductComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private classRoomService: ClassRoomService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  ClassRoomId: string;
  isAction: boolean = false;
  listSelect: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];

  ListProduct: any = [];
  IsRequest: boolean;
  modelSearch: any = {
    TotalItem: 0,

    Id: '',
    Code: '',
    Name: '',
    ProductGroupId: '',
    ListIdSelect: [],
    ListIdChecked: [],
    Quantity:'1'
  }

  ngOnInit() {
    this.modelSearch.ClassRoomId = this.ClassRoomId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchProduct();
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên thiết bị',
        FieldName: 'Name',
        Placeholder: 'Nhập tên thiết bị ...',
        Type: 'text'
      },
      {
        Name: 'Nhóm thiết bị',
        FieldName: 'ProductGroupId',
        Placeholder: 'Nhóm thiết bị',
        Type: 'select',
        DataType: this.constants.SearchDataType.ProductGroup,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  searchProduct() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.ListProduct.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    //this.materialService.searchMaterial(this.modelSearch).subscribe(data => {
    this.classRoomService.searchProduct(this.modelSearch).subscribe(data => {
      this.ListProduct = data.ListResult;
      this.ListProduct.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  checkAll(isCheck: any) {
    if (isCheck) {
      this.ListProduct.forEach(element => {
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

  addRow() {
    this.ListProduct.forEach(element => {
      if (element.Checked) {
        element.Quantity = 1;
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.ListProduct.indexOf(element);
      if (index > -1) {
        this.ListProduct.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.ListProduct.push(element);
      }
    });
    this.ListProduct.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
  }

  clear() {
    this.modelSearch = {
      TotalItem: 0,
      Id: '',
      Code: '',
      Name: '',
      ProductGroupId: '',
      listData: [],
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
    this.searchProduct();
  }

  choose() {
    var a = 0;
    for (var item of this.listSelect) {
      if (item.Quantity <= 0) {
        a++;
      }
    }
    if (a > 0) {
      this.messageService.showMessage("Bạn không được để trống số lượng thiết bị")
    }
    else {
      this.classRoomService.GetPriceProduct(this.listSelect).subscribe(data => {
        if (data) {
          this.listSelect = data;
          this.activeModal.close(this.listSelect);
        }
      }, error => {
        this.messageService.showError(error);
      });
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
