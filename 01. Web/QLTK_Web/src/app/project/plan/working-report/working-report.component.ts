import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AppSetting, ComboboxService, Configuration, Constants, MessageService, DateUtils } from 'src/app/shared';
import { PlanService } from '../../service/plan.service';
import { ScheduleProjectService } from '../../service/schedule-project.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-working-report',
  templateUrl: './working-report.component.html',
  styleUrls: ['./working-report.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class WorkingReportComponent implements OnInit {

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

  workingReport: any[] = []
  startIndex: number = 1;
  projectId: string;

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
    this.searchWorkingReport();
  }

  searchWorkingReport() {
    this.searchModel.ProjectId = this.projectId;

    this.searchModel.DateFromActualStartDate = null;
    if (this.searchModel.DateFromActualStartDateV != null) {
      this.searchModel.DateFromActualStartDate = this.dateUtils.convertObjectToDate(this.searchModel.DateFromActualStartDateV);
    }else {
      this.searchModel.DateFromActualStartDate = null;
    }

    this.searchModel.DateToActualStartDate = null;
    if (this.searchModel.DateToActualStartDateV != null) {
      this.searchModel.DateToActualStartDate = this.dateUtils.convertObjectToDate(this.searchModel.DateToActualStartDateV);
    }else {
      this.searchModel.DateToActualStartDate = null;
    }

    this.searchModel.DateFromActualEndDate = null;
    if (this.searchModel.DateFromActualEndDateV != null) {
      this.searchModel.DateFromActualEndDate = this.dateUtils.convertObjectToDate(this.searchModel.DateFromActualEndDateV);
    }else {
      this.searchModel.DateFromActualEndDate = null;
    }

    this.searchModel.DateToActualEndDate = null;
    if (this.searchModel.DateToActualEndDateV != null) {
      this.searchModel.DateToActualEndDate = this.dateUtils.convertObjectToDate(this.searchModel.DateToActualEndDateV);
    }else {
      this.searchModel.DateToActualEndDate = null;
    }

    this.scheduleProjectService.searchWorkingReport(this.searchModel).subscribe(
      data => {
        this.workingReport = data.ListResult;
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
    this.searchWorkingReport();
  } 

  CloseModal() {
    this.activeModal.close(false);
  }
  exportExcel() {
    this.searchModel.ProjectId = this.projectId;
    this.searchModel.DateFromActualStartDate = null;
    if (this.searchModel.DateFromActualStartDateV != null) {
      this.searchModel.DateFromActualStartDate = this.dateUtils.convertObjectToDate(this.searchModel.DateFromActualStartDateV);
    }else {
      this.searchModel.DateFromActualStartDate = null;
    }

    this.searchModel.DateToActualStartDate = null;
    if (this.searchModel.DateToActualStartDateV != null) {
      this.searchModel.DateToActualStartDate = this.dateUtils.convertObjectToDate(this.searchModel.DateToActualStartDateV);
    }else {
      this.searchModel.DateToActualStartDate = null;
    }

    this.searchModel.DateFromActualEndDate = null;
    if (this.searchModel.DateFromActualEndDateV != null) {
      this.searchModel.DateFromActualEndDate = this.dateUtils.convertObjectToDate(this.searchModel.DateFromActualEndDateV);
    }else {
      this.searchModel.DateFromActualEndDate = null;
    }

    this.searchModel.DateToActualEndDate = null;
    if (this.searchModel.DateToActualEndDateV != null) {
      this.searchModel.DateToActualEndDate = this.dateUtils.convertObjectToDate(this.searchModel.DateToActualEndDateV);
    }else {
      this.searchModel.DateToActualEndDate = null;
    }
    this.scheduleProjectService.exportExcelWorkingReport(this.searchModel).subscribe(d => {
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
