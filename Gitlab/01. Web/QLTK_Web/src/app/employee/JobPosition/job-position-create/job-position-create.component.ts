import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { MessageService, ComboboxService} from 'src/app/shared';
import { JobPositionServiceService } from '../../service/job-position-service.service';

@Component({
  selector: 'app-job-position-create',
  templateUrl: './job-position-create.component.html',
  styleUrls: ['./job-position-create.component.scss']
})
export class JobPositionCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private jobPosition: JobPositionServiceService,
    private combobox: ComboboxService
  ) { }
  ModalInfo = {
    Title: 'Thêm mới chức vụ',
    SaveText: 'Lưu',
  };

  isAction: boolean = false;
  Id: string;
  listJobResition: any[] = []
  model: any = {
    page: 1,
    PageSize: 10,
    totalItems: 0,
    PageNumber: 1,
    OrderBy: 'Index',
    OrderType: true,
    IsOnlyOne: false,
    Id: '',
    Name: '',
    Index: 1,
    Description: '',
  }
  ngOnInit() {

    if (this.Id) {
      this.ModalInfo.Title = 'Chỉnh sửa chức vụ';
      this.ModalInfo.SaveText = 'Lưu';
      this.getJobResitionInfo();

    }
    else {
      this.ModalInfo.Title = "Thêm chức vụ";

    }
    this.getCbbJobResition();
  }

  getJobResitionInfo() {
    this.jobPosition.GetJobPositions({ Id: this.Id }).subscribe(data => {
      this.model = data;
    });
  } 
  getCbbJobResition() {
    this.combobox.getCbbJobPositions().subscribe((data: any) => {
      this.listJobResition = data;
      if (this.Id == null || this.Id == '') {
        this.model.Index = data[data.length - 1].Index;
      } else {
        this.listJobResition.splice(this.listJobResition.length - 1, 1);
      }

    });
  }
  createJobResition(isContinue) {
    this.model.CreateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.jobPosition.AddJobPositions(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
         this.getCbbJobResition();
          this.model = {
            Id: '',
            Name: '',
            Index: 0,
            Description: '',
            IsOnlyOne: false,

          };
          this.messageService.showSuccess('Thêm mới chức vụ thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới chức vụ thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  updateJobResition() {
    this.model.UpdateBy = JSON.parse(localStorage.getItem('qltkcurrentUser')).userid;
    this.jobPosition.UpdateJobPositions(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật chức vụ thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
  save(isContinue: boolean) {
    if (this.Id) {
      this.updateJobResition();
    }
    else {
      this.createJobResition(isContinue);
    }
  }
  saveAndContinue() {
    this.save(true);
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
