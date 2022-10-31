import { Component, OnInit } from '@angular/core';
import { NgbActiveModal,  NgbTimeAdapter, NgbTimepickerConfig } from '@ng-bootstrap/ng-bootstrap';
import { MessageService, Constants, DateUtils } from 'src/app/shared';
import { NtsTimeStringAdapter } from 'src/app/shared/common/nts-time-string-adapter';
import { PlanService } from '../../service/plan.service';

@Component({
  selector: 'app-create-time-perform',
  templateUrl: './create-time-perform.component.html',
  styleUrls: ['./create-time-perform.component.scss'],
  providers: [{ provide: NgbTimeAdapter, useClass: NtsTimeStringAdapter }]
})
export class CreateTimePerformComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private service: PlanService,
    public dateUtils: DateUtils,
    public constants: Constants,
    private ngbTimepickerConfig: NgbTimepickerConfig
  ) {
    ngbTimepickerConfig.spinners = false;
  }

  modalInfo = {
    Title: 'Thời gian thực hiện',
    SaveText: 'Lưu',
  };

  date = new Date();
  isAction: boolean = false;
  startWorking: any;
  planId: string;
  namePlan: string;
  done: number;
  projectId: string;
  model: any = {
    Id: '',
    Name: '',
    ObjectId: '',
    ProjectId: null,
    WorkDate: null,
    TotalTime: 0,
    Done: 0,
    SBUId: '',
    DepartmentId: '',
    ObjectType: 1,
    CreateBy: '',
    Note: '',
    StartTime: null,
    EndTime: null
  }

  ngOnInit() {
    this.model.ObjectId = this.planId;
    this.model.Name = this.namePlan;
    this.model.Done = this.done;
    this.model.ProjectId = this.projectId;
    this.startWorking = this.dateUtils.createObjectDate(this.date.getFullYear(), this.date.getMonth() + 1, this.date.getDate());
  }

  addWorkDiary() {
    if (this.startWorking != null) {
      this.model.WorkDate = this.dateUtils.convertObjectToDate(this.startWorking);
    }
    else {
      this.model.WorkDate = null;
    }

    this.service.addWorkDiary(this.model).subscribe(
      data => {
        this.messageService.showSuccess('Cập nhật thời gian thực hiện thành công!');
        this.closeModal(true);
      }, error => {
        this.messageService.showError(error);
      }
    );
  }

  save() {
    this.addWorkDiary();
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }
}
