import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, AppSetting, Constants, ComboboxService } from 'src/app/shared';

import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ProductCreatesService } from '../services/product-create.service';
import { ProductGroupService } from '../services/product-group.service';

@Component({
  selector: 'app-product-need-publications',
  templateUrl: './product-need-publications.component.html',
  styleUrls: ['./product-need-publications.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductNeedPublicationsComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private appSetting: AppSetting,
    private modalService: NgbModal,
    public constant: Constants,
    private combobox: ComboboxService,
    private productCreatesService: ProductCreatesService,
  ) { }

  ModalInfo = {
    Title: 'Thêm mới nhóm thiết bị',
    SaveText: 'Lưu',

  };
  isAction: boolean = false;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    Name: '',
    Code: '',
    Publications: 0,
  }
  IdProduct: string;
  listData: any = [];
  publications: Number;
  Id: string;
  ngOnInit() {
    this.model.Id = this.IdProduct;
    this.model.Publications = this.publications;
    this.getProductNeedPublications();
      this.ModalInfo.Title = 'Danh sách dự án cần có tài liệu';
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }


  getProductNeedPublications() {
    this.productCreatesService.getProductNeedPublications(this.model).subscribe(
      data => {
        this.listData = data;
        
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }




}
