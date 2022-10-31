import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, AppSetting, FileProcess, Constants } from 'src/app/shared';
import { SpecializeService } from 'src/app/practice/specialize/service/specialize.service';
import { ExpertService } from '../../service/expert.service';

@Component({
  selector: 'app-select-specialize',
  templateUrl: './select-specialize.component.html',
  styleUrls: ['./select-specialize.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SelectSpecializeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public fileProcess: FileProcess,
    public constants: Constants,
    public appset: AppSetting,
    private specializeService: SpecializeService,
    private expertService: ExpertService
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  ExpertId: string;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];
  ListExpertId: any = [];
  IsRequest: boolean;

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã chuyên môn',
    Items: [
      {
        Name: 'Tên chuyên môn',
        FieldName: 'Name',
        Placeholder: 'Nhập tên chuyên môn',
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
    EmployeeId: '',
    listData: [],
    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }

  ngOnInit() {
    this.modelSearch.ExpertId = this.ExpertId;
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchSpecialize();
  }

  searchSpecialize() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.expertService.searchSpecialize(this.modelSearch).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }


  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listData.indexOf(element);
      if (index > -1) {
        this.listData.splice(index, 1);
      }
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        this.listData.push(element);
      }
    });
    this.listData.forEach(element => {
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
    this.searchSpecialize();
  }


  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  checkAll(isCheck) {
    if (isCheck) {
      this.listData.forEach(element => {
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
