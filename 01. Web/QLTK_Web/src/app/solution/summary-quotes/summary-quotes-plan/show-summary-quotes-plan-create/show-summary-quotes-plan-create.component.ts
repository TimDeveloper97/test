import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, DateUtils, MessageService } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { SummaryQuotesService } from 'src/app/solution/service/summary-quotes.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-show-summary-quotes-plan-create',
  templateUrl: './show-summary-quotes-plan-create.component.html',
  styleUrls: ['./show-summary-quotes-plan-create.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowSummaryQuotesPlanCreateComponent implements OnInit {

  constructor(
    private activeModal: NgbActiveModal,
    private comboboxService: ComboboxService,
    private router: Router,
    private messageService: MessageService,
    public constants: Constants,
    public dateUtils: DateUtils,
    private service: SummaryQuotesService,
  ){ }
  
  ModalInfo = {
    Title: 'Thêm công việc',
    SaveText: 'Lưu',
  };

  employees: any[] = [];
  model: any = {
    Name:'',
    EmployeeId:'',
    PlanStartDate:null,
    PlanDueDate:null,
    QuotationId:'',
    StepInQuotationId: '',
    Description:'',
    EstimateTime:'',
    DoneRatio:''
  }

  planStartDate: any;
  planDueDate: any;
  employeeColumnName: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];

  QuotationId: string;
  StepInQuotationId: string;
  
  ngOnInit(): void {
    this.model.QuotationId = this.QuotationId;
    this.model.StepInQuotationId = this.StepInQuotationId;
    this.getListEmployee();
    this.getEmployeeCharge(this.QuotationId);
  }

  getEmployeeCharge(QuotationId){
    this.service.getEmployeeCharge(QuotationId).subscribe(data => {
      this.model.EmployeeId = data.Id;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getListEmployee(){
    this.service.getListEmployee().subscribe(data => {
      this.employees = data;
    },
      error => {
        this.messageService.showError(error);
      });
  }
  closeModal() {
    this.activeModal.close();
  }

  save(){
    if (this.planStartDate) {
      this.model.PlanStartDate = this.dateUtils.convertObjectToDate(this.planStartDate);
    }
    if (this.planDueDate) {
      this.model.PlanDueDate = this.dateUtils.convertObjectToDate(this.planDueDate);
    }
    this.service.createQuotationPlan(this.model).subscribe(
      data => {
        this.model = {
          Name:'',
          EmployeeId:'',
          PlanStartDate:null,
          PlanDueDate:null,
          QuotationId:'',
          StepInQuotationId: '',
          Description:'',
          EstimateTime:'',
          DoneRatio:''
        };
        //this.getCbbData();
        this.getListEmployee();
        this.messageService.showSuccess('Thêm mới công việc cho bước báo giá thành công!');
        this.activeModal.close();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
