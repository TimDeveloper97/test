import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Configuration, MessageService, AppSetting, Constants, ComponentService } from 'src/app/shared';

import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { DxTreeListComponent } from 'devextreme-angular';
import { ModuleServiceService } from '../../services/module-service.service';
import { ModuleGroupService } from '../../services/module-group-service';
import { ModuleGroupCreateComponent } from '../../modulegroup/module-group-create/module-group-create.component';

@Component({
  selector: 'app-module-manage',
  templateUrl: './module-manage.component.html',
  styleUrls: ['./module-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class ModuleManageComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private router: Router,
    private config: Configuration,
    private messageService: MessageService,
    private modalService: NgbModal,
    public appSetting: AppSetting,
    private moduleServiceService: ModuleServiceService,
    private moduleGroupService: ModuleGroupService,
    public constant: Constants,
    private componentService: ComponentService
  ) {
    this.pagination = Object.assign({}, appSetting.Pagination);
    this.items = [
      { Id: 1, text: 'Thêm', icon: 'fa fa-plus text-success' },
      { Id: 2, text: 'Sửa', icon: 'fa fa-edit text-warning' },
      { Id: 3, text: 'Xóa', icon: 'fas fa-times text-danger' }
    ];
  }

  fileTemplate = this.config.ServerApi + 'Template/Import_SyncSaleModule_Template.xls';
  ModalInfo = {
    Title: 'Module',
    SaveText: 'Lưu',
  };

  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };

  StartIndex = 0;
  priceMax = 0;
  leadtimeMax = 0;
  items: any;
  pagination;
  LstpageSize = [5, 10, 15, 20, 25, 30];
  ListModuleGroup: any[] = [];
  listModule: any[] = [];
  listFile: any[] = [
    { Id: 0, Name: 'TK Cơ khí' },
    { Id: 1, Name: 'TK Điện' },
    { Id: 2, Name: 'TK Điện tử' },
    { Id: 3, Name: 'In Film' },
    { Id: 4, Name: 'HMI' },
    { Id: 5, Name: 'PLC' },
    { Id: 6, Name: 'Phần mềm' },
  ]

  modelModelGroup: any = {
    Id: '',
    Code: '',
    Name: '',
    ParentId: '',
    TotalItems: 0,
  }

  modelAll: any = {
    Id: '',
    Name: 'Tất cả',
    Code: '',
  }

  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Tìm kiếm theo mã/tên module ...',
    Items: [
      {
        Name: 'Mô tả',
        FieldName: 'Note',
        Placeholder: 'Nhập mô tả module ...',
        Type: 'text'
      },
      {
        Name: 'Loại tài liệu',
        FieldName: 'File',
        Placeholder: 'Loại tài liệu',
        Type: 'select',
        Data: this.listFile,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng dữ liệu',
        FieldName: 'IsEnought',
        Placeholder: 'Tình trạng dữ liệu',
        Type: 'select',
        Data: this.constant.ModuleIsEnought,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tình trạng sử dụng',
        FieldName: 'Status',
        Placeholder: 'Tình trạng sử dụng',
        Type: 'select',
        Data: this.constant.ModuleStatus,
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
        RelationIndexTo: 5,
        Permission: ['F020105'],
      },
      {
        Name: 'Phòng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Phòng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        RelationIndexFrom: 4,
        Permission: ['F020105'],
      },
      {
        Name: 'Chuyển thư viện',
        FieldName: 'IsSendSale',
        Placeholder: 'Chọn tình trạng chuyển thư viện',
        Type: 'select',
        Data: this.constant.SendSale,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };

  modelModule: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    TotalItemExten: 0,
    Date: null,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    ModuleGroupId: '',
    Code: '',
    Name: '',
    Note: '',
    Status: '',
    State: '',
    Specification: '',
    FileElectric: '',
    FileElectronic: '',
    FileMechanics: '',
    FileProgram: '',
    Pricing: '',
    OrderTime: '',
    IsHMI: '',
    IsPLC: '',
    CurrentVersion: '',
    UpdateDate: '',
    IsEnought: '',
    File: '',
    SBUId: '',
    DepartmentId: '',
    IsManual: '',
    ManualExist: '',
    IsSendSale: null,
  }

  moduleGroupId: '';
  selectedModelGroupId = '';
  height = 0;
  selectGroupKeys: any[] = [];
  expandGroupKeys: any[] = [];

  ngOnInit() {
    this.modelModule.SBUId = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
    this.modelModule.DepartmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;
    this.height = window.innerHeight - 140;
    this.appSetting.PageTitle = "Module";

    this.selectedModelGroupId = localStorage.getItem("selectedModuleGroupId");

    this.searchModuleGroup();
    this.searchModule(this.selectedModelGroupId);
  }

  itemClick(e) {
    if (this.moduleGroupId == '' || this.moduleGroupId == null) {
      this.messageService.showMessage("Đây không phải nhóm module!")
    } else {
      if (e.itemData.Id == 1) {
        this.showCreateUpdate(this.moduleGroupId, false);
      }
      else if (e.itemData.Id == 2) {
        this.showCreateUpdate(this.moduleGroupId, true);
      }
      else if (e.itemData.Id == 3) {
        this.showConfirmDeleteModuleGroup(this.moduleGroupId);
      }
    }
  }

  clear() {
    this.modelModule = {
      PageSize: 10,
      totalItems: 0,
      TotalItemExten: 0,
      Date: null,
      PageNumber: 1,
      OrderBy: 'Code',
      OrderType: true,

      Id: '',
      ModuleGroupId: '',
      Code: '',
      Name: '',
      Note: '',
      IsEnought: '',
      State: '',
      Specification: '',
      FileElectric: '',
      FileElectronic: '',
      FileMechanics: '',
      FileProgram: '',
      Pricing: '',
      OrderTime: '',
      IsHMI: '',
      IsPLC: '',
      CurrentVersion: '',
      UpdateDate: '',
      File: '',
      Status: '',
      IsSendSale: null,
    }
    this.searchModuleGroup();
    this.searchModule(this.moduleGroupId);
  }

  searchModuleGroup() {
    this.moduleGroupService.searchModuleGroup(this.modelModelGroup).subscribe((data: any) => {
      if (data.ListResult) {
        this.ListModuleGroup = data.ListResult;
        this.ListModuleGroup.unshift(this.modelAll);
        this.modelModelGroup.TotalItems = data.TotalItem;

        this.setSelectGroup();
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  setSelectGroup() {
    if (!this.selectedModelGroupId) {
      this.selectedModelGroupId = '';
    }
    else {
      var parentId = '';
      this.expandGroupKeys = [];
      this.ListModuleGroup.forEach(element => {
        if (element.Id == this.selectedModelGroupId) {
          parentId = element.ParentId;
        }
      });

      this.setExpandKey(parentId);
    }

    this.selectGroupKeys = [this.selectedModelGroupId];
  }

  setExpandKey(parentId) {
    var group;
    this.ListModuleGroup.forEach(element => {
      if (element.Id == parentId) {
        group = element;
      }
    });

    if (group) {
      this.expandGroupKeys.push(group.Id);
      this.setExpandKey(group.ParentId);
    }
  }

  searchModule(moduleGroupId: string) {
    this.modelModule.ModuleGroupId = moduleGroupId;
    this.moduleServiceService.SearchModul(this.modelModule).subscribe((data: any) => {
      if (data.ListResult) {
        this.StartIndex = ((this.modelModule.PageNumber - 1) * this.modelModule.PageSize + 1);
        this.listModule = data.ListResult;
        this.modelModule.totalItems = data.TotalItem;
        if (this.checkeds) {
          this.listModule.forEach(element => {
            element.Checked = true;
          });
        }
        this.priceMax = data.Status5;
        this.leadtimeMax = data.Status1;
        this.modelModule.State = data.State;
        this.modelModule.TotalNoFile = data.TotalNoFile;
        this.modelModule.Date = data.Date;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  updateModuleIsDMTV() {
    this.modelModule.ListModule = this.listModule;
    this.moduleServiceService.UpdateModuleIsDMTV(this.modelModule).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật danh sách module thành công!');
      }, error => {
        this.messageService.showError(error);
      });
  }

  ShowCreate() {
    localStorage.setItem("selectedModuleGroupId", this.selectedModelGroupId);
    for (var item of this.ListModuleGroup) {
      if (item.Id == this.selectedModelGroupId) {
        localStorage.setItem("ModuleGroupCode", item.Code);
        break;
      }
    }
    this.router.navigate(['module/quan-ly-module/them-moi-module']);
  }

  ShowUpdate(Id: string) {
    localStorage.setItem("selectedModuleGroupId", this.selectedModelGroupId);
    this.router.navigate(['module/quan-ly-module/chinh-sua-module/', Id]);
  }

  exportExcel() {
    this.moduleServiceService.ExportExcel(this.modelModule).subscribe(d => {
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

  showConfirmDelete(Id: string) {
    localStorage.setItem("selectedModelGroupId", this.selectedModelGroupId);
    this.messageService.showConfirm("Bạn có chắc muốn xoá module này không?").then(
      data => {
        this.delete(Id);
      },
      error => {

      }
    );
  }

  delete(Id: string) {
    this.moduleServiceService.DeleteGroupModul({ Id: Id }).subscribe(
      data => {
        this.selectedModelGroupId = localStorage.getItem("selectedModelGroupId");
        this.searchModule(this.selectedModelGroupId);
        this.messageService.showSuccess('Xóa module thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    // this.selectedModelGroupId = e.selectedRowKeys[0];
    // this.searchModule(e.selectedRowKeys[0]);
    // this.moduleGroupId = e.selectedRowKeys[0];

    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedModelGroupId) {
      this.selectedModelGroupId = e.selectedRowKeys[0];
      this.searchModule(e.selectedRowKeys[0]);
      this.moduleGroupId = e.selectedRowKeys[0];
      localStorage.setItem("selectedModuleGroupId", this.selectedModelGroupId);
    }
  }


  showConfirmDeleteModuleGroup(Id: string) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa nhóm module này không?").then(
      data => {
        this.modelModelGroup.Id = Id;
        this.searchModuleGroupById(Id);
      },
      error => {

      }
    );
  }
  logUserId: string;
  deleteModuleGroup(Id: string) {
    this.moduleGroupService.deleteModuleGroup({ Id: Id, LogUserId: this.logUserId }).subscribe(
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
  showCreateUpdate(Id: string, isUpdate: boolean) {
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
  searchModuleGroupById(Id: string) {
    this.moduleGroupService.searchModuleGroupById(this.modelModelGroup).subscribe((data: any) => {
      if (data) {
        this.listDAById = data;
        this.modelModelGroup.TotalItems = data.TotalItem;
        if (this.listDAById.length == 1) {
          this.deleteModuleGroup(Id);
        } else {
          this.messageService.showConfirm("Xóa nhóm module này sẽ xóa hết cả các nhóm module con thuộc nhóm, Bạn có chắc chắn muốn xóa không?").then(
            data => {
              this.deleteModuleGroup(Id);
            },
            error => {

            }
          );
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });

  }

  fileImport: any;
  showImportSyncSaleModule() {
    this.componentService.showImportExcel(this.fileTemplate, false).subscribe(data => {
      if (data) {
        this.fileImport = data;
        this.moduleServiceService.importSyncSaleModule(data, this.isConfirm).subscribe(
          data => {
            if (data) {
              this.importConfirm(data);
            } else {
              this.searchModule(this.moduleGroupId);
              this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
            }
          },
          error => {
            this.messageService.showError(error);
          });
      }
    });
  }

  importConfirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.moduleServiceService.importSyncSaleModule(this.fileImport, true,).subscribe(
          data => {
            this.searchModule(this.moduleGroupId);
              this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
          },
          error => {
            this.messageService.showError(error);
          });
      }
    );
  }

  checkeds: boolean = false;
  listCheck: any[] = [];
  selectAllFunction() {
    this.listModule.forEach(element => {
      element.Checked = this.checkeds;
    });
  }

  pushChecker(row: any) {
    if (row.Checked) {
      this.listCheck.push(row);
    } else {
      this.checkeds = false;
      this.listCheck.splice(this.listCheck.indexOf(row), 1);
    }
  }

  isConfirm: boolean = false;
  syncSaleModule(isConfirm: boolean) {
    let list = [];
    this.listCheck.forEach(element => {
      list.push(element.Id);
    });
    this.moduleServiceService.syncSaleModule(this.checkeds, isConfirm, list).subscribe(
      data => {
        if (data) {
          this.confirm(data);
        } else {
          this.searchModule(this.moduleGroupId);
          this.messageService.showSuccess('Đồng bộ sản phẩm kinh doanh thành công!');
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  confirm(message: string) {
    this.messageService.showConfirm(message).then(
      data => {
        this.syncSaleModule(true);
      }
    );
  }
}
