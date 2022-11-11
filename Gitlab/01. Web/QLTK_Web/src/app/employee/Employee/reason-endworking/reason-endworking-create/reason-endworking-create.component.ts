import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { clear } from 'console';
import { ReasonEndworkingService } from 'src/app/employee/service/reason-endworking.service';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-reason-endworking-create',
  templateUrl: './reason-endworking-create.component.html',
  styleUrls: ['./reason-endworking-create.component.scss']
})
export class ReasonEndworkingCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private reasonService: ReasonEndworkingService,
    private combobox: ComboboxService,
    private constant: Constants
  ) { }

  modalInfo = {
    Title: 'Thêm mới lý do nghỉ việc',
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
      this.modalInfo.Title = 'Chỉnh sửa lý do nghỉ việc';
      this.modalInfo.SaveText = 'Lưu';
      this.getReasonInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới lý do nghỉ việc';
    }
  }

  getReasonInfo() {
    this.reasonService.getReasonById({ Id: this.id }).subscribe(
      result => {
        this.reasonModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  createReason(isContinue) {
    this.reasonService.create(this.reasonModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới lý do nghỉ việc thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới lý do nghỉ việc thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateReason() {
    this.reasonService.update(this.reasonModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật lý do nghỉ việc thành công!');
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
