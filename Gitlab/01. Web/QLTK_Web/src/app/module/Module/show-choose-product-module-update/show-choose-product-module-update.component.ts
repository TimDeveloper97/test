import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-show-choose-product-module-update',
  templateUrl: './show-choose-product-module-update.component.html',
  styleUrls: ['./show-choose-product-module-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowChooseProductModuleUpdateComponent implements OnInit {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private titleservice: Title,
    public constant: Constants,
    private activeModal: NgbActiveModal,
  ) { }

  isAction: boolean = false;
  listData = [];
  isSelectAll: false;
  listChecker = [];

  modalInfo = {
    Title: 'Danh sách thiết bị sử dụng module',
    SaveText: 'Lưu',
  };

  ngOnInit() {
  }

  changeCheckAll() {
    this.listData.forEach(element => {
      element.IsChecked = this.isSelectAll;
    });
  }

  closeModal(isOK: boolean) {
    if (isOK) {
      this.activeModal.close(this.listData);
    } else {
      this.activeModal.close(false);
    }
  }
}
