import { Component, OnInit, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { AppSetting, MessageService, Configuration, Constants, DateUtils } from 'src/app/shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DashbroadProjectService } from '../service/dashbroad-project.service';
import { ViewDetailComponent } from '../view-detail/view-detail/view-detail.component';

@Component({
  selector: 'app-dashbroad-project',
  templateUrl: './dashbroad-project.component.html',
  styleUrls: ['./dashbroad-project.component.scss']
})
export class DashbroadProjectComponent implements OnInit, AfterViewInit, OnDestroy {

  constructor(
    public appSetting: AppSetting,
    private messageService: MessageService,
    private modalService: NgbModal,
    private config: Configuration,
    private dashbroadProjectService: DashbroadProjectService,
    public dateUtils: DateUtils,
    public constant: Constants
  ) { }
  @ViewChild('scrollPracticeMaterial',{static:false}) scrollPracticeMaterial: ElementRef;
  @ViewChild('scrollPracticeMaterialHeader',{static:false}) scrollPracticeMaterialHeader: ElementRef;

  @ViewChild('scrollPlanDesign',{static:false}) scrollPlanDesign: ElementRef;
  @ViewChild('scrollPlanDesignHeader',{static:false}) scrollPlanDesignHeader: ElementRef;

  @ViewChild('scrollPlanDesignProject',{static:false}) scrollPlanDesignProject: ElementRef;
  @ViewChild('scrollPlanDesignProjectHeader',{static:false}) scrollPlanDesignProjectHeader: ElementRef;

  @ViewChild('scrollPlanDocument',{static:false}) scrollPlanDocument: ElementRef;
  @ViewChild('scrollPlanDocumentHeader',{static:false}) scrollPlanDocumentHeader: ElementRef;

  @ViewChild('scrollPlanDocumentProject',{static:false}) scrollPlanDocumentProject: ElementRef;
  @ViewChild('scrollPlanDocumentProjectHeader',{static:false}) scrollPlanDocumentProjectHeader: ElementRef;

  @ViewChild('scrollPlanDelivery',{static:false}) scrollPlanDelivery: ElementRef;
  @ViewChild('scrollPlanDeliveryHeader',{static:false}) scrollPlanDeliveryHeader: ElementRef;

  @ViewChild('scrollPlanDeliveryProject',{static:false}) scrollPlanDeliveryProject: ElementRef;
  @ViewChild('scrollPlanDeliveryProjectHeader',{static:false}) scrollPlanDeliveryProjectHeader: ElementRef;

  StartIndex = 0;
  model: any = {
    ProjectId: '',
    TotalProject: '', //t???ng s??? d??? ??n ??ang tri???n khai
    TotalProjectFinish: '', // D??? ??n ho??n th??nh
    TotalProjectDelayDeadline: '', //D??? ??n d??? ki???n b??? ch???m
    TotalProjectNotPlan: '', // D??? ??n kh??ng c?? k??? ho???ch
    TotalProjectOnSchedule: '',//D??? ??n ????ng ti???n ?????
    listTaskType: [],
    Total_Task_Delay: '',
    DateFrom: '',
    DateTo: '',
    DateToV: '',
    DateFromV: '',
    ProjCode: '',
    SBUId: '',
    DepartmentId: ''
  }

  searchOptions: any = {
    FieldContentName: 'ProjCode',
    Placeholder: 'T??m ki???m theo m??/t??n d??? ??n',
    Items: [
      {
        Name: 'T??n kh??ch h??ng',
        FieldName: 'CustomerName',
        Placeholder: 'Nh???p t??n kh??ch h??ng',
        Type: 'text'
      },
      {
        Name: 'SBU',
        FieldName: 'SBUId',
        Placeholder: 'SBU',
        Type: 'select',
        DataType: this.constant.SearchDataType.SBU,
        DisplayName: 'Name',
        ValueName: 'Id',
        IsRelation: true,
        Permission: ['F060005'],
        RelationIndexTo: 2
      },
      {
        Name: 'Ph??ng ban',
        FieldName: 'DepartmentId',
        Placeholder: 'Ph??ng ban',
        Type: 'select',
        DataType: this.constant.SearchDataType.Department,
        DisplayName: 'Name',
        ValueName: 'Id',
        Permission: ['F060005'],
        RelationIndexFrom: 1
      },
      {
        Name: 'Lo???i kh??ch h??ng',
        FieldName: 'CustomerTypeId',
        Placeholder: 'Lo???i kh??ch h??ng',
        Type: 'select',
        DataType: this.constant.SearchDataType.CustomerType,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tr???ng th??i d??? ??n',
        FieldName: 'Status',
        Placeholder: 'Tr???ng th??i d??? ??n',
        Type: 'select',
        Data: this.constant.ProjectStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Tr???ng th??i t??i li???u',
        FieldName: 'DocumentStatus',
        Placeholder: 'Tr???ng th??i t??i li???u',
        Type: 'select',
        Data: this.constant.ProjectDocumentStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'Th???i gian',
        FieldNameTo: 'DateToV',
        FieldNameFrom: 'DateFromV',
        Type: 'date'
      },
      {
        Name: 'Lo???i d??? ??n',
        FieldName: 'Type',
        Placeholder: 'Lo???i d??? ??n',
        Type: 'select',
        Data: this.constant.ProjectTypes,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
      {
        Name: 'V???n ????? t???n ?????ng',
        FieldName: 'ErrorStatus',
        Placeholder: 'V???n ????? t???n ?????ng',
        Type: 'select',
        Data: this.constant.ProjectErrorStatus,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  clear() {
    this.model = {};
    this.getListProject();
  }

  ngOnInit() {
    this.appSetting.PageTitle = "Dashboard D??? ??n";
    let currentUser = JSON.parse(localStorage.getItem('qltkcurrentUser'));
    if (currentUser) {
      if (!this.model.SBUId) {
        this.model.SBUId = currentUser.sbuId;
      }
      this.model.DepartmentId = currentUser.departmentId;
    }
    this.getListProject();

  }

  ngAfterViewInit() {    
    this.scrollPracticeMaterial.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollPracticeMaterialHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);

    if (this.scrollPlanDesign && this.scrollPlanDesign.nativeElement) {
      this.scrollPlanDesign.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
    
    if (this.scrollPlanDesignProject && this.scrollPlanDesignProject.nativeElement) {
      this.scrollPlanDesignProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }

    if (this.scrollPlanDocument && this.scrollPlanDocument.nativeElement) {
      this.scrollPlanDocument.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDocumentHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }

    if (this.scrollPlanDocumentProject && this.scrollPlanDocumentProject.nativeElement) {
      this.scrollPlanDocumentProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDocumentProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }

    if (this.scrollPlanDelivery && this.scrollPlanDelivery.nativeElement) {
      this.scrollPlanDelivery.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDeliveryHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }

    if (this.scrollPlanDeliveryProject && this.scrollPlanDeliveryProject.nativeElement) {
      this.scrollPlanDeliveryProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDeliveryProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
  }

  ngOnDestroy() {
    this.scrollPracticeMaterial.nativeElement.removeEventListener('ps-scroll-x', null);
    if (this.scrollPlanDesign && this.scrollPlanDesign.nativeElement) {
      this.scrollPlanDesign.nativeElement.removeEventListener('ps-scroll-x', null);
    }

    if (this.scrollPlanDesignProject && this.scrollPlanDesignProject.nativeElement) {
      this.scrollPlanDesignProject.nativeElement.removeEventListener('ps-scroll-x', null);
    }

    if (this.scrollPlanDocument && this.scrollPlanDocument.nativeElement) {
      this.scrollPlanDocument.nativeElement.removeEventListener('ps-scroll-x', null);
    }

    if (this.scrollPlanDocumentProject && this.scrollPlanDocumentProject.nativeElement) {
      this.scrollPlanDocumentProject.nativeElement.removeEventListener('ps-scroll-x', null);
    }

    if (this.scrollPlanDelivery && this.scrollPlanDelivery.nativeElement) {
      this.scrollPlanDelivery.nativeElement.removeEventListener('ps-scroll-x', null);
    }

    if (this.scrollPlanDeliveryProject && this.scrollPlanDeliveryProject.nativeElement) {
      this.scrollPlanDeliveryProject.nativeElement.removeEventListener('ps-scroll-x', null);
    }
  }

  tabChange($event) {
    if ($event.nextId === 'tab-design') {
      this.scrollPlanDesign.nativeElement.removeEventListener('ps-scroll-x', null);
      this.scrollPlanDesignProject.nativeElement.removeEventListener('ps-scroll-x', null);

      this.scrollPlanDesign.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);

      this.scrollPlanDesignProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDesignProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);

    }
    else if ($event.nextId === 'tab-document') {

      this.scrollPlanDocument.nativeElement.removeEventListener('ps-scroll-x', null);
      this.scrollPlanDocumentProject.nativeElement.removeEventListener('ps-scroll-x', null);

      this.scrollPlanDocument.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDocumentHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);

      this.scrollPlanDocumentProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDocumentProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);

    }
    else if ($event.nextId === 'tab-delivery') {
      this.scrollPlanDelivery.nativeElement.removeEventListener('ps-scroll-x', null);
      this.scrollPlanDeliveryProject.nativeElement.removeEventListener('ps-scroll-x', null);

      this.scrollPlanDelivery.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDeliveryHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);

      this.scrollPlanDeliveryProject.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
        this.scrollPlanDeliveryProjectHeader.nativeElement.scrollLeft = event.target.scrollLeft;
      }, true);
    }
  }
  height = 200;

  listProjectInPlan: any[];
  // listProjectDesign: any[];
  // listProjectDoc: any[];
  // listProjectTransfer: any[];
  // listReportDesignProject: any[];
  // listReportDocProject: any[];
  // listReportTransferProject: any[];
  //listYear: any[] = [];
  projects: any[];
  getListProject() {
    if(this.model.SBUId == null){
      this.model.SBUId ='';
    }
    if (this.model.DateFromV) {
      this.model.DateFrom = this.dateUtils.convertObjectToDate(this.model.DateFromV);
    }else{
      this.model.DateFrom ='';
    }
    if (this.model.DateToV) {
      this.model.DateTo = this.dateUtils.convertObjectToDate(this.model.DateToV);
    }else{
      this.model.DateTo ='';
    }
    this.dashbroadProjectService.getListProject(this.model).subscribe((data: any) => {
      this.listProjectInPlan = data.ListProjectInPlan;
      // this.listProjectDesign = data.ListProjectDesign;
      // this.listProjectDoc = data.ListProjectDoc;
      // this.listProjectTransfer = data.ListProjectTransfer;
      // this.listReportDesignProject = data.ListReportDesignProject;
      // this.listReportDocProject = data.ListReportDocProject;
      // this.listReportTransferProject = data.ListReportTransferProject;
      this.model.TotalProject = data.TotalProject;
      this.model.TotalProjectFinish = data.TotalProjectFinish;
      this.model.TotalProjectDelayDeadline = data.TotalProjectDelayDeadline;
      this.model.TotalProjectNotPlan = data.TotalProjectNotPlan;
      this.model.Total_Task_Delay = data.Total_Task_Delay;
      this.model.TotalProjectOnSchedule = data.TotalProjectOnSchedule;

      this.projects = data.Projects;
    },
      error => {
        this.messageService.showError(error);
      });
  }
  list_Design: any[] = [];

  viewDetail(planType, projectId, valueType) {
    let activeModal = this.modalService.open(ViewDetailComponent, { container: 'body', windowClass: 'view-detail-model', backdrop: 'static' })
    activeModal.componentInstance.planType = planType;
    activeModal.componentInstance.projectId = projectId;
    activeModal.componentInstance.valueType = valueType;
    activeModal.result.then((result) => {
      if (result) {
        this.getListProject();
      }
    }, (reason) => {
    });

  }

}
