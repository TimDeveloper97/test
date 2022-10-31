import { Component, OnInit } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { CustomerTypeService } from '../../service/customer-type.service';

@Component({
  selector: 'app-customer-type-create',
  templateUrl: './customer-type-create.component.html',
  styleUrls: ['./customer-type-create.component.scss']
})
export class CustomerTypeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private CustomerTypeService: CustomerTypeService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private modalService: NgbModal,
    private comboboxService: ComboboxService,

  ) { }

  modalInfo = {
    Title: 'Thêm mới loại khách hàng',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  parentId: string;
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    ParentId: ''
  }

  selectCustomerTypeId: '';
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }]

  ngOnInit() {
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa loại khách hàng';
      this.modalInfo.SaveText = 'Lưu';
      this.getCustomerTypeInfo();
      //this.getCbbCustomerTypeExceptId();
    }
    else {
      this.model.ParentId = this.parentId;
      this.modalInfo.Title = "Thêm mới loại khách hàng";
      //this.getCbbCustomerType();
    }
    this.getCbbCustomerType();
  }

  getCustomerTypeInfo() {
    this.CustomerTypeService.getCustomerTypeInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  createCustomerType(isContinue) {
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

  updateCustomerType() {
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
      this.updateCustomerType();
    }
    else {
      this.createCustomerType(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.CustomerTypeService.createCustomerType(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới loại khách hàng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới loại khách hàng thành công!');

          this.closeModal(data);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.CustomerTypeService.updateCustomerType(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật loại khách hàng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  ListCustomerType: any[] = [];
  getCbbCustomerType() {
    this.comboboxService.getListCustomerType().subscribe((data: any) => {
      if (this.Id == null || this.Id == "") {
        this.ListCustomerType = data;
      } else {
        data.forEach(element => {
          if (element.Id == this.Id) {
          }
          else {
            this.ListCustomerType.push(element);
          }
        });
      }

    },
      error => {
        this.messageService.showError(error);
      });
  }

}
