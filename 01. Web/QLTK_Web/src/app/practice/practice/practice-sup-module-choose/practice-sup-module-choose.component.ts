import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, Constants, AppSetting } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeService } from '../../service/practice.service';

@Component({
  selector: 'app-practice-sup-module-choose',
  templateUrl: './practice-sup-module-choose.component.html',
  styleUrls: ['./practice-sup-module-choose.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PracticeSupModuleChooseComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private service: PracticeService,
    public appSetting: AppSetting,
  ) { }

  practiceId: string;
  editField: string;
  isAction: boolean = false;
  checkedTop: boolean = false;
  checkedBot: boolean = false;
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
    PracticeId: '',
    ManufactureName: '',
    RawMaterialCode: '',
    Quantity: '',
    Pricing: '',
    Note: '',
    Leadtime: '',
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
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm tên/mã module ...',
    Items: [     
    ]
  };

  ngOnInit() {
    //this.appSetting.PageTitle = "Chỉnh sửa bài thực hành, giáo trình";
    this.modelSearch.PracticeId = this.practiceId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchModule();
  }

  searchModule() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchModule(this.modelSearch).subscribe(data => {
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
    this.searchModule();
  }

  addRow() {
    // this.listBase.forEach(element => {
    //   if (element.Checked) {
    //     element.Quantity = 1;
    //     this.listSelect.push(element);
    //   }
    // });
    // this.listSelect.forEach(element => {
    //   var index = this.listBase.indexOf(element);
    //   if (index > -1) {
    //     this.listBase.splice(index, 1);
    //   }
    // });

    for (let index = 0; index < this.listBase.length; index++) {
      if (this.listBase[index].Checked) {
        this.listBase[index].Quantity = 1;
        this.listSelect.push(this.listBase[index]);
        this.listBase.splice(index, 1);
        index--;
      }
    }
    this.searchModule();
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

  choose() {
    var a = 0;
    for (var item of this.listSelect) {
      if (item.Quantity <= 0) {
        a++;
      }
      item.Type = 2;
    }
    if (a > 0) {
      this.messageService.showMessage("Bạn không được để trống số lượng thiết bị")
    }
    else {
      this.service.getPriceModule(this.listSelect).subscribe(data => {
        if (data) {
          this.listSelect = data;
          this.activeModal.close(this.listSelect);
        }
      }, error => {
        this.messageService.showError(error);
      });
    }
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
