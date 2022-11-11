import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Console } from 'console';
import { AppSetting, Configuration, Constants, MessageService } from 'src/app/shared';
import { ProjectServiceService } from '../service/project-service.service';
import { ShowExportExcelProjectPlanComponent } from './show-export-excel-project-plan/show-export-excel-project-plan.component';
import { ShowListMoneyCollectionComponent } from './show-list-money-collection/show-list-money-collection.component';

@Component({
  selector: 'app-money-collection-report',
  templateUrl: './money-collection-report.component.html',
  styleUrls: ['./money-collection-report.component.scss']
})
export class MoneyCollectionReportComponent implements OnInit {
  

  constructor(
    public appSetting: AppSetting,
    public constant: Constants,
    private messageService: MessageService,
    public service: ProjectServiceService,
    private modalService: NgbModal,
    private config: Configuration,
  ) { }

  
  @ViewChild('scrollDepartment', { static: false }) scrollDepartment: ElementRef;
  @ViewChild('scrollDepartmentHeader', { static: false }) scrollDepartmentHeader: ElementRef;

  date = new Date();
  model: any = {
    ProjectId: '',
    CustomerId: '',
    Year: this.date.getFullYear(),
    StartYear: this.date.getFullYear(),
    EndYear: this.date.getFullYear()
  }

  searchOptions: any = {
    FieldContentName: 'Year',
    Placeholder: 'Tìm kiếm theo năm',
    Items: [
      {
        Placeholder: 'Chọn dự án',
        Name: 'Mã dự án',
        FieldName: 'ProjectId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Project,
        Columns: [{ Name: 'Code', Title: 'Mã dự án' }, { Name: 'Name', Title: 'Tên dự án' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Placeholder: 'Chọn khách hàng',
        Name: 'Khách hàng',
        FieldName: 'CustomerId',
        Type: 'dropdown',
        SelectMode: 'single',
        DataType: this.constant.SearchDataType.Customer,
        Columns: [{ Name: 'Code', Title: 'Mã khách hàng' }, { Name: 'Name', Title: 'Tên khách hàng' }],
        DisplayName: 'Code',
        ValueName: 'Id',
      },
      {
        Name: 'Loại thanh toán',
        FieldName: 'Name',
        Placeholder: 'Loại thanh toán',
        Type: 'select',
        Data: this.constant.SelectPaymentName,
        DisplayName: 'Name',
        ValueName: 'Id'
      },
    ]
  };
  
  empMinWidth = 250;

  errorChangePlans: any[] = [];
  months: any[] = [];
  isExist: boolean;
  departments: any[] = [];
  totalBadDebtInYear = 0;
  totalBadDebt = 0;
  IsTotal = 1;

  ngOnInit(): void {
    this.appSetting.PageTitle = "Báo cáo thu tiền dự án";
    this.report();
    this.getTotalBadDebt(this.model.Year);
  }

  getTotalBadDebt(year){
    this.service.getTotalBadDebt(year).subscribe(
      data => {
        this.totalBadDebtInYear = data.totalBadDebtInYear;
        this.totalBadDebt = data.totalBadDebt;
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  ngAfterViewInit() {
    this.scrollDepartment.nativeElement.addEventListener('ps-scroll-x', (event: any) => {
      this.scrollDepartmentHeader.nativeElement.scrollLeft = event.target.scrollLeft;
    }, true);
  }

  ngOnDestroy() {
    this.scrollDepartment.nativeElement.removeEventListener('ps-scroll-x', null);
    
  }

  report(){
    this.model.StartYear = this.model.Year;
    this.model.EndYear = this.model.Year;
    this.service.Report(this.model).subscribe(
      data => {
        this.departments = data.Departments;
        this.months = data.Months;
        // data.Months.forEach(element => {
        //   element.forEach(item => {
        //     item.Result.forEach(t => {
        //       t.
        //     })
        //     item.AllProjects = item.Result.TotalProject
        //   });
        // });
        this.getTotalBadDebt(this.model.Year);
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }

  showProject(data) {
    let activeModal = this.modalService.open(ShowListMoneyCollectionComponent, { container: 'body', windowClass: 'show-list-money-collection-modal', backdrop: 'static' })
    activeModal.componentInstance.ProjectId = this.model.ProjectId;
    activeModal.componentInstance.CustomerId = this.model.CustomerId;
    activeModal.componentInstance.DepartmentId = data.DepartmentId;
    activeModal.componentInstance.Month = data.Month;
    activeModal.componentInstance.IsExist = data.IsExist;
    activeModal.componentInstance.Year = this.model.Year;
    activeModal.componentInstance.ListProject = data.ListProject;
    activeModal.componentInstance.TotalProject = data.TotalProject;
    activeModal.result.then((result) => {
      if (result) {

      }
    }, (reason) => {
    });
  }
 

  showAllProject(data) {
    let activeModal = this.modalService.open(ShowListMoneyCollectionComponent, { container: 'body', windowClass: 'show-list-money-collection-modal', backdrop: 'static' })
    activeModal.componentInstance.ProjectId = this.model.ProjectId;
    activeModal.componentInstance.CustomerId = this.model.CustomerId;
    activeModal.componentInstance.Month = data.Month;
    activeModal.componentInstance.IsExist = data.IsExist;
    activeModal.componentInstance.Year = this.model.Year;
    activeModal.componentInstance.TotalListProject = data.ListProject;
    activeModal.componentInstance.TotalProject = data.TotalProject;
    activeModal.componentInstance.IsTotal = this.IsTotal;
    activeModal.result.then((result) => {
      if (result) {

      }
    }, (reason) => {
    });
  }


  exportExcel() {
    let activeModal = this.modalService.open(ShowExportExcelProjectPlanComponent, { container: 'body', windowClass: 'show-export-excel-project-plan-modal', backdrop: 'static' })
    
    activeModal.result.then((result) => {
      if (result) {

      }
    }, (reason) => {
    });
  }

  
}
