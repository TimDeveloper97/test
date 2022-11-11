   
import { Component, OnInit,  Input} from '@angular/core';
import { Router } from '@angular/router';

import { AppSetting, MessageService, Constants } from 'src/app/shared';
import { ErrorService } from '../../service/error.service';

@Component({
  selector: 'app-problem-exist-history-change-plan',
  templateUrl: './problem-exist-history-change-plan.component.html',
  styleUrls: ['./problem-exist-history-change-plan.component.scss']
})
export class ProblemExistHistoryChangePlanComponent implements OnInit {
  @Input() Id: string;
  ErrorId: string;
  ProjectId: string;
  ErrorFixId: string;

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private errorPlanService: ErrorService,
    public constants: Constants,
    private router: Router,
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
    this.router.navigate(['du-an/quan-ly-van-de']);
  }
}




  