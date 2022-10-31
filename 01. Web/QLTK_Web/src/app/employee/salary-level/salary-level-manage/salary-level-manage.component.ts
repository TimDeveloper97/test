import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Constants, MessageService, Configuration, ComponentService  } from 'src/app/shared';
import { SalaryLevelService } from '../../service/salary-level.service';
import { SalaryLevelCreateComponent } from '../salary-level-create/salary-level-create.component';

@Component({
  selector: 'app-salary-level-manage',
  templateUrl: './salary-level-manage.component.html',
  styleUrls: ['./salary-level-manage.component.scss']
})
export class SalaryLevelManageComponent implements OnInit {

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    private salaryLeveltService: SalaryLevelService,
    private componentService: ComponentService,
    public constant: Constants) { }

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Mã mức lương',
        FieldName: 'Code',
        Placeholder: 'Nhập mã mức lương ...',
        Type: 'text'
      },
      {
        Placeholder: 'Chọn nhóm lương',
        Name: 'Nhóm lương',
        FieldName: 'SalaryGroupId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.GroupSalary,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn ngạch',
        Name: 'Ngạch',
        FieldName: 'SalaryTypeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.SalaryType,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,

    Name: '',
    Code: '',
  };

  salaryLevels: any[] = [];
  salaryGroups: any[] = [];
  salaryTypes: any[] = [];

  fileTemplate = '';
  startIndex = 0;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý mức lương";
    this.search();
  }

  search() {
    this.salaryLeveltService.search(this.searchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
        this.salaryLevels = data.ListResult;
        this.searchModel.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteReason(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá mức lương này không?").then(
      data => {
        this.deleteReason(Id);
      },
      error => {

      }
    );
  }

  deleteReason(Id: string) {
    this.salaryLeveltService.delete({ Id: Id }).subscribe(
      data => {
        this.search();
        this.messageService.showSuccess('Xóa mức lương thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(SalaryLevelCreateComponent, { container: 'body', windowClass: 'salarylevel-create-model', backdrop: 'static' })
    activeModal.componentInstance.id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.search();
      }
    }, (reason) => {
    });
  }

  clear() {
    this.searchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,

      Name: '',
      Code: '',
    };
    this.search();
  }

  showImportPopup() {
    this.salaryLeveltService.getGroupInTemplate().subscribe(d => {
      this.fileTemplate = this.config.ServerApi + d;
      this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
        if (data) {
          this.salaryLeveltService.importFile(data).subscribe(
            data => {
              this.search();
              this.messageService.showSuccess('Import hãng sản xuất thành công!');
            },
            error => {
              this.messageService.showError(error);
            });
        }
      });
    }, e => {
      this.messageService.showError(e);
    });
  }

}
