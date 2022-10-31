import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { GroupUserService } from '../../service/group-user.service';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-group-user-create',
  templateUrl: './group-user-create.component.html',
  styleUrls: ['./group-user-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class GroupUserCreateComponent implements OnInit {
  @ViewChild('scrollHeaderOne',{static:false}) scrollHeaderOne: ElementRef;
  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: GroupUserService,
    private combobox: ComboboxService,
    public constant: Constants
  ) { }

  modalInfo = {
    Title: 'Thêm mới nhóm quyền',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  checkeds = false;
  listDepartmant: any[] = [];
  columnNameDepartment = [{ Name: 'Code', Title: 'Mã phòng ban' }, { Name: 'Name', Title: 'Tên phòng ban' }];
  listSBU: any[] = [];
  columnNameSBU = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  model: any = {
    Id: '',
    Name: '',
    DepartmentId: '',
    IsDisable: '1',
    Description: '',
    ListPermission: [],
    SBUId: ''
  }

  ngOnInit() {
    window.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollHeaderOne.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    this.getCbbSBU();
    if (this.Id) {
      this.model.Id = this.Id;
      this.modalInfo.Title = 'Chỉnh sửa nhóm quyền';
      this.modalInfo.SaveText = 'Lưu';
     
    }
    else {
      this.modalInfo.Title = "Thêm mới nhóm quyền";
      this.model.SBUId = JSON.parse(localStorage.getItem('qltkcurrentUser')).sbuId;
      this.model.DepartmentId = JSON.parse(localStorage.getItem('qltkcurrentUser')).departmentId;

    }
    this.getGroupUserInfo();
    this.getCbbDepartmentBySBU();
  }

  getGroupUserInfo() {
    this.service.getGroupUserInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  getCbbSBU() {
    this.combobox.getCbbSBU().subscribe(data => {
      this.listSBU = data;
    }, error => {
      this.messageService.showError(error);
    });
  }
  getCbbDepartmentBySBU() {
    this.combobox.getCbbDepartmentBySBU(this.model.SBUId).subscribe(data => {
      this.listDepartmant = data;
    }, error => {
      this.messageService.showError(error);
    });
  }

  createGroupUser(isContinue) {
    this.service.createGroupUser(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm quyền thành công!');
          this.getGroupUserInfo();
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm quyền thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateGroupUser(isUpdateUser) {
    this.model.IsUpdateUser = isUpdateUser;
    this.service.updateGroupUser(this.model).subscribe(
      data => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm quyền thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateGroupUser(false);
    }
    else {
      this.createGroupUser(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  
  saveAndUpdate() {
    if (this.Id) {
      this.updateGroupUser(true);
    }
  }


  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  selectAllFunction() {
    if (this.checkeds) {
      this.model.ListPermission.forEach(element => {
        element.Checked = true;
      });
    } else {
      this.model.ListPermission.forEach(element => {
        element.Checked = false;
      });
    }
  }

  checkParent(GroupFunctionId: any, Checked: any, index: any) {
    if (GroupFunctionId == "") {
      if (Checked) {
        for (let i = index + 1; i < this.model.ListPermission.length; i++) {
          if (this.model.ListPermission[i].Index == "") {
            this.model.ListPermission[i].Checked = true;
          } else break;
        }
      } else {
        for (let i = index + 1; i < this.model.ListPermission.length; i++) {
          if (this.model.ListPermission[i].Index == "") {
            this.model.ListPermission[i].Checked = false;
          } else break;
        }
      }
    }
  }
}
