import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';

import { EmployGroupServiceService } from '../../service/employ-group-service.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-employee-group-create',
  templateUrl: './employee-group-create.component.html',
  styleUrls: ['./employee-group-create.component.scss']
})
export class EmployeeGroupCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private employeeGroupService: EmployGroupServiceService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ModalInfo = {
    Title: 'Thêm mới nhóm tiêu chí',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  EmployeeGroupId: string;
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Code',
    OrderType: true,

    EmployeeGroupId: '',
    Name: '',
    Code: '',
    Note: '',
    IsUpdateUser: false
  }

  ngOnInit() {
    if (this.EmployeeGroupId) {
      this.ModalInfo.Title = 'Chỉnh sửa nhóm nhân viên';
      this.ModalInfo.SaveText = 'Lưu';
      this.getEmployeeGroup();
    }
    else {
      this.ModalInfo.Title = 'Thêm mới nhóm nhân viên';
    }
  }

  getEmployeeGroup() {
    this.employeeGroupService.GetEmployeeGroup({ EmployeeGroupId: this.EmployeeGroupId }).subscribe(data => {
      this.model = data;
    });
  }

  createEmployeGroup(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.employeeGroupService.AddEmployeeGroup(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {};
          this.messageService.showSuccess('Thêm mới nhóm nhân viên thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nhóm nhân viên thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateEmployeeGroup() {
    this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.employeeGroupService.UpdateEmployeeGroup(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nhóm nhân viên thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      if (this.EmployeeGroupId) {
        this.updateEmployeeGroup();
      }
      else {
        this.createEmployeGroup(isContinue);
      }
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          if (this.EmployeeGroupId) {
            this.updateEmployeeGroup();
          }
          else {
            this.createEmployeGroup(isContinue);
          }
        },
        error => {
          
        }
      );
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
