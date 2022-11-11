import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, MessageService } from 'src/app/shared';

@Component({
  selector: 'app-material-create',
  templateUrl: './material-create.component.html',
  styleUrls: ['./material-create.component.scss']
})
export class MaterialCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private combobox: ComboboxService,
  ) { }

  modalInfo = {
    Title: 'Thêm mới sản phẩm',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listMaterialGroupName: any[] = [];
  listManufactureCode: any[] = [];

  model: any = {
    Id: '',
    Name: '',
    Code: '',
    MaterialGroupName: '',
    ManufactureId: '',
    Quantity: 1,
    Pricing: 0,
    customerRequirementId: '',
  }
  ListMaterial: any = [];
  CustomerRequirementId: string;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã' }, { Name: 'Name', Title: 'Tên' },];

  ngOnInit() {
    this.model.customerRequirementId = this.CustomerRequirementId;
    this.getCbbMaterialGroup();
    this.getCbbManufacture();
    this.modalInfo.Title = 'Thêm mới sản phẩm';
    this.modalInfo.SaveText = 'Lưu';

  }


  create(isContinue) {
    this.ListMaterial.push(this.model);
    if (isContinue) {
      this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
      this.model = {
        Id: '',
        Name: '',
        Code: '',
        MaterialGroupName: '',
        ManufactureId: '',
        Quantity: 1,
        Pricing: 0,
        customerRequirementId: this.CustomerRequirementId,
      }
    }
    else {
      this.messageService.showSuccess('Thêm mới sản phẩm thành công!');
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

  getCbbMaterialGroup() {
    this.combobox.getCbbMaterialGroup().subscribe(
      data => {
        this.listMaterialGroupName = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbManufacture() {
    this.combobox.getCbbManufacture().subscribe(
      data => {
        this.listManufactureCode = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }


}
