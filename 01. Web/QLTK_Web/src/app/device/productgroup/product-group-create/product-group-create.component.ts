import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, AppSetting, Constants, ComboboxService } from 'src/app/shared';
import { ProductGroupService } from '../../services/product-group.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-product-group-create',
  templateUrl: './product-group-create.component.html',
  styleUrls: ['./product-group-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ProductGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private appSetting: AppSetting,
    private serviceProductGroup: ProductGroupService,
    private modalService: NgbModal,
    public constant: Constants,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService
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
    ParentId: '',
    Description: '',
  }
  idUpdate: string;
  listProductGroup: any[] = [];
  listProductGroupId: any[] = [];
  listData: any = [];
  parentId: string;
  Id: string;
  ngOnInit() {

    if (this.idUpdate) {
      this.getCbbProductGroupForUpdate();
      this.ModalInfo.Title = 'Chỉnh sửa nhóm thiết bị';
      this.ModalInfo.SaveText = 'Lưu';
      this.getProductGroupInfo();
    }
    else {
      this.model.ParentId = this.parentId; // gán PanrenId 
      this.getCbbProductGroup();
      this.ModalInfo.Title = "Thêm mới nhóm thiết bị";
    }
  }

  //lấy info update
  getProductGroupInfo() {
    this.serviceProductGroup.getProductGroupInfo({ Id: this.idUpdate }).subscribe(data => {
      this.model = data;
    },
    error => {
      this.messageService.showError(error);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    this.model.ListProductStandard = this.listData;
    if (this.idUpdate) {
      this.updateProductGroup();
    }
    else {
      this.model.ListProductStandard = this.listData;
      this.createProductGroup(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  getCbbProductGroup() {
    this.combobox.getCbbProductGroup().subscribe((data: any) => {
      this.listProductGroup = data.ListResult;
      // lấy list id expandedRowKeys 
      for (var item of this.listProductGroup) {
        this.listProductGroupId.push(item.Id);
      }
    });
  }

  getCbbProductGroupForUpdate() {
    this.serviceProductGroup.getCbbProductGroupForUpdate({ Id: this.idUpdate }).subscribe((data: any) => {
      this.listProductGroup = data;
      // lấy list id expandedRowKeys 
      for (var item of this.listProductGroup) {
        this.listProductGroupId.push(item.Id);
      }
    });
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.serviceProductGroup.createProductGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm thiết bị thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm thiết bị thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //Thêm mới
  createProductGroup(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supCreate(isContinue);
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supCreate(isContinue);
        },
        error => {
          
        }
      );
    }
  }

  supUpdate() {
    this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.serviceProductGroup.updateProductGroup(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm thiết bị thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //cập nhật nhóm module
  updateProductGroup() {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supUpdate();
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supUpdate();
        },
        error => {
          
        }
      );
    }
  }

  //Combobox đa cấp
  treeBoxValue: string;
  isDropDownBoxOpened = false;
  selectedRowKeys: any[] = [];
  rowFocusIndex = -1;
  syncTreeViewSelection() {
    if (!this.treeBoxValue) {
      this.selectedRowKeys = [];
    } else {
      this.selectedRowKeys = [this.treeBoxValue];
    }
  }

  treeView_itemSelectionChanged(e) {
    this.treeBoxValue = e.selectedRowKeys[0];
    this.model.ParentId = e.selectedRowKeys[0];
    this.closeDropDownBox();
  }
  closeDropDownBox() {
    this.isDropDownBoxOpened = false;
  }

}
