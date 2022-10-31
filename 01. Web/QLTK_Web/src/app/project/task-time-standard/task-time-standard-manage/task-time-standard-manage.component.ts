import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, AfterViewInit } from '@angular/core';

import { AppSetting, MessageService, Constants, ComboboxService, Configuration, DateUtils, ComponentService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TaskTimeStandardService } from '../../service/task-time-standard.service';
import { TaskTimeStandardCreateComponent } from '../task-time-standard-create/task-time-standard-create.component';
import { TaskService } from '../../service/task.service';
import { TaskTimeStandardChooseYearModalComponent } from '../../task-time-standard-choose-year-modal/task-time-standard-choose-year-modal.component';

@Component({
  selector: 'app-task-time-standard-manage',
  templateUrl: './task-time-standard-manage.component.html',
  styleUrls: ['./task-time-standard-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TaskTimeStandardManageComponent implements OnInit, AfterViewInit  {
  height = 600;
  constructor(
    public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    private taskTimeStandardService: TaskTimeStandardService,
    public constant: Constants,
    public comboboxService: ComboboxService,
    private taskService: TaskService,
    private dateUtils: DateUtils,
    private componentService: ComponentService,
  ) { }
  //departmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).de;
  fileTemplate = this.config.ServerApi + 'Template/Template_ThoiGianTieuChuanNhanVien.xlsx';

  StartIndex = 0;
  listData: any[] = [];
  listTask: any[] = [];
  ListWorkType: any[] = [];
  listSBU: any[] = [];
  listDepartment: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'WorkTypeName',
    OrderType: true,
    Id: '',
    Type: 0,
    TimeStandard: '',
    EmployeeId: '',
    SBUId: '',
    DepartmentId: '',
    ListWorkType: [],
    Name: '',
    DepartmentName: '',
    ModuleGroupId: ''
  }

  calculateAverageModel: any = {
    SBUId: '',
    DepartmentId: '',
    Name: '',
    ModuleGroupId: '',
    DateFrom: null,
    DateTo: null,
    IsCalculate: false,
    IsExportExcel: false,
    IsImport: false
  }

  @ViewChild('scrollPractice',{static:false}) scrollPractice: ElementRef;
  @ViewChild('scrollPracticeHeader',{static:false}) scrollPracticeHeader: ElementRef;
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên nhân viên',
    Items: [
      {
        Name: 'Nhóm module',
        FieldName: 'ModuleGroupId',
        Type: 'dropdowntree',
        SelectMode: 'single',
        ParentId: 'ParentId',
        DataType: this.constant.SearchDataType.ModuleGroup,
        Columns: [{ Name: 'Code', Title: 'Mã nhóm module' }, { Name: 'Name', Title: 'Tên nhóm module' }],
        DisplayName: 'Code',
        ValueName: 'Id',
        Placeholder: 'Nhóm module'
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
        RelationIndexTo: 2
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'dropdown',
        DataType: this.constant.SearchDataType.Department,
        Columns: [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }],
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 1
      },
    ]
  };

  modelTask: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    Name: '',
    Type: '',
    Checked: '',
    ModuleGroupId: '',
    listData: [],
  }
  status = 1;
  ngOnInit() {
    this.model.DepartmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;;
    this.height = window.innerHeight - 350;
    this.appSetting.PageTitle = "Thời gian tiêu chuẩn cho công việc";
    this.searchTaskTimeStandard();
    this.searchTask();
    this.getCBBSBU();
  }

  ngAfterViewInit(){
    this.scrollPractice.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  getCBBSBU() {
    this.comboboxService.getCBBSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  ngOnDestroy() {
    this.scrollPractice.nativeElement.removeEventListener('ps-scroll-x', null);
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

  searchTask() {
    this.taskService.searchTask(this.modelTask).subscribe((data: any) => {
      if (data.ListResult) {
        this.listTask = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  moduleGrTaskTimeStandId: '';
  listAvg: any[] = [];
  searchTaskTimeStandard() {
    this.taskTimeStandardService.searchTaskTimeStandard(this.model).subscribe((data: any) => {
      if (data) {
        this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.searchResult.ListResult;

        this.model.TotalItems = data.searchResult.TotalItem;
        this.ListWorkType = [];
        this.listAvg = data.listAvgTimeStand;
        this.moduleGrTaskTimeStandId = data.ModuleGrTaskTimeStandId;
        this.status = data.status;
        data.searchResult.ListResult.forEach(element => {
          this.ListWorkType = element.ListWorkType;
        });
        //this.functionAvg();
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  save() {
    this.model.listData = this.listData;
    this.taskTimeStandardService.createListTaskTim(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới thời gian tiêu chuẩn cho từng công việc thành công !');
      });
  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'TimeStandard',
      OrderType: true,
      Id: '',
      Type: 0,
      TimeStandard: '',
      EmployeeId: '',
      SBUId: '',
      DepartmentId: '',
      ModuleGroupId: ''
    }
    this.ListWorkType = []
    this.model.DepartmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;;
    this.searchTaskTimeStandard();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(TaskTimeStandardCreateComponent, { container: 'body', windowClass: 'task-time-standard-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchTaskTimeStandard();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteTaskTimeStandard(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thời gian cho công việc này không?").then(
      data => {
        this.deleteTaskTimeStandard(Id);
      },
      error => {
        
      }
    );
  }

  deleteTaskTimeStandard(Id: string) {
    this.taskTimeStandardService.deleteTaskTimeStandard({ Id: Id }).subscribe(
      data => {
        this.searchTaskTimeStandard();
        this.messageService.showSuccess('Xóa thời gian cho công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  selectIndex = -1;
  loadValue(param, index) {
    this.selectIndex = index;
  }
  modelTime = {
    ListTimeStand: [],
    TaskId: '',
  }
  ListTemp: any[] = [];
  ListWorkTypeForEmployee: any[] = [];
  functionAvg() {
    let index = 0;
    this.listAvg = [];
    let count = 0;
    let workTimeSum = 0;
    this.ListWorkType.forEach(worktime => {
      count = 0;
      workTimeSum = 0;
      this.listData.forEach(el => {
        el.ListWorkType.forEach(element => {
          if (worktime.TaskId == element.TaskId && element.TimeStandard > 0) {
            count++;
            workTimeSum += element.TimeStandard;
          }
        });
      });
      if (count == 0) {

        this.listAvg.push({ Avg: 0 })
      }
      else {
        let temp = (workTimeSum / count).toFixed(2);
        this.listAvg.push({ Avg: temp })

      }
    });

    let ListTimeStand: any[] = [];
    // this.ListWorkType.forEach(element => {
    //   this.modelTime = {
    //     ListTimeStand: [],
    //     TaskId: '',
    //   };
    //   for (let index = 0; index < this.ListWorkTypeForEmployee.length; index++) {
    //     if (element.TaskId == this.ListWorkTypeForEmployee[index].TaskId) {
    //       this.modelTime.ListTimeStand.push(this.ListWorkTypeForEmployee[index].TimeStandard)
    //     }
    //   }
    //   this.model.TaskId = element.TaskId;
    //   this.ListTemp.push(this.modelTime);
    // });
  }
  moduleGrTime = {
    Id: '',
    DepartmentId: '',
    ModuleGroupId: '',
    Status: 0,
  }

  // Config(){
  //   this.moduleGrTime.DepartmentId = this.model.DepartmentId;
  //   this.moduleGrTime.ModuleGroupId = this.model.ModuleGroupId;

  //   this.taskTimeStandardService.createListTaskTim(this.moduleGrTime).subscribe(
  //     data => {
  //       this.messageService.showSuccess('Thêm mới thời gian tiêu chuẩn cho từng công việc thành công !');
  //     });
  // }

  Config() {
    if (this.model.DepartmentId == null || this.model.DepartmentId == '') {
      this.messageService.showMessage("Bạn phải chọn phòng ban");
    }
    if (this.model.ModuleGroupId == null || this.model.ModuleGroupId == '') {
      this.messageService.showMessage("Bạn phải chọn nhóm module");
    }
    if (this.model.DepartmentId != null && this.model.DepartmentId != '' && this.model.ModuleGroupId != null && this.model.ModuleGroupId != '') {
      this.moduleGrTime.DepartmentId = this.model.DepartmentId;
      this.moduleGrTime.ModuleGroupId = this.model.ModuleGroupId;
      this.moduleGrTime.Status = 1;
      if (this.moduleGrTaskTimeStandId == null || this.moduleGrTaskTimeStandId == '') {
        this.taskTimeStandardService.createModuleGroupTimeStandard(this.moduleGrTime).subscribe(
          data => {
            this.messageService.showSuccess('Cấu hình thành công!');
            this.status = 1;
            this.searchTaskTimeStandard();
          },
          error => {
            this.messageService.showError(error);
          }
        );
      } else {
        this.moduleGrTime.Id = this.moduleGrTaskTimeStandId;
        this.taskTimeStandardService.updateModuleGroupTimeStandard(this.moduleGrTime).subscribe(
          data => {
            this.messageService.showSuccess('Cấu hình thành công!');
            this.status = 1;
            this.searchTaskTimeStandard();
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }

    }
  }

  NotConfig() {
    if (this.model.DepartmentId == null || this.model.DepartmentId == '') {
      this.messageService.showMessage("Bạn phải chọn phòng ban");
    }
    if (this.model.ModuleGroupId == null || this.model.ModuleGroupId == '') {
      this.messageService.showMessage("Bạn phải chọn nhóm module");
    }
    if (this.model.DepartmentId != null && this.model.DepartmentId != '' && this.model.ModuleGroupId != null && this.model.ModuleGroupId != '') {
      this.moduleGrTime.DepartmentId = this.model.DepartmentId;
      this.moduleGrTime.ModuleGroupId = this.model.ModuleGroupId;
      this.moduleGrTime.Status = 0;
      if (this.moduleGrTaskTimeStandId == null || this.moduleGrTaskTimeStandId == '') {
        this.taskTimeStandardService.createModuleGroupTimeStandard(this.moduleGrTime).subscribe(
          data => {
            this.messageService.showSuccess('Cấu hình thành công!');
            this.status = 0;
            //this.searchTaskTimeStandard();
          },
          error => {
            this.messageService.showError(error);
          }
        );
      } else {
        this.moduleGrTime.Id = this.moduleGrTaskTimeStandId;
        this.taskTimeStandardService.updateModuleGroupTimeStandard(this.moduleGrTime).subscribe(
          data => {
            this.messageService.showSuccess('Cấu hình thành công!');
            this.status = 0;
            //this.searchTaskTimeStandard();
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }

    }

  }

  CaculateAverageInYear() {
    let activeModal = this.modalService.open(TaskTimeStandardChooseYearModalComponent, { container: 'body', windowClass: 'task-time-standard-choose-year-modal', backdrop: 'static' })
    activeModal.result.then((result) => {
      if (result.DateFrom != '') {
        this.calculateAverageModel.DateFrom = this.dateUtils.convertObjectToDate(result.DateFrom)
      }
      if (result.DateTo != '') {
        this.calculateAverageModel.DateTo = this.dateUtils.convertObjectToDate(result.DateTo)
      }
      this.calculateAverageModel.DepartmentId = this.model.DepartmentId;
      this.calculateAverageModel.ModuleGroupId = this.model.ModuleGroupId;
      this.calculateAverageModel.SBUId = this.model.SBUId;
      this.calculateAverageModel.Name = this.model.Name;
      this.calculateAverageModel.IsCalculate = result.IsCalculate;
      this.calculateAverageModel.IsExportExcel = result.IsExportExcel;
      this.calculateAverageModel.IsImport = result.IsImport;
      if (this.calculateAverageModel.IsImport) {
        var fileImport = result.FileImport;
        this.taskTimeStandardService.importExcelTaskTimeStandard(fileImport).subscribe(
          data => {
            if (data == "OK") {
              this.messageService.showSuccess('Import file thành công!');
            }
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
      else {
        this.taskTimeStandardService.calculateAverageTaskTimeStandard(this.calculateAverageModel).subscribe(
          data => {
            if (data != "") {
              if (this.calculateAverageModel.IsExportExcel) {
                var link = document.createElement('a');
                link.setAttribute("type", "hidden");
                link.href = this.config.ServerApi + data;
                link.download = 'Download.docx';
                document.body.appendChild(link);
                // link.focus();
                link.click();
                document.body.removeChild(link);
              }
            }
            else {
              this.messageService.showMessage("Không có dữ liệu để tính toán!");
            }
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
    }, (reason) => {
    });
  }

  importTaskTimeStandard() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.taskTimeStandardService.importTaskTimeStandard(data).subscribe(
          data => {
            this.searchTaskTimeStandard();
            this.messageService.showSuccess('Import thời gian tiêu chuẩn thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }
}
