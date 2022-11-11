import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { OutputResultService } from '../../service/output-result.service';

@Component({
  selector: 'app-choose-document',
  templateUrl: './choose-document.component.html',
  styleUrls: ['./choose-document.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseDocumentComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private outputResultService: OutputResultService,) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  ClassRoomId: string;
  isAction: boolean = false;
  listSelect: any = [];
  ListIdSelect: any = [];
  ListIdSelectRequest: any = [];

  listData: any = [];
  IsRequest: boolean;

  modelSearch: any = {
    TotalItem: 0,
    Name: '',
    Code: '',
    DocumentGroupId: '',
    ListIdSelect: [],
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên ...',
    Items: [
      {
        Name: 'Mã tài liệu',
        FieldName: 'Code',
        Placeholder: 'Nhập mã tài liệu',
        Type: 'text'
      },
      {
        Name: 'Nhóm tài liệu',
        FieldName: 'DocumentGroupId',
        Placeholder: 'Nhóm tài liệu',
        Type: 'select',
        DataType: this.constants.SearchDataType.DocumentGroup,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  ngOnInit(): void {
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.search();
  }

  search() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.outputResultService.searchDocument(this.modelSearch).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItem = data.TotalItem;
    }, error => {
      this.messageService.showError(error);
    })
  }

  checkAll(isCheck: any) {
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

  addRow() {
    this.listData.forEach(element => {
      if (element.Checked) {
        element.Type = 3;
        element.Quantity = 1;
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
      TotalItem: 0,
      Name: '',
      ListIdSelect: [],
      SBUId: '',
      DepartmentId: ''
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
    this.search();
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
