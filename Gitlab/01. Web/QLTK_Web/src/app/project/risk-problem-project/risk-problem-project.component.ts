import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { ScheduleProjectService } from '../service/schedule-project.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-risk-problem-project',
  templateUrl: './risk-problem-project.component.html',
  styleUrls: ['./risk-problem-project.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RiskProblemProjectComponent implements OnInit {

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,

    Name: '',
    PhoneNumber: "",
    WorkTypeId: '',
    Email: '',
    ApplyDateTo: null,
    ApplyDateFrom: null,
    ApplyDateToV: null,
    ApplyDateFromV: null,
    Status:'0'
  };

  searchOptions: any = {
    FieldContentName: 'Name',
    Placeholder: 'Tìm kiếm theo mã/tên ứng viên',
    Items: [
      
    ]
  };

  overallProject: any[] = []
  startIndex: number = 1;
  projectId: string;
  listStageOfProduct: any[] = [];

  constructor(public appSetting: AppSetting,
    private messageService: MessageService,
    public config: Configuration,
    private router: Router,
    private modalService: NgbModal,
    public constant: Constants,
    private comboboxService: ComboboxService,
    private scheduleProjectService: ScheduleProjectService,
    private activeModal: NgbActiveModal,
    private dateUtils: DateUtils) { }

  ngOnInit(): void {
    this.searchRiskProblemProject();
  }

  searchRiskProblemProject() {
    this.searchModel.ProjectId = this.projectId;

    this.scheduleProjectService.searchRiskProblemProject(this.searchModel).subscribe(
      data => {
        this.overallProject = data.ListResult;
        this.listStageOfProduct = data.ListDayChange;
        this.searchModel.TotalItems = data.TotalItem;
        this.startIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
      },
      error => {
        this.messageService.showError(error);
      });
  }

  clear() {
    this.searchModel = {

    };
    this.searchRiskProblemProject();
  } 
  CloseModal() {
    this.activeModal.close(false);
  }
  exportExcel() {
    this.scheduleProjectService.exportExcelWorkingReport(this.projectId).subscribe(d => {
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
