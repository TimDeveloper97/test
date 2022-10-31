import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ComboboxService, Constants, DateUtils, MessageService } from 'src/app/shared';
import { forkJoin } from 'rxjs';
import { SummaryQuotesService } from 'src/app/solution/service/summary-quotes.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-show-summary-quotes-plan-update',
  templateUrl: './show-summary-quotes-plan-update.component.html',
  styleUrls: ['./show-summary-quotes-plan-update.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowSummaryQuotesPlanUpdateComponent implements OnInit {

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
  }

  planStartDate: any;
  planDueDate: any;
  employeeColumnName: any[] = [{ Name: 'Code', Title: 'Mã nhân viên' }, { Name: 'Name', Title: 'Tên nhân viên' }];

  QuotationPlanId: string;
  
  ngOnInit(): void {
    this.getQuotationPlanById(this.QuotationPlanId);
    this.getListEmployee();
  }

  getListEmployee(){
    this.service.getListEmployee().subscribe(data => {
      this.employees = data;
    },
      error => {
        this.messageService.showError(error);
      });
  }

  getQuotationPlanById(QuotationPlanId){
    this.service.getQuotationPlanById(QuotationPlanId).subscribe(
      data => {
        this.ModalInfo.Title = 'Chỉnh sửa công việc' + ' - ' + data.Name;
        this.model = data;
        this.model.Description = data.Descripton;
        if (data.PlanStartDate) {
          let dateArray = data.PlanStartDate.split('T')[0];
          let dateValue = dateArray.split('-');
          let PlanStartDateV = {
            'day': parseInt(dateValue[2]),
            'month': parseInt(dateValue[1]),
            'year': parseInt(dateValue[0])
          };
          this.planStartDate = PlanStartDateV;
        }
        if (data.PlanDueDate) {
          let dateArray = data.PlanDueDate.split('T')[0];
          let dateValue = dateArray.split('-');
          let PlanDueDateV = {
            'day': parseInt(dateValue[2]),
            'month': parseInt(dateValue[1]),
            'year': parseInt(dateValue[0])
          };
          this.planDueDate = PlanDueDateV;
        }
       
      },
      error => {
        this.messageService.showError(error);
      }
    );
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
    this.service.updateQuotationPlan(this.model).subscribe(
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
        };
        this.getListEmployee();
        this.messageService.showSuccess('Cập nhật công việc cho bước báo giá thành công!');
        this.activeModal.close();
      },
      error => {
        this.messageService.showError(error);
      }
    );
  }
}
