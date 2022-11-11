import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TiviReportErrorService } from '../services/tivi-report-error.service';

@Component({
  selector: 'app-tivi-report-error',
  templateUrl: './tivi-report-error.component.html',
  styleUrls: ['./tivi-report-error.component.scss']
})
export class TiviReportErrorComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public reportErrorService: TiviReportErrorService,
    private config: Configuration,
    public dateUtils: DateUtils,
    private modalService: NgbModal,
  ) { }

  @ViewChild('scrollDepartmentManage', { static: false }) scrollDepartmentManage: ElementRef;
  @ViewChild('scrollDepartmentManageHeader', { static: false }) scrollDepartmentManageHeader: ElementRef;

  @ViewChild('scrollDepartment', { static: false }) scrollDepartment: ElementRef;
  @ViewChild('scrollDepartmentHeader', { static: false }) scrollDepartmentHeader: ElementRef;

  @ViewChild('scrollEmployee', { static: false }) scrollEmployee: ElementRef;
  @ViewChild('scrollEmployeeHeader', { static: false }) scrollEmployeeHeader: ElementRef;
  depMinWidth = 250;
  empMinWidth = 250;
  departmentSelectIndex = -1;
  employeeSelectIndex = -1;

  ngOnInit() {
    this.appSetting.PageTitle = "Báo cáo Vấn đề tồn đọng";
    this.report();
  }

  ngAfterViewInit() {
    this.scrollDepartmentManage.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollDepartmentManageHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollDepartment.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollDepartmentHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.scrollEmployee.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollEmployeeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollDepartmentManage.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollDepartment.nativeElement.removeEventListener('ps-scroll-x', null);
    this.scrollEmployee.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  errorFixs: any[] = [];
  errorFixBys: any[] = [];
  departments: any[] = [];
  errorProjects: any[] = [];
  stages: any[] = [];

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'ModuleCode',
    OrderType: true,
    ModuleName: '',
    ModuleCode: '',
    ProjectId: '',
    DateFrom: null,
    DateTo: null,
    CustomerId: '',
    CustomerFinalId:''
  }

  searchOptions: any = {
    FieldContentName: 'ModuleCode',
    Placeholder: 'Tìm kiếm theo mã ...',
    Items: [
      {
        Name: 'Tên module',
        FieldName: 'ModuleName',
        Placeholder: 'Nhập tên module ...',
        Type: 'text'
      },
      {
        Placeholder: 'Chọn dự án',
        Name: 'Mã dự án',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Project,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn khách hàng',
        Name: 'Khách hàng thương mại',
        FieldName: 'CustomerId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn khách hàng',
        Name: 'Khách hàng cuối',
        FieldName: 'CustomerFinalId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn TT dự án',
        Name: 'Tình trạng dự án',
        FieldName: 'ProjectStatus',
        Type: 'select',
        DisplayName: 'Name',
        Data: this.constant.ProjectStatus,
        ValueName: 'Id'
      },
      {
        Placeholder: 'Chọn nhân viên',
        Name: 'Người khắc phục',
        FieldName: 'EmployeeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employees,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn bộ phận',
        Name: 'Dự án theo phòng kinh doanh',
        FieldName: 'DepartmentManageId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn bộ phận',
        Name: 'Bộ phận chịu trách nhiệm',
        FieldName: 'DepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn bộ phận',
        Name: 'Bộ phận khắc phục',
        FieldName: 'FixDepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã ' }, { Name: 'Name', Title: 'Tên ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Name: 'Thời gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
    ]
  };


  report() {
    this.model.DateFrom = null;
    if (this.model.DateFromV != null) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }

    this.model.DateTo = null;
    if (this.model.DateToV != null) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV)
    }

    this.reportErrorService.report(this.model).subscribe(
      data => {
        this.errorFixs = data.ErrorFixs;
        this.errorFixBys = data.ErrorFixBys;
        this.departments = data.Departments;
        this.stages = data.Stages;
        this.errorProjects = data.ErrorProjects;

        this.empMinWidth = 250 + this.departments.length * 270;
        this.depMinWidth = 370 + this.stages.length * 270;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'ModuleCode',
      OrderType: true,
      ModuleName: '',
      ModuleCode: '',
      ProjectId: '',
      DateFrom: null,
      DateTo: null,
    }
    let now = new Date();
    // let dateFromV = { day: 1, month: 4, year: now.getFullYear() };
    // this.model.DateFromV = dateFromV;
    // let dateToV = { day: 31, month: 3, year: now.getFullYear() + 1 };
    // this.model.DateToV = dateToV;

    this.report();
  }

  exportExcel() {
    this.reportErrorService.exportExcel(this.model).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

  selectDepartment(index) {
    if (this.departmentSelectIndex != index) {
      this.departmentSelectIndex = index
    }
  }

  selectEmployee(index) {
    if (this.employeeSelectIndex != index) {
      this.employeeSelectIndex = index
    }
  }
}
