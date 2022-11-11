import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { AppSetting, MessageService, Constants, ComboboxService, DateUtils } from 'src/app/shared';
import { EmployeePresentService } from '../service/employee-present.service';
import * as moment from 'moment';

@Component({
  selector: 'app-employee-present',
  templateUrl: './employee-present.component.html',
  styleUrls: ['./employee-present.component.scss']
})
export class EmployeePresentComponent implements OnInit, AfterViewInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    private service: EmployeePresentService,
    private comboboxService: ComboboxService,
    public dateUtils: DateUtils,
  ) { }
  @ViewChild('scrollPracticeMaterial',{static:false}) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;

  ngOnInit() {
    this.model.Date = new Date();
    this.appSetting.PageTitle = "Báo cáo nhân viện hiện tại";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.model.SBUId = currentUser.sbuId;
      this.model.DepartmentId = currentUser.departmentId;
    }
    this.getCbbSBU();
    this.getEmployeePresent();
    this.getEmployee();
    this.GetCbbProjectBySBUId_DepartmentId();

  }

  ngAfterViewInit(){
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  height = 200;
  model: any = {
    Date: '',
    SBUId: '',
    DepartmentId: '',
    Code: '',
    ProjectId: '',
    ModuleGroupId: ''
  }

  listData: any[] = [];
  Total_Employee_Status_Use: number;
  Total_Employee_Incomplete_CK: number;
  Total_Employee_Incomplete_Dn: number;
  Total_Employee_Incomplete_Dt: number;
  listEmployeeSkill: any[] = [];
  Total_Course_Not_Start: number;
  Total_Course_Delay: number;
  Total_Error_Employee: number;
  list_Error_Employee: any[] = [];
  listEmployeeCourse: any[] = [];
  listEmployeePerformance: any[] = [];

  listEmployeeHeader: any[] = [];
  listSkillEmployee: any[] = [];
  listWorkType: any[] = [];

  DateFormV: null;
  DateToV: null;

  listErrorEmployeeByProduct: any[] = [];
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã và tên nhân viên',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        RelationIndexTo: 1
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 0
      },
    ]
  };

  clear() {
    this.model = {

      SBUId: '',
      DepartmentId: '',
      Code: '',
      ProjectId: '',
      ModuleGroupId: ''
    }
    this.model.Date = new Date();
    this.listDepartment = [];
    this.getEmployeePresent();
  }
  listSBU: any[] = [];
  listDepartment: any[] = [];
  listErrorByEmployee: any[] = [];
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


  GetCbbDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getEmployeePresent() {
    if (this.DateFormV) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.DateFormV);
    } else {
      this.model.DateFrom = null;
    }
    if (this.DateToV) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.DateToV)
    } else {
      this.model.DateTo = null;
    }
    if (this.model.ProjectId == null || this.model.ProjectId == '') {
      this.GetCbbProjectBySBUId_DepartmentId();
    }
    if (this.model.ProjectId) {
      this.getGroupProducts();
    }
    this.getEmployee();
    this.service.getEmployeePresent(this.model).subscribe((data: any) => {
      this.listData = data;
      this.Total_Employee_Status_Use = data.Total_Employee_Status_Use;
      // this.Total_Employee_Incomplete_CK = data.Total_Employee_Incomplete_CK;
      // this.Total_Employee_Incomplete_Dn = data.Total_Employee_Incomplete_Dn;
      // this.Total_Employee_Incomplete_Dt = data.Total_Employee_Incomplete_Dt;
      this.listEmployeeSkill = data.ListEmployeeSkill;
      this.Total_Course_Not_Start = data.Total_Course_Not_Start;
      this.Total_Course_Delay = data.Total_Course_Delay;
      this.Total_Error_Employee = data.Total_Error_Employee;
      this.list_Error_Employee = data.List_Error_Employee;
      this.listEmployeeCourse = data.ListEmployeeCourse;

      this.listEmployeeHeader = data.ListEmployeeHeader;
      this.listSkillEmployee = data.ListSkillEmployee;

      this.listErrorByEmployee = data.ListErrorByEmployee;
      this.listErrorEmployeeByProduct = data.ListErrorEmployeeByProduct;
      this.listWorkType = data.ListWorkType;
    }, error => {
      this.messageService.showError(error);
    });
  }
  listProject: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }];
  columnNameModule: any[] = [{ Name: 'Code', Title: 'Mã nhóm sản phẩm' }, { Name: 'Name', Title: 'Tên nhóm sản phẩm' }];

  GetCbbProjectBySBUId_DepartmentId() {
    this.service.GetCbbProjectBySBUId_DepartmentId(this.model.SBUId, this.model.DepartmentId).subscribe((data: any) => {
      this.listProject = data;
    }, error => {
      this.messageService.showError(error);
    });
  }
  // Nhóm sp - (nhóm module) 
  listGroupProduct: any[] = [];
  getGroupProducts() {
    this.service.GetGroupProducts(this.model.ProjectId).subscribe((data: any) => {
      this.listGroupProduct = data;
    }, error => {
      this.messageService.showError(error);
    });
  }


  getEmployee() {
    this.service.getEmployeePresent(this.model).subscribe((data: any) => {
      this.listEmployeePerformance = data.ListPerformance;
    }, error => {
      this.messageService.showError(error);
    });
  }

  btnMonth(isNext) {
    if (isNext) {
      this.model.Date = moment(this.model.Date).add(1, 'M');
    } else {
      this.model.Date = moment(this.model.Date).add(-1, 'M');
    }
    this.getEmployee();
  }

}
