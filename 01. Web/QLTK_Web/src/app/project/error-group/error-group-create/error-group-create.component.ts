import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-error-group-create',
  templateUrl: './error-group-create.component.html',
  styleUrls: ['./error-group-create.component.scss']
})
export class ErrorGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: ErrorService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhóm lỗi',
    SaveText: 'Lưu',
  };

  listErrorGroup: any[] = [];
  listErrorGroupId: any[] = [];

  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];

  listType: any[] = [
    { Id: '1', Name: 'Là lỗi trực quan' },
    { Id: '2', Name: 'Là lỗi nguyên nhân' },
    { Id: '3', Name: 'Là lỗi chi phí' }
  ]
  isAction: boolean = false;
  Id: string;
  check: string;
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    Type: null,
    ParentId:null,
  }
  columnType: any[] = [{Name: 'Name', Title: 'Loại' }];

  ngOnInit() {
    this.getCbbProductGroup();
    if (this.Id) {
      if (this.check) {
        this.modalInfo.Title = 'Chỉnh sửa nhóm vấn đề';
      } else {
        this.modalInfo.Title = 'Chỉnh sửa nhóm lỗi';
      }
      this.modalInfo.SaveText = 'Lưu';
      this.getErrorGroupInfo();
    }
    else {
      if (this.check) {
        this.modalInfo.Title = "Thêm mới nhóm vấn đề";
      } else {
        this.modalInfo.Title = "Thêm mới nhóm lỗi";
      }
    }
  }

  getCbbProductGroup() {
    this.combobox.getListErrorGroup().subscribe((data: any) => {
      this.listErrorGroup = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getErrorGroupInfo() {
    this.service.getErrorGroupInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  createErrorGroup(isContinue) {
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

  updateErrorGroup() {
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
      this.updateErrorGroup();
    }
    else {
      this.createErrorGroup(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.service.createErrorGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          if (this.check) {
            this.messageService.showSuccess('Thêm mới nhóm vấn đề thành công!');
          } else {
            this.messageService.showSuccess('Thêm mới nhóm lỗi thành công!');
          }
        }
        else {
          if (this.check) {
            this.messageService.showSuccess('Thêm mới nhóm vấn đề thành công!');
          } else {
            this.messageService.showSuccess('Thêm mới nhóm lỗi thành công!');
          }
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.service.updateErrorGroup(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        if (this.check) {
          this.messageService.showSuccess('Cập nhật nhóm vấn đề thành công!');
        } else {
          this.messageService.showSuccess('Cập nhật nhóm lỗi thành công!');
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.ParentId = e.selectedRowKeys[0];
    this.closeDropDownBox();
  }
  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

}
