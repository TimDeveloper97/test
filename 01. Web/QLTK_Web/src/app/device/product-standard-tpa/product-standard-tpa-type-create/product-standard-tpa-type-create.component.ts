import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { ProductStandardTpaTypeService } from '../../services/product-standard-tpa-type.service';

@Component({
  selector: 'app-product-standard-tpa-type-create.',
  templateUrl: './product-standard-tpa-type-create.component.html',
  styleUrls: ['./product-standard-tpa-type-create..component.scss']
})
export class ProductStandardTPATypeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private productStandardTpaTypeService: ProductStandardTpaTypeService
  ) { }

  modalInfo = {
    Title: 'Thêm mới chủng loại hàng hóa',
    SaveText: 'Lưu',

  };

  columnName: any[] = [{ Name: 'Name', Title: 'Tên chủng loại' }]

  isAction: boolean = false;
  id: string;
  parentId: string;
  modelCombobox = {
    Id: null
  }
  listTypes: any[] = [];
  listDA = [];
  model: any = {
    Id: '',
    Name: '',
    Index: 0,
    Note: '',
  }


  ngOnInit() {
    this.getListType();
    if (this.id) {
      this.modelCombobox.Id = this.id;
      this.modalInfo.Title = 'Chỉnh sửa chủng loại hàng hóa';
      this.modalInfo.SaveText = 'Lưu';
      this.getTypeInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới chủng loại hàng hóa";
      this.model.ParentId = this.parentId;
    }
    // this.getCbbType();

  }

  getListType() {
    this.productStandardTpaTypeService.getListType().subscribe((data: any) => {
      if (data) {
        this.listDA = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getTypeInfo() {
    this.productStandardTpaTypeService.getTypeInfo({ Id: this.id }).subscribe(data => {
      this.model = data;
    });
  }

  createType(isContinue) {
    this.productStandardTpaTypeService.createType(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới chủng loại thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới chủng loại thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateType() {
    this.productStandardTpaTypeService.updateType(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật chủng loại  thành công');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbType() {
    if (this.model.ParentId != null && this.model.ParentId != '') {
      this.modelCombobox.Id = this.model.ParentId;
    }
    else {
      this.modelCombobox.Id = null;
    }

    this.productStandardTpaTypeService.getCbbType(this.modelCombobox).subscribe((data: any) => {
      this.listTypes = data;
      if (this.id == null || this.id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listTypes.splice(this.listTypes.length - 1, 1);
      }
    });
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.updateType();
    }
    else {
      this.createType(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
