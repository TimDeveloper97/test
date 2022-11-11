import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-supplier-create',
  templateUrl: './supplier-create.component.html',
  styleUrls: ['./supplier-create.component.scss']
})
export class SupplierCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private combobox: ComboboxService,
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhà cung cấp',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    CodeProduct: '',
    NameProduct: '',
    Email: '',
    PhoneNumber: '',
    customerRequirementId: '',
  }
  ListSupplier: any = [];
  CustomerRequirementId: string;

  ngOnInit() {
    this.model.customerRequirementId = this.CustomerRequirementId;
    this.modalInfo.Title = 'Thêm mới nhà cung cấp';
    this.modalInfo.SaveText = 'Lưu';

  }


  create(isContinue) {
    this.ListSupplier.push(this.model);
    if (isContinue) {
      this.messageService.showSuccess('Thêm mới nhà cung cấp thành công!');
      this.model = {
        Id: '',
        Name: '',
        Code: '',
        CodeProduct: '',
        NameProduct: '',
        Email: '',
        PhoneNumber: '',
        customerRequirementId: this.CustomerRequirementId,
      }
    }
    else {
      this.messageService.showSuccess('Thêm mới nhà cung cấp thành công!');
      this.closeModal(true);
    }
  }


  save(isContinue: boolean) {
    this.create(isContinue);
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
