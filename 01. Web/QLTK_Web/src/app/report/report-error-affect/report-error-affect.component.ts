import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportErrorAffectService } from '../service/report-error-affect.service';
import { ReportErrorWorkComponent } from '../report-error-work/report-error-work.component';

@Component({
  selector: 'app-report-error-affect',
  templateUrl: './report-error-affect.component.html',
  styleUrls: ['./report-error-affect.component.scss']
})
export class ReportErrorAffectComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public reportErrorService: ReportErrorAffectService,
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
    let now = new Date();
    // let dateFromV = { day: 1, month: 4, year: now.getFullYear() };
    // var monthNow = now.getMonth() + 1;
    // if(monthNow < dateFromV.month)
    // {
    //   dateFromV.year = dateFromV.year - 1;
    // }
    // this.model.DateFromV = dateFromV;

    // let dateToV = { day: 31, month: 3, year: now.getFullYear() + 1 };
    // if(monthNow < dateToV.month)
    // {
    //   dateToV.year = dateToV.year - 1;
    // }
    // this.model.DateToV = dateToV;

    this.appSetting.PageTitle = "B??o c??o y???u t??? ???nh h?????ng";
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
    Placeholder: 'T??m ki???m theo m?? ...',
    Items: [
      {
        Name: 'T??n module',
        FieldName: 'ModuleName',
        Placeholder: 'Nh???p t??n module ...',
        Type: 'text'
      },
      {
        Placeholder: 'Ch???n d??? ??n',
        Name: 'M?? d??? ??n',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Project,
        Columns: [{ Name: 'Code', Title: 'M?? d??? ??n' }, { Name: 'Name', Title: 'T??n d??? ??n' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Ch???n kh??ch h??ng',
        Name: 'Kh??ch h??ng th????ng m???i',
        FieldName: 'CustomerId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'M?? kh??ch h??ng' }, { Name: 'Name', Title: 'T??n kh??ch h??ng' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Ch???n kh??ch h??ng',
        Name: 'Kh??ch h??ng cu???i',
        FieldName: 'CustomerFinalId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'M?? kh??ch h??ng' }, { Name: 'Name', Title: 'T??n kh??ch h??ng' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Ch???n TT d??? ??n',
        Name: 'T??nh tr???ng d??? ??n',
        FieldName: 'ProjectStatus',
        Type: 'select',
        DisplayName: 'Name',
        Data: this.constant.ProjectStatus,
        ValueName: 'Id'
      },
      {
        Placeholder: 'Ch???n nh??n vi??n',
        Name: 'Ng?????i kh???c ph???c',
        FieldName: 'EmployeeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employees,
        Columns: [{ Name: 'Code', Title: 'M?? ' }, { Name: 'Name', Title: 'T??n ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Ch???n b??? ph???n',
        Name: 'D??? ??n theo ph??ng kinh doanh',
        FieldName: 'DepartmentManageId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'M?? ' }, { Name: 'Name', Title: 'T??n ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Ch???n b??? ph???n',
        Name: 'B??? ph???n ch???u tr??ch nhi???m',
        FieldName: 'DepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'M?? ' }, { Name: 'Name', Title: 'T??n ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Ch???n b??? ph???n',
        Name: 'B??? ph???n kh???c ph???c',
        FieldName: 'FixDepartmentId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'M?? ' }, { Name: 'Name', Title: 'T??n ' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Name: 'Th???i gian',
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

  showErrorWork(workType, objectId1, objectId2, objectType, deadline) {
    let activeModal = this.modalService.open(ReportErrorWorkComponent, { container: 'body', windowClass: 'report-error-work-modal', backdrop: 'static' })
    activeModal.componentInstance.searchModel = Object.assign({}, this.model);
    if (objectType == 1) {
      activeModal.componentInstance.searchModel.DepartmentId = objectId1;
      activeModal.componentInstance.searchModel.AffectId = objectId2;
    }
    else {
      activeModal.componentInstance.searchModel.EmployeeId = objectId1;
      activeModal.componentInstance.searchModel.DepartmentId = objectId2;
    }
    
    activeModal.componentInstance.searchModel.WorkType = workType;
    activeModal.componentInstance.searchModel.Deadline = deadline;
    activeModal.componentInstance.searchModel.IsAffect = true;
    activeModal.result.then((result) => {
      if (result) {

      }
    }, (reason) => {
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
