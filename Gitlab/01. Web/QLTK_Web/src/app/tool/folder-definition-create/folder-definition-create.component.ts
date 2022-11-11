import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { FolderDefinitionService } from '../services/folder-definition.service';

@Component({
  selector: 'app-folder-definition-create',
  templateUrl: './folder-definition-create.component.html',
  styleUrls: ['./folder-definition-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FolderDefinitionCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private folderDefinitionService: FolderDefinitionService,
    private comboboxService: ComboboxService,
  ) { }

  infoModal = {
    Title: 'Thêm mới thư mục',
    SaveText: 'Lưu',
  };

  statusCheckFile = false;
  statusUpload = false;
  statusCheckFolder = false;
  folderDefinitionManageId: string;
  typeDefinitionId: number;
  objectType: number;

  listFolderDefinitionBetween: any[] = [
    { Id: 0, Name: '' },
    { Id: 1, Name: 'Mã nhóm sản phẩm' },
    { Id: 2, Name: 'Mã sản phẩm' },
    { Id: 3, Name: 'Mã nhóm cha sản phẩm' },
    { Id: 4, Name: '( )' }
  ]

  listFolderDefinitionBetweenIndex: any[] = [
    { Id: 0, Name: '' },
    { Id: 1, Name: 'Từ 01-99' },
    { Id: 2, Name: 'Từ a-z' },
  ]

  folderDefinitionModel = {
    FolderDefinitionId: '',
    TypeDefinitionId: 0,
    FolderDefinitionManageId: '',
    FolderDefinitionFirst: '',
    FolderDefinitionBetween: '',
    FolderDefinitionLast: '',
    FolderDefinitionBetweenIndex: '',
    StatusCheckFile: 0,
    FolderType: 0,
    StatusCheckFolder: 0,
    CheckExtensionFile: false,
    ExtensionFile: '',
    ObjectType: 1,
    SBUId:'',
    DepartmentId:''
  };

  ngOnInit() {
    this.folderDefinitionModel.FolderDefinitionManageId = this.folderDefinitionManageId;
    this.folderDefinitionModel.TypeDefinitionId = this.typeDefinitionId;
    this.folderDefinitionModel.ObjectType = this.objectType;
    this.getCbbSBU();
  }

  listSBU:[];
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

  listDepartment: [];
  getCbbDepartment() {
    this.folderDefinitionModel.DepartmentId = '';
    this.comboboxService.getCbbDepartmentBySBU(this.folderDefinitionModel.SBUId).subscribe(
      data => {
        this.listDepartment = data;
        this.folderDefinitionModel.DepartmentId = null;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createModuleDesignDocument() {
    if (
      !this.folderDefinitionModel.FolderDefinitionFirst &&
      !this.folderDefinitionModel.FolderDefinitionBetween &&
      !this.folderDefinitionModel.FolderDefinitionLast &&
      !this.folderDefinitionModel.FolderDefinitionBetweenIndex) {
      this.messageService.showConfirm("Bạn chưa định nghĩa tên thư mục");
      return;
    }

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
    this.folderDefinitionService.createFolderDefinition(this.folderDefinitionModel).subscribe(
      data => {
        this.messageService.showSuccess('Thêm mới thư mục thành công!');
        this.closeModal();
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  save() {
    this.createModuleDesignDocument();
  }

  closeModal() {
    this.activeModal.close(true);
  }

}
