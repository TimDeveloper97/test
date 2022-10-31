import { Component, OnInit, ViewChild } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TaskService } from '../../service/task.service';
import { TaskCreateComponent } from '../task-create/task-create.component';
import { ModuleGroupService } from 'src/app/module/services/module-group-service';
import { DxTreeListComponent } from 'devextreme-angular';
import { ModuleGroupCreateComponent } from 'src/app/module/modulegroup/module-group-create/module-group-create.component';
import { TaskModuleGroupService } from '../../service/task-module-group.service';


@Component({
  selector: 'app-task-manage',
  templateUrl: './task-manage.component.html',
  styleUrls: ['./task-manage.component.scss']
})
export class TaskManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private taskService: TaskService,
    private serviceTaskModuleGroup: TaskModuleGroupService,
    private moduleGroupService: ModuleGroupService,
    public constant: Constants
  ) {
    this.items = [
      { Id: 1, text: 'Thêm mới nhóm module', icon: 'fas fa-plus' },
      { Id: 2, text: 'Chỉnh sửa nhóm module', icon: 'fa fa-edit' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times' }
    ];
  }
  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo tên công việc',
    Items: [
      {
        Name: 'Loại công việc',
        FieldName: 'Type',
        Placeholder: 'Loại công việc',
        Type: 'select',
        Data: this.constant.StatusTask,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
      },
    ]
  };
  CheckLoading = 0;
  checkeds = false;
  items: any;
  StartIndex = 0;
  listData: any[] = [];
  model: any = {

    Id: '',
    Name: '',
    Type: '',
    Checked: '',
    ModuleGroupId: null,
  }

  listType = [{ Id: '1', Name: 'Thiết kế' },
  { Id: '2', Name: 'Tài liệu' },
  { Id: '3', Name: 'Chuyển giao' },
  { Id: '4', Name: 'Giải pháp' },
  { Id: '5', Name: 'Hỗ trợ' },
  ]
  height = 0;

  ngOnInit() {
    this.height = window.innerHeight - 150;
    this.appSetting.PageTitle = "Cấu hình công việc theo nhóm module";
    this.searchModuleGroup();
    this.searchTask('');
  }
  ListChecker = [];
  searchTask(moduleGroupId: string) {
    this.model.ModuleGroupId = moduleGroupId;
    this.taskService.searchTask(this.model).subscribe((data: any) => {
      let dem = 0;
      if (data.ListResult) {
        // this.StartIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        data.ListResult.forEach(element => {
          if (element.Checked == true) {
            dem++;
          }
        });

        if (data.ListResult && data.TotalItem == 0) {
          this.listData = [];
        }
        else if (dem > 0) {
          this.listData = data.ListResult;
        }
        else {
          if ((this.ListChecker.length == 0 && moduleGroupId !== "") || this.ListChecker.length != data.TotalItem) {
            this.listData = data.ListResult;
          } else {
            this.listData = this.ListChecker;
          }
        }

        //this.listData = data.ListResult;
        this.model.TotalItems = data.TotalItem;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  pushChecker() {
    let dem = 0;
    this.CheckLoading++;
    this.ListChecker = [];
    var listCheck = [];
    var listNoCheck = [];
    this.listData.forEach(element => {
      this.ListChecker.push(element);
      if (element.Checked) {
        listCheck.push(element);
      }
      else {
        listNoCheck.push(element);
      }
    });

    var index = 0;
    listCheck.forEach(item => {
      if (index < item.Index) {
        index = item.Index;
      }
    });

    // var listResultData =[];
    // this.taskService.searchTask(this.model).subscribe((data: any) => {
    //   if (data.ListResult) {
    //     listResultData = data.ListResult;
    //   }
    // });
    // var demCheck = 0;
    // listResultData.forEach(demcheck => {
    //   if(demcheck.Checked ==true){
    //     demCheck ++;
    //   }
    // });
    if (this.CheckLoading > 1) {
      index++;
    }
    else {
      index;
    }



    var object = listCheck[Object.keys(listCheck)[Object.keys(listCheck).length - 1]];
    listNoCheck.sort(function (a, b) {
      var nameA = a.Name.toUpperCase(); // bỏ qua hoa thường
      var nameB = b.Name.toUpperCase(); // bỏ qua hoa thường
      if (nameA < nameB) {
        return -1;
      }
      if (nameA > nameB) {
        return 1;
      }
      return 0;
    });
    if (object != null) {
      object.Index = index;
    }

    listCheck.sort(function (a, b) {
      var nameA = a.Index;
      var nameB = b.Index;
      if (nameA < nameB) {
        return -1;
      }
      if (nameA > nameB) {
        return 1;
      }
      return 0;
    });
    var totalNocheck = index;
    this.listData = [];
    this.listData = listCheck;
    listNoCheck.forEach(nocheck => {
      totalNocheck++;
      nocheck.Index = totalNocheck;
      this.listData.push(nocheck);
    });
  }
  selectAllFunction() {
    let demChecked = 0;
    if (this.checkeds) {
      this.listData.forEach(element => {
        element.Checked = true;
      });
    } else {
      this.listData.forEach(element => {
        element.Checked = false;
      });
    }
    // this.listData.forEach(element => {
    //   if(element.Checked == false){
    //     demChecked ++;
    //   }
    //   if(demChecked == 0){
    //     this.checkeds = true;
    //   }
    // });

  }

  clear() {
    this.model = {
      PageSize: 10,
      TotalItems: 0,
      PageNumber: 1,
      OrderBy: 'Name',
      OrderType: true,
      Name: '',
      Type: '',
      ModuleGroupId: '',
    }
    this.searchTask(this.moduleGroupId);
  }

  showCreateUpdate(Id: string) {
    let activeModal = this.modalService.open(TaskCreateComponent, { container: 'body', windowClass: 'task-create-model', backdrop: 'static' })
    activeModal.componentInstance.Id = Id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchTask(this.moduleGroupId);
      }
    }, (reason) => {
    });
  }

  showConfirmDeleteTask(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá công việc này không?").then(
      data => {
        this.deleteTask(Id);
      },
      error => {

      }
    );
  }

  deleteTask(Id: string) {
    this.taskService.deleteTask({ Id: Id }).subscribe(
      data => {
        this.searchTask(this.moduleGroupId);
        this.messageService.showSuccess('Xóa công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  modelModelGroup: any = {
    Id: '',
    Code: '',
    Name: '',
    ParentId: '',
    TotalItems: 0,
  }

  selectedModelGroupId = '';
  listModuleGroupId = [];
  listModuleGroup: any = [];

  searchModuleGroup() {
    this.moduleGroupService.searchModuleGroup(this.modelModelGroup).subscribe((data: any) => {
      if (data.ListResult) {
        //this.StartIndex = ((this.modelMaterialGroup.PageNumber - 1) * this.modelMaterialGroup.PageSize + 1);
        this.listModuleGroup = data.ListResult;
        this.modelModelGroup.TotalItems = data.TotalItem;
        if (this.selectedModelGroupId == null) {
          this.selectedModelGroupId = this.listModuleGroup[0].Id;
        }
        this.treeView.selectedRowKeys = [this.selectedModelGroupId];
        for (var item of this.listModuleGroup) {
          this.listModuleGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmDeleteModuleGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa nhóm module này không?").then(
      data => {
        this.model.Id = Id;
        this.confirmDeleteModuleGroup(Id);
      },
      error => {

      }
    );
  }

  deleteModuleGroup(Id: string) {
    this.moduleGroupService.deleteModuleGroup({ Id: Id }).subscribe(
      data => {
        //this.check=true;
        this.modelModelGroup.Id = '';
        this.searchModuleGroup();
        this.messageService.showSuccess('Xóa nhóm module thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  // popup thêm mới và chỉnh sửa
  showCreateUpdateModuleGroup(Id: string, isUpdate: boolean) {
    let activeModal = this.modalService.open(ModuleGroupCreateComponent, { container: 'body', windowClass: 'modulegroupgreate-model', backdrop: 'static' })
    if (isUpdate) {
      activeModal.componentInstance.idUpdate = Id;
    } else {
      activeModal.componentInstance.parentId = Id;
    }

    activeModal.result.then((result) => {
      if (result) {
        this.searchModuleGroup();
      }
    }, (reason) => {
    });
  }

  //xoá nhóm module
  listDAById: any[] = [];
  confirmDeleteModuleGroup(Id: string) {
    var checkDelete = false;
    for (let item of this.listModuleGroup) {
      if (item.ParentId == Id) {
        checkDelete = true;
      }
    }
    if (checkDelete == true) {
      this.messageService.showConfirm("Xóa nhóm module này sẽ xóa hết cả các nhóm module con thuộc nhóm, Bạn có chắc chắn muốn xóa không?")
        .then(
          data => {
            this.deleteModuleGroup(Id);
          },
          error => {

          }
        );
    } else {
      this.deleteModuleGroup(Id);
    }
  }

  moduleGroupId: '';
  onSelectionChanged(e) {
    // this.selectedModelGroupId = e.selectedRowKeys[0];
    // //this.searchModule(e.selectedRowKeys[0]);
    // this.moduleGroupId = e.selectedRowKeys[0];
    // this.model.ModuleGroupId = this.selectedModelGroupId;
    // this.searchTask(this.moduleGroupId);
    // this.pushChecker();
    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedModelGroupId) {
      this.selectedModelGroupId = e.selectedRowKeys[0];
      this.searchTask(e.selectedRowKeys[0]);
      this.moduleGroupId = e.selectedRowKeys[0];
      this.model.ModuleGroupId = this.selectedModelGroupId;
      this.CheckLoading = 0;
      this.pushChecker();
    }
  }

  modelTaskModule: any = {
    Id: '',
    ModuleGroupId: '',
    ListTickTask: [],
  }

  save() {
    this.listData.forEach(element => {
      if (element.Checked) {
        this.modelTaskModule.ListTickTask.push(element);
      }
    });
    this.modelTaskModule.ModuleGroupId = this.selectedModelGroupId;
    this.serviceTaskModuleGroup.createTaskModuleGroup(this.modelTaskModule).subscribe(
      data => {
        this.searchTask(this.moduleGroupId);
        this.modelTaskModule = {
          Id: '',
          ListTickTask: [],
          ModuleGroupId: '',
        };
        this.messageService.showSuccess('Thêm mới cấu hình công việc - nhóm module!');
      });
  }

  typeId: number;
  /// Skien click chuột phải
  itemClick(e) {
    if (e.itemData.Id == 1) {
      this.showCreateUpdateModuleGroup(this.moduleGroupId, false);
    } else if (e.itemData.Id == 2) {
      this.showCreateUpdateModuleGroup(this.moduleGroupId, true);
    } else if (e.itemData.Id == 3) {
      this.showConfirmDeleteModuleGroup(this.moduleGroupId);
    }
  }
}
