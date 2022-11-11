import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { MessageService, AppSetting, Constants, ComboboxService } from 'src/app/shared';
import { ConfigMaterialService } from '../services/config-material.service';

@Component({
  selector: 'app-config-material',
  templateUrl: './config-material.component.html',
  styleUrls: ['./config-material.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ConfigMaterialComponent implements OnInit {
  Index = 1;
  NameParameter = '';
  Value = '';
  ListMaterialGroup = [];
  ListMaterialGroupSource = [];
  ListParameter = [];
  ListParameterSource = [];
  ListValue = [];

  treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;

  treeSourceBoxValue: string;
  isDropDownSourceBoxOpened = false;
  selectedRowSourceKeys: any[] = [];
  rowSourceFocusIndex = -1;

  model = {
    MaterialGroupId: '',
    MaterialGroupSourceId: ''
  }

  parameterModel = {
    Id: '',
    MaterialGroupId: '',
    Name: '',
    ListValue: []
  }

  valueModel = {
    MaterialParameterId: '',
    Value: ''
  }
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  groupModel: any = {
    Id: '',
    ListParameter: []
  };

  paramSelectIndex = null;

  constructor(
    public appset: AppSetting,
    public constants: Constants,
    private messageService: MessageService,
    private configMaterialService: ConfigMaterialService,
    private comboboxService: ComboboxService
  ) { }

  ngOnInit() {
    this.appset.PageTitle = "Cấu hình thông số vật tư";
    this.getCbbMaterialGroup();
  }

  getCbbMaterialGroup() {
    this.configMaterialService.getListMaterialGroup().subscribe(
      data => {
        this.ListMaterialGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getParameterByGroupId() {
    this.configMaterialService.getParameterByGroupId(this.model.MaterialGroupId).subscribe(
      data => {
        this.groupModel.Id = this.model.MaterialGroupId;
        this.groupModel.ListParameter = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getParameterByGroupSourceId() {
    this.configMaterialService.getParameterByGroupId(this.model.MaterialGroupSourceId).subscribe(
      data => {
        this.ListParameterSource = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  checkParameter = true;
  addRowParameter() {
    this.checkParameter = true;
    if (this.ListParameter.length > 0) {
      this.ListParameter.forEach(item => {
        if (item.Name == this.NameParameter) {
          this.checkParameter = false;
        }
      });
    }

    if (this.NameParameter == '') {
      this.messageService.showMessage("Bạn không được để trống Tên thông số");
    }
    else if (this.NameParameter.length > 100) {
      this.messageService.showMessage("Tên thông số vượt quá 100 ký tự cho phép, vui lòng kiểm tra lại");
    }
    else if (!this.checkParameter) {
      this.messageService.showMessage("Tên thông số đã tồn tại trong nhóm vật tư, hãy kiểm tra lại");
    }
    else {
      var paramM = Object.assign({}, this.parameterModel);
      paramM.Name = this.NameParameter;
      this.groupModel.ListParameter.push(paramM);
      this.NameParameter = '';
    }
  }

  checkValue = true;
  addRowValue() {
    this.checkValue = true;
    if (this.ListValue.length > 0) {
      this.ListValue.forEach(item => {
        if (item.Value == this.Value) {
          this.checkValue = false;
        }
      });
    }

    if (this.Value == '') {
      this.messageService.showMessage("Bạn không được để trống Giá trị");
    }
    else if (this.Value.length > 50) {
      this.messageService.showMessage("Giá trị vượt quá 50 ký tự cho phép, vui lòng kiểm tra lại");
    }
    else if (!this.checkValue) {
      this.messageService.showMessage("Giá trị đã tồn tại trong thông số, hãy kiểm tra lại");
    }
    else if (this.selectIndex < 0) {
      this.messageService.showMessage("Bạn chưa chọn thông số, hãy kiểm tra lại");
    }
    else {
      var valueM = Object.assign({}, this.valueModel);
      valueM.Value = this.Value;
      this.groupModel.ListParameter[this.selectIndex].ListValue.push(valueM);
      this.Value = '';

    }
  }

  selectIndex = -1;
  loadValue(param, index) {
    this.selectIndex = index;
    this.Value = '';
    if (!param.Id && param.ListValue.length <= 0) {
      this.groupModel.ListParameter[this.selectIndex].ListValue = [];
    }
  }

  selectSourceIndex: any;
  parameterName = '';
  loadSelect(index, Name) {
    this.selectSourceIndex = index;
    this.parameterName = Name;
  }

  checkAdd = false;
  addFromSource() {
    this.checkAdd = true;
    this.ListParameter.forEach(item => {
      if (item.Name == this.parameterName) {
        this.checkAdd = false;
      }
    });

    if (this.parameterName == '') {
      this.messageService.showMessage("Bạn chưa chọn Thông số cần chuyển, kiểm tra lại");
    }
    else if (this.model.MaterialGroupId == '') {
      this.messageService.showMessage("Bạn chưa chọn Nhóm vật tư cần chuyển, kiểm tra lại");
    }
    else if (!this.checkAdd) {
      this.messageService.showMessage("Tên thông số đã có, bạn hãy kiểm tra lại");
    }
    else {
      var paramM = Object.assign({}, this.parameterModel);
      paramM.Name = this.parameterName;
      paramM.ListValue = [];
      this.groupModel.ListParameter.push(paramM);
      this.parameterName = '';
    }
  }

  save() {
    this.groupModel.Id = this.model.MaterialGroupId;
    if (this.groupModel.Id == '') {
      this.messageService.showMessage("Chưa chọn nhóm vật tư cần lưu!");
    }
    else {
      var isValid = true;
      if (this.groupModel.ListParameter.length > 0) {
        for (var i = 0; i < this.groupModel.ListParameter.length; i++) {
          if (this.groupModel.ListParameter[i].Name == '') {
            this.messageService.showMessage("Bạn không được để trống Tên thông số");
            isValid = false;
            break;
          }
          else if (this.groupModel.ListParameter[i].Name.length > 100) {
            this.messageService.showMessage("Tên thông số vượt quá 100 ký tự cho phép, vui lòng kiểm tra lại");
            isValid = false;
            break;
          }
          if (this.groupModel.ListParameter[i].ListValue.length > 0) {
            for (var j = 0; j < this.groupModel.ListParameter[i].ListValue.length; j++) {
              if (this.groupModel.ListParameter[i].ListValue[j].Value == '') {
                this.messageService.showMessage("Bạn không được để trống Giá trị");
                isValid = false;
                break;
              }
              else if (this.groupModel.ListParameter[i].ListValue[j].Value.length > 50) {
                this.messageService.showMessage("Giá trị vượt quá 50 ký tự cho phép, vui lòng kiểm tra lại");
                isValid = false;
                break;
              }
            }
          }

        }
      }

      if (isValid) {
        this.configMaterialService.saveConfig(this.groupModel).subscribe(
          data => {
            if (data) {
              this.selectIndex = -1;
              this.NameParameter = "";
              this.messageService.showSuccess("Thêm mới thông số vật tư thành công!");

            }
          },
          error => {
            this.messageService.showError(error);
          }
        );
      }
    }
  }

  deleteParam(id, index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá thông số này?").then(
      data => {
        if (data == true) {
          this.configMaterialService.checkRelationshipParam(id).subscribe((data: any) => {
            if (data) {
              this.messageService.showMessage("Thông số đang được sử dụng, không thể xóa!");
            }
            else {
              this.groupModel.ListParameter[index].IsDelete = true;
              this.groupModel.ListParameter.splice(index, 1);
              this.messageService.showSuccess("Xóa thông số vật tư thành công!");
              this.selectIndex--;
            }
          },
            error => {
              this.messageService.showError(error);
            });
        }
      },
      error => {
        
      }
    );
  }

  deleteValue(id, index) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xoá giá này?").then(
      data => {
        if (data == true) {
          this.configMaterialService.checkRelationshipValue(id).subscribe((data: any) => {
            if (data) {
              this.messageService.showMessage("Giá trị đang được sử dụng, không thể xóa!");
            }
            else {
              this.groupModel.ListParameter[this.selectIndex].ListValue[index].IsDelete = true;
              this.groupModel.ListParameter[this.selectIndex].ListValue.splice(index, 1);
              this.messageService.showMessage("Xóa giá trị thông số vật tư thành công!");
            }
          },
            error => {
              this.messageService.showError(error);
            });
        }
      },
      error => {
        
      }
    );
  }

  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

  closeDropDownBoxSource() {
    this.isDropDownSourceBoxOpened = false;
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.MaterialGroupId = e.selectedRowKeys[0];
    this.getParameterByGroupId();
    this.closeDropDownBox();
  }

  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  syncTreeViewSourceSelection() {
    if (!this.treeSourceBoxValue) {
      this.selectedRowSourceKeys = [];
    } else {
      this.selectedRowSourceKeys = [this.treeSourceBoxValue];
    }
  }

  treeViewSource_itemSelectionChanged(e) {
    this.treeSourceBoxValue = e.selectedRowKeys[0];
    this.model.MaterialGroupSourceId = e.selectedRowKeys[0];
    this.getParameterByGroupSourceId();
    this.closeDropDownBoxSource();
  }

  showConfirmDelete(id:string)
  {

  }
}
