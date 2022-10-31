import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, ComboboxService } from 'src/app/shared';
import { SkillGroupService } from '../service/skill--group.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-skill-group-create',
  templateUrl: './skill-group-create.component.html',
  styleUrls: ['./skill-group-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SkillGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private serviceSkillGroup: SkillGroupService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService,

  ) { }
  ModalInfo = {
    Title: 'Thêm mới nhóm kỹ năng',
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
    ParentId: null,
    Description: '',
  }
  idUpdate: string;
  listSkillGroup: any[] = [];
  listSkillGroupId: any[] = [];
  listData: any = [];
  parentId: string;
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
  ngOnInit() {
    if (this.idUpdate) {
      this.getCbbSkillGroupForUpdate();
      this.ModalInfo.Title = 'Chỉnh sửa nhóm kỹ năng';
      this.ModalInfo.SaveText = 'Lưu';
      this.getSkillGroupInfo();
    }
    else {
      this.getCbbSkillGroup();
      this.ModalInfo.Title = "Thêm mới nhóm kỹ năng";
      this.model.ParentId = this.parentId; // gán PanrenId 
    }
  }

  //lấy info update
  getSkillGroupInfo() {
    this.serviceSkillGroup.getSkillGroupInfo({ Id: this.idUpdate }).subscribe(data => {
      this.model = data;
    },
      error => {
        this.messageService.showError(error);
        this.closeModal(true);
      });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    this.model.ListSkillStandard = this.listData;
    if (this.idUpdate) {
      this.updateSkillGroup();
    }
    else {
      this.model.ListSkillStandard = this.listData;
      this.createSkillGroup(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  getCbbSkillGroup() {
    this.combobox.getCbbSkillGroup().subscribe((data: any) => {
      this.listSkillGroup = data;
      // lấy list id expandedRowKeys 
      for (var item of this.listSkillGroup) {
        this.listSkillGroupId.push(item.Id);
      }
    });
  }

  getCbbSkillGroupForUpdate() {
    this.serviceSkillGroup.getCbbSkillGroupForUpdate({ Id: this.idUpdate }).subscribe((data: any) => {
      this.listSkillGroup = data;
      // lấy list id expandedRowKeys 
      for (var item of this.listSkillGroup) {
        this.listSkillGroupId.push(item.Id);
      }
    });
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.serviceSkillGroup.createSkillGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            ParentId: null,
            Description: '',
          };
          this.messageService.showSuccess('Thêm mới nhóm kỹ năng thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm kỹ năng thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //Thêm mới
  createSkillGroup(isContinue) {
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
    this.serviceSkillGroup.updateSkillGroup(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm kỹ năng thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //cập nhật nhóm module
  updateSkillGroup() {
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