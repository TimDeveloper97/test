import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, FileProcess, AppSetting, Constants } from 'src/app/shared';
import { WorkPlaceService } from 'src/app/practice/workplace/service/work-place.service';
import { ExpertService } from '../../service/expert.service';

@Component({
  selector: 'app-select-work-place',
  templateUrl: './select-work-place.component.html',
  styleUrls: ['./select-work-place.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SelectWorkPlaceComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private workPlaceService: WorkPlaceService,
    private expertService: ExpertService,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  ExpertId: string;
  isAction: boolean = false;
  listSelect: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  ListExpertId: any = [];
  listBase: any = [];
  IsRequest: boolean;
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã địa chỉ công tác',
    Items: [
      {
        Name: 'Tên địa chỉ công tác',
        FieldName: 'Name',
        Placeholder: 'Nhập tên địa chỉ công tác',
        Type: 'text'
      },
    ]
  };
  modelSearch: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Code: '',
    Name: '',
    ExpertId: '',
    listBase: [],
    ListIdSelect: [],
    ListIdChecked: [],
  }

  ngOnInit() {
    this.modelSearch.ExpertId = this.ExpertId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchWorkPlace();
  }

  searchWorkPlace() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.expertService.searchWorkPlace(this.modelSearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
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

  clear() {
    this.modelSearch = {
      page: 1,
      PageSize: 10,
      TotalItem: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Code: '',
      Name: '',
      Description: '',
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
    this.searchWorkPlace();
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  checkAll(isCheck: any) {
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
