import { Component, OnInit, ViewChild, OnDestroy, ElementRef, AfterViewInit } from '@angular/core';

import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { DashboardEmployeeService } from '../service/dashboard-employee.service';
import { forkJoin } from 'rxjs';
import { DxTreeListComponent } from 'devextreme-angular';

@Component({
  selector: 'app-dashboard-employee',
  templateUrl: './dashboard-employee.component.html',
  styleUrls: ['./dashboard-employee.component.scss']
})
export class DashboardEmployeeComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    private combobox: ComboboxService,
    private service: DashboardEmployeeService,
  ) { }

  listData: any[] = [];
  listGeneral: any[] = [];
  listYear: any[] = [];
  listMonth: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any[] = [];
  listSBU: any[] = [];
  listModuleGroup: any[] = [];
  selectedRowKeys: any[] = [];
  treeBoxValue: string[];
  moduleGroupId: ''
  moduleGroupName = '';
  isDropDownBoxOpened = false;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  columnNameEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  DateNow = new Date();
  totalItems = 0;


  model: any = {
    WorkType: 1,
    SBUId: '',
    DepartmentId: '',
    TimeType: '6',
    EmployeeId: '',
    DateFromV: null,
    DateToV: null,
    DateFrom: null,
    DateTo: null,
    Year: null,
    Month: this.DateNow.getMonth() + 1,
    ListModuleGroupId: []
  }
  searchOptions: any = {
    FieldContentName: '',
    Placeholder: 'Tìm kiếm ',
    Items: [
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.SBU,
        Columns: [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        IsRelation: true,
        Permission: ['F061002'],
        RelationIndexTo: 1,
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Permission: ['F061002'],
        RelationIndexFrom: 0,
      },
      {
        Name: 'Nhóm',
        FieldName: 'WorkType',
        Placeholder: 'Nhóm',
        Type: 'select',
        Data: this.constant.WorkType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thời gian',
        FieldName: 'TimeType',
        Placeholder: 'Nhóm',
        Type: 'select',
        Data: this.constant.SearchDebtTimeTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  // searchOptionCV: any = {
  //   FieldContentName: '',
  //   Placeholder: 'Tìm kiếm ',
  //   Items: [
  //     {
  //       Name: 'Nhóm Module',
  //       FieldName: 'ListModuleGroupId',
  //       Type: 'dropdowntree',
  //       SelectMode: 'multiple',
  //       DataType: this.constant.SearchDataType.ModuleGroup,
  //       Columns: [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }],
  //       DisplayName: 'Code',
  //       ValueName: 'Id',
  //       Placeholder: 'Chọn Nhóm Module'
  //     },
  //     {
  //       Name: 'Nhân viên',
  //       FieldName: 'EmployeeId',
  //       Type: 'dropdown',
  //       SelectMode: 'single',
  //       DataType: this.constant.SearchDataType.Employees,
  //       Columns: [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }],
  //       DisplayName: 'Name',
  //       ValueName: 'Id',
  //       Placeholder: 'Chọn nhân viên'
  //     }
  //   ]
  // };

  @ViewChild('scrollEmployee',{static:false}) scrollEmployee: ElementRef;
  @ViewChild('scrollEmployeeHeader',{static:false}) scrollEmployeeHeader: ElementRef;

  ngOnInit() {
    this.appSetting.PageTitle = "Dashbroad nhân sự";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.model.SBUId = currentUser.sbuId;
      this.model.DepartmentId = currentUser.departmentId;
    }
    this.getCbbSBU();
    forkJoin([
      this.combobox.getCbbSBU(),
      this.combobox.getCbbDepartmentBySBU(this.model.SBUId)]
    ).subscribe(([res1, res2]) => {
      this.listSBU = res1;
      this.listDepartment = res2;
    });
    this.search();
    this.loadMonth();
    this.loadYear();
    this.initModel();
    this.GetEmployee();
    this.getCBBModuleGroup();

  }

  ngAfterViewInit(){
    this.scrollEmployee.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollEmployeeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollEmployee.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  // bảng 1 
  search() {
    this.SearchListEmployee();
  }

  searchDashboard() {
    this.getCbbDepartment()
    this.search();
  }

  searchEmployee() {
    this.getGeneralDashboardEmployee();
  }

  SearchListEmployee() {
    this.service.searchListEmployee(this.model).subscribe((data: any) => {
      this.listData = data.ListDashboard;
      this.totalItems = data.TotalItems;
    }, error => {
      this.messageService.showError(error);
    });
  }
  // Tìm kiếm nhân viên
  GetEmployee() {
    this.service.GetEmployee(this.model).subscribe((data: any) => {
      this.listEmployee = data;

    }, error => {
      this.messageService.showError(error);
    });
  }

  // Bảng 2 
  getGeneralDashboardEmployee() {
    this.service.getGeneralDashboardEmployee(this.model).subscribe((data: any) => {
      this.listGeneral = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbbSBU() {
    this.combobox.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbDepartment() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  clear() {
    this.model = {
      WorkType: 1,
      TimeType: '0',
      DateFromV: null,
      DateToV: null,
      DateFrom: null,
      DateTo: null,
      Year: null,
      Month: this.DateNow.getMonth() + 1,
      EmployeeId: '',
      ListModuleGroupId: []
    };
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.model.SBUId = currentUser.sbuId;
      this.model.DepartmentId = currentUser.departmentId;
    }
    this.search();
    this.loadMonth();
    this.loadYear();
    this.initModel();
    this.getCBBModuleGroup();
    this.listGeneral = [];
  }

  loadMonth() {
    for (var month = 1; month < 13; month++)
      this.listMonth.push(month);
  }

  loadYear() {
    for (var year = 2017; year <= new Date().getFullYear(); year++) {
      this.listYear.push(year);
    }
  }

  createObjectDate(year: number, month: number, day: number) {
    var objectDate = new ObjectDate();
    objectDate.day = day;
    objectDate.month = month;
    objectDate.year = year;
    return objectDate;
  }

  initModel() {
    this.model.Year = new Date().getFullYear();
    this.model.DateFromV = new ObjectDate();

    // Set ngày từ
    this.model.DateFromV = this.createObjectDate(parseInt(this.model.Year), 1, 1);

    // Set ngày đến
    this.model.DateToV = this.createObjectDate(parseInt(this.model.Year), 12, 31);
  }

  getCBBModuleGroup() {
    this.combobox.getCbbModuleGroup().subscribe((data: any) => {
      if (data) {
        this.listModuleGroup = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e;
    this.model.ListModuleGroupId = e;
    if (e == null) {
      this.model.ListModuleGroupId = [];
    }
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;
  }

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
  }
}

export class ObjectDate {
  day: Number;
  month: Number;
  year: Number;
}
