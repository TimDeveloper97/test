import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { LaborContractService } from 'src/app/employee/service/labor-contract.service';
import { Constants, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-labor-contract-create',
  templateUrl: './labor-contract-create.component.html',
  styleUrls: ['./labor-contract-create.component.scss']
})
export class LaborContractCreateComponent implements OnInit {

  constructor(
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private laborContractService: LaborContractService,) { }

  modalInfo = {
    Title: 'Thêm mới loại hợp đồng lao động',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  laborContractModel: any = {
    Id: '',
    Name: '',
    Type: null,
    Note: ''
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa loại hợp đồng lao động';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới loại hợp đồng lao động';
    }
  }

  getInfo() {
    this.laborContractService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.laborContractModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.laborContractService.create(this.laborContractModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới loại hợp đồng lao động thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới loại hợp đồng lao động thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.laborContractService.update(this.laborContractModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật loại hợp đồng lao động thành công!');
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
    this.laborContractModel = {
      Id: '',
      Name: '',
      Type: null,
      Note: ''
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
