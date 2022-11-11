import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { DepartmentService } from '../../service/department.service';

@Component({
  selector: 'app-department-create',
  templateUrl: './department-create.component.html',
  styleUrls: ['./department-create.component.scss']
})
export class DepartmentCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private departmentservice: DepartmentService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  modalInfo = {
    Title: 'Thêm mới phòng ban',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  SBUId: string;
  listDepartment: any[] = []
  listManager = [];
  listSBU = [];
  columnName: any[] = [{ Name: 'Code', Title: 'Mã SBU' }, { Name: 'Name', Title: 'Tên SBU' }];
  model: any = {
    Id: '',
    Name: '',
    Code: '',
    Description: '',
    PhoneNumber: null,
    Status: '0',
    SBUId: '',
    IsDesign :false,
  }

  ngOnInit() {
    this.model.Code = 'ab';
    this.getCbSBU();
    this.getCbManager();
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa phòng ban';
      this.modalInfo.SaveText = 'Lưu';
      this.getDepartmentInfo();
    }
    else {
      this.modalInfo.Title = "Thêm mới phòng ban";
      this.model.SBUId = this.SBUId;
    }
  }

  getDepartmentInfo() {
    this.departmentservice.getDepartmentInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  getCbManager() {
    this.departmentservice.getCbbManager().subscribe(
      data => {
        this.listManager = data;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbSBU() {
    this.departmentservice.getCbbSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  createDepartment(isContinue) {
    //kiểm tra ký tự đặc việt trong Mã
    var validCode = this.checkSpecialCharacter.checkCode(this.model.Code);
    if (validCode) {
      this.supCreate(isContinue)
    } else {
      this.messageService.showConfirmCode("Mã không được chứa ký tự đặc biệt!").then(
        data => {
          this.supCreate(isContinue)
        },
        error => {
          
        }
      );
    }
  }

  updateDepartment() {
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

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateDepartment();
    }
    else {
      this.createDepartment(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

  supCreate(isContinue) {
    this.departmentservice.createDepartment(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model.Name = '';
          this.model.Code = '';
          this.model.Email = '';
          this.model.ManagerId = '';
          this.model.Status = '';
          this.messageService.showSuccess('Thêm mới phòng ban thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới phòng ban thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  supUpdate() {
    this.departmentservice.updateDepartment(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật phòng ban thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
