import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { DocumentService } from '../services/document.service';

@Component({
  selector: 'app-choose-work',
  templateUrl: './choose-work.component.html',
  styleUrls: ['./choose-work.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseWorkComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private documentService: DocumentService,) { }

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
    ListIdSelect: [],
    Code: '',
    FlowStageId: '',
    Type: null,
    IsDesignModule: null,
    SBUId: '',
    DepartmentId: ''
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Mã công việc',
        FieldName: 'Code',
        Placeholder: 'Nhập mã công việc ...',
        Type: 'text'
      },
      // {
      //   Name: 'Loại công việc',
      //   FieldName: 'Type',
      //   Placeholder: 'Loại công việc',
      //   Type: 'select',
      //   Data: this.constants.TaskTypes,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      // {
      //   Name: 'Thiết kế module nguồn',
      //   FieldName: 'IsDesignModule',
      //   Placeholder: 'Thiết kế module nguồn',
      //   Type: 'select',
      //   Data: this.constants.TaskIsDesignModule,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constants.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 2,
        // Permission: ['F030405'],
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constants.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 1,
        // Permission: ['F030405'],
      },
      // {
      //   Name: 'Trình độ',
      //   FieldName: 'DepartmentId',
      //   Placeholder: 'Phòng ban',
      //   Type: 'select',
      //   DataType: this.constants.SearchDataType.Department,
      //   DisplayName: 'Name',
      //   ValueName: 'Id',
      //   RelationIndexFrom: 3,
      //   // Permission: ['F030405'],
      // },
    ]
  };

  ngOnInit(): void {
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchWork();
  }

  searchWork() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    //this.materialService.searchMaterial(this.modelSearch).subscribe(data => {
    this.documentService.searchTask(this.modelSearch).subscribe(data => {
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
    this.searchWork();
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
