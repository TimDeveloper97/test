import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { element } from 'protractor';
import { ComboboxService, Configuration, Constants, DateUtils, MessageService } from 'src/app/shared';
import { ScheduleProjectService } from '../../service/schedule-project.service';

@Component({
  selector: 'app-report-plan-by-date',
  templateUrl: './report-plan-by-date.component.html',
  styleUrls: ['./report-plan-by-date.component.scss'],
  encapsulation: ViewEncapsulation.None,

})
export class ReportPlanByDateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    public constant: Constants,
    private dateUtils: DateUtils,
    private messageService: MessageService,
    private scheduleProjectService: ScheduleProjectService,
    private config: Configuration,


  ) { }

  isAction: boolean = false;
  projectId: string;

  daysOfMonth: any = [];
  dayOfWeek: any = [];
  listProjectProduct: any = [];
  listExplanedId: any = [];
  ItemSearch: any[] = [];


  modelSearchProject: any = {
    ProjectId: '',
    PlanStartDate: null,
    PlanEndDate: null,
  }


  ngOnInit(): void {
    this.modelSearchProject.ProjectId = this.projectId;
  }

  closeModal(isOK: boolean) {
    this.activeModal.close(isOK ? isOK : this.isAction);
  }




  exportExcel() {
    if (this.modelSearchProject.PlanStartDate) {
      this.modelSearchProject.PlanStartDate = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanStartDate);
    }
    if (this.modelSearchProject.PlanEndDate) {
      this.modelSearchProject.PlanEndDate = this.dateUtils.convertObjectToDate(this.modelSearchProject.PlanEndDate);
    }
    this.exportExcelProjectPlan()
    this.closeModal(true);
  }

  exportExcelProjectPlan() {
    this.scheduleProjectService.exportExcelProjectPlan(this.modelSearchProject).subscribe(d => {
      var link = document.createElement('a');
      link.setAttribute("type", "hidden");
      link.href = this.config.ServerApi + d;
      link.download = 'Download.docx';
      document.body.appendChild(link);
      // link.focus();
      link.click();
      document.body.removeChild(link);
    }, e => {
      this.messageService.showError(e);
    });
  }

}
