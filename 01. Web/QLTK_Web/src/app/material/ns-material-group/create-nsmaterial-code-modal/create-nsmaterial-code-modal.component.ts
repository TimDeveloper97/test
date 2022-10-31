import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { AppSetting, MessageService, ComboboxService, Constants } from 'src/app/shared';
import { NsMaterialGroupService } from '../../services/ns-material-group.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-nsmaterial-code-modal',
  templateUrl: './create-nsmaterial-code-modal.component.html',
  styleUrls: ['./create-nsmaterial-code-modal.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class CreateNsmaterialCodeModalComponent implements OnInit {
  ModalInfo = {
    Title: 'Tạo mã vật tư phi tiêu chuẩn',
  };

  model: any = {
    Id: '',
    Code: '',
    Name: '',
    ManufactureId: '',
    NSMaterialTypeId: '',
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,
  };
  manufactureId = '';
  startIndex = 0;
  index = 1;
  listData = [];
  listManufacture = [];
  listNSMaterialType = [];
  selectIndex = -1;
  isAction: boolean = false;
  code = '';
  isNext = false;
  listSelect = [];
  modelSelect = {
    Code: '',
    Value: ''
  }
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  constructor(
    public appset: AppSetting,
    private messageService: MessageService,
    private comboboxService: ComboboxService,
    private nsMaterialGroupService: NsMaterialGroupService,
    private activeModal: NgbActiveModal,
    public constants: Constants,
  ) { }

  ngOnInit() {
    this.getCbbManufacture();
    this.getCbbNSMaterialType();
    this.searchNSMaterialGroup();
  }

  getCbbManufacture() {
    this.comboboxService.getCbbManufacture().subscribe(
      data => {
        this.listManufacture = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbNSMaterialType() {
    this.comboboxService.getCbbNSMaterialType().subscribe(
      data => {
        this.listNSMaterialType = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  searchNSMaterialGroup() {
    this.model.ManufactureId = this.manufactureId;
    this.nsMaterialGroupService.getNSMaterialGroup(this.model).subscribe(
      data => {
        this.startIndex = ((this.model.PageNumber - 1) * this.model.PageSize + 1);
        this.listData = data.ListNSMaterialGroup;
        this.model.TotalItem = data.TotalItem;

      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getParamByIndex(index) {
    this.listSelect = [];
    this.selectIndex = index;
    this.code = this.listData[this.selectIndex].Code;
    this.listData[this.selectIndex].ListParameter.forEach(item => {
      var select = Object.assign({}, this.modelSelect);
      select.Value = '';
      select.Code = item.Code;
      this.listSelect.push(select);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  changeValue(param, index) {
    this.code = this.code.replace("[" + param.Code + "]", param.Value);
    this.listSelect[index].Value = param.Value;
  }

  createCode() {
    var strParam = '';
    for (var i = 0; i < this.listSelect.length; i++) {
      if (this.listSelect[i].Value == '') {
        strParam = this.listSelect[i].Code;
        break;
      }
    }
    if (strParam != '') {
      this.messageService.showMessage("Bạn không được để trống giá trị của thông số " + strParam);
    }
    else {
      this.messageService.showSuccess("Thêm mới mã vật tư phi tiêu chuẩn thành công!");
      this.activeModal.close(this.code);
    }

  }

  next() {
    if (this.selectIndex >= 0) {
      this.isNext = true;
    }
    else {
      this.messageService.showMessage("Chưa chọn mã vật tư, hãy kiểm tra lại!");
    }
  }

  back() {
    this.isNext = false;
  }

  clear() {
    this.model.Code = '';
    this.model.Name = '';
    this.model.ManufactureId = '';
    this.model.NSMaterialTypeId = '';
    this.searchNSMaterialGroup();
  }
}
