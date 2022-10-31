import { Component, OnInit, Input, ViewEncapsulation, Directive } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

import { EmployeeUserTabService } from '../../service/employee-user-tab.service';
import { AppSetting, MessageService, Constants, ComboboxService } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import list from 'devextreme/ui/list';

@Component({
  selector: 'app-employee-user-tab',
  templateUrl: './employee-user-tab.component.html',
  styleUrls: ['./employee-user-tab.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class EmployeeUserTabComponent implements OnInit {
  @Input() Idemploy: string;
  @Input() EmployeeName: string;
  @Input() EmployeeCode: string;
  @Input() DepartmentId: string;
  constructor(
    private activeModal: NgbActiveModal,
    public appSetting: AppSetting,
    private messageService: MessageService,
    private router: Router,
    public constant: Constants,
    public comboboxService: ComboboxService,
    private employeeUserTabService: EmployeeUserTabService
  ) { }

  ModalInfo = {
    Title: 'Thêm mới nhân viên',
    SaveText: 'Lưu',
  };

  Id: string;
  isAction: boolean = false;
  listFunction: any[] = [];
  listPermission: any[] = [];
  listUserGroup: any[] = [];
  listCheckUserGroup: any[] = [];
  ListPermission: any[] = [];
  groupFunctions: any[] = [];
  isSelectAll = false;
  StartIndex = 0;
  listFunctionIndex = 0;
  modelUserGroup: any = {
    Id: '',
    Name: '',
    Code: '',
  }
  isIndeterminate = false;

  model: any = {
    PageSize: 10,
    TotalItems: 0,
    demlistPer: 0,
    PageNumber: 1,

    Id: '',
    UserName: '',
    IsDisable: 0,
    GroupUserId: '',
    EmployeeId: '',
    GroupFunctionId: '',
    IsCheck: false,
    ListGroupFunction: [],
  }
  groupSelectIndex = -1;

  index = 0;
  totalValue = 0;
  idGroup = "";
  perCheckTotal = 0;

  ngOnInit() {
    this.model.EmployeeId = this.Idemploy;
    forkJoin([
      this.comboboxService.getCBBGroupUser(this.model.EmployeeId),
      this.employeeUserTabService.getUserInfo({ EmployeeId: this.model.EmployeeId })]
    ).subscribe(([res1, res2]) => {
      if (this.model.EmployeeId) {
        this.ModalInfo.Title = 'Chỉnh sửa tài khoản';
        this.ModalInfo.SaveText = 'Lưu';

      }
      else {
        this.ModalInfo.Title = 'Thêm mới tài khoản';
      }

      this.listUserGroup = res1;
      this.groupFunctions = res2.ListGroupFunction;
      this.model = res2;
      this.groupSelectIndex = 0;
      
      for(let i = 0; i < this.groupFunctions.length; i++)
      {
        let checkCount = this.groupFunctions[i].CheckCount;
        let length = this.groupFunctions[i].Permissions.length;
        if (checkCount == 0) {
          this.groupFunctions[i].IsIndeterminate = false;
        }
        else {
          if (checkCount < length) {
            this.groupFunctions[i].IsIndeterminate = true;
          }
          else {
            this.groupFunctions[i].IsIndeterminate = false;
            this.isSelectAll = this.groupFunctions[i].CheckCount == this.groupFunctions[i].Permissions.length;
            this.groupFunctions[i].IsChecked = this.isSelectAll;
          }
        }
      }
    });
  }

  selectGroupFunction(group, index) {
    this.groupSelectIndex = index;
    // this.perCheckTotal = 0;
    // this.groupFunctions[index].Permissions.forEach(permission => {
    //   if (permission.IsChecked) {
    //     this.perCheckTotal++;
    //   }
    // });
    this.isSelectAll = this.groupFunctions[index].CheckCount == this.groupFunctions[index].Permissions.length;
  }

  changeGroupFunctionCheck(group, index) {
    group.Permissions.forEach(permission => {
      if (permission.IsChecked && !group.IsChecked) {
        // this.perCheckTotal--;
        group.CheckCount--;
      }
      if (!permission.IsChecked && group.IsChecked) {
        // this.perCheckTotal++;
        group.CheckCount++;
      }
      permission.IsChecked = group.IsChecked;
    });

    if (index == this.groupSelectIndex) {
      this.isSelectAll = group.IsChecked;
    }
    this.groupFunctions[index].IsIndeterminate = false;
  }

  selectAllPermission() {
    this.groupFunctions[this.groupSelectIndex].Permissions.forEach(permission => {
      if (permission.IsChecked && !this.isSelectAll) {
        // this.perCheckTotal--;
        this.groupFunctions[this.groupSelectIndex].CheckCount--;
      }
      if (!permission.IsChecked && this.isSelectAll) {
        // this.perCheckTotal++;
        this.groupFunctions[this.groupSelectIndex].CheckCount++;
      }
      permission.IsChecked = this.isSelectAll;
    });
    this.groupFunctions[this.groupSelectIndex].IsChecked = this.isSelectAll;
    this.groupFunctions[this.groupSelectIndex].IsIndeterminate = false;
  }

  selectPermission(permission) {
    if (!permission.IsChecked) {
      // this.perCheckTotal--;
      this.groupFunctions[this.groupSelectIndex].CheckCount--;
      let listPermission = this.groupFunctions[this.groupSelectIndex].Permissions.filter(t=>t.ScreenCode === permission.Code && t.IsChecked === true);
      if(listPermission.length > 0)
      {
        listPermission.forEach(item => {
          item.IsChecked = false;
          this.selectPermission(item);
        });
      }
    }
    else {
      // this.perCheckTotal++;
      this.groupFunctions[this.groupSelectIndex].CheckCount++;
      let mainPermission = this.groupFunctions[this.groupSelectIndex].Permissions.find(t=>t.Code === permission.ScreenCode);
      if(mainPermission && !mainPermission.IsChecked)
      {
        permission.IsChecked = false;
        this.messageService.showMessage("Quyền " + mainPermission.Name + " chưa được chọn nên không thể phân quyền này!");
      }
    }

    let checkCount = this.groupFunctions[this.groupSelectIndex].CheckCount;
    let length = this.groupFunctions[this.groupSelectIndex].Permissions.length;
    if (checkCount == 0) {
      this.groupFunctions[this.groupSelectIndex].IsIndeterminate = false;
    }
    else {
      if (checkCount < length) {
        this.groupFunctions[this.groupSelectIndex].IsIndeterminate = true;
      }
      else {
        this.groupFunctions[this.groupSelectIndex].IsIndeterminate = false;
        this.isSelectAll = this.groupFunctions[this.groupSelectIndex].CheckCount == this.groupFunctions[this.groupSelectIndex].Permissions.length;
        this.groupFunctions[this.groupSelectIndex].IsChecked = this.isSelectAll;
      }
    }

  }

  checkSpace() {
    var replaceName = this.model.UserName.trim();
    this.model.UserName = replaceName.split(' ').join('');
  }

  // lấy dữ liệu combobox
  getCBBGroupUser() {
    this.comboboxService.getCBBGroupUser(this.model.EmployeeId).subscribe(
      data => {
        this.listUserGroup = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  changeGroupUser() {
    if (this.model.GroupUserId) {
      this.isSelectAll = false;
      this.employeeUserTabService.getGroupPermission(this.model.GroupUserId).subscribe(
        data => {
          this.groupFunctions.forEach(group => {
            group.IsChecked = false;
            group.CheckCount = 0;
            group.Permissions.forEach(permission => {
              permission.IsChecked = false;
              data.forEach(groupF => {
                if (groupF.Id == permission.Id) {
                  permission.IsChecked = true;
                  group.CheckCount++;
                }
              });
            });

            let checkCount = group.CheckCount;
            let length = group.Permissions.length;
            group.IsChecked = group.CheckCount == group.Permissions.length;
            if (checkCount > 0 && checkCount < length) {
              group.IsIndeterminate = true;
            }
            else {
              group.IsIndeterminate = false;
            }
          });

          this.isSelectAll = this.groupFunctions[this.groupSelectIndex].CheckCount == this.groupFunctions[this.groupSelectIndex].Permissions.length;
        }, error => {
          this.messageService.showError(error);
        });
    }
  }

  save(isContinue: boolean) {
    if (this.model.Id) {
      var username = this.model.UserName.trim();
      if (username.indexOf(' ') > 0) {
        this.messageService.showConfirm("Tên tài khoản không được có dấu cách. Bạn có muốn xóa dấu cách không?").then(
          data => {
            this.checkSpace();
            this.update();
          },
          error => {
            
          }
        );
      }
      else {
        this.update();
      }

    }
    else {
      var username = this.model.UserName.trim();
      if (username.indexOf(' ') > 0) {
        this.messageService.showConfirm("Tên tài khoản không được có dấu cách. Bạn có muốn xóa dấu cách không?").then(
          data => {
            this.checkSpace();
            this.create();
          },
          error => {
            
          }
        );
      }
      else {
        this.create();
      }
    }
  }

  // thêm mới tài khoản
  create() {
    this.model.EmployeeId = this.Idemploy;
    this.model.ListGroupFunction = this.groupFunctions;
    this.employeeUserTabService.create(this.model).subscribe(
      data => {
        this.messageService.showMessage('Thêm tài khoản thành công!');
        this.closeModal(true);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  // cập nhật tài khoản
  update() {
    this.model.ListGroupFunction = this.groupFunctions;
    this.employeeUserTabService.UpdateUser(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật tài khoản thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.router.navigate(['nhan-vien/quan-ly-nhan-vien']);
  }

}
