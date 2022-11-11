import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService } from 'src/app/shared';
import { FlowStageService } from '../../service/flow-stage.service';

@Component({
  selector: 'app-choose-output-result',
  templateUrl: './choose-output-result.component.html',
  styleUrls: ['./choose-output-result.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseOutputResultComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private flowStageService: FlowStageService,
    public constant: Constants,) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  ClassRoomId: string;
  isAction: boolean = false;
  listSelect: any[] = [];
  ListIdSelect: any[] = [];
  ListIdSelectRequest: any[] = [];

  listData: any = [];
  IsRequest: boolean;
  flowStageId:string;
  departmentId:string;

  modelSearch: any = {
    TotalItem: 0,
    Name: '',
    ListIdSelect: [],
    SBUId: '',
    DepartmentId: '',
    FlowStageId: '',
    ListIdChecked: []
  }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên ...',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 1
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 0
      },
      {
        Name: 'Dòng chảy',
        FieldName: 'FlowStageId',
        Placeholder: 'Dòng chảy',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.FlowStage,
        Columns: [{ Name: 'Code', Title: 'Mã dòng chảy' }, { Name: 'Name', Title: 'Tên dòng chảy' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  ngOnInit(): void {

    this.modelSearch.FlowStageId = this.flowStageId;
    this.modelSearch.DepartmentId = this.departmentId;

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
    //this.materialService.searchMaterial(this.modelSearch).subscribe(data => {
    this.flowStageService.searchOutputResult(this.modelSearch).subscribe(data => {
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
