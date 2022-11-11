import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ModuleTestCriteriaService } from '../../services/module-test-criteria.service';

@Component({
  selector: 'app-module-choose-test-criteia',
  templateUrl: './module-choose-test-criteia.component.html',
  styleUrls: ['./module-choose-test-criteia.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class ModuleChooseTestCriteiaComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    public constant: Constants,
    private moduleTestCriter: ModuleTestCriteriaService,
    private comboboxService: ComboboxService
  ) { }
  isAction: boolean = false;
  modelSearch: any = {
    page: 1,
    PageSize: 10,
    TotalItem: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    DataType: null,
    Id: '',
    Name: '',
    Code: '',
    TestCriteriaGroupId: '',
    TechnicalRequirements: '',
    Note: '',
    ListTestCeiteriaModel: [],

    ListIdSelect: [],
    ListIdChecked: [],
    IsRequest: false
  }
  Checked: string;
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  listCRI: any[] = [];
  listBase: any = [];
  listSelect: any = [];
  isRequest: boolean;
  listIdSelect: any = [];
  listIdSelectRequest: any = [];

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo Mã/Tên tiêu chuẩn',
    Items: [
      {
        Name: 'Nhóm tiêu chí',
        FieldName: 'TestCriteriaGroupId',
        Placeholder: 'Chọn nhóm tiêu chí',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.TestCriteriaGroup,
        Columns: [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }],
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Loại',
        FieldName: 'DataType',
        Placeholder: 'Loại tiêu chuản',
        Type: 'select',
        DisplayName: 'Name',
        Data: this.constant.ListWorkType,
        ValueName: 'Id'
      }
    ]
  };

  ngOnInit() {
    this.listIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchTestCriteia();
  }

  searchTestCriteia() {
    this.listSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.moduleTestCriter.searchTestCriteriaInfo(this.modelSearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.TotalItems = data.TotalItems;
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
      TestCriteriaGroupId: '',
      TechnicalRequirements: '',
      Note: '',
      ListTestCriteriaModule: [],

      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.modelSearch.IsRequest = this.isRequest;
    if (this.isRequest) {
      this.listIdSelectRequest.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    } else {
      this.listIdSelect.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    }
    this.searchTestCriteia();
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
