import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';
import { ProductCreatesService } from '../services/product-create.service';

@Component({
  selector: 'app-show-product-module-update',
  templateUrl: './show-product-module-update.component.html',
  styleUrls: ['./show-product-module-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowProductModuleUpdateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constant: Constants,
    private activeModal: NgbActiveModal,
    private service: ProductCreatesService
  ) { }

  isAction: boolean = false;
  productId: string;
  listProductModuleUpdate = [];
  isSelectAll: false;

  modalInfo = {
    Title: 'Danh sách module có thay đổi',
    SaveText: 'Lưu',
  };

  ngOnInit() {
  }

  updateProductModuleUpdate() {
    this.service.deleteProductModuleUpdate(this.productId).subscribe(
      data => {
        this.messageService.showSuccess('Xác nhận thành công!');
        this.closeModal(false);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(true);
  }

}
