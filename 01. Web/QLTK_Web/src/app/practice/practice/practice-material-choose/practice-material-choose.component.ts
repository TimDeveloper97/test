import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { PracticeService } from '../../service/practice.service';
import { MessageService, Constants, AppSetting } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

@Component({
  selector: 'app-practice-material-choose',
  templateUrl: './practice-material-choose.component.html',
  styleUrls: ['./practice-material-choose.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PracticeMaterialChooseComponent implements OnInit {

  constructor(
    private router: Router,
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
    private service: PracticeService,
    public appSetting: AppSetting,

  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  PracticeId: string;
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

  ngOnInit() {
    this.appSetting.PageTitle = "Chỉnh sửa bài thực hành/công đoạn";
    this.modelSearch.PracticeId = this.PracticeId;
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
    this.service.searchMaterialPractice(this.modelSearch).subscribe(data => {
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

  choose() {
    var a = 0;
    for (var item of this.listSelect) {
      if (item.Quantity <= 0) {
        a++;
      }
    }
    if (a > 0) {
      this.messageService.showMessage("Bạn không được để trống số lượng vật tư")
    }
    else {
      this.activeModal.close(this.listSelect);
    }
  }

  CloseModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
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
