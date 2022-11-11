import { Component, OnInit, ViewChild, ViewEncapsulation, AfterViewInit} from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DxTreeListComponent } from 'devextreme-angular';

import { Constants, MessageService, AppSetting, ComboboxService, PermissionService } from 'src/app/shared';
import { FolderDefinitionService } from '../services/folder-definition.service';
import { FileDefinitionService } from '../services/file-definition.service';
import { FolderDefinitionCreateComponent } from '../folder-definition-create/folder-definition-create.component';

@Component({
  selector: 'app-folder-definition',
  templateUrl: './folder-definition.component.html',
  styleUrls: ['./folder-definition.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FolderDefinitionComponent implements OnInit, AfterViewInit {

  @ViewChild(DxTreeListComponent) treeView;

  constructor(
    public constant: Constants,
    public appSetting: AppSetting,
    private folderDefinitionService: FolderDefinitionService,
    private fileDefinitionService: FileDefinitionService,
    private messageService: MessageService,
    private modalService: NgbModal,
    private comboboxService: ComboboxService,
    public permissionService: PermissionService
  ) {
    this.items = [
      { Id: 1, text: 'Thêm mới thư mục' },
      { Id: 2, text: 'Thêm mới thư mục con' },
      { Id: 3, text: 'Xóa' }
    ];
  }

  startIndex = 1;
  items: any;
  statusCheckFile = false;
  statusCheckFolder = false;
  typeId: number;
  height = 0;
  searchModel = {
    TypeDefinitionId: 1,
    ObjectType: 1,
    SBUId: '',
    DepartmentId: '',
  };

  folderDefinitionModel = {
    FolderDefinitionId: '',
    TypeDefinitionId: 1,
    FolderDefinitionManageId: '',
    FolderDefinitionFirst: '',
    FolderDefinitionBetween: '',
    FolderDefinitionLast: '',
    FolderDefinitionBetweenIndex: '',
    StatusCheckFile: 0,
    StatusCheckFolder: 0,
    CheckExtensionFile: false,
    ExtensionFile: '',
    FolderType: 0,
    ObjectType: 1,
  };

  fileDefinitionModel = {
    FileDefinitionId: '',
    FolderDefinitionId: '',
    FileDefinitionNameFirst: '',
    FileDefinitionNameBetween: '',
    FileDefinitionNameBetweenIndex: '',
    FileDefinitionNameLast: '',
    FileType: 0,
    TypeDefinitionId: 1
  };

  documentModel = {
    Id: ''
  }

  folderDefinitionId: string;
  listFolder = [];
  selectedListFolderId = '';
  listFile = [];
  listDepartment: [];
  listFolderDefinitionId = [];
  listSBU: [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  departmentColumnName: any[] = [{ Name: 'Code', Title: 'Mã Phòng ban' }, { Name: 'Name', Title: 'Tên phòng bàn' }];

  itemClick(e) {

    if (this.searchModel.DepartmentId != this.userDepartmentId) {
      this.messageService.showMessage('Bạn không được cài đặt cho phòng ban khác!');
    }
    else {
      if (e.itemData.Id == 1) {
        this.folderDefinitionId = null;
        this.typeId = this.searchModel.TypeDefinitionId;
        this.showCreateFolder();
      }
      if (e.itemData.Id == 2) {
        if (this.folderDefinitionId == null) {
          this.messageService.showMessage("Bạn chưa chọn thư mục");
        } else {
          this.typeId = this.searchModel.TypeDefinitionId;
          this.showCreateFolder();
        }
      }
      if (e.itemData.Id == 3) {
        this.showConfirmdeleteFolderDefinition(this.folderDefinitionId);
      }
    }
  }

  userDepartmentId = '';

  ngOnInit() {
    this.height = window.innerHeight - 200;
    this.appSetting.PageTitle = "Cài đặt định nghĩa thư mục";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));

    if (currentUser) {
      this.searchModel.DepartmentId = currentUser.departmentId;
      this.userDepartmentId = currentUser.departmentId;
      this.searchModel.SBUId = currentUser.sbuId;
      this.getCbbDepartment();
    }

    this.getCbbSBU();
  }

  ngAfterViewInit(){
    this.getListFolder();
  }

  getCbbSBU() {
    this.comboboxService.getCBBSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeSbu() {
    this.getCbbDepartment();
    this.getListFolder();
  }

  getCbbDepartment() {
    this.listDepartment = [];
    this.comboboxService.getCbbDepartmentBySBU(this.searchModel.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListFolder() {
    this.clearFolder();
    this.folderDefinitionService.getListFolderDefinition(this.searchModel).subscribe((data: any) => {
      if (data) {
        this.listFolder = data;
        if (this.selectedListFolderId == null) {
          this.selectedListFolderId = this.listFolder[0].FolderDefinitionId;
          this.folderDefinitionId = this.selectedListFolderId;
        }
        this.treeView.selectedRowKeys = [this.selectedListFolderId];
        this.getFolderDefinitionInfo();
        this.searchFile(this.selectedListFolderId);
        for (var item of this.listFolder) {
          this.listFolderDefinitionId.push(item.FolderDefinitionId);
        }
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  getFolderDefinitionInfo() {
    this.folderDefinitionService.getFolderDefinitionInfo({ FolderDefinitionId: this.folderDefinitionId }).subscribe(data => {
      this.folderDefinitionModel = data;
      if (this.folderDefinitionModel.StatusCheckFile == 0) {
        this.statusCheckFile = false;
      }
      else {
        this.statusCheckFile = true;
      }

      if (this.folderDefinitionModel.StatusCheckFolder == 0) {
        this.statusCheckFolder = false;
      }
      else {
        this.statusCheckFolder = true;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  onSelectionChanged(e) {
    // this.selectedListFolderId = e.selectedRowKeys[0];
    // this.searchFile(e.selectedRowKeys[0]);
    // this.folderDefinitionId = e.selectedRowKeys[0];
    // if (this.folderDefinitionId) {
    //   this.getFolderDefinitionInfo();
    // }

    if (e.selectedRowKeys[0] != null && e.selectedRowKeys[0] != this.selectedListFolderId) {
      this.selectedListFolderId = e.selectedRowKeys[0];
      this.searchFile(e.selectedRowKeys[0]);
      this.folderDefinitionId = e.selectedRowKeys[0];

      if (this.folderDefinitionId) {
        this.getFolderDefinitionInfo();
      }
    }
  }

  updateFolder() {
    if (this.statusCheckFile == false) {
      this.folderDefinitionModel.StatusCheckFile = 0;
    }
    else {
      this.folderDefinitionModel.StatusCheckFile = 1;
    }

    if (this.statusCheckFolder == false) {
      this.folderDefinitionModel.StatusCheckFolder = 0;
    }
    else {
      this.folderDefinitionModel.StatusCheckFolder = 1;
    }

    this.folderDefinitionService.updateFolderDefinition(this.folderDefinitionModel).subscribe(
      () => {
        this.messageService.showSuccess('Cập nhật thư mục thành công!');
        this.getListFolder();
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  createFile() {
    if (this.fileDefinitionModel.FileDefinitionNameFirst == "" && this.fileDefinitionModel.FileDefinitionNameBetween == "" && this.fileDefinitionModel.FileDefinitionNameBetweenIndex == "" && this.fileDefinitionModel.FileDefinitionNameLast == "") {
      this.messageService.showMessage("Tên file không được để trống");
      return
    }

    this.fileDefinitionService.createFileDefinition(this.fileDefinitionModel).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới tài liệu thành công!');
        this.searchFile(this.folderDefinitionId);
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  updateFile() {
    this.fileDefinitionService.updateFileDefinition(this.fileDefinitionModel).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật tài liệu thành công!');
        this.searchFile(this.folderDefinitionId);
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  searchFile(folderId: string) {
    this.fileDefinitionModel.FolderDefinitionId = folderId;
    this.fileDefinitionService.getListFileDefinition(this.fileDefinitionModel).subscribe((data: any) => {
      if (data) {
        this.listFile = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  selectIndex = -1;
  getFileDefinitionInfo(FileDefinitionId) {
    this.selectIndex = FileDefinitionId;
    this.fileDefinitionService.getFileDefinitionInfo({ FileDefinitionId: FileDefinitionId }).subscribe(data => {
      this.fileDefinitionModel = data;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  clearFolder() {
    this.treeView.selectedRowKeys = null;
    this.folderDefinitionId = "";
    this.listFolder = [];
    this.selectedListFolderId = null;
    this.listFile = [];
    this.listFolderDefinitionId = [];
    this.folderDefinitionModel = {
      FolderDefinitionId: '',
      TypeDefinitionId: 1,
      FolderDefinitionManageId: '',
      FolderDefinitionFirst: '',
      FolderDefinitionBetween: '',
      FolderDefinitionLast: '',
      FolderDefinitionBetweenIndex: '',
      StatusCheckFile: 0,
      StatusCheckFolder: 0,
      CheckExtensionFile: false,
      ExtensionFile: '',
      FolderType: 0,
      ObjectType: 1,
    };
  }

  clear() {
    this.fileDefinitionModel = {
      FileDefinitionId: '',
      FolderDefinitionId: '',
      FileDefinitionNameFirst: '',
      FileDefinitionNameBetween: '',
      FileDefinitionNameBetweenIndex: '',
      FileDefinitionNameLast: '',
      FileType: 0,
      TypeDefinitionId: 1
    };
    this.searchFile(this.folderDefinitionId);
  }

  showConfirmdeleteFolderDefinition(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thư mục này không?").then(
      data => {
        this.deleteFolderDefinition(Id);
      },
      error => {
        
      }
    );
  }

  deleteFolderDefinition(Id: string) {
    this.folderDefinitionService.deleteFolderDefinition({ FolderDefinitionId: Id }).subscribe(
      data => {
        this.getListFolder();
        this.messageService.showSuccess('Xóa thư mục thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showConfirmdeleteFileDefinition(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá tài liệu này không?").then(
      data => {
        this.deleteFileDefinition(Id);
      },
      error => {
        
      }
    );
  }

  deleteFileDefinition(Id: string) {
    this.fileDefinitionService.deleteFileDefinition({ FileDefinitionId: Id }).subscribe(
      data => {
        this.searchFile(this.folderDefinitionId);
        this.messageService.showSuccess('Xóa tài liệu thành công!');
        this.clear();
      },
      error => {
        this.messageService.showError(error);
      });
  }

  showCreateFolder() {
    let activeModal = this.modalService.open(FolderDefinitionCreateComponent, { container: 'body', windowClass: 'folder-definition-create', backdrop: 'static' });
    activeModal.componentInstance.folderDefinitionManageId = this.folderDefinitionId;
    activeModal.componentInstance.typeDefinitionId = this.typeId;
    activeModal.componentInstance.objectType = this.searchModel.ObjectType;
    activeModal.result.then((result) => {
      if (result) {
        this.getListFolder();
      }
    }, (reason) => {
    });
  }

}
