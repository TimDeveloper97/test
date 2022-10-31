import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { ModuleGroupService } from '../../services/module-group-service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-module-group-choose-product-standard',
  templateUrl: './module-group-choose-product-standard.component.html',
  styleUrls: ['./module-group-choose-product-standard.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ModuleGroupChooseProductStandardComponent implements OnInit {

  constructor(
    private messageService: MessageService,
    private moduleGroupService: ModuleGroupService,
    private activeModal: NgbActiveModal,
    private comboboxService: ComboboxService,
    public constant: Constants,
  ) { }

  isAction: boolean = false;
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  modelsearch: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Id: '',
    ProductStandardGroupId: '',
    ProductStandardGroupName: '',
    SBUId: '',
    DepartmentId: '',
    DepartmentName: '',
    Code: '',
    Name: '',
    Content: '',
    Target: '',
    Note: '',
    Version: '',
    EditContent: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: '',
    CreateByName: '',
    ListIdSelect: [],
    ListIdChecked: [],
    DataType: null,
  }
  listBase: any = [];
  listSelect: any = [];
  ListIdSelect: any = [];
  ListProductStandardGroup: any = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo Mã/Tên tiêu chuẩn',
    Items: [
      {
        Name: 'Nhóm tiêu chuẩn',
        FieldName: 'ProductStandardGroupId',
        Placeholder: 'Chọn nhóm tiêu chuẩn',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.ProductStandardGroup,
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
      },
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
        RelationIndexTo: 3,
      },
      {
        Name: 'Bộ phận sử dụng',
        FieldName: 'DepartmentId',
        Placeholder: 'Bộ phận sử dụng',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 2,
      },
    ]
  };

  ngOnInit() {
    this.ListIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });
    this.searchProductStandard();
  }

  searchProductStandard() {
    this.listSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element.Id);
    });
    this.listBase.forEach(element => {
      if (element.Checked) {
        this.modelsearch.ListIdChecked.push(element.Id);
      }
    });
    this.moduleGroupService.searchProductStandard(this.modelsearch).subscribe(data => {
      this.listBase = data.ListResult;
      this.listBase.forEach((element, index) => {
        element.Index = index + 1;
      });
      this.modelsearch.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    })
  }

  clear() {
    this.modelsearch = {
      page: 1,
      PageSize: 10,
      totalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      ProductStandardGroupId: '',
      ProductStandardGroupName: '',
      DepartmentId: '',
      DepartmentName: '',
      SBUId: '',
      Code: '',
      Name: '',
      Content: '',
      Target: '',
      Note: '',
      Version: '',
      EditContent: '',
      CreateBy: '',
      CreateDate: '',
      UpdateBy: '',
      UpdateDate: '',

      CreateByName: '',
      ListIdSelect: [],
      ListIdChecked: [],
    }

    this.ListIdSelect.forEach(element => {
      this.modelsearch.ListIdSelect.push(element);
    });

    this.searchProductStandard();
  }

  addRow() {
    var index = 1;
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
    for (var item of this.listBase) {
      item.Index = index;
      index++;
    }
  }

  removeRow() {
    var index = 1;
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
    for (var item of this.listBase) {
      item.Index = index;
      index++;
    }
  }

  choose() {
    this.activeModal.close(this.listSelect);
  }

  CloseModal() {
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
