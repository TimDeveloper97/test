import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DesignStructureService } from '../services/designstructure-service';
import { MessageService, PermissionService } from 'src/app/shared';

@Component({
  selector: 'app-create-design-structure',
  templateUrl: './create-design-structure.component.html',
  styleUrls: ['./create-design-structure.component.scss']
})
export class CreateDesignStructureComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private designStructureService: DesignStructureService,
    private messageService: MessageService,
    public permissionService: PermissionService
  ) { }

  modalInfo = {
    Title: 'Tạo thư mục',
  };
  isAction: boolean = false;
  id: string;
  parentId: string;
  type: any;
  objectType: any;
  model: any = {
    Name: '',
    Type: '',
    ParentId: '',
    Description: '',
    Contain: '',
    Extension: '',
    IsOpen: false
  };

  columnName: any[] = [{ Name: 'Name', Title: 'Tên thu mục' }];
  folders: any = [];
  ngOnInit() {
    if (this.id) {
      this.modalInfo.Title = "Sửa thư mục";
      this.getInfoStructure();
    } else {
      this.model.ParentId = this.parentId;
      this.model.Type = this.type;
      this.model.ObjectType = this.objectType;
      this.getFolders('');
    }
  }

  getFolders(departmentId) {
    this.designStructureService.getFolderParent({ Type: this.type, ObjectType: this.objectType, DesignStructureId: this.id ,DepartmentId:departmentId}).subscribe(
      data => {
        this.folders = data;
      },
      error => {
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.updateDesignStructure();
    }
    else {
      this.createDesignStructure(isContinue);
    }
  }

  createDesignStructure(isContinue) {
    this.designStructureService.createDesignStructure(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Tạo thư mục thành công!');
        }
        else {
          this.messageService.showSuccess('Tạo thư mục thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getInfoStructure() {
    this.designStructureService.getInfoDesignStructure({ Id: this.id }).subscribe(data => {
      this.model = data;     
      this.getFolders(this.model.DepartmentId);
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateDesignStructure() {
    this.designStructureService.updateDesignStructure(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Sửa thư mục thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
