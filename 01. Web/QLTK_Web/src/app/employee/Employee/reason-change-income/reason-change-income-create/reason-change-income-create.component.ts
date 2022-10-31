import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ReasonChangeIncomeService } from 'src/app/employee/service/reason-change-income.service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-reason-change-income-create',
  templateUrl: './reason-change-income-create.component.html',
  styleUrls: ['./reason-change-income-create.component.scss']
})
export class ReasonChangeIncomeCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private reasonChangeIncomeService: ReasonChangeIncomeService,) { }

  modalInfo = {
    Title: 'Thêm mới lý do điều chỉnh thu nhập',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  reasonModel: any = {
    Id: '',
    Name: '',
    Note: ''
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa lý do điều chỉnh thu nhập';
      this.modalInfo.SaveText = 'Lưu';
      this.getReasonInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới lý do điều chỉnh thu nhập';
    }
  }

  getReasonInfo() {
    this.reasonChangeIncomeService.getReasonById({ Id: this.id }).subscribe(
      result => {
        this.reasonModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  createReason(isContinue) {
    this.reasonChangeIncomeService.create(this.reasonModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới lý do điều chỉnh thu nhập thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới lý do điều chỉnh thu nhập thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateReason() {
    this.reasonChangeIncomeService.update(this.reasonModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật lý do điều chỉnh thu nhập thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.updateReason();
    }
    else {
      this.createReason(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.reasonModel = {
      Id: '',
      Name: '',
      Note: ''
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
