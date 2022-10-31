import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { WorkLocationService } from 'src/app/employee/service/work-location.service';
import { MessageService } from 'src/app/shared';

@Component({
  selector: 'app-work-location-create',
  templateUrl: './work-location-create.component.html',
  styleUrls: ['./work-location-create.component.scss']
})
export class WorkLocationCreateComponent implements OnInit {

  constructor(private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private workLocationService: WorkLocationService,) { }

  modalInfo = {
    Title: 'Thêm mới nơi làm việc',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  id: string;

  worklocationModel: any = {
    Id: '',
    Name: '',
    Note: ''
  };

  ngOnInit(): void {
    if (this.id) {
      this.modalInfo.Title = 'Chỉnh sửa nơi làm việc';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfo();
    }
    else {
      this.modalInfo.Title = 'Thêm mới nơi làm việc';
    }
  }

  getInfo() {
    this.workLocationService.getInfo({ Id: this.id }).subscribe(
      result => {
        this.worklocationModel = result;
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  create(isContinue) {
    this.workLocationService.create(this.worklocationModel).subscribe(
      data => {
        if (isContinue) {
          this.clear();
          this.isAction = true;
          this.messageService.showSuccess('Thêm mới nơi làm việc thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới nơi làm việc thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  update() {
    this.workLocationService.update(this.worklocationModel).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật nơi làm việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  save(isContinue: boolean) {
    if (this.id) {
      this.update();
    }
    else {
      this.create(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  clear() {
    this.worklocationModel = {
      Id: '',
      Name: '',
      Note: ''
    };
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

}
