import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, AppSetting, Constants, Configuration, ComboboxService, ComponentService, DateUtils } from 'src/app/shared';
import { ImportEmployeeComponent } from '../import-employee/import-employee.component';
import { EmployeeServiceService } from '../../service/employee-service.service';
import { EmployeeUpdateService } from '../../service/employee-update.service';

import { Observable, Subject } from 'rxjs';

@Component({
  selector: 'app-employee-manage',
  templateUrl: './employee-manage.component.html',
  styleUrls: ['./employee-manage.component.scss']
})
export class EmployeeManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,
    private modalService: NgbModal,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private employeeService: EmployeeServiceService,
    private employeeUpdateService: EmployeeUpdateService,
    private componentService: ComponentService,
    private dateUtils: DateUtils
  ) { }
  StartIndex = 0;
  listData: any[] = [];
  listEmployee: any[] = [];
  listSBU: any[] = [];
  listDepartment: any[] = [];
  fileTemplate = this.config.ServerApi + 'Template/Import_Nhân viên_Template.xls';

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Status1: 0,
    Status2: 0,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Status: '',
    ImagePath: '',
    Name: '',
    Code: '',
    UserName: '',
    DepartmentName: '',
    JobPositionName: '',
    Address: '',
    DateOfBirth: '',
    Email: '',
    SBUId: '',
    DepartmentId: '',
    IsDisable: 1
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên nhân viên',
    Items: [
      {
        Name: 'Tài khoản',
        FieldName: 'UserName',
        Placeholder: 'Nhập tài khoản',
        Type: 'text'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 2
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 1
      },
      {
        Name: 'Tình trạng làm việc',
        FieldName: 'Status',
        Placeholder: 'Tình trạng làm việc',
        Type: 'select',
        Data: this.constant.EmployeeStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian vào làm việc',
        FieldNameFrom: 'StartWorkingFromV',
        FieldNameTo: 'StartWorkingToV',
        Type: 'date',
      },
      {
        Name: 'Thời gian hết hạn hợp đồng',
        FieldNameFrom: 'ContractExpirationDateFromV',
        FieldNameTo: 'ContractExpirationDateToV',
        Type: 'date',
      },
      {
        Name: 'MST TNCN',
        FieldName: 'TaxCode',
        Placeholder: 'MST TNCN',
        Type: 'text'
      },
      {
        Name: 'Vị trí công việc',
        FieldName: 'WorkTypeId',
        Placeholder: 'Vị trí công việc',
        Type: 'select',
        DataType: this.constant.SearchDataType.WorkType,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
    ]
  };

  ngOnInit() {
    this.getCbbSBU();
    this.getCbbDepartment();
    this.appSetting.PageTitle = "Nhân viên";
    this.SearchEmployees();
  }

  SearchEmployees() {
    if (this.model.StartWorkingFromV) {
      this.model.StartWorkingFrom = this.dateUtils.convertObjectToDate(this.model.StartWorkingFromV);
    }

    if (this.model.StartWorkingToV) {
      this.model.StartWorkingTo = this.dateUtils.convertObjectToDate(this.model.StartWorkingToV);
    }

    if (this.model.ContractExpirationDateFromV) {
      this.model.ContractExpirationDateFrom = this.dateUtils.convertObjectToDate(this.model.ContractExpirationDateFromV);
    }

    if (this.model.ContractExpirationDateToV) {
      this.model.ContractExpirationDateTo = this.dateUtils.convertObjectToDate(this.model.ContractExpirationDateToV);
    }

    this.employeeService.SearchEmployee(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
        this.model.Status1 = data.Status1;
        this.model.Status2 = data.Status2;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCbbSBU() {
    this.comboboxService.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }


  getCbbDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  LockOpen(Id: string) {
    this.model.Id = Id;
    this.model.IsDisable = 0;
    this.employeeUpdateService.LockEmployee(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Mở khóa nhân viên thành công!');
        this.model.IsDisable = 0;
        this.SearchEmployees();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  LockEmployee(Id: string) {
    this.model.Id = Id;
    this.model.IsDisable = 1;
    this.employeeUpdateService.LockEmployee(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Khóa nhân viên thành công!');
        this.model.IsDisable = 0;
        this.SearchEmployees();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showConfirmDeleteTestCriteria(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá nhân viên này không?").then(
      data => {
        this.deleteJobPostiton(Id);
      },
      error => {

      }
    );
  }

  deleteJobPostiton(Id: string) {
    this.employeeService.DeleteEmployee({ Id: Id }).subscribe(
      data => {
        this.SearchEmployees();
        this.messageService.showSuccess('Xóa nhân viên thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Status1: 0,
      Status2: 0,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      Status: '',
      ImagePath: '',
      Name: '',
      Code: '',
      UserName: '',
      DepartmentName: '',
      JobPositionName: '',
      Address: '',
      DateOfBirth: '',
      Email: '',
      SBUId: '',
      DepartmentId: '',
    }
    this.listDepartment = [];
    this.SearchEmployees();
  }



  exportExcel() {
    this.employeeService.ExPort(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link)
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  showCreateUpdate() {
    this.router.navigate(['nhan-vien/quan-ly-nhan-vien/them-moi']);
  }

  showUpdate(Id: string) {
    this.router.navigate(['nhan-vien/quan-ly-nhan-vien/chinh-sua/', Id]);
  }

  showImportEmployeePopup() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.employeeService.importFileEmployee(data).subscribe(
          data => {
            this.SearchEmployees();
            this.messageService.showSuccess('Import nhân viên thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  showConfirmResetPassword(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn cài lại mật khẩu nhân viên về mặc định?").then(
      data => {
        this.resetPassword(Id);
      },
      error => {

      }
    );
  }
  resetPassword(Id: string) {
    this.employeeService.ResetPassword(Id).subscribe(
      data => {
        this.SearchEmployees();
        this.messageService.showSuccess('Cài đặt lại mật khẩu thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  importExcel() {
    this.componentService.showImportExcelCallback(this.getTemplate, false).subscribe(data => {
      if (data) {
        this.employeeService.importFileEmployee(data).subscribe(
          data => {
            this.SearchEmployees();
            this.messageService.showSuccess('Import nhân viên thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  getTemplate = (): Observable<any> => {
    return this.employeeService.ExportTemplate({});
  }
}
