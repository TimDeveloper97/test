import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { Constants, MessageService, AppSetting, ComboboxService } from 'src/app/shared';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PracticeGroupService } from '../../service/practice-group.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { SelectSpecializeComponent } from '../../Expert/select-specialize/select-specialize/select-specialize.component';

@Component({
  selector: 'app-practice-group-create',
  templateUrl: './practice-group-create.component.html',
  styleUrls: ['./practice-group-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PracticeGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private appSetting: AppSetting,
    private servicePracticeGroup: PracticeGroupService,
    private modalService: NgbModal,
    public constant: Constants,
    private checkSpecialCharacter: CheckSpecialCharacter,
    private combobox: ComboboxService
  ) { }


  ModalInfo = {
    Title: 'Thêm mới nhóm bài thực hành/công đoạn',
    SaveText: 'Lưu',

  };
  columnName: any[] = [{ Name: 'Code', Title: 'Mã nhóm' }, { Name: 'Name', Title: 'Tên nhóm' }];
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
    ListSpecialize: []
  }
  idUpdate: string;
  listPracticeGroup: any[] = [];
  listPracticeGroupId: any[] = [];
  listData: any = [];
  parentId: string;
  Id: string;
  listSpecialize: any = [];
  height = 0;
  ngOnInit() {
    this.height = window.innerHeight - 140;
    if (this.idUpdate) {
      this.getCbbPracticeGroupForUpdate();
      this.ModalInfo.Title = 'Chỉnh sửa nhóm bài thực hành';
      this.ModalInfo.SaveText = 'Lưu';
      this.getPracticeGroupInfo();
    }
    else {

      this.ModalInfo.Title = "Thêm mới nhóm bài thực hành";
      this.model.ParentId = this.parentId; // gán PanrenId 
    }
    this.getCbbPracticeGroup();
  }

  showSelectSpecialize() {
    let activeModal = this.modalService.open(SelectSpecializeComponent, { container: 'body', windowClass: 'select-specialize-model', backdrop: 'static' });
    //var ListIdSelectRequest = [];
    var ListIdSelect = [];
    this.listSpecialize.forEach(element => {
      ListIdSelect.push(element.Id);
    });

    activeModal.componentInstance.ListIdSelect = ListIdSelect;
    activeModal.result.then((result) => {
      if (result && result.length > 0) {
        result.forEach(element => {
          this.listSpecialize.push(element);
        });
      }
    }, (reason) => {

    });
  }

  showConfirmDeleteSpecialize(row) {
    this.messageService.showConfirm("Bạn có chắc muốn xoá chuyên môn này không?").then(
      data => {
        this.listSpecialize.splice(this.listSpecialize.indexOf(row), 1);
      },
      error => {
        
      }
    );
  }

  //lấy info update
  getPracticeGroupInfo() {
    this.servicePracticeGroup.getPracticeGroupInfo({ Id: this.idUpdate }).subscribe(data => {
      this.model = data;
      this.listSpecialize = data.ListSpecialize;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  save(isContinue: boolean) {
    this.model.ListPracticeStandard = this.listData;
    if (this.idUpdate) {
      this.updatePracticeGroup();
    }
    else {
      this.model.ListPracticeStandard = this.listData;
      this.createPracticeGroup(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  getCbbPracticeGroup() {
    this.combobox.getCbbPracticeGroup().subscribe((data: any) => {
      this.listPracticeGroup = data;
      // lấy list id expandedRowKeys 
      for (var item of this.listPracticeGroup) {
        this.listPracticeGroupId.push(item.Id);
      }
    });
  }

  getCbbPracticeGroupForUpdate() {
    this.servicePracticeGroup.getCbbPracticeGroupForUpdate({ Id: this.idUpdate }).subscribe((data: any) => {
      this.listPracticeGroup = data;
      // lấy list id expandedRowKeys 
      for (var item of this.listPracticeGroup) {
        this.listPracticeGroupId.push(item.Id);
      }
    });
  }

  supCreate(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.model.ListSpecialize = this.listSpecialize;
    this.servicePracticeGroup.createPracticeGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm bài thực hành thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm bài thực hành thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //Thêm mới
  createPracticeGroup(isContinue) {
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
    this.model.ListSpecialize = this.listSpecialize;
    this.servicePracticeGroup.updatePracticeGroup(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm bài thực hành thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  //cập nhật nhóm module
  updatePracticeGroup() {
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
