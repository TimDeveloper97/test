import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { FunctionService } from '../../services/module-funcion.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { add } from '@tweenjs/tween.js';

@Component({
  selector: 'app-function-create',
  templateUrl: './function-create.component.html',
  styleUrls: ['./function-create.component.scss']
})

export class FunctionCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private functionService: FunctionService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới tính năng',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id!: string;
  listFunction: any[] = []
  listGroup = [];
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
    FunctionGroupId: '',
    TechnicalRequire: ''
  }
  FunctionGroupId = '';
  createBy = '';
  ngOnInit() {
    let userStore = localStorage.getItem('qltkcurrentUser');
    if (userStore) {
      this.createBy = JSON.parse(userStore).userid;
    }

    this.getGroup();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa tính năng';
      this.ModalInfo.SaveText = 'Lưu';
      this.getFunctionInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới tính năng";
      if (this.FunctionGroupId != null) {
        this.model.FunctionGroupId = this.FunctionGroupId;
      }
    }
  }

  getFunctionInfo() {
    this.functionService.getFunction({ Id: this.Id }).subscribe(data => {
      this.model = data;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getGroup() {
    this.functionService.getListGroup().subscribe(
      data => {
        this.listGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  addFunction(isContinue: boolean) {
    this.functionService.createFunction(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới tính năng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới tính năng thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createFunction(isContinue: boolean) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.CreateBy = this.createBy;
      this.addFunction(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.CreateBy = this.createBy;
          this.addFunction(isContinue);
        },
        error => {
          
        }
      );
    }

  }

  saveFunction() {
    this.functionService.updateFunction(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật tính năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateFunction() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.model.UpdateBy = this.createBy;
      this.saveFunction();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = this.createBy;
          this.saveFunction();
        },
        error => {
          
        }
      );
    }
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateFunction();
    }
    else {
      this.createFunction(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
