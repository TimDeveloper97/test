import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DocumentTypeService } from '../../services/document-type.service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-document-type-create',
  templateUrl: './document-type-create.component.html',
  styleUrls: ['./document-type-create.component.scss']
})
export class DocumentTypeCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private documentTypeService: DocumentTypeService,) { }

  modalInfo = {
    Title: 'Thêm mới loại tài liệu',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  documentTypeModel: any = {
    Id: '',
    Name: '',
    Code:'',
    Description: ''
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa loại tài liệu';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới loại tài liệu';
    }
  }

  getInfo() {
    this.documentTypeService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.documentTypeModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.documentTypeService.create(this.documentTypeModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới loại tài liệu thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới loại tài liệu thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.documentTypeService.update(this.documentTypeModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật loại tài liệu thành công!');
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
    this.documentTypeModel = {
      Id: '',
      Name: '',
      Code:'',
      Note: ''
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
