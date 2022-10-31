import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { AppSetting, ComponentService, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { ChooseMaterialImportPrComponent } from '../choose-material-import-pr/choose-material-import-pr.component';
import { ImportPrService } from '../services/import-pr.service';

@Component({
  selector: 'app-import-pr',
  templateUrl: './import-pr.component.html',
  styleUrls: ['./import-pr.component.scss']
})
export class ImportPrComponent implements OnInit, AfterViewInit {
  checkedTop = false;
  listSelect = [];
  startIndex = 1;
  listId = [];

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

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên vật tư',
    Items: [
      {
        Name: 'Tình trạng',
        FieldName: 'Status',
        Placeholder: 'Chọn tình trạng hồ sơ',
        Type: 'select',
        Data: this.constant.ImportPRProductStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
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

  listData: any[] = [];

  fileTemplate = this.config.ServerApi + 'Template/ImportPR_Template.xlsx';

  @ViewChild('scrollHeader', { static: false }) scrollHeader: ElementRef;
  @ViewChild('scrollImportPR', { static: false }) scrollImportPR: ElementRef;

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private router: Router,
    private componentService: ComponentService,
    private config: Configuration,
    private importPRService: ImportPrService,
    private dateUtils: DateUtils
  ) { }

  ngOnInit(): void {
    this.appSetting.PageTitle = 'Import PR';

    this.searchModel.EmployeeId = JSON.parse(localStorage.getItem('qltkcurrentUser')).employeeId;

    this.searchPR();
  }

  ngAfterViewInit() {
    this.scrollImportPR.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  searchPR() {
    if (this.searchModel.DueDateFromV) {
      this.searchModel.DateFrom = this.dateUtils.convertObjectToDate(this.searchModel.DueDateFromV);
    }

    if (this.searchModel.DueDateToV) {
      this.searchModel.DateTo = this.dateUtils.convertObjectToDate(this.searchModel.DueDateToV);
    }

    this.importPRService.searchImportPR(this.searchModel).subscribe((result: any) => {
      this.listData = result.ListResult;
      this.searchModel.TotalItems = result.TotalItem;
      this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      this.listData.forEach(element => {
        if(this.listId.includes(element.Id))
        {
          element.Checked = true;
        }
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

  importFilePR() {
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

  chooseMaterialImportPR() {
    let activeModal = this.modalService.open(ChooseMaterialImportPrComponent, { container: 'body', windowClass: 'choose-material-import-pr', backdrop: 'static' });
    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {

    });
  }

  checkAll() {    
    this.listId = [];
    this.listData.forEach(element => {
      if (this.checkedTop && !element.Status ) {
        element.Checked = true;
        this.listId.push(element.Id);
      } else {
        element.Checked = false;
      }
    });
  }

  checkItem(row, index) {
    if (this.listData.filter(item => { if (!item.Checked) { return item } }).length == 0) {
      this.checkedTop = true;
      this.listId.push(row.Id);
    }
    else {
      this.checkedTop = false;
      this.listId.splice(index,1);
    }
  }

  createProfile() {
    this.listSelect = [];
    this.listData.forEach(element => {
      if (element.Checked) {
        this.listSelect.push(element);
      }
    });

    if (this.listSelect.length > 0) {
      localStorage.setItem('importPrIds', JSON.stringify(this.listSelect));
      this.router.navigate(['nhap-khau/ho-so-nhap-khau/them-moi']);
    }
    else {
      this.messageService.showMessage("Bạn chưa chọn vật tư để tạo hồ sơ!");
    }
  }

  showConfirmDelete(id) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá vật tư này không?").then(
      data => {
        this.importPRService.deleteProduct({ Id: id }).subscribe((result: any) => {
          this.messageService.showSuccess('Xóa vật tư thành công!');
          this.searchPR();
        }, error => {
          this.messageService.showError(error);
        });
      }
    );
  }
}
