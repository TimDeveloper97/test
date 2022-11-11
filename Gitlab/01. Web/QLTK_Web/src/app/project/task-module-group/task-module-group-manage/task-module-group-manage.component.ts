import { Component, OnInit } from '@angular/core';

import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TaskModuleGroupService } from '../../service/task-module-group.service';
import { TaskModuleGroupCreateComponent } from '../task-module-group-create/task-module-group-create.component';

@Component({
  selector: 'app-task-module-group-manage',
  templateUrl: './task-module-group-manage.component.html',
  styleUrls: ['./task-module-group-manage.component.scss']
})
export class TaskModuleGroupManageComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private taskModuleGroupService: TaskModuleGroupService,
    public constant: Constants,
    public comboboxService : ComboboxService
  ) { }

  StartIndex = 0;
  listData: any[] = [];
  model: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'Name',
    OrderType: true,
    Id: '',
    TaskId: '',
    ModuleGroupId: '',
  }

  listTask: any = [];
  listModuleGroup: any = [];
  
  ngOnInit() {
    this.appSetting.PageTitle = "Cấu hình công việc - nhóm module";
    this.searchTaskModuleGroup();
    this.getListTask();
    this.getListGroupModule();
  }
  
  //get combobox
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

  getListGroupModule() {
    this.comboboxService.getCbbModuleGroup().subscribe((data: any) => {
      if (data) {
        this.listModuleGroup = data.ListResult;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchTaskModuleGroup() {
    this.taskModuleGroupService.searchTaskModuleGroups(this.model).subscribe((data: any) => {
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
      OrderBy: '',
      OrderType: true,
      Id: '',
      TaskId: '',
      ModuleGroupId: '',
    }
    this.searchTaskModuleGroup();
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(TaskModuleGroupCreateComponent, { container: 'body', windowClass: 'task-module-group-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchTaskModuleGroup();
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteTaskModuleGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá công việc - nhóm module này không?").then(
      data => {
        this.deleteTaskModuleGroup(Id);
      },
      error => {
        
      }
    );
  }

  deleteTaskModuleGroup(Id: string) {
    this.taskModuleGroupService.deleteTaskModuleGroup({ Id: Id }).subscribe(
      data => {
        this.searchTaskModuleGroup();
        this.messageService.showSuccess('Xóa công việc - nhóm module thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

}
