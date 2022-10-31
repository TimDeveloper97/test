import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-bank-create',
  templateUrl: './bank-create.component.html',
  styleUrls: ['./bank-create.component.scss']
})
export class BankCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới tài khoản ngân hàng',
    SaveText: 'Lưu',
  };
  listCreate: any[] = [];
  listExpert: any[] = [];
  isAction: boolean = false;
  Id: string;
  row: any = {};
  isAdd = false;
  model: any = {
    Id: '',
    ExpertId: '',
    Name: '',
    AccountName: '',
    Account: ''
  }

  listData: any[] = []
  listTemp: any[] = [];

  ngOnInit() {
    if (this.isAdd == false) {
      this.model = this.row;
      this.ModalInfo.Title = 'Chỉnh sửa tài khoản ngân hàng';
      this.ModalInfo.SaveText = 'Lưu';
    }
    else {
      this.Id = '';
      this.ModalInfo.Title = 'Thêm mới tài khoản ngân hàng';
    }
  }

  isCheck = true;
  save(isContinue: boolean) {
    if (this.isAdd == true) {
      this.listCreate.push(this.model)
      if (isContinue == false) {
        this.activeModal.close({ isAdd: true, modelTemp: this.listCreate });
        
      } else {
        this.model = {};
        
      }

    } else {

      this.activeModal.close({ isAdd: false, modelTemp: this.model });

      this.messageService.showSuccess('Cập nhật tài khoản ngân hàng thành công!');
    }
  }


  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close({ isAdd: true, modelTemp: this.listCreate });
  }
}
