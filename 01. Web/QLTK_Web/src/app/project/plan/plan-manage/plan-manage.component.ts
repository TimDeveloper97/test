import { Component, OnInit, ElementRef, ViewChild, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { AppSetting, MessageService, Constants, Configuration, ComboboxService, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PlanService } from '../../service/plan.service';
import { PlanCreateComponent } from '../plan-create/plan-create.component';
import { PlanViewComponent } from '../plan-view/plan-view.component';
import { CreateTimePerformComponent } from '../create-time-perform/create-time-perform.component';

@Component({
  selector: 'app-plan-manage',
  templateUrl: './plan-manage.component.html',
  styleUrls: ['./plan-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PlanManageComponent implements OnInit, AfterViewInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    private planService: PlanService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    public dateUtils: DateUtils,

  ) { }

  @ViewChild('scrollPracticeMaterial', { static: false }) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader', { static: false }) scrollPracticeMaterialHeader: ElementRef;

  statusDones: any[] = [
    { Id: 1, Name: 'Chưa thực hiện' },
    { Id: 2, Name: 'Đang thực hiện' },
    { Id: 3, Name: 'Đã hoàn thành'},
    { Id: 4, Name: 'Tạm dừng' },
    { Id: 4, Name: 'Hủy' },
  ];

  StartIndex = 0;
  listData: any[] = [];
  
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    TaskName: '',
    ProjectId: '',
    EmployeeId: '',
    Done: 90,
    DoneType: 5,
    EmployeeCode: '',
    DesignCode: '',
    ContractCode: '',
    PlanStartDateToV: null,
    PlanStartDateFromV: null,
    PlanDueDateToV: null,
    PlanDueDateFromV: null,

    PlanStartDateTo: null,
    PlanStartDateFrom: null,
    PlanDueDateTo: null,
    PlanDueDateFrom: null,
  }

  columnNameEmployee: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];
  planType: any[] = [
    { Id: 1, Name: "Công việc kế hoạch" },
    { Id: 2, Name: "Vấn đề tồn đọng" },
    { Id: 3, Name: "Công việc báo giá" },
  ]

  searchOptions: any = {
    Placeholder: 'Tìm kiếm theo tên công việc',
    FieldContentName: 'TaskName',
    Items: [
      {
        Placeholder: 'Chọn dự án',
        Name: 'Dự án',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.ProjectByUser,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Name: 'Nhân viên',
        FieldName: 'EmployeeId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Employees,
        Columns: [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        Placeholder: 'Chọn nhân viên',
        Permission: ['F060705', 'F110702'],
      },
      {
        Placeholder: 'Mã hoặc tên theo thiết kế',
        Name: 'Theo thiết kế',
        FieldName: 'DesignCode',
        Type: 'text',
      },
      {
        Placeholder: 'Mã hoặc tên theo hợp đồng',
        Name: 'Theo hợp đồng',
        FieldName: 'ContractCode',
        Type: 'text',
      },
      {
        Placeholder: '%',
        Name: '% hoàn thành',
        FieldName: 'Done',
        FieldNameType: 'DoneType',
        Type: 'number',
      },
      {
        Name: 'Thời gian bắt đầu dự kiến',
        FieldNameTo: 'PlanStartDateToV',
        FieldNameFrom: 'PlanStartDateFromV',
        Type: 'date'
      },
      {
        Name: 'Thời gian kết thúc dự kiến',
        FieldNameTo: 'PlanDueDateToV',
        FieldNameFrom: 'PlanDueDateFromV',
        Type: 'date'
      },
      {
        Name: 'Loại công việc',
        FieldName: 'Types',
        Placeholder: 'Loại công việc',
        Type: 'select',
        Data: this.planType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Vị trí',
        FieldName: 'WorkTypeId',
        Placeholder: 'Vị trí',
        Type: 'select',
        DataType: this.constant.SearchDataType.WorkType,
        DisplayName: 'Name',
        ValueName: 'Id',
      },
      {
        Name: 'Tình trạng công việc',
        FieldName: 'Status',
        Placeholder: 'Tình trạng công việc',
        Type: 'select',
        Data: this.statusDones,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  height = 100;

  ngOnInit() {
    this.height = window.innerHeight - 400;
    this.appSetting.PageTitle = "Quản lý kế hoạch";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.model.EmployeeId = currentUser.employeeId;
    }

    this.searchPlan();
    this.getListProject();
    this.getListEmployeeStatus();
    this.getListTask();
  }

  ngAfterViewInit() {
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  // get comboboxx
  listProject: [];
  getListProject() {
    this.comboboxService.getListProject().subscribe(
      data => {
        this.listProject = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  listTask: [];
  getListTask() {
    this.comboboxService.getListTask().subscribe(
      data => {
        this.listTask = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  listEmployee: [];
  getListEmployeeStatus() {
    this.comboboxService.getListEmployeesStatus().subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchPlan() {
    if (this.model.PlanStartDateFromV) {
      this.model.PlanStartDateFrom = this.dateUtils.convertObjectToDate(this.model.PlanStartDateFromV);
    } else {
      this.model.PlanStartDateFrom = null;
    }
    if (this.model.PlanStartDateToV) {
      this.model.PlanStartDateTo = this.dateUtils.convertObjectToDate(this.model.PlanStartDateToV)
    } else {
      this.model.PlanStartDateTo = null;
    }

    if (this.model.PlanDueDateFromV) {
      this.model.PlanDueDateFrom = this.dateUtils.convertObjectToDate(this.model.PlanDueDateFromV);
    } else {
      this.model.PlanDueDateFrom = null;
    }
    if (this.model.PlanDueDateToV) {
      this.model.PlanDueDateTo = this.dateUtils.convertObjectToDate(this.model.PlanDueDateToV)
    } else {
      this.model.PlanDueDateTo = null;
    }


    this.planService.searchPlan(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
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
      OrderBy: 'TaskId',
      OrderType: true,
      Id: '',
      ResponsiblePersion: '',
      TaskId: '',
      ProjectId: '',
      Status: -1,
    }
    this.searchPlan();
  }

  showCreateUpdate(Id: string, Types, ExecutionTime) {
    let activeModal = this.modalService.open(PlanCreateComponent, { container: 'body', windowClass: 'plan-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.Types = Types;
    activeModal.componentInstance.ExecutionTime = ExecutionTime;
    activeModal.result.then((result) => {
      if (result) {
        this.searchPlan();
      }
    }, (reason) => {
    });
  }

  viewPlan(Id: string, Types, Done, TaskName, Description, ContractName, DesignCode, EstimateTime, ExecutionTime, ResponsiblePersionName, PlanStartDate, PlanDueDate, ActualStartDate, ActualEndDate) {
    let activeModal = this.modalService.open(PlanViewComponent, { container: 'body', windowClass: 'plan-view-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.componentInstance.Types = Types;
    activeModal.componentInstance.Done = Done;
    activeModal.componentInstance.ContractName = ContractName;
    activeModal.componentInstance.EstimateTime = EstimateTime;
    activeModal.componentInstance.ExecutionTime = ExecutionTime;
    activeModal.componentInstance.TaskName = TaskName;
    activeModal.componentInstance.Description = Description;
    activeModal.componentInstance.ResponsiblePersionName = ResponsiblePersionName;
    activeModal.componentInstance.DesignCode = DesignCode;
    activeModal.componentInstance.PlanStartDate = PlanStartDate;
    activeModal.componentInstance.PlanDueDate = PlanDueDate;
    activeModal.componentInstance.ActualStartDate = ActualStartDate;
    activeModal.componentInstance.ActualEndDate = ActualEndDate;
    activeModal.result.then((result) => {
      if (result) {
        this.searchPlan();
      }
    }, (reason) => {
    });
  }

  showCreateTimePerform(Id: string, Name: string, Done: number, ProjectId: string) {
    let activeModal = this.modalService.open(CreateTimePerformComponent, { container: 'body', windowClass: 'create-time-perform-model', backdrop: 'static' })
    activeModal.componentInstance.planId = Id;
    activeModal.componentInstance.namePlan = Name;
    activeModal.componentInstance.done = Done;
    activeModal.componentInstance.projectId = ProjectId;
    activeModal.result.then((result) => {
      if (result) {
        this.searchPlan();
      }
    }, (reason) => {
    });
  }

  exportExcel() {
    this.planService.exportExel(this.model).subscribe(d => {
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
}
