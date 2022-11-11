import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ComboboxService, MessageService } from 'src/app/shared';

import { ProjectAttachService } from '../../service/project-attach.service';

@Component({
  selector: 'app-project-attach-tab-type',
  templateUrl: './project-attach-tab-type.component.html',
  styleUrls: ['./project-attach-tab-type.component.scss']
})
export class ProjectAttachTabTypeComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private projectAttachService: ProjectAttachService,
    private comboboxService: ComboboxService
  ) { }

  modalInfo = {
    Title: 'Thêm mới chủng loại tài liệu',
    SaveText: 'Lưu',

  };

  columnName: any[] = [{ Name: 'Name', Title: 'Tên chủng loại' }]

  isAction: boolean = false;
  id: string;
  parentId: string;
  modelCombobox = {
    Id: null
  }
  ProjectId : string;
  listTypes: any[] = [];
  listDA = [];
  model: any = {
    Id: '',
    Name: '',
    Index: 0,
    Note: '',
    EXWRate: 0,
    PublicRate:0,
    projectId : '',
  }

  listSBU:any[] = [];

  ngOnInit() {
    this.getListType();
    this.getCbbSBU();
    if (this.id) {
      this.modelCombobox.Id = this.id;
      this.modalInfo.Title = 'Chỉnh sửa nhóm tài liệu';
      this.modalInfo.SaveText = 'Lưu';
      this.getTypeInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm tài liệu";
      this.model.ParentId = this.parentId;
    }
    this.model.projectId = this.ProjectId;
    // this.getCbbType();

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

  getListType() {
    this.projectAttachService.getProjectDocType(this.ProjectId).subscribe((data: any) => {
      if (data) {
        this.listDA = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getTypeInfo() {
    this.projectAttachService.getTypeInfo({ Id: this.id }).subscribe(data => {
      this.model = data;
    });
  }

  createType(isContinue) {
    this.projectAttachService.createType(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateType() {
    this.projectAttachService.updateType(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật chủng loại  thành công');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.updateType();
    }
    else {
      this.createType(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
