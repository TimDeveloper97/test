import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DesignStructureService } from '../services/designstructure-service';
import { MessageService, Constants, Configuration, PermissionService, ComboboxService } from 'src/app/shared';
import { CreateDesignStructureComponent } from '../create-design-structure/create-design-structure.component';
import { DxTreeListComponent } from 'devextreme-angular';
import { CreateDesignStructureFileComponent } from '../create-design-structure-file/create-design-structure-file.component';

@Component({
  selector: 'app-design-structure-manager',
  templateUrl: './design-structure-manager.component.html',
  styleUrls: ['./design-structure-manager.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DesignStructureManagerComponent implements OnInit {
  @ViewChild(DxTreeListComponent) treeView;
  constructor(
    private activeModal: NgbActiveModal,
    private designStructureService: DesignStructureService,
    private messageService: MessageService,
    public constant: Constants,
    private modalService: NgbModal,
    private config: Configuration,
    private comboboxService: ComboboxService,
    public permissionService: PermissionService
  ) { }
  isAction: boolean = false;
  listDesignStructure: any = [];
  columnNameSBU: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  columnNameDepartment: any[] = [{ Name: 'Code', Title: 'Mã Phòng ban' }, { Name: 'Name', Title: 'Tên Phòng ban' }];
  listSBU: any = [];
  listDepartment: any = [];

  model: any = {
    SBUId: '',
    DepartmentId: '',
    Type: 1,
    ObjectType: 1
  }
  modelFile: any = {
    Id: '',
    DesignStructureId: '',
    Name: '',
    Description: '',
    Exist: false
  }
  listFile: any = [];
  fileName: string;
  isChecked: boolean = false;
  parentId: string;
  ngOnInit() {
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      this.getCBBSBU();
      this.model.SBUId = currentUser.sbuId;
      this.model.DepartmentId = currentUser.departmentId;
      this.getCBBDepartment()
    }
    this.searchDesignStructure();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  getCBBSBU() {
    this.comboboxService.getCbbSBU().subscribe((data: any) => {
      if (data) {
        this.listSBU = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe((data: any) => {
      if (data) {
        this.listDepartment = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  searchDesignStructure() {
    this.designStructureService.searchDesignStructure(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listDesignStructure = data.ListResult;
        this.treeView.selectedRowKeys = [this.parentId];
        for (var i = 0; i < this.listDesignStructure.length; i++) {
          if (this.listDesignStructure[i].Id == this.treeView.selectedRowKeys) {
            this.listFile = this.listDesignStructure[i].ListFile;
          }
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }


  showCreateUpdate(id: string) {
    let activeModal = this.modalService.open(CreateDesignStructureComponent, { container: 'body', windowClass: 'createdesignstructure-modal', backdrop: 'static' })
    activeModal.componentInstance.id = id;
    activeModal.componentInstance.parentId = this.parentId;
    activeModal.componentInstance.type = this.model.Type;
    activeModal.componentInstance.objectType = this.model.ObjectType;
    activeModal.result.then((result) => {
      if (result) {
        this.searchDesignStructure();
      }
    }, (reason) => {
    });
  }

  showCreateUpdateFile(id) {
    let activeModal = this.modalService.open(CreateDesignStructureFileComponent, { container: 'body', windowClass: 'createdesignstructure-modal', backdrop: 'static' })
    activeModal.componentInstance.parentId = this.parentId;
    activeModal.componentInstance.id = id;
    activeModal.result.then((result) => {
      if (result) {
        this.searchDesignStructure();
      }
    }, (reason) => {
    });
  }

  addRow() {
    if (this.modelFile.Name) {
      this.designStructureService.createDesignStructureFile(this.modelFile).subscribe(
        data => {
          this.messageService.showSuccess('Tạo file thành công!');
          this.listFile.push(this.modelFile);
          this.modelFile = {
            Id: '',
            DesignStructureId: '',
            Name: '',
            Description: '',
            Exist: false
          }
          this.modelFile.DesignStructureId = this.parentId;
          this.searchDesignStructure();

        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showMessage("Bạn chưa nhập tên file!");
    }

  }

  updateDesignStructureFile(model: any) {
    this.designStructureService.updateDesignStructureFile(model).subscribe(
      () => {
        this.messageService.showSuccess('Sửa file thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  DownloadAFile(path: string) {
    if (!path) {
      this.messageService.showError("Không có file để tải");
    }
    var link = document.createElement('a');
    link.setAttribute("type", "hidden");
    link.href = this.config.ServerFileApi + path;
    link.download = "aaaaa";
    document.body.appendChild(link);
    // link.focus();
    link.click();
    document.body.removeChild(link);

  }


  showConfirmDeleteRow(id: string, index: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá file này không?").then(
      data => {
        this.deleteDesignStructureFile(id, index);
      },
        error => {
          
        }
    );
  }

  deleteDesignStructureFile(id: string, index: string) {
    this.designStructureService.deleteDesignStructureFile({ Id: id }).subscribe(
      data => {
        this.messageService.showSuccess('Xóa file thành công!');
        this.searchDesignStructure();
      },
      error => {
        this.messageService.showError(error);
      });
  }

  onSelectionChanged(e) {
    this.parentId = e.selectedRowKeys[0];
    this.listFile = e.selectedRowsData[0].ListFile;
    this.modelFile.DesignStructureId = e.selectedRowKeys[0];
  }

  showConfirmDeleteDesignStructure(Id: string) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá thư mục này không?").then(
      data => {
        this.deleteDesignStructure(Id);
      },
      error => {
        
      }
    );
  }

  deleteDesignStructure(Id: string) {
    this.designStructureService.deleteDesignStructure({ Id: Id }).subscribe(
      data => {
        this.searchDesignStructure();
        this.messageService.showSuccess('Xóa thư mục thành công!');
      },
      error => {
        this.messageService.showError(error);
      });
  }
}
