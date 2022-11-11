import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, DateUtils, Constants } from 'src/app/shared';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportErrorProgressService } from '../service/report-error-progress.service';
@Component({
  selector: 'app-report-error-change-plan',
  templateUrl: './report-error-change-plan.component.html',
  styleUrls: ['./report-error-change-plan.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportErrorChangePlanComponent implements OnInit {

  constructor(
    private combobox: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private appSetting: AppSetting,
    public dateUtils: DateUtils,
    private reportErrorProgressService: ReportErrorProgressService,
    private router: Router,
    private activeModal: NgbActiveModal,
    private routeA: ActivatedRoute,
    public constant: Constants,
    public fileProcess: FileProcess,
  ) { }

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    Index: '',
    DepartmentId: '',
  };

  statIndex = 1;
  errorWorks: any[] = [];
  listDayChange: any[] = [];

  departmentSelectIndex = -1;
  
  @ViewChild('scrollPlan', { static: false }) scrollPlan: ElementRef;
  @ViewChild('scrollPlanHeader', { static: false }) scrollPlanHeader: ElementRef;

  ngOnInit(): void {
    this.searchModel.PageSize = 10;
    this.searchModel.TotalItems = 0;
    this.searchModel.PageNumber = 1;

    this.getErrorChangePlan();
  }

  closeModal() {
    this.activeModal.close();
  }

  getErrorChangePlan() {
    this.reportErrorProgressService.getErrorChangePlan(this.searchModel).subscribe(data => {
      //this.searchModel.TotalItems = data.TotalItem;
      this.errorWorks = data.result;
      this.listDayChange = data.listHead;
      this.statIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
    }, error => {
      this.messageService.showError(error);
    });
  }

  selectDepartment(index) {
    if (this.departmentSelectIndex != index) {
      this.departmentSelectIndex = index
    }
  }
}
