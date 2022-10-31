import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { FlowStageService } from '../../service/flow-stage.service';
import { TaskService } from '../../service/task.service';
import { FlowStageCreateComponent } from '../flow-stage-create/flow-stage-create.component';
import { TaskFlowStageCreateComponent } from '../task-flow-stage-create/task-flow-stage-create.component';

@Component({
  selector: 'app-task-manage',
  templateUrl: './task-manage.component.html',
  styleUrls: ['./task-manage.component.scss']
})
export class TaskManageComponent implements OnInit {
  items: any;
  constructor(public appSetting: AppSetting,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    public constant: Constants,
    private flowStageService: FlowStageService,
    private taskService: TaskService,
    private router: Router,) {
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }

  flowStageSearchModel: any = {
    page: 1,
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Name: '',
    Code: '',
  };

  taskSearchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Status1: 0,
    Status2: 0,
    OrderBy: 'CreateDate',
    OrderType: true,

    Name: '',
    Code: '',
    FlowStageId: '',
    Type: null,
    IsDesignModule: null,
    SBUId: '',
    DepartmentId: ''
  };

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên',
    Items: [
      {
        Name: 'Mã công việc',
        FieldName: 'Code',
        Placeholder: 'Nhập mã công việc ...',
        Type: 'text'
      },
      {
        Name: 'Loại công việc',
        FieldName: 'Type',
        Placeholder: 'Loại công việc',
        Type: 'select',
        Data: this.constant.TaskTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Thiết kế module nguồn',
        FieldName: 'IsDesignModule',
        Placeholder: 'Thiết kế module nguồn',
        Type: 'select',
        Data: this.constant.TaskIsDesignModule,
        DisplayName: 'Name',
        ValueName: 'Id'
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
        RelationIndexTo: 4,
        // Permission: ['F030405'],
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 3,
        // Permission: ['F030405'],
      },
      {
        Name: 'Trình độ',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 3,
        // Permission: ['F030405'],
      },
      {
        Name: 'Công việc theo dự án',
        FieldName: 'IsProjectWork',
        Placeholder: 'Công việc theo dự án',
        Type: 'select',
        Data: this.constant.TaskIsDesignModule,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Kết quả đầu vào',
        FieldName: 'CodeInput',
        Placeholder: 'Nhập kết quả đầu vào ...',
        Type: 'text'
      },
      {
        Name: 'Kết quả đầu ra',
        FieldName: 'CodeOutput',
        Placeholder: 'Nhập kết quả đầu ra ...',
        Type: 'text'
      },
    ]
  };

  height = 0;
  expandGroupKeys: any[] = [];
  selectGroupKeys: any[] = [];
  selectedGroupId: any = '';
  flowStages: any[] = [];
  startIndex = 1;
  tasks: any[] = [];

  ngOnInit(): void {
    this.appSetting.PageTitle = "Quản lý công việc";
    this.height = window.innerHeight - 140;
    this.searchFlowStage();
  }

  searchFlowStage() {
    this.flowStageService.search(this.flowStageSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.flowStages = data.ListResult;
        let modelAll: any = {
          Id: '',
          Name: 'Tất cả',
          Code: '',
        }
        this.flowStages.unshift(modelAll);
        this.flowStageSearchModel.TotalItems = data.TotalItem;

        this.searchTask();
        this.selectGroupKeys = [this.selectedGroupId];
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchTask() {
    this.taskSearchModel.FlowStageId = this.selectedGroupId;
    this.taskService.search(this.taskSearchModel).subscribe((data: any) => {
      if (data.ListResult) {
        this.tasks = data.ListResult;
        this.taskSearchModel.TotalItems = data.TotalItem;
        this.startIndex = ((this.taskSearchModel.PageNumber - 1) * this.taskSearchModel.PageSize + 1);
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedGroupId) {
      this.selectedGroupId = e.selectedRowKeys[0];
      this.searchTask();
      localStorage.setItem("selectedFlowStageId", this.selectedGroupId);
    }
  }

  itemClick(e) {
    if (this.selectedGroupId == '' || this.selectedGroupId == null) {
      this.messageService.showMessage("Đây không phải dòng chảy!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreatUpdateFlowStage(this.selectedGroupId, false);
      }
      else if (e.itemData.Id == 2) {
        this.showCreatUpdateFlowStage(this.selectedGroupId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteFlowStage(this.selectedGroupId);
      }
    }
  }

  showCreatUpdateFlowStage(id: any, isUpdate: boolean) {
    let activeModal = this.modalService.open(FlowStageCreateComponent, { container: 'body', windowClass: 'flow-stage-create-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.id = this.selectedGroupId;
    } else {
      activeModal.componentInstance.parentId = id;
    }

    activeModal.result.then((result) => {
      if (result) {
        this.searchFlowStage();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteFlowStage(flowStageId: any) {
    this.messageService.showConfirm("Bạn có chắc muốn dòng chảy này không?").then(
      data => {
        this.deleteFlowStage(flowStageId);
      },
      error => {

      }
    );
  }

  deleteFlowStage(flowStageId: any) {
    this.flowStageService.delete({ Id: flowStageId }).subscribe(
      data => {
        this.searchFlowStage();
        this.messageService.showSuccess('Xóa dòng chảy thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.taskSearchModel = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      Status1: 0,
      Status2: 0,
      OrderBy: 'CreateDate',
      OrderType: true,

      Name: '',
      Code: '',
      FlowStageId: '',
      Type: null,
      IsDesignModule: null
    };
    this.searchTask();
  }

  showCreateUpdateTask(id: string) {
    if (id) {
      this.router.navigate(['nhan-vien/quan-ly-cong-viec/chinh-sua/', id]);
    } else {
      this.router.navigate(['nhan-vien/quan-ly-cong-viec/them-moi'], { queryParams: { flowId: this.taskSearchModel.FlowStageId } });
    }
  }

  showConfirmDeleteTask(id: any) {
    this.messageService.showConfirm("Bạn có chắc muốn công việc này không?").then(
      data => {
        this.deleteFlowTask(id);
      },
      error => {

      }
    );
  }

  deleteFlowTask(id: any) {
    this.taskService.delete({ Id: id }).subscribe(
      data => {
        this.searchFlowStage();
        this.messageService.showSuccess('Xóa công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  exportExcel() {
    this.taskService.exportExcel(this.taskSearchModel).subscribe(d => {
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
