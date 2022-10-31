import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { Subscription, forkJoin } from 'rxjs';
import * as moment from 'moment';

import { MessageService, AppSetting, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { PlanService } from '../../service/plan.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ScheduleProjectService } from '../../service/schedule-project.service';
import * as internal from 'stream';

@Component({
  selector: 'app-plan-create',
  templateUrl: './plan-create.component.html',
  styleUrls: ['./plan-create.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class PlanCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private planService: PlanService,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public comboboxService: ComboboxService,
    public dateUtils: DateUtils,
    private scheduleProjectService: ScheduleProjectService,
    public constants: Constants,

  ) { }
  Id: string;
  Types: number;
  ExecutionTime: any;
  model: any = {}

  ngOnInit() {
    this.getPlanInfo();
  }

  startDateTemp: any;
  endDateTemp: any;
  actualStartDate: any;
  actualEndDate: any;

  isAction: boolean = false;
  listPlan: any[] = []

  /**
   * Lấy thông tin công việc
   */
  getPlanInfo() {
    this.planService.getPlanInfo({ Id: this.Id, Types: this.Types}).subscribe(data => {
      this.model = data;
      this.model.Types = this.Types;
      this.model.ExecutionTime = this.ExecutionTime;

      if (data.ActualStartDate!= null)
      {
        this.model.ActualStartDate =  this.dateUtils.convertDateTimeToObject(data.ActualStartDate)
      }      

      if (data.ActualEndDate!= null)
      {
        this.model.ActualEndDate =  this.dateUtils.convertDateTimeToObject(data.ActualEndDate)
      }

      if (this.model.PlanStartDate!= null)
      {
        this.model.PlanStartDate =  this.dateUtils.convertDateTimeToObject(this.model.PlanStartDate)
      }      

      if (this.model.PlanDueDate!= null)
      {
        this.model.PlanDueDate =  this.dateUtils.convertDateTimeToObject(this.model.PlanDueDate)
      }

    }, error => {
      this.messageService.showError(error);
    });

  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }

  updatePlan() {

    this.model.Type = this.Types;
    if(this.model.ActualStartDate){
    this.model.ActualStartDate = this.dateUtils.convertObjectToDate(this.model.ActualStartDate);
    }

    if(this.model.ActualEndDate){
    this.model.ActualEndDate = this.dateUtils.convertObjectToDate(this.model.ActualEndDate);
    }

    if(this.model.PlanStartDate){
      this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.model.PlanStartDate);
    }

    if(this.model.PlanDueDate){
      this.model.PlanDueDate = this.dateUtils.convertObjectToDate(this.model.PlanDueDate);
    }

    this.planService.updateProgress(this.model).subscribe(data => {
      this.activeModal.close(data);
    }, error => {
      this.messageService.showError(error);
    });

  }  
}
