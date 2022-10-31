import { Component, OnInit } from '@angular/core';

import { Constants, MessageService, ComboboxService, AppSetting, Configuration } from 'src/app/shared';
import { MaterialGroupService } from '../../services/materialgroup-service';
import { CodeRuleService } from '../../services/code-rule.service';
import { ChooseMaterialGroupModalComponent } from '../choose-material-group-modal/choose-material-group-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Component({
  selector: 'app-code-rule-manage',
  templateUrl: './code-rule-manage.component.html',
  styleUrls: ['./code-rule-manage.component.scss']
})
export class CodeRuleManageComponent implements OnInit {

  Index = 1;
  listCodeRule = [];
  code = '';
  materialGroupId = '';
  materialGroupTPAId = '';
  manufactureId = "";
  unitId = "";
  type = "";
  length = 0;
  ListMaterialGroupTPA = [];
  ListMaterialGroup = [];
  ListMaterialGroupId = [];

  treeBoxValueMaterialGroup: string;
  isDropDownBoxOpenedMaterialGroup = false;

  materialGroupValue: string;
  isOpenedMaterialGroup = false;

  newRow = {
    Code: '',
    MaterialGroupId: '',
    MaterialGroupName: '',
    MaterialGroupTPAId: '',
    MaterialGroupTPAName: '',
    Length: 0,
    ManufactureId: '',
    Type: '',
    UnitId: '',
  }

  codeRuleModel = {
    ListModel: []
  }

  searchModel={
    Code: ''
  }

  listType: any = [{ Id: "1", Name: "Vật tư tiêu chuẩn" }, { Id: "2", Name: "Vật tư phi tiêu chuẩn" }];

  constructor(
    public constants: Constants,
    private comboboxService: ComboboxService,
    private messageService: MessageService,
    private materialGroupService: MaterialGroupService,
    private codeRuleService: CodeRuleService,
    private modalService: NgbModal,
    public appSetting: AppSetting,
    private config: Configuration,
  ) { }
  scrollConfig: PerfectScrollbarConfigInterface = {
    suppressScrollX: false,
    suppressScrollY: true,
    minScrollbarLength: 20,
    wheelPropagation: true
  };
  listManufacture: any = [];
  listUnit: any = [];

  ngOnInit() {
    this.appSetting.PageTitle = "Định nghĩa mã vật tư";
    this.getMaterialGroup();
    this.getMaterialGroupTPA();
    this.getCBBManufacture();
    this.getCBBUnit();
    this.searchCodeRule();
  }
  searchOptions: any = {
    FieldContentName: 'Code',
    Placeholder: 'Ký tự bắt đầu của Mã Vật Tư',
    Items: [
    ]
  };
  isValid = true;
  checkValid() {
    for (var element of this.listCodeRule) {
      if (!element.Code || !element.MaterialGroupId || !element.MaterialGroupTPAId || !element.ManufactureId || !element.Type || !element.UnitId) {
        this.isValid = false;
        break;
      }
    }
  }

  deleteCodeRule(index: any) {
    this.messageService.showConfirm("Bạn có chắc chắn muốn xóa định nghĩa mã vật tư này không?").then(
      data => {
        this.listCodeRule.splice(index, 1);
      },
      error => {
        
      }
    );
  }

  getCBBManufacture() {
    this.comboboxService.getCbbManufacture().subscribe((data: any) => {
      if (data) {
        this.listManufacture = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getCBBUnit() {
    this.comboboxService.getCbbUnit().subscribe((data: any) => {
      if (data) {
        this.listUnit = data;
        for (var i of this.listUnit) {
          if (i.Id == "" || i.Id == null) {
            this.listUnit.splice(this.listUnit.indexOf(i), 1);
          }
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  addRow() {
    if(this.code !=''){
      var row = Object.assign({}, this.newRow);
      row.Code = this.code;
      row.Length = this.length;
      row.MaterialGroupId = this.materialGroupId[0];
      this.ListMaterialGroup.forEach(item => {
        if (item.Id == row.MaterialGroupId) {
          row.MaterialGroupName = item.Name;
        }
      });
      row.MaterialGroupTPAId = this.materialGroupTPAId;
      this.ListMaterialGroupTPA.forEach(item => {
        if (item.Id == row.MaterialGroupTPAId) {
          row.MaterialGroupTPAName = item.Name;
        }
      });
  
      row.ManufactureId = this.manufactureId;
      row.UnitId = this.unitId;
      row.Type = this.type;
      this.listCodeRule.push(row);
      this.code = '';
      this.length = 0;
      this.materialGroupId = '';
      this.materialGroupTPAId = '';
      this.manufactureId = '';
      this.unitId = '';
      this.type = '';
      this.treeBoxValueMaterialGroup = '';
    }
    else{
      this.messageService.showMessage("Bạn không được để trống mã!");
    }
    
  }

  getMaterialGroup() {
    this.materialGroupService.getListMaterialGroup().subscribe((data: any) => {
      if (data) {
        this.ListMaterialGroup = data;
        for (var item of this.ListMaterialGroup) {
          this.ListMaterialGroupId.push(item.Id);
        }
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getMaterialGroupTPA() {
    this.comboboxService.getCbbMaterialGroupTPA().subscribe((data: any) => {
      if (data) {
        this.ListMaterialGroupTPA = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  syncTreeViewSelectionMaterialGroup() {
    if (!this.treeBoxValueMaterialGroup) {
      this.newRow.MaterialGroupId = '';
    } else {
      this.newRow.MaterialGroupId = this.treeBoxValueMaterialGroup;
    }
  }

  treeViewMaterialGroup(e) {
    this.treeBoxValueMaterialGroup = e.selectedRowKeys[0];
    this.materialGroupTPAId = e.selectedRowsData[0].MaterialGroupTPAId;
    this.closeDropDownBox();
  }

  closeDropDownBox() {
    this.isDropDownBoxOpenedMaterialGroup = false;
  }

  save() {
    this.checkValid();
    if (this.isValid) {
      this.codeRuleModel.ListModel = this.listCodeRule;
      this.codeRuleService.saveCodeRule(this.codeRuleModel).subscribe((data: any) => {
        if (data) {
          this.messageService.showSuccess('Lưu cấu hình mã vật tư thành công!');
          this.searchCodeRule();
        }
      },
        error => {
          this.messageService.showError(error);
        });
    } else {
      this.messageService.showMessage("Bạn chưa nhập đầy đủ thông tin! Vui lòng nhập lại.");
      this.isValid = true;
    }

  }

  searchCodeRule() {
    this.codeRuleService.searchCodeRule(this.searchModel).subscribe((data: any) => {
      if (data) {
        this.listCodeRule = data;
      }
    },
      error => {
        this.messageService.showError(error);
      });
  }

  chooseMaterialGroup(i) {
    let activeModal = this.modalService.open(ChooseMaterialGroupModalComponent, { container: 'body', windowClass: 'module-choose-folder-download', backdrop: 'static' });
    activeModal.result.then((result) => {
      if (result) {
        this.listCodeRule[i].MaterialGroupTPAId = result.MaterialGroupTPAId;
        this.listCodeRule[i].MaterialGroupName = result.MaterialGroupName;
        this.listCodeRule[i].MaterialGroupId = result.MaterialGroupId;
      }
    }, (reason) => {

    });
  }

  clear(){
    this.searchModel.Code = '';
    this.searchCodeRule();

  }
}
