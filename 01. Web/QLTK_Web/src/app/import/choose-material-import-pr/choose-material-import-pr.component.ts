import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ComponentService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { IPageInfo } from 'ngx-virtual-scroller';
import { ImportPrService } from '../services/import-pr.service';

@Component({
  selector: 'app-choose-material-import-pr',
  templateUrl: './choose-material-import-pr.component.html',
  styleUrls: ['./choose-material-import-pr.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ChooseMaterialImportPrComponent implements OnInit {

  fileTemplate = this.config.ServerApi + 'Template/ImportPR_Template.xlsx';
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên vật tư',
    Items: [
      // {
      //   Name: 'Tình trạng',
      //   FieldName: 'Status',
      //   Placeholder: 'Chọn tình trạng hồ sơ',
      //   Type: 'select',
      //   Data: this.constant.ImportPRProductStatus,
      //   DisplayName: 'Name',
      //   ValueName: 'Id'
      // },
      {
        Name: 'Nhân viên mua hàng',
        FieldName: 'EmployeeId',
        Placeholder: 'Chọn nhân viên mua hàng',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employee,
        Columns: [{ Name: 'Code', Title: 'Mã Nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Permission: ['F120753']
      },
      {
        Name: 'Mã PR',
        FieldName: 'PRCode',
        Placeholder: 'Nhập mã PR',
        Type: 'text',
      },
      {
        Name: 'Mã dự án',
        FieldName: 'ProjectCode',
        Placeholder: 'Nhập mã dự án',
        Type: 'text',
      },
      {
        Name: 'Hãng sản xuất',
        FieldName: 'ManufactureId',
        Placeholder: 'Mã nhà sản xuất',
        Type: 'dropdown', SelectMode: 'single',
        DataType: this.constant.SearchDataType.Manuafacture,
        Columns: [{ Name: 'Code', Title: 'Mã NSX' }, { Name: 'Name', Title: 'Tên NSX' }],
        DisplayName: 'Code',
        ValueName: 'Id'
      },
      {
        Name: 'Ngày yêu cầu',
        FieldNameFrom: 'DueDateFromV',
        FieldNameTo: 'DueDateToV',
        Type: 'date',
      }
    ]
  };

  startIndex = 1;
  startIndexSelected = 1;
  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
    Code: '',
    Status: false,
    ProjectCode: '',
    ManufactureId: '',
    ListId: [],
    EmployeeId: '',
    PRCode: '',
    DateFrom: null,
    DueDateTo: null,
    DateTo: null,
    DueDateToV: null
  };

  ListId: any[] = [];
  listBase: any[] = [];
  listSelect: any[] = [];
  checkedTop: boolean = false;
  checkedBot: boolean = false;
  loading: boolean;

  constructor(
    private messageService: MessageService,
    private activeModal: NgbActiveModal,
    private importPRService: ImportPrService,
    public constant: Constants,
    private componentService: ComponentService,
    private config: Configuration,
    private dateUtils: DateUtils
  ) { }

  ngOnInit(): void {
    this.searchModel.ListId = [...this.ListId];
    this.searchModel.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    this.searchPR();
  }

  searchPR() {
    this.searchModel.ListId = [...this.ListId];
    this.listSelect.forEach(item => {
      this.searchModel.ListId.push(item.Id);
    });

    if (this.searchModel.DueDateFromV) {
      this.searchModel.DateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DueDateFromV);
    }

    if (this.searchModel.DueDateToV) {
      this.searchModel.DateTo = this.dateUtils.convertObjectToDate(this.searchModel.DueDateToV);
    }

    this.importPRService.searchChoosePR(this.searchModel).subscribe((result: any) => {
      this.listBase = result.ListResult;

      this.listBase.forEach((item, index) => {
        item.Index = index + 1;
      });

    }, error => {
      this.messageService.showError(error);
    });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,
      Code: '',
      Status: false,
      ProjectCode: '',
      ManufactureId: '',
      ListId: [],
      EmployeeId: '',
      DateFrom: null,
      DueDateTo: null,
      DateTo: null,
      DueDateToV: null
    };
    this.searchModel.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;
    this.searchPR();

  }

  addRow() {    
    this.listBase.forEach(element => {
      if (element.Checked) {
        element.Checked = false;
        this.listSelect.push(element);
      }
    });
    this.listSelect.forEach(element => {
      var index = this.listBase.indexOf(element);
      if (index > -1) {
        this.listBase.splice(index, 1);
      }
    });
    this.listBase.forEach((element, index) => {
      element.Index = index + 1;
    });
  }

  removeRow() {
    this.listSelect.forEach(element => {
      if (element.Checked) {
        element.Checked = false;
        this.listBase.push(element);
      }
    });

    this.listBase.forEach(element => {
      var index = this.listSelect.indexOf(element);
      if (index > -1) {
        this.listSelect.splice(index, 1);
      }
    });
    this.listBase.forEach((element, index) => {
      element.Index = index + 1;
    });
  }

  closeModal() {
    this.activeModal.close(false);
  }

  checkAll(isCheck: boolean) {
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

  public fetchMore(event: IPageInfo) {
    if (event.endIndex !== this.listBase.length - 1) return;
    this.loading = true;
    this.fetchNextChunk(this.listBase.length, 10).then(chunk => {
      this.listBase = this.listBase.concat(chunk);
      this.loading = false;
    }, () => this.loading = false);
  }

  fetchNextChunk(skip: number, limit: number): Promise<[]> {
    return new Promise((resolve, reject) => {
      this.searchModel.PageSize = limit;
      this.searchModel.PageNumber = skip;
      this.searchPR();
    });
  }

  chooseMaterialImportPR() {
    this.activeModal.close(this.listSelect);
  }

  import() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.importPRService.importFile(data).subscribe(
          data => {
            this.searchPR();
            this.messageService.showSuccess('Import PR thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }
}
