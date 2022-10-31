import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Constants, DateUtils, MessageService } from 'src/app/shared';
import { DocumentService } from '../services/document.service';

@Component({
  selector: 'app-document-review-create',
  templateUrl: './document-review-create.component.html',
  styleUrls: ['./document-review-create.component.scss']
})
export class DocumentReviewCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public dateUtils: DateUtils,
    private documentService: DocumentService,
    public constant: Constants,) { }

  isAction: boolean = false;
  documentId: any;
  id: any;
  modalInfo = {
    Title: 'Thêm mới review',
    SaveText: 'Lưu',
  };

  minDateNotificationV: NgbDateStruct;

  reviewModel: any = {
    Id: '',
    DocumentId: '',
    Problem: '',
    Status: 1,
    ProblemDate: null,
    FinishExpectedDate: null,
    FinishDate: null
  };

  problemDate: any = null;
  finishExpectedDate: any = null;
  finishDate: any = null;

  ngOnInit(): void {
    this.reviewModel.DocumentId = this.documentId;
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa vấn đề';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới vấn đề';
    }
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  getInfo() {
    this.documentService.getProblemInfo({ Id: this.id }).subscribe(
      result => {
        this.reviewModel = result;
        this.problemDate = this.dateUtils.convertDateToObject(this.reviewModel.ProblemDate);
        if (this.reviewModel.FinishExpectedDate != null && this.reviewModel.FinishExpectedDate != "") {
          this.finishExpectedDate = this.dateUtils.convertDateToObject(this.reviewModel.FinishExpectedDate);
        }
        if (this.reviewModel.FinishDate != null && this.reviewModel.FinishDate != "") {
          this.finishDate = this.dateUtils.convertDateToObject(this.reviewModel.FinishDate);
        }
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.documentService.createProblem(this.reviewModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới review thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới review thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.documentService.updateProblem(this.reviewModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật review thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    let isOk = true;
    if (this.problemDate != null) {
      this.reviewModel.ProblemDate = this.dateUtils.convertObjectToDate(this.problemDate)
    }

    let currentDate = new Date();
    let problemDateValid = new Date(this.reviewModel.ProblemDate);
    let finishExpectedDateValid = null;
    let finishDateValid = null;

    if (currentDate < problemDateValid) {
      this.messageService.showMessage("Ngày phát hiện vấn đề lớn hơn ngày hiện tại");
      isOk = false;
    }

    if (this.finishExpectedDate != null) {
      this.reviewModel.FinishExpectedDate = this.dateUtils.convertObjectToDate(this.finishExpectedDate);
      finishExpectedDateValid = new Date(this.reviewModel.FinishExpectedDate);
      if (finishExpectedDateValid < problemDateValid) {
        this.messageService.showMessage("Ngày hoàn thành dự kiến nhỏ hơn ngày phát hiện vấn đề");
        isOk = false;
      }
    }

    if (this.finishDate != null) {
      this.reviewModel.FinishDate = this.dateUtils.convertObjectToDate(this.finishDate);
      finishDateValid = new Date(this.reviewModel.FinishDate);
      if (finishDateValid < problemDateValid) {
        this.messageService.showMessage("Ngày hoàn thành nhỏ hơn ngày phát hiện vấn đề");
        isOk = false;
      }
    }

    if (finishExpectedDateValid != null && finishDateValid != null && finishExpectedDateValid > finishDateValid) {
      this.messageService.showMessage("Ngày hoàn thành nhỏ hơn ngày hoàn thành dự kiến");
      isOk = false;
    }

    if (isOk) {
      if (this.id) {
        this.update();
      }
      else {
        this.create(isContinue);
      }
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.reviewModel = {
      Id: '',
      DocumentId: '',
      Problem: '',
      Status: 1,
      ProblemDate: null,
      FinishExpectedDate: null,
      FinishDate: null
    };
    this.problemDate = null;
    this.finishExpectedDate = null;
    this.finishDate = null;
  }


}
