import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { MaterialGroupService } from '../../services/materialgroup-service';

@Component({
  selector: 'app-materialgroup-create',
  templateUrl: './materialgroup-create.component.html',
  styleUrls: ['./materialgroup-create.component.scss']
})
export class MaterialgroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private materialGroupService: MaterialGroupService
  ) { }

  ModalInfo = {
    Title: 'Thêm mới nhóm vật tư',
    SaveText: 'Lưu',

  };

  isAction: boolean = false;
  Id: string;
  ListTPA: any[] = [];
  ListMaterialGroup: any[] = [];
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    Id: '',
    MaterialGroupTPAId: '',
    MaterialGroupTPAName: '',
    MaterialGroupTPACode: '',
    Name: '',
    Code: '',
    ParentId: '',
    Description: '',
    Type: '', // 1: điện. điện tử  - 2: cơ khí
  }
  selectMaterialGroupId:'';
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }]

  treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  parentId: string;
  ngOnInit() {
    this.getCbbTPA();
    this.getCbbMaterialGroup();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa nhóm vật tư';
      this.ModalInfo.SaveText = 'Lưu';
      this.getMaterialGroupInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm nhóm vật tư";
      //this.model.ParentId = this.parentId;
      if(this.selectMaterialGroupId != null){
        this.model.ParentId = this.selectMaterialGroupId;
      }
    }
  }

  getName(MaterialGroupId) {
    for (var item of this.ListMaterialGroup) {
      if (item.Id == MaterialGroupId) {
        this.model.Code = item.Code;
      }
    }
  }

  getCbbTPA() {
    this.materialGroupService.getCbbTPA().subscribe((data: any) => {
      this.ListTPA = data;
    });
  }

  getCbbMaterialGroup() {
    this.materialGroupService.getCbbMaterialGroup(this.Id).subscribe((data: any) => {
      this.ListMaterialGroup = data;
    });
    
  }

  getMaterialGroupInfo() {
    this.materialGroupService.getMaterialGroupInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  createMaterialGroup(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var index1 = this.model.Code.indexOf("*");
    var index2 = this.model.Code.indexOf("{");
    var index3 = this.model.Code.indexOf("}");
    var index4 = this.model.Code.indexOf("!");
    var index5 = this.model.Code.indexOf("^");
    var index6 = this.model.Code.indexOf("<");
    var index7 = this.model.Code.indexOf(">");
    var index8 = this.model.Code.indexOf("?");
    var index9 = this.model.Code.indexOf("|");
    var index10 = this.model.Code.indexOf(",");
    var index11 = this.model.Code.indexOf("_");
    var index12 = this.model.Code.indexOf(" ");

    var validCode = true;
    if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
      || index10 != -1 || index11 != -1 || index12 != -1) {
      validCode = false;
    }


    if (validCode) {
      this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.materialGroupService.createMaterialGroup(this.model).subscribe(
        data => {
          if (isContinue) {
            this.isAction = true;
            this.model = {};
            this.messageService.showSuccess('Thêm mới nhóm vật tư thành công!');
          }
          else {
            this.messageService.showSuccess('Thêm mới nhóm vật tư thành công!');
            this.closeModal(true);
          }
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.materialGroupService.createMaterialGroup(this.model).subscribe(
            data => {
              if (isContinue) {
                this.isAction = true;
                this.model = {};
                this.messageService.showSuccess('Thêm mới nhóm vật tư thành công!');
              }
              else {
                this.messageService.showSuccess('Thêm mới nhóm vật tư thành công!');
                this.closeModal(true);
              }
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {
          
        }
      );
    }
  }

  updateMaterialGroup() {
    //kiểm tra ký tự đặc việt trong Mã
    var index1 = this.model.Code.indexOf("*");
    var index2 = this.model.Code.indexOf("{");
    var index3 = this.model.Code.indexOf("}");
    var index4 = this.model.Code.indexOf("!");
    var index5 = this.model.Code.indexOf("^");
    var index6 = this.model.Code.indexOf("<");
    var index7 = this.model.Code.indexOf(">");
    var index8 = this.model.Code.indexOf("?");
    var index9 = this.model.Code.indexOf("|");
    var index10 = this.model.Code.indexOf(",");
    var index11 = this.model.Code.indexOf("_");
    var index12 = this.model.Code.indexOf(" ");

    var validCode = true;
    if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
      || index10 != -1 || index11 != -1 || index12 != -1) {
      validCode = false;
    }


    if (validCode) {
      this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
      this.materialGroupService.updateMaterialGroup(this.model).subscribe(
        () => {
          this.activeModal.close(true);
          this.messageService.showSuccess('Cập nhật nhóm vật tư thành công!');
        },
        error => {
          this.messageService.showError(error);
        }
      );
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
          this.materialGroupService.updateMaterialGroup(this.model).subscribe(
            () => {
              this.activeModal.close(true);
              this.messageService.showSuccess('Cập nhật nhóm vật tư thành công!');
            },
            error => {
              this.messageService.showError(error);
            }
          );
        },
        error => {
          
        }
      );
    }
  }


  save(isContinue: boolean) {
    if (this.Id) {
      this.updateMaterialGroup();
    }
    else {
      this.createMaterialGroup(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  onRowDblClick() {
    this.isDropDownBoxOpened = false;
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.ParentId = e.selectedRowKeys[0];
    this.getName(e.selectedRowKeys[0]);
    //this.model.MaterialGroupSourceId = e.selectedRowKeys[0];
    // this.getParameterByGroupSourceId();
    this.closeDropDownBox();
  }
  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

}
