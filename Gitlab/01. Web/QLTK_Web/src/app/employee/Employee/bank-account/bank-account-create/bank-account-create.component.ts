import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BankAccountService } from 'src/app/employee/service/bank-account.service';
import { ComboboxService, Constants, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-bank-account-create',
  templateUrl: './bank-account-create.component.html',
  styleUrls: ['./bank-account-create.component.scss']
})
export class BankAccountCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private bankAccountService: BankAccountService,
    private combobox: ComboboxService,
    private constant: Constants
  ) { }

  modalInfo = {
    Title: 'Thêm mới ngân hàng',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  bankAccountModel: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: ''
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa ngân hàng';
      this.modalInfo.SaveText = 'Lưu';
      this.getBankAccountInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới ngân hàng';
    }
  }

  getBankAccountInfo() {
    this.bankAccountService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.bankAccountModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.bankAccountService.update(this.bankAccountModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật ngân hàng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.bankAccountService.create(this.bankAccountModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới ngân hàng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới ngân hàng thành công!');
          this.closeModal(true);
        }
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
    this.bankAccountModel = {
      Id: '',
      Name: '',
      Code: '',
      Note: ''
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
