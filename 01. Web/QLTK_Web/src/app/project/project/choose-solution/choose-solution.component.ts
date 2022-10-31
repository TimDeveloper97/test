import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Constants, AppSetting, DateUtils } from 'src/app/shared';
import { ProjectSolutionService } from '../../service/project-solution.service';

@Component({
  selector: 'app-choose-solution',
  templateUrl: './choose-solution.component.html',
  styleUrls: ['./choose-solution.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseSolutionComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public constants: Constants,
    public appset: AppSetting,
    private service: ProjectSolutionService,
    public dateUtils: DateUtils,
  ) { }

  checkedTop: boolean = false;
  checkedBot: boolean = false;
  isAction: boolean = false;
  listSelect: any = [];
  listData: any = [];
  listIdSelect: any = [];
  listIdSelectRequest: any = [];
  IsRequest: boolean;

  modelSearch: any = {
    Id: '',
    Code: '',
    Name: '',
    SolutionGroupId: '',
    DateToV: null,
    DateFromV: null,
    CustomerName: '',
    EndCustomerName: '',
    SBUId: '',
    DepartmentId: '',
    SolutionMaker: '',
    ListIdSelect: [],
    ListIdChecked: [],
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã hoặc tên giải pháp',
    Items: [
      {
        Name: 'Nhóm',
        FieldName: 'SolutionGroupId',
        Type: 'dropdowntree',
        SelectMode: 'single',
        DataType: this.constants.SearchDataType.SolutionGroup,
        Columns: [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn nhóm giải pháp',
        ParentId: 'ParentId'
        //IsRelation: true,
      },
      {
        Name: 'Thời gian bắt đầu',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'Khách hàng',
        FieldName: 'CustomerName',
        Placeholder: 'Nhập khách hàng',
        Type: 'text'
      },
      {
        Name: 'Khách hàng cuối',
        FieldName: 'EndCustomerName',
        Placeholder: 'Nhập khách hàng cuối',
        Type: 'text'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constants.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Chọn SBU',
        RelationIndexTo: 5,
        IsRelation: true,
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
        RelationIndexFrom: 4,
        RelationIndexTo: 6,
        IsRelation: true,
      },
      {
        Name: 'Nhân viên lập',
        FieldName: 'SolutionMaker',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constants.SearchDataType.Employee,
        Columns: [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn nhân viên',
        RelationIndexFrom: 5,
      },
      // {
      //   Name: 'Trạng thái',
      //   FieldName: 'Status',
      //   Placeholder: 'Trạng thái',
      //   Type: 'select',
      //   Data: this.constants.StatusSolution,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // }
    ]
  };

  ngOnInit() {
    this.listIdSelect.forEach(element => {
      this.modelSearch.ListIdSelect.push(element);
    });
    this.searchSolution();
  }

  searchSolution() {
    if (this.modelSearch.DateFromV) {
      this.modelSearch.DateFrom = this.dateUtils.convertObjectToDate(this.modelSearch.DateFromV);
    } else {
      this.modelSearch.DateFrom = null;
    }
    if (this.modelSearch.DateToV) {
      this.modelSearch.DateTo = this.dateUtils.convertObjectToDate(this.modelSearch.DateToV)
    } else {
      this.modelSearch.DateTo = null;
    }
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelSearch.ListIdChecked.push(element.Id);
      }
    });
    this.service.searchSolution(this.modelSearch).subscribe(data => {
      this.listData = data.ListResult;
      this.listData.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelSearch.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  choose() {
    this.activeModal.close(this.listSelect);
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
      Id: '',
      Code: '',
      Name: '',
      SolutionGroupId: '',
      DateToV: null,
      DateFromV: null,
      CustomerName: '',
      EndCustomerName: '',
      SBUId: '',
      DepartmentId: '',
      SolutionMaker: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }
    this.modelSearch.IsRequest = this.IsRequest;
    if (this.IsRequest) {
      this.listIdSelectRequest.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    } else {
      this.listIdSelect.forEach(element => {
        this.modelSearch.ListIdSelect.push(element);
      });
    }
    this.searchSolution();
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
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
}
