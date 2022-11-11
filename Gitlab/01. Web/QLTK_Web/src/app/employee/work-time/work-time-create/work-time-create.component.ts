import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, FileProcess, DateUtils, Configuration } from 'src/app/shared';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { UploadfileService } from 'src/app/upload/uploadfile.service';
import { WorkTimeService } from '../../service/work-time.service';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-work-time-create',
  templateUrl: './work-time-create.component.html',
  styleUrls: ['./work-time-create.component.scss']
})
export class WorkTimeCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private messageService: MessageService,
    public constant: Constants,
    public fileProcess: FileProcess,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public dateUtils: DateUtils,
    private config: Configuration,
    public workTimeService: WorkTimeService,
  ) { }
  modalInfo = {
    Title: 'Thời gian làm việc',
    SaveText: 'Lưu',
  };
  isAction: boolean = false;
  Id: string;
  startTimeV:any;
  endTimeV:any;
  model: any = {
    Id: '',
    Name: '',
    StartTime: '',
    EndTime: ''
  }
  ngOnInit() {
    if (this.Id) {
      this.modalInfo.Title = 'Chỉnh sửa thời gian làm việc';
      this.modalInfo.SaveText = 'Lưu';
      this.getInfor();
    }
    else {
      this.startTimeV= {hour: 12, minute: 0};
      this.endTimeV= {hour: 12, minute: 0};
      this.modalInfo.Title = "Thêm mới thời gian làm việc";
    }
  }

  getInfor() {
    this.workTimeService.getInforWorkTime({ Id: this.Id }).subscribe(data => {
      this.model = data;
      this.startTimeV = this.dateUtils.convertTimeToObject(data.StartTime);
      this.endTimeV = this.dateUtils.convertTimeToObject(data.EndTime);
    });
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  createWorkTime(isContinue) {
    if(this.startTimeV != null){
      this.model.StartTime = this.dateUtils.convertObjectToTime(this.startTimeV);
    }
    if(this.endTimeV != null){
      this.model.EndTime = this.dateUtils.convertObjectToTime(this.endTimeV);
    }
    
    this.workTimeService.createWorkTime(this.model).subscribe(
      data => {
        if (isContinue) {
          this.isAction = true;
          this.model = {
            Id: '',
            Name: '',
            StartTime: '',
            EndTime: ''
          };
          this.messageService.showSuccess('Thêm mới thời gian làm việc thành công!');
        }
        else {
          this.messageService.showSuccess('Thêm mới thời gian làm việc thành công!');
          this.closeModal(true);
        }
      },
      error => {
        this.messageService.showError(error);
      });
  }


  updateWorkTime() {
    if(this.startTimeV != null){
      this.model.StartTime = this.dateUtils.convertObjectToTime(this.startTimeV);
    }
    if(this.endTimeV != null){
      this.model.EndTime = this.dateUtils.convertObjectToTime(this.endTimeV);
    }
    this.workTimeService.updateWorkTime(this.model).subscribe(
      () => {
        this.activeModal.close(true);
        this.messageService.showSuccess('Cập nhật thời gian làm việc thành công!');
      },
      error => {
        this.messageService.showError(error);
      }
    );

  }

  save(isContinue: boolean) {
    if (this.Id) {
      this.updateWorkTime();
    }
    else {
      this.createWorkTime(isContinue);
    }
  }

  saveAndContinue() {
    this.save(true);
  }

}
