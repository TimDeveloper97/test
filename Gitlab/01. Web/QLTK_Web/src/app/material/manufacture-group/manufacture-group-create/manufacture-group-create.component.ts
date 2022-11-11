import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ManufactureGroupService } from '../../services/manufacture-group.service';

@Component({
  selector: 'app-manufacture-group-create',
  templateUrl: './manufacture-group-create.component.html',
  styleUrls: ['./manufacture-group-create.component.scss']
})
export class ManufactureGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ManufactureGroupService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhóm hãng sản xuất',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
  }

  ngOnInit() {
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa nhóm hãng sản xuất';
      this.modalInfo.SaveText = 'Lưu';
      this.getManufactureGroupInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm hãng sản xuất";
    }
  }

  getManufactureGroupInfo() {
    this.service.getManufactureGroupInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  createManufactureGroup(isContinue) {
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

  updateManufactureGroup() {
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
      this.updateManufactureGroup();
    }
    else {
      this.createManufactureGroup(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.service.createManufactureGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm hãng sản xuất thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm hãng sản xuất thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.service.updateManufactureGroup(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm hãng sản xuất thành công!');
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
