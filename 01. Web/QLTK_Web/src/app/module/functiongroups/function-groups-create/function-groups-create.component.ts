import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { FunctionGroupsService } from '../../services/function-groups.service';

@Component({
  selector: 'app-function-groups-create',
  templateUrl: './function-groups-create.component.html',
  styleUrls: ['./function-groups-create.component.scss']
})
export class FunctionGroupsCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private serviceFunctionGroup:FunctionGroupsService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới nhóm tiêu chuẩn',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listFunctionGroup: any[] = []

  modelFunctionGroup: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
  }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa nhóm tính năng';
      this.ModalInfo.SaveText = 'Lưu';
      this.getFunctionGroupInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới nhóm tính năng";
    }
  }

  getFunctionGroupInfo() {
    this.serviceFunctionGroup.getFunctionGroup({ Id: this.Id }).subscribe(data => {
      this.modelFunctionGroup = data;
    },
    error => {
      this.messageService.showError(error);
    });
  }

  createFunctionGroup(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.modelFunctionGroup.Code);
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

  updateFunctionGroup() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.modelFunctionGroup.Code);
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
      this.updateFunctionGroup();
    }
    else {
      this.createFunctionGroup(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.modelFunctionGroup.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.serviceFunctionGroup.createFunctionGroup(this.modelFunctionGroup).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.modelFunctionGroup = {};
          this.messageService.showSuccess('Thêm mới nhóm tính năng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm tính năng thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.modelFunctionGroup.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.serviceFunctionGroup.updateFunctionGroup(this.modelFunctionGroup).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm tính năng thành công!');
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
