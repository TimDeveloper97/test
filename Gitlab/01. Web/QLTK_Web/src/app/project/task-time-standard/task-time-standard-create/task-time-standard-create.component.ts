import { Component, OnInit } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, ComboboxService } from 'src/app/shared';
import { TaskTimeStandardService } from '../../service/task-time-standard.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-task-time-standard-create',
  templateUrl: './task-time-standard-create.component.html',
  styleUrls: ['./task-time-standard-create.component.scss']
})
export class TaskTimeStandardCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private taskTimeStandardService: TaskTimeStandardService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public constant: Constants,
    private comboboxService: ComboboxService
  ) { }

  ngOnInit() {
    this.getCBBSBU();
    this.getListEmployee();
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa thời gian tiêu chuẩn';
      this.ModalInfo.SaveText = 'Lưu';
      this.getTaskTimeStandardInfo();

    }
    else {
      this.ModalInfo.Title = "Thêm mới thời gian tiêu chuẩn";
    }
  }

  ModalInfo = {
    Title: 'Thêm mới thời gian tiêu chuẩn cho từng loại công việc',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    Type: '',
    SBUId: '',
    DepartmentId: '',
    EmployeeId: '',
    TimeStandard: '',
  }

  listSBU: any[] = [];
  listDepartment: any[] = [];
  listEmployee: any = [];

  getCBBSBU() {
    this.comboboxService.getCBBSBU().subscribe(
      data => {
        this.listSBU = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getCbbDepartment() {
    this.comboboxService.getCbbDepartmentBySBU(this.model.SBUId).subscribe(
      data => {
        this.listDepartment = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getListEmployee() {
    this.comboboxService.getEmployeeByDepartmentWithStatus(this.model.DepartmentId).subscribe(
      data => {
        this.listEmployee = data;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  getTaskTimeStandardInfo() {
    this.taskTimeStandardService.getTaskTimeStandardInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.getCbbDepartment();
      this.getListEmployee();
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }
  createTaskTimeStandard(isContinue) {
    this.addTaskTimeStandard(isContinue);
  }

  addTaskTimeStandard(isContinue) {
    this.taskTimeStandardService.createTaskTimeStandard(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Type: '',
            DepartmentId: '',
            EmployeeId: '',
            TimeStandard: '',
          };
          this.messageService.showSuccess('Thêm mới thời gian tiêu chuẩn thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới thời gian tiêu chuẩn thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateTaskTimeStandard();
    }
    else {
      this.createTaskTimeStandard(isContinue);
    }
  }

  saveTaskTimeStandard() {
    this.taskTimeStandardService.updateTaskTimeStandard(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật thời gian tiêu chuẩn thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateTaskTimeStandard() {
    this.saveTaskTimeStandard();
  }

}
