import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Constants, MessageService } from 'src/app/shared';
import * as internal from 'stream';
import { ProjectServiceService } from '../../service/project-service.service';

@Component({
  selector: 'app-show-list-money-collection',
  templateUrl: './show-list-money-collection.component.html',
  styleUrls: ['./show-list-money-collection.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowListMoneyCollectionComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    private activeModal: NgbActiveModal,
    private messageService: MessageService,
    public service: ProjectServiceService,
    public constant: Constants
  ) { }

  @ViewChild('scrollPlan', { static: false }) scrollPlan: ElementRef;
  @ViewChild('scrollPlanHeader', { static: false }) scrollPlanHeader: ElementRef;

  searchModel: any = {
    PageSize: 10,
    TotalItems: 0,
    PageNumber: 1,
    OrderBy: 'ProjectId',
    OrderType: true,
  };
  IsExist: boolean;
  ProjectId: string;
  Month: number;
  Year: number;
  TotalProject: number;
  DepartmentId: string;
  CustomerId: string;
  IsTotal: any;
  ListProject: any[] = [];

  statIndex = 1;
  listReport: any[] = [];

  ngOnInit(): void {
    this.searchModel.PageSize = 10;
    this.searchModel.TotalItems = 0;
    this.searchModel.PageNumber = 1;
    this.searchModel.DepartmentId = this.DepartmentId;
    this.searchModel.ProjectId = this.ProjectId;
    this.searchModel.CustomerId = this.CustomerId;
    this.searchModel.Month = this.Month;
    this.searchModel.IsExist = this.IsExist;
    this.searchModel.Year = this.Year;
    this.searchModel.ListProject = this.ListProject;
    this.TotalProject = this.TotalProject;
    if(this.IsTotal == 1)
    {
      this.searchModel.IsTotal = this.IsTotal;
    }
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

  closeModal() {
    this.activeModal.close();
  }

  search() {
    this.service.getListReportProject(this.searchModel).subscribe(data => {
      this.searchModel.TotalItems = data.TotalItem;
      this.listReport = data.ListResult;
      this.statIndex = ((this.searchModel.PageNumber - 1) * this.searchModel.PageSize + 1);
    }, error => {
      this.messageService.showError(error);
    });
  }
}
