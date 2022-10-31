import { nullSafeIsEquivalent } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DateUtils, MessageService } from 'src/app/shared';
import { DocumentService } from '../services/document.service';

@Component({
  selector: 'app-document-promulgate-create',
  templateUrl: './document-promulgate-create.component.html',
  styleUrls: ['./document-promulgate-create.component.scss']
})
export class DocumentPromulgateCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private documentService: DocumentService,
    public dateUtils: DateUtils,) { }

  modalInfo = {
    Title: 'Thêm mới ban hành',
    SaveText: 'Ban hành',
  };

  isAction: boolean = false;
  id: string;
  documentId: string;

  promulgateModel: any = {
    Id: '',
    DocumentId: '',
    Reason: '',
    Content: '',
    PromulgateDate: null,
    ReviewDateFrom: null,
    ReviewDateTo:null,
    ApproveDate:null
  };

  promulgateDate: any = null;

  reviewDateFrom: any = null;
  reviewDateTo:any=null;
  approveDate:any=null;

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa ban hành';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới ban hành';
    }
    this.promulgateModel.DocumentId = this.documentId;
  }

  getInfo() {
    this.documentService.getPromulgateInfo({ Id: this.id }).subscribe(
      result => {
        this.promulgateModel = result;
        this.promulgateDate = this.dateUtils.convertDateToObject(this.promulgateModel.PromulgateDate);
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.documentService.createPromulgate(this.promulgateModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới ban hành thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới ban hành thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.documentService.updatePromulgate(this.promulgateModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật ban hành thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.promulgateDate != null) {
      this.promulgateModel.PromulgateDate = this.dateUtils.convertObjectToDate(this.promulgateDate)
    }
    if (this.reviewDateFrom != null) {
      this.promulgateModel.ReviewDateFrom = this.dateUtils.convertObjectToDate(this.reviewDateFrom)
    }
    if (this.reviewDateTo != null) {
      this.promulgateModel.ReviewDateTo = this.dateUtils.convertObjectToDate(this.reviewDateTo)
    }
    if (this.approveDate != null) {
      this.promulgateModel.ApproveDate = this.dateUtils.convertObjectToDate(this.approveDate)
    }
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
    this.promulgateModel = {
      Id: '',
      Reason: '',
      Content: '',
      PromulgateDate: null
    };
    this.promulgateDate = null;
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
