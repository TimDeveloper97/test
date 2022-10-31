import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { ComboboxService, MessageService, Configuration, FileProcess, AppSetting, DateUtils, Constants } from 'src/app/shared';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ReportErrorService } from '../service/report-error.service';
import { ReportErrorProgressService } from '../service/report-error-progress.service';

@Component({
  selector: 'app-report-error-work',
  templateUrl: './report-error-work.component.html',
  styleUrls: ['./report-error-work.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ReportErrorWorkComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    private combobox: ComboboxService,
    private messageService: MessageService,
    private config: Configuration,
    public fileProcessImage: FileProcess,
    private appSetting: AppSetting,
    public dateUtils: DateUtils,
    private reportErrorService: ReportErrorService,
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
  };
  statIndex = 1;
  errorWorks: any[] = [];
  isProgress = false;

  @ViewChild('scrollPlan', { static: false }) scrollPlan: ElementRef;
  @ViewChild('scrollPlanHeader', { static: false }) scrollPlanHeader: ElementRef;

  ngOnInit() {
    this.searchModel.PageSize = 10;
    this.searchModel.TotalItems = 0;
    this.searchModel.PageNumber = 1;

    this.search();
  }

  ngAfterViewInit() {
    this.scrollPlan.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPlanHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollPlan.nativeElement.removeEventListener('ps-scroll-x', null);
  }

  search() {
    if (!this.isProgress) {
      this.getErrorWork();
    }
    else {
      this.getErrorProgressWork();
    }
  }

  getErrorWork() {
    this.reportErrorService.getWork(this.searchModel).subscribe(data => {
      this.searchModel.TotalItems = data.TotalItem;
      this.errorWorks = data.ListResult;
      this.statIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
    }, error => {
      this.messageService.showError(error);
    });
  }

  getErrorProgressWork() {
    this.reportErrorProgressService.getWork(this.searchModel).subscribe(data => {
      this.searchModel.TotalItems = data.TotalItem;
      this.errorWorks = data.ListResult;
      this.statIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
    }, error => {
      this.messageService.showError(error);
    });
  }

  closeModal() {
    this.activeModal.close();
  }
}
