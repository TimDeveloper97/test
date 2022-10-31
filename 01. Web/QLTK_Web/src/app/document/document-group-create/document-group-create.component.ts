import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, MessageService } from 'src/app/shared';
import { DocumentGroupService } from '../services/document-group.service';

@Component({
  selector: 'app-document-group-create',
  templateUrl: './document-group-create.component.html',
  styleUrls: ['./document-group-create.component.scss']
})
export class DocumentGroupCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private documentGroupService: DocumentGroupService,
    private comboboxService: ComboboxService) { }

  isAction: boolean = false;
  id: any;
  parentId: any = null;
  modalInfo = {
    Title: 'Thêm mới nhóm tài liệu',
    SaveText: 'Lưu',
  };

  documentGroupModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    ParentId: null
  }

  documentGroups: any[] = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }]

  ngOnInit(): void {
    this.getCbbDocumentGroup();
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm tài liệu';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.documentGroupModel.ParentId = this.parentId;
      this.modalInfo.Title = 'Thêm mới nhóm tài liệu';
    }
  }

  getCbbDocumentGroup() {
    this.comboboxService.getDocumentGroup().subscribe(data => {
      this.documentGroups = data;
    }, error => {
      this.messageService.showError(error);
    })
  }

  getInfo() {
    this.documentGroupService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.documentGroupModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.documentGroupService.create(this.documentGroupModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới nhóm tài liệu thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm tài liệu thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.documentGroupService.update(this.documentGroupModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm tài liệu thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  clear() {
    let groupId = this.documentGroupModel.ParentId;
    this.documentGroupModel = {
      Id: '',
      Name: '',
      Code: '',
      Note: '',
      ParentId: groupId
    };
    this.getCbbDocumentGroup();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


}
