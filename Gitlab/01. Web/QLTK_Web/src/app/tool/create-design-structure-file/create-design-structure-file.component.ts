import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileProcess, MessageService, PermissionService } from 'src/app/shared';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { DesignStructureService } from '../services/designstructure-service';

@Component({
  selector: 'app-create-design-structure-file',
  templateUrl: './create-design-structure-file.component.html',
  styleUrls: ['./create-design-structure-file.component.scss']
})
export class CreateDesignStructureFileComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public fileProcessDataSheet: FileProcess,
    private uploadService: UploadfileService,
    private messageService: MessageService,
    private designStructureService: DesignStructureService,
    public permissionService: PermissionService
  ) { }
  modalInfo = {
    Title: 'Tạo file',
  };

  model: any = {
    Name: '',
    Path: '',
    Exist: false,
    IsTemplate: false,
    IsInsertData: false,
    DesignStructureId: '',
  }
  isAction: boolean = false;
  ngOnInit() {
    if (this.id) {
      this.modalInfo.Title = "Sửa file";
      this.getInfoStructureFile();
    } else {
      this.model.DesignStructureId = this.parentId;
    }

  }
  file: any = {
    name: '',
  };
  listFile: any = [];
  id: string;
  parentId: string;
  uploadFileClick($event) {
    this.listFile = [];
    var fileDataSheet = this.fileProcessDataSheet.getFileOnFileChange($event);
    for (var item of fileDataSheet) {
      item.IsFileUpload = true;
      this.listFile.push(item);
    }
    this.file = this.listFile[0];
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.updateDesignStructureFile();
    }
    else {
      this.createDesignStructureFile(isContinue);
    }
  }

  getInfoStructureFile() {
    this.designStructureService.getInfoDesignStructureFile({ Id: this.id }).subscribe(data => {
      this.model = data;
    },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateDesignStructureFile() {
    this.uploadService.uploadListFile(this.listFile, 'DesignStructure/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          this.model.Path = item.FileUrl;
        });
      }
      this.designStructureService.updateDesignStructureFile(this.model).subscribe(
        () => {
          this.messageService.showSuccess('Sửa file thành công!');
          this.closeModal(true);
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }

  createDesignStructureFile(isContinue) {
    this.uploadService.uploadListFile(this.listFile, 'DesignStructure/').subscribe((event: any) => {
      if (event.length > 0) {
        event.forEach((item, index) => {
          this.model.Path = item.FileUrl;
        });
      }
      this.designStructureService.createDesignStructureFile(this.model).subscribe(
        data => {
          this.messageService.showSuccess('Tạo file thành công!');
          this.listFile = [];
          this.model = {
            Name: '',
            Path: '',
            Exist: false,
            IsTemplate: false,
            IsInsertData: false,
            DesignStructureId: '',
          }
          this.closeModal(true);
        },
        error => {
          this.messageService.showError(error);
        }
      );
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
