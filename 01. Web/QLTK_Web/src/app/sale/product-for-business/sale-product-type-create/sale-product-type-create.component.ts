import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ComboboxService, MessageService } from 'src/app/shared';
import { SaleProductTypeService } from '../../service/sale-product-type.service';

@Component({
  selector: 'app-sale-product-type-create',
  templateUrl: './sale-product-type-create.component.html',
  styleUrls: ['./sale-product-type-create.component.scss']
})
export class SaleProductTypeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private saleProductTypeService: SaleProductTypeService,
    private comboboxService: ComboboxService
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
    EXWRate: 0,
    PublicRate:0
  }

  listSBU:any[] = [];

  ngOnInit() {
    this.getListType();
    this.getCbbSBU();
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

  getCbbSBU() {
    this.comboboxService.getCBBSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListType() {
    this.saleProductTypeService.getCbbType().subscribe((data: any) => {
      if (data) {
        this.listDA = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getTypeInfo() {
    this.saleProductTypeService.getTypeInfo({ Id: this.id }).subscribe(data => {
      this.model = data;
    });
  }

  createType(isContinue) {
    this.saleProductTypeService.createType(this.model).subscribe(
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
    this.saleProductTypeService.updateType(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật chủng loại  thành công');
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
