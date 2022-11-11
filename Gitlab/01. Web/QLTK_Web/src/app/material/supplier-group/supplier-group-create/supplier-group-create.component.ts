import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { SupplierGroupService } from '../../services/supplier-group.service';

@Component({
  selector: 'app-supplier-group-create',
  templateUrl: './supplier-group-create.component.html',
  styleUrls: ['./supplier-group-create.component.scss']
})
export class SupplierGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: SupplierGroupService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhóm nhà cung cấp',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string = '';

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
  }
  createBy = null;

  ngOnInit() {

    let userStore = localStorage.getItem('qltkcurrentUser');

    if (userStore) {
      this.createBy = JSON.parse(userStore).userid;
    }

    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm nhà cung cấp';
      this.modalInfo.SaveText = 'Lưu';
      this.getSupplierGroupInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm nhà cung cấp";
    }
  }

  getSupplierGroupInfo() {
    this.service.getSupplierGroupInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  createSupplierGroup(isContinue: boolean) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supCreate(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supCreate(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  updateSupplierGroup() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supUpdate();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supUpdate();
        },
        error => {
          
        }
      );
    }
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateSupplierGroup();
    }
    else {
      this.createSupplierGroup(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue: boolean) {
    this.model.CreateBy =  this.createBy;
    this.service.createSupplierGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm nhà cung cấp thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm nhà cung cấp thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.model.UpdateBy =  this.createBy;
    this.service.updateSupplierGroup(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm nhà cung cấp thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
