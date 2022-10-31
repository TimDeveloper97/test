import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { DocumentService } from '../services/document.service';

@Component({
  selector: 'app-choose-document-reference',
  templateUrl: './choose-document-reference.component.html',
  styleUrls: ['./choose-document-reference.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseDocumentReferenceComponent implements OnInit {

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
    Id: '',
    Code: '',
    Name: '',
    DocumentGroupId: '',
    ListIdSelect: [],
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo Tên/ Mã',
    Items: [
      {
        Name: 'Từ khóa',
        FieldName: 'Keyword',
        Placeholder: 'Nhập từ khóa',
        Type: 'text'
      },
      {
        Name: 'Trạng thái',
        FieldName: 'Status',
        Placeholder: 'Trạng thái',
        Type: 'select',
        Data: this.constants.DocumentStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Loại ban hành',
        FieldName: 'CompilationType',
        Placeholder: 'Loại ban hành',
        Type: 'select',
        Data: this.constants.CompilationType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      // {
      //   Name: 'SBU',
      //   FieldName: 'SBUId',
      //   Type: 'dropdown',
      //   SelectMode: 'single',
      //   DataType: this.constant.SearchDataType.SBU,
      //   Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
      //   DisplayName: 'Code',
      //   ValueName: 'Id',
      //   Placeholder: 'Chọn SBU',
      //   RelationIndexTo: 3,
      //   IsRelation: true,
      // },
      {
        Name: 'Nhà cung cấp ban hành',
        FieldName: 'CompilationSuppliserId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constants.SearchDataType.Supplier,
        Columns: [{ Name: 'Code', Title: 'Mã nhà cung cấp' }, { Name: 'Name', Title: 'Tên nhà cung cấp' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn nhà cung cấp',
        // RelationIndexFrom: 2,
        // IsRelation: true,
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constants.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn phòng ban',
        // RelationIndexFrom: 2,
        // IsRelation: true,
      },
      {
        Name: 'Loại tài liệu',
        FieldName: 'DocumentTypeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constants.SearchDataType.DocumentType,
        Columns: [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn loại tài liệu',
      },
      {
        Name: 'Ngày ban hành',
        FieldNameFrom: 'PromulgateLastDateFromV',
        FieldNameTo: 'PromulgateLastDateToV',
        Type: 'date',
      },
      {
        Name: 'Ngày phê duyệt',
        FieldNameFrom: 'PromulgateDateFromV',
        FieldNameTo: 'PromulgateDateToV',
        Type: 'date',
      },
      {
        Name: 'Thời gian hiệu lực',
        FieldNameFrom: 'EffectiveDateFromV',
        FieldNameTo: 'EffectiveDateToV',
        Type: 'date',
      }
    ]
  };

  ngOnInit(): void {
    this.ListIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchChooseDocument();
  }

  searchChooseDocument() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    //this.materialService.searchMaterial(this.modelSearch).subscribe(data => {
    this.documentService.searchChooseDocument(this.modelSearch).subscribe(data => {
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
    this.searchChooseDocument();
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
