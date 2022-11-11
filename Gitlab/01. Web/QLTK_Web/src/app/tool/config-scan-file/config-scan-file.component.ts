import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ConfigScanFileService } from '../services/config-scan-file.service';

@Component({
  selector: 'app-config-scan-file',
  templateUrl: './config-scan-file.component.html',
  styleUrls: ['./config-scan-file.component.scss']
})
export class ConfigScanFileComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ConfigScanFileService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  modalInfo = {
    Title: 'Thêm mới Template',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listType: any[] = [
    { Id: 1, Name: 'Biểu mẫu cơ khí' },
    { Id: 2, Name: 'Biểu mẫu điện' },
    { Id: 3, Name: 'Biểu mẫu điện tử' }
  ]

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    Type: 1,
    PathFolderC: ''
  }

  ngOnInit() {
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa Template';
      this.modalInfo.SaveText = 'Lưu';
      this.getConfigScanFileInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới Template";
    }
  }

  getConfigScanFileInfo() {
    this.service.getConfigScanFileInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  createConfigScanFile(isContinue) {
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

  updateConfigScanFile() {
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
      this.updateConfigScanFile();
    }
    else {
      this.createConfigScanFile(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.service.createConfigScanFile(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới Template thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới Template thành công!');
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
    this.service.updateConfigScanFile(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật Template thành công!');
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
