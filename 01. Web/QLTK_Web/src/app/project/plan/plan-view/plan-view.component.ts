import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { MessageService, AppSetting, Constants, DateUtils, ComboboxService } from 'src/app/shared';
import { PlanService } from '../../service/plan.service';
import { NgbActiveModal,NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CheckSpecialCharacter } from 'src/app/shared/common/check-special-characters';
import { ScheduleProjectService } from '../../service/schedule-project.service';
import { CreateTimePerformComponent } from '../create-time-perform/create-time-perform.component';

@Component({
  selector: 'app-plan-view',
  templateUrl: './plan-view.component.html',
  styleUrls: ['./plan-view.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class PlanViewComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    private planService: PlanService,
    private modalService: NgbModal,
    private checkSpecialCharacter: CheckSpecialCharacter,
    public comboboxService: ComboboxService,
    public dateUtils: DateUtils,
    private scheduleProjectService: ScheduleProjectService,
    public constant: Constants
  ) { }

  ModalInfo:any ={

  }
  Id: string;
  Types: number;
  ContractName: string;
  ResponsiblePersionName: string;
  EstimateTime : number;
  ExecutionTime : any;
  DesignCode: string;
  PlanDueDate: any;
  PlanStartDate: any;
  ActualStartDate: any;
  ActualEndDate: any;
  model: any = {};
  isDesignFinishDate = false;
  isMakeFinishDate = false;
  isTransferDate= false;
  Description: string;
  TaskName: string;
  Done: any;

  ngOnInit() {
    if(this.Types == 3)
    {
      this.model.ExecutionTime = this.ExecutionTime;
      this.model.ResponsiblePersionName = this.ResponsiblePersionName;
      this.model.Description = this.Description;
      this.model.TaskName = this.TaskName;
      this.model.Done = this.Done;
      if (this.ActualStartDate!= null)
      {
        this.model.ActualStartDate =  this.dateUtils.convertDateTimeToObject(this.ActualStartDate)
      }      

      if (this.ActualEndDate!= null)
      {
        this.model.ActualEndDate =  this.dateUtils.convertDateTimeToObject(this.ActualEndDate)
      }

      if (this.PlanStartDate!= null)
      {
        this.model.PlanStartDate =  this.dateUtils.convertDateTimeToObject(this.PlanStartDate)
      }      

      if (this.PlanDueDate!= null)
      {
        this.model.PlanDueDate =  this.dateUtils.convertDateTimeToObject(this.PlanDueDate)
      }

    }
    if(this.Types != 3)
    {
      this.getPlanInfo();
    }
    this.ModalInfo.Title = 'Xem thông tin kế hoạch';
  }

  getPlanInfo() {

    this.planService.getPlanView(this.Id, this.Types).subscribe(data => {
      this.model = data;
      this.model.ExecutionTime = this.ExecutionTime;
      this.model.EstimateTime = this.EstimateTime;
      this.model.DesignCode = this.DesignCode;
      this.model.PlanStartDate = this.PlanStartDate;
      this.model.PlanDueDate = this.PlanDueDate;
      this.model.ContractName = this.ContractName;
      this.model.ActualStartDate = this.ActualStartDate;
      this.model.ActualEndDate = this.ActualEndDate;
    }, error => {
      this.messageService.showError(error);
    });
  }

  showCreateTimePerform() {
    let activeModal = this.modalService.open(CreateTimePerformComponent, { container: 'body', windowClass: 'create-time-perform-model', backdrop: 'static' })
    activeModal.componentInstance.planId = this.model.Id;
    activeModal.componentInstance.namePlan = this.model.Name;
    activeModal.componentInstance.done = this.model.Done;
    activeModal.componentInstance.projectId = this.model.ProjectId;
    activeModal.result.then((result) => {
      if (result) {
      }
    }, (reason) => {
    });
  }

  closeModal() {
    this.activeModal.close();
  }
}
