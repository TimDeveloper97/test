import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'src/app/shared';
import { TaskService } from '../../service/task.service';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';

@Component({
  selector: 'app-task-create',
  templateUrl: './task-create.component.html',
  styleUrls: ['./task-create.component.scss']
})
export class TaskCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private taskService: TaskService,
    private checkSpecialCharacter: CheckSpecialCharacter
  ) { }

  ngOnInit() {
    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa công việc';
      this.ModalInfo.SaveText = 'Lưu';
      this.getTaskInfo();
    }
    else {
      this.ModalInfo.Title = "Thêm mới công việc";
    }
  }

  ModalInfo = {
    Title: 'Thêm mới công việc',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;

  model: any = {
    Id: '',
    Name: '',
    Type: '',
    Description: '',
  }

  listType = [{ Id: 1, Name: 'Thiết kế' },
  { Id: 2, Name: 'Tài liệu' },
  { Id: 3, Name: 'Chuyển giao' },
  { Id: 4, Name: 'Giải pháp' },
  { Id: 5, Name: 'Hỗ trợ' },
]

  getTaskInfo() {
    this.taskService.getTaskInfo({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  saveAndContinue() {
    this.save(true);
  }

  createTask(isContinue) {
    this.addTask(isContinue);
  }

  addTask(isContinue) {
    this.taskService.createTask(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            Code: '',
            Description: '',
          };
          this.messageService.showSuccess('Thêm mới công việc thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới công việc thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateTask();
    }
    else {
      this.createTask(isContinue);
    }
  }

  saveTask() {
    this.taskService.updateTask(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật công việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  updateTask() {
    //kiểm tra ký tự đặc việt trong Mã
    this.saveTask();
  }
}
