import { Component, OnInit, ViewChild } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, ComboboxService } from 'src/app/shared';
import { TaskModuleGroupService } from '../../service/task-module-group.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { DxTreeListComponent } from 'devextreme-angular';

@Component({
  selector: 'app-task-module-group-create',
  templateUrl: './task-module-group-create.component.html',
  styleUrls: ['./task-module-group-create.component.scss']
})
export class TaskModuleGroupCreateComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: TaskModuleGroupService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private comboboxService: ComboboxService
  ) { }

  ngOnInit() {
    this.getListTask();
    this.getListModuleGroup();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa cấu hình công việc - nhóm module';
      this.ModalInfo.SaveText = 'Lưu';
      this.getTaskModuleGroupInfo();

    }
    else {
      this.ModalInfo.Title = "Thêm mới cấu hình công việc - nhóm module";
    }
  }
  ModalInfo = {
    Title: 'Thêm mới cấu hình công việc - nhóm module',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    TaskId: '',
    ModuleGroupId: '',
  }
  ListTask: any = [];
  ListModuleGroup: any = [];
  //get combobox
  getListTask() {
    this.comboboxService.getListTask().subscribe(
      data => {
        this.ListTask = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListModuleGroup() {
    this.comboboxService.getCbbModuleGroup().subscribe(
      data => {
        this.ListModuleGroup = data.ListResult;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getTaskModuleGroupInfo() {
    this.service.getTaskModuleGroupInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }
  createTaskModuleGroup(isContinue) {
    this.addTaskModuleGroup(isContinue);
  }

  addTaskModuleGroup(isContinue) {
    this.service.createTaskModuleGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            TaskId: '',
            ModuleGroupId: '',
          };
          this.messageService.showSuccess('Thêm mới cấu hình công việc - nhóm module thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới cấu hình công việc - nhóm module thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateTaskModuleGroup();
    }
    else {
      this.createTaskModuleGroup(isContinue);
    }
  }

  saveTaskModuleGroup() {
    this.service.updateTaskModuleGroup(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật cấu hình công việc - nhóm module thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateTaskModuleGroup() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.saveTaskModuleGroup();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.saveTaskModuleGroup();
        },
        error => {
          
        }
      );
    }
  }

  treeBoxValue: string[];
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];

  syncTreeViewSelection(e) {
    var component = (e && e.component) || (this.treeView && this.treeView.instance);
  }
  onRowDblClick() {
    this.isDropDownBoxOpened = false;

  }
  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.ModuleGroupId = e.selectedRowKeys[0];
    //this.model.ModuleGroupId = e.selectedRowKeys.Id;
  }
  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

}
