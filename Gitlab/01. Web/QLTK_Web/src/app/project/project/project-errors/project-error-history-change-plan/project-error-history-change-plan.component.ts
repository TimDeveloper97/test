import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { ErrorService } from '../../../service/error.service';

@Component({
  selector: 'app-project-error-history-change-plan',
  templateUrl: './project-error-history-change-plan.component.html',
  styleUrls: ['./project-error-history-change-plan.component.scss']
})
export class ProjectErrorHistoryChangePlanComponent implements OnInit {

  @Input() Id: string;
  ErrorId: string;
  ProjectId: string;
  ErrorFixId: string;

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    public constants: Constants,
    private activeModal: NgbActiveModal,
    private errorPlanService: ErrorService,
  ) { }

  listchangedPlan: any[] = [];
  model: any = {
    Id: '',
    ErrorId: '',
  }
  ngOnInit(): void {
    this.model.ErrorId = this.Id;
    this.searchChangedPlan();
  }

  searchChangedPlan() {
    this.errorPlanService.searchChangedPlan(this.model).subscribe((data: any) => {
      if (data.ListResult) {
        this.listchangedPlan = data.ListResult;
      }
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.activeModal.close(true);
  }

}
