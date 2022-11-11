import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ReasonChangeInsuranceService } from 'src/app/employee/service/reason-change-insurance.service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-reason-change-insurance-create',
  templateUrl: './reason-change-insurance-create.component.html',
  styleUrls: ['./reason-change-insurance-create.component.scss']
})
export class ReasonChangeInsuranceCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private reasonChangeInsuranceService: ReasonChangeInsuranceService,) { }

  modalInfo = {
    Title: 'Thêm mới nơi làm việc',
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
      this.modalInfo.Title = 'Chỉnh sửa lý do điều chỉnh mức đóng BHXH';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới lý do điều chỉnh mức đóng BHXH';
    }
  }


  getInfo() {
    this.reasonChangeInsuranceService.getReasonById({ Id: this.id }).subscribe(
      result => {
        this.reasonModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.reasonChangeInsuranceService.create(this.reasonModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới lý do điều chỉnh mức đóng BHXH thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới lý do điều chỉnh mức đóng BHXH thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.reasonChangeInsuranceService.update(this.reasonModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật lý do điều chỉnh mức đóng BHXH thành công!');
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
