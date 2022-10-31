import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { ProductStandardGroupService } from '../../services/product-standard-group.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-product-standard-group-create',
  templateUrl: './product-standard-group-create.component.html',
  styleUrls: ['./product-standard-group-create.component.scss']
})
export class ProductStandardGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private ProductStandardGroupService: ProductStandardGroupService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới nhóm tiêu chuẩn',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listProductStandardGroup: any[] = []

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Note: '',
  }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa nhóm tiêu chuẩn sản phẩm';
      this.ModalInfo.SaveText = 'Lưu';
      this.getProductStandardGroupInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới nhóm tiêu chuẩn sản phẩm";
    }
  }

  getProductStandardGroupInfo() {
    this.ProductStandardGroupService.getProductStandardGroup({ Id: this.Id }).subscribe(data => {
      this.model = data;
    },
    error => {
      this.messageService.showError(error);
    });
  }

  createProductStandardGroup(isContinue) {
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

  updateProductStandardGroup() {
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
      this.updateProductStandardGroup();
    }
    else {
      this.createProductStandardGroup(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.ProductStandardGroupService.createProductStandardGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm tiêu chuẩn sản phẩm thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm tiêu chuẩn sản phẩm thành công!');
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
    this.ProductStandardGroupService.updateProductStandardGroup(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm tiêu chuẩn sản phẩm thành công!');
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
